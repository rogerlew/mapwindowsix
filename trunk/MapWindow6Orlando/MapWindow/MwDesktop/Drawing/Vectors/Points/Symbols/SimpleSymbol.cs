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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 3:01:03 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using MapWindow.Main;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// SimpleSymbol
    /// </summary>
    public class SimpleSymbol : OutlinedSymbol, ISimpleSymbol
    {
        #region Private Variables

        private Color _color;
        private PointShapes _pointShape;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SimpleSymbol
        /// </summary>
        public SimpleSymbol()
        {
            Configure();
        }

        /// <summary>
        /// Creates a point symbol with the specified color
        /// </summary>
        /// <param name="color"></param>
        public SimpleSymbol(Color color)
        {
            Configure();
            _color = color;
        }

        /// <summary>
        /// Creates a point symbol with the specified color and shape
        /// </summary>
        /// <param name="color">Creates a point symbol with the specified color and shape</param>
        /// <param name="shape">Creates a point Symbol with the specified shape</param>
        public SimpleSymbol(Color color, PointShapes shape)
        {
            Configure();
            _color = color;
            _pointShape = shape;
        }

        /// <summary>
        /// Creates a SimpleSymbol with the specified color, shape and size.  The size is used for 
        /// both the horizontal and vertical directions.
        /// </summary>
        /// <param name="color">The color of the symbol</param>
        /// <param name="shape">The shape of the symbol</param>
        /// <param name="size">The size of the symbol</param>
        public SimpleSymbol(Color color, PointShapes shape, double size)
        {
            Configure();
            _color = color;
            _pointShape = shape;
            Size = new Size2D(size, size);
        }

        private void Configure()
        {
            base.SymbolType = SymbolTypes.Simple;
            _color = Global.RandomColor();
            _pointShape = PointShapes.Rectangle;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Gets the font color of this symbol to represent the color of this symbol
        /// </summary>
        /// <returns>The color of this symbol as a font</returns>
        public override Color GetColor()
        {
            return _color;
        }
        /// <summary>
        /// Sets the fill color of this symbol to the specified color
        /// </summary>
        /// <param name="color">The System.Drawing.Color</param>
        public override void SetColor(Color color)
        {
            _color = color;
        }

        /// <summary>
        /// Handles the specific drawing for this symbol
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleSize"></param>
        protected override void OnDraw(Graphics g, double scaleSize)
        {
            if (scaleSize == 0) return;
            if (Size.Width == 0 || Size.Height == 0) return;
            SizeF size = new SizeF((float)(scaleSize * Size.Width), (float)(scaleSize * Size.Height));
            Brush fillBrush = new SolidBrush(Color);
            float w = (float)(OutlineWidth * scaleSize);
            Pen scalePen = new Pen(OutlineColor, w);
            scalePen.Alignment = PenAlignment.Outset;
            GraphicsPath gp = new GraphicsPath();
            switch (PointShape)
            {
                case PointShapes.Diamond:
                    AddRegularPoly(gp, size, 4);
                    break;
                case PointShapes.Ellipse:
                    AddEllipse(gp, size);
                    break;
                case PointShapes.Hexagon:
                    AddRegularPoly(gp, size, 6);
                    break;
                case PointShapes.Pentagon:
                    AddRegularPoly(gp, size, 5);
                    break;
                case PointShapes.Rectangle:
                    gp.AddRectangle(new RectangleF(-size.Width/2, -size.Height/2, size.Width, size.Height));
                    break;
                case PointShapes.Star:
                    AddStar(gp, size);
                    break;
                case PointShapes.Triangle:
                    AddRegularPoly(gp, size, 3);
                    break;
            }
            g.FillPath(fillBrush, gp);
            OnDrawOutline(g, scaleSize, gp);
            gp.Dispose();
           
        }

        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Color
        /// </summary>
        [XmlIgnore]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        /// <summary>
        /// Only provided because XML Serialization doesn't work for colors
        /// </summary>
        [XmlElement("Color"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Serialize("XmlColor"), ShallowCopy]
		public string XmlColor
        {
            get { return ColorTranslator.ToHtml(_color); }
            set { _color = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// Gets or sets the opacity as a floating point value ranging from 0 to 1, where
        /// 0 is fully transparent and 1 is fully opaque.  This actually adjusts the alpha of the color value.
        /// </summary>
		[Serialize("Opacity"), ShallowCopy]
		public float Opacity
        {
            get { return _color.A / 255F; }
            set 
            {
                int alpha = (int)(value * 255);
                if (alpha > 255) alpha = 255;
                if (alpha < 0) alpha = 0;
                if (alpha != _color.A)
                {
                    _color = Color.FromArgb(alpha, _color);
                }

            }
        }

        
        /// <summary>
        /// Gets or sets the PointTypes enumeration that describes how to draw the simple symbol.
        /// </summary>
		[Serialize("PointShapes")]
		public PointShapes PointShape
        {
            get { return _pointShape; }
            set { _pointShape = value; }
        }

       

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs during the randomizing process
        /// </summary>
        /// <param name="generator"></param>
        protected override void OnRandomize(Random generator)
        {
            _color = generator.NextColor();
            Opacity = generator.NextFloat();
            _pointShape = generator.NextEnum<PointShapes>();

            base.OnRandomize(generator);
        }

        #endregion

        #region Private Methods




        /// <summary>
        /// Draws a 5 pointed star with the points having twice the radius as the bends.
        /// </summary>
        /// <param name="gp">The System.Drawing.Drawing2D.GraphicsPath to add the start to</param>
        /// <param name="ScaledSize">The System.Drawing.SizeF size to fit the Start to</param>
        public static void AddStar(GraphicsPath gp, SizeF ScaledSize)
        {

            PointF[] PolyPoints = new PointF[11];
            for (int I = 0; I <= 10; I++)
            {
                double ang = I * Math.PI / 5;

                float x = Convert.ToSingle(Math.Cos(ang)) * ScaledSize.Width / 2f;
                float y = Convert.ToSingle(Math.Sin(ang)) * ScaledSize.Height / 2f;
                if (I % 2 == 0)
                {
                    x /= 2; // the shorter radius points of the star
                    y /= 2;
                }
                PolyPoints[I] = new PointF(x, y);
            }
            gp.AddPolygon(PolyPoints);
        }

        /// <summary>
        /// Draws a 5 pointed star with the points having twice the radius as the bends.
        /// </summary>
        /// <param name="g">The System.Drawing.Graphics surface to draw on</param>
        /// <param name="ScaledBorderPen">The System.Drawing.Pen to draw the border with</param>
        /// <param name="fillBrush">The System.Drawing.Brush to use to fill the Star</param>
        /// <param name="ScaledSize">The System.Drawing.SizeF size to fit the Start to</param>
        public static void DrawStar(Graphics g, Pen ScaledBorderPen, Brush fillBrush, SizeF ScaledSize)
        {

            PointF[] PolyPoints = new PointF[11];
            for (int I = 0; I <= 10; I++)
            {
                double ang = I * Math.PI / 5;

                float x = Convert.ToSingle(Math.Cos(ang)) * ScaledSize.Width / 2f;
                float y = Convert.ToSingle(Math.Sin(ang)) * ScaledSize.Height / 2f;
                if (I % 2 == 0)
                {
                    x /= 2; // the shorter radius points of the star
                    y /= 2;
                }
                PolyPoints[I] = new PointF(x, y);
            }
            if (fillBrush != null)
            {
                g.FillPolygon(fillBrush, PolyPoints, FillMode.Alternate);
            }
            if (ScaledBorderPen != null)
            {
                g.DrawPolygon(ScaledBorderPen, PolyPoints);
            }

        }

        /// <summary>
        /// Draws an ellipse on the specified graphics surface.
        /// </summary>
        /// <param name="gp">The System.Drawing.Drawing2D.GraphicsPath to add this shape to</param>
        /// <param name="ScaledSize">The size to fit the ellipse into (the ellipse will be centered at 0,0)</param>
        public static void AddEllipse(GraphicsPath gp, SizeF ScaledSize)
        {
            PointF UpperLeft = new PointF(-ScaledSize.Width / 2, -ScaledSize.Height / 2);
            RectangleF destRect = new RectangleF(UpperLeft, ScaledSize);
            gp.AddEllipse(destRect);
        }

        /// <summary>
        /// Draws an ellipse on the specified graphics surface.
        /// </summary>
        /// <param name="g">The graphics surface to draw on</param>
        /// <param name="ScaledBorderPen">The Pen to use for the border, or null if no border should be drawn</param>
        /// <param name="fillBrush">The Brush to use for the fill, or null if no fill should be drawn</param>
        /// <param name="ScaledSize">The size to fit the ellipse into (the ellipse will be centered at 0,0)</param>
        public static void DrawEllipse(Graphics g, Pen ScaledBorderPen, Brush fillBrush, SizeF ScaledSize)
        {
            PointF UpperLeft = new PointF(-ScaledSize.Width / 2, -ScaledSize.Height / 2);
            RectangleF destRect = new RectangleF(UpperLeft, ScaledSize);
            if (fillBrush != null)
            {
                g.FillEllipse(fillBrush, destRect);
            }
            if (ScaledBorderPen != null)
            {
                g.DrawEllipse(ScaledBorderPen, destRect);
            }
        }

        /// <summary>
        /// Draws a regular polygon with equal sides.  The first point will be located all the way to the right on the X axis.
        /// </summary>
        /// <param name="gp">Specifies the System.Drawing.Drawing2D.GraphicsPath surface to draw on</param>
        /// <param name="ScaledSize">Specifies the System.Drawing.SizeF to fit the polygon into</param>
        /// <param name="NumSides">Specifies the integer number of sides that the polygon should have</param>
        public static void AddRegularPoly(GraphicsPath gp, SizeF ScaledSize, int NumSides)
        {
            PointF[] PolyPoints = new PointF[NumSides + 1];

            // Instead of figuring out the points in cartesian, figure them out in angles and re-convert them.
            for (int I = 0; I <= NumSides; I++)
            {
                double ang = I * (2 * Math.PI) / NumSides;
                float x = Convert.ToSingle(Math.Cos(ang)) * ScaledSize.Width / 2f;
                float y = Convert.ToSingle(Math.Sin(ang)) * ScaledSize.Height / 2f;
                PolyPoints[I] = new PointF(x, y);
            }
            gp.AddPolygon(PolyPoints);
        }

        /// <summary>
        /// Draws a regular polygon with equal sides.  The first point will be located all the way to the right on the X axis.
        /// </summary>
        /// <param name="g">Specifies the System.Drawing.Graphics surface to draw on</param>
        /// <param name="ScaledBorderPen">Specifies the System.Drawing.Pen to use for the border</param>
        /// <param name="fillBrush">Specifies the System.Drawing.Brush to use for to fill the shape</param>
        /// <param name="ScaledSize">Specifies the System.Drawing.SizeF to fit the polygon into</param>
        /// <param name="NumSides">Specifies the integer number of sides that the polygon should have</param>
        public static void DrawRegularPoly(Graphics g, Pen ScaledBorderPen, Brush fillBrush, SizeF ScaledSize, int NumSides)
        {
            PointF[] PolyPoints = new PointF[NumSides + 1];

            // Instead of figuring out the points in cartesian, figure them out in angles and re-convert them.
            for (int I = 0; I <= NumSides; I++)
            {
                double ang = I * (2 * Math.PI) / NumSides;
                float x = Convert.ToSingle(Math.Cos(ang)) * ScaledSize.Width / 2f;
                float y = Convert.ToSingle(Math.Sin(ang)) * ScaledSize.Height / 2f;
                PolyPoints[I] = new PointF(x, y);
            }
            if (fillBrush != null)
            {
                g.FillPolygon(fillBrush, PolyPoints, FillMode.Alternate);
            }
            if (ScaledBorderPen != null)
            {
                g.DrawPolygon(ScaledBorderPen, PolyPoints);
            }

        }


        #endregion


       
    }
}
