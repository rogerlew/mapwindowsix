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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/14/2008 8:52:08 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using MapWindow.Components;
using MapWindow.Geometries;
using MapWindow.Serialization;
using Point=System.Drawing.Point;

namespace MapWindow.Data
{


    /// <summary>
    /// ImageData (not named Image because of conflicting with the Dot Net Image object)
    /// </summary>
    public class ImageData : DataSet, IImageData
    {
        #region Private Variables

        private WorldFile _worldFile;

        [Serialize("FileName", ConstructorArgumentIndex = 0)]
        private string _filename;
        private RasterBounds _bounds;
        private int _numBands;
        private int _stride;
        private byte[] _values;
        private int _bytesPerPixel;
        private int _width;
        private int _height;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ImageData
        /// </summary>
        public ImageData()
        {
            TypeName = "Image";
            _bounds = new RasterBounds(0, 0, new double[] { 0, 1, 0, 0, 0, -1 });
            _worldFile = new WorldFile();
        }

        /// <summary>
        /// Opens the specified image file
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        public ImageData(string filename)
        {
            Open(filename);
        }

        #endregion

        #region Methods

      

        /// <summary>
        /// Copies the values from the specified source image.
        /// </summary>
        /// <param name="source">The source image to copy values from.</param>
        public virtual void CopyValues(IImageData source)
        {
            if (InternalDataSet != null) InternalDataSet.CopyValues(source);
        }

       

        /// <summary>
        /// Creates a new image and world file, placing the default bounds at the origin, with one pixel per unit.
        /// </summary>
        /// <param name="filename">The string filename</param>
        /// <param name="width">The integer width</param>
        /// <param name="height">The integer height</param>
        public virtual void CreateNew(string filename, int width, int height)
        {
            if (InternalDataSet != null) InternalDataSet.Dispose();
            InternalDataSet = DataManager.DefaultDataManager.CreateImage(filename, height, width);
        }

        /// <summary>
        /// Disposes any unmanaged memory constructs.
        /// </summary>
        public virtual void Dispose()
        {
            if(InternalDataSet != null) InternalDataSet.Dispose();
        }

        /// <summary>
        /// Attempts to create a bitmap for the entire image.  This may cause memory exceptions.
        /// </summary>
        /// <returns>A Bitmap of the image.</returns>
        public virtual Bitmap GetBitmap()
        {
            if (InternalDataSet != null) return InternalDataSet.GetBitmap();
            return null;
        }

        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">The geographic extents to retrieve data for</param>
        /// <param name="size">The rectangle that defines the size of the drawing area in pixels</param>
        /// <returns>A bitmap captured from the main image </returns>
        public Bitmap GetBitmap(IEnvelope envelope, Size size)
        {
            return GetBitmap(envelope, new Rectangle(new Point(0, 0), size));
        }

        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">The geographic extents to retrieve data for</param>
        /// <param name="window">The rectangle that defines the size of the drawing area in pixels</param>
        /// <returns>A bitmap captured from the main image </returns>
        public virtual Bitmap GetBitmap(IEnvelope envelope, Rectangle window)
        {
            if (InternalDataSet != null) return InternalDataSet.GetBitmap(envelope, window);
            return null;
        }

        /// <summary>
        /// Opens the file, assuming that the filename has already been specified
        /// </summary>
        public virtual void Open()
        {
            string filename = Filename; // in case we have an internal image, record this now.
            if (InternalDataSet != null)
            {
                InternalDataSet.Dispose();
            }
            InternalDataSet = DataManager.DefaultDataManager.OpenImage(filename);
        }

        /// <summary>
        /// Opens the file with the specified filename
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        public void Open(string filename)
        {
            Filename = filename;
            Open();
        }

        /// <summary>
        /// Forces the image to read values from the graphic image format to the byte array format
        /// </summary>
        public virtual void ReadBytes()
        {
            if (InternalDataSet != null) InternalDataSet.ReadBytes();
        }

        /// <summary>
        /// Saves the image and associated world file to the current filename.
        /// </summary>
        public virtual void Save()
        {
            if (InternalDataSet != null) InternalDataSet.Save();
        }

        /// <summary>
        /// Saves the image to a new filename.
        /// </summary>
        /// <param name="filename">The string filename to save the image to.</param>
        public virtual void SaveAs(string filename)
        {
            DataManager.DefaultDataManager.CreateImage(filename, Height, Width);
        }

        /// <summary>
        /// Forces the image to copy values from the byte array format to the image format.
        /// </summary>
        public virtual void WriteBytes()
        {
            if (InternalDataSet != null) InternalDataSet.WriteBytes();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an integer indicating how many bytes exist for each pixel.
        /// Eg. 32 ARGB = 4, 24 RGB = 3, 16 bit GrayScale = 2
        /// </summary>
        public int BytesPerPixel
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.BytesPerPixel;
                return  _bytesPerPixel;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.BytesPerPixel = value;
                }
                _bytesPerPixel = value;
            }
        }

       

        /// <summary>
        /// Gets or sets the image bounds being used to define the georeferencing of the image
        /// </summary>
        public RasterBounds Bounds
        {
            get 
            {
                if (InternalDataSet != null)return InternalDataSet.Bounds;
                return _bounds; 
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Bounds = value;
                    return;
                }
                _bounds = value;
                _worldFile.Affine = _bounds.AffineCoefficients;
               
            }
        }

        /// <summary>
        /// Gets or sets the Bounds.Envelope
        /// </summary>
        public IEnvelope Envelope
        {
            get { return Bounds.Envelope; }
            set { Bounds.Envelope = value; }
         
        }
        
        /// <summary>
        /// Gets or sets the filename.  
        /// </summary>
        public string Filename
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.Filename;
                return _filename;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Filename = value;
                    return;
                }
                _filename = value;
            }
        }

        /// <summary>
        /// Creates a color structure from the byte values in the values array that correspond to the
        /// specified position.
        /// </summary>
        /// <param name="row">The integer row index for the pixel.</param>
        /// <param name="column">The integer column index for the pixel.</param>
        /// <returns>A System.Drawing.Color.</returns>
        public virtual Color GetColor(int row, int column)
        {
            if (InternalDataSet != null) return InternalDataSet.GetColor(row, column);
            int bpp = BytesPerPixel;
            int strd = Stride;
            int b = Values[row * strd + column * bpp];
            int g = Values[row * strd + column * bpp + 1];
            int r = Values[row * strd + column * bpp + 2];
            int a = 255;
            if (BytesPerPixel == 4)
            {
                a = Values[row * strd + column * bpp + 3];
            }
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Sets the color value into the byte array based on the row and column position of the pixel.
        /// </summary>
        /// <param name="row">The integer row index of the pixel to set the color of.</param>
        /// <param name="column">The integer column index of the pixel to set the color of </param>
        /// <param name="col">The color to copy values from</param>
        public virtual void SetColor(int row, int column, Color col)
        {
            if (InternalDataSet != null)
            {
                InternalDataSet.SetColor(row, column, col);
                return;
            }
            int bpp = BytesPerPixel;
            int strd = Stride;
            Values[row * strd + column * bpp] = col.B;
            Values[row * strd + column * bpp + 1] = col.G;
            Values[row * strd + column * bpp + 2] = col.R;
            if (BytesPerPixel == 4)
            {
                Values[row * strd + column * bpp + 3] = col.A;
            }
        }


        /// <summary>
        /// Gets the image height in pixels
        /// </summary>
        public virtual int Height
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.Height;
                return _height;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Height = value;
                    return;
                }
                _height = value;
            }

        }

        /// <summary>
        /// Gets or sets the number of bands that are in the image.  One band is a gray valued image, 3 bands for color RGB and 4 bands
        /// for ARGB.
        /// </summary>
        public int NumBands
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.NumBands;
                return _numBands;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.NumBands = value;
                    return;
                }
                _numBands = value;
            }
        }

        /// <summary>
        /// Gets or sets the stride in bytes.
        /// </summary>
        public int Stride
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.Stride;
                return _stride;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Stride = value;
                    return;
                }
                _stride = value;
            }
        }


        /// <summary>
        /// Gets a one dimensional array of byte values
        /// </summary>
        public virtual byte[] Values
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.Values;
                return _values;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Values = value;
                    return;
                }
                _values = value;

            }
        }

        /// <summary>
        /// Gets the image width in pixels
        /// </summary>
        public virtual int Width
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.Width;
                return _width;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Width = value;
                    return;
                }
                _width = value;
            }
            
        }

        /// <summary>
        /// Gets or sets the world file that stores the georeferencing information for this image.
        /// </summary>
        public WorldFile WorldFile
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.WorldFile;
                return _worldFile;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.WorldFile = value;
                    return;
                }
                _worldFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the internal dataset as an InternalDataset
        /// </summary>
        protected new IImageData InternalDataSet
        {
            get { return base.InternalDataSet as IImageData; }
            set { base.InternalDataSet = value; }
        }
        

        #endregion




        
    }
}
