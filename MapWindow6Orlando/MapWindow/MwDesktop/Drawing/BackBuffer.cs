//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Drawing;
using MapWindow.Geometries;
namespace MapWindow.Drawing
{
    /// <summary>
    /// This class contains two separate layers.  The first is the BackImage where features that don't change
    /// over time are drawn.  This way if heavy calculations are required to draw the background, you don't
    /// have to re-draw the background over and over again every time a sprite moves.  The Front image is
    /// for small sprites that change rapidly, but 
    /// </summary>
    public class BackBuffer
    {
        #region Events

        /// <summary>
        /// Occurs after something changes the geographic extents.  This refers to the outer
        /// geographic extents, not the view extents.
        /// </summary>
        public event EventHandler ExtentsChanged;


        #endregion

        #region Private Variables

        /// <summary>
        /// The image being shown
        /// </summary>
        private Bitmap _image;
       
        /// <summary>
        /// The real world extents for the entire buffer
        /// </summary>
        private IEnvelope _extents;

        readonly int _originalThreadID;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new BackBuffer bitmap that is the specified size.
        /// </summary>
        /// <param name="width">The width of the bitmap</param>
        /// <param name="height">The height of the bitmap</param>
        public BackBuffer(int width, int height)
        {
            _image = new Bitmap(width, height);
            _extents = new Envelope();
            _originalThreadID = System.Threading.Thread.CurrentThread.GetHashCode();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the back buffer.
        /// </summary>
        public virtual void Clear()
        {
            Graphics g = Graphics.FromImage(_image);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, _image.Width, _image.Height));
            g.Dispose();
        }


        /// <summary>
        /// This draws the back buffer image onto a graphics object that has been scaled and
        /// translated via the graphics transforms into world coordinates.  The bounds
        /// are specified in geographic coordinates, as are the extents for this graphics object.
        /// </summary>
        /// <param name="g">The System.Drawing.Graphics surface to draw on.</param>
        /// <param name="bounds">The geographic extents to draw</param>
        public virtual void Draw2D(Graphics g, IEnvelope bounds)
        {
            // Ensure we have something on the backbuffer that we want to draw
            if (_extents == null) return;
            if (_extents.Width == 0 || _extents.Height == 0) return;
            if (bounds.Intersects(_extents) == false) return;

            IEnvelope sourceBounds = bounds.Intersection(_extents);
            RectangleF sourceRect = ProjToPixel(sourceBounds);
            //RectangleF destRect = new RectangleF(0, 0,Convert.ToSingle(sourceBounds.Width), Convert.ToSingle(sourceBounds.Height));

            float sx = Convert.ToSingle(_extents.Width/bounds.Width);
            float sy = Convert.ToSingle(_extents.Height/bounds.Height);
            float tx = 0f;
            float ty = 0f;

            if (bounds.Minimum.X < _extents.Minimum.X) tx = Convert.ToSingle((bounds.Minimum.X - _extents.Minimum.X) * Width / bounds.Width);
            if (bounds.Maximum.Y > _extents.Maximum.Y)ty = Convert.ToSingle((bounds.Maximum.Y - _extents.Maximum.Y) * Height / bounds.Height);
            RectangleF destRect = new RectangleF(-tx, ty, sourceRect.Width * sx, sourceRect.Height * sy);
            g.DrawImage(_image, destRect, sourceRect, GraphicsUnit.Pixel);
           
        }

    

        #endregion

        #region Properties

       

        /// <summary>
        /// Gets or sets the geographic extents for the entire back buffer.
        /// </summary>
        public IEnvelope Extents
        {
            get { return _extents; }
            set 
            { 
                _extents = value;
                //System.Diagnostics.Debug.WriteLine("Buffer Extents Set.");
            }
        }

        /// <summary>
        /// Gets the graphics drawing surface for the BackBuffer
        /// </summary>
        public Graphics Graphics
        {
            get { return Graphics.FromImage(_image); }
        }


        /// <summary>
        /// Gets or sets the height of this backbuffer in pixels
        /// </summary>
        public int Height
        {
            get { return _image.Height; }
        }

       
        /// <summary>
        /// Gets or sets the actual Bitmap being used as the back buffer.
        /// </summary>
        public Bitmap Image
        {
            get {
                return _image;
            }
            set
            {
                lock (this)
                {
                    _image = value;
                
                }        
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testPoint"></param>
        /// <returns></returns>
        public Coordinate PixelToProj(PointF testPoint)
        {
            Coordinate coord = new Coordinate();
            coord.X = testPoint.X * _extents.Width / _image.Width + _extents.Minimum.X;
            coord.Y = _extents.Maximum.Y - testPoint.Y * _extents.Height / _image.Height; 
            return coord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testExtents"></param>
        /// <returns></returns>
        public IEnvelope PixelToProj(RectangleF testExtents)
        {
            Coordinate UL = PixelToProj(new PointF(testExtents.Left, testExtents.Top));
            Coordinate LR = PixelToProj(new PointF(testExtents.Right, testExtents.Bottom));
            IEnvelope result = new Envelope(UL.X, UL.Y, (LR.X - UL.X), (LR.Y - UL.Y));
            return result;
        }

        /// <summary>
        /// Calculates a system.Drawing rectangle that corresponds to the specified real world
        /// envelope if it were drawn in pixels on the background image.  
        /// </summary>
        /// <param name="testExtents"></param>
        /// <returns></returns>
        public RectangleF ProjToPixel(IEnvelope testExtents)
        {
            RectangleF result = new RectangleF(0F, 0F, _image.Width, _image.Height);
            if(_extents == null || _extents.IsNull)return result;
            
            PointF UL = ProjToPixel(new Coordinate(testExtents.Minimum.X, testExtents.Maximum.Y));
            PointF LR = ProjToPixel(new Coordinate(testExtents.Maximum.X, testExtents.Minimum.Y));
            
            return new RectangleF(UL.X, UL.Y, (LR.X - UL.X), (LR.Y - UL.Y));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public PointF ProjToPixel(Coordinate coord)
        {
            PointF pt = new PointF();
            pt.X = Convert.ToSingle(((coord.X - _extents.Minimum.X) * _image.Width / _extents.Width));
            pt.Y = Convert.ToSingle(((_extents.Maximum.Y - coord.Y) * _image.Height / _extents.Height));
            return pt;
        }



        /// <summary>
        /// Boolean, true if the current thread is different from the original thread, indicating
        /// that cross-threading is taking place.
        /// </summary>
        public bool InvokeRequired
        {
            get
            {
                if (System.Threading.Thread.CurrentThread.GetHashCode() != _originalThreadID)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the width of the back buffer in pixels.  This will copy
        /// the old back buffer to a new bitmap with the new width/height
        /// </summary>
        public int Width
        {
            get { return _image.Width; }
        }

        /// <summary>
        /// Obtains a graphics object already organized into world coordinates.
        /// The client rectangle and world coordinates are used in order to determine
        /// the scale and translation of the transform necessary in the graphics object.
        /// </summary>
        public virtual Graphics WorldGraphics
        {
            get
            {
                return Graphics.FromImage(_image);

                // The following code was made obsolete by the introduction of a draw window
                //if (_extents.IsNull) return result;
                //System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
                //float sx = Convert.ToSingle(_extents.Width / (double)_image.Width);
                //float sy = Convert.ToSingle(_extents.Height / (double)_image.Height);
                //m.Scale(1/sx, -1/sy);
                //float dx = Convert.ToSingle(_extents.Minimum.X);
                //float dy = Convert.ToSingle(_extents.Maximum.Y);
                //m.Translate(-dx, -dy);
                //result.Transform = m;
                //RectangleF clipRect = new RectangleF(Convert.ToSingle(_extents.Minimum.X), Convert.ToSingle(_extents.Maximum.Y), Convert.ToSingle(_extents.Width), Convert.ToSingle(_extents.Height));
                ////result.Clip = new Region(clipRect);
                //return result;
            }

        }
       

       

        #endregion

        #region Protected Methods


       

        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        protected virtual void OnExtentsChanged()
        {
            if (ExtentsChanged != null)
            {
                ExtentsChanged(this, new EventArgs());
            }
        }

       


        #endregion


        /// <summary>
        /// The envelope bounds in geographic coordinates.
        /// </summary>
        public IEnvelope Envelope
        {
            get { return _extents; }
        }


    }
}
