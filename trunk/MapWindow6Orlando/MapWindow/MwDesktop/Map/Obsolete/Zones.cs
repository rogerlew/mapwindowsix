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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/7/2008 3:06:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.GeoMap
{


    /// <summary>
    ///  ______________________
    /// |      |  North |      |
    /// |      |--------|      |
    /// | West | Center | East |
    /// |      |--------|      |
    /// |      |  South |      |
    ///  ----------------------
    /// </summary>
    public enum Zones
    {
        /// <summary>
        /// The center zone is usually what is shown most often and updates first
        /// </summary>
        Center = 0,
        /// <summary>
        /// The north zone is only above the center zone and does not extend further east or west
        /// </summary>
        North = 1,
        /// <summary>
        /// The east zone extends all the way from the bottom to the top and is the same width as the center zone
        /// </summary>
        East = 2,
        /// <summary>
        /// The south zone is only below the center
        /// </summary>
        South = 3,
        /// <summary>
        /// The western zone extends all the way from teh bottom to the top and is the same width as the center zone.
        /// </summary>
        West = 4,
        /// <summary>
        /// Disregard the current draw-by-zone concept
        /// </summary>
        None = -1


    }
}
