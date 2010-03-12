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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 3:29:12 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using MapWindow.Main;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// SimpleStroke
    /// </summary>
    [Serializable,
    XmlRoot("SimpleStroke")]
    public class SimpleStroke: Stroke, ISimpleStroke
    {
        #region Private Variables

        private Color _color;
        private double _width;
        private DashStyle _dashStyle;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SimpleStroke
        /// </summary>
        public SimpleStroke()
        {
            _color = Global.RandomDarkColor(1);
            _width = 1;
        }

        /// <summary>
        /// Sets the width of the line as part of the constructor
        /// </summary>
        /// <param name="width">The double width of the line to set</param>
        public SimpleStroke(double width)
        {
            Width = width;
        }

        /// <summary>
        /// Creates a new simple stroke with the specified color
        /// </summary>
        /// <param name="color">The color to use for the stroke</param>
        public SimpleStroke(Color color)
        {
            Color = color;
            _width = 1;
        }

        /// <summary>
        /// Creates a new simple stroke with the specified width and color
        /// </summary>
        /// <param name="width"></param>
        /// <param name="color"></param>
        public SimpleStroke(double width, Color color)
        {
            Width = width;
            Color = color;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a color to represent this line.  If the stroke doesn't work as a color,
        /// then this color will be gray.
        /// </summary>
        /// <returns></returns>
        public override Color GetColor()
        {
            return _color;
        }

        /// <summary>
        /// Sets the color of this stroke to the specified color if possible.
        /// </summary>
        /// <param name="color">The color to assign to this color.</param>
        public override void SetColor(Color color)
        {
            _color = color;
        }

        /// <summary>
        /// Creates a system drawing pen that has all of the symbolic information
        /// necessary for this stroke.
        /// </summary>
        /// <param name="width">The base width.  In symbolic drawing this is 1, but in 
        /// geographic drawing, this will be a number representing the result of a
        /// transform from projToPixel width.</param>
        /// <returns></returns>
        public override Pen ToPen(double width)
        {
            float w = (float)(width * Width);
            Pen result = new Pen(Color, w);
            result.DashStyle = _dashStyle;
            if (_dashStyle == DashStyle.Custom)
            {
                result.DashPattern = new[] { 1F };
            }
            result.LineJoin = LineJoin.Round;
            result.StartCap = LineCap.Round;
            result.EndCap = LineCap.Round;
            return result;
        }

      
        



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color for this drawing layer
        /// </summary>
		[Serialize("Color")]
		public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the dash style
        /// </summary>
		[Serialize("DashStyle")]
		public DashStyle DashStyle
        {
            get { return _dashStyle; }
            set { _dashStyle = value; }
        }

        /// <summary>
        /// gets or sets the opacity of the color.  1 is fully opaque while 0 is fully transparent.
        /// </summary>
		[Serialize("Opacity")]
		public float Opacity
        {
            get { return _color.A/255F; }
            set 
            {
                float val = value;
                if (val > 1) val = 1F;
                if (val < 0) val = 0F;
                _color = Color.FromArgb((int)(val * 255), _color.R, Color.G, Color.B);
            }
        }

        /// <summary>
        /// Gets or sets the width of this line relative to the 
        /// width passed in.
        /// </summary>
		[Serialize("Width")]
		public double Width
        {
            get { return _width; }
            set { _width = value; }
        }


      

        #endregion

        #region Protected Methods

        ///// <summary>
        ///// Adds SimpleStroke copy content
        ///// </summary>
        ///// <param name="copy"></param>
        //protected override void OnCopy(Descriptor copy)
        //{
        //    base.OnCopy(copy);
        //    ISimpleStroke ss = copy as ISimpleStroke;
        //    ss.Color = Color;
        //    ss.Width = Width;
        //    ss.DashStyle = this.DashStyle;
        //}

        /// <summary>
        /// Handles randomization of simple stroke content
        /// </summary>
        /// <param name="generator">The random generator to use for randomizing characteristics.</param>
        protected override void OnRandomize(Random generator)
        {
            _color = generator.NextColor();
            Opacity = generator.NextFloat();
            _width = generator.NextFloat(10);
            _dashStyle = generator.NextEnum<DashStyle>();
            base.OnRandomize(generator);
        }


        #endregion

    }
}
