//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/14/2008 8:46:23 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using MapWindow.Main;
using MapWindow.Plugins;
namespace MapWindow.Data
{


    /// <summary>
    /// DotNetImageProvider uses the standard image object to support basic image types through standard in-ram treatments.
    /// Images are not responsible for producing grid values that can be represented symbolically.
    /// </summary>
    public class DotNetImageProvider : IImageDataProvider
    {
        #region Private Variables

        private IProgressHandler _prog;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DotNetImageProvider
        /// </summary>
        public DotNetImageProvider()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of an Image.
        /// </summary>
        /// <param name="filename">The string filename to use</param>
        /// <param name="width">The integer width in pixels</param>
        /// <param name="height">The integer height in pixels</param>
        /// <param name="inRam">Boolean, true if the entire contents should be stored in memory</param>
        /// <param name="progHandler">A Progress handler to use</param>
        /// <returns>A New IImageData object allowing access to the content of the image</returns>
        public IImageData Create(string filename, int width, int height, bool inRam, IProgressHandler progHandler)
        {
            MWImageData img = new MWImageData();
            img.CreateNew(filename, width, height);
            return img;
        }

        /// <summary>
        /// Opens a new Image with the specified filename
        /// </summary>
        /// <param name="filename">The string file to open</param>
        /// <returns>An IImageData object</returns>
        public IImageData Open(string filename)
        {
            MWImageData img = new MWImageData();
            img.Open(filename);
            return img;
        }
        IDataSet IDataProvider.Open(string filename)
        {
            MWImageData img = new MWImageData();
            img.Open(filename);
            return img;
        }

      


        #endregion

        #region Properties

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimeted
        /// by the | symbol.  Each will appear in MapWindow's open file dialog filter, preceeded by the name provided
        /// on this object.
        /// </summary>
        public string DialogReadFilter
        {

            get { 
                return "Images|*.bmp;*.emf;*.exf;*.gif;*.ico;*.jpg;*.mbp;*.png;*.tif;*.wmf" + 
                         "|Bitmap | *.bmp";
            }
        }

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in MapWindow's open file dialog filter, preceeded by the name provided on this object.
        /// </summary>
        public string DialogWriteFilter
        {
            get 
            {
                return "Images|*.bmp;*.emf;*.exf;*.gif;*.ico;*.jpg;*.mbp;*.png;*.tif;*.wmf" +
                    "|Bitmap | *.bmp";
            }
        }

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider.  Example might be GDAL. 
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        public string Name
        {
            get { return "DotNet Image"; }
        }

        /// <summary>
        /// This provides a basic description of what your provider does.
        /// </summary>
        public string Description
        {
            get { return "A Dot Net Image object driven image handling system."; }
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
