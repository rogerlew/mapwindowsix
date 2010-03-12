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

using System.Collections;
using System.Collections.Generic;
using MapWindow.Geometries;


namespace MapWindow.GeometriesGraph.Index
{
    /// <summary>
    /// Finds all intersections in one or two sets of edges,
    /// using the straightforward method of
    /// comparing all segments.
    /// This algorithm is too slow for production use, but is useful for testing purposes.
    /// </summary>
    public class SimpleEdgeSetIntersector : EdgeSetIntersector
    {        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="si"></param>
        /// <param name="testAllSegments"></param>
        public override void ComputeIntersections(IList edges, SegmentIntersector si, bool testAllSegments)
        {            
            for (IEnumerator i0 = edges.GetEnumerator(); i0.MoveNext(); ) 
            {
                Edge edge0 = (Edge)i0.Current;
                for (IEnumerator i1 = edges.GetEnumerator(); i1.MoveNext(); ) 
                {
                    Edge edge1 = (Edge)i1.Current;
                    if (testAllSegments || edge0 != edge1)
                        ComputeIntersects(edge0, edge1, si);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges0"></param>
        /// <param name="edges1"></param>
        /// <param name="si"></param>
        public override void ComputeIntersections(IList edges0, IList edges1, SegmentIntersector si)
        {            
            for (IEnumerator i0 = edges0.GetEnumerator(); i0.MoveNext(); )
            {
                Edge edge0 = (Edge)i0.Current;
                for (IEnumerator i1 = edges1.GetEnumerator(); i1.MoveNext(); )
                {
                    Edge edge1 = (Edge)i1.Current;
                        ComputeIntersects(edge0, edge1, si);
                }
            }
        }

        /// <summary>
        /// Performs a brute-force comparison of every segment in each Edge.
        /// This has n^2 performance, and is about 100 times slower than using
        /// monotone chains.
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="e1"></param>
        /// <param name="si"></param>
        private static void ComputeIntersects(Edge e0, Edge e1, SegmentIntersector si)
        {
            IList<Coordinate> pts0 = e0.Coordinates;
            IList<Coordinate> pts1 = e1.Coordinates;
            for (int i0 = 0; i0 < pts0.Count - 1; i0++) 
                for (int i1 = 0; i1 < pts1.Count - 1; i1++)             
                    si.AddIntersections(e0, i0, e1, i1);            
        }
    }
}
