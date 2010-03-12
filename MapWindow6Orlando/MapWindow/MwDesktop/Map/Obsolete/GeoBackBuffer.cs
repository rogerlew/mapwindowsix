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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/7/2008 1:46:17 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace MapWindow.GeoMap
{


    /// <summary>
    /// GeoBackBuffer
    /// </summary>
    public class GeoBackBuffer : IBasicBackBuffer
    {
        #region Private Variables

        Bitmap _image; // Always draw from this image
        Bitmap _editImage; // Always draw to this image.
        Rectangle _clientRectangle;
        Rectangle _sourceRectangle;
        Rectangle _bufferRectangle;
        

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoBackBuffer
        /// </summary>
        public GeoBackBuffer(Rectangle ClientRectangle)
        {
            // make the back buffer 3 times the size of the map on the screen
            _image = new Bitmap(ClientRectangle.Width * 3, ClientRectangle.Height * 3, PixelFormat.Format32bppArgb);
            _bufferRectangle = new Rectangle(0, 0, ClientRectangle.Width * 3, ClientRectangle.Height * 3);
            _editImage = new Bitmap(ClientRectangle.Width * 3, ClientRectangle.Height * 3, PixelFormat.Format32bppArgb);
            _clientRectangle = ClientRectangle;
            _sourceRectangle = new Rectangle(_clientRectangle.Width, _clientRectangle.Height, _clientRectangle.Width, _clientRectangle.Height);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This clears the specified clip rectangle.  If no rectangle is specified, it clears the current
        /// ClientRectangle region.
        /// </summary>
        public void Clear(Rectangle clipRectangle, Color BackgroundColor)
        {
            Rectangle clip = clipRectangle;
            if (clipRectangle.IsEmpty) clip = _sourceRectangle;
           
            Graphics g = Graphics.FromImage(_editImage);
            Brush b = new SolidBrush(BackgroundColor);
            g.FillRectangle(b, clip);
            b.Dispose();
            g.Dispose();
        }

        /// <summary>
        /// Draws the cliprectangel to the graphics device.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        public virtual void Draw(Graphics g, Rectangle clipRectangle)
        {
            Rectangle temp = ClientToBuffer(clipRectangle);
            Rectangle clip = Rectangle.Intersect(temp, _bufferRectangle);
            Rectangle result = BufferToClient(clip);
            g.DrawImage(_image, result, clip, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Translates and scales a given rectangle from client coordinates to the equivalent
        /// coordinates as determined by the ClientRectangle for this object.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Rectangle ClientToBuffer(Rectangle rect)
        {
            Rectangle result = rect;
            System.Drawing.Point tl = new System.Drawing.Point(rect.X, rect.Y);
            System.Drawing.Point br = new System.Drawing.Point(rect.Right, rect.Bottom);
            tl = ClientToBuffer(tl);
            br = ClientToBuffer(br);
            return new Rectangle(tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);
        }

        /// <summary>
        /// Translates and scales a given rectangle from client coordinates to the equivalent
        /// coordinates as determined by the ClientRectangle for this object.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Rectangle BufferToClient(Rectangle rect)
        {
            if (_sourceRectangle.IsEmpty) return rect;
            if (_sourceRectangle.Width == 0 || _sourceRectangle.Height == 0) return rect;
            System.Drawing.Point tl = new System.Drawing.Point(rect.X, rect.Y);
            System.Drawing.Point br = new System.Drawing.Point(rect.Right, rect.Bottom);
            tl = BufferToClient(tl);
            br = BufferToClient(br);
            return new Rectangle(tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);
        }

        /// <summary>
        /// Gets a the factor
        /// </summary>
        public System.Drawing.Point ClientToBuffer(System.Drawing.Point position)
        {
            System.Drawing.Point result = new System.Drawing.Point();
            result.X = (position.X * _sourceRectangle.Width) / _clientRectangle.Width + _sourceRectangle.X;
            result.Y = (position.Y * _sourceRectangle.Height) / _clientRectangle.Height + _sourceRectangle.Y;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public System.Drawing.Point BufferToClient(System.Drawing.Point position)
        {
            System.Drawing.Point result = new System.Drawing.Point();
            result.X = ((position.X - _sourceRectangle.X) * _clientRectangle.Width) / _sourceRectangle.Width;
            result.Y = ((position.Y - _sourceRectangle.Y) * _clientRectangle.Height) / _sourceRectangle.Height;
            return result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The current image stored as a back buffer
        /// </summary>
        public Bitmap Image
        {
            get { return _image; }
            set { _image = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle representing the true size of the map control
        /// </summary>
        public Rectangle ClientRectangle
        {
            get { return _clientRectangle; }
            set { _clientRectangle = value; }
        }

        /// <summary>
        /// Gets or sets the source rectangle in back buffer coordinates where the image
        /// should be copied from.
        /// </summary>
        public Rectangle SourceRectangle
        {
            get { return _sourceRectangle; }
            set { _sourceRectangle = value; }
        }

        /// <summary>
        /// Gets a drawing device that is configured so that drawing using client coordinates
        /// will automatically draw in the correct spot on the back buffer
        /// </summary>
        public Graphics CreateGraphics()
        {
            // Draw to the edit image, not the master image.  Otherwise we could have a conflict
            // where we are trying to draw to the map and update the image at the same time.
            _editImage = new Bitmap(_image.Width, _image.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(_editImage);
            g.DrawImageUnscaled(_image, 0, 0);
            Matrix m = new Matrix();
            m.Translate((float)_sourceRectangle.X, (float)_sourceRectangle.Y);
            m.Scale((float)_clientRectangle.Width/(float)_sourceRectangle.Width,  (float)_clientRectangle.Height/(float)_sourceRectangle.Height);
            g.Transform = m;
            return g;
            
        }

        /// <summary>
        /// When the image has been updated this rapidly replaced the image with the edit image
        /// </summary>
        public void Update()
        {
            if (_image != null) _image.Dispose();
            _image = _editImage;
        }

       
       



        #endregion



    }
}
