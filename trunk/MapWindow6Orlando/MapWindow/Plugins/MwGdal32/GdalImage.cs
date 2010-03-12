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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using MapWindow.Data;
using MapWindow.Geometries;
using OSGeo.GDAL;
namespace MapWindow.Gdal32
{


    /// <summary>
    /// gdalImage
    /// </summary>
    public class GdalImage : ImageData
    {
        #region Private Variables

        Dataset _dataset;
        Bitmap _image;
        Band _red;
        Band _green;
        Band _blue;
        Band _alpha;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of gdalImage, and gets much of the header information without actually
        /// reading any values from the file.
        /// </summary>
        public GdalImage(string filename)
        {
            Filename = filename;
            WorldFile = new WorldFile();
            WorldFile.Affine = new double[6];

            Gdal.AllRegister();
            ReadHeader();
            

        }


        /// <summary>
        /// Creates a new instance of gdalImage
        /// </summary>
        public GdalImage()
        {
            Gdal.AllRegister();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the size of the whole image, but doesn't keep the image open unless it was already open.
        /// </summary>
        /// <returns></returns>
        private void ReadHeader()
        {
            try
            {
                _dataset = Gdal.Open(Filename, Access.GA_Update);
            }
            catch
            {
                try
                {
                    _dataset = Gdal.Open(Filename, Access.GA_ReadOnly);
                }
                catch (Exception ex)
                {
                    throw new GdalException(ex.ToString());
                }
            }
            Width = _dataset.RasterXSize;
            Height = _dataset.RasterYSize;
            NumBands = _dataset.RasterCount;
            double[] test = new double[6];
            _dataset.GetGeoTransform(test);
            Bounds = new RasterBounds(base.Height, base.Width, test);
            WorldFile.Affine = test;
            Close();
        }

        public override void Close()
        {
            _dataset.Dispose();
            _dataset = null;
        }
       

        /// <summary>
        /// Attempts to open the specified file.
        /// </summary>
        public override void Open()
        {
            try
            {
                _dataset = Gdal.Open(Filename, Access.GA_Update);
            }
            catch
            {
                try
                {
                    _dataset = Gdal.Open(Filename, Access.GA_ReadOnly);
                }
                catch (Exception ex)
                {
                    throw new GdalException(ex.ToString());
                }
            }

            _red = _dataset.GetRasterBand(1);
            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_PaletteIndex)
            {
                ReadPaletteBuffered();
            }
            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex)
            {
                ReadGrayIndex();
            }
            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_RedBand)
            {
                ReadRGB();
            }
            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_AlphaBand)
            {
                ReadARGB();
            }


        }

        /// <summary>
        /// This needs to return the actual image and override the base behavior that handles
        /// the internal variables only.
        /// </summary>
        /// <param name="envelope">The envelope to grab image data for.</param>
        /// <param name="window">A Rectangle</param>
        /// <returns></returns>
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
            float dx = (float)((WorldFile.Affine[0] - envelope.Minimum.X) * (window.Width / envelope.Width));
            float dy = (float)((envelope.Maximum.Y - WorldFile.Affine[3]) * (window.Height / envelope.Height));
            g.Transform = new System.Drawing.Drawing2D.Matrix(cw, 0, 0, ch, dx, dy);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            if (_image != null)
            {
                g.DrawImage(_image, new System.Drawing.Point(0, 0));
            }
            g.Dispose();
            return result;
        }



        #endregion

        #region Properties



        #endregion

        private void ReadPaletteBuffered()
        {
            ColorTable ct = _red.GetRasterColorTable();
            if (ct == null)
            {
                throw new GdalException("Image was stored with a palette interpretation but has no color table.");
            }
            if (ct.GetPaletteInterpretation() != PaletteInterp.GPI_RGB)
            {
                throw new GdalException("Only RGB palette interpreation is currently supported by this plugin, " + ct.GetPaletteInterpretation() + " is not supported.");
            }
            int width = Width;
            int height = Height;
            byte[] r = new byte[width * height];
            _red.ReadRaster(0, 0, width, height, r, width, height, 0, 0);
            
            _image = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            BitmapData bData = _image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            _image.UnlockBits(bData);
            
            const int bpp = 4;
            int stride = Stride;
            byte[] vals = new byte[width * height * 4];
            byte[][] colorTable = new byte[256][];
            for (int i = 0; i < 255; i++ )
            {
                ColorEntry ce = ct.GetColorEntry(i);
                colorTable[i] = new[]{(byte)ce.c3, (byte)ce.c2, (byte)ce.c1, (byte)ce.c4};
            }
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Array.Copy(colorTable[r[col + row * width]], 0, vals, row * stride + col * bpp, 4);
                }
            }
            Values = vals;
            WriteBytes();
        }


        private void ReadGrayIndex()
        {
            Width = _red.XSize;
            Height = _red.YSize;
            _image = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);
            byte[] r = new byte[Width * Height];
            _red.ReadRaster(0, 0, Width, Height, r, Width, Height, 0, 0);
            BitmapData bData = _image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            _image.UnlockBits(bData);
            byte[] vals = new byte[Width * Height * 4];
            
            BytesPerPixel = 4;
            int height = Height;
            int width = Width;
            int stride = Stride;
            int bpp = BytesPerPixel;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * stride + col * bpp] = r[row * width + col];
                    vals[row * stride + col * bpp + 1] = r[row * width + col];
                    vals[row * stride + col * bpp + 2] = r[row * width + col];
                    vals[row * stride + col * bpp + 3] = 255;
                }
            }
            Values = vals;
            WriteBytes();
        }

        private void ReadRGB()
        {

            if (_dataset.RasterCount < 3)
            {
                throw new GdalException("RGB Format was indicated but there are only " + _dataset.RasterCount + " bands!");
            }
            _green = _dataset.GetRasterBand(2);
            _blue = _dataset.GetRasterBand(3);

            Width = _red.XSize;
            Height = _red.YSize;

            _image = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);


            byte[] r = new byte[Width * Height];
            byte[] g = new byte[Width * Height];
            byte[] b = new byte[Width * Height];

            _red.ReadRaster(0, 0, Width, Height, r, Width, Height, 0, 0);
            _green.ReadRaster(0, 0, Width, Height, g, Width, Height, 0, 0);
            _blue.ReadRaster(0, 0, Width, Height, b, Width, Height, 0, 0);

            BitmapData bData = _image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            _image.UnlockBits(bData);
            byte[] vals = new byte[Width * Height * 4];
            BytesPerPixel = 4;
            int stride = Stride;
            int bpp = BytesPerPixel;
            int width = Width;
            int height = Height;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * stride + col * bpp] = b[row * width + col];
                    vals[row * stride + col * bpp + 1] = g[row * width + col];
                    vals[row * stride + col * bpp + 2] = r[row * width + col];
                    vals[row * stride + col * bpp + 3] = 255;
                }
            }
            Values = vals;
            WriteBytes();
        }

        private void ReadARGB()
        {
            if (_dataset.RasterCount < 4)
            {
                throw new GdalException("ARGB Format was indicated but there are only " + _dataset.RasterCount + " bands!");
            }
            _alpha = _red;
            _red = _dataset.GetRasterBand(2);
            _green = _dataset.GetRasterBand(3);
            _blue = _dataset.GetRasterBand(4);

            Width = _red.XSize;
            Height = _red.YSize;

            _image = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);

            byte[] a = new byte[Width * Height];
            byte[] r = new byte[Width * Height];
            byte[] g = new byte[Width * Height];
            byte[] b = new byte[Width * Height];

            
            _alpha.ReadRaster(0, 0, Width, Height, a, Width, Height, 0, 0);
            _red.ReadRaster(0, 0, Width, Height, r, Width, Height, 0, 0);
            _green.ReadRaster(0, 0, Width, Height, g, Width, Height, 0, 0);
            _blue.ReadRaster(0, 0, Width, Height, b, Width, Height, 0, 0);

            BitmapData bData = _image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            _image.UnlockBits(bData);
            Values = new byte[Width * Height * 4];
            BytesPerPixel = 4;
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    Values[row * Stride + col * BytesPerPixel] = b[row * Width + col];
                    Values[row * Stride + col * BytesPerPixel + 1] = g[row * Width + col];
                    Values[row * Stride + col * BytesPerPixel + 2] = r[row * Width + col];
                    Values[row * Stride + col * BytesPerPixel + 3] = a[row * Width + col];
                }
            }
            WriteBytes();
        }

        /// <summary>
        /// Reads the actual image values from the image file into the array of Values, which can be accessed by the Color property.
        /// </summary>
        public override void ReadBytes()
        {
            BitmapData bData = _image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            Marshal.Copy(bData.Scan0, Values, 0, bData.Height * bData.Stride);
            _image.UnlockBits(bData);
        }

        /// <summary>
        /// Writes the byte values stored in the Bytes array into the bitmap image.
        /// </summary>
        public override void WriteBytes()
        {
            Rectangle bnds = new Rectangle(0, 0, Width, Height);
            BitmapData bData = _image.LockBits(bnds, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(Values, 0, bData.Scan0, Values.Length);
            _image.UnlockBits(bData);
        }

    }
}
