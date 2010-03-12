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
using System.Collections.Generic;
namespace MapWindow.Geometries
{
    /// <summary>
    /// Factory for Geometry stuff
    /// </summary>
    public interface IGeometryFactory
    {

        #region Public Properties

        /// <summary>
        /// Non-static accessor for what should probably be static.
        /// </summary>
        IGeometryFactory Default
        {
            get;
            set;
        }

        /// <summary>
        /// Floating reference
        /// </summary>
        IGeometryFactory Floating
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        IGeometryFactory FloatingSingle
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int SRID
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        PrecisionModels PrecisionModel
        {
            get;
        }

       
        #endregion

        /// <summary>
        /// CoordinateSequenceFactory that can manufacture a coordinate sequence
        /// </summary>
        ICoordinateSequenceFactory CoordinateSequenceFactory { get;}
        /// <summary>
        /// Generic constructor that parses a list and tries to form a working 
        /// object that implements MapWindow.Interfaces.IGeometry
        /// </summary>
        /// <param name="geomList">some list of things</param>
        /// <returns>An object that implements MapWindow.Geometries.IGeometry</returns>
        IGeometry BuildGeometry(IList geomList);

        /// <summary>
        /// Method to produce a point given a coordinate
        /// </summary>
        /// <param name="coord">An object that implements MapWindow.Geometries.ICoordinate</param>
        /// <returns>An object that implements MapWindow.Geometries.IPoint</returns>
        IPoint CreatePoint(Coordinate coord);


        /// <summary>
        /// Creates a new object that implements MapWindow.Geometries.MultiLineString
        /// given an array of objects that implement MapWindow.Geometries.ILineStringBase
        /// </summary>
        /// <param name="lineStrings">The Array of objects that implement MapWindow.Geometries.IlineStringBase </param>
        /// <returns>A new MultiLineString that implements IMultiLineString</returns>
        IMultiLineString CreateMultiLineString(IBasicLineString[] lineStrings);

        /// <summary>
        /// Creates an object that implements MapWindow.Geometries.IGeometryCollection
        /// from an array of objects that implement MapWindow.Geometries.IGeometry
        /// </summary>
        /// <param name="geometries">An array of objects that implement MapWindow.Geometries.IGeometry</param>
        /// <returns>A new object that implements MapWindow.Geometries.IGeometryCollection</returns>
        IGeometryCollection CreateGeometryCollection(IGeometry[] geometries);

        /// <summary>
        /// Creates an object that implements MapWindow.Geometries.IMultiPolygon from an array of
        /// objects that implement MapWindow.Geometries.IPolygon
        /// </summary>
        /// <param name="polygons">An Array of objects that implement MapWindow.Geometries.IPolygon</param>
        /// <returns>An object that implements MapWindow.Geometries.IMultiPolygon</returns>
        IMultiPolygon CreateMultiPolygon(IPolygon[] polygons);

        /// <summary>
        /// Creates an object that implements MapWindow.Geometries.ILinearRing from an array of MapWindow.Geometries.ICoordinates
        /// </summary>
        /// <param name="coordinates">An array of objects that implement ICoordinate</param>
        /// <returns>An object that implements MapWindow.Geometries.ILinearRing</returns>
        ILinearRing CreateLinearRing(IList<Coordinate> coordinates);


        /// <summary>
        /// Creates an object that implements MapWindow.Geometries.IMultiPoint from an array of objects that implement MapWindow.Geometries.ICoordinate
        /// </summary>
        /// <param name="coordinates">An array of objects that implement MapWindow.Geometries.ICoordinate</param>
        /// <returns>An object that implements MapWindow.Geometries.IMultiPoint</returns>
        IMultiPoint CreateMultiPoint(IEnumerable<ICoordinate> coordinates);

            /// <summary>
        /// Creates an object that implements MapWindow.Geometries.IMultiPoint from an array of objects that implement MapWindow.Geometries.ICoordinate
        /// </summary>
        /// <param name="coordinates">An array of objects that implement MapWindow.Geometries.ICoordinate</param>
        /// <returns>An object that implements MapWindow.Geometries.IMultiPoint</returns>
        IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> coordinates);

        /// <summary>
        /// Creates an object that implements MapWindow.Geometries.ILineString from an array of objects that implement MapWindow.Geometries.ICoordinate
        /// </summary>
        /// <param name="coordinates">An array of objects that implement MapWindow.Geometries.ICoordinate</param>
        /// <returns>A MapWindow.Geometries.ILineString</returns>
        ILineString CreateLineString(IList<Coordinate> coordinates);


        /// <summary>
        /// Creates an object that implements MapWindow.Geometries.IGeometry that is a copy
        /// of the specified object that implements MapWindow.Geometries.IGeometry
        /// </summary>
        /// <param name="g">An object that implements MapWindow.Geometries.IGeometry</param>
        /// <returns>An object that implements MapWindow.Geometries.IGeometry</returns>
        IGeometry CreateGeometry(IGeometry g);

        /// <summary>
        /// Creates an object that implements MapWindow.Geometries.IPolygon given a specified
        /// MapWindow.Geometries.ILinearRing shell and an array of
        /// MapWindow.Geometries.ILinearRing that represent the holes
        /// </summary>
        /// <param name="shell">The outer perimeter of the polygon, represented by an object that implements MapWindow.Geometries.ILinearRing</param>
        /// <param name="holes">The interior holes in the polygon, represented by an array of objects that implements MapWindow.Geometries.ILinearRing</param>
        /// <returns>An object that implements MapWindow.Geometries.IPolygon</returns>
        IPolygon CreatePolygon(ILinearRing shell, ILinearRing[] holes);
        
      
    }
}
