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
using Iesi.Collections;
using MapWindow.Geometries;
using MapWindow.Analysis.Topology.Planargraph;
using MapWindow.Analysis.Topology.Utilities;
namespace MapWindow.Analysis.Topology.Operation.Polygonize
{
    /// <summary>
    /// Represents a planar graph of edges that can be used to compute a
    /// polygonization, and implements the algorithms to compute the
    /// EdgeRings formed by the graph.
    /// The marked flag on DirectedEdges is used to indicate that a directed edge
    /// has be logically deleted from the graph.
    /// </summary>
    public class PolygonizeGraph : PlanarGraph
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static int GetDegreeNonDeleted(Node node)
        {
            IList edges = node.OutEdges.Edges;
            int degree = 0;
            for (IEnumerator i = edges.GetEnumerator(); i.MoveNext(); ) 
            {
                PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) i.Current;
                if (! de.IsMarked)
                    degree++;
            }
            return degree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        private static int GetDegree(Node node, long label)
        {
            IList edges = node.OutEdges.Edges;
            int degree = 0;
            for (IEnumerator i = edges.GetEnumerator(); i.MoveNext(); )
            {
                PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) i.Current;
                if (de.Label == label) 
                    degree++;
            }
            return degree;
        }

        /// <summary>
        /// Deletes all edges at a node.
        /// </summary>
        /// <param name="node"></param>
        public static void DeleteAllEdges(Node node)
        {
            IList edges = node.OutEdges.Edges;
            for (IEnumerator i = edges.GetEnumerator(); i.MoveNext(); )
            {
                PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) i.Current;
                de.IsMarked = true;
                PolygonizeDirectedEdge sym = (PolygonizeDirectedEdge) de.Sym;
                if (sym != null) sym.IsMarked = true;
            }
        }

        private readonly IGeometryFactory _factory;

        /// <summary>
        /// Create a new polygonization graph.
        /// </summary>
        /// <param name="factory"></param>
        public PolygonizeGraph(IGeometryFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Add a <c>LineString</c> forming an edge of the polygon graph.
        /// </summary>
        /// <param name="line">The line to add.</param>
        public virtual void AddEdge(LineString line)
        {
            if (line.IsEmpty) return;
            IList<Coordinate> linePts = CoordinateArrays.RemoveRepeatedPoints(line.Coordinates);
            Coordinate startPt = linePts[0];
            Coordinate endPt = linePts[linePts.Count - 1];

            Node nStart = GetNode(startPt);
            Node nEnd = GetNode(endPt);

            DirectedEdge de0 = new PolygonizeDirectedEdge(nStart, nEnd, linePts[1], true);
            DirectedEdge de1 = new PolygonizeDirectedEdge(nEnd, nStart, linePts[linePts.Count - 2], false);
            Edge edge = new PolygonizeEdge(line);
            edge.SetDirectedEdges(de0, de1);
            Add(edge);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        private Node GetNode(Coordinate pt)
        {
            Node node = FindNode(pt);
            if (node == null) 
            {
                node = new Node(pt);
                // ensure node is only added once to graph
                Add(node);
            }
            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeNextCWEdges()
        {
            // set the next pointers for the edges around each node
            for (IEnumerator iNode = GetNodeEnumerator(); iNode.MoveNext(); ) 
            {
                Node node = (Node) iNode.Current;
                ComputeNextCWEdges(node);
            }
        }

        /// <summary>
        /// Convert the maximal edge rings found by the initial graph traversal
        /// into the minimal edge rings required by NTS polygon topology rules.
        /// </summary>
        /// <param name="ringEdges">The list of start edges for the edgeRings to convert.</param>
        private static void ConvertMaximalToMinimalEdgeRings(IEnumerable ringEdges)
        {
            for (IEnumerator i = ringEdges.GetEnumerator(); i.MoveNext(); ) 
            {
                PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) i.Current;
                long label = de.Label;
                IList intNodes = FindIntersectionNodes(de, label);

                if (intNodes == null) continue;
                // flip the next pointers on the intersection nodes to create minimal edge rings
                for (IEnumerator iNode = intNodes.GetEnumerator(); iNode.MoveNext(); )
                {
                    Node node = (Node) iNode.Current;
                    ComputeNextCCWEdges(node, label);
                }
            }
        }

        /// <summary>
        /// Finds all nodes in a maximal edgering which are self-intersection nodes
        /// </summary>
        /// <param name="startDE"></param>
        /// <param name="label"></param>
        /// <returns> 
        /// The list of intersection nodes found,
        /// or null if no intersection nodes were found.       
        /// </returns>
        private static IList FindIntersectionNodes(PolygonizeDirectedEdge startDE, long label)
        {
            PolygonizeDirectedEdge de = startDE;
            IList intNodes = null;
            do 
            {
                if (de == null) continue;
                Node node = de.FromNode;
                if (GetDegree(node, label) > 1) 
                {
                    if (intNodes == null)
                        intNodes = new ArrayList();
                    intNodes.Add(node);
                }
                de = de.Next;
                Assert.IsTrue(de != null, "found null DE in ring");
                Assert.IsTrue(de == startDE || !de.IsInRing, "found DE already in ring");
            } 
            while (de != startDE);
            return intNodes;
        }

        /// <summary>
        /// Computes the EdgeRings formed by the edges in this graph.        
        /// </summary>
        /// <returns>A list of the{EdgeRings found by the polygonization process.</returns>
        public virtual IList GetEdgeRings()
        {
            // maybe could optimize this, since most of these pointers should be set correctly already by deleteCutEdges()
            ComputeNextCWEdges();
            // clear labels of all edges in graph
            Label(DirectedEdges, -1);
            IList maximalRings = FindLabeledEdgeRings(DirectedEdges);
            ConvertMaximalToMinimalEdgeRings(maximalRings);

            // find all edgerings
            IList edgeRingList = new ArrayList();
            for (IEnumerator i = DirectedEdges.GetEnumerator(); i.MoveNext(); )
            {
                PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) i.Current;
                if (de.IsMarked) continue;
                if (de.IsInRing) continue;

                EdgeRing er = FindEdgeRing(de);
                edgeRingList.Add(er);
            }
            return edgeRingList;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirEdges">A List of the DirectedEdges in the graph.</param>
        /// <returns>A List of DirectedEdges, one for each edge ring found.</returns>
        private static IList FindLabeledEdgeRings(IEnumerable dirEdges)
        {
            IList edgeRingStarts = new ArrayList();
            // label the edge rings formed
            long currLabel = 1;
            for (IEnumerator i = dirEdges.GetEnumerator(); i.MoveNext(); ) 
            {
                PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) i.Current;
                if (de.IsMarked) continue;
                if (de.Label >= 0) continue;

                edgeRingStarts.Add(de);
                IList edges = FindDirEdgesInRing(de);

                Label(edges, currLabel);
                currLabel++;
            }
            return edgeRingStarts;
        }

        /// <summary>
        /// Finds and removes all cut edges from the graph.
        /// </summary>
        /// <returns>A list of the <c>LineString</c>s forming the removed cut edges.</returns>
        public virtual IList DeleteCutEdges()
        {
            ComputeNextCWEdges();
            // label the current set of edgerings
            FindLabeledEdgeRings(DirectedEdges);
            /*
            * Cut Edges are edges where both dirEdges have the same label.
            * Delete them, and record them
            */
            IList cutLines = new ArrayList();
            for (IEnumerator i = DirectedEdges.GetEnumerator(); i.MoveNext(); )
            {
                PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) i.Current;
                if (de.IsMarked) continue;

                PolygonizeDirectedEdge sym = (PolygonizeDirectedEdge) de.Sym;
                if (de.Label == sym.Label) 
                {
                    de.IsMarked = true;
                    sym.IsMarked = true;

                    // save the line as a cut edge
                    PolygonizeEdge e = (PolygonizeEdge) de.Edge;
                    cutLines.Add(e.Line);
                }
            }
            return cutLines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirEdges"></param>
        /// <param name="label"></param>
        private static void Label(IEnumerable dirEdges, long label)
        {
            for (IEnumerator i = dirEdges.GetEnumerator(); i.MoveNext(); )
            {
                PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) i.Current;
                de.Label = label;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        private static void ComputeNextCWEdges(Node node)
        {
            DirectedEdgeStar deStar = node.OutEdges;
            PolygonizeDirectedEdge startDE = null;
            PolygonizeDirectedEdge prevDE = null;

            // the edges are stored in CCW order around the star
            for (IEnumerator i = deStar.Edges.GetEnumerator(); i.MoveNext(); )
            {
                PolygonizeDirectedEdge outDE = (PolygonizeDirectedEdge) i.Current;
                if (outDE.IsMarked) continue;

                if (startDE == null) startDE = outDE;
                if (prevDE != null) 
                {
                    PolygonizeDirectedEdge sym = (PolygonizeDirectedEdge) prevDE.Sym;
                    sym.Next = outDE;
                }
                prevDE = outDE;
            }
            if (prevDE != null) 
            {
                PolygonizeDirectedEdge sym = (PolygonizeDirectedEdge) prevDE.Sym;
                sym.Next = startDE;
            }
        }

        /// <summary>
        /// Computes the next edge pointers going CCW around the given node, for the
        /// given edgering label.
        /// This algorithm has the effect of converting maximal edgerings into minimal edgerings
        /// </summary>
        /// <param name="node"></param>
        /// <param name="label"></param>
        private static void ComputeNextCCWEdges(Node node, long label)
        {
            DirectedEdgeStar deStar = node.OutEdges;
            //PolyDirectedEdge lastInDE = null;
            PolygonizeDirectedEdge firstOutDE = null;
            PolygonizeDirectedEdge prevInDE = null;

            // the edges are stored in CCW order around the star
            IList edges = deStar.Edges;
            //for (IEnumerator i = deStar.Edges.GetEnumerator(); i.MoveNext(); ) {
            for (int i = edges.Count - 1; i >= 0; i--) 
            {
                PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) edges[i];
                PolygonizeDirectedEdge sym = (PolygonizeDirectedEdge) de.Sym;

                PolygonizeDirectedEdge outDE = null;
                if (de.Label == label) outDE = de;
                
                PolygonizeDirectedEdge inDE = null;
                if (sym.Label == label) inDE =  sym;

                if (outDE == null && inDE == null) continue;  // this edge is not in edgering

                if (inDE != null) 
                    prevInDE = inDE;                

                if (outDE != null) 
                {
                    if (prevInDE != null) 
                    {
                        prevInDE.Next = outDE;
                        prevInDE = null;
                    }
                    if (firstOutDE == null)
                        firstOutDE = outDE;
                }
            }
            if (prevInDE != null)
            {
                Assert.IsTrue(firstOutDE != null);
                prevInDE.Next = firstOutDE;
            }
        }

        /// <summary>
        /// Traverse a ring of DirectedEdges, accumulating them into a list.
        /// This assumes that all dangling directed edges have been removed
        /// from the graph, so that there is always a next dirEdge.
        /// </summary>
        /// <param name="startDE">The DirectedEdge to start traversing at.</param>
        /// <returns>A List of DirectedEdges that form a ring.</returns>
        private static IList FindDirEdgesInRing(PolygonizeDirectedEdge startDE)
        {
            PolygonizeDirectedEdge de = startDE;
            IList edges = new ArrayList();
            do 
            {
                edges.Add(de);
                de = de.Next;
                Assert.IsTrue(de != null, "found null DE in ring");
                Assert.IsTrue(de == startDE || !de.IsInRing, "found DE already in ring");
            }
            while (de != startDE);
            return edges;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDE"></param>
        /// <returns></returns>
        private EdgeRing FindEdgeRing(PolygonizeDirectedEdge startDE)
        {
            PolygonizeDirectedEdge de = startDE;
            EdgeRing er = new EdgeRing(_factory);
            do 
            {
                er.Add(de);
                de.Ring = er;
                de = de.Next;
                Assert.IsTrue(de != null, "found null DE in ring");
                Assert.IsTrue(de == startDE || ! de.IsInRing, "found DE already in ring");
            }
            while (de != startDE);
            return er;
        }

        /// <summary>
        /// Marks all edges from the graph which are "dangles".
        /// Dangles are which are incident on a node with degree 1.
        /// This process is recursive, since removing a dangling edge
        /// may result in another edge becoming a dangle.
        /// In order to handle large recursion depths efficiently,
        /// an explicit recursion stack is used.
        /// </summary>
        /// <returns>A List containing the LineStrings that formed dangles.</returns>
        public virtual IList DeleteDangles()
        {
            IList nodesToRemove = FindNodesOfDegree(1);
            ISet dangleLines = new HashedSet();

            Stack nodeStack = new Stack();
            for (IEnumerator i = nodesToRemove.GetEnumerator(); i.MoveNext(); ) 
                nodeStack.Push(i.Current);
            
            while (nodeStack.Count != 0) 
            {
                Node node = (Node) nodeStack.Pop();

                DeleteAllEdges(node);
                IList nodeOutEdges = node.OutEdges.Edges;
                for (IEnumerator i = nodeOutEdges.GetEnumerator(); i.MoveNext(); ) 
                {
                    PolygonizeDirectedEdge de = (PolygonizeDirectedEdge) i.Current;
                    // delete this edge and its sym
                    de.IsMarked = true;
                    PolygonizeDirectedEdge sym = (PolygonizeDirectedEdge) de.Sym;
                    if (sym != null) sym.IsMarked = true;

                    // save the line as a dangle
                    PolygonizeEdge e = (PolygonizeEdge) de.Edge;
                    dangleLines.Add(e.Line);

                    Node toNode = de.ToNode;
                    // add the toNode to the list to be processed, if it is now a dangle
                    if (GetDegreeNonDeleted(toNode) == 1)
                        nodeStack.Push(toNode);
                }
            }
            return new ArrayList(dangleLines);
        }
    }
}
