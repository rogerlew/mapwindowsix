//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using MapWindow.Analysis.Topology.Algorithm;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Projections;
using MapWindow.Serialization;
using MapWindow.Tools;

namespace MapWindow.Data
{
    /// <summary>
    /// Rather than being an uninstantiable base class, this is a "router" class that will
    /// make decisions based on the file extension or whatever being used and decide the best
    /// IFeatureSet that matches the specifications.
    /// </summary>
    public class FeatureSet: DataSet, IFeatureSet
    {

        #region Events
        /// <summary>
        /// Occurs when the vertices are invalidated, encouraging a re-draw
        /// </summary>
        public event EventHandler VerticesInvalidated;

        /// <summary>
        /// Occurs when a new feature is added to the list
        /// </summary>
        public event EventHandler<FeatureEventArgs> FeatureAdded;

        /// <summary>
        /// Occurs when a feature is removed from the list.
        /// </summary>
        public event EventHandler<FeatureEventArgs> FeatureRemoved;
        

        #endregion

        #region Variables


        private IEnvelope _envelope;

        private FeatureTypes _featureType;

        private IProgressHandler _progressHandler;

        private ProgressMeter _progressMeter;

        private DataTable _dataTable;

        private string _filename;

        IFeatureList _features;

        private List<ShapeRange> _shapeIndices;

        private double[] _vertices;
        private double[] _z;
        private double[] _m;

        private bool _verticesAreValid;

        private CoordinateTypes _coordinateType;

        private Dictionary<DataRow, IFeature> _featureLookup;
        private Extent _extent;
        private bool _indexMode;

        #endregion

        #region Constructors

        /// <summary>
        /// This doesn't do anything exactly because there is no file-specific information yet
        /// </summary>
        public FeatureSet()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new FeatureSet
        /// </summary>
        /// <param name="featureType">The Feature type like point, line or polygon</param>
        public FeatureSet(FeatureTypes featureType) : this()
        {
            _featureType = featureType;
        }


        /// <summary>
        /// This creates a new featureset by checking each row of the table.  If the WKB feature
        /// type matches the specifeid featureTypes, then it will copy that.
        /// </summary>
        /// <param name="wkbTable"></param>
        /// <param name="wkbColumnIndex"></param>
        /// <param name="indexed"></param>
        /// <param name="type"></param>
        public FeatureSet(DataTable wkbTable, int wkbColumnIndex, bool indexed, FeatureTypes type)
        {
            if(_indexMode)
            {
                // Assume this DataTable has WKB in column[0] and the rest of the columns are attributes.
                FeatureSetPack result = new FeatureSetPack();
                foreach (DataRow row in wkbTable.Rows)
                {
                    byte[] data = (byte[])row[0];
                    MemoryStream ms = new MemoryStream(data);
                    WkbFeatureReader.ReadFeature(ms, result);
                }
                // convert lists of arrays into a single vertex array for each shape type.
                result.StopEditing();
                // Make sure all the same columns exist in the same order
                result.Polygons.CopyTableSchema(wkbTable);
                // Assume that all the features happend to be polygons
                foreach (DataRow row in wkbTable.Rows)
                {
                    // Create a new row
                    DataRow dest = result.Polygons.DataTable.NewRow();
                    // 
                    dest.ItemArray = row.ItemArray;

                }
                
            }
            
        }
      

        /// <summary>
        /// Creates a new FeatureSet using a given list of IFeatures.
        /// This will copy the existing features, rather than removing
        /// them from their parent feature set.
        /// </summary>
        /// <param name="inFeatures">The list of IFeatures</param>
        public FeatureSet(IList<IFeature> inFeatures)
        {
            _progressHandler = Components.DataManager.DefaultDataManager.ProgressHandler;
            _progressMeter = new ProgressMeter(_progressHandler);
            _dataTable = new DataTable();
            _dataTable.RowDeleted += DataTableRowDeleted;
            
            if (inFeatures.Count > 0)
            {
                FeatureType = inFeatures[0].FeatureType;
            }
            _features = new FeatureList(this);
            if (inFeatures.Count > 0)
            {
                if (inFeatures[0].ParentFeatureSet != null)
                {
                    CopyTableSchema(inFeatures[0].ParentFeatureSet);
                }
                else
                {
                    if (inFeatures[0].DataRow != null)
                    {
                        CopyTableSchema(inFeatures[0].DataRow.Table);
                    }
                }
                _features.SuspendEvents(); 
                foreach (IFeature f in inFeatures)
                {
                    IFeature myFeature = f.Copy();
                    _features.Add(myFeature);
                }
                _features.ResumeEvents();
            }
        }

        void DataTableRowDeleted(object sender, DataRowChangeEventArgs e)
        {
            Features.Remove(_featureLookup[e.Row]);
            _featureLookup.Remove(e.Row);
        }

        /// <summary>
        /// Generates a new feature, adds it to the features and returns the value.
        /// </summary>
        /// <returns>The feature that was added to this featureset</returns>
        public IFeature AddFeature(IBasicGeometry geometry)
        {
            IFeature f = new Feature(geometry, this);
            return f;
        }


   

        private void Configure()
        {
            _indexMode = false; // this is false unless we are loading it from a specific shapefile case.
            _featureLookup = new Dictionary<DataRow, IFeature>();
            _progressHandler = Components.DataManager.DefaultDataManager.ProgressHandler;
            _progressMeter = new ProgressMeter(_progressHandler);
            _features = new FeatureList(this);
            _features.FeatureAdded += FeaturesFeatureAdded;
            _features.FeatureRemoved += FeaturesFeatureRemoved;
            _dataTable = new DataTable();
        }

        void FeaturesFeatureRemoved(object sender, FeatureEventArgs e)
        {
            _verticesAreValid = false;
            _featureLookup.Remove(e.Feature.DataRow);
            if (FeatureRemoved != null) FeatureRemoved(sender, e);
        }

        void FeaturesFeatureAdded(object sender, FeatureEventArgs e)
        {
            _verticesAreValid = false; // invalidate vertices
            if (e.Feature.DataRow == null) return;
            if(_featureLookup.ContainsKey(e.Feature.DataRow))
            {
                _featureLookup[e.Feature.DataRow] = e.Feature;
            }
            else
            {
                _featureLookup.Add(e.Feature.DataRow, e.Feature);
            }
            if (FeatureAdded != null) FeatureAdded(sender, e);
        }

   

        #endregion

        #region Methods

        /// <summary>
        /// Adds any type of list or array of shapes.  If this featureset is not in index moded,
        /// it will add the features to the featurelist and suspend events for faster copying.
        /// If it is, then we only build the vertex array once, rather than continually 
        /// rebuilding it.
        /// </summary>
        /// <param name="shapes">An enumerable collection of shapes.</param>
        public void AddShapes(IEnumerable<Shape> shapes)
        {
            if(InternalDataSet != null)
            {
                InternalDataSet.AddShapes(shapes);
            }
            if(!_indexMode)
            {
                _features.SuspendEvents();
                foreach (Shape shape in shapes)
                {
                    AddShape(shape);
                }
                _features.ResumeEvents();
                return;
            }
            
            bool vertsExist = _vertices != null;
            int count = vertsExist ? _vertices.Length / 2 : 0;
            int numPoints = count;
            bool hasZ = (_z != null);
            bool hasM = (_m != null);
            foreach (Shape shape in shapes)
            {
                numPoints += shape.Range.NumPoints;
                if(shape.M != null) hasM = true;
                if (shape.Z != null) hasZ = true;
            }
            double[] vertices = new double[numPoints*2];
            if (vertsExist) Array.Copy(_vertices, 0, vertices, 0, _vertices.Length);

            if(hasM)
            {
                double[] m = new double[numPoints];
                if (_m != null) Array.Copy(_m, 0, m, 0, _m.Length);
                _m = m;
            }
            if(hasZ)
            {
                double[] z = new double[numPoints];
                if  (_z != null) Array.Copy(_z, 0, z, 0, _z.Length);
                _z = z;
            }

            int offset = count;
            foreach (Shape shape in shapes)
            {
                int start = shape.Range.StartIndex;
                int num = shape.Range.NumPoints;
                if (shape.Vertices != null)
                {
                     if(shape.Vertices.Length - start >= num)Array.Copy(shape.Vertices, start*2, vertices, offset*2, num*2);
                }
           
                if(shape.M != null && (shape.M.Length - start) >= num)Array.Copy(shape.M, start, _m, offset, num);
                if(shape.Z != null && (shape.Z.Length - start) >= num)Array.Copy(shape.Z, start, _z, offset, num);
                ShapeRange newRange = shape.Range.Copy();
                newRange.StartIndex = offset;
                offset += num;
                ShapeIndices.Add(newRange);
            }
            Vertex = vertices;
            UpdateEnvelopes();
            
            
            
        }

        /// <summary>
        /// If this featureset is in index mode, this will append the vertices and shapeindex of the shape.
        /// Otherwise, this will add a new feature based on this shape.  If the attributes of the shape are not null,
        /// this will attempt to append a new datarow It is up to the developer
        /// to ensure that the object array of attributes matches the this featureset.  If the Attributes of this feature are loaded,
        /// this will add the attributes in ram only.  Otherwise, this will attempt to insert the attributes as a 
        /// new record using the "AddRow" method.  The schema of the object array should match this featureset's column schema.
        /// </summary>
        /// <param name="shape">The shape to add to this featureset.</param>
        public void AddShape(Shape shape)
        {
            if(InternalDataSet != null)
            {
                InternalDataSet.AddShape(shape);
                return;
            }
            IFeature addedFeature = null;
            
            // This first section controls the indices which need to happen regardless
            // because drawing uses indices even if editors like working with features.
            int count = (_vertices != null) ? _vertices.Length / 2 : 0; // Original number of points
            int totalCount = shape.Range.NumPoints + count;
            int start = shape.Range.StartIndex;
            int num = shape.Range.NumPoints;

            double[] vertices = new double[totalCount*2];
            if (_vertices != null) Array.Copy(_vertices, 0, vertices, 0, _vertices.Length);
            if(shape.Vertices != null) Array.Copy(shape.Vertices, start * 2, vertices, count * 2, num * 2);

            if(_m != null || shape.M != null)
            {
                double[] m = new double[totalCount];
                if (_m != null) Array.Copy(_m, 0, m, 0, _m.Length);
                if (shape.M != null) Array.Copy(shape.Vertices, start, m, count, num);
                _m = m;
            }
            if(_z != null || shape.Z != null)
            {
                double[] z = new double[totalCount];
                if (_z != null) Array.Copy(_z, 0, z, 0, _z.Length);
                if (shape.Z != null) Array.Copy(shape.Vertices, start, z, count, num);
                _z = z;
            }
            
            shape.Range.StartIndex = count;
            ShapeIndices.Add(shape.Range);
            Vertex = vertices;
            shape.Vertices = vertices; 
            if (Extent == null) Extent = new Extent();
            Extent.ExpandToInclude(shape.Range.Extent);
            Envelope = Extent.ToEnvelope();
            
            if(!_indexMode)
            {
                // Just add a new feature
                addedFeature = new Feature(shape);
                
                Features.Add(addedFeature);
                addedFeature.DataRow = AddAttributes(shape);
                if(Extent == null)Extent = new Extent();
                if (!shape.Range.Extent.IsEmpty()) Extent.ExpandToInclude(new Extent(addedFeature.Envelope));
                Envelope = Extent.ToEnvelope();
                
            }

            
            
            

            
        }
        /// <summary>
        /// handles the attributes while adding a shape
        /// </summary>
        /// <param name="shape"></param>
        /// <returns>A data row, but only if attributes are populated</returns>
        private DataRow AddAttributes(Shape shape)
        {
            // Handle attributes if the array is not null.  Assumes compatible schema.
            if (shape.Attributes != null)
            {
                DataColumn[] columns = GetColumns();
                Dictionary<string, object> rowContent = new Dictionary<string, object>();
                object[] fixedContent = new object[columns.Length];
                DataRow addedRow;
                if (shape.Attributes.Length != columns.Length)
                {
                    throw new ArgumentException("Attribute column count mismatch.");
                }
                for (int iField = 0; iField < columns.Length; iField++)
                {
                    object value = shape.Attributes[iField];
                    if (value != null)
                    {
                        if (columns[iField].DataType != value.GetType())
                        {
                            // this may throw an exception if the type casting fails
                            value = Convert.ChangeType(value, columns[iField].DataType);
                        }
                    }
                    fixedContent[iField] = value;
                    rowContent.Add(columns[iField].ColumnName, value);
                }
                if (AttributesPopulated)
                {
                    // just add a new datarow
                    addedRow = _dataTable.NewRow();
                    addedRow.ItemArray = fixedContent;
                    return addedRow;
                }
                // Insert a new row in the source 
                AddRow(rowContent);
            }
            return null;
        }
        
       

        /// <summary>
        /// Gets a shape at the specified shape index.  If the featureset is in 
        /// indexmode, this returns a copy of the shape.  If not, it will create
        /// a new shape based on the specified feature.
        /// </summary>
        /// <param name="index">The zero based integer index of the shape.</param>
        /// <param name="getAttributes">If getAttributes is true, then this also try to get attributes for that shape.
        /// If attributes are loaded, then it will use the existing datarow.  Otherwise, it will read the attributes
        /// from the file.  (This second option is not recommended for large repeats.  In such a case, the attributes
        /// can be set manually from a larger bulk query of the source attributes.)</param>
        /// <returns>The Shape object</returns>
        public virtual Shape GetShape(int index, bool getAttributes)
        {
            if (InternalDataSet != null)
            {
                return InternalDataSet.GetShape(index, getAttributes);
            }
            if (_indexMode == false) return new Shape(Features[index]);
            Shape result = new Shape(FeatureType);
            // This will also deep copy the parts, attributes and vertices
            ShapeRange range = ShapeIndices[index];
            result.Range = range.Copy();
            int start = range.StartIndex;
            int numPoints = range.NumPoints;
            double[] vertices = new double[numPoints * 2];
            
            double[] m = new double[numPoints];
            Array.Copy(_vertices, start*2, vertices, 0, numPoints * 2);
            if (_z != null && (_z.Length - start) >= numPoints)
            {
                result.Z = new double[numPoints];
                Array.Copy(_z, start, result.Z, 0, numPoints);
            }
            if(_m != null && (_m.Length - start) >= numPoints)
            {
                result.M = new double[numPoints];
                Array.Copy(_m, start, result.M, 0, numPoints);
            }
            result.Vertices = vertices;
         

            // There is presumed to be only a single shape in the output array.
            result.Range.StartIndex = 0;
            if(AttributesPopulated)
            {
                if (getAttributes) result.Attributes = DataTable.Rows[index].ItemArray;
            }
            else
            {
                DataTable dt = GetAttributes(index, 1);
                if(dt != null && dt.Rows.Count > 0)
                {
                    result.Attributes = dt.Rows[0].ItemArray;
                }
            }
            
            return result;
        }

        /// <summary>
        /// Gets the specified feature by constructing it from the vertices, rather
        /// than requiring that all the features be created. (which takes up a lot of memory).
        /// </summary>
        /// <param name="index">The integer index</param>
        public virtual IFeature GetFeature(int index)
        {
            if(InternalDataSet != null)
            {
                return InternalDataSet.GetFeature(index);
            }
            if (_indexMode == false) return Features[index];
            
            if(FeatureType == FeatureTypes.Point)
            {
                return GetPoint(index);
            }
            if(FeatureType == FeatureTypes.Line)
            {
                return GetLine(index);
            }
            if(_featureType == FeatureTypes.Polygon)
            {
                return GetPolygon(index);
            }
            if(_featureType == FeatureTypes.MultiPoint)
            {
                return GetMultiPoint(index);
            }
            return null;
        }

        protected IFeature GetMultiPoint(int index)
        {
            ShapeRange shape = ShapeIndices[index];
            List<Coordinate> coords = new List<Coordinate>();
            foreach (PartRange part in shape.Parts)
            {
                int i = part.StartIndex;
                foreach (Vertex vertex in part)
                {
                    Coordinate c = new Coordinate(vertex);
                    coords.Add(c);
                    if (M != null && M.Length != 0) c.M = M[i]; 
                    if (Z != null && Z.Length != 0) c.Z = Z[i];
                    i++;
                }
            }
            MultiPoint mp = new MultiPoint(coords);
            return new Feature(mp);
        }

        protected IFeature GetPoint(int index)
        {
            Coordinate c = new Coordinate(Vertex[index*2], Vertex[index*2 + 1]);
            if (M != null && M.Length != 0) c.M = M[index];
            if (Z != null && Z.Length != 0) c.Z = Z[index];
            Feature f = new Feature(new Point(c));
            f.ParentFeatureSet = this;
            f.RecordNumber = index;
            // Attributes only retrieved in the overridden case
            return f;
        }

        protected IFeature GetLine(int index)
        {
            ShapeRange shape = ShapeIndices[index];
            List<IBasicLineString> lines = new List<IBasicLineString>();
            foreach (PartRange part in shape.Parts)
            {
                int i = part.StartIndex;
                List<Coordinate> coords = new List<Coordinate>();
                foreach (Vertex d in part)
                {
                    Coordinate c = new Coordinate(d);
                    coords.Add(c);
                    if (M != null && M.Length > 0) c.M = M[i];
                    if (Z != null && Z.Length > 0) c.Z = Z[i];
                    i++;
                }
                lines.Add(new LineString(coords));
            }
            Feature f = shape.Parts.Count > 1 ? new Feature(new MultiLineString(lines)) : new Feature(lines[0]);
            f.ParentFeatureSet = this;
            f.ShapeIndex = shape;
            f.RecordNumber = shape.RecordNumber;
            f.ContentLength = shape.ContentLength;
            f.ShapeType = shape.ShapeType;
            // Attribute reading is only handled in the overridden case.
            return f;
        }

        /// <summary>
        /// If the FeatureType is polygon, this is the code for converting the vertex array
        /// into a feature.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected IFeature GetPolygon(int index)
        {
            Feature feature = new Feature();
            feature.Envelope = ShapeIndices[index].Extent.ToEnvelope();
            ShapeRange shape = ShapeIndices[index];
            List<ILinearRing> shells = new List<ILinearRing>();
            List<ILinearRing> holes = new List<ILinearRing>();
            foreach (PartRange part in shape.Parts)
            {
                List<Coordinate> coords = new List<Coordinate>();
                int i = part.StartIndex;
                foreach (Vertex d in part)
                {
                    Coordinate c = new Coordinate(d);
                    if (M != null && M.Length > 0) c.M = M[i];
                    if (Z != null && Z.Length > 0) c.Z = Z[i];
                    i++;
                    coords.Add(c);
                }
                LinearRing ring = new LinearRing(coords);
                if (shape.Parts.Count == 1)
                {
                    shells.Add(ring);
                }
                else
                {
                    if (CGAlgorithms.IsCounterClockwise(ring.Coordinates))
                    {
                        holes.Add(ring);
                    }
                    else
                    {
                        shells.Add(ring);
                    }
                    //if(part.IsHole())
                    //{
                    //    holes.Add(ring);
                    //}
                    //else
                    //{
                    //    shells.Add(ring);
                    //}
                }
            }
            //// Now we have a list of all shells and all holes
            List<ILinearRing>[] holesForShells = new List<ILinearRing>[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                holesForShells[i] = new List<ILinearRing>();
            }

            // Find holes
            for (int i = 0; i < holes.Count; i++)
            {
                ILinearRing testRing = holes[i];
                ILinearRing minShell = null;
                IEnvelope minEnv = null;
                IEnvelope testEnv = testRing.EnvelopeInternal;
                Coordinate testPt = testRing.Coordinates[0];
                ILinearRing tryRing;
                for (int j = 0; j < shells.Count; j++)
                {
                    tryRing = shells[j];
                    IEnvelope tryEnv = tryRing.EnvelopeInternal;
                    if (minShell != null)
                        minEnv = minShell.EnvelopeInternal;
                    bool isContained = false;

                    if (tryEnv.Contains(testEnv)
                        && (CGAlgorithms.IsPointInRing(testPt, tryRing.Coordinates)
                        || (PointInList(testPt, tryRing.Coordinates))))
                    {
                        isContained = true;
                    }

                    // Check if this new containing ring is smaller than the current minimum ring
                    if (isContained)
                    {
                        if (minShell == null || minEnv.Contains(tryEnv))
                        {
                            minShell = tryRing;
                        }
                        holesForShells[j].Add(holes[i]);
                    }
                }
            }

            IPolygon[] polygons = new Polygon[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                polygons[i] = new Polygon(shells[i], holesForShells[i].ToArray());
            }

            if (polygons.Length == 1)
            {
                feature.BasicGeometry = polygons[0];
            }
            else
            {
                // It's a multi part
                feature.BasicGeometry = new MultiPolygon(polygons);
            }
            // feature.FID = feature.RecordNumber; FID is now dynamic
            feature.ParentFeatureSet = this;
            feature.ShapeIndex = shape;
            feature.RecordNumber = shape.RecordNumber;
            feature.ContentLength = shape.ContentLength;
            feature.ShapeType = shape.ShapeType;
            //Attributes handled in the overridden case
            return feature;
        }

        /// <summary>
        /// Test if a point is in a list of coordinates.
        /// </summary>
        /// <param name="testPoint">TestPoint the point to test for.</param>
        /// <param name="pointList">PointList the list of points to look through.</param>
        /// <returns>true if testPoint is a point in the pointList list.</returns>
        private static bool PointInList(Coordinate testPoint, IEnumerable<Coordinate> pointList)
        {
            foreach (Coordinate p in pointList)
                if (p.Equals2D(testPoint))
                    return true;
            return false;
        }


        /// <summary>
        /// Given a datarow, this will return the associated feature.  This FeatureSet
        /// uses an internal dictionary, so that even if the items are re-ordered
        /// or have new members inserted, this lookup will still work.
        /// </summary>
        /// <param name="row">The DataRow for which to obtaind the feature</param>
        /// <returns>The feature to obtain that is associated with the specified data row.</returns>
        public IFeature FeatureFromRow(DataRow row)
        {
            return _featureLookup[row];
        }

        /// <summary>
        /// Copies all the features from the specified featureset.  
        /// </summary>
        /// <param name="source">The source IFeatureSet to copy features from.</param>
        /// <param name="copyAttributes">Boolean, true if the attributes should be copied as well.  If this is true,
        /// and the attributes are not loaded, a FillAttributes call will be made.</param>
        public void CopyFeatures(IFeatureSet source, bool copyAttributes)
        {
            ProgressMeter = new ProgressMeter(ProgressHandler, "Copying Features", ShapeIndices.Count);
            Vertex = source.Vertex.Copy();
            _shapeIndices = new List<ShapeRange>();
            foreach (ShapeRange range in source.ShapeIndices)
            {
                _shapeIndices.Add(range.Copy());
            }
            foreach (DataColumn dc in source.GetColumns())
            {
                if (dc != null)
                {
                    DataColumn outCol = new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping);
                    Field fld = new Field(outCol);
                    DataTable.Columns.Add(fld);
                }
            }
            if(source.AttributesPopulated)
            {
                // Handle data table content directly
                if(!IndexMode)
                {
                    // If not in index mode, just handle this using features
                    Features.SuspendEvents();
                    foreach (IFeature f in source.Features)
                    {
                        IFeature copy = AddFeature(f.BasicGeometry);
                        if (copyAttributes)
                        {
                            for (int col = 0; col < source.DataTable.Columns.Count; col++)
                            {
                                copy.DataRow[col] = f.DataRow[col];
                            }
                        }
                    }
                    Features.ResumeEvents();
                }
                else
                {
                    // We need to copy the attributes, but just copy a datarow
                    DataTable = source.DataTable.Copy();
                }
            }
            else
            {
                 AttributesPopulated = false;
                // Handle data table content directly
                if (!IndexMode)
                {
                    // If not in index mode, just handle this using features
                    Features.SuspendEvents();
                    foreach (IFeature f in source.Features)
                    {
                        AddFeature(f.BasicGeometry);
                    }
                    Features.ResumeEvents();
                }
                if(copyAttributes)
                {
                   
                    // We need to copy the attributes, but use the page system
                    int maxRow = NumRows();
                    const int pageSize = 10000;
                    int numPages = (int)Math.Ceiling(maxRow/(double)pageSize);
                    for(int i = 0; i < numPages; i++)
                    {
                        int numRows = pageSize;
                        if (i == numPages - 1) numRows = numPages - (pageSize*i);
                        DataTable dt = source.GetAttributes(i * pageSize, numRows);
                        SetAttributes(i*pageSize, dt);
                    }
                    
                }
            }
            
            
        }

        /// <summary>
        /// Copies only the names and types of the attribute fields, without copying any of the attributes or features.
        /// </summary>
        /// <param name="source">The source featureSet to obtain the schema from.</param>
        public void CopyTableSchema(IFeatureSet source)
        {
            DataTable.Columns.Clear();
            DataTable dt = source.DataTable;
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc != null)
                {
                    DataColumn outCol = new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping);
                    Field fld = new Field(outCol);
                    DataTable.Columns.Add(fld);
                }
            }
        }
        /// <summary>
        /// Copies the Table schema (column names/data types)
        /// from a DatatTable, but doesn't copy any values.
        /// </summary>
        /// <param name="sourceTable">The Table to obtain schema from.</param>
        public void CopyTableSchema(DataTable sourceTable)
        {
            DataTable.Columns.Clear();
            foreach (DataColumn dc in sourceTable.Columns)
            {
                if (dc != null)
                {
                    DataColumn outCol = new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping);
                    Field fld = new Field(outCol);
                    DataTable.Columns.Add(fld);
                }
            }
        }

        /// <summary>
        /// Retrieves a subset using exclusively the features matching the specified values.
        /// </summary>
        /// <param name="indices">An integer list of indices to copy into the new FeatureSet</param>
        /// <returns>A FeatureSet with the new items.</returns>
        public FeatureSet CopySubset(List<int> indices)
        {
            FeatureSet copy = MemberwiseClone() as FeatureSet;
            if (copy == null) return null;
            copy.Features = new FeatureList(copy);
            foreach (int row in indices)
            {
                copy.Features.Add(Features[row]);
            }
            copy.InvalidateEnvelope(); // the new set will likely have a different envelope bounds
            return copy;
            
        }
        IFeatureSet IFeatureSet.CopySubset(List<int> indices)
        {
            return CopySubset(indices);
        }

        /// <summary>
        /// Copies the subset of specified features to create a new featureset that is restricted to
        /// just the members specified.
        /// </summary>
        /// <param name="filterExpression">The string expression to test.</param>
        /// <returns>A FeatureSet that has members that only match the specified members.</returns>
        FeatureSet CopySubset(string filterExpression)
        {
            return CopySubset(Find(filterExpression));
        }
        IFeatureSet IFeatureSet.CopySubset(string filterExpression)
        {
            return CopySubset(filterExpression);
        }


        /// <summary>
        /// Obtains the list of feature indices that match the specified filter expression.
        /// </summary>
        /// <param name="filterExpression">The filter expression to find features for.</param>
        /// <returns>The list of integers that are the FIDs of the specified values.</returns>
        public List<int> Find(string filterExpression)
        {
            List<int> result = new List<int>();
            DataRow[] hits = _dataTable.Select(filterExpression);
            foreach (DataRow dr in hits)
            {
                result.Add(_dataTable.Rows.IndexOf(dr));
            }
            return result;
        }

        /// <summary>
        /// Launches a data dialog that allows the user to choose data.
        /// </summary>
        public virtual void Open()
        {
            InternalDataSet = Components.DataManager.DefaultDataManager.OpenVector();
        }

        /// <summary>
        /// This attempts to open the specified file as a valid IFeatureSet.  This will require that
        /// the default data manager can work with the file format at runtime.
        /// </summary>
        /// <param name="filename">The string filename for this featureset.</param>
        public void Open(string filename)
        {
            InternalDataSet = Components.DataManager.DefaultDataManager.OpenVector(filename, true, _progressHandler);
        }


        /// <summary>
        /// This instructs the default data manager to launch an open file dialog with the filter set to VectorReadFilter
        /// </summary>
        /// <returns></returns>
        public static IFeatureSet OpenFile()
        {
            return Components.DataManager.DefaultDataManager.OpenVector();
        }

        /// <summary>
        /// This will return the correct feature type by reading the filename.
        /// </summary>
        /// <param name="filename">A string specifying the file with the extension .shp to open.</param>
        /// <returns>A correct featureSet which is exclusively for reading the .shp data</returns>
        public static IFeatureSet OpenFile(string filename)
        {
            return Components.DataManager.DefaultDataManager.OpenFile(filename) as IFeatureSet;
        }

        /// <summary>
        /// Generates a new FeatuerSet, if possible, from the specified filename.
        /// </summary>
        /// <param name="filename">The string filename to attempt to load into a new FeatureSet.</param>
        /// <param name="progressHandler">An IProgressHandler for progress messages</param>
        public static IFeatureSet OpenFile(string filename, IProgressHandler progressHandler)
        {
            return Components.DataManager.DefaultDataManager.OpenFile(filename, true, progressHandler) as IFeatureSet;
            
        }

        /// <summary>
        /// Instructs the shapefile to read all the attributes from the file.
        /// This may also be a cue to read values from a database.
        /// </summary>
        public virtual void FillAttributes()
        {
            FillAttributes(_progressHandler);
        }

        /// <summary>
        /// Instructs the shapefile to read all the attributes from the file.
        /// This may also be a cue to read values from a database.
        /// </summary>
        public virtual void FillAttributes(IProgressHandler progressHandler)
        {
            // Not supported at this level
            if (InternalDataSet != null) InternalDataSet.FillAttributes();
        }

        /// <summary>
        /// This method simply invalidates the cached envelope, forcing a recalculation based on the current geometries.
        /// </summary>
        public void InvalidateEnvelope()
        {
            _envelope = null;
            
        }

        /// <summary>
        /// This forces the vertex initialization.
        /// </summary>
        public void InitializeVertices()
        {
            OnInitializeVertices();
        }

        /// <summary>
        /// Switches a boolean so that the next time that the vertices are requested,
        /// they must be re-calculated from the feature coordinates.
        /// </summary>
        /// <remarks>This only affects reading values from the Vertices cache</remarks>
        public void InvalidateVertices()
        {
            _verticesAreValid = false;
            OnVerticesInvalidated();
        }


        /// <summary>
        /// If this feature layer has a datasource, it will simply invoke the save method for that
        /// datasource.
        /// </summary>
        /// <exception cref="System.ApplicationException">The datasource or filename has not yet been specified for this layer.</exception>
        /// <exception cref="System.NotSupportedException">The specified extension is not currently supported.</exception>
        public void Save()
        {
            SaveAs(Filename, true);
        }

        /// <summary>
        /// Saves a datasource to the file.
        /// </summary>
        /// <param name="filename">The string filename location to save to</param>
        /// <param name="overwrite">Boolean, if this is true then it will overwrite a file of the existing name.</param>
        /// <exception cref="System.ApplicationException">A file of that name already exists and overwrite was set to false</exception>
        /// <exception cref="System.NotSupportedException">The specified extension is not currently supported.</exception>
        public virtual void SaveAs(string filename, bool overwrite)
        {
            if (InternalDataSet != null)
            {
                InternalDataSet.SaveAs(filename, overwrite);
            }
            else
            {
                if (FeatureType == FeatureTypes.Unspecified)
                {
                    if (Features.Count > 0)
                    {
                        FeatureType = Features[0].FeatureType;
                    }
                }
                IFeatureSet result = Components.DataManager.DefaultDataManager.CreateVector(filename, FeatureType, ProgressHandler);
                // Previously I prevented setting of the features list, but I think we can negate that decision.  No reason to prevent it that I can see.
                // Same for the ShapeIndices.  I'd like to simply set them here and prevent the hassle of a pure copy process.
                result.Vertex = Vertex;
                result.ShapeIndices = ShapeIndices;
                result.Extent = Extent;
                if(!IndexMode)
                {
                    result.Features = Features;
                }
                result.ProgressHandler = ProgressHandler;
                result.Projection = Projection;
                result.Save();
                InternalDataSet = result;
            }
        }

        /// <summary>
        /// Skips the features themselves and uses the shapeindicies instead.
        /// </summary>
        /// <param name="region">The region to select members from</param>
        /// <returns>A list of integer valued shape indices that are selected.</returns>
        public virtual List<int> SelectIndices(Extent region)
        {
            List<int> result = new List<int>();
            List<ShapeRange> shapes = ShapeIndices; // get from internal dataset if necessary
            if(shapes != null)
            {
                for (int shp = 0; shp < shapes.Count; shp++)
                {
                    if (_shapeIndices[shp].Extent.Intersects(region)) result.Add(shp);
                }
            }
            return result;
        }

        /// <summary>
        /// returns only the features that have envelopes that
        /// intersect with the specified envelope.
        /// </summary>
        /// <param name="region">The specified region to test for intersect with</param>
        /// <returns>A List of the IFeature elements that are contained in this region</returns>
        public virtual List<IFeature> Select(IEnvelope region)
        {
            IEnvelope ignoreMe;
            return Select(region, out ignoreMe);
        }

        /// <summary>
        /// returns only the features that have envelopes that
        /// intersect with the specified envelope.  If in indexMode, this uses the ShapeIndices to create
        /// features on demand, rather than loading all the features.  It is much faster to use selectIndices
        /// when in index mode.
        /// </summary>
        /// <param name="region">The specified region to test for intersect with</param>
        /// <param name="affectedRegion">This returns the geographic extents that contains the modified contents.</param>
        /// <returns>A List of the IFeature elements that are contained in this region</returns>
        public virtual List<IFeature> Select(IEnvelope region, out IEnvelope affectedRegion)
        {
            List<IFeature> result = new List<IFeature>();
            if(IndexMode)
            {
                ShapeRange aoi = new ShapeRange(new Extent(region));
                IEnvelope env = region.Copy();
                Extent affected = new Extent();
                List<ShapeRange> shapes = ShapeIndices;
                if(shapes != null)
                {
                    for(int shp = 0; shp < shapes.Count; shp++)
                    {
                        if (!shapes[shp].Intersects(aoi)) continue;
                        IFeature f = GetFeature(shp);
                        affected.ExpandToInclude(shapes[shp].Extent);
                        result.Add(f);
                    }
                }
                affectedRegion = affected.ToEnvelope();
                return result;
            }

            affectedRegion = new Envelope();

            foreach (IFeature feature in Features)
            {
                if (!feature.Envelope.Intersects(region)) continue;
                result.Add(feature);
                affectedRegion.ExpandToInclude(feature.Envelope);
            }
            return result;

        }

        /// <summary>
        /// This version is more tightly integrated to the DataTable and returns the row indices, rather
        /// than attempting to link the results to features themselves, which may not even exist.
        /// </summary>
        /// <param name="filterExpression">The filter expression</param>
        /// <returns>The list of indices</returns>
        public virtual List<int> SelectIndexByAttribute(string filterExpression)
        {
            if (InternalDataSet != null) return InternalDataSet.SelectIndexByAttribute(filterExpression);
            List<int> result = new List<int>();
            if(AttributesPopulated && DataTable != null)
            {
                DataRow[] rows = DataTable.Select(filterExpression);
                foreach (DataRow row in rows)
                {
                    result.Add(DataTable.Rows.IndexOf(row));
                }
            }
            else
            {
                // page in sets of 10,000 rows
                int numPages = (int)Math.Ceiling((double)NumRows()/10000);
                for(int page = 0; page < numPages; page++)
                {
                    DataTable table = GetAttributes(page*10000, 10000);
                    DataRow[] rows = table.Select(filterExpression);
                    foreach (DataRow row in rows)
                    {
                        result.Add(table.Rows.IndexOf(row) + page * 10000);
                    }
                }
            }
            return result;
        }
       

        /// <summary>
        /// Selects using a string filter expression to obtain the desired features.
        /// Field names should be in square brackets.  Alternately, if the field name of [FID]
        /// is used, then it will use the row index instead if no FID field is found.
        /// </summary>
        /// <param name="filterExpression">The features to return.</param>
        /// <returns>The list of desired features.</returns>
        public virtual List<IFeature> SelectByAttribute(string filterExpression)
        {
            List<IFeature> result = new List<IFeature>();
            DataTable dt = DataTable;
            bool isTemp = false;
            if(filterExpression != null)
            {
                if (filterExpression.Contains("[FID]") && DataTable.Columns.Contains("FID") == false)
                {
                    AddFid();
                    isTemp = true;
                }
            }
           
            DataRow[] rows = dt.Select(filterExpression);
            if(FeatureLookup != null)
            {
                foreach (DataRow dr in rows)
                {
                    result.Add(FeatureLookup[dr]);
                }
                
            }
           
            if(isTemp)dt.Columns.Remove("FID");
            return result;
        }

        /// <summary>
        /// Adds the FID values as a field called FID, but only if the FID field
        /// does not already exist
        /// </summary>
        public void AddFid()
        {
            DataTable dt = DataTable;
            if (dt.Columns.Contains("FID")) return;
            dt.Columns.Add("FID", typeof (int));
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["FID"] = i;
            }
        }
        
        #endregion


        #region Properties

       

        /// <summary>
        /// Gets or sets the coordinate type across the entire featureset.
        /// </summary>
        public CoordinateTypes CoordinateType
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.CoordinateType;
                return _coordinateType;
            }
            set
            {
                if(InternalDataSet != null)
                {
                    InternalDataSet.CoordinateType = value;
                    return;
                }
                _coordinateType = value;

            }

        }

        /// <summary>
        /// Gets whether or not the attributes have all been loaded into the data table.
        /// </summary>
        public virtual bool AttributesPopulated
        {
            get
            {
                // This is true by default, and only overridden in cases where we can grab attributes from a file
                return InternalDataSet == null || InternalDataSet.AttributesPopulated;
            }
            set
            {
                if(InternalDataSet != null)
                {
                    InternalDataSet.AttributesPopulated = true;
                }
           
            }
        }

        /// <summary>
        /// DataTable is the System.Data.DataTable for all the attributes of this FeatureSet.
        /// This will call FillAttributes if it is accessed and that has not yet been called.
        /// </summary>
        public virtual DataTable DataTable
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.DataTable;
                return _dataTable;
            }
            protected set
            {
                OnDataTableExcluded(_dataTable);
                _dataTable = value;
                OnDataTableIncluded(_dataTable);
            }
        }

        /// <summary>
        /// Allows the wiring of event handlers related to the data Table.
        /// </summary>
        /// <param name="dataTable"></param>
        protected virtual void OnDataTableIncluded(DataTable dataTable)
        {
            if (dataTable != null) dataTable.RowDeleted += DataTableRowDeleted;
        }

        /// <summary>
        /// Allows the un-wiring of event handlers related to the data Table.
        /// </summary>
        /// <param name="dataTable"></param>
        protected virtual void OnDataTableExcluded(DataTable dataTable)
        {
            if (dataTable != null) dataTable.RowDeleted -= DataTableRowDeleted;
        }

        /// <summary>
        /// Returns the bounding envelope that contains all
        /// the features for the entire featureset
        /// </summary>
        public virtual IEnvelope Envelope
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Envelope;
                }
                if(_indexMode)
                {
                    if (_extent == null) UpdateEnvelopes();
                }
                else
                {
                    if (_envelope == null) UpdateEnvelopes();
                }
                return _envelope;
            }
            protected set
            {
                _envelope = value;
            }
        }

        /// <summary>
        /// This is the envelope in Extent form.  This may be cached.
        /// </summary>
        public virtual Extent Extent
        {
            get
            {
                return InternalDataSet != null ? InternalDataSet.Extent : _extent;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Extent = value;
                }
                else
                {
                    _extent = value;  
                }
                
            }
        }

        /// <summary>
        /// A list of the features in this layer
        /// </summary>
        public virtual IFeatureList Features
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.Features;
                if(_indexMode && (_features == null || _features.Count == 0))
                {
                    // People working with features like this probably want to see changes from the features themselves.
                    FeaturesFromVertices();
                    _indexMode = false;
                }
                return _features;
            }
            set
            {
                OnExcludeFeatures(_features);
                _features = value;
                
            }
        }

        protected void FeaturesFromVertices()
        {
            _features = new FeatureList(this);
            _features.IncludeAttributes = false; // load these on demand later.
            _features.SuspendEvents();
            for (int shp = 0; shp < ShapeIndices.Count; shp++)
            {
                _features.Add(GetFeature(shp));
            }
            _features.ResumeEvents();
            _features.IncludeAttributes = true; // from this point on, any features that get added will also add a datarow to the data-Table.
        }

        /// <summary>
        /// If this is true, then the ShapeIndices and Vertex values are used,
        /// and features are created on demand.  Otherwise the list of Features
        /// is used directly.
        /// </summary>
        public bool IndexMode
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.IndexMode;
                return _indexMode;
            }
            set
            {
                if (InternalDataSet != null) InternalDataSet.IndexMode = value;
                _indexMode = value;
            }
        }

        /// <summary>
        /// Occurs when removing the feature list, allowing events to be disconnected
        /// </summary>
        /// <param name="features"></param>
        protected virtual void OnExcludeFeatures(IFeatureList features)
        {
            if (_features == null) return;
            _features.FeatureAdded -= FeaturesFeatureAdded;
            _features.FeatureRemoved -= FeaturesFeatureRemoved;
        }

        /// <summary>
        /// Occurs when setting the feature list, allowing events to be connected
        /// </summary>
        /// <param name="features"></param>
        protected virtual void OnIncludeFeatures(IFeatureList features)
        {
            if (_features == null) return;
            _features.FeatureAdded += FeaturesFeatureAdded;
            _features.FeatureRemoved += FeaturesFeatureRemoved;
        }

        /// <summary>
        /// Reprojects all of the in-ram vertices of this featureset.
        /// This will also update the projection to be the specified projection.
        /// </summary>
        /// <param name="targetProjection">The projection information to reproject the coordinates to.</param>
        public void Reproject(ProjectionInfo targetProjection)
        {
            if(InternalDataSet != null)InternalDataSet.Reproject(targetProjection);
            Projections.Reproject.ReprojectPoints(Vertex, Z, Projection, targetProjection, 0, Vertex.Length/2);
            UpdateCoordinates();
            UpdateEnvelopes();
            Projection = targetProjection;

        }

        ///// <summary>
        ///// Obsolete and slow.  First get the X and Y array once, and then access the members directly from that array.
        ///// </summary>
        //public double[][] Vertices
        //{
        //    get
        //    {
        //        int len = _vertices.Length/2;
        //        double[][] result = new double[len][];
        //        for (int i = 0; i < len; i++)
        //        {
        //            result[i] = new[] { _vertices[i*2], _vertices[i*2+1] };
        //        }
        //        return result;
        //    }
        //    protected set
        //    {
        //        for(int i = 0; i < value.Length; i++)
        //        {
        //            _vertices[i*2] = value[i][0];
        //            _vertices[i*2 + 1] = value[i][1];
        //        }
        //    }
        //}

        /// <summary>
        /// After changing coordinates, this will force the re-calculation of envelopes on a feature
        /// level as well as on the featureset level.  If this featureset is in index mode, 
        /// this will update the shape extents instead.
        /// </summary>
        public void UpdateEnvelopes()
        {
            if (InternalDataSet != null)
            {
                InternalDataSet.UpdateEnvelopes();
                return;
            }
            _envelope = new Envelope();
            _extent = new Extent();
            if(!_indexMode)
            {
                if (Features == null || Features.Count <= 0)
                {
                    _extent = new Extent(-180, -90, 180, 90);
                    _envelope = _extent.ToEnvelope();
                    return;
                }
                for (int shp = 0; shp < Features.Count; shp++)
                {
                    Features[shp].UpdateEnvelope();
                    _envelope.ExpandToInclude(Features[shp].Envelope);
                }
                _extent = new Extent(_envelope);
            }
            else
            {
                if(_shapeIndices == null || _shapeIndices.Count == 0)
                {
                    _extent = new Extent(-180, -90, 180, 90);
                    _envelope = _extent.ToEnvelope();
                    return;
                }
                foreach (ShapeRange range in _shapeIndices)
                {
                    _extent.ExpandToInclude(range.Extent);
                }
                _envelope = _extent.ToEnvelope();
            }
            
        }

    
        /// <summary>
        /// Gets the feature lookup Table itself.
        /// </summary>
        public Dictionary<DataRow, IFeature> FeatureLookup
        {
            get
            {
                if(InternalDataSet != null)
                {
                    return InternalDataSet.FeatureLookup;
                }
                return _featureLookup;
            }
        }

        /// <summary>
        /// Gets or sets an enumeration specifying whether this 
        /// featureset contains Lines, Points, Polygons or an 
        /// unspecified type.
        /// </summary>
        public FeatureTypes FeatureType
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.FeatureType;
                }
                return _featureType;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.FeatureType = value;
                    return;
                }
                _featureType = value;
            }
            
        }

     

        /// <summary>
        /// Many file formats do not permit multiple layers.  In such cases, no datasource
        /// is needed, and the feature layer can specify a filename to work with directly.
        /// Exceptions will be thrown if the filename specified is not one of the valid
        /// formats that supports single layer files.
        /// </summary>
		[Serialize("Filename", ConstructorArgumentIndex = 0)]
        public virtual string Filename
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.Filename;
                return _filename;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Filename = value;
                    return;
                }
                _filename = value;
            }
        }

       

        /// <summary>
        /// Gets or sets the progress handler to use for methods called by this featureset.
        /// Setting this resets the progress meter to work with the new handler.
        /// </summary>
        public virtual IProgressHandler ProgressHandler
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.ProgressHandler;
                return _progressHandler;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.ProgressHandler = value;
                    return;
                }
                _progressHandler = value;
                _progressMeter = new ProgressMeter(_progressHandler);
            }
        }

        /// <summary>
        /// Gets or sets a progress meter that is directly connected to the progress handler
        /// </summary>
        protected virtual ProgressMeter ProgressMeter
        {
            get { return _progressMeter; }
            set { _progressMeter = value; }
        }

    

        /// <summary>
        /// These specifically allow the user to make sense of the Vertices array.  These are 
        /// fast acting sealed classes and are not meant to be overridden or support clever
        /// new implementations.
        /// </summary>
        public List<ShapeRange> ShapeIndices
        {
            get 
            {
                if(InternalDataSet != null)
                {
                    return InternalDataSet.ShapeIndices;
                }
                if (_shapeIndices == null) OnInitializeVertices();
                return _shapeIndices;
            }
            set
            {
                if (InternalDataSet != null) InternalDataSet.ShapeIndices = value;
                _shapeIndices = value;
            }
        }

        /// <summary>
        /// Gets an array of Vertex structures with X and Y coordinates
        /// </summary>
        public double[] Vertex
        {
            get 
            {
                if(InternalDataSet != null)
                {
                    return InternalDataSet.Vertex;
                }
                if (_verticesAreValid == false)
                {
                    OnInitializeVertices();
                }
                return _vertices;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Vertex = value;
                }
                _vertices = value;
                if(_shapeIndices != null)
                {
                    foreach (ShapeRange shape in _shapeIndices)
                    {
                        shape.SetVertices(_vertices);
                    }
                }
                _verticesAreValid = true;
            }
        }

       

        public double[] Z
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Z;
                }
                if (_verticesAreValid == false)
                {
                    OnInitializeVertices();
                }
                return _z;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Z = value;
                }
                _z = value;
            }
        }

        public double[] M
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.M;
                }
                if (_verticesAreValid == false)
                {
                    OnInitializeVertices();
                }
                return _m;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.M = value;
                }
                _m = value;
            }
        }

        /// <summary>
        /// Gets a boolean that indicates whether or not the InvalidateVertices has been called
        /// more recently than the cached vertex array has been built.
        /// </summary>
        public bool VerticesAreValid
        {
            get
            {
                if(InternalDataSet != null)
                {
                    return InternalDataSet.VerticesAreValid;
                }
                return _verticesAreValid;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the internal dataset, specifically as an IFeatureSet
        /// </summary>
        protected new IFeatureSet InternalDataSet
        {
            get { return base.InternalDataSet as IFeatureSet; }
            set { base.InternalDataSet = value; }
        }

        /// <summary>
        /// Fires the VerticesInvalidated event
        /// </summary>
        protected virtual void OnVerticesInvalidated()
        {
            if (VerticesInvalidated != null) VerticesInvalidated(this, new EventArgs());
        }

        /// <summary>
        /// This forces the cached vertices array to be copied back to the individual X and Y values of the coordinates themselves.
        /// </summary>
        private void UpdateCoordinates()
        {
            int i = 0;
            foreach (IFeature f in _features)
            {
                IList<Coordinate> coords = f.Coordinates; // this should be all the coordinates, for all parts of the geometry.

                if (coords == null) continue;
                foreach (Coordinate c in coords)
                {
                    c.X = _vertices[i*2];
                    c.Y = _vertices[i*2+1];
                    i++;
                }
            }
        }

        


        /// <summary>
        /// Occurs when the vertices are being re-calculated.
        /// </summary>
        protected virtual void OnInitializeVertices()
        {
            int count = 0;
            foreach (IFeature f in _features)
            {
                count += f.NumPoints;
            }
            _vertices = new double[count*2];
            int i = 0;
            foreach(IFeature f in _features)
            {
                IList<Coordinate> coords = f.Coordinates; // this should be all the coordinates, for all parts of the geometry.

                if (coords == null) continue;
                foreach (Coordinate c in coords)
                {
                    _vertices[i*2] = c.X;
                    _vertices[i*2+1] = c.Y;
                    // vertexValues.Add(c.Values); // essentially add a reference pointer to the internal array of values
                    i++;
                }
            }
            //_vertices = vertexValues.ToArray(); // not sure, but I bet arrays a smidge faster at indexed access than lists

             
            _shapeIndices = new List<ShapeRange>();
            int vIndex = 0;
            for(int shp = 0; shp < _features.Count; shp++) 
            {
                IFeature f = _features[shp];
                

                ShapeRange shx = new ShapeRange(FeatureType);
                shx.Extent = new Extent(f.Envelope);
                _shapeIndices.Add(shx);
                f.ShapeIndex = shx;
               
                // for simplicity in looping, there is always at least one part. 
                // That way, the shape range can be ignored and the parts loop used instead.
                shx.Parts = new List<PartRange>();
                int shapeStart = vIndex;
                for (int part = 0; part < f.NumGeometries; part++)
                {
                    PartRange prtx = new PartRange(_vertices, shapeStart, vIndex-shapeStart, FeatureType);
                    IBasicPolygon bp = f.GetBasicGeometryN(part) as IBasicPolygon;
                    if(bp != null)
                    {
                        // Account for the Shell
                        prtx.NumVertices = bp.Shell.NumPoints;
                        
                        vIndex += bp.Shell.NumPoints;
                        // The part range should be adjusted to no longer include the holes
                        foreach (var hole in bp.Holes)
                        {
                            PartRange holex = new PartRange(_vertices, shapeStart, vIndex - shapeStart, FeatureType);
                            holex.NumVertices = hole.NumPoints;
                            shx.Parts.Add(holex);
                            vIndex += hole.NumPoints;
                        }
                    }
                    else
                    {
                        int numPoints = f.GetBasicGeometryN(part).NumPoints;
                        
                        // This is not a polygon, so just add the number of points.
                        vIndex += numPoints;
                        prtx.NumVertices = numPoints;
                    }
                    shx.Parts.Add(prtx);
                }
                

            }
           // _vertices = vertexValues.ToArray();
            _verticesAreValid = true;
        }



        /// <summary>
        /// Gets the count of members that match the expression
        /// </summary>
        /// <param name="expressions">The array of string expressions to test</param>
        /// <param name="progressHandler">The form or control to show progress, or allow cancelation</param>
        /// <param name="maxSampleSize">The integer maximum sample size from which to draw counts.  If this is negative, it will not be used.</param>
        /// <returns>The array of integer counts of the memebrs that match the expressions.</return>
        public virtual int[] GetCounts(string[] expressions, ICancelProgressHandler progressHandler, int maxSampleSize)
        {
            if (InternalDataSet != null) return InternalDataSet.GetCounts(expressions, progressHandler, maxSampleSize);
            int[] counts = new int[expressions.Length];
            for (int i = 0; i < expressions.Length; i++ )
            {
                counts[i] = DataTable.Select(expressions[i]).Length;
            }
            return counts;
        }




        #region IDataPageRetriever Members

        /// <summary>
        /// saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The values as demarked by the field names holding the new values to store.</param>
        public virtual void Edit(int index, Dictionary<string, object> values)
        {
            //overridden in sub-classes
            return;
        }

        /// <summary>
        /// saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The object array holding the new values to store.</param>
        public virtual void Edit(int index, DataRow values)
        {
           
        }

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        public virtual void AddRow(Dictionary<string, object> values)
        { 
            // overridden in sub-class
            return;
        }

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        public virtual void AddRow(DataRow values)
        {
            // overridden in sub-class
            return;
        }

        public virtual DataTable GetAttributes(int startIndex, int numRows)
        {
            // overridden in sub-classes
            return null;
        }

        /// <summary>
        /// Converts a page of content from a DataTable format, saving it back to the source.
        /// </summary>
        /// <param name="startIndex">The 0 based integer index representing the first row in the file (corresponding to the 0 row of the data table)</param>
        /// <param name="pageValues">The DataTable representing the rows to set.  If the row count is larger than the dataset, this will add the rows instead.</param>
        public virtual void SetAttributes(int startIndex, DataTable pageValues)
        {
            // overridden in sub-classes
            return;
        }

        public virtual int NumRows()
        {
            // overridden in sub-classes to prevent relying on an in-memory data table.
            if (InternalDataSet != null) return InternalDataSet.NumRows();
            return DataTable.Rows.Count;
        }

        /// <summary>
        /// Gets the column with the specified name
        /// </summary>
        /// <param name="name">the name to search</param>
        /// <returns>the Field matching the specified name, or null if the name is not in the list of columns.</returns>
        public DataColumn GetColumn(string name)
        {
            DataColumn[] columns = GetColumns();
            foreach (DataColumn field in columns)
            {
                if (field.ColumnName == name) return field;
            }
            return null;
        }

        /// <summary>
        /// Gets a copy of the columns.  This is useful for reading information about the columns,
        /// but doesn't allow editing of the original content.
        /// </summary>
        /// <returns>A list of Columns.</returns>
        public virtual DataColumn[] GetColumns()
        {
            if (InternalDataSet != null) return InternalDataSet.GetColumns();
            List<DataColumn> columns = new List<DataColumn>();
            foreach (DataColumn column in DataTable.Columns)
            {
                columns.Add(new DataColumn(column.ColumnName, column.DataType));   
            }
            return columns.ToArray();
            // overridden in sub-classes
            return null;
        }

        #endregion
    }
}
