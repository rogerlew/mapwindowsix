//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 12:53:17 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Data
{


    /// <summary>
    /// RasterEnumerator
    /// </summary>
    internal class ShortRasterEnumerator : IEnumerator<ShortRaster>, IEnumerator<IRaster>
    {


        #region Private Variables

        IEnumerator<ShortRaster> _enumrator;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RasterEnumerator
        /// </summary>
        /// <param name="list">The list to build an enumarator for</param>
        public ShortRasterEnumerator(List<ShortRaster> list)
        {
            _enumrator = list.GetEnumerator();

        }

        #endregion


        #region IEnumerator<ShortRaster> Members

        /// <summary>
        /// Retrieves the current ShortRaster from this calculator.
        /// </summary>
        public ShortRaster Current
        {
            get { return _enumrator.Current; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes any unmanaged memory objects
        /// </summary>
        public void Dispose()
        {
            _enumrator.Dispose();
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return _enumrator.Current as object; }
        }

        /// <summary>
        /// Advances the enumerator to the next member.
        /// </summary>
        /// <returns>A boolean which is false if there are no more members in the list.</returns>
        public bool MoveNext()
        {
            return _enumrator.MoveNext();
        }

        /// <summary>
        /// Resets the enumerator to the position before the start of the list.
        /// </summary>
        public void Reset()
        {
            _enumrator.Reset();
        }

        #endregion

        #region IEnumerator<IRaster> Members


        IRaster IEnumerator<IRaster>.Current
        {
            get { return _enumrator.Current as IRaster; }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            _enumrator.Dispose();
        }

        #endregion

        #region IEnumerator Members


        bool System.Collections.IEnumerator.MoveNext()
        {
            return _enumrator.MoveNext();
        }

        void System.Collections.IEnumerator.Reset()
        {
            _enumrator.Reset();
        }

        #endregion
    }
}
