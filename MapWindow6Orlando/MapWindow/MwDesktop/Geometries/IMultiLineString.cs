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

namespace MapWindow.Geometries
{
    /// <summary>
    /// A type specific Geometry collection that deals with ILineStrings
    /// </summary>
    public interface IMultiLineString: IGeometryCollection
    {
        /// <summary>
        /// Changes the default indexer to assume that the members are ILineString instead of simply IGeometry
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        new ILineString this[int index] { get; }
        
    }
}
