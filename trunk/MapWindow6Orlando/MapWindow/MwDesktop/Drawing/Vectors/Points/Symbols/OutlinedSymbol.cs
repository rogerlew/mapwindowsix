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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/13/2009 3:01:35 PM
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
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// OutlinedSymbol
    /// </summary>
    public class OutlinedSymbol : Symbol, IOutlinedSymbol
    {
        #region Private Variables

        private bool _useOutline;
        private Color _outlineColor;
        private double _outlineWidth;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of OutlinedSymbol
        /// </summary>
        public OutlinedSymbol()
        {
            _useOutline = false;
            _outlineWidth = 1;
            _outlineColor = Color.Black;
        }

        #endregion

        #region Methods

       

        /// <summary>
        /// Copies only the use outline, outline width and outline color properties from the specified symbol.
        /// </summary>
        /// <param name="symbol">The symbol to copy from.</param>
        public void CopyOutline(IOutlinedSymbol symbol)
        {
            _outlineColor = symbol.OutlineColor;
            _outlineWidth = symbol.OutlineWidth;
            _useOutline = symbol.UseOutline;
        }
      
        /// <summary>
        /// Handles the drawing code, extending it to include some outline content.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleSize"></param>
        protected override void OnDraw(Graphics g, double scaleSize)
        {
            base.OnDraw(g, scaleSize);
            float dx = (float)(Size.Width * scaleSize / 2);
            float dy = (float)(Size.Height * scaleSize / 2);
            GraphicsPath gp = new GraphicsPath();
            gp.AddRectangle(new RectangleF(-dx, -dy, 2F * dx, 2f * dy));
            OnDrawOutline(g, scaleSize, gp);
            gp.Dispose();
            
        }

        /// <summary>
        /// Actually handles the rendering of the outline itself.  
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleSize"></param>
        /// <param name="gp"></param>
        protected virtual void OnDrawOutline(Graphics g, double scaleSize, GraphicsPath gp)
        {
            if (_useOutline == false) return;
            if (_outlineWidth == 0) return;
            Pen p = new Pen(_outlineColor);
            p.Width = (float)(scaleSize * _outlineWidth);
            p.Alignment = PenAlignment.Outset;
            g.DrawPath(p, gp);
            p.Dispose();
        }
         

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the outline color that surrounds this specific symbol.
        /// (this will have the same shape as the symbol, but be larger.
        /// </summary>
        [XmlIgnore]
        public Color OutlineColor
        {
            get { return _outlineColor; }
            set { _outlineColor = value; }
        }
        
        /// <summary>
        /// Provided for XML serialization
        /// </summary>
        [XmlElement("OutlineColor"), Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Serialize("XmlOutlineColor")]
		public string XmlOutlineColor
        {
            get { return ColorTranslator.ToHtml(_outlineColor); }
            set { _outlineColor = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// This redefines the Alpha channel of the color to a floating point opacity
        /// that ranges from 0 to 1.
        /// </summary>
		[Serialize("OutlineOpacity")]
		public float OutlineOpacity
        {
            get { return (float)_outlineColor.A / 255F; }
            set
            {
                int alpha = (int)(value * 255);
                if (alpha > 255) alpha = 255;
                if (alpha < 0) alpha = 0;
                if (alpha != (int)_outlineColor.A)
                {
                    _outlineColor = Color.FromArgb(alpha, _outlineColor);
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the outline beyond the size of this symbol.
        /// </summary>
		[Serialize("OutlineWidth")]
		public double OutlineWidth
        {
            get { return _outlineWidth; }
            set { _outlineWidth = value; }
        }

        /// <summary>
        /// Gets or sets the boolean outline
        /// </summary>
		[Serialize("UseOutline")]
		public bool UseOutline
        {
            get { return _useOutline; }
            set { _useOutline = value; }
        }
      


        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs during the randomize process and allows future overriding of the process for sub-classes
        /// </summary>
        /// <param name="generator"></param>
        protected override void OnRandomize(Random generator)
        {
            // randomize properties of the base class & any properties that are types that implement IRandomizable
            base.OnRandomize(generator);
            
            // randomize whatever is left
            _useOutline = (generator.Next(0, 1) == 1);
            _outlineColor = Global.RandomColor();
            _outlineWidth = generator.Next();
        }

        #endregion



    }
}
