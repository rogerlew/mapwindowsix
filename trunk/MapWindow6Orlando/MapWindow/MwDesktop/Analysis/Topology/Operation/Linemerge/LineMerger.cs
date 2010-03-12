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
using MapWindow.Analysis.Topology.Planargraph;
using MapWindow.Analysis.Topology.Utilities;

namespace MapWindow.Analysis.Topology.Operation.Linemerge
{
    /// <summary>
    /// Sews together a set of fully noded LineStrings. Sewing stops at nodes of degree 1
    /// or 3 or more -- the exception is an isolated loop, which only has degree-2 nodes,
    /// in which case a node is simply chosen as a starting point. The direction of each
    /// merged LineString will be that of the majority of the LineStrings from which it
    /// was derived.
    /// Any dimension of Geometry is handled -- the constituent linework is extracted to 
    /// form the edges. The edges must be correctly noded; that is, they must only meet
    /// at their endpoints.  The LineMerger will still run on incorrectly noded input
    /// but will not form polygons from incorrected noded edges.
    /// </summary>
    public class LineMerger 
    {
        /// <summary>
        /// 
        /// </summary>
        private class AnonymousGeometryComponentFilterImpl : IGeometryComponentFilter
        {
            private LineMerger container = null;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="container"></param>
            public AnonymousGeometryComponentFilterImpl(LineMerger container)
            {
                this.container = container;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="component"></param>
            public virtual void Filter(IGeometry component)
            {
                if (component is LineString)
                    container.Add((LineString)component);
            }
        }

        /// <summary>
        /// Adds a collection of Geometries to be processed. May be called multiple times.
        /// Any dimension of Geometry may be added; the constituent linework will be
        /// extracted.
        /// </summary>
        /// <param name="geometries"></param>
        public virtual void Add(IList geometries) 
        {
            IEnumerator i = geometries.GetEnumerator();
            while (i.MoveNext())
            {
                Geometry geometry = (Geometry) i.Current;
                Add(geometry);
            }
        }

        private LineMergeGraph graph = new LineMergeGraph();
        private IList mergedLineStrings = null;
        private IList edgeStrings = null;
        private IGeometryFactory factory = null;

        /// <summary>
        /// Adds a Geometry to be processed. May be called multiple times.
        /// Any dimension of Geometry may be added; the constituent linework will be
        /// extracted.
        /// </summary>
        /// <param name="geometry"></param>
        public virtual void Add(Geometry geometry)
        {            
            geometry.Apply(new AnonymousGeometryComponentFilterImpl(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineString"></param>
        private void Add(LineString lineString) 
        {
            if (factory == null) 
                this.factory = lineString.Factory;            
            graph.AddEdge(lineString);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void Merge() 
        {
            if (mergedLineStrings != null) 
                return; 
            edgeStrings = new ArrayList();
            BuildEdgeStringsForObviousStartNodes();
            BuildEdgeStringsForIsolatedLoops();
            mergedLineStrings = new ArrayList();    
            for (IEnumerator i = edgeStrings.GetEnumerator(); i.MoveNext(); ) 
            {
                EdgeString edgeString = (EdgeString) i.Current;
                mergedLineStrings.Add(edgeString.ToLineString());
            }    
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildEdgeStringsForObviousStartNodes() 
        {
            BuildEdgeStringsForNonDegree2Nodes();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildEdgeStringsForIsolatedLoops() 
        {
            BuildEdgeStringsForUnprocessedNodes();
        }  

        /// <summary>
        /// 
        /// </summary>
        private void BuildEdgeStringsForUnprocessedNodes() 
        {
            IEnumerator i = graph.Nodes.GetEnumerator();
            while (i.MoveNext())
            {
                Node node = (Node) i.Current;
                if (!node.IsMarked) 
                { 
                    Assert.IsTrue(node.Degree == 2);
                    BuildEdgeStringsStartingAt(node);
                    node.IsMarked = true;
                }
            }
        }  

        /// <summary>
        /// 
        /// </summary>
        private void BuildEdgeStringsForNonDegree2Nodes() 
        {
            IEnumerator i = graph.Nodes.GetEnumerator();
            while (i.MoveNext()) 
            {
                Node node = (Node) i.Current;
                if (node.Degree != 2) 
                { 
                    BuildEdgeStringsStartingAt(node);
                    node.IsMarked = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        private void BuildEdgeStringsStartingAt(Node node) 
        {
            IEnumerator i = node.OutEdges.GetEnumerator();
            while (i.MoveNext()) 
            {
                LineMergeDirectedEdge directedEdge = (LineMergeDirectedEdge) i.Current;
                if (directedEdge.Edge.IsMarked)
                    continue;
                edgeStrings.Add(BuildEdgeStringStartingWith(directedEdge));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private EdgeString BuildEdgeStringStartingWith(LineMergeDirectedEdge start) 
        {    
            EdgeString edgeString = new EdgeString(factory);
            LineMergeDirectedEdge current = start;
            do 
            {
                edgeString.Add(current);
                current.Edge.IsMarked = true;
                current = current.Next;      
            }
            while (current != null && current != start);
            return edgeString;
        }

        /// <summary>
        /// Returns the LineStrings built by the merging process.
        /// </summary>
        public virtual IList MergedLineStrings 
        {
            get
            {
                Merge();
                return mergedLineStrings;
            }
        }
    }
}
