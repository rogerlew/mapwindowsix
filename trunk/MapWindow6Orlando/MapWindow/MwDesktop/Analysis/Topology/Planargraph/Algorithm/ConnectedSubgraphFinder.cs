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

using MapWindow.Analysis.Topology.Algorithm;

namespace MapWindow.Analysis.Topology.Planargraph.Algorithm
{
    /// <summary>
    /// Finds all connected <see cref="Subgraph" />s of a <see cref="PlanarGraph" />.
    /// </summary>
    public class ConnectedSubgraphFinder
    {
        private PlanarGraph graph;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConnectedSubgraphFinder"/> class.
        /// </summary>
        /// <param name="graph">The <see cref="PlanarGraph" />.</param>
        public ConnectedSubgraphFinder(PlanarGraph graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IList GetConnectedSubgraphs()
        {
            IList subgraphs = new ArrayList();

            GraphComponent.SetVisited(graph.GetNodeEnumerator(), false);
            IEnumerator ienum = graph.GetEdgeEnumerator();
            while(ienum.MoveNext())
            {
                Edge e = ienum.Current as Edge;
                Node node = e.GetDirEdge(0).FromNode;
                if (!node.IsVisited)
                    subgraphs.Add(FindSubgraph(node));                
            }
            return subgraphs;
        }

        private Subgraph FindSubgraph(Node node)
        {
            Subgraph subgraph = new Subgraph(graph);
            AddReachable(node, subgraph);
            return subgraph;
        }

        /// <summary>
        /// Adds all nodes and edges reachable from this node to the subgraph.
        /// Uses an explicit stack to avoid a large depth of recursion.
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="subgraph"></param>
        private void AddReachable(Node startNode, Subgraph subgraph)
        {
            Stack nodeStack = new Stack();
            nodeStack.Push(startNode);
            while (!(nodeStack.Count == 0))
            {
                Node node = (Node)nodeStack.Pop();
                AddEdges(node, nodeStack, subgraph);
            }
        }

        /// <summary>
        /// Adds the argument node and all its out edges to the subgraph.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeStack"></param>
        /// <param name="subgraph"></param>
        private void AddEdges(Node node, Stack nodeStack, Subgraph subgraph)
        {
            node.IsVisited = true;
            IEnumerator i = ((DirectedEdgeStar)node.OutEdges).GetEnumerator();
            while(i.MoveNext())
            {
                DirectedEdge de = (DirectedEdge)i.Current;
                subgraph.Add(de.Edge);
                Node toNode = de.ToNode;
                if (!toNode.IsVisited) 
                    nodeStack.Push(toNode);
            }
        }
    }
}
