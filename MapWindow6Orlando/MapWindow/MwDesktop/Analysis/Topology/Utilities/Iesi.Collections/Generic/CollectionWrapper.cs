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
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Iesi.Collections.Generic 
{
    /// <summary>
    /// Generic Collection wrapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionWrapper<T> : EnumerableWrapper<T>, ICollection<T>
    {
        private ICollection innerCollection;
        
        /// <summary>
        /// Constructor for CollectionWrapper
        /// </summary>
        /// <param name="toWrap">The Collection to Wrap</param>
        public CollectionWrapper(ICollection toWrap) :base(toWrap)
        {
            this.innerCollection = toWrap;
        }
        

        #region ICollection<T> Members

        /// <summary>
        /// Throws a readonly exception
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            ThrowReadOnlyException();
        }

        /// <summary>
        ///  Throws a readonly exception
        /// </summary>
        public void Clear()
        {
            ThrowReadOnlyException();
        }

        /// <summary>
        /// Returns a boolean that is true if this collection contains item
        /// </summary>
        /// <param name="item">The item to test</param>
        /// <returns>A Boolean that is true if this collection contains item</returns>
        public bool Contains(T item)
        {
            foreach (object o in innerCollection)
                if ( (object)item == o) return true;
            return false;
        }

        /// <summary>
        /// Copies all the members of array to this set
        /// </summary>
        /// <param name="array">The array to copy</param>
        /// <param name="arrayIndex">The index with which to start copying</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            innerCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The integer count
        /// </summary>
        public int Count
        {
            get { return innerCollection.Count; }
        }

        /// <summary>
        /// Always returns True
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; //always return true since the old ICollection does not support mutation 
            }
        }

        /// <summary>
        /// Throws a readonly exception
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return ThrowReadOnlyException();
        }

        #endregion
        
        private bool ThrowReadOnlyException()
        {
            throw new NotSupportedException("The ICollection is read-only.");
        
        }
    }
}
