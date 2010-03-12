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
// The Initial Developer of this Original Code is Ted Dunsford. 2/17/2008 5:00:07 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Drawing
{
    /// <summary>
    /// Some of the more common relationships between elevation and geographic coordinates
    /// </summary>
    public enum ElevationScenarios 
    {
        /// <summary>
        /// The elevation values are in centimeters, but the geographic projection uses decimal degrees
        /// </summary>
        ElevationCentiMeters_ProjectionDegrees,
        /// <summary>
        /// The elevation values are in centimeters, but the geographic projection uses Meters
        /// </summary>
        ElevationCentiMeters_ProjectionMeters,
        /// <summary>
        /// The elevation values are in centimeters, but the geographic projection uses Feet
        /// </summary>
        ElevationCentiMeters_ProjectionFeet,
        /// <summary>
        /// The elevation values are in feet, but the geographic projection uses decimal degrees
        /// </summary>
        ElevationFeet_ProjectionDegrees,
        /// <summary>
        /// The elevation values are in feet, but the geographic projection uses meters
        /// </summary>
        ElevationFeet_ProjectionMeters,
        /// <summary>
        /// The elevation values are in feet, but the geographic projection uses feet
        /// </summary>
        ElevationFeet_ProjectionFeet,
        /// <summary>
        /// The elevation values are in meters, but the geographic projection uses decimal degrees
        /// </summary>
        ElevationMeters_ProjectionDegrees,
        /// <summary>
        /// The elevation values are in meters, but the geographic projection uses meters
        /// </summary>
        ElevationMeters_ProjectionMeters,
        /// <summary>
        /// The elevation values are in meters, but the geographic projection uses feet
        /// </summary>
        ElevationMeters_ProjectionFeet
       
    
    }


}
