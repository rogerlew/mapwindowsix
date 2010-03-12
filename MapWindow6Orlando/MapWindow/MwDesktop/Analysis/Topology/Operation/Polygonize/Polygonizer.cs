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
namespace MapWindow.Analysis.Topology.Operation.Polygonize
{
    /// <summary>
    /// Polygonizes a set of Geometrys which contain linework that
    /// represents the edges of a planar graph.
    /// Any dimension of Geometry is handled - the constituent linework is extracted
    /// to form the edges.
    /// The edges must be correctly noded; that is, they must only meet
    /// at their endpoints.  The Polygonizer will still run on incorrectly noded input
    /// but will not form polygons from incorrected noded edges.
    /// The Polygonizer reports the follow kinds of errors:
    /// Dangles - edges which have one or both ends which are not incident on another edge endpoint
    /// Cut Edges - edges which are connected at both ends but which do not form part of polygon
    /// Invalid Ring Lines - edges which form rings which are invalid
    /// (e.g. the component lines contain a self-intersection).
    /// </summary>
    public class Polygonizer
    {
        #region Private Variables

        private LineStringAdder _lineStringAdder = null; // Default adder
        private IList _dangles = new ArrayList();
        private PolygonizeGraph _graph;
        private IList _cutEdges = new ArrayList();
        private IList _invalidRingLines = new ArrayList();
        private IList _holeList = null;
        private IList _shellList = null;
        private IList _polyList = null;

        #endregion

        
       
        
       

        /// <summary>
        /// Create a polygonizer with the same {GeometryFactory}
        /// as the input <c>Geometry</c>s.
        /// </summary>
        public Polygonizer() 
        {
            _lineStringAdder = new LineStringAdder(this);
        }

        /// <summary>
        /// Add a collection of geometries to be polygonized.
        /// May be called multiple times.
        /// Any dimension of Geometry may be added;
        /// the constituent linework will be extracted and used.
        /// </summary>
        /// <param name="geomList">A list of <c>Geometry</c>s with linework to be polygonized.</param>
        public virtual void Add(IList geomList)
        {
            for (IEnumerator i = geomList.GetEnumerator(); i.MoveNext(); ) 
            {
                Geometry geometry = (Geometry) i.Current;
                Add(geometry);
            }
        }

        /// <summary>
        /// Add a point to the linework to be polygonized.
        /// May be called multiple times.
        /// Any dimension of Geometry may be added;
        /// the constituent linework will be extracted and used
        /// </summary>
        /// <param name="g">A <c>Geometry</c> with linework to be polygonized.</param>
        public virtual void Add(IGeometry g)
        {
            g.Apply(_lineStringAdder);
        }

        /// <summary>
        /// Add a linestring to the graph of polygon edges.
        /// </summary>
        /// <param name="line">The <c>LineString</c> to add.</param>
        private void Add(LineString line)
        {
            // create a new graph using the factory from the input Geometry
            if (_graph == null)
                _graph = new PolygonizeGraph(line.Factory);
            _graph.AddEdge(line);
        }

        /// <summary>
        /// Compute and returns the list of polygons formed by the polygonization.
        /// </summary>        
        public virtual IList Polygons
        {
            get
            {
                Polygonize();
                return _polyList;
            }
        }

        /// <summary> 
        /// Compute and returns the list of dangling lines found during polygonization.
        /// </summary>
        public virtual IList Dangles
        {
            get
            {
                Polygonize();
                return _dangles;
            }
            protected set
            {
                _dangles = value;
            }
        }

        /// <summary>
        /// Compute and returns the list of cut edges found during polygonization.
        /// </summary>
        public virtual IList CutEdges
        {
            get
            {
                Polygonize();
                return _cutEdges;
            }
            protected set
            {
                _cutEdges = value;
            }
        }

        /// <summary>
        /// Compute and returns the list of lines forming invalid rings found during polygonization.
        /// </summary>
        public virtual IList InvalidRingLines
        {
            get
            {
                Polygonize();
                return _invalidRingLines;
            }
            protected set
            {
                _invalidRingLines = value;
            }
        }

        /// <summary>
        /// Perform the polygonization, if it has not already been carried out.
        /// </summary>
        private void Polygonize()
        {
            // check if already computed
            if (_polyList != null) return;
            _polyList = new ArrayList();

            // if no geometries were supplied it's possible graph could be null
            if (_graph == null) return;

            _dangles = _graph.DeleteDangles();
            _cutEdges = _graph.DeleteCutEdges();
            IList edgeRingList = _graph.GetEdgeRings();

            IList validEdgeRingList = new ArrayList();
            _invalidRingLines = new ArrayList();
            FindValidRings(edgeRingList, validEdgeRingList, _invalidRingLines);

            FindShellsAndHoles(validEdgeRingList);
            AssignHolesToShells(_holeList, _shellList);

            _polyList = new ArrayList();
            for (IEnumerator i = _shellList.GetEnumerator(); i.MoveNext(); ) 
            {
                EdgeRing er = (EdgeRing) i.Current;
                _polyList.Add(er.Polygon);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edgeRingList"></param>
        /// <param name="validEdgeRingList"></param>
        /// <param name="invalidRingList"></param>
        private void FindValidRings(IList edgeRingList, IList validEdgeRingList, IList invalidRingList)
        {
            for (IEnumerator i = edgeRingList.GetEnumerator(); i.MoveNext(); ) 
            {
                EdgeRing er = (EdgeRing) i.Current;
                if (er.IsValid)
                     validEdgeRingList.Add(er);
                else invalidRingList.Add(er.LineString);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edgeRingList"></param>
        private void FindShellsAndHoles(IList edgeRingList)
        {
            _holeList = new ArrayList();
            _shellList = new ArrayList();
            for (IEnumerator i = edgeRingList.GetEnumerator(); i.MoveNext(); ) 
            {
                EdgeRing er = (EdgeRing) i.Current;
                if (er.IsHole)
                     _holeList.Add(er);
                else _shellList.Add(er);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="holeList"></param>
        /// <param name="shellList"></param>
        private static void AssignHolesToShells(IList holeList, IList shellList)
        {
            for (IEnumerator i = holeList.GetEnumerator(); i.MoveNext(); ) 
            {
                EdgeRing holeER = (EdgeRing) i.Current;
                AssignHoleToShell(holeER, shellList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="holeER"></param>
        /// <param name="shellList"></param>
        private static void AssignHoleToShell(EdgeRing holeER, IList shellList)
        {
            EdgeRing shell = EdgeRing.FindEdgeRingContaining(holeER, shellList);
            if (shell != null)
                shell.AddHole(holeER.Ring);
        }
    }
}
