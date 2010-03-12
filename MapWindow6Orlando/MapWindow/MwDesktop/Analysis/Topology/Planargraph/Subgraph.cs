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
using System.Collections.Generic;
using System.Diagnostics;

using Iesi.Collections;
using Iesi.Collections.Generic;

namespace MapWindow.Analysis.Topology.Planargraph
{
    /// <summary>
    /// A subgraph of a <see cref="PlanarGraph" />.
    /// A subgraph may contain any subset of <see cref="Edge" />s
    /// from the parent graph.
    /// It will also automatically contain all <see cref="DirectedEdge" />s
    /// and <see cref="Node" />s associated with those edges.
    /// No new objects are created when edges are added -
    /// all associated components must already exist in the parent graph.
    /// </summary>
    public class Subgraph
    {

        /// <summary>
        /// 
        /// </summary>
        protected PlanarGraph parentGraph;
        
        /// <summary>
        /// 
        /// </summary>
        protected ISet<Edge> edges = new HashedSet<Edge>();
        
        /// <summary>
        /// 
        /// </summary>
        protected IList<DirectedEdge> dirEdges = new List<DirectedEdge>();
        
        /// <summary>
        /// 
        /// </summary>
        protected NodeMap nodeMap = new NodeMap();

        /// <summary>
        /// Creates a new subgraph of the given <see cref="PlanarGraph" />.
        /// </summary>
        /// <param name="parentGraph"></param>
        public Subgraph(PlanarGraph parentGraph) 
        {
            this.parentGraph = parentGraph;
        }
        
        /// <summary>
        ///  Gets the <see cref="PlanarGraph" /> which this subgraph is part of.
        /// </summary>
        /// <returns></returns>
        public virtual PlanarGraph GetParent()
        {
            return parentGraph;
        }

        /// <summary>
        /// Adds an <see cref="Edge" /> to the subgraph.
        /// The associated <see cref="DirectedEdge" />s and <see cref="Node" />s are also added.
        /// </summary>
        /// <param name="e">The <see cref="Edge" /> to add.</param>
        public virtual void Add(Edge e)
        {                        
            if (edges.Contains(e))  
                return;

            edges.Add(e);

            dirEdges.Add(e.GetDirEdge(0));
            dirEdges.Add(e.GetDirEdge(1));
            
            nodeMap.Add(e.GetDirEdge(0).FromNode);
            nodeMap.Add(e.GetDirEdge(1).FromNode);            
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator" /> over the <see cref="DirectedEdge" />s in this graph,
        /// in the order in which they were added.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetDirEdgeEnumerator() 
        { 
            return dirEdges.GetEnumerator(); 
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator" /> over the <see cref="Edge" />s in this graph,
        /// in the order in which they were added.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetEdgeEnumerator() 
        { 
            return edges.GetEnumerator(); 
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator" /> over the <see cref="Node" />s in this graph.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetNodeEnumerator() 
        { 
            return nodeMap.GetEnumerator(); 
        }

        /// <summary>
        /// Tests whether an <see cref="Edge" /> is contained in this subgraph.
        /// </summary>
        /// <param name="e">The <see cref="Edge" /> to test.</param>
        /// <returns><c>true</c> if the <see cref="Edge" /> is contained in this subgraph.</returns>
        public virtual bool Contains(Edge e) 
        { 
            return edges.Contains(e); 
        }
    }
}
