//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://www.gnu.org/licenses/lgpl.html and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Text;

using MapWindow.Geometries;
using MapWindow.Analysis.Topology.Utilities;

namespace MapWindow.Analysis.Topology.Noding
{
    /// <summary>
    /// Implements a robust method of comparing the relative position of two points along the same segment.
    /// The coordinates are assumed to lie "near" the segment.
    /// This means that this algorithm will only return correct results
    /// if the input coordinates have the same precision and correspond to rounded values
    /// of exact coordinates lying on the segment.
    /// </summary>
    public class SegmentPointComparator
    {

        /// <summary>
        ///  Compares two <see cref="Coordinate" />s for their relative position along a segment
        /// lying in the specified <see cref="Octant" />.
        /// </summary>
        /// <param name="octant"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns>
        /// -1 if node0 occurs first, or
        ///  0 if the two nodes are equal, or
        ///  1 if node1 occurs first.
        /// </returns>
        public static int Compare(Octants octant, Coordinate p0, Coordinate p1)
        {
            // nodes can only be equal if their coordinates are equal
            if (p0.Equals2D(p1)) 
                return 0;

            int xSign = RelativeSign(p0.X, p1.X);
            int ySign = RelativeSign(p0.Y, p1.Y);

            switch (octant)
            {
                case Octants.Zero: 
                    return CompareValue(xSign, ySign);
                case Octants.One:
                    return CompareValue(ySign, xSign);
                case Octants.Two:
                    return CompareValue(ySign, -xSign);
                case Octants.Three:
                    return CompareValue(-xSign, ySign);
                case Octants.Four:
                    return CompareValue(-xSign, -ySign);
                case Octants.Five:
                    return CompareValue(-ySign, -xSign);
                case Octants.Six:
                    return CompareValue(-ySign, xSign);
                case Octants.Seven:
                    return CompareValue(xSign, -ySign);
            }

            Assert.ShouldNeverReachHere("invalid octant value: " + octant);
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="x1"></param>
        /// <returns></returns>
        public static int RelativeSign(double x0, double x1)
        {
            if (x0 < x1) 
                return -1;
            if (x0 > x1) 
                return 1;
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compareSign0"></param>
        /// <param name="compareSign1"></param>
        /// <returns></returns>
        private static int CompareValue(int compareSign0, int compareSign1)
        {
            if (compareSign0 < 0)
                return -1;
            if (compareSign0 > 0) 
                return 1;
            if (compareSign1 < 0)
                return -1;
            if (compareSign1 > 0) 
                return 1;
            return 0;

        }
    }
}
