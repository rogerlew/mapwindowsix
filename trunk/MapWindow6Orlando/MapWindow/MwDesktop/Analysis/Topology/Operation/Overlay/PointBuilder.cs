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

using MapWindow.Geometries;
namespace MapWindow.Analysis.Topology.Operation.Overlay
{
    /// <summary>
    /// Constructs <c>Point</c>s from the nodes of an overlay graph.
    /// </summary>
    public class PointBuilder
    {
        private OverlayOp op;
        private IGeometryFactory geometryFactory;
        private PointLocator ptLocator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="geometryFactory"></param>
        /// <param name="ptLocator"></param>
        public PointBuilder(OverlayOp op, IGeometryFactory geometryFactory, PointLocator ptLocator)
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
        /// A list of the Points in the result of the specified overlay operation.
        /// </returns>
        public virtual IList Build(SpatialFunctions opCode)
        {
            IList nodeList = CollectNodes(opCode);
            IList resultPointList = SimplifyPoints(nodeList);
            return resultPointList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opCode"></param>
        /// <returns></returns>
        private IList CollectNodes(SpatialFunctions opCode)
        {
            IList resultNodeList = new ArrayList();
            // add nodes from edge intersections which have not already been included in the result
            IEnumerator nodeit = op.Graph.Nodes.GetEnumerator();
            while (nodeit.MoveNext()) 
            {
                Node n = (Node)nodeit.Current;
                if (!n.IsInResult)
                {
                    Label label = n.Label;
                    if (OverlayOp.IsResultOfOp(label, opCode))                    
                        resultNodeList.Add(n);                    
                }
            }
            return resultNodeList;
        }

        /// <summary>
        /// This method simplifies the resultant Geometry by finding and eliminating
        /// "covered" points.
        /// A point is covered if it is contained in another element Geometry
        /// with higher dimension (e.g. a point might be contained in a polygon,
        /// in which case the point can be eliminated from the resultant).
        /// </summary>
        /// <param name="resultNodeList"></param>
        /// <returns></returns>
        private IList SimplifyPoints(IList resultNodeList)
        {
            IList nonCoveredPointList = new ArrayList();
            IEnumerator it = resultNodeList.GetEnumerator();
            while (it.MoveNext()) 
            {
                Node n = (Node)it.Current;
                Coordinate coord = n.Coordinate;
                if (!op.IsCoveredByLA(coord))
                {
                    IPoint pt = geometryFactory.CreatePoint(coord);
                    nonCoveredPointList.Add(pt);
                }
            }
            return nonCoveredPointList;
        }
    }
}
