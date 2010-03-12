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

using MapWindow.Analysis.Topology.Utilities;

namespace MapWindow.Analysis.Topology.Index.Strtree
{
    /// <summary> 
    /// A node of the STR tree. The children of this node are either more nodes
    /// (AbstractNodes) or real data (ItemBoundables). If this node contains real data
    /// (rather than nodes), then we say that this node is a "leaf node".
    /// </summary>
    public abstract class AbstractNode : IBoundable 
    {
        private ArrayList childBoundables = new ArrayList();
        private object bounds = null;
        private int level;

        /// <summary> 
        /// Constructs an AbstractNode at the given level in the tree
        /// </summary>
        /// <param name="level">
        /// 0 if this node is a leaf, 1 if a parent of a leaf, and so on; the
        /// root node will have the highest level.
        /// </param>
        public AbstractNode(int level) 
        {
            this.level = level;
        }

        /// <summary> 
        /// Returns either child AbstractNodes, or if this is a leaf node, real data (wrapped
        /// in ItemBoundables).
        /// </summary>
        public virtual IList ChildBoundables
        {
            get
            {
                return childBoundables;
            }
        }

        /// <summary>
        /// Returns a representation of space that encloses this Boundable,
        /// preferably not much bigger than this Boundable's boundary yet fast to
        /// test for intersection with the bounds of other Boundables. The class of
        /// object returned depends on the subclass of AbstractSTRtree.
        /// </summary>
        /// <returns> 
        /// An Envelope (for STRtrees), an Interval (for SIRtrees), or other
        /// object (for other subclasses of AbstractSTRtree).
        /// </returns>        
        protected abstract object ComputeBounds();

        /// <summary>
        /// 
        /// </summary>
        public virtual object Bounds
        {
            get
            {
                if (bounds == null)
                {
                    bounds = ComputeBounds();
                }
                return bounds;
            }
        }

        /// <summary>
        /// Returns 0 if this node is a leaf, 1 if a parent of a leaf, and so on; the
        /// root node will have the highest level.
        /// </summary>
        public virtual int Level
        {
            get
            {
                return level;
            }
        }

        /// <summary>
        /// Adds either an AbstractNode, or if this is a leaf node, a data object
        /// (wrapped in an ItemBoundable).
        /// </summary>
        /// <param name="childBoundable"></param>
        public virtual void AddChildBoundable(IBoundable childBoundable) 
        {
            Assert.IsTrue(bounds == null);
            childBoundables.Add(childBoundable);
        }
    }
}
