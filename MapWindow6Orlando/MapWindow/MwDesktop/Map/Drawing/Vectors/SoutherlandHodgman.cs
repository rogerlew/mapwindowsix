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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/17/2009 3:33:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
namespace MapWindow.Map
{


    /// <summary>
    /// DoutherlandHodgmanClipper
    /// </summary>
    public static class SoutherlandHodgman
    {
        static Rectangle DrawingBounds = new Rectangle(-32000, -32000, 64000, 64000);
        const int BoundRight = 0;
        const int BoundTop = 1;
        const int BoundLeft = 2;
        const int BoundBottom = 3;
        const int X = 0;
        const int Y = 1;

        /// <summary>
        /// Calculates the Southerland-Hodgman clip using the actual drawing coordinates.
        /// This hopefully will be much faster than NTS which seems unncessarilly slow to calculate.
        /// http://www.codeguru.com/cpp/misc/misc/graphics/article.php/c8965
        /// </summary>
        /// <param name="points"></param>
        /// <returns>A modified list of points that has been clipped to the drawing bounds</returns>
        public static List<System.Drawing.PointF> Clip(List<System.Drawing.PointF> points)
        {
            List<System.Drawing.PointF> result = points;
            for (int direction = 0; direction < 4; direction++)
            {
                result = ClipDirection(result, direction);
            }
            
            return result;
        }

        private static List<System.Drawing.PointF> ClipDirection(List<System.Drawing.PointF> points, int direction)
        {
            bool inside;
            bool previousInside = true;
            List<System.Drawing.PointF> result = new List<System.Drawing.PointF>();
            System.Drawing.PointF previous = System.Drawing.PointF.Empty;
            foreach (System.Drawing.PointF point in points)
            {
                inside = IsInside(point, direction);
                if (previousInside && inside)
                {
                    // both points are inside, so simply add the current point
                    result.Add(point);
                    previous = point;
                }
                if (previousInside && inside == false)
                {
                    if (previous.IsEmpty == false)
                    {
                        // crossing the boundary going out, so insert the intersection instead
                        previous = BoundIntersection(previous, point, direction);
                        result.Add(previous);
                    }
                    else
                    {
                        previous = point;
                    }
                }
                if (previousInside == false && inside)
                {
                    // crossing the boundary going in, so insert the intersection AND the new point
                    result.Add(BoundIntersection(previous, point, direction));
                    result.Add(point);
                    previous = point;
                }
                if (previousInside == false && inside == false)
                {
                    previous = point;
                }
                
                previousInside = inside;
            }
            // be sure to close the polygon if it is not closed
            if (result != null && result.Count > 0)
            {
                if (result[0].X != result[result.Count - 1].X || result[0].Y != result[result.Count - 1].Y)
                {
                    result.Add(new PointF(result[0].X, result[0].Y));
                }
            }
            return result;
        }

        private static bool IsInside(System.Drawing.PointF point, int direction)
        {
            switch (direction)
            {
                case BoundRight:
                    if (point.X <= DrawingBounds.Right) return true;
                    return false;
                case BoundLeft:
                    if (point.X >= DrawingBounds.Left) return true;
                    return false;
                case BoundTop:
                    if (point.Y >= DrawingBounds.Top) return true;
                    return false;
                case BoundBottom:
                    if (point.Y <= DrawingBounds.Bottom) return true;
                    return false;
            }
            return false;
        }

        private static System.Drawing.PointF BoundIntersection(System.Drawing.PointF start, System.Drawing.PointF end, int direction)
        {
            System.Drawing.PointF result = new System.Drawing.PointF();
            switch (direction)
            {
                case BoundRight:
                    result.X = DrawingBounds.Right;
                    result.Y = start.Y + (end.Y - start.Y) * (DrawingBounds.Right - start.X) / (end.X - start.X);
                    break;
                case BoundLeft:
                    result.X = DrawingBounds.Left;
                    result.Y = start.Y + (end.Y - start.Y) * (DrawingBounds.Left - start.X) / (end.X - start.X);
                    break;
                case BoundTop:
                    result.Y = DrawingBounds.Top;
                    result.X = start.X + (end.X - start.X) * (DrawingBounds.Top - start.Y) / (end.Y - start.Y);
                    break;
                case BoundBottom:
                    result.Y = DrawingBounds.Bottom;
                    result.X = start.X + (end.X - start.X) * (DrawingBounds.Bottom - start.Y) / (end.Y - start.Y);
                    break;
            }
            return result;
        }


        /// <summary>
        /// Calculates the Southerland-Hodgman clip using the actual drawing coordinates.
        /// This specific overload works with arrays of doubles instead of System.Drawing.PointF structures.
        /// This hopefully will be much faster than NTS which seems unncessarilly slow to calculate.
        /// http://www.codeguru.com/cpp/misc/misc/graphics/article.php/c8965
        /// </summary>
        /// <param name="vertexValues">The list of arrays of doubles where the X index is 0 and the Y index is 1.</param>
        /// <returns>A modified list of points that has been clipped to the drawing bounds</returns>
        public static List<double[]> Clip(List<double[]> vertexValues)
        {
            List<double[]> result = vertexValues;
            for (int direction = 0; direction < 4; direction++)
            {
                result = ClipDirection(result, direction);
            }

            return result;
        }

        private static List<double[]> ClipDirection(List<double[]> points, int direction)
        {
            bool inside;
            bool previousInside = true;
            List<double[]> result = new List<double[]>();
            double[] previous = new double[2];
            bool isFirst = true;
            foreach (double[] point in points)
            {
                inside = IsInside(point, direction);
                if (previousInside && inside)
                {
                    // both points are inside, so simply add the current point
                    result.Add(point);
                    previous = point;
                }
                if (previousInside && inside == false)
                {
                    if (isFirst == false)
                    {
                        // crossing the boundary going out, so insert the intersection instead
                        previous = BoundIntersection(previous, point, direction);
                        result.Add(previous);
                    }
                    else
                    {
                        previous = point;
                    }
                }
                if (previousInside == false && inside)
                {
                    // crossing the boundary going in, so insert the intersection AND the new point
                    result.Add(BoundIntersection(previous, point, direction));
                    result.Add(point);
                    previous = point;
                }
                if (previousInside == false && inside == false)
                {
                    previous = point;
                }
                isFirst = false;
                previousInside = inside;
            }
            // be sure to close the polygon if it is not closed
            if (result != null && result.Count > 0)
            {
                if (result[0][X] != result[result.Count - 1][X] || result[0][Y] != result[result.Count - 1][Y])
                {
                    result.Add(new double[]{result[0][X], result[0][Y]});
                }
            }
            return result;
        }

        private static bool IsInside(double[] point, int direction)
        {
            switch (direction)
            {
                case BoundRight:
                    if (point[X] <= DrawingBounds.Right) return true;
                    return false;
                case BoundLeft:
                    if (point[X] >= DrawingBounds.Left) return true;
                    return false;
                case BoundTop:
                    if (point[Y] >= DrawingBounds.Top) return true;
                    return false;
                case BoundBottom:
                    if (point[Y] <= DrawingBounds.Bottom) return true;
                    return false;
            }
            return false;
        }

        private static double[] BoundIntersection(double[] start, double[] end, int direction)
        {
            double[] result = new double[2];
            switch (direction)
            {
                case BoundRight:
                    result[X] = DrawingBounds.Right;
                    result[Y] = start[Y] + (end[Y] - start[Y]) * (DrawingBounds.Right - start[X]) / (end[X] - start[X]);
                    break;
                case BoundLeft:
                    result[X] = DrawingBounds.Left;
                    result[Y] = start[Y] + (end[Y] - start[Y]) * (DrawingBounds.Left - start[X]) / (end[X] - start[X]);
                    break;
                case BoundTop:
                    result[Y] = DrawingBounds.Top;
                    result[X] = start[X] + (end[X] - start[X]) * (DrawingBounds.Top - start[Y]) / (end[Y] - start[Y]);
                    break;
                case BoundBottom:
                    result[Y] = DrawingBounds.Bottom;
                    result[X] = start[X] + (end[X] - start[X]) * (DrawingBounds.Bottom - start[Y]) / (end[Y] - start[Y]);
                    break;
            }
            return result;
        }


    }
}
