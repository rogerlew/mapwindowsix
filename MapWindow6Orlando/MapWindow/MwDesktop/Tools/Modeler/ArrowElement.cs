//********************************************************************************************************
// Product Name: MapWindow.Tools.ArrowElement
// Description:  An abstract class that handles drawing arrows between elements in the modeler window
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
    internal class ArrowElement : ModelElement
    {
        ModelElement _startElement;
        ModelElement _stopElement;

        Point _startPoint;
        Point _stopPoint;

        System.Drawing.Drawing2D.GraphicsPath _arrowPath = new System.Drawing.Drawing2D.GraphicsPath();

        /// <summary>
        /// Creates an instance of an ArrowElement
        /// </summary>
        /// <param name="sourceElement">The element the arrow starts at</param>
        /// <param name="destElement">the element the arrow ends at</param>
        /// <param name="modelElements">A list of all the elements in the model</param>
        public ArrowElement(ModelElement sourceElement, ModelElement destElement, List<ModelElement> modelElements) : base(modelElements)
        {

            _startElement = sourceElement;
            _stopElement = destElement;
            updateDimentions();
            Shape = ModelShapes.Arrow;
            Location = _startElement.Location;
        }

        public void updateDimentions()
        {
            //Updates the location and size of the element based on the elements its attached to
            Location = _startElement.Location;
            Width = _stopElement.Location.X - _startElement.Location.X;
            Height = _stopElement.Location.Y - _startElement.Location.Y;
            _startPoint = new Point(_startElement.Width - 4, (_startElement.Height / 2));
            _stopPoint = new Point(Width, Height + (StopElement.Height / 2));
        }

        /// <summary>
        /// Repaints the form with cool background and stuff
        /// </summary>
        /// <param name="graph">The graphics object to paint to, the element will be drawn to 0,0</param>
        override public void Paint(Graphics graph)
        {
            //Draws Rectangular Shapes
            if (Shape == ModelShapes.Arrow)
            {
                _arrowPath = new System.Drawing.Drawing2D.GraphicsPath();

                //Draws the basic shape
                Pen arrowPen;
                if (Highlight < 1)
                    arrowPen = new Pen(Color.Cyan, 3F);
                else
                    arrowPen = new Pen(Color.Black, 3F);

                //Draws the curved arrow
                Point[] lineArray = new Point[4];
                lineArray[0] = new Point(_startPoint.X, _startPoint.Y);
                lineArray[1] = new Point(_startPoint.X - ((_startPoint.X - _stopPoint.X) / 3), _startPoint.Y);
                lineArray[2] = new Point(_stopPoint.X - ((_stopPoint.X - _startPoint.X) / 3), _stopPoint.Y);
                lineArray[3] = new Point(_stopPoint.X, _stopPoint.Y);
                graph.DrawBeziers(arrowPen, lineArray);
                _arrowPath.AddBeziers(lineArray);
                _arrowPath.Flatten();
 
                //Draws the arrow head
                Point[] arrowArray = new Point[3];
                arrowArray[0] = _stopPoint;
                arrowArray[1] = new Point(_stopPoint.X - (5 * Math.Sign(_stopPoint.X - _startPoint.X)),_stopPoint.Y - 2);
                arrowArray[2] = new Point(_stopPoint.X - (5 * Math.Sign(_stopPoint.X - _startPoint.X)), _stopPoint.Y + 2);
                graph.DrawPolygon(arrowPen,arrowArray);

                //Garbage collection
                arrowPen.Dispose();
            }
        }

        /// <summary>
        /// Calculates if a point is within the shape that defines the element
        /// </summary>
        /// <param name="point">A point to test in the virtual modeling plane</param>
        /// <returns></returns>
        public override bool pointInElement(Point point)
        {
            Rectangle temp = new Rectangle(point, new Size(1,1));
            temp.Inflate(2, 2);
            return elementInRectangle(temp);
        }

        /// <summary>
        /// Returns true if the element intersect the rectangle from the parent class
        /// </summary>
        /// <param name="rect">The rectangle to test must be in the virtual modeling coordinant plane</param>
        /// <returns></returns>
        public override bool elementInRectangle(Rectangle rect)
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

            if (Shape == ModelShapes.Arrow)
            {
                Geometries.Point[] arrowPoints = new Geometries.Point[_arrowPath.PointCount];
                for(int i = 0; i < _arrowPath.PointCount; i++)
                {
                    arrowPoints[i] = new Geometries.Point(_arrowPath.PathPoints[i].X + this.Location.X,_arrowPath.PathPoints[i].Y + this.Location.Y);
                }
                Geometries.LineString arrowLine = new MapWindow.Geometries.LineString(arrowPoints);
                return (arrowLine.Intersects(rectanglePoly));
            }
            return false;
        }

        #region -------------------- Properties

        /// <summary>
        /// Gets or sets the destination Element
        /// </summary>
        public ModelElement StopElement
        {
            get { return _stopElement; }
            set { _stopElement = value; }
        }

        /// <summary>
        /// Gets or sets the source element
        /// </summary>
        public ModelElement StartElement
        {
            get { return _startElement; }
            set { _startElement = value; }
        }
        
        /// <summary>
        /// the point in the arrows coordinants we draw from 
        /// </summary>
        public Point StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        /// <summary>
        /// the point in the arrows coordinants we stop drawing from 
        /// </summary>
        public Point StopPoint
        {
            get { return _stopPoint; }
            set { _stopPoint = value; }
        }

        #endregion
    }
}
