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
    /// <c>GeometryCollection</c> classes support the concept of
    /// applying a <c>IGeometryFilter</c> to the <c>Geometry</c>.
    /// The filter is applied to every element <c>Geometry</c>.
    /// A <c>IGeometryFilter</c> can either record information about the <c>Geometry</c>
    /// or change the <c>Geometry</c> in some way.
    /// <c>IGeometryFilter</c> is an example of the Gang-of-Four Visitor pattern.
    /// </summary>
    public interface IGeometryFilter
    {
        /// <summary>
        /// Performs an operation with or on <c>geom</c>.
        /// </summary>
        /// <param name="geom">A <c>Geometry</c> to which the filter is applied.</param>
        void Filter(IGeometry geom);
    }
}
