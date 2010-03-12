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
using MapWindow.Geometries;
using MapWindow.Analysis.Topology.Index;
using MapWindow.Analysis.Topology.Index.Chain;
using MapWindow.Analysis.Topology.Index.Strtree;

namespace MapWindow.Analysis.Topology.Noding.Snapround
{

    /// <summary>
    /// "Snaps" all <see cref="SegmentString" />s in a <see cref="ISpatialIndex" /> containing
    /// <see cref="MonotoneChain" />s to a given <see cref="HotPixel" />.
    /// </summary>
    public class MCIndexPointSnapper
    {

        /// <summary>
        /// 
        /// </summary>
        // Public in java code... temporary modified for "safe assembly" in Sql2005
        internal static int numberSnaps = 0;        

        private IList monoChains = null;
        private STRtree index = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MCIndexPointSnapper"/> class.
        /// </summary>
        /// <param name="monoChains"></param>
        /// <param name="index"></param>
        public MCIndexPointSnapper(IList monoChains, ISpatialIndex index)
        {
            this.monoChains = monoChains;
            this.index = (STRtree)index;
        }

        /// <summary>
        /// 
        /// </summary>
        private class QueryVisitor : IItemVisitor
        {
            Envelope env = null;
            HotPixelSnapAction action = null;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="env"></param>
            /// <param name="action"></param>
            public QueryVisitor(Envelope env, HotPixelSnapAction action)
            {
                this.env = env;
                this.action = action;
            }

            /// <summary>
            /// </summary>
            /// <param name="item"></param>
            public void VisitItem(object item)
            {
                MonotoneChain testChain = (MonotoneChain)item;
                testChain.Select(env, action);
            }
        }

        /// <summary>
        /// Snaps (nodes) all interacting segments to this hot pixel.
        /// The hot pixel may represent a vertex of an edge,
        /// in which case this routine uses the optimization
        /// of not noding the vertex itself
        /// </summary>
        /// <param name="hotPixel">The hot pixel to snap to.</param>
        /// <param name="parentEdge">The edge containing the vertex, if applicable, or <c>null</c>.</param>
        /// <param name="vertexIndex"></param>
        /// <returns><c>true</c> if a node was added for this pixel.</returns>
        public bool Snap(HotPixel hotPixel, SegmentString parentEdge, int vertexIndex)
        {
            Envelope pixelEnv = hotPixel.GetSafeEnvelope();
            HotPixelSnapAction hotPixelSnapAction = new HotPixelSnapAction(hotPixel, parentEdge, vertexIndex);
            index.Query(pixelEnv, new QueryVisitor(pixelEnv, hotPixelSnapAction));
            return hotPixelSnapAction.IsNodeAdded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hotPixel"></param>
        /// <returns></returns>
        public bool Snap(HotPixel hotPixel)
        {
            return Snap(hotPixel, null, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        public class HotPixelSnapAction : MonotoneChainSelectAction
        {
            private HotPixel hotPixel = null;
            private SegmentString parentEdge = null;
            private int vertexIndex;
            private bool isNodeAdded = false;

            /// <summary>
            /// Initializes a new instance of the <see cref="HotPixelSnapAction"/> class.
            /// </summary>
            /// <param name="hotPixel"></param>
            /// <param name="parentEdge"></param>
            /// <param name="vertexIndex"></param>
            public HotPixelSnapAction(HotPixel hotPixel, SegmentString parentEdge, int vertexIndex)
            {
                this.hotPixel = hotPixel;
                this.parentEdge = parentEdge;
                this.vertexIndex = vertexIndex;
            }

            /// <summary>
            /// 
            /// </summary>
            public bool IsNodeAdded
            {
                get
                {
                    return isNodeAdded;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mc"></param>
            /// <param name="startIndex"></param>
            public override void Select(MonotoneChain mc, int startIndex)
            {
                SegmentString ss = (SegmentString)mc.Context;
                // don't snap a vertex to itself
                if (parentEdge != null) 
                    if (ss == parentEdge && startIndex == vertexIndex)
                        return;
                isNodeAdded = SimpleSnapRounder.AddSnappedNode(hotPixel, ss, startIndex);
            }

        }

    }
}
