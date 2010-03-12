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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/19/2008 11:01:48 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using MapWindow.Drawing;

namespace MapWindow.Map
{


    /// <summary>
    /// LayerEnumerator
    /// </summary>
    public class GeoLayerEnumerator : IEnumerator<IMapLayer>
    {
        IEnumerator<ILayer> _internalEnumerator;


        /// <summary>
        /// Creates a new instance of LayerEnumerator
        /// </summary>
        public GeoLayerEnumerator(IEnumerator<ILayer> source)
        {
            _internalEnumerator = source;
        }

       

        /// <summary>
        /// Retrieves the current member as an ILegendItem
        /// </summary>
        public IMapLayer Current
        {
            get 
            {
                return _internalEnumerator.Current as IMapLayer; 
            }
        }
        object System.Collections.IEnumerator.Current
        {
            get { return _internalEnumerator.Current as object; }
        }

       

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
        /// Moves to the next member
        /// </summary>
        /// <returns>boolean, true if the enumerator was able to advance</returns>
        public bool MoveNext()
        {
            while (_internalEnumerator.MoveNext())
            {
                IMapLayer result = _internalEnumerator.Current as IMapLayer;
                if (result != null) return true;
            }
            return false;
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
