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
using System.Collections;
namespace MapWindow.Geometries
{
    /// <summary>
    /// Specific topology functions for Mutigeometry code
    /// </summary>
    public interface IGeometryCollection: IGeometry, IEnumerable
    {
        /// <summary>
        /// Returns the number of geometries contained by this <see cref="IGeometryCollection" />.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the iTh <see cref="IGeometry"/> element in the collection.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        IGeometry this[int i]
        {
            get;
        }

        /// <summary>
        /// Return <c>true</c> if all features in collection are of the same type.
        /// </summary>
        bool IsHomogeneous { get;}

        /// <summary>
        /// Gets a System.Array of all the geometries in this collection
        /// </summary>
        IGeometry[] Geometries { get; set; }
    }
}
