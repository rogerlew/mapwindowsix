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

namespace MapWindow.Analysis.Topology.Index.Bintree
{
    /// <summary>
    /// An <c>BinTree</c> (or "Binary Interval Tree")
    /// is a 1-dimensional version of a quadtree.
    /// It indexes 1-dimensional intervals (which of course may
    /// be the projection of 2-D objects on an axis).
    /// It supports range searching
    /// (where the range may be a single point).
    /// This implementation does not require specifying the extent of the inserted
    /// items beforehand.  It will automatically expand to accomodate any extent
    /// of dataset.
    /// This index is different to the Interval Tree of Edelsbrunner
    /// or the Segment Tree of Bentley.
    /// </summary>
    public class Bintree
    {
        /// <summary>
        /// Ensure that the Interval for the inserted item has non-zero extents.
        /// Use the current minExtent to pad it, if necessary.
        /// </summary>
        public static Interval EnsureExtent(Interval itemInterval, double minExtent)
        {
            double min = itemInterval.Min;
            double max = itemInterval.Max;
            // has a non-zero extent
            if (min != max) 
                return itemInterval;
            // pad extent
            if (min == max)
            {
                min = min - minExtent / 2.0;
                max = min + minExtent / 2.0;
            }
            return new Interval(min, max);
        }

        private Root root;
        
        /*
        * Statistics:
        * minExtent is the minimum extent of all items
        * inserted into the tree so far. It is used as a heuristic value
        * to construct non-zero extents for features with zero extent.
        * Start with a non-zero extent, in case the first feature inserted has
        * a zero extent in both directions.  This value may be non-optimal, but
        * only one feature will be inserted with this value.
        **/
        private double minExtent = 1.0;

        /// <summary>
        /// 
        /// </summary>
        public Bintree()
        {
            root = new Root();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Depth
        {
            get
            {
                if (root != null) 
                    return root.Depth;
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Count
        {
            get
            {
                if (root != null) 
                    return root.Count;
                return 0;
            }
        }

        /// <summary>
        /// Compute the total number of nodes in the tree.
        /// </summary>
        /// <returns>The number of nodes in the tree.</returns>
        public virtual int NodeSize
        {
            get
            {
                if (root != null) 
                    return root.NodeCount;
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemInterval"></param>
        /// <param name="item"></param>
        public virtual void Insert(Interval itemInterval, object item)
        {
            CollectStats(itemInterval);
            Interval insertInterval = EnsureExtent(itemInterval, minExtent);            
            root.Insert(insertInterval, item);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetEnumerator()
        {
            IList foundItems = new ArrayList();
            root.AddAllItems(foundItems);
            return foundItems.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public virtual IList Query(double x)
        {
            return Query(new Interval(x, x));
        }

        /// <summary>
        /// min and max may be the same value.
        /// </summary>
        /// <param name="interval"></param>
        public virtual IList Query(Interval interval)
        {
            /*
             * the items that are matched are all items in intervals
             * which overlap the query interval
             */
            IList foundItems = new ArrayList();
            Query(interval, foundItems);
            return foundItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="foundItems"></param>
        public virtual void Query(Interval interval, IList foundItems)
        {
            root.AddAllItemsFromOverlapping(interval, foundItems);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        private void CollectStats(Interval interval)
        {
            double del = interval.Width;
            if (del < minExtent && del > 0.0)
                minExtent = del;
        }
    }
}
