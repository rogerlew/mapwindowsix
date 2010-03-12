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
using MapWindow.Analysis.Topology.Index.Quadtree;

namespace MapWindow.Analysis.Topology.Index.Bintree
{
    /// <summary>
    /// A Key is a unique identifier for a node in a tree.
    /// It contains a lower-left point and a level number. The level number
    /// is the power of two for the size of the node envelope.
    /// </summary>
    public class Key
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static int ComputeLevel(Interval interval)
        {
            double dx = interval.Width;            
            int level = DoubleBits.GetExponent(dx) + 1;
            return level;
        }

        // the fields which make up the key
        private double pt = 0.0;
        private int level = 0;

        // auxiliary data which is derived from the key for use in computation
        private Interval interval;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        public Key(Interval interval)
        {
            ComputeKey(interval);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double Point
        {
            get
            {
                return pt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Level
        {
            get
            {
                return level;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Interval Interval
        {
            get
            {
                return interval;
            }
        }

        /// <summary>
        /// Return a square envelope containing the argument envelope,
        /// whose extent is a power of two and which is based at a power of 2.
        /// </summary>
        /// <param name="itemInterval"></param>
        public virtual void ComputeKey(Interval itemInterval)
        {
            level = ComputeLevel(itemInterval);
            interval = new Interval();
            ComputeInterval(level, itemInterval);
            // MD - would be nice to have a non-iterative form of this algorithm
            while (!interval.Contains(itemInterval))
            {
                level += 1;
                ComputeInterval(level, itemInterval);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="itemInterval"></param>
        private void ComputeInterval(int level, Interval itemInterval)
        {
            double size = DoubleBits.PowerOf2(level);            
            pt = Math.Floor(itemInterval.Min / size) * size;
            interval.Init(pt, pt + size);
        }
    }
}
