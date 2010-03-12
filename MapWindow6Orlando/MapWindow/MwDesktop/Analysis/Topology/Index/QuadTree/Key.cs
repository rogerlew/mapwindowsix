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

namespace MapWindow.Analysis.Topology.Index.Quadtree
{
    /// <summary> 
    /// A Key is a unique identifier for a node in a quadtree.
    /// It contains a lower-left point and a level number. The level number
    /// is the power of two for the size of the node envelope.
    /// </summary>
    public class Key
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static int ComputeQuadLevel(IEnvelope env)
        {
            double dx = env.Width;
            double dy = env.Height;
            double dMax = dx > dy ? dx : dy;
            int level = DoubleBits.GetExponent(dMax) + 1;
            return level;
        }

        // the fields which make up the key
        private Coordinate pt = new Coordinate();
        private int level = 0;

        // auxiliary data which is derived from the key for use in computation
        private IEnvelope env = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemEnv"></param>
        public Key(IEnvelope itemEnv)
        {
            ComputeKey(itemEnv);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Coordinate Point
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
        public virtual IEnvelope Envelope
        {
            get
            {
                return env;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Coordinate Centre
        {
            get
            {
                return new Coordinate((env.Minimum.X + env.Maximum.X) / 2, (env.Minimum.Y + env.Maximum.Y) / 2);
            }
        }

        /// <summary>
        /// Return a square envelope containing the argument envelope,
        /// whose extent is a power of two and which is based at a power of 2.
        /// </summary>
        /// <param name="itemEnv"></param>
        public virtual void ComputeKey(IEnvelope itemEnv)
        {
            level = ComputeQuadLevel(itemEnv);
            env = new Envelope();
            ComputeKey(level, itemEnv);
            // MD - would be nice to have a non-iterative form of this algorithm
            while (!env.Contains(itemEnv))
            {
                level += 1;
                ComputeKey(level, itemEnv);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="itemEnv"></param>
        private void ComputeKey(int level, IEnvelope itemEnv)
        {
            double quadSize = DoubleBits.PowerOf2(level);            
            pt.X = Math.Floor(itemEnv.Minimum.X / quadSize) * quadSize;
            pt.Y = Math.Floor(itemEnv.Minimum.Y / quadSize) * quadSize;
            env = new Envelope(pt.X, pt.X + quadSize, pt.Y, pt.Y + quadSize);
        }
    }
}
