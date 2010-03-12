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
using System;
using System.Collections.Generic;

namespace MapWindow.Geometries
{
    /// <summary>
    /// This is a lightweight version of the components that represent the strictly data related
    /// commands.
    /// </summary>
    public interface IBasicGeometry: ICloneable
    {

        /// Coordinates
        /// <summary>
        /// Using an IList guarantees that we can access indexed coordinates, but the actual implementation
        /// can be either in the form of an array or a list.
        /// </summary>
        IList<Coordinate> Coordinates { get; set; }

        /// <summary>
        /// This returns the index'th BasicGeometry where index is between 0 and NumGeometries - 1.
        /// If there is only one geometry, this will return this object.
        /// </summary>
        /// <param name="index">An integer index between 0 and NumGeometries - 1 specifying the basic geometry</param>
        /// <returns>A BasicGeometry representing only the specific sub-geometry specified</returns>
        IBasicGeometry GetBasicGeometryN(int index);

        /// <summary>
        /// Returns this Geometry's bounding box. If this Geometry is the empty point,
        /// returns an empty Point. If the Geometry is a point, returns a non-empty Point.
        /// Otherwise, returns a Polygon whose points are (minx, miny), (maxx, miny), 
        /// (maxx, maxy), (minx, maxy), (minx, miny).
        /// </summary>
        IEnvelope Envelope { get; }

        /// <summary>
        /// Returns the string that is the valid GML markup string.
        /// </summary>
        /// <returns>A String containing the valid GML</returns>
        string ExportToGML();

      

        /// <summary>
        /// Returns the Well-known Binary representation of this <c>Geometry</c>.
        /// For a definition of the Well-known Binary format, see the OpenGIS Simple
        /// Features Specification.
        /// </summary>
        /// <returns>The Well-known Binary representation of this <c>Geometry</c>.</returns>
        byte[] ToBinary();


        /// ToString
        /// <summary>
        /// Returns the Well-known Text representation of this <c>Geometry</c>.
        /// For a definition of the Well-known Text format, see the OpenGIS Simple
        /// Features Specification.
        /// </summary>
        /// <returns>
        /// The Well-known Text representation of this <c>Geometry</c>.
        /// </returns>
        string ToString();

        /// NumGeometries
        /// <summary>
        /// Returns the number of Geometries in a Geometry Collection, or 1, if the geometry is not a collection
        /// </summary>
        int NumGeometries { get; }

        /// NumPoints
        /// <summary>
        /// Returns the count of this geometry's vertices.  The geometries contained by
        /// composite geometries must be geometries.  That is, they must implement NumPoints.
        /// </summary>
        int NumPoints { get; }

        /// <summary>
        /// Clarifies the subtype of geometry in string format in accordance with 
        /// OGC convenction, but most internal identification simply uses
        /// the type identification.
        /// </summary>
        string GeometryType { get; }

        /// <summary>
        /// Specifies either a Point, Line, Polygon or Empty feature type for simple flow control
        /// </summary>
        FeatureTypes FeatureType { get; }

        /// <summary>
        /// Forces the cached envelope to be re-calculated using the coordinates.
        /// </summary>
        void UpdateEnvelope();
      
    }
}
