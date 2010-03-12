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
namespace MapWindow.Analysis.Topology.Planargraph
{
    /// <summary>
    /// A sorted collection of <c>DirectedEdge</c>s which leave a <c>Node</c>
    /// in a <c>PlanarGraph</c>.
    /// </summary>
    public class DirectedEdgeStar
    {
        /// <summary>
        /// The underlying list of outgoing DirectedEdges.
        /// </summary>
        protected IList outEdges = new ArrayList();

        private bool sorted = false;

        /// <summary>
        /// Constructs a DirectedEdgeStar with no edges.
        /// </summary>
        public DirectedEdgeStar() { }

        /// <summary>
        /// Adds a new member to this DirectedEdgeStar.
        /// </summary>
        /// <param name="de"></param>
        public virtual void Add(DirectedEdge de)
        {            
            outEdges.Add(de);
            sorted = false;
        }

        /// <summary>
        /// Drops a member of this DirectedEdgeStar.
        /// </summary>
        /// <param name="de"></param>
        public virtual void Remove(DirectedEdge de)
        {
            outEdges.Remove(de);
        }

        /// <summary>
        /// Returns an Iterator over the DirectedEdges, in ascending order by angle with the positive x-axis.
        /// </summary>
        public virtual IEnumerator GetEnumerator()
        {            
            SortEdges();
            return outEdges.GetEnumerator();
        }
        
        /// <summary>
        /// Returns the number of edges around the Node associated with this DirectedEdgeStar.
        /// </summary>
        public virtual int Degree
        {
            get
            {
                return outEdges.Count;
            }
        }

        /// <summary>
        /// Returns the coordinate for the node at wich this star is based.
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                IEnumerator it = GetEnumerator();
                if (!it.MoveNext()) 
                    return null;
                DirectedEdge e = (DirectedEdge)it.Current;
                return e.Coordinate;
            }
        }

        /// <summary>
        /// Returns the DirectedEdges, in ascending order by angle with the positive x-axis.
        /// </summary>
        public virtual IList Edges
        {
            get
            {
                SortEdges();
                return outEdges;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SortEdges()
        {
            if (!sorted)
            {
                ArrayList list  = (ArrayList)outEdges;
                list.Sort();
                sorted = true;                
            }
        }

        /// <summary>
        /// Returns the zero-based index of the given Edge, after sorting in ascending order
        /// by angle with the positive x-axis.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public virtual int GetIndex(Edge edge)
        {
            SortEdges();
            for (int i = 0; i < outEdges.Count; i++)
            {
                DirectedEdge de = (DirectedEdge)outEdges[i];
                if (de.Edge == edge)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns the zero-based index of the given DirectedEdge, after sorting in ascending order
        /// by angle with the positive x-axis.
        /// </summary>
        /// <param name="dirEdge"></param>
        /// <returns></returns>
        public virtual int GetIndex(DirectedEdge dirEdge)
        {
            SortEdges();
            for (int i = 0; i < outEdges.Count; i++)
            {
                DirectedEdge de = (DirectedEdge)outEdges[i];
                if (de == dirEdge)
                    return i;
            }
            return -1;
        }

        /// <summary> 
        /// Returns the remainder when i is divided by the number of edges in this
        /// DirectedEdgeStar. 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual int GetIndex(int i)
        {
            int modi = i % outEdges.Count;
            //I don't think modi can be 0 (assuming i is positive) [Jon Aquino 10/28/2003] 
            if (modi < 0) 
                modi += outEdges.Count;
            return modi;
        }

        /// <summary>
        /// Returns the DirectedEdge on the left-hand side of the given DirectedEdge (which
        /// must be a member of this DirectedEdgeStar). 
        /// </summary>
        /// <param name="dirEdge"></param>
        /// <returns></returns>
        public virtual DirectedEdge GetNextEdge(DirectedEdge dirEdge)
        {
            int i = GetIndex(dirEdge);
            return (DirectedEdge)outEdges[GetIndex(i + 1)];
        }
    }
}
