//********************************************************************************************************
// Product Name: MapWindow.Tools.ModelElement
// Description:  An abstract class that handles drawing boxes for elements in the modeler window
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
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapWindow.Tools
{
    /// <summary>
    /// Defines the base class for all model components
    /// </summary>
    internal class ModelElement : ICloneable
    {
        #region --------------- class variables

        private Color _color = Color.Wheat;
        private ModelShapes _shape = ModelShapes.Triangle;
        private double _highlight = 1;
        private int _width = 170;
        private int _height = 100;
        private Point _location = new Point(0,0);
        private string _name = "";
        private Font _font = System.Drawing.SystemFonts.MessageBoxFont;
        private List<ModelElement> _modelElements;

        #endregion

        #region --------------- Constructors
        
        /// <summary>
        /// Creates an instance of the model Element
        /// <param name="modelElements">A list of all the elements in the model</param>
        /// </summary>
        public ModelElement(List<ModelElement> modelElements)
        {
            _modelElements = modelElements;
        }

        #endregion

        #region --------------- Methods

        /// <summary>
        /// Returns a shallow copy of the Parameter class
        /// </summary>
        /// <returns>A new Parameters class that is a shallow copy of the original parameters class</returns>
        public ModelElement Copy()
        {
            return MemberwiseClone() as ModelElement;
        }

        /// <summary>
        /// This returns a duplicate of this object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return Copy() as object;
        }

        /// <summary>
        /// Darkens the component slightly
        /// </summary>
        /// <param name="highlighted">Darkens if true returns to normal if false</param>
        public virtual void Highlighted(bool highlighted)
        {
            if (highlighted == true)
                _highlight = 0.85;
            else
                _highlight = 1.0;
        }

        /// <summary>
        /// Repaints the form with cool background and stuff
        /// </summary>
        /// <param name="graph">The graphics object to paint to, the element will be drawn to 0,0</param>
        public virtual void Paint(Graphics graph)
        {
            //Sets up the colors to use
            Pen outlinePen = new Pen(MapWindow.Main.Global.ColorFromHSL(Color.GetHue(), Color.GetSaturation(), Color.GetBrightness() * 0.6 * Highlight), 1.75F);
            Color gradientTop = MapWindow.Main.Global.ColorFromHSL(Color.GetHue(), Color.GetSaturation(), Color.GetBrightness() * 0.7 * Highlight);
            Color gradientBottom = MapWindow.Main.Global.ColorFromHSL(Color.GetHue(), Color.GetSaturation(), Color.GetBrightness() * 1.0 * Highlight);

            //The path used for drop shadows
            System.Drawing.Drawing2D.GraphicsPath shadowPath = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.ColorBlend colorBlend = new System.Drawing.Drawing2D.ColorBlend(3);
            colorBlend.Colors = new Color[] { Color.Transparent, Color.FromArgb(180, Color.DarkGray), Color.FromArgb(180, Color.DimGray) };
            colorBlend.Positions = new float[] { 0f, 0.125f,1f};

            //Draws Rectangular Shapes
            if (Shape == ModelShapes.Rectangle)
            {
                //Draws the shadow
                shadowPath.AddPath(GetRoundedRect(new Rectangle(5, 5, this.Width, this.Height), 10), true);
                System.Drawing.Drawing2D.PathGradientBrush shadowBrush = new System.Drawing.Drawing2D.PathGradientBrush(shadowPath);
                shadowBrush.WrapMode = System.Drawing.Drawing2D.WrapMode.Clamp;
                shadowBrush.InterpolationColors = colorBlend;
                graph.FillPath(shadowBrush, shadowPath);

                //Draws the basic shape
                System.Drawing.Rectangle fillRectange = new Rectangle(0, 0, this.Width - 5, this.Height - 5);
                System.Drawing.Drawing2D.GraphicsPath fillArea = GetRoundedRect(fillRectange, 5);
                System.Drawing.Drawing2D.LinearGradientBrush myBrush = new System.Drawing.Drawing2D.LinearGradientBrush(fillRectange, gradientBottom, gradientTop, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                graph.FillPath(myBrush, fillArea);
                graph.DrawPath(outlinePen, fillArea);
                
                //Draws the status light
                drawStatusLight(graph);
                
                //Draws the text
                SizeF textSize = graph.MeasureString(Name, Font, this.Width);
                RectangleF textRect;
                if ((textSize.Width < this.Width) || (textSize.Height < this.Height))
                    textRect = new RectangleF((this.Width - textSize.Width) / 2, (this.Height - textSize.Height) / 2, textSize.Width, textSize.Height);
                else
                    textRect = new RectangleF(0, (this.Height - textSize.Height) / 2, this.Width, textSize.Height);
                graph.DrawString(Name, Font, new SolidBrush(Color.FromArgb(50, Color.Black)), textRect);
                textRect.X = textRect.X - 1;
                textRect.Y = textRect.Y - 1;
                graph.DrawString(Name, Font, System.Drawing.Brushes.Black, textRect);

                //Garbage collection
                fillArea.Dispose();
                myBrush.Dispose();
            }

            //Draws Ellipse Shapes
            if (_shape == ModelShapes.Ellipse)
            {
                //Draws the shadow
                shadowPath.AddEllipse(0, 5, this.Width+5, this.Height);
                System.Drawing.Drawing2D.PathGradientBrush shadowBrush = new System.Drawing.Drawing2D.PathGradientBrush(shadowPath);
                shadowBrush.WrapMode = System.Drawing.Drawing2D.WrapMode.Clamp;
                shadowBrush.InterpolationColors = colorBlend;
                graph.FillPath(shadowBrush, shadowPath);

                //Draws the Ellipse
                System.Drawing.Rectangle fillArea = new Rectangle(0, 0, this.Width, this.Height);
                System.Drawing.Drawing2D.LinearGradientBrush myBrush = new System.Drawing.Drawing2D.LinearGradientBrush(fillArea, gradientBottom, gradientTop, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                graph.FillEllipse(myBrush, 1, 1, this.Width - 5, this.Height - 5);
                graph.DrawEllipse(outlinePen, 1, 1, this.Width - 5, this.Height - 5);

                //Draws the text
                SizeF textSize = graph.MeasureString(_name, _font, this.Width);
                RectangleF textRect;
                if ((textSize.Width < this.Width) || (textSize.Height < this.Height))
                    textRect = new RectangleF((this.Width - textSize.Width) / 2, (this.Height - textSize.Height) / 2, textSize.Width, textSize.Height);
                else
                    textRect = new RectangleF(0, (this.Height - textSize.Height) / 2, this.Width, textSize.Height);
                graph.DrawString(Name, Font, new SolidBrush(Color.FromArgb(50, Color.Black)), textRect);
                textRect.X = textRect.X - 1;
                textRect.Y = textRect.Y - 1;
                graph.DrawString(Name, Font, System.Drawing.Brushes.Black, textRect);

                //Garbage collection
                myBrush.Dispose();
            }

            //Draws Triangular Shapes
            if (_shape == ModelShapes.Triangle)
            {
                //Draws the shadow
                Point[] ptShadow = new Point[4];
                ptShadow[0] = new Point(5, 5);
                ptShadow[1] = new Point(this.Width + 5, ((this.Height - 5) / 2) + 5);
                ptShadow[2] = new Point(5, this.Height+2);
                ptShadow[3] = new Point(5, 5);
                shadowPath.AddLines(ptShadow);
                System.Drawing.Drawing2D.PathGradientBrush shadowBrush = new System.Drawing.Drawing2D.PathGradientBrush(shadowPath);
                shadowBrush.WrapMode = System.Drawing.Drawing2D.WrapMode.Clamp;
                shadowBrush.InterpolationColors = colorBlend;
                graph.FillPath(shadowBrush, shadowPath);

                //Draws the shape
                Point[] pt = new Point[4];
                pt[0] = new Point(0, 0);
                pt[1] = new Point(this.Width - 5, (this.Height-5) / 2);
                pt[2] = new Point(0, this.Height-5);
                pt[3] = new Point(0, 0);
                System.Drawing.Drawing2D.GraphicsPath myPath = new System.Drawing.Drawing2D.GraphicsPath();
                myPath.AddLines(pt);
                System.Drawing.Rectangle fillArea = new Rectangle(1, 1, this.Width - 5, this.Height - 5);
                System.Drawing.Drawing2D.LinearGradientBrush myBrush = new System.Drawing.Drawing2D.LinearGradientBrush(fillArea, gradientBottom, gradientTop, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                graph.FillPath(myBrush, myPath);
                graph.DrawPath(outlinePen, myPath);

                //Draws the text
                SizeF textSize = graph.MeasureString(Name, Font, this.Width);
                RectangleF textRect;
                if ((textSize.Width < this.Width) || (textSize.Height < this.Height))
                    textRect = new RectangleF((this.Width - textSize.Width) / 2, (this.Height - textSize.Height) / 2, textSize.Width, textSize.Height);
                else
                    textRect = new RectangleF(0, (this.Height - textSize.Height) / 2, this.Width, textSize.Height);
                graph.DrawString(Name, Font, System.Drawing.Brushes.Black, textRect);

                //Garbage collection
                myBrush.Dispose();
            }
            
            //Garbage collection
            shadowPath.Dispose();
            outlinePen.Dispose();
        }

        /// <summary>
        /// This does nothing in the base class but child classes may override it
        /// </summary>
        /// <param name="graph"></param>
        protected virtual void drawStatusLight(Graphics graph)
        {
        }

        /// <summary>
        /// Returns true if the point is in the extents rectangle of the element
        /// </summary>
        /// <param name="pt">A point to test, assuming 0,0 is the top left corner of the shapes drawing rectangle</param>
        /// <returns></returns>
        protected virtual bool pointInExtents(Point pt)
        {
            if ((pt.X > 0 && pt.X < Width) && (pt.Y > 0 && pt.Y < Height))
                return true;
            return false;
        }

        /// <summary>
        /// Calculates if a point is within the shape that defines the element
        /// </summary>
        /// <param name="point">A point to test in the virtual modeling plane</param>
        /// <returns></returns>
        public virtual bool pointInElement(Point point)
        {
            Point pt = new Point(point.X - Location.X, point.Y - Location.Y);

            switch (Shape)
            {
                case ModelShapes.Rectangle:
                    if ((pt.X > 0 && pt.X < Width) && (pt.Y > 0 && pt.Y < Height))
                        return true;
                    break;

                case ModelShapes.Ellipse:
                    double a = Width / 2;
                    double b = Height / 2;
                    double x = pt.X - a;
                    double y = pt.Y - b;
                    if (((x * x) / (a * a)) + ((y * y) / (b * b)) <= 1)
                        return true;
                    break;

                case ModelShapes.Triangle:
                    if ((pt.X >= 0) && (pt.X < Width))
                    {
                        double y1 = (((Height / 2.0) / Width) * pt.X) + 0;
                        double y2 = (-((Height / 2.0) / Width) * pt.X) + Height;
                        if ((pt.Y < y2) && (pt.Y > y1))
                            return true;
                    }
                    break;

                default:
                    return false;
            }
            return false;

        }

        /// <summary>
        /// Returns true if the element intersect the rectangle from the parent class
        /// </summary>
        /// <param name="rect">The rectangle to test must be in the virtual modeling coordinant plane</param>
        /// <returns></returns>
        public virtual bool elementInRectangle(Rectangle rect)
        {
            Geometries.IGeometry rectanglePoly;
            if ((rect.Height == 0) && (rect.Width == 0))
            {
                rectanglePoly = new Geometries.Point(rect.X, rect.Y);
            }
            else if (rect.Width == 0)
            {
                Geometries.Point[] rectanglePoints = new Geometries.Point[2];
                rectanglePoints[0] = new Geometries.Point(rect.X, rect.Y);
                rectanglePoints[1] = new Geometries.Point(rect.X, rect.Y + rect.Height);
                rectanglePoly = new MapWindow.Geometries.LineString(rectanglePoints);
            }
            else if (rect.Height == 0)
            {
                Geometries.Point[] rectanglePoints = new Geometries.Point[2];
                rectanglePoints[0] = new Geometries.Point(rect.X, rect.Y);
                rectanglePoints[1] = new Geometries.Point(rect.X + rect.Width, rect.Y);
                rectanglePoly = new MapWindow.Geometries.LineString(rectanglePoints);
            }
            else
            {
                Geometries.Point[] rectanglePoints = new Geometries.Point[5];
                rectanglePoints[0] = new Geometries.Point(rect.X, rect.Y);
                rectanglePoints[1] = new Geometries.Point(rect.X, rect.Y + rect.Height);
                rectanglePoints[2] = new Geometries.Point(rect.X + rect.Width, rect.Y + rect.Height);
                rectanglePoints[3] = new Geometries.Point(rect.X + rect.Width, rect.Y);
                rectanglePoints[4] = new Geometries.Point(rect.X, rect.Y);
                rectanglePoly = new Geometries.Polygon(new Geometries.LinearRing(rectanglePoints));
            }

            switch (Shape)
            {
                case ModelShapes.Rectangle:
                    return (rect.IntersectsWith(this.Rectangle));

                case ModelShapes.Ellipse:
                    int b = Height / 2;
                    int a = Width / 2;
                    Geometries.Point[] ellipsePoints = new Geometries.Point[(4 * a) + 1];
                    for (int x = -a; x <= a; x++)
                    {
                        if (x == 0)
                        {
                            ellipsePoints[x + a] = new Geometries.Point(Location.X + x + a, Location.Y);
                            ellipsePoints[3 * a - x] = new Geometries.Point(Location.X + x + a, Location.Y + Height);
                        }
                        else
                        {
                            ellipsePoints[x + a] = new Geometries.Point(Location.X + x + a, Location.Y + b - System.Math.Sqrt(System.Math.Abs(((b * b * x * x) / (a * a)) - (b * b))));
                            ellipsePoints[3 * a - x] = new Geometries.Point(Location.X + x + a, Location.Y + b + System.Math.Sqrt(System.Math.Abs(((b * b * x * x) / (a * a)) - (b * b))));
                        }
                    }

                    Geometries.Polygon ellipsePoly = new Geometries.Polygon(new Geometries.LinearRing(ellipsePoints));
                    return (ellipsePoly.Intersects(rectanglePoly));

                case ModelShapes.Triangle:
                    Geometries.Point[] trianglePoints = new Geometries.Point[4];
                    trianglePoints[0] = new Geometries.Point(Location.X, Location.Y);
                    trianglePoints[1] = new Geometries.Point(Location.X, Location.Y + Height);
                    trianglePoints[2] = new Geometries.Point(Location.X + Width - 5, Location.Y + ((this.Height - 5) / 2));
                    trianglePoints[3] = new Geometries.Point(Location.X, Location.Y);
                    Geometries.Polygon trianglePoly = new Geometries.Polygon(new Geometries.LinearRing(trianglePoints));
                    return (trianglePoly.Intersects(rectanglePoly));

                default:
                    return false;
            }

        }
        
        /// <summary>
        /// Returns true if a point is in a rectangle
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="rect"></param>
        protected virtual bool pointInRectangle(Point pt, Rectangle rect)
        {
            if ((pt.X >= rect.X && pt.X <= (rect.X + rect.Width)) && (pt.Y >= rect.Y && pt.Y <= (rect.Y + rect.Height)))
                return true;
            else
                return false;
        }

        /// <summary>
        /// When a double click is caught by the parent class call this method
        /// </summary>
        public virtual bool DoubleClick()
        {
            return true;
        }

        /// <summary>
        /// Creates a rounded corner rectangle from a regular rectangel
        /// </summary>
        /// <param name="baseRect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRect(RectangleF baseRect, float radius)
        {
            if ((radius <= 0.0F) || radius >= ((Math.Min(baseRect.Width, baseRect.Height)) / 2.0))
            {
                System.Drawing.Drawing2D.GraphicsPath mPath = new System.Drawing.Drawing2D.GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }

            float diameter = radius * 2.0F;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            // top left arc 
            path.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Returns true if this model element is downstram of the potentialUpstream element
        /// </summary>
        /// <param name="potentialUpstream"></param>
        /// <returns></returns>
        public bool isDownstreamOf(ModelElement potentialUpstream)
        { return isDownstreamOf(potentialUpstream, this); }

        private bool isDownstreamOf(ModelElement potentialUpstream, ModelElement child)
        {
            foreach (ModelElement mEl in _modelElements)
            {
                ArrowElement mAr = mEl as ArrowElement;
                if (mAr != null)
                {
                    if (mAr.StopElement == null) continue;
                    if (mAr.StopElement == child)
                    {
                        if (mAr.StartElement == null) continue;
                        if (mAr.StartElement == potentialUpstream) return true;
                        else
                        {
                            foreach (ModelElement parents in getParents(mAr.StartElement))
                            {
                                if (isDownstreamOf(potentialUpstream, parents)) return true;
                            }
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a list of all model elements that are direct parents of this element
        /// </summary>
        /// <returns></returns>
        public List<ModelElement> getParents()
        { return getParents(this); }

        private List<ModelElement> getParents(ModelElement child)
        {
            List<ModelElement> listParents = new List<ModelElement>();
            foreach (ModelElement mEl in _modelElements)
            {
                ArrowElement mAr = mEl as ArrowElement;
                if (mAr != null)
                    if (mAr.StopElement != null)
                        if (mAr.StopElement == child) listParents.Add(mAr.StartElement);
            }
            return listParents;
        }

        /// <summary>
        /// Returns true if this model element is downstream of the potentialUpstream element
        /// </summary>
        /// <param name="potentialDownstream"></param>
        /// <returns></returns>
        public bool isUpstreamOf(ModelElement potentialDownstream)
        { return isUpstreamOf(potentialDownstream, this); }

        private bool isUpstreamOf(ModelElement potentialDownstream, ModelElement parent)
        {
            foreach (ModelElement mEl in _modelElements)
            {
                ArrowElement mAr = mEl as ArrowElement;
                if (mAr != null)
                {
                    if (mAr.StartElement == null) continue;
                    if (mAr.StartElement == parent)
                    {
                        if (mAr.StopElement == null) continue;
                        if (mAr.StopElement == potentialDownstream) return true;
                        else
                        {
                            foreach (ModelElement children in getChildren(mAr.StartElement))
                            {
                                if (isUpstreamOf(potentialDownstream, children)) return true;
                            }
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a list of all model elements that are direct children of this element
        /// </summary>
        /// <returns></returns>
        public List<ModelElement> getChildren()
        { return getChildren(this); }

        private List<ModelElement> getChildren(ModelElement parent)
        {
            List<ModelElement> listChildren = new List<ModelElement>();
            foreach (ModelElement mEl in _modelElements)
            {
                ArrowElement mAr = mEl as ArrowElement;
                if (mAr != null)
                    if (mAr.StartElement != null)
                        if (mAr.StartElement == parent) listChildren.Add(mAr.StopElement);
            }
            return listChildren;
        }

        #endregion

        #region --------------- Properties

        /// <summary>
        /// Gets a list of all elements in the model
        /// </summary>
        internal List<ModelElement> ModelElements
        {
            get { return _modelElements; }
            set { _modelElements = value; }
        }

        /// <summary>
        /// Returns 1 if the object is not highlighted less than 1 if it is highlighted
        /// </summary>
        public double Highlight
        {
            get { return _highlight; }
            set { _highlight = value; }
        }

        /// <summary>
        /// Gets or sets the text that is drawn on the element
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the font used to draw the text on the element
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// Gets or Sets the shape of the model component
        /// </summary>
        public ModelShapes Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        /// <summary>
        /// Gets or set the base color of the shapes gradient
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets a rectangle representing the element, top left corner being the location of the parent form of the element
        /// </summary>
        public Rectangle Rectangle
        {
            get { return(new Rectangle(Location.X, Location.Y, Width, Height)); }
        }

        /// <summary>
        /// Gets or sets the width of the element
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the shape of the element
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets the location of the element in the parent form
        /// </summary>
        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }

        #endregion
    }
}
