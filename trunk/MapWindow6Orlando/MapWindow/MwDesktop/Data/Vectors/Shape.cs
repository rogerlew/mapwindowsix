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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/6/2010 11:28:53 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using MapWindow.Geometries;

namespace MapWindow.Data
{


    /// <summary>
    /// The shape caries information about the raw vertices as well as a shapeRange.
    /// It is effectively away to move around a single shape.
    /// </summary>
    public class Shape : ICloneable
    {
        #region Private Variables

        private ShapeRange _shapeRange;
        private double[] _vertices;
        private double[] _z;
        private double[] _m;
        private object[] _attributes;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Shape
        /// </summary>
        public Shape()
        {

        }

        /// <summary>
        /// Creates a new shape type where the shaperange exists and has a type specified
        /// </summary>
        /// <param name="type"></param>
        public Shape(FeatureTypes type)
        {
            _shapeRange = new ShapeRange(type);
        }

        /// <summary>
        /// Creates a shape based on the specified feature.  This shape will be standing alone,
        /// all by itself.  The fieldnames and field types will be null.
        /// </summary>
        /// <param name="feature"></param>
        public Shape(IFeature feature)
        {
            _shapeRange = new ShapeRange(feature.FeatureType);
            _attributes = feature.DataRow.ItemArray;
            IList<Coordinate> coords = feature.Coordinates;
            _vertices = new double[feature.NumPoints*2];
            _z = new double[feature.NumPoints];
            _m = new double[feature.NumPoints];
            for (int i = 0; i < coords.Count; i++)
            {
                Coordinate c = coords[i];
                _vertices[i*2] = c.X;
                _vertices[i*2 + 1] = c.Y;
                _z[i] = c.Z;
                _m[i] = c.M;
            }
            int offset = 0;
            for(int ig = 0; ig < feature.NumGeometries; ig++)
            {
                IBasicGeometry g = feature.GetBasicGeometryN(ig);
                PartRange prt = new PartRange(_vertices, 0, offset, feature.FeatureType);
                _shapeRange.Parts.Add(prt);
                offset += g.NumPoints;
            }
        }

       
        #endregion

        #region Methods



        /// <summary>
        /// Copies the field names and types from the parent feature set if they are currently null.
        /// Attempts to copy the members of the feature's datarow.  This assumes the features have been
        /// loaded into memory and are available on the feature's DataRow property.
        /// </summary>
        /// <param name="feature">An IFeature to copy the attributes from.  If the schema is null, this will try to use the parent featureset schema.</param>
        public void CopyAttributes(IFeature feature)
        {

            object[] dr = feature.DataRow.ItemArray;
            _attributes =new object[dr.Length];
            Array.Copy(dr, _attributes, dr.Length);
        }
        
        /// <summary>
        /// In cases where the attributes are not in ram yet, this can obtain only the attributes for the
        /// specified shape.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        public void CopyAttributes(IFeatureSet source, int index)
        {
            
        }


        #endregion

        #region Properties

        /// <summary>
        /// Without changing the feature type or anything else, simply update the local coordinates
        /// to include the new coordinates.  All the new coordinates will be considered one part.
        /// Since point and multi-point shapes don't have parts, they will just be appended to the
        /// original part.
        /// </summary>
        public void AddPart(IEnumerable<Coordinate> coordinates, CoordinateTypes coordType)
        {
            bool hasM = (coordType == CoordinateTypes.M || coordType == CoordinateTypes.Z);
            bool hasZ = (coordType == CoordinateTypes.Z);
            List<double> vertices = new List<double>();
            List<double> z = new List<double>();
            List<double> m = new List<double>();
            int numPoints = 0;
            int oldNumPoints = (_vertices != null) ? _vertices.Length/2: 0;
            foreach (Coordinate coordinate in coordinates)
            {
                if(_shapeRange.Extent == null)_shapeRange.Extent = new Extent();
                _shapeRange.Extent.ExpandToInclude(coordinate.X, coordinate.Y);
                vertices.Add(coordinate.X);
                vertices.Add(coordinate.Y);
                if(hasM)m.Add(coordinate.M);
                if(hasZ)z.Add(coordinate.Z);
                numPoints++;
            }
            // Using public accessor also updates individual part references
            Vertices = (_vertices != null) ? _vertices.Concat(vertices).ToArray() : vertices.ToArray();
            if(hasZ) _z = (_z != null) ? _z.Concat(z).ToArray() : z.ToArray();
            if(hasM) _m = (_m != null) ? _m.Concat(m).ToArray() : m.ToArray();

            if(_shapeRange.FeatureType == FeatureTypes.MultiPoint || _shapeRange.FeatureType == FeatureTypes.Point)
            {
                // Only one part exists
                _shapeRange.Parts[0].NumVertices += numPoints;
            }
            else
            {
                PartRange part = new PartRange(_vertices, _shapeRange.StartIndex, oldNumPoints, _shapeRange.FeatureType );
                part.NumVertices = numPoints;
                _shapeRange.Parts.Add(part);

            }    
        }


        /// <summary>
        /// Gives a way to cycle through the vertices of this shape.
        /// </summary>
        public ShapeRange Range
        {
            get { return _shapeRange; }
            set { _shapeRange = value; }
        }

        /// <summary>
        /// The double vertices in X1, Y1, X2, Y2, ... ,Xn, Yn order.
        /// </summary>
        public double[] Vertices
        {
            get { return _vertices; }
            set
            {
                _vertices = value;
                foreach (PartRange part in _shapeRange.Parts)
                {
                    part.Vertices = value;
                }
            }
        }

        /// <summary>
        /// The Z values if any
        /// </summary>
        public double[] Z
        {
            get { return _z; }
            set { _z = value; }
        }

        /// <summary>
        /// The M values if any, organized in order.
        /// </summary>
        public double[] M
        {
            get { return _m; }
            set { _m = value; }
        }

        /// <summary>
        /// Gets or sets the attributes.  Since the most likely use is to copy values from one source to 
        /// another, this should be an independant array in each shape and be deep-copied.
        /// </summary>
        public object[] Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }


        #endregion




        #region ICloneable Members

        /// <summary>
        /// This creates a duplicate shape, also copying the vertex array to
        /// a new array containing just this shape, as well as duplicating the attribute array.
        /// The FieldNames and FieldTypes are a shallow copy since this shouldn't change.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {

            if (_shapeRange == null) return new Shape();

            Shape copy = (Shape)MemberwiseClone();
            int numPoints = _shapeRange.NumPoints;
            int start = _shapeRange.StartIndex;
            
            copy.Range = _shapeRange.Copy();
            copy.Attributes = new object[_attributes.Length];
            // Be sure to set vertex array AFTER the shape range to update part indices correctly.
            double[] verts = new double[numPoints * 2];
            Array.Copy(_vertices, start, verts, 0, numPoints * 2);
            copy.Vertices = verts;
            if (_z != null && (_z.Length - start) >= numPoints)
            {
                copy.Z = new double[numPoints];
                Array.Copy(_z, start, copy._z, 0, numPoints);
            }
            if (_m != null && (_m.Length - start) >= numPoints)
            {
                copy.M = new double[numPoints];
                Array.Copy(_m, start, copy._m, 0, numPoints);
            }
            // Update the start-range to work like a stand-alone shape.
            copy.Range.StartIndex = 0;
            Array.Copy(_attributes, 0, copy._attributes, 0, _attributes.Length);
            return copy;

        }

       
      

        #endregion
    }
}
