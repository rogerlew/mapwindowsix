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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/2/2010 10:53:04 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************



namespace MapWindow.Data
{


    /// <summary>
    /// WkbGeometryTypes
    /// </summary>
    public enum WkbGeometryTypes
    {
        /// <summary>
        /// Point.
        /// </summary>
        Point = 1,

        /// <summary>
        /// LineString.
        /// </summary>
        LineString = 2,

        /// <summary>
        /// Polygon.
        /// </summary>
        Polygon = 3,

        /// <summary>
        /// MultiPoint.
        /// </summary>
        MultiPoint = 4,

        /// <summary>
        /// MultiLineString.
        /// </summary>
        MultiLineString = 5,

        /// <summary>
        /// MultiPolygon.
        /// </summary>
        MultiPolygon = 6,

        /// <summary>
        /// GeometryCollection.
        /// </summary>
        GeometryCollection = 7


    }
}
