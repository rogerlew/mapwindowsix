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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/16/2009 1:28:16 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.Collections.Generic;
using MapWindow.Analysis;
using MapWindow.Geometries;


namespace MapWindow.Data
{


    /// <summary>
    /// ShapeIndex
    /// </summary>
    public sealed class ShapeRange : ICloneable
    {
        #region Private Variables

        private int _startIndex;
        private int _numPoints;
        private Extent _extent;

        /// <summary>
        /// The starting index for the entire shape range.
        /// </summary>
        public int StartIndex
        {
            get { return _startIndex; }
            set
            {
                NumPoints = 0;
                foreach (PartRange part in Parts)
                {
                    part.ShapeOffset = value;
                    NumPoints += part.NumVertices;
                }
                _startIndex = value;
            }
        }

        /// <summary>
        /// Creates a shallow copy of evertying except the parts list and extent, which are deep copies.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            ShapeRange copy = (ShapeRange)MemberwiseClone();
            copy.Parts = new List<PartRange>();
            foreach (PartRange part in Parts)
            {
                copy.Parts.Add(part.Copy());
            }
            copy.Extent = Extent.Copy();
            return copy;
        }

        

        
        /// <summary>
        /// If this is null, then there is only one part for this ShapeIndex.
        /// </summary>
        public List<PartRange> Parts;

        /// <summary>
        /// Gets or sets the extent of this shape range.  Setting this will prevent
        /// the automatic calculations.
        /// </summary>
        public Extent Extent
        {
            get
            {
                if (_extent == null) _extent = CalculateExtents();
                return _extent;
            }
            set
            {
                _extent = value;
            }
        }

        /// <summary>
        /// The record number   
        /// </summary>
        public int RecordNumber;
        /// <summary>
        /// The content length  
        /// </summary>
        public int ContentLength;
        /// <summary>
        /// The shape type for the header of this shape
        /// </summary>
        public ShapeTypes ShapeType;

        /// <summary>
        /// The number of points in the entire shape
        /// </summary>
        public int NumPoints
        {
            get
            {
                if(_numPoints <  0)
                {
                    int n = 0;
                    foreach (PartRange part in Parts)
                    {
                        n += part.NumVertices;
                    }
                    return n;
                }
                return _numPoints;
            }
            set { _numPoints = value; }
        }

        
        private int _numParts;

        /// <summary>
        /// The feature type
        /// </summary>
        public readonly FeatureTypes FeatureType;

        /// <summary>
        /// Control the epsilon to use for the intersect calculations
        /// </summary>
        public double Epsilon = double.Epsilon;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a blank instance of a shaperange where vertices can be assigned later.
        /// </summary>
        public ShapeRange(FeatureTypes type)
        {
            FeatureType = type;
            Parts = new List<PartRange>();
            _numParts = -1; // default to relying on the parts list instead of the cached value.
            _numPoints = -1; // rely on accumulation from parts instead of a solid number
        }

        /// <summary>
        /// Creates a new "point" shape that has only the one point.
        /// </summary>
        /// <param name="v"></param>
        public ShapeRange(Vertex v)
        {
            FeatureType = FeatureTypes.Point;
            Parts = new List<PartRange>();
            _numParts = -1;
            double[] coords = new double[2];
            coords[0] = v.X;
            coords[1] = v.Y;
            PartRange prt = new PartRange(coords, 0,0, FeatureTypes.Point);
            prt.NumVertices = 1;
            Parts.Add(prt);

        }

        /// <summary>
        /// Creates a shaperange
        /// </summary>
        /// <param name="env"></param>
        public ShapeRange(IEnvelope env)
        {
            Extent = new Extent(env);
            Parts = new List<PartRange>();
            _numParts = -1;
            // Counter clockwise
            // 1 2
            // 4 3
            double[] coords = new double[8];
            // C1
            coords[0] = Extent.XMin;
            coords[1] = Extent.YMax;
            // C2
            coords[2] = Extent.XMax;
            coords[3] = Extent.YMax;
            // C3
            coords[4] = Extent.XMax;
            coords[5] = Extent.YMin;
            // C4
            coords[6] = Extent.XMin;
            coords[7] = Extent.YMin;

            FeatureType = FeatureTypes.Polygon;
            ShapeType = ShapeTypes.Polygon;
            PartRange pr = new PartRange(coords, 0,0, FeatureTypes.Polygon);
            pr.NumVertices = 4;
            Parts.Add(pr);
        }

        /// <summary>
        /// This creates a polygon shape from an extent. 
        /// </summary>
        /// <param name="ext">The extent to turn into a polygon shape.</param>
        public ShapeRange(Extent ext)
        {
            Extent = ext;
            Parts = new List<PartRange>();
            _numParts = -1;
            // Counter clockwise
            // 1 2
            // 4 3
            double[] coords = new double[8];
            // C1
            coords[0] = ext.XMin;
            coords[1] = ext.YMax;
            // C2
            coords[2] = ext.XMax;
            coords[3] = ext.YMax;
            // C3
            coords[4] = ext.XMax;
            coords[5] = ext.YMin;
            // C4
            coords[6] = ext.XMin;
            coords[7] = ext.YMin;

            FeatureType = FeatureTypes.Polygon;
            ShapeType = ShapeTypes.Polygon;
            PartRange pr = new PartRange(coords, 0,0, FeatureTypes.Polygon);
            pr.NumVertices = 4;
            Parts.Add(pr);
            
        }


        #endregion

        #region Methods

        /// <summary>
        /// Forces each of the parts to adopt an extent equal to a calculated extents.
        /// The extents for the shape will expand to include those.
        /// </summary>
        public Extent CalculateExtents()
        {
            Extent ext = new Extent();
            foreach (PartRange part in Parts)
            {
                ext.ExpandToInclude(part.CalculateExtent());
            }
            Extent = ext;
            return ext;
        }

        /// <summary>
        /// Gets the first vertex from the first part.
        /// </summary>
        /// <returns></returns>
        public Vertex First()
        {
            double[] verts = Parts[0].Vertices;
            Vertex result = new Vertex(verts[StartIndex], verts[StartIndex+1]);
            return result;
        }


        /// <summary>
        /// This calculations processes the intersections
        /// </summary>
        /// <param name="shape">The shape to do intersection calculations with.</param>
        public bool Intersects(ShapeRange shape)
        {
            // Extent check first.  If the extents don't intersect, then this doesn't intersect.
            if (!Extent.Intersects(shape.Extent)) return false;
            
            switch (FeatureType)
            {
                case FeatureTypes.Polygon:
                    PolygonShape.Epsilon = Epsilon;
                    return PolygonShape.Intersects(this, shape);
                case FeatureTypes.Line:
                    LineShape.Epsilon = Epsilon;
                    return LineShape.Intersects(this, shape);
                case FeatureTypes.Point:
                    PointShape.Epsilon = Epsilon;
                    return PointShape.Intersects(this, shape);
                default:
                    return false;
            }

        }

      
        /// <summary>
        /// This sets the vertex array by cycling through each part index and updates.
        /// </summary>
        /// <param name="vertices">The double array of vertices that should be referenced by the parts.</param>
        public void SetVertices(double[] vertices)
        {
            foreach (PartRange prt in Parts)
            {
                prt.Vertices = vertices;
            }
        }

        /// <summary>
        /// Given a vertex, this will determine the part that the vertex is within.
        /// </summary>
        /// <param name="vertexOffset"></param>
        /// <returns></returns>
        public int PartIndex(int vertexOffset)
        {
            int i = 0;
            foreach (PartRange part in Parts)
            {
                if(part.StartIndex <= vertexOffset && part.EndIndex >= vertexOffset) return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// gets the integer end index as calculated from the number of points and the start index
        /// </summary>
        /// <returns></returns>
        public int EndIndex()
        {
            return StartIndex + NumPoints - 1;
        }

        /// <summary>
        /// If this is set, then it will cache an integer count that is independant from Parts.Count.
        /// If this is not set, (or set to a negative value) then getting this will return Parts.Count
        /// </summary>
        public int NumParts
        {
            get
            {
                if (_numParts < 0) return Parts.Count;
                return _numParts;
            }
            set
            {
                _numParts = value;
            }
        }

        #endregion

     



    }
}
