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
using Iesi.Collections;
using MapWindow.Geometries;

namespace MapWindow.Analysis.Topology.Utilities
{
    /// <summary>
    /// A <c>CoordinateFilter</c> that builds a set of <c>Coordinate</c>s.
    /// The set of coordinates contains no duplicate points.
    /// </summary>
    public class UniqueCoordinateArrayFilter : ICoordinateFilter 
    {
        private readonly ISet _table = new SortedSet();
        private readonly ArrayList _list = new ArrayList();


        /// <summary>
        /// Returns the gathered <c>Coordinate</c>s.
        /// </summary>
        public virtual Coordinate[] Coordinates
        {
            get
            {
                return (Coordinate[])_list.ToArray(typeof(Coordinate));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        public virtual void Filter(Coordinate coord) 
        {
            if (!_table.Contains(coord)) 
            {
                _list.Add(coord);
                _table.Add(coord);
            }
        }
    }
}
