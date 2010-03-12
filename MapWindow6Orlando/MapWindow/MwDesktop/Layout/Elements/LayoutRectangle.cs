//********************************************************************************************************
// Product Name: MapWindow.Layout.LayoutRectangle
// Description:  The MapWindow LayoutRectangle element, holds draws text for the layout
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

using System.Drawing;
using System.ComponentModel;

namespace MapWindow.Layout.Elements
{
    /// <summary>
    /// A control that draws a standard colored rectangle to the print layout
    /// </summary>
    public class LayoutRectangle : LayoutElement
    {
        private Color _color;
        private Color _backColor;
        private float _outlineWidth;

        #region ------------------ Public Properties

        /// <summary>
        /// Gets or sets the font used to draw this text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the color of the text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public Color Color
        {
            get { return _color; }
            set { _color = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the line thickness
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public float OutlineWidth
        {
            get { return _outlineWidth; }
            set { _outlineWidth = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        #endregion

        #region ------------------- public methods

        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutRectangle()
        {
            Name = "Rectangle";
            _color = Color.Black;
            _backColor = Color.Transparent;
            _outlineWidth = 2;
            ResizeStyle = ResizeStyle.HandledInternally;
        }

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">Boolean, true if the drawing is occuring to an actual print document</param>
        override public void Draw(Graphics g, bool printing)
        {
            RectangleF fillRect = Rectangle;
            Pen outlinePen = null;
            RectangleF outline = fillRect;

            //Sets up the rectangles and pen if its needed
            if (_outlineWidth > 0)
            {
                outlinePen = new Pen(_color, _outlineWidth);
                fillRect.Inflate(-outlinePen.Width, -outlinePen.Width);
                outline = Rectangle;
                outline.Inflate(-outlinePen.Width / 2, -outlinePen.Width / 2);
            }

            Brush fillBrush = new SolidBrush(_backColor);
            g.FillRectangle(fillBrush, fillRect);
            fillBrush.Dispose();

            //Draws the outline if its needed
            if (_outlineWidth <= 0) return;
            if (outlinePen == null) return;
            g.DrawRectangle(outlinePen, outline.X, outline.Y, outline.Width, outline.Height);
            outlinePen.Dispose();
        }

        #endregion
    }
}
