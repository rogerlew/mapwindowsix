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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2010 2:03:50 PM
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

namespace MapWindow.Data
{


    /// <summary>
    /// Segment
    /// </summary>
    public class Segment
    {
        #region Private Variables

        /// <summary>
        /// Gets or sets the precision for calculating equality, but this is just a re-direction to Vertex.Epsilon
        /// </summary>
        public static double Epsilon
        {
            get { return Vertex.Epsilon; }
            set { Vertex.Epsilon = value; }
        }

        /// <summary>
        /// The start point of the segment
        /// </summary>
        public Vertex P1;
        /// <summary>
        /// the end point of the segment
        /// </summary>
        public Vertex P2;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a segment from double valued ordinates.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public Segment(double x1, double y1, double x2, double y2)
        {
            P1 = new Vertex(x1, y1);
            P2 = new Vertex(x2, y2);
        }

        /// <summary>
        /// Creates a new instance of Segment
        /// </summary>
        public Segment(Vertex p1, Vertex p2)
        {
            P1 = p1;
            P2 = p2;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the intersection count to detect if there is an intersection
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Intersects(Segment other)
        {
            return (IntersectionCount(other) > 0);
        }

        /// <summary>
        /// Returns 0 if no intersections occur, 1 if an intersection point is found,
        /// and 2 if the segments are colinear and overlap.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int IntersectionCount(Segment other)
        {
            double x1 = P1.X;
            double y1 = P1.Y;
            double x2 = P2.X;
            double y2 = P2.Y;
            double x3 = other.P1.X;
            double y3 = other.P1.Y;
            double x4 = other.P2.X;
            double y4 = other.P2.Y;
            double denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
           
            // if denom is 0, then the two lines are parallel
            double na = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
            double nb = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);
            // if denom is 0 AND na and nb are 0, then the lines are coincident and DO intersect
            if (Math.Abs(denom) < Epsilon && Math.Abs(na) < Epsilon && Math.Abs(nb) < Epsilon) return 2;
            // If denom is 0, but na or nb are not 0, then the lines are parallel and not coincident
            if (denom == 0) return 0;
            double ua = na / denom;
            double ub = nb / denom;
            if (ua < 0 || ua > 1) return 0; // not intersecting with segment a
            if (ub < 0 || ub > 1) return 0; // not intersecting with segment b
            // If we get here, then one intersection exists and it is found on both line segments
            return 1;
        }

        /// <summary>
        /// Tests to see if the specified segment contains the point within Epsilon tollerance.
        /// </summary>
        /// <returns></returns>
        public bool IntersectsVertex(Vertex point)
        {
            double x1 = P1.X;
            double y1 = P1.Y;
            double x2 = P2.X;
            double y2 = P2.Y;
            double pX = point.X;
            double pY = point.Y;
            // COllinear
            if (Math.Abs((x2 - x1) * (pY - y1) - (pX - x1) * (y2 - y1)) > Epsilon) return false;
            // In the x is in bounds and it is colinear, it is on the segment
            if (x1 < x2)
            {
                if (x1 <= pX && pX <= x2) return true;
            }
            else
            {
                if (x2 <= pX && pX <= x1) return true;
            }
            return false;
        }
        



        #endregion



    }
}
