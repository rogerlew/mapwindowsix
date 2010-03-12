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
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/9/2008 2:43:38 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using MapWindow.Geometries;

namespace MapWindow.Data
{


    /// <summary>
    /// MWImageData
    /// </summary>
    public class MWImageData : ImageData
    {

        private Bitmap _myImage;

        /// <summary>
        /// Creates an empty imagedata to be created or loaded
        /// </summary>
        public MWImageData()
        {
            
        }

        /// <summary>
        /// Creates a new MWImageData class from the specified filename.
        /// </summary>
        /// <param name="filename"></param>
        public MWImageData(string filename)
        {
            Filename = filename;
            Open(filename);
        }

        /// <summary>
        /// Creates the bitmap from the raw image specified.  The bounds should be set on this later.
        /// </summary>
        /// <param name="rawImage">The raw image.</param>
        public MWImageData(Image rawImage)
        {
            _myImage = new Bitmap(rawImage.Width, rawImage.Height);
            Graphics g = Graphics.FromImage(_myImage);
            g.DrawImageUnscaled(rawImage, 0, 0);
            g.Dispose();
            WorldFile = new WorldFile();
            double[] aff = new[] { 1.0, 0, 0, -1.0, 0, _myImage.Height };
            Bounds = new RasterBounds(_myImage.Height, _myImage.Width, aff);
            MemorySetup();
        }

        /// <summary>
        /// Uses a bitmap and a geographic envelope in order to define a new imageData object.
        /// </summary>
        /// <param name="rawImage">The raw image</param>
        /// <param name="bounds">The envelope bounds</param>
        public MWImageData(Bitmap rawImage, IEnvelope bounds)
        {
            _myImage = rawImage;
            Bounds = new RasterBounds(_myImage.Height, _myImage.Width, bounds);
            MemorySetup();
        }

        /// <summary>
        /// Constructs a new imagedata of the specified width and height.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MWImageData(int width, int height)
        {
            WorldFile = new WorldFile();
            double[] aff = new[] { 1.0, 0, 0, -1.0, 0, height };
            Bounds = new RasterBounds(height, width, aff);
            _myImage = new Bitmap(width, height);
            MemorySetup();
        }

        // Makes sure that the content of the image is reflected in bytes.
        private void MemorySetup()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            _myImage.Save(ms, ImageFormat.Bmp);
            ms.Position = 0;
            NumBands = 4;
            BytesPerPixel = 4;
            Rectangle bnds = new Rectangle(0, 0, _myImage.Width, _myImage.Height);
            BitmapData bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            base.Values = new byte[bData.Height * bData.Stride];
            Marshal.Copy(bData.Scan0, base.Values, 0, bData.Height * bData.Stride);
            _myImage.UnlockBits(bData);
        }

        /// <summary>
        /// Closes the image content.
        /// </summary>
        public override void Close()
        {
            // This doesn't need to do anything
        }

        /// <summary>
        /// Copies the values from the specified source image.
        /// </summary>
        /// <param name="source">The source image to copy values from.</param>
        public override void CopyValues(IImageData source)
        {
            Values = (byte[])source.Values.Clone();
            NumBands = source.NumBands;
            BytesPerPixel = source.BytesPerPixel;
        }

     
       
        /// <summary>
        /// Creates a new image and world file, placing the default bounds at the origin, with one pixel per unit.
        /// </summary>
        /// <param name="filename">The string filename</param>
        /// <param name="width">The integer width</param>
        /// <param name="height">The integer height</param>
        public override void CreateNew(string filename, int width, int height)
        {
            Filename = filename;
            WorldFile = new WorldFile();
            double[] aff = new[] { 1.0, 0, 0, -1.0, 0, height };
            Bounds = new RasterBounds(height, width, aff);
            WorldFile.Filename = WorldFile.GenerateFilename(filename);
            _myImage = new Bitmap(width, height);
            string ext = System.IO.Path.GetExtension(filename);
            switch (ext)
            {
                case ".bmp": _myImage.Save(filename, ImageFormat.Bmp); break;
                case ".emf": _myImage.Save(filename, ImageFormat.Emf); break;
                case ".exf": _myImage.Save(filename, ImageFormat.Exif); break;
                case ".gif": _myImage.Save(filename, ImageFormat.Gif); break;
                case ".ico": _myImage.Save(filename, ImageFormat.Icon); break;
                case ".jpg": _myImage.Save(filename, ImageFormat.Jpeg); break;
                case ".mbp": _myImage.Save(filename, ImageFormat.MemoryBmp); break;
                case ".png": _myImage.Save(filename, ImageFormat.Png); break;
                case ".tif": _myImage.Save(filename, ImageFormat.Tiff); break;
                case ".wmf": _myImage.Save(filename, ImageFormat.Wmf); break;
            }
            NumBands = 4;
            BytesPerPixel = 4;
            ReadBytes();
        }

       


        /// <summary>
        /// Release any unmanaged memory objects
        /// </summary>
        public override void Dispose()
        {
            _myImage.Dispose();
        }

        /// <summary>
        /// Returns the internal bitmap in this case.  In other cases, this may have to be constructed
        /// from the unmanaged memory content.
        /// </summary>
        /// <returns>A Bitmap that represents the entire image.</returns>
        public override Bitmap GetBitmap()
        {
            return _myImage;
        }



        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">The geographic extents to retrieve data for</param>
        /// <param name="window">The rectangle that defines the size of the drawing area in pixels</param>
        /// <returns>A bitmap captured from the main image </returns>
        public override Bitmap GetBitmap(IEnvelope envelope, Rectangle window)
        {
            if (window.Width == 0 || window.Height == 0) return null;
            Bitmap result = new Bitmap(window.Width, window.Height);
            //if (_bounds.Envelope.Intersects(envelope) == false) return result;
            Graphics g = Graphics.FromImage(result);

            double imageToWorldW = WorldFile.Affine[1];
            double imageToWorldH = WorldFile.Affine[5];

            float cw = (float)(imageToWorldW * (window.Width / envelope.Width)); // cell width
            float ch = -(float)(imageToWorldH * (window.Height / envelope.Height)); // cell height
            //float sx = cw * (float)(_worldFile.Affine[2] / _worldFile.Affine[1]);
            //float sy = ch * (float)(_worldFile.Affine[4] / _worldFile.Affine[5]);
            const float sx = 0;
            const float sy = 0;
            float l = (float) (Bounds.Left());
            float t = (float) (Bounds.Top());
            float dx = (float)((l - envelope.Minimum.X) * (window.Width / envelope.Width));
            float dy = (float)((envelope.Maximum.Y - t) * (window.Height / envelope.Height));
            g.Transform = new Matrix(cw, sx, sy, ch, dx, dy);
            g.PixelOffsetMode = PixelOffsetMode.Half;
            if(cw > 1 || ch > 1) g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(_myImage, new PointF(0, 0));
            g.Dispose();
            return result;

        }


        /// <summary>
        /// Opens the file, assuming that the filename has already been specified using a Dot Net Image object
        /// </summary>
        public override void Open()
        {
            if (_myImage != null) _myImage.Dispose();
            Image temp;
            try
            {
                temp = Image.FromFile(Filename);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("The Dot Net Image object threw the following exception: " + ex.Message);
                return;
            }
            _myImage = new Bitmap(temp.Width, temp.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(_myImage);
            g.DrawImage(temp, new Rectangle(0, 0, temp.Width, temp.Height));
            g.Dispose();
            temp.Dispose();
            WorldFile = new WorldFile(Filename);
            if (WorldFile.Affine == null)
            {
                WorldFile.Affine = new[] { .5, 1.0, 0, _myImage.Height - .5, 0, -1.0 };
            }
          
            Bounds = new RasterBounds(_myImage.Height, _myImage.Width, WorldFile.Affine);
            NumBands = 4;
            BytesPerPixel = 4;
            ReadBytes();
        }

        /// <summary>
        /// Saves the current image and world file.
        /// </summary>
        public override void Save()
        {
            _myImage.Save(Filename);
            WorldFile.Save();

        }

        /// <summary>
        /// Saves the image to the specified filename
        /// </summary>
        /// <param name="filename">The string filename to save this as</param>
        public override void SaveAs(string filename)
        {
            Filename = filename;
            if (WorldFile != null)
            {
                WorldFile.Filename = WorldFile.GenerateFilename(filename);
            }
            Save();
        }

        /// <summary>
        /// Gets the height of this image
        /// </summary>
        public override int Height
        {
            get
            {
                if(_myImage != null)return _myImage.Height;
                return 0;
            }
        }

        /// <summary>
        /// Gets the width of this image
        /// </summary>
        public override int Width
        {
            get
            {
                if(_myImage != null)return _myImage.Width;
                return 0;
            }
        }


        /// <summary>
        /// Forces the image to read values from the graphic image format to the byte array format
        /// </summary>
        public override void ReadBytes()
        {
            Rectangle bnds = new Rectangle(0, 0, Width, Height);
            BitmapData bData = null;
            if (NumBands == 4)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                
            }
            if (NumBands == 3)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }
            if (NumBands == 1)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format16bppGrayScale);
            }
            if (bData == null)
            {
                throw new ApplicationException("The specified number of bands is not supported.");
            }
            Stride = bData.Stride;

            Values = new byte[bData.Height * bData.Stride];
            Marshal.Copy(bData.Scan0, Values, 0, bData.Height * bData.Stride);
            _myImage.UnlockBits(bData);
        }

        /// <summary>
        /// Forces the image to copy values from the byte array format to the image format.
        /// </summary>
        public override void WriteBytes()
        {
            Rectangle bnds = new Rectangle(0, 0, Width, Height);
            BitmapData bData = null;
            if (NumBands == 4)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            }
            if (NumBands == 3)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }
            if (NumBands == 1)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format16bppGrayScale);
            }
            if (bData == null)
            {
                throw new ApplicationException("The specified number of bands is not supported.");
            }
            Marshal.Copy(Values, 0, bData.Scan0, Values.Length);
            _myImage.UnlockBits(bData);
        }

    }


}
