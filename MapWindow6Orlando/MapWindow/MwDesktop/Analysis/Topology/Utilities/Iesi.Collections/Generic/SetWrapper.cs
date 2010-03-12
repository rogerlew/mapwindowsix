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
    /// A wrapper that can wrap a ISet as a generic ISet&lt;T&gt; 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// In most operations, there is no copying of collections. The wrapper just delegate the function to the wrapped.
    /// The following functions' implementation may involve collection copying:
    /// Union, Intersect, Minus, ExclusiveOr, ContainsAll, AddAll, RemoveAll, RetainAll
    /// </remarks>
    /// <exception cref="InvalidCastException">
    /// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
    /// </exception>
    public sealed class SetWrapper<T> : ISet<T>
    {
        private ISet innerSet;
        
        
        private SetWrapper(){}
        
        /// <summary>
        /// Sets the Wrapper
        /// </summary>
        /// <param name="toWrap"></param>
        public SetWrapper(ISet toWrap)
        {
            if (toWrap == null)
                throw new ArgumentNullException();
            this.innerSet = toWrap;
            
        }
        
       
        #region ISet<T> Members

        #region Operators
        
        /// <summary>
        /// Combines the two Sets
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ISet<T> Union(ISet<T> a)
        {
            return getSetCopy().Union(a);
        }

        /// <summary>
        /// Intersects the two sets
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ISet<T> Intersect(ISet<T> a)
        {
            return getSetCopy().Intersect(a);  
        }


        /// <summary>
        /// Subtracts a set from this set
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ISet<T> Minus(ISet<T> a)
        {
            return getSetCopy().Minus(a);
        }

        /// <summary>
        /// Creates a new set that includes members in one group or the other but not both
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ISet<T> ExclusiveOr(ISet<T> a)
        {
            return getSetCopy().ExclusiveOr(a);
        } 
        
        #endregion

        /// <summary>
        /// Gets or sets a boolean that is true if this set contains o
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Contains(T o)
        {
            return innerSet.Contains(o);
        }

        /// <summary>
        /// returns a boolean that is true if this set contains all the members of c
        /// </summary>
        /// <param name="c">The Collection to test</param>
        /// <returns></returns>
        public bool ContainsAll(ICollection<T> c)
        {
            return innerSet.ContainsAll(getSetCopy(c));
        }

        /// <summary>
        /// Gets a boolean that indicates if this set is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return innerSet.IsEmpty; }
        }
        
        /// <summary>
        /// Attempts to add o to this set, and returns true if successful
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Add(T o)
        {
            return innerSet.Add(o);
        }

        /// <summary>
        /// Attempts to add the collection c to this set and returns true if successful
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool AddAll(ICollection<T> c)
        {
            return innerSet.AddAll(getSetCopy(c));
        }

        /// <summary>
        /// Attempts to remove o from this set and returns true if successful
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Remove(T o)
        {
            return innerSet.Remove(o);
        }

        /// <summary>
        /// Attempts to remove every member of c from this set and returns true if successful
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool RemoveAll(ICollection<T> c)
        {
            return innerSet.RemoveAll(getSetCopy(c));
        }

        /// <summary>
        /// Removes any members not in c from this set and returns true if successful
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool RetainAll(ICollection<T> c)
        {
            return innerSet.RemoveAll(getSetCopy(c));
        }

        /// <summary>
        /// Clears all the members of the set
        /// </summary>
        public void Clear()
        {
            innerSet.Clear();
        }

        /// <summary>
        /// Creates a deep copy of all the members of this set
        /// </summary>
        /// <returns></returns>
        public ISet<T> Clone()
        {
            return new SetWrapper<T>((ISet)innerSet.Clone());
        }

        /// <summary>
        /// Gets an integer specifying the number of members to this set
        /// </summary>
        public int Count
        {
            get {
                return innerSet.Count;
            }
        }

    
        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        /// <summary>
        /// Copies all the members of the specified arrayIndex to this set
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            innerSet.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets a boolean indicating if this set is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Creates an enumerator for this set
        /// </summary>
        /// <returns>A type specific EnumeratorWrapper </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new EnumeratorWrapper<T>(innerSet.GetEnumerator());
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return innerSet.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region private methods
        private Set<T> getSetCopy(ICollection<T> c)
        {
            return new HashedSet<T>(c);
        }
        private Set<T> getSetCopy(ICollection c)
        {
            Set<T> retVal = new HashedSet<T>();
            ((ISet)retVal).AddAll(c);
            return retVal;
        }
        private Set<T> getSetCopy()
        {
            return getSetCopy(innerSet);
        } 
        #endregion
    }
}
