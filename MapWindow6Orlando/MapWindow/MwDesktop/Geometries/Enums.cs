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
        /// Buffer styles.
        /// </summary>
        public enum BufferStyles
        {
            /// <summary> 
            /// Specifies a round line buffer end cap endCapStyle (Default).
            /// </summary>/
            CapRound = 1,

            /// <summary> 
            /// Specifies a butt (or flat) line buffer end cap endCapStyle.
            /// </summary>
            CapButt = 2,

            /// <summary>
            /// Specifies a square line buffer end cap endCapStyle.
            /// </summary>
            CapSquare = 3,
        }

        
        /// <summary>
        /// Constants representing the dimensions of a point, a curve and a surface.
        /// Also, constants representing the dimensions of the empty point and
        /// non-empty geometries, and a wildcard dimension meaning "any dimension".
        /// </summary>
        public enum Dimensions
        {
            /// <summary>
            /// Dimension value of a point (0).
            /// </summary>
            Point = 0,

            /// <summary>
            /// Dimension value of a curve (1).
            /// </summary>
            Curve = 1,

            /// <summary>
            /// Dimension value of a surface (2).
            /// </summary>
            Surface = 2,

            /// <summary>
            /// Dimension value of a empty point (-1).
            /// </summary>
            False = -1,

            /// <summary>
            /// Dimension value of non-empty geometries (= {Point,Curve,A}).
            /// </summary>
            True = -2,

            /// <summary>
            /// Dimension value for any dimension (= {False, True}).
            /// </summary>
            Dontcare = -3
        }

        /// <summary>
        /// This is enumerates not only the specific types, but very specifically
        /// that the types are the Topology variety, and not simply the vector variety
        /// </summary>
        public enum GeometryTypes
        {
            /// <summary>
            /// A non-geometry rectangle that strictly shows extents
            /// </summary>
            Envelope,
            /// <summary>
            /// A collective group of geometries
            /// </summary>
            GeometryCollection,
            /// <summary>
            /// A closed linestring that doesn't self intersect
            /// </summary>
            LinearRing,
            /// <summary>
            /// Any linear collection of points joined by line segments
            /// </summary>
            LineString,
            /// <summary>
            /// A collection of independant LineStrings that are not connected
            /// </summary>
            MultiLineString,
            /// <summary>
            /// A Grouping of points 
            /// </summary>
            MultiPoint,
            /// <summary>
            /// A Collection of unconnected polygons
            /// </summary>
            MultiPolygon,
            /// <summary>
            /// A single coordinate location
            /// </summary>
            Point,
            /// <summary>
            /// At least one Linear Ring shell with any number of linear ring "holes"
            /// </summary>
            Polygon,
            /// <summary>
            /// Any other type
            /// </summary>
            Unknown
        }

    /// <summary>
    /// A list of DE-9IM row indices clarifying the interior, boundary or exterior.
    /// </summary>
    public enum Locations
    {
        /// <summary>
        /// DE-9IM row index of the interior of the first point and column index of
        /// the interior of the second point. Location value for the interior of a
        /// point.
        /// int value = 0;
        /// </summary>
        Interior = 0,

        /// <summary>
        /// DE-9IM row index of the boundary of the first point and column index of
        /// the boundary of the second point. Location value for the boundary of a
        /// point.
        /// int value = 1;
        /// </summary>
        Boundary = 1,

        /// <summary>
        /// DE-9IM row index of the exterior of the first point and column index of
        /// the exterior of the second point. Location value for the exterior of a
        /// point.
        /// int value = 2;
        /// </summary>
        Exterior = 2,

        /// <summary>
        /// Used for uninitialized location values.
        /// int value = -1;
        /// </summary>
        Null = -1,
    }


    /// <summary>
    /// Standard ordinate index values.
    /// </summary>
    public enum Ordinates
    {
        /// <summary>
        /// X Ordinate = 0.
        /// </summary>
        X = 0,

        /// <summary>
        /// Y Ordinate = 1.
        /// </summary>
        Y = 1,

        /// <summary>
        /// Z Ordinate = 2.
        /// </summary>
        Z = 2,

        /// <summary>
        /// M Ordinate = 3.
        /// </summary>
        M = 3,
    }


    /// <summary>
    /// The types of Precision Model which NTS supports.
    /// </summary>
    public enum PrecisionModels
    {
        /// <summary> 
        /// Floating precision corresponds to the standard 
        /// double-precision floating-point representation, which is
        /// based on the IEEE-754 standard
        /// </summary>
        Floating = 0,

        /// <summary>
        /// Floating single precision corresponds to the standard
        /// single-precision floating-point representation, which is
        /// based on the IEEE-754 standard
        /// </summary>
        FloatingSingle = 1,

        /// <summary> 
        /// Fixed Precision indicates that coordinates have a fixed number of decimal places.
        /// The number of decimal places is determined by the log10 of the scale factor.
        /// </summary>
        Fixed = 2,
    }

    /// <summary>
    /// An abreviated list for quick classification 
    /// </summary>
    public enum FeatureTypes
    {
        /// <summary>
        /// None specified or custom
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Point
        /// </summary>
        Point = 1,

        /// <summary>
        /// Line
        /// </summary>
        Line = 2,

        /// <summary>
        /// Polygon
        /// </summary>
        Polygon = 3,

        /// <summary>
        /// MultiPoint
        /// </summary>
        MultiPoint = 4
    }


    /// <summary>
    /// Shapefile Shape types enumeration
    /// </summary>
    public enum ShapeGeometryTypes
    {
        /// <summary>
        /// Null Shape
        /// </summary>
        NullShape = 0,

        /// <summary>
        /// Point
        /// </summary>
        Point = 1,

        /// <summary>
        /// LineString
        /// </summary>
        LineString = 3,

        /// <summary>
        /// Polygon
        /// </summary>
        Polygon = 5,

        /// <summary>
        /// MultiPoint
        /// </summary>
        MultiPoint = 8,

        /// <summary>
        /// PointMZ
        /// </summary>
        PointZM = 11,

        /// <summary>
        /// PolyLineMZ
        /// </summary>
        LineStringZM = 13,

        /// <summary>
        /// PolygonMZ
        /// </summary>
        PolygonZM = 15,

        /// <summary>
        /// MultiPointMZ
        /// </summary>
        MultiPointZM = 18,

        /// <summary>
        /// PointM
        /// </summary>
        PointM = 21,

        /// <summary>
        /// LineStringM
        /// </summary>
        LineStringM = 23,

        /// <summary>
        /// PolygonM
        /// </summary>
        PolygonM = 25,

        /// <summary>
        /// MultiPointM
        /// </summary>
        MultiPointM = 28,

        /// <summary>
        /// MultiPatch
        /// </summary>
        MultiPatch = 31,

        /// <summary>
        /// PointZ
        /// </summary>
        PointZ = 9,

        /// <summary>
        /// LineStringZ
        /// </summary>
        LineStringZ = 10,

        /// <summary>
        /// PolygonZ
        /// </summary>
        PolygonZ = 19,

        /// <summary>
        /// MultiPointZ
        /// </summary>
        MultiPointZ = 20,
    }        
    

    /// <summary>
    /// Field Types
    /// </summary>
    public enum VectorFieldType
    {
        // from GDAL 1.3.1
        /// <summary>
        /// Integer
        /// </summary>
        OFTInteger = 0,
        /// <summary>
        /// Integer List
        /// </summary>
        OFTIntegerList = 1,
        /// <summary>
        /// Real
        /// </summary>
        OFTReal = 2,
        /// <summary>
        /// Real List
        /// </summary>
        OFTRealList = 3,
        /// <summary>
        /// String
        /// </summary>
        OFTString = 4,
        /// <summary>
        /// String List
        /// </summary>
        OFTStringList = 5,
        /// <summary>
        /// Wide String
        /// </summary>
        OFTWideString = 6,
        /// <summary>
        /// Side String List
        /// </summary>
        OFTWideStringList = 7,
        /// <summary>
        /// Binary
        /// </summary>
        OFTBinary = 8,
        /// <summary>
        /// Date
        /// </summary>
        OFTDate = 9,
        /// <summary>
        /// Time
        /// </summary>
        OFTTime = 10,

        /// <summary>
        /// DateTime
        /// </summary>
        OFTDateTime = 11,
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid = -1
    };

    /// <summary>
    /// Vector Geometry Types
    /// </summary>
    public enum VectorGeometryType : long
    {
        // subset from GDAL 1.3.1
        /// <summary>
        /// Unknown
        /// </summary>
        wkbUnknown = 0,             /* non-standard */
        /// <summary>
        /// Well Known Binary Point
        /// </summary>
        wkbPoint = 1,               /* rest are standard WKB type codes */
        /// <summary>
        /// Well Known Binary LineString
        /// </summary>
        wkbLineString = 2,

        /// <summary>
        /// Well Known Binary Polygon
        /// </summary>
        wkbPolygon = 3,
        /// <summary>
        /// Well Known Binary MultiPoint
        /// </summary>
        wkbMultiPoint = 4,
        /// <summary>
        /// Well Known Binary MultiLineString
        /// </summary>
        wkbMultiLineString = 5,

        /// <summary>
        /// Well Known Binary MultiPolygon
        /// </summary>
        wkbMultiPolygon = 6,

        /// <summary>
        /// Well Known Binary Geometry Collection
        /// </summary>
        wkbGeometryCollection = 7,

        /// <summary>
        /// Well Known Binary Linear Ring
        /// /* non-standard, just for createGeometry() */
        /// </summary>
        wkbLinearRing = 101,        
        
        /// <summary>
        /// Well Known Binary Point with Z value
        /// </summary>
        wkbPoint25D = (long)0x80000001,   /* 2.5D extensions as per 99-402 */
        /// <summary>
        /// Well Known Binary Line String with Z values
        /// </summary>
        wkbLineString25D = (long)0x80000002,
        
        /// <summary>
        /// Well Known Binary Polygon with Z values
        /// </summary>
        wkbPolygon25D = (long)0x80000003,

        /// <summary>
        /// Well Known Binary MultiPoint with Z values
        /// </summary>
        wkbMultiPoint25D = (long)0x80000004,

        /// <summary>
        /// Well Known Binary LineString with z values
        /// </summary>
        wkbMultiLineString25D = (long)0x80000005,

        /// <summary>
        /// Well Known Binary MultiPolygon with z values
        /// </summary>
        wkbMultiPolygon25D = (long)0x80000006
    };

       
}
