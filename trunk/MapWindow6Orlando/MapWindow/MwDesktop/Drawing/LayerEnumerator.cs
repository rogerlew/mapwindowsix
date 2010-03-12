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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2008 4:36:33 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;

namespace MapWindow.Drawing
{


    /// <summary>
    /// LayerEnumerator
    /// </summary>
    public class LayerLegendEnumerator : IEnumerator<ILegendItem>
    {
        readonly IEnumerator<ILayer> _internalEnumerator;


        /// <summary>
        /// Creates a new instance of LayerEnumerator
        /// </summary>
        public LayerLegendEnumerator(IEnumerator<ILayer> source)
        {
            _internalEnumerator = source;
        }


        #region IEnumerator<ILegendItem> Members

        /// <summary>
        /// Retrieves the current member as an ILegendItem
        /// </summary>
        public ILegendItem Current
        {
            get { return _internalEnumerator.Current; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Calls the Dispose method
        /// </summary>
        public void Dispose()
        {
            _internalEnumerator.Dispose();
        }

        #endregion

        #region IEnumerator Members

        /// <summary>
        /// Returns the current member as an object
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return _internalEnumerator.Current; }
        }

        /// <summary>
        /// Moves to the next member
        /// </summary>
        /// <returns>boolean, true if the enumerator was able to advance</returns>
        public bool MoveNext()
        {
            return _internalEnumerator.MoveNext();
        }

        /// <summary>
        /// Resets to before the first member
        /// </summary>
        public void Reset()
        {
            _internalEnumerator.Reset();
        }

        #endregion
    }
}
