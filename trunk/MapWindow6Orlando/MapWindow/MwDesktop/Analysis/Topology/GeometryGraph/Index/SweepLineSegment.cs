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

using System.Collections.Generic;
using MapWindow.Geometries;
namespace MapWindow.GeometriesGraph.Index
{
    /// <summary>
    /// 
    /// </summary>
    public class SweepLineSegment
    {
        private readonly Edge _edge;
        private readonly IList<Coordinate> _pts;
        readonly int _ptIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="ptIndex"></param>
        public SweepLineSegment(Edge edge, int ptIndex)
        {
            _edge = edge;
            _ptIndex = ptIndex;
            _pts = edge.Coordinates;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double MinX
        {
            get
            {
                double x1 = _pts[_ptIndex].X;
                double x2 = _pts[_ptIndex + 1].X;
                return x1 < x2 ? x1 : x2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double MaxX
        {
            get
            {
                double x1 = _pts[_ptIndex].X;
                double x2 = _pts[_ptIndex + 1].X;
                return x1 > x2 ? x1 : x2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="si"></param>
        public virtual void ComputeIntersections(SweepLineSegment ss, SegmentIntersector si)
        {
            si.AddIntersections(_edge, _ptIndex, ss._edge, ss._ptIndex);
        }
    }
}
