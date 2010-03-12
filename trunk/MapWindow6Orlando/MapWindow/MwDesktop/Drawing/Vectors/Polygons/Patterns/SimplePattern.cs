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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/19/2009 1:15:47 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.ComponentModel;
using System.Drawing;
using MapWindow.Main;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// SimplePattern
    /// </summary>
    public class SimplePattern : Pattern, ISimplePattern
    {
        #region Private Variables

        private Color _fillColor;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SimplePattern
        /// </summary>
        public SimplePattern()
        {
            Color c = Global.RandomLightColor(1F);
            Configure(c);
        }

        /// <summary>
        /// Creates a new SimplePattern with the specified fill color
        /// </summary>
        /// <param name="fillColor">The fill color to use for this simple pattern</param>
        public SimplePattern(Color fillColor)
        {
            Outline = null;
            Configure(fillColor);
        }

        private void Configure(Color fillColor)
        {
            if (Outline != null)
            {
                if (Outline.Strokes.Count > 0)
                {
                    ISimpleStroke ss = Outline.Strokes[0] as ISimpleStroke;
                    if (ss != null) fillColor = ss.Color.Lighter(.5F);
                }
            }
            _fillColor = fillColor;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Gets the fill color
        /// </summary>
        /// <returns></returns>
        public override Color GetFillColor()
        {
            return _fillColor;
        }

        /// <summary>
        /// Sets the fill color
        /// </summary>
        /// <param name="color"></param>
        public override void SetFillColor(Color color)
        {
            _fillColor = color;
        }

        /// <summary>
        /// Fills the path
        /// </summary>
        /// <param name="g">The System.Drawing.Graphics device to draw to</param>
        /// <param name="gp">The System.Drawing.GraphicsPath to fill using this pattern</param>
        public override void FillPath(Graphics g, System.Drawing.Drawing2D.GraphicsPath gp)
        {
            Brush b = new SolidBrush(_fillColor);
            g.FillPath(b, gp);
            b.Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets solid Color used for filling this pattern.
        /// </summary>
        [Description("Gets or sets solid Color used for filling this pattern.")]
		[Serialize("FillColor")]
		public Color FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        /// <summary>
        /// Sets the opacity of this simple pattern by modifying the alpha channel of the fill color.
        /// </summary>
        [Description("Sets the opacity of this simple pattern by modifying the alpha channel of the fill color.")]
		[Serialize("Opacity")]
		public float Opacity
        {
            get
            {
                return _fillColor.GetOpacity();
            }
            set
            {
                _fillColor = _fillColor.ToTransparent(value);
            }
        }

        #endregion

       
    }
}
