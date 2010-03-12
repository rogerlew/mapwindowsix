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
using MapWindow.GeometriesGraph;
using MapWindow.Analysis.Topology.Algorithm;
using MapWindow.Analysis.Topology.Index.Sweepline;
using MapWindow.Analysis.Topology.Utilities;
namespace MapWindow.Analysis.Topology.Operation.Valid
{
    /// <summary>
    /// Tests whether any of a set of <c>LinearRing</c>s are
    /// nested inside another ring in the set, using a <c>SweepLineIndex</c>
    /// index to speed up the comparisons.
    /// </summary>
    public class SweeplineNestedRingTester
    {
        private readonly GeometryGraph _graph;  // used to find non-node vertices
        private readonly IList _rings = new ArrayList();
        private SweepLineIndex _sweepLine;
        private Coordinate _nestedPt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        public SweeplineNestedRingTester(GeometryGraph graph)
        {
            _graph = graph;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Coordinate NestedPoint 
        {
            get
            {
                return _nestedPt; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ring"></param>
        public virtual void Add(LinearRing ring)
        {
            _rings.Add(ring);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNonNested()
        {
            BuildIndex();
            OverlapAction action = new OverlapAction(this);
            _sweepLine.ComputeOverlaps(action);
            return action.IsNonNested;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildIndex()
        {
            _sweepLine = new SweepLineIndex();
            for (int i = 0; i < _rings.Count; i++) 
            {
                LinearRing ring = (LinearRing) _rings[i];
                IEnvelope env = ring.EnvelopeInternal;
                SweepLineInterval sweepInt = new SweepLineInterval(env.Minimum.X, env.Maximum.X, ring);
                _sweepLine.Add(sweepInt);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerRing"></param>
        /// <param name="searchRing"></param>
        /// <returns></returns>
        private bool IsInside(ILinearRing innerRing, ILinearRing searchRing)
        {
            IList<Coordinate> innerRingPts = innerRing.Coordinates;
            IList<Coordinate> searchRingPts = searchRing.Coordinates;
            if (!innerRing.EnvelopeInternal.Intersects(searchRing.EnvelopeInternal))
                return false;
            Coordinate innerRingPt = IsValidOp.FindPointNotNode(innerRingPts, searchRing, _graph);
            Assert.IsTrue(innerRingPt != null, "Unable to find a ring point not a node of the search ring");
            bool isInside = CGAlgorithms.IsPointInRing(innerRingPt, searchRingPts);
            if (isInside) 
            {
                _nestedPt = new Coordinate(innerRingPt);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public class OverlapAction : ISweepLineOverlapAction
        {
            private readonly SweeplineNestedRingTester _container;
            bool _isNonNested = true;

            /// <summary>
            /// 
            /// </summary>
            public virtual bool IsNonNested
            {
                get 
                { 
                    return _isNonNested; 
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="container"></param>
            public OverlapAction(SweeplineNestedRingTester container)
            {
                _container = container;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="s0"></param>
            /// <param name="s1"></param>
            public virtual void Overlap(SweepLineInterval s0, SweepLineInterval s1)
            {
                LinearRing innerRing = (LinearRing) s0.Item;
                LinearRing searchRing = (LinearRing) s1.Item;
                if (innerRing == searchRing) 
                    return;
                if (_container.IsInside(innerRing, searchRing))
                    _isNonNested = false;
            }

        }
    }
}
