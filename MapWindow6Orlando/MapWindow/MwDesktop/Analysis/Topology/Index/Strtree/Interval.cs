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
    /// A contiguous portion of 1D-space. Used internally by SIRtree.
    /// </summary>
    public class Interval
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public Interval(Interval other) : this(other.min, other.max) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Interval(double min, double max)
        {
            Assert.IsTrue(min <= max);
            this.min = min;
            this.max = max;
        }

        private double min;
        private double max;

        /// <summary>
        /// 
        /// </summary>
        public virtual double Centre
        {
            get
            {
                return (min + max) / 2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns><c>this</c></returns>
        public virtual Interval ExpandToInclude(Interval other)
        {
            max = Math.Max(max, other.max);
            min = Math.Min(min, other.min);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Intersects(Interval other)
        {
            return !(other.min > max || other.max < min);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) 
        {
            if (!(o is Interval))             
                return false;            
            Interval other = (Interval) o;
            return min == other.min && max == other.max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }    
}
