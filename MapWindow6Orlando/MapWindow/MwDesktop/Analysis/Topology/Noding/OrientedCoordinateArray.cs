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
using System.Collections.Generic;
using MapWindow.Geometries;
namespace MapWindow.Analysis.Topology.Noding
{

    /// <summary>
    ///  Allows comparing <see cref="Coordinate" /> arrays in an orientation-independent way.
    /// </summary>
    public class OrientedCoordinateArray : IComparable
    {

        private readonly IList<Coordinate> _pts;
        private readonly bool _orientation;

        /// <summary>
        /// Creates a new <see cref="OrientedCoordinateArray" />}
        /// for the given <see cref="Coordinate" /> array.
        /// </summary>
        /// <param name="pts"></param>
        public OrientedCoordinateArray(IList<Coordinate> pts)
        {
            _pts = pts;
            _orientation = Orientation(pts);
        }

        /// <summary>
        /// Computes the canonical orientation for a coordinate array.
        /// </summary>
        /// <param name="pts"></param>
        /// <returns>
        /// <c>true</c> if the points are oriented forwards, or
        /// <c>false</c>if the points are oriented in reverse.
        /// </returns>
        private static bool Orientation(IList<Coordinate> pts)
        {
            return CoordinateArrays.IncreasingDirection(pts) == 1;
        }

        /// <summary>
        /// Compares two <see cref="OrientedCoordinateArray" />s for their relative order.
        /// </summary>
        /// <param name="o1"></param>
        /// <returns>
        /// -1 this one is smaller, or
        ///  0 the two objects are equal, or
        ///  1 this one is greater.
        /// </returns>
        public int CompareTo(object o1)
        {
            OrientedCoordinateArray oca = (OrientedCoordinateArray)o1;
            return CompareOriented(_pts, _orientation, oca._pts, oca._orientation);            
        }

        private static int CompareOriented(IList<Coordinate> pts1, bool orientation1, IList<Coordinate> pts2, bool orientation2)
        {
            int dir1 = orientation1 ? 1 : -1;
            int dir2 = orientation2 ? 1 : -1;
            int limit1 = orientation1 ? pts1.Count : -1;
            int limit2 = orientation2 ? pts2.Count : -1;

            int i1 = orientation1 ? 0 : pts1.Count - 1;
            int i2 = orientation2 ? 0 : pts2.Count - 1;            
            while(true)
            {
                int compPt = pts1[i1].CompareTo(pts2[i2]);
                if (compPt != 0)
                    return compPt;

                i1 += dir1;
                i2 += dir2;
                bool done1 = i1 == limit1;
                bool done2 = i2 == limit2;
                if(done1 && !done2) 
                    return -1;
                if(!done1 && done2) 
                    return 1;
                if(done1) 
                    return 0;
            }
        }
    }
}
