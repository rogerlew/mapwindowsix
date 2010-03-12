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
        /// Simple Wrapper for wrapping an regular Enumerator as a generic Enumberator&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="InvalidCastException">
        /// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
        /// </exception>
        public struct EnumeratorWrapper<T> : IEnumerator<T>
        {
            private IEnumerator innerEnumerator;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="toWrap"></param>
            public EnumeratorWrapper(IEnumerator toWrap)
            {
                this.innerEnumerator = toWrap;
            }

            #region IEnumerator<T> Members

            /// <summary>
            /// The current member being enumerated
            /// </summary>
            public T Current
            {
                get { return (T)innerEnumerator.Current; }
            }

            #endregion

            #region IDisposable Members

            /// <summary>
            /// Disposes the innerEnumerator
            /// </summary>
            public void Dispose()
            {
                this.innerEnumerator = null;
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { return innerEnumerator.Current; }
            }

            /// <summary>
            /// Moves to the next element
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                return innerEnumerator.MoveNext();
            }

            /// <summary>
            /// Resets the enumerator to the starting position
            /// </summary>
            public void Reset()
            {
                innerEnumerator.Reset();
            }

            #endregion
        }
   
}
