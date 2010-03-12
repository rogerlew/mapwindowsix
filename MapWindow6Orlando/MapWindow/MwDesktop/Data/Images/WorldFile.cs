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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/14/2008 10:48:06 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Data
{


    /// <summary>
    /// WorldFiles complement images, giving georeference information for those images.  The basic idea is to calculate
    /// everything based on the top left corner of the image.  
    /// </summary>
    public class WorldFile
    {
        #region Private Variables

      
        private string _filename;
        private double[] _affine;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of WorldFile
        /// </summary>
        public WorldFile()
        {

        }

        /// <summary>
        /// Automatically creates a new worldfile based on the specified image filename.
        /// </summary>
        /// <param name="imageFilename">Attempts to open the filename for the world file for the image if it exists.</param>
        public WorldFile(string imageFilename)
        {
            _filename = GenerateFilename(imageFilename);
            if (System.IO.File.Exists(imageFilename))
            {
                Open();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the string extensions that accompanies one of the dot net image formats.
        /// </summary>
        /// <param name="format">The System.Drawing.Imaging.ImageFormat for the image itself</param>
        /// <returns>The string extension</returns>
        public string GetExtension(ImageFormat format)
        {
            
           if(format == ImageFormat.Bmp) return ".bpw";
           if(format == ImageFormat.Emf) return ".efw";
           if(format == ImageFormat.Exif) return ".exw";
           if(format == ImageFormat.Gif) return ".gfw";
           if(format == ImageFormat.Icon) return ".iow"; 
           if(format == ImageFormat.Jpeg) return ".jgw";
           if(format == ImageFormat.MemoryBmp) return ".mpw";
           if(format == ImageFormat.Png) return ".pgw";
           if(format == ImageFormat.Tiff) return ".tfw"; 
           if(format == ImageFormat.Wmf) return ".wfw";
           return ".wld";
        }

        /// <summary>
        /// Given the filename of an image, this creates a new filename with the appropriate extension.
        /// This will also set the filename of this world file to that extension.
        /// </summary>
        /// <param name="imageFilename">The filename of the image</param>
        /// <returns>the filename of the world file</returns>
        public string GenerateFilename(string imageFilename)
        {
            string ext = System.IO.Path.GetExtension(imageFilename);
            string result = ".wld";
            switch (ext)
            {
                case ".bmp": result = ".bpw"; break;
                case ".emf": result = ".efw"; break;
                case ".exf": result = ".exw"; break;
                case ".gif": result = ".gfw"; break;
                case ".ico": result = ".iow"; break;
                case ".jpg": result = ".jgw"; break;
                case ".mbp": result = ".mww"; break;
                case ".png": result = ".pgw"; break;
                case ".tif": result = ".tfw"; break;
                case ".wmf": result = ".wft"; break;
            }
            return System.IO.Path.ChangeExtension(imageFilename, result);
        }

        /// <summary>
        /// Opens the worldfile specified by the Filename property and loads the values
        /// </summary>
        public void Open()
        {
            if (System.IO.File.Exists(_filename))
            {
                StreamReader sr = new StreamReader(_filename);
                _affine = new double[6];
                _affine[1] = double.Parse(sr.ReadLine()); // Dx
                _affine[2] = double.Parse(sr.ReadLine()); // Skew X
                _affine[4] = double.Parse(sr.ReadLine()); // Skew Y
                _affine[5] = double.Parse(sr.ReadLine()); // Dy
                _affine[0] = double.Parse(sr.ReadLine()); // Top Left X
                _affine[3] = double.Parse(sr.ReadLine()); // Top Left Y
                sr.Close();
            }
            
           
        }

        /// <summary>
        /// Opens an existing worldfile based on the specified filename
        /// </summary>
        /// <param name="filename"></param>
        public void Open(string filename)
        {
            _filename = filename;
            Open();
        }

        /// <summary>
        /// Saves the current affine coordinates to the current filename
        /// </summary>
        public void Save()
        {
            if (System.IO.File.Exists(_filename)) System.IO.File.Delete(_filename);
            StreamWriter sw = new StreamWriter(_filename);
            sw.WriteLine(_affine[1].ToString());  // Dx
            sw.WriteLine(_affine[2].ToString());  // Skew X
            sw.WriteLine(_affine[4].ToString());  // Skew Y
            sw.WriteLine(_affine[5].ToString());  // Dy 
            sw.WriteLine(_affine[0].ToString());  // Top Left X
            sw.WriteLine(_affine[3].ToString());  // Top Left Y
            sw.Close();
        }

        /// <summary>
        /// Saves the current coordinates to a file
        /// </summary>
        /// <param name="filename">Gets or sets the filename to use for an image</param>
        public void SaveAs(string filename)
        {
            _filename = filename;
            Save();
        }

        /// <summary>
        /// Creates a System.Drawing.Matrix that is in float coordinates that represents this world file
        /// </summary>
        /// <returns>A System.Drawing.Drawing2D.Matrix that transforms an image to the geographic coordinates</returns>
        public Matrix ToMatrix()
        {
            float m11 = Convert.ToSingle(_affine[1]);
            float m12 = Convert.ToSingle(_affine[2]);
            float m21 = Convert.ToSingle(_affine[4]);
            float m22 = Convert.ToSingle(_affine[5]);
            float dx = Convert.ToSingle(_affine[0]);
            float dy = Convert.ToSingle(_affine[3]);
            return new Matrix(m11, m12, m21, m22, dx, dy);
        }

        #endregion

        #region Properties

        /// <summary>
        /// gets the coordinates in the Affine order:
        /// X' = [0] + [1] X + [2] Y
        /// Y' = [3] + [4] X + [5] Y
        /// </summary>
        public double[] Affine
        {
            get { return _affine; }
            set { _affine = value; }
        }

        /// <summary>
        /// Gets or sets the filename to use for this world file.
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// Gets or sets the cellHeight
        /// </summary>
        public double CellHeight
        {
            get { return _affine[5]; }
            set { _affine[5] = value; }
        }

        /// <summary>
        /// Gets or sets the cellWidth
        /// </summary>
        public double CellWidth
        {
            get { return _affine[1]; }
            set { _affine[1] = value; }
        }

        
        /// <summary>
        /// Gets or sets longitude or X position corresponding to the center of the cell in the top left corner of the image
        /// </summary>
        public double TopLeftX
        {
            get { return _affine[0]; }
            set { _affine[0] = value; }
        }

        /// <summary>
        /// Gets or sets the latitude or Y position corresponding to the center of the cell in the top left corner of the image
        /// </summary>
        public double TopLeftY
        {
            get { return _affine[3]; }
            set { _affine[3] = value; }
        }

        /// <summary>
        /// Gets or sets how much the longitude or X position of a cell in the image depends on the row position of the cell.
        /// </summary>
        public double HorizontalSkew
        {
            get { return _affine[2]; }
            set { _affine[2] = value; }
        }

        /// <summary>
        /// Gets or sets how much the latitude or Y position of a cell in the image depends on the column position of the cell.
        /// </summary>
        public double VerticalSkew
        {
            get { return _affine[4]; }
            set { _affine[4] = value; }
        }

        /// <summary>
        /// Gets a string list of file extensions that might apply to world files.  
        /// </summary>
        public string DialogFilter
        {
            get { return "generic (*.wld)|*.wld|Bitmap (*.bpw)|*.bpw|EMF (*.efw)|*efw|Exif (*.exw)|GIF (*.gif)|*.gif|Icon (*.iow)|*.iow|JPEG (*.jgw)|*.jgw|Memory Bitmap (*.mpw)|*.mpw|PNG (*.pgw)|*.pgw|Tif (*.tfw)|*.tfw|WMF (*.wfw)|*.wfw"; }
        }
       

        #endregion



    }
}
