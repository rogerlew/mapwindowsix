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


using MapWindow.GeometriesGraph;
using MapWindow.Analysis.Topology.Operation;
using MapWindow.Geometries;

namespace MapWindow.Analysis.Topology.Operation.Relate
{
    /// <summary>
    /// Implements the <c>Relate()</c> operation on <c>Geometry</c>s.
    /// </summary>
    public class RelateOp : GeometryGraphOperation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IntersectionMatrix Relate(IGeometry a, IGeometry b)
        {
            RelateOp relOp = new RelateOp(a, b);
            IntersectionMatrix im = relOp.IntersectionMatrix;
            return im;
        }

        private RelateComputer relate = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g0"></param>
        /// <param name="g1"></param>
        public RelateOp(IGeometry g0, IGeometry g1) : base(g0, g1)
        {            
            relate = new RelateComputer(arg);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IntersectionMatrix IntersectionMatrix
        {
            get
            {
                return relate.ComputeIM();
            }
        }
    }
}