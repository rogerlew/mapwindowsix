//********************************************************************************************************
// Product Name: MapWindow.Interfaces Alpha
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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System.Collections.Generic;

namespace MapWindow.Geometries
{
    /// <summary>
    /// This supports some of the basic data-related capabilities of a polygon, but no topology functions.
    /// Each of these uses the specifically different nomenclature so that the parallel concepts in a 
    /// full Polygon can return the appropriate datatype.  Since Polygon will Implement IPolygonBase, it
    /// is the responsibility of the developer to perform the necessary casts when returning this
    /// set from the more complete topology classes.
    /// </summary>
    public interface IBasicPolygon: IBasicGeometry
    {
        

        /// <summary>
        /// Gets the list of Interior Rings in the form of ILineStringBase objects
        /// </summary>
        ICollection<IBasicLineString> Holes
        {
            get; set;
        }


        /// <summary>
        /// Gets the exterior ring of the polygon as an ILineStringBase.
        /// </summary>
        IBasicLineString Shell
        {
            get; set;
        }

        /// <summary>
        /// Gets the count of holes or interior rings
        /// </summary>
        int NumHoles { get;}
    }
}
