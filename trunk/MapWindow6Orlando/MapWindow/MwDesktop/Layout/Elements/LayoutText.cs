//********************************************************************************************************
// Product Name: MapWindow.Layout
// Description:  The MapWindow LayoutText element, holds draws text for the layout
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

//********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Text;
using System.ComponentModel;

namespace MapWindow.Layout.Elements
{
    /// <summary>
    /// Controls a rectangle 
    /// </summary>
    public class LayoutText : LayoutElement 
    {
        private String _text;
        private Font _font;
        private Color _color;
        private TextRenderingHint _textHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
        private ContentAlignment _contentAlignment;

        #region ------------------ Public Properties

        /// <summary>
        /// Gets or sets the text thats drawn in the graphics object
        /// </summary>
        [Browsable(true),Editor(typeof(System.ComponentModel.Design.MultilineStringEditor),typeof(System.Drawing.Design.UITypeEditor)),Category("Symbol")]
        public String Text
        {
            get { return _text; }
            set { _text = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the content alignment
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public ContentAlignment ContentAlignment
        {
            get { return _contentAlignment; }
            set { _contentAlignment = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the font used to draw this text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public Font Font
        {
            get { return _font; }
            set { _font = value; base.UpdateThumbnail(); base.OnInvalidate(); }
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
        /// Gets or sets the hinting used to draw the text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public TextRenderingHint TextHint
        {
            get { return _textHint; }
            set { _textHint = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        #endregion

        #region ------------------- public methods
        
        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutText()
        {
            Name = "Text Box";
            _font = new Font("Arial", 10);
            _color = Color.Black;
            _text = "Text Box";
            _textHint = TextRenderingHint.AntiAliasGridFit;
            ResizeStyle = ResizeStyle.HandledInternally;
            _contentAlignment = ContentAlignment.TopLeft;
        }

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">Boolean, true if printing to the file</param>
        override public void Draw(Graphics g, bool printing)
        {
            g.TextRenderingHint = _textHint;
            Brush colorBrush = new SolidBrush(_color);
            StringFormat sf = StringFormat.GenericDefault;
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            
            switch (_contentAlignment)
            {
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
            }
            g.DrawString(_text, _font, colorBrush, Rectangle, sf);

            sf.Dispose();
            colorBrush.Dispose();
        }

        #endregion
    }
}
