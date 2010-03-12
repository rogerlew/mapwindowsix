//********************************************************************************************************
// Product Name: MapWindow.Layout.Elements.LayoutBitmap
// Description:  The MapWindow LayoutBitmap element, holds bitmaps loaded from disk for the layout
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
// The Original Code is MapWindow.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
//********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;

namespace MapWindow.Layout.Elements
{
    /// <summary>
    /// The layout bitmap provides the ability to add any custom image to the layout
    /// </summary>
    public class LayoutBitmap : LayoutElement 
    {
        private string _filename;
        private Bitmap _bitmap;
        private bool _draft;
        private bool _preserveAspectRatio;

        #region ------------------ Public Properties

        /// <summary>
        /// Preserves the aspect ratio if this boolean is true, otherwise it allows stretching of
        /// the bitmap to occur
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public bool PreserveAspectRatio
        {
            get { return _preserveAspectRatio; }
            set { _preserveAspectRatio = value; UpdateThumbnail(); OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the string filename of the bitmap to use
        /// </summary>
        [Browsable(true), Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(UITypeEditor)), Category("Symbol")]
        public string Filename
        {
            get { return _filename; }
            set 
            {
                try
                {
                    new Bitmap(value);
                    _filename = value;
                    if (_bitmap != null) _bitmap.Dispose();
                    _bitmap = new Bitmap(_filename);
                }
                catch
                {
                    _filename = "";
                }
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Allows for a faster but lower quality bitmap to be rendered to the screen
        /// and a higher quality bitmap to be printed during actual printing.
        /// </summary>
        [Browsable(true), Category("Behavior")]
        public bool Draft
        {
            get { return _draft; }
            set
            {
                _draft = value;
                if (_draft)
                {
                    if (_bitmap != null) _bitmap.Dispose();
                    _bitmap = null;
                }
                else
                {
                    if (System.IO.File.Exists(_filename))
                        _bitmap = new Bitmap(_filename);
                }
                OnInvalidate();
            }
        }

        #endregion

        #region ------------------- public methods
        
        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutBitmap()
        {
            ResizeStyle = ResizeStyle.HandledInternally;
            _preserveAspectRatio = true;
            Name = "Bitmap";
            _draft = true;
            _filename = "";
            ResizeStyle = ResizeStyle.StretchToFit;
        }

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">Boolean, true if this is being drawn to a print document</param>
        override public void Draw(Graphics g, bool printing)
        {
            //When printing we use this code
            if (printing)
            {
                Bitmap original = new Bitmap(_filename);
                if (_preserveAspectRatio)
                {
                    if ((Size.Width / original.Width) < (Size.Height / original.Height))
                        g.DrawImage(original, new RectangleF(LocationF.X, LocationF.Y, Size.Width, Size.Width * original.Height / original.Width));
                    else
                        g.DrawImage(original, new RectangleF(LocationF.X, LocationF.Y, Size.Height * original.Width / original.Height, Size.Height));
                }
                else
                    g.DrawImage(original, new RectangleF(LocationF.X, LocationF.Y, Size.Width, Size.Height));
                original.Dispose();
                return;
            }

            if (Resizing == false && Draft)
            {
                if (System.IO.File.Exists(_filename))
                {
                    if ((_bitmap == null) || (_bitmap != null && _bitmap.Width != Convert.ToInt32(Size.Width)))
                    {
                        Bitmap original = new Bitmap(_filename);
                        if (_bitmap != null) _bitmap.Dispose();
                        _bitmap = new Bitmap(Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Width * original.Height / original.Width), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        Graphics graph = Graphics.FromImage(_bitmap);
                        graph.DrawImage(original, 0, 0, _bitmap.Width, _bitmap.Height);
                        original.Dispose();
                        graph.Dispose();
                    }
                }
            }

            if (_bitmap == null) return;
            if (_preserveAspectRatio)
            {
                if ((Size.Width / _bitmap.Width) < (Size.Height / _bitmap.Height))
                    g.DrawImage(_bitmap, new RectangleF(LocationF.X, LocationF.Y, Size.Width, Size.Width * _bitmap.Height / _bitmap.Width));
                else
                    g.DrawImage(_bitmap, new RectangleF(LocationF.X, LocationF.Y, Size.Height * _bitmap.Width / _bitmap.Height, Size.Height));
            }
            else
                g.DrawImage(_bitmap, new RectangleF(LocationF.X, LocationF.Y, Size.Width, Size.Height));
        }

        #endregion
    }
}
