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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2008 3:30:45 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;

namespace MapWindow.Drawing
{


    /// <summary>
    /// Opp
    /// </summary>
    public static class Opp
    {


        /// <summary>
        /// Calculates a new rectangle by using the input points to define the far corners.
        /// The position of the points doesn't matter.
        /// </summary>
        /// <param name="pt1">One of the points to form a rectangle with.</param>
        /// <param name="pt2">The second point to use when drawing a rectangle.</param>
        /// <returns>A System.Drawing.Rectangle created from the points.</returns>
        public static Rectangle RectangleFromPoints(System.Drawing.Point pt1, System.Drawing.Point pt2)
        {
            int X = Math.Min(pt1.X, pt2.X);
            int Y = Math.Min(pt1.Y, pt2.Y);
            int W = Math.Max(pt1.X, pt2.X) - X;
            int H = Math.Max(pt1.Y, pt2.Y) - Y;
            return new Rectangle(X, Y, W, H);
        }
     


    }
}
