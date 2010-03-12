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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2008 9:45:35 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
namespace MapWindow.GeoMap
{

    /// <summary>
    /// ZoneEventArgs
    /// </summary>
    public class ZoneEventArgs : EventArgs
    {
        #region Private Variables

     
        Zones _zone;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ZoneEventArgs
        /// </summary>
        public ZoneEventArgs(Zones inZone)
        {
            _zone = inZone;
        }

        #endregion

    

        #region Properties

        /// <summary>
        /// Gets the zone for thiz event
        /// </summary>
        public Zones Zone
        {
            get { return _zone; }
            protected set { _zone = value; }
        }


        #endregion



    }
}
