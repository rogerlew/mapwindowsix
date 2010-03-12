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
using MapWindow.GeometriesGraph;
using MapWindow.GeometriesGraph.Index;
using MapWindow.Analysis.Topology.Operation;
using MapWindow.Analysis.Topology.Algorithm;
using MapWindow.Analysis.Topology.Operation.Relate;

namespace MapWindow.Analysis.Topology.Operation.Valid
{
    /// <summary> 
    /// Checks that a {GeometryGraph} representing an area
    /// (a <c>Polygon</c> or <c>MultiPolygon</c> )
    /// is consistent with the SFS semantics for area geometries.
    /// Checks include:
    /// Testing for rings which self-intersect (both properly and at nodes).
    /// Testing for duplicate rings.
    /// If an inconsistency if found the location of the problem is recorded.
    /// </summary>
    public class ConsistentAreaTester 
    {
        private readonly LineIntersector li = new RobustLineIntersector();
        private GeometryGraph geomGraph;
        private RelateNodeGraph nodeGraph = new RelateNodeGraph();

        // the intersection point found (if any)
        private Coordinate invalidPoint;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geomGraph"></param>
        public ConsistentAreaTester(GeometryGraph geomGraph)
        {
            this.geomGraph = geomGraph;
        }

        /// <summary>
        /// Returns the intersection point, or <c>null</c> if none was found.
        /// </summary>        
        public virtual Coordinate InvalidPoint
        {
            get
            {
                return invalidPoint;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsNodeConsistentArea
        {
            get
            {
                /*
                * To fully check validity, it is necessary to
                * compute ALL intersections, including self-intersections within a single edge.
                */
                SegmentIntersector intersector = geomGraph.ComputeSelfNodes(li, true);
                if (intersector.HasProperIntersection)
                {
                    invalidPoint = intersector.ProperIntersectionPoint;
                    return false;
                }
                nodeGraph.Build(geomGraph);
                return IsNodeEdgeAreaLabelsConsistent;
            }
        }

        /// <summary>
        /// Check all nodes to see if their labels are consistent.
        /// If any are not, return false.
        /// </summary>
        private bool IsNodeEdgeAreaLabelsConsistent
        {
            get
            {
                for (IEnumerator nodeIt = nodeGraph.GetNodeEnumerator(); nodeIt.MoveNext(); )
                {
                    RelateNode node = (RelateNode)nodeIt.Current;
                    if (!node.Edges.IsAreaLabelsConsistent)
                    {
                        invalidPoint = (Coordinate)node.Coordinate.Clone();
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Checks for two duplicate rings in an area.
        /// Duplicate rings are rings that are topologically equal
        /// (that is, which have the same sequence of points up to point order).
        /// If the area is topologically consistent (determined by calling the
        /// <c>isNodeConsistentArea</c>,
        /// duplicate rings can be found by checking for EdgeBundles which contain more than one EdgeEnd.
        /// (This is because topologically consistent areas cannot have two rings sharing
        /// the same line segment, unless the rings are equal).
        /// The start point of one of the equal rings will be placed in invalidPoint.
        /// Returns <c>true</c> if this area Geometry is topologically consistent but has two duplicate rings.
        /// </summary>
        public virtual bool HasDuplicateRings
        {
            get
            {
                for (IEnumerator nodeIt = nodeGraph.GetNodeEnumerator(); nodeIt.MoveNext(); )
                {
                    RelateNode node = (RelateNode)nodeIt.Current;
                    for (IEnumerator i = node.Edges.GetEnumerator(); i.MoveNext(); )
                    {
                        EdgeEndBundle eeb = (EdgeEndBundle)i.Current;
                        if (eeb.EdgeEnds.Count > 1)
                        {
                            invalidPoint = eeb.Edge.GetCoordinate(0);
                            return true;
                        }
                    }
                }
                return false;
            }
        }
    }
}
