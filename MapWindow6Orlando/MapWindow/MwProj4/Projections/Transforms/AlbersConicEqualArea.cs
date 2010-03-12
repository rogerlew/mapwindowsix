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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/26/2009 2:50:35 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

namespace MapWindow.Projections
{


    /// <summary>
    /// AlbersConicEqualArea
    /// </summary>
    public class AlbersConicEqualArea : AlbersEqualArea
    {
    

        #region Constructors

        /// <summary>
        /// Creates a new instance of AlbersConicEqualArea
        /// </summary>
        public AlbersConicEqualArea()
        {
            Name = "Albers_Conic_Equal_Area";
            Proj4Name = "aea";
        }

        #endregion

    

    }
}
