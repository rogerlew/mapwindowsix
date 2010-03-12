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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/26/2010 10:01:48 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Projections
{


    /// <summary>
    /// HotineObliqueMercatorAzimuthalCenter - Used to properly direct the Swiss Oblique Mercator 
    /// when it appears as Hotine Oblique Mercator Azimuthal Center.  At current, the Azimuth
    /// parameter is ignored as I don't know how to send it into Proj4.
    /// </summary>
    public class HotineObliqueMercatorAzimuthCenter : SwissObliqueMercator
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HotineObliqueMercatorAzimuthalCenter
        /// </summary>
        public HotineObliqueMercatorAzimuthCenter()
        {
            Name = "Hotine_Oblique_Mercator_Azimuth_Center";
        }

        #endregion




    }
}
