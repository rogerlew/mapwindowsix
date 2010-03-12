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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2008 8:28:18 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.ComponentModel;
namespace MapWindow.Geometries
{


    /// <summary>
    /// ISize interface for expressing a length in the X, Y or Z directions
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface ISize
    {
        
        #region Properties

        /// <summary>
        /// Gets or sets the size in the x direction or longitude
        /// </summary>
        double XSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size in the y direction or latitude
        /// </summary>
        double YSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size in the z direction or altitude
        /// </summary>
        double ZSize
        {
            get;
            set;
        }


        #endregion



    }
}
