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
namespace MapWindow.Analysis.Topology.Noding
{
    /// <summary>
    /// Represents an intersection point between two <see cref="SegmentString" />s.
    /// </summary>
    public class SegmentNode : IComparable
    {        

        /// <summary>
        /// 
        /// </summary>
        public readonly Coordinate Coordinate = null;   // the point of intersection
        
        /// <summary>
        /// 
        /// </summary>
        public readonly int SegmentIndex = 0;   // the index of the containing line segment in the parent edge

        private readonly SegmentString segString = null;
        private readonly Octants segmentOctant = Octants.Null;
        private readonly bool isInterior = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentNode"/> class.
        /// </summary>
        /// <param name="segString"></param>
        /// <param name="coord"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="segmentOctant"></param>
        public SegmentNode(SegmentString segString, Coordinate coord, int segmentIndex, Octants segmentOctant) 
        {
            this.segString = segString;
            this.Coordinate = new Coordinate(coord);
            this.SegmentIndex = segmentIndex;
            this.segmentOctant = segmentOctant;
            isInterior = !coord.Equals2D(segString.GetCoordinate(segmentIndex));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsInterior
        { 
            get
            {
                return isInterior; 
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxSegmentIndex"></param>
        /// <returns></returns>
        public bool IsEndPoint(int maxSegmentIndex)
        {
            if(SegmentIndex == 0 && ! isInterior) 
                return true;
            if(SegmentIndex == maxSegmentIndex) 
                return true;
            return false;
        } 

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>
        /// -1 this SegmentNode is located before the argument location, or
        ///  0 this SegmentNode is at the argument location, or
        ///  1 this SegmentNode is located after the argument location.   
        /// </returns>
        public int CompareTo(object obj)
        {
            SegmentNode other = (SegmentNode) obj;
            if(SegmentIndex < other.SegmentIndex) 
                return -1;
            if(SegmentIndex > other.SegmentIndex) 
                return 1;
            if (Coordinate.Equals2D(other.Coordinate)) 
                return 0;
            return SegmentPointComparator.Compare(segmentOctant, Coordinate, other.Coordinate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outstream"></param>
        public void Write(System.IO.StreamWriter outstream)
        {
            outstream.Write(Coordinate);
            outstream.Write(" seg # = " + SegmentIndex);
        }
    }
}
