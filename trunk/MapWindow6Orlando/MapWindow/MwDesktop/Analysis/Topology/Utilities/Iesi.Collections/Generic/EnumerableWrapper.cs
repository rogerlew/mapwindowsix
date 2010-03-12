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
using System.Text;
using System.Collections;

namespace Iesi.Collections.Generic
{
    /// <summary>
    /// A Simple Wrapper for wrapping an regular Enumerable as a generic Enumberable&lt;T&gt;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="InvalidCastException">
    /// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
    /// </exception>
    public  class EnumerableWrapper <T> : IEnumerable<T>
    {
        private IEnumerable innerEnumerable;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="innerEnumerable"></param>
        public EnumerableWrapper(IEnumerable innerEnumerable)
        {
            this.innerEnumerable = innerEnumerable;
        }

        /// <summary>
        /// Tests equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals( this.GetType() )) return false;
            if (obj == this) return true;
            return this.innerEnumerable.Equals(((EnumerableWrapper<T>)obj).innerEnumerable);
        }
        /// <summary>
        /// Obtains a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IEnumerable<T> Members

        /// <summary>
        /// Gets an enumerator for this Enumerable Wrapper
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new EnumeratorWrapper<T>(this.innerEnumerable.GetEnumerator());
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.innerEnumerable.GetEnumerator();    
        }

        #endregion
       
    }
}
