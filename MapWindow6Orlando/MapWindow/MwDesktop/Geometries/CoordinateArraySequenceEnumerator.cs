//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/8/2008 4:04:43 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections;
using System.Collections.Generic;

namespace MapWindow.Geometries
{


    /// <summary>
    /// CoordinateArraySequenceEnumerator
    /// </summary>
    public class CoordinateArraySequenceEnumerator: IEnumerator<Coordinate>
    {
        #region Private Variables

        readonly IEnumerator _baseEnumerator;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CoordinateArraySequenceEnumerator
        /// </summary>
        public CoordinateArraySequenceEnumerator(IEnumerator inBaseEnumerator)
        {
            _baseEnumerator = inBaseEnumerator;

        }

        #endregion

       


        #region IEnumerator<ICoordinate> Members

        /// <summary>
        /// Gets the current member
        /// </summary>
        public Coordinate Current
        {
            get { return _baseEnumerator.Current as Coordinate; }
        }
        object IEnumerator.Current
        {
            get { return _baseEnumerator.Current; }
        }



        #endregion

        #region IDisposable Members

        /// <summary>
        /// Does nothing
        /// </summary>
        public void Dispose()
        {
          
        }

        #endregion

        #region IEnumerator Members

        /// <summary>
        /// Advances the enumerator to the next member
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            return _baseEnumerator.MoveNext();
        }

        /// <summary>
        /// Resets the enumerator to the original position
        /// </summary>
        public void Reset()
        {
            _baseEnumerator.Reset();
        }

        #endregion
    }
}
