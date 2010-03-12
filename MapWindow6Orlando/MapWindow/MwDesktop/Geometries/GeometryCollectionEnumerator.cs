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

namespace MapWindow.Geometries
{
    /// <summary>
    /// Iterates over all <c>Geometry</c>'s in a <c>GeometryCollection</c>. 
    /// Implements a pre-order depth-first traversal of the <c>GeometryCollection</c>
    /// (which may be nested). The original <c>GeometryCollection</c> is
    /// returned as well (as the first object), as are all sub-collections. It is
    /// simple to ignore the <c>GeometryCollection</c> objects if they are not
    /// needed.
    /// </summary>    
    public class GeometryCollectionEnumerator : IEnumerator
    {

        /// <summary>
        /// The <c>GeometryCollection</c> being iterated over.
        /// </summary>
        private readonly IGeometryCollection _parent;

        /// <summary>
        /// Indicates whether or not the first element (the <c>GeometryCollection</c>
        /// ) has been returned.
        /// </summary>
        private bool _atStart;

        /// <summary>
        /// The number of <c>Geometry</c>s in the the <c>GeometryCollection</c>.
        /// </summary>
        private readonly int _max;

        /// <summary>
        /// The index of the <c>Geometry</c> that will be returned when <c>next</c>
        /// is called.
        /// </summary>
        private int _index;

        /// <summary>
        /// The iterator over a nested <c>GeometryCollection</c>, or <c>null</c>
        /// if this <c>GeometryCollectionIterator</c> is not currently iterating
        /// over a nested <c>GeometryCollection</c>.
        /// </summary>
        private GeometryCollectionEnumerator _subcollectionEnumerator;

        /// <summary>
        /// Constructs an iterator over the given <c>GeometryCollection</c>.
        /// </summary>
        /// <param name="parent">
        /// The collection over which to iterate; also, the first
        /// element returned by the iterator.
        /// </param>
        public GeometryCollectionEnumerator(IGeometryCollection parent) 
        {
            _parent = parent;
            _atStart = true;
            _index = 0;
            _max = parent.NumGeometries;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool MoveNext() 
        {
            if (_atStart) 
                return true;
            if (_subcollectionEnumerator != null) 
            {
                if (_subcollectionEnumerator.MoveNext())  
                    return true;
                _subcollectionEnumerator = null;
            }
            if (_index >= _max) 
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual object Current
        {
            get
            {
                // the _parent GeometryCollection is the first object returned
                if (_atStart) 
                {
                    _atStart = false;
                    return _parent;
                }
                if (_subcollectionEnumerator != null)
                {
                    if (_subcollectionEnumerator.MoveNext()) 
                        return _subcollectionEnumerator.Current;
                    _subcollectionEnumerator = null;
                }
                if (_index >= _max) 
                    throw new ArgumentOutOfRangeException(); 
                
                IGeometry obj = _parent.GetGeometryN(_index++);
                if (obj is GeometryCollection) 
                {
                    _subcollectionEnumerator = new GeometryCollectionEnumerator((GeometryCollection) obj);
                    // there will always be at least one element in the sub-collection
                    return _subcollectionEnumerator.Current;
                }
                return obj;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Reset()
        {
            _atStart = true;
            _index = 0;            
        }
    }    
}
