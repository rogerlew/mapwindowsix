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

namespace MapWindow.Analysis.Topology.Index.Strtree
{
    /// <summary>
    /// One-dimensional version of an STR-packed R-tree. SIR stands for
    /// "Sort-Interval-Recursive". STR-packed R-trees are described in:
    /// P. Rigaux, Michel Scholl and Agnes Voisard. Spatial Databases With
    /// Application To GIS. Morgan Kaufmann, San Francisco, 2002.
    /// </summary>
    public class SIRtree : AbstractSTRtree 
    {
        /// <summary>
        /// 
        /// </summary>
        private class AnnonymousComparerImpl : IComparer
        {    
            /// <summary>
            /// 
            /// </summary>
            /// <param name="o1"></param>
            /// <param name="o2"></param>
            /// <returns></returns>
    
            public int Compare(object o1, object o2) 
            {
                return new SIRtree().CompareDoubles(((Interval)((IBoundable)o1).Bounds).Centre, 
                                                    ((Interval)((IBoundable)o2).Bounds).Centre);
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        private class AnonymousIntersectsOpImpl : IIntersectsOp
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="aBounds"></param>
            /// <param name="bBounds"></param>
            /// <returns></returns>
            public bool Intersects(object aBounds, object bBounds) 
            {
                return ((Interval)aBounds).Intersects((Interval)bBounds);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class AnonymousAbstractNodeImpl : AbstractNode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="nodeCapacity"></param>
            public AnonymousAbstractNodeImpl(int nodeCapacity) : base(nodeCapacity) { }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override object ComputeBounds()
            {
                Interval bounds = null;
                for (IEnumerator i = ChildBoundables.GetEnumerator(); i.MoveNext(); )
                {
                    IBoundable childBoundable = (IBoundable)i.Current;
                    if (bounds == null)
                         bounds = new Interval((Interval)childBoundable.Bounds);
                    else bounds.ExpandToInclude((Interval)childBoundable.Bounds);
                }
                return bounds;
            }
        }       

        private IComparer comparator = new AnnonymousComparerImpl(); 
        private IIntersectsOp intersectsOp = new AnonymousIntersectsOpImpl();

        /// <summary> 
        /// Constructs an SIRtree with the default (10) node capacity.
        /// </summary>
        public SIRtree() : this(10) { }

        /// <summary> 
        /// Constructs an SIRtree with the given maximum number of child nodes that
        /// a node may have.
        /// </summary>
        public SIRtree(int nodeCapacity) : base(nodeCapacity) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected override AbstractNode CreateNode(int level) 
        {                
            return new AnonymousAbstractNodeImpl(level);
        }

        /// <summary> 
        /// Inserts an item having the given bounds into the tree.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="item"></param>
        public virtual void Insert(double x1, double x2, object item) 
        {
            base.Insert(new Interval(Math.Min(x1, x2), Math.Max(x1, x2)), item);
        }

        /// <summary>
        /// Returns items whose bounds intersect the given value.
        /// </summary>
        /// <param name="x"></param>
        public virtual IList Query(double x) 
        {
            return Query(x, x);
        }

        /// <summary> 
        /// Returns items whose bounds intersect the given bounds.
        /// </summary>
        /// <param name="x1">Possibly equal to x2.</param>
        /// <param name="x2">Possibly equal to x1.</param>
        public virtual IList Query(double x1, double x2) 
        {
            return base.Query(new Interval(Math.Min(x1, x2), Math.Max(x1, x2)));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override IIntersectsOp IntersectsOp
        {
            get
            {
                return intersectsOp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IComparer GetComparer() 
        {
            return comparator;
        }
    }
}
