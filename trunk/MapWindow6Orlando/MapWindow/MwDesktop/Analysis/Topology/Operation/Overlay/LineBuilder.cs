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


using MapWindow.GeometriesGraph;
using MapWindow.Analysis.Topology.Algorithm;
using MapWindow.Analysis.Topology.Utilities;

using MapWindow.Geometries;
namespace MapWindow.Analysis.Topology.Operation.Overlay
{
    /// <summary>
    /// Forms NTS LineStrings out of a the graph of <c>DirectedEdge</c>s
    /// created by an <c>OverlayOp</c>.
    /// </summary>
    public class LineBuilder
    {
        private OverlayOp op;
        private IGeometryFactory geometryFactory;
        private PointLocator ptLocator;

        private IList lineEdgesList = new ArrayList();
        private IList resultLineList = new ArrayList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="geometryFactory"></param>
        /// <param name="ptLocator"></param>
        public LineBuilder(OverlayOp op, IGeometryFactory geometryFactory, PointLocator ptLocator)
        {
            this.op = op;
            this.geometryFactory = geometryFactory;
            this.ptLocator = ptLocator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opCode"></param>
        /// <returns>
        /// A list of the LineStrings in the result of the specified overlay operation.
        /// </returns>
        public virtual IList Build(SpatialFunctions opCode)
        {
            FindCoveredLineEdges();
            CollectLines(opCode);
            BuildLines(opCode);
            return resultLineList;
        }

        /// <summary>
        /// Find and mark L edges which are "covered" by the result area (if any).
        /// L edges at nodes which also have A edges can be checked by checking
        /// their depth at that node.
        /// L edges at nodes which do not have A edges can be checked by doing a
        /// point-in-polygon test with the previously computed result areas.
        /// </summary>
        private void FindCoveredLineEdges()
        {            
            // first set covered for all L edges at nodes which have A edges too
            IEnumerator nodeit = op.Graph.Nodes.GetEnumerator();
            while(nodeit.MoveNext())
            {
                Node node = (Node)nodeit.Current;                
                ((DirectedEdgeStar)node.Edges).FindCoveredLineEdges();
            }

            /*
             * For all Curve edges which weren't handled by the above,
             * use a point-in-poly test to determine whether they are covered
             */
            IEnumerator it = op.Graph.EdgeEnds.GetEnumerator();
            while (it.MoveNext()) 
            {
                DirectedEdge de = (DirectedEdge)it.Current;
                Edge e = de.Edge;                
                if (de.IsLineEdge && !e.IsCoveredSet)
                {                    
                    bool isCovered = op.IsCoveredByA(de.Coordinate);                    
                    e.IsCovered = isCovered;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opCode"></param>
        private void CollectLines(SpatialFunctions opCode)
        {
            IEnumerator it = op.Graph.EdgeEnds.GetEnumerator();
            while (it.MoveNext()) 
            {
                DirectedEdge de = (DirectedEdge)it.Current;                
                CollectLineEdge(de, opCode, lineEdgesList);
                CollectBoundaryTouchEdge(de, opCode, lineEdgesList);
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="de"></param>
        /// <param name="opCode"></param>
        /// <param name="edges"></param>
        public void CollectLineEdge(DirectedEdge de, SpatialFunctions opCode, IList edges)
        {
            Label label = de.Label;
            Edge e = de.Edge;
            // include Curve edges which are in the result
            if (de.IsLineEdge)
            {                
                if (!de.IsVisited && OverlayOp.IsResultOfOp(label, opCode) && !e.IsCovered)
                {                    
                    edges.Add(e);                    
                    de.VisitedEdge = true;
                }
            }
        }

        /// <summary>
        /// Collect edges from Area inputs which should be in the result but
        /// which have not been included in a result area.
        /// This happens ONLY:
        /// during an intersection when the boundaries of two
        /// areas touch in a line segment
        /// OR as a result of a dimensional collapse.
        /// </summary>
        /// <param name="de"></param>
        /// <param name="opCode"></param>
        /// <param name="edges"></param>
        public virtual void CollectBoundaryTouchEdge(DirectedEdge de, SpatialFunctions opCode, IList edges)
        {            
            Label label = de.Label;            
            if (de.IsLineEdge)  
                return;         // only interested in area edges         
            if (de.IsVisited)   
                return;         // already processed
            if (de.IsInteriorAreaEdge)  
                return; // added to handle dimensional collapses            
            if (de.Edge.IsInResult) 
                return;     // if the edge linework is already included, don't include it again

            // sanity check for labelling of result edgerings
            Assert.IsTrue(!(de.IsInResult || de.Sym.IsInResult) || !de.Edge.IsInResult);            
            // include the linework if it's in the result of the operation
            if (OverlayOp.IsResultOfOp(label, opCode) && opCode == SpatialFunctions.Intersection)
            {
                edges.Add(de.Edge);
                de.VisitedEdge = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opCode"></param>
        private void BuildLines(SpatialFunctions opCode)
        {            
            for (IEnumerator it = lineEdgesList.GetEnumerator(); it.MoveNext(); )
            {
                Edge e = (Edge)it.Current;                
                ILineString line = geometryFactory.CreateLineString(e.Coordinates);
                resultLineList.Add(line);
                e.IsInResult = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edgesList"></param>
        private void LabelIsolatedLines(IList edgesList)
        {
            IEnumerator it = edgesList.GetEnumerator();
            while (it.MoveNext()) 
            {
                Edge e = (Edge)it.Current;
                Label label = e.Label;
                if (e.IsIsolated)
                {
                    if (label.IsNull(0))
                         LabelIsolatedLine(e, 0);
                    else LabelIsolatedLine(e, 1);
                }
            }
        }

        /// <summary>
        /// Label an isolated node with its relationship to the target point.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="targetIndex"></param>
        private void LabelIsolatedLine(Edge e, int targetIndex)
        {
            Locations loc = ptLocator.Locate(e.Coordinate, op.GetArgGeometry(targetIndex));
            e.Label.SetLocation(targetIndex, loc);
        }
    }
}
