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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/5/2009 1:12:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// The Projection interface that acts as a useful tool for calculating pixel coordinates
    /// to geographic coordinates and vise-versa.
    /// </summary>
    public interface IProj
    {
 

        #region Properties
        /// <summary>
        /// The Rectangle representation of the geographic extents in image coordinates
        /// </summary>
        Rectangle ImageRectangle
        {
            get;
        }
      

        /// <summary>
        /// The geographic extents
        /// </summary>
        IEnvelope GeographicExtents
        {
            get;
        }
        


        #endregion



    }
}
