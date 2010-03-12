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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 4:28:29 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// PictureSymbol
    /// </summary>
    public class PictureSymbol : OutlinedSymbol, IPictureSymbol
    {
        #region Private Variables


        private Image _image;
        private Image _original; // Non-transparent version
        private string _imageFilename;
        private float _opacity;
        private bool _disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PictureSymbol
        /// </summary>
        public PictureSymbol()
        {
            base.SymbolType = SymbolTypes.Picture;
            _opacity = 1F;
        }

        /// <summary>
        /// Creates a new instance of a PictureSymbol from the specified image
        /// </summary>
        /// <param name="image">The image to use when creating the symbol</param>
        public PictureSymbol(Image image)
        {
            base.SymbolType = SymbolTypes.Picture;
            _opacity = 1F;
            this.Image = image;
        
            
        }

        /// <summary>
        /// Creates a new instance of a PictureSymbol from the specified image.
        /// The larger dimension from the image will be adjusted to fit the size,
        /// while the smaller dimension will be kept proportional.
        /// </summary>
        /// <param name="image">The image to use for this symbol</param>
        /// <param name="size">The double size to use for the larger of the two dimensions of the image.</param>
        public PictureSymbol(Image image, double size)
        {
            base.SymbolType = SymbolTypes.Picture;
            _opacity = 1F;
            this.Image = image;
            if (image == null) return;
            double scale = 1;
            if(image.Width > image.Height)
            {
                scale = size / (double)image.Width;
            }
            else
            {
                scale = size / (double)image.Height;
            }
            base.Size = new Size2D(scale * image.Width, scale * image.Height);
        }

        /// <summary>
        /// Creates a new instance of a PictureSymbol from the specified icon
        /// </summary>
        /// <param name="icon">The icon to use when creating this symbol</param>
        public PictureSymbol(Icon icon)
        {
            base.SymbolType = SymbolTypes.Picture;
            _opacity = 1F;
            _original = icon.ToBitmap();
            _image = MakeTransparent(_original, _opacity);
        }

        /// <summary>
        /// Creates a new PictureSymbol given an existing imageData object.
        /// </summary>
        /// <param name="imageData">The imageData object to use.</param>
        public PictureSymbol(IImageData imageData)
        {
            base.SymbolType = SymbolTypes.Picture;
            _opacity = 1F;
            _image = imageData.GetBitmap();
            _original = _image;
            _imageFilename = imageData.Filename;
        }

        #endregion

        #region Methods

       

        public override void SetColor(Color color)
        {

            Bitmap bm = new Bitmap(_image.Width, _image.Height);
            Graphics g = Graphics.FromImage(bm);

            float r = (color.R/255f)/2;
            float gr = (color.G/255f)/2;
            float b = (color.B/255f)/2;
            // GrayScale first

            //ColorMatrix cm = new ColorMatrix(new float[][]{   new float[]{0.229f,0.229f,0.229f,0,0},
            //                      new float[]{0.587f,0.587f,0.587f,0,0},
            //                      new float[]{0.114f,0.144f,0.144f,0,0},
            //                      new float[]{0,0,0,1,0,0},
            //                      new float[]{0,0,0,0,1,0},
            //                      new float[]{0,0,0,0,0,1}});

            ColorMatrix cm = new ColorMatrix(new float[][]{   new float[]{r,gr,b,0,0},
                                  new float[]{r,gr,b,0,0},
                                  new float[]{r,gr,b,0,0},
                                  new float[]{0,0,0,1,0,0},
                                  new float[]{0,0,0,0,1,0},
                                  new float[]{0,0,0,0,0,1}});

          


            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm);
            g.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height), 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();
            _image = bm;


        }


        /// <summary>
        /// Disposes the current images
        /// </summary>
        public void Dispose()
        {
            OnDisposing();
            _disposed = true;
        }



        #endregion

        #region Properties

        /// <summary>
        /// This is set to true when the dispose method is called.
        /// This will be set to false again if the image is set after that.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
            set { _disposed = value; }
        }


        /// <summary>
        /// Gets or sets the image to use when the PictureMode is set to Image
        /// </summary>
        [XmlIgnore]
        public Image Image
        {
            get { return _image; }
            set 
            {
                if (_original != null && _original != value)
                {
                    _original.Dispose();
                    _original = null;
                }
                if (_image != null && _image != _original && _image != value)
                {
                    _image.Dispose();
                    _image = null;
                }
                _original = value;
                _image = MakeTransparent(value, _opacity);
                if (_original != null) _disposed = false;
            }
        }

        /// <summary>
        /// Gets or sets the string image filename to use
        /// </summary>
		[Serialize("ImageFilename")]
		public string ImageFilename
        {
            get { return _imageFilename; }
            set 
            {
                _imageFilename = value;
                if (_original != null) _original.Dispose();
                _original = null;
                if (_image != null) _image.Dispose();
                _image = null;
                if (_imageFilename == null) return;
                if (System.IO.File.Exists(_imageFilename) == false) return;
                if (System.IO.Path.GetExtension(_imageFilename) == ".ico")
                {
                    _original = new Icon(_imageFilename).ToBitmap();
                }
                else
                {
                    _original = Image.FromFile(_imageFilename);
                }
                _image = MakeTransparent(_original, _opacity);
            }
        }

        /// <summary>
        /// Gets or sets the opacity for this image.  Setting this will automatically change the image in memory.
        /// </summary>
		[Serialize("Opacity")]
		public float Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                if (_image != null && _image != _original) _image.Dispose();
                _image = MakeTransparent(_original, _opacity);
            }
        }



        #endregion

        #region Protected Methods

        /// <summary>
        /// This helps the copy process by preparing the internal variables AFTER memberwiseclone has created this
        /// 
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Descriptor copy)
        {
            // Setting the image property has a built in check to dispose the older reference.
            // After a MemberwiseClone, however, this older reference is still in use in the original PictureSymbol.
            // We must, therefore, set the private variables to null before doing the cloning.
            PictureSymbol duplicate = copy as PictureSymbol;
            if (duplicate != null)
            {
                duplicate._image = null;
                duplicate._original = null;
            }
            base.OnCopy(copy);
            if(duplicate != null)
            {
                duplicate._image = _image.Copy();
                duplicate._original = _original.Copy();
            }
        }

        /// <summary>
        /// OnDraw
        /// </summary>
        /// <param name="g">Graphics object</param>
        /// <param name="scaleSize">The double scale Size</param>
        protected override void OnDraw(Graphics g, double scaleSize)
        {
            float dx = (float)(scaleSize * Size.Width/2);
            float dy = (float)(scaleSize * Size.Height / 2);
            GraphicsPath gp = new GraphicsPath();
            gp.AddRectangle(new RectangleF(-dx, -dy, dx*2, dy*2));
            if (_image != null)
            {
                g.DrawImage(_image, new RectangleF(-dx, -dy, dx*2, dy*2));
            }
            base.OnDrawOutline(g, scaleSize, gp);
            gp.Dispose();
        }

        /// <summary>
        /// We can randomize the opacity
        /// </summary>
        /// <param name="generator"></param>
        protected override void OnRandomize(Random generator)
        {
            base.OnRandomize(generator);
            Opacity = generator.NextFloat();
            
        }

       

        /// <summary>
        /// Overrideable functions for handling the basic disposal of image classes in this object.
        /// </summary>
        protected virtual void OnDisposing()
        {
            _image.Dispose();
            _original.Dispose();
        }

        #endregion

        private static Image MakeTransparent(Image image, float opacity)
        {
            if (image == null) return null;
            if (opacity == 1F) return image.Clone() as Image;
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            Graphics g = Graphics.FromImage(bmp);
            float[][] ptsArray ={ 
                 new float[] {1, 0, 0, 0, 0},  // R
                 new float[] {0, 1, 0, 0, 0},  // G
                 new float[] {0, 0, 1, 0, 0},  // B
                 new float[] {0, 0, 0, opacity, 0}, // A
                 new float[] {0, 0, 0, 0, 1}};
            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            ImageAttributes att = new ImageAttributes();
            att.SetColorMatrix(clrMatrix);
            g.DrawImage(image, new Rectangle(0, 0,image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, att);
            g.Dispose();
            return bmp;
        }
    }
}
