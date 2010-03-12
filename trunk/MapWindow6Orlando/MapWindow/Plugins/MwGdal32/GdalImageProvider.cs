//********************************************************************************************************
// Product Name: GDALPlugin.dll Alpha
// Description:  A Data provider module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from GDALPlugin.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/10/2008 11:32:21 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Diagnostics;
using System.IO;
using MapWindow.Main;
using MapWindow.Plugins;
using MapWindow.Data;
using System.Windows.Forms;
namespace MapWindow.Gdal32
{
    /// <summary>
    /// GDalImageProvider acts as the factory to create IImageData files that use the GDAL libraries
    /// </summary>
    public class GdalImageProvider : IImageDataProvider
    {

        private IProgressHandler _prog;

        #region IImageDataProvider Members

        /// <summary>
        /// Creates a new image given the specified file format
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="inRam"></param>
        /// <param name="progHandler"></param>
        /// <returns></returns>
        public IImageData Create(string filename, int width, int height, bool inRam, MapWindow.Main.IProgressHandler progHandler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens an existing file using the specified parameters
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IImageData Open(string filename)
        {
            return OpenFile(filename);
        }
        IDataSet IDataProvider.Open(string filename)
        {
            return OpenFile(filename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private IImageData OpenFile(string filename)
        {
            
            
            GdalImage result = new GdalImage(filename);
            if(result.Width > 8000 || result.Height > 8000)
            {

                // Firstly, if there are pyramids inside of the GDAL file itself, we can just work with this directly,
                // without creating our own pyramid image.




                // For now, we can't get fast, low-res versions without some kind of pyramiding happening.
                // since that can take a while for huge images, I'd rather do this once, and create a kind of
                // standardized file-based pyramid system.  Maybe in future pyramid tiffs could be used instead?
                string pyrFile = Path.ChangeExtension(filename, ".mwi");
                if(File.Exists(pyrFile))
                {
                    if(File.Exists(Path.ChangeExtension(pyrFile, ".mwh")))
                    {
                        return new PyramidImage(filename); 
                    }
                    File.Delete(pyrFile);
                }
                
                GdalImageSource gs = new GdalImageSource(filename);
                PyramidImage py = new PyramidImage(pyrFile, gs.Bounds);
                int width = gs.Bounds.NumColumns;
                int blockHeight = 64000000 / width;
                if (blockHeight > gs.Bounds.NumRows) blockHeight = gs.Bounds.NumRows;
                int numBlocks = (int)Math.Ceiling(gs.Bounds.NumRows/(double)blockHeight);
                ProgressMeter pm = new ProgressMeter(ProgressHandler, "Copying Data To Pyramids", numBlocks* 2);
                ProgressHandler.Progress("pyramid", 0, "Copying Data To Pyramids: 0% Complete");
                Application.DoEvents();
                for(int j = 0; j < numBlocks; j++)
                {
                    int h = blockHeight;
                    if(j==numBlocks-1)
                    {
                        h = gs.Bounds.NumRows - j*blockHeight;
                    }
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    byte[] vals = gs.ReadWindow(j*blockHeight, 0, h, width, 0);
                    Debug.WriteLine("Reading Value time: " + sw.ElapsedMilliseconds);
                    pm.CurrentValue = j * 2 + 1;
                    sw.Reset();
                    sw.Start();
                    py.WriteWindow(vals, j * blockHeight, 0, h, width, 0);
                    sw.Stop();
                    Debug.WriteLine("Writing Pyramid time: " + sw.ElapsedMilliseconds);
                    pm.CurrentValue = (j+1) * 2;
                }                
                gs.Dispose();
                pm.Reset();
                py.CreatePyramids(ProgressHandler);
                py.WriteHeader(pyrFile);
                return py;
            }
            result.Open(filename);
            return result;
        }

       


        #endregion

        #region IDataProvider Members

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description
        {
            get { return "Images supported by GDAL"; }
        }

        /// <summary>
        /// Gets or sets the dialog read filter
        /// </summary>
        public string DialogReadFilter
        {
            get { return "Images|*.gen;*.thf;*.blx;*.xlb;*.kap;*.bt;*.doq;*.dt0;*.dt2;*.ers;*.n1;*.fits;*.hdr;*.grb;*.img;*.mpr;*.mpl;*.j2k;*.tif;*.sid;*.ecw;*.jp2;*.png;*.ppm;*.pgm;*.rik;*.rsw,*.mtw*.ddf;*.ter;*.dem"; }
        }

        /// <summary>
        /// Gets or sets the dialog write filter
        /// </summary>
        public string DialogWriteFilter
        {
            get { return "Images|*.tif;*.sid;*.ecw;*.jp2"; }
        }

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        public string Name
        {
            get { return "GDAL"; }
        }

        /// <summary>
        /// Gets or sets the progress handler
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _prog; }
            set { _prog = value; }
        }
       

        #endregion
    }
}
