using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MapWindow.Geometries;
using MapWindow.Data.IO;

using MapWindow.Data;
using MapWindow.Analysis.DataManagement.Shapefile.dBase;
using MapWindow.Analysis.DataManagement.Shapefile;
using MapWindow.Analysis.Logging;

namespace MapWindow.Analysis.DataManagement.Shapefile.Shape
{
	/// <summary>
	/// This class writes ESRI Shapefiles.
	/// </summary>
    public class ShapeWriter
    {
        private IGeometryFactory geometryFactory = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileWriter" /> class 
        /// using <see cref="GeometryFactory.Default" /> with a <see cref="PrecisionModels.Floating" /> precision.
        /// </summary>
        public ShapeWriter() : this(new GeometryFactory().Default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileWriter" /> class
        /// with the given <see cref="GeometryFactory" />.
        /// </summary>
        /// <param name="geometryFactory"></param>
        public ShapeWriter(IGeometryFactory geometryFactory)
        {
            this.geometryFactory = geometryFactory;
        }

        /// <summary>
        /// Writes an entire shapefile to disk in one call using a geometryCollection.
        /// </summary>
        /// <remarks>
        /// Assumes the type given for the first geometry is the same for all subsequent geometries.
        /// For example, is, if the first Geometry is a Multi-polygon/ Polygon, the subsequent geometies are
        /// Muli-polygon/ polygon and not lines or points.
        /// The dbase file for the corresponding shapefile contains one column called row. It contains 
        /// the row number.
        /// </remarks>
        /// <param name="filename">The filename to write to (minus the .shp extension).</param>
        /// <param name="geometryCollection">The GeometryCollection to write.</param>		
        public virtual void Write(string filename, GeometryCollection geometryCollection)
        {
            System.IO.FileStream shpStream = new System.IO.FileStream(filename + ".shp", System.IO.FileMode.Create);
            System.IO.FileStream shxStream = new System.IO.FileStream(filename + ".shx", System.IO.FileMode.Create);
            BigEndianBinaryWriter shpBinaryWriter = new BigEndianBinaryWriter(shpStream);
            BigEndianBinaryWriter shxBinaryWriter = new BigEndianBinaryWriter(shxStream);

            // assumes
            ShapeHandler handler = Shapefile.GetShapeHandler(Shapefile.GetShapeType(geometryCollection.Geometries[0]));

            IGeometry body;
            int numShapes = geometryCollection.NumGeometries;
            // calc the length of the shp file, so it can put in the header.
            int shpLength = 50;
            for (int i = 0; i < numShapes; i++)
            {
                body = geometryCollection.Geometries[i];
                shpLength += 4; // length of header in WORDS
                shpLength += handler.GetLength(body); // length of shape in WORDS
            }

            int shxLength = 50 + (4 * numShapes);

            // write the .shp header
            ShapefileHeader ShapeHeader = new ShapefileHeader();
            ShapeHeader.FileLength = shpLength;

            // get envelope in external coordinates
            IEnvelope env = geometryCollection.EnvelopeInternal;
            IEnvelope bounds = ShapeHandler.GetEnvelopeExternal(new PrecisionModel(geometryFactory.PrecisionModel), env);
            ShapeHeader.Bounds = bounds;

            // assumes Geometry type of the first item will the same for all other items
            // in the collection.
            ShapeHeader.ShapeType = Shapefile.GetShapeType(geometryCollection.Geometries[0]);
            ShapeHeader.Write(shpBinaryWriter);

            // write the .shx header
            ShapefileHeader shxHeader = new ShapefileHeader();
            shxHeader.FileLength = shxLength;
            shxHeader.Bounds = ShapeHeader.Bounds;

            // assumes Geometry type of the first item will the same for all other items in the collection.
            shxHeader.ShapeType = Shapefile.GetShapeType(geometryCollection.Geometries[0]);
            shxHeader.Write(shxBinaryWriter);

            // write the individual records.
            int _pos = 50; // header length in WORDS
            for (int i = 0; i < numShapes; i++)
            {
                body = geometryCollection.Geometries[i];
                int recordLength = handler.GetLength(body);
                shpBinaryWriter.WriteIntBE(i + 1);
                shpBinaryWriter.WriteIntBE(recordLength);

                shxBinaryWriter.WriteIntBE(_pos);
                shxBinaryWriter.WriteIntBE(recordLength);

                _pos += 4; // length of header in WORDS
                handler.Write(body, shpBinaryWriter, geometryFactory);
                _pos += recordLength; // length of shape in WORDS
            }

            shxBinaryWriter.Flush();
            shxBinaryWriter.Close();
            shpBinaryWriter.Flush();
            shpBinaryWriter.Close();

            // WriteDummyDbf(filename + ".dbf", numShapes);	
        }

        /// <summary>
        /// Writes the geometry information from the specified IFeatureLayer to the file
        /// </summary>
        /// <param name="filename">A filename</param>
        /// <param name="Layer">An IFeatureLayer to save</param>
        public virtual void Write(string filename, IFeatureSet Layer)
        {
            if (Layer == null) return;
            if (Layer.Features == null) return;
            if (Layer.Features.Count == 0) return;
            string rootname, shpFile, shxFile;
            
            rootname = System.IO.Path.GetDirectoryName(filename) + "\\" + System.IO.Path.GetFileNameWithoutExtension(filename);
            shpFile = rootname + ".shp";
            shxFile = rootname + ".shx";
            
            System.IO.FileStream shpStream = new System.IO.FileStream(shpFile, System.IO.FileMode.Create);
            System.IO.FileStream shxStream = new System.IO.FileStream(shxFile, System.IO.FileMode.Create);
            BigEndianBinaryWriter shpBinaryWriter = new BigEndianBinaryWriter(shpStream);
            BigEndianBinaryWriter shxBinaryWriter = new BigEndianBinaryWriter(shxStream);

            // assumes
           // ShapeHandler handler = Shapefile.GetShapeHandler(Shapefile.GetShapeType(geometryCollection.Geometries[0]));

            ShapeHandler handler = null;

            switch (Layer.Features[0].FeatureType)
            {
                case FeatureTypes.Unspecified: return;
                case FeatureTypes.Line: handler = new MultiLineHandler();
                    break;
                case FeatureTypes.Point: handler = new PointHandler();
                    break;
                case FeatureTypes.Polygon: handler = new PolygonHandler();
                    break;
            }

            IBasicGeometry body;
            int numShapes = Layer.Features.Count;
            // calc the length of the shp file, so it can put in the header.
            int shpLength = 50;
            for (int i = 0; i < numShapes; i++)
            {
                body = Layer.Features[i].BasicGeometry;
                shpLength += 4; // length of header in WORDS
                shpLength += handler.GetLength(body); // length of shape in WORDS
            }

            int shxLength = 50 + (4 * numShapes);

            // write the .shp header
            ShapefileHeader ShapeHeader = new ShapefileHeader();
            ShapeHeader.FileLength = shpLength;

            // get envelope in external coordinates
            IEnvelope env = Layer.Envelope;
            //IEnvelope bounds = ShapeHandler.GetEnvelopeExternal(new PrecisionModel(geometryFactory.PrecisionModel), env);
            ShapeHeader.Bounds = env;

            // write the .shx header
            ShapefileHeader shxHeader = new ShapefileHeader();
            shxHeader.FileLength = shxLength;
            shxHeader.Bounds = ShapeHeader.Bounds;

            // assumes Geometry type of the first item will the same for all other items
            // in the collection.
            switch(Layer.FeatureType)
            {
                case FeatureTypes.Polygon:
                    ShapeHeader.ShapeType = ShapeGeometryTypes.Polygon;
                    shxHeader.ShapeType = ShapeGeometryTypes.Polygon;
                    break;
                case FeatureTypes.Point:
                    ShapeHeader.ShapeType = ShapeGeometryTypes.Point;
                    shxHeader.ShapeType = ShapeGeometryTypes.Point;
                    break;
                case FeatureTypes.Line:
                    ShapeHeader.ShapeType = ShapeGeometryTypes.LineString;
                    shxHeader.ShapeType = ShapeGeometryTypes.LineString;
                    break;
            }
            
            ShapeHeader.Write(shpBinaryWriter);

            // assumes Geometry type of the first item will the same for all other items in the collection.
            
            shxHeader.Write(shxBinaryWriter);

            // write the individual records.
            int _pos = 50; // header length in WORDS
            for (int i = 0; i < numShapes; i++)
            {
                body = Layer.Features[i].BasicGeometry;
                int recordLength = handler.GetLength(body);
                shpBinaryWriter.WriteIntBE(i + 1);
                shpBinaryWriter.WriteIntBE(recordLength);

                shxBinaryWriter.WriteIntBE(_pos);
                shxBinaryWriter.WriteIntBE(recordLength);

                _pos += 4; // length of header in WORDS
                handler.Write(body, shpBinaryWriter, geometryFactory);
                _pos += recordLength; // length of shape in WORDS
            }

            shxBinaryWriter.Flush();
            shxBinaryWriter.Close();
            shpBinaryWriter.Flush();
            shpBinaryWriter.Close();

        }

        //    /// <summary>
        //    /// Writes the shapes to a shapefile from the BaseGeometries array.  This will open,
        //    /// close, and save the 
        //    /// </summary>
        //    /// <remarks>
        //    /// Assumes the type given for the first geometry is the same for all subsequent geometries.
        //    /// For example, is, if the first Geometry is a Multi-polygon/ Polygon, the subsequent geometies are
        //    /// Muli-polygon/ polygon and not lines or points.
        //    /// The dbase file for the corresponding shapefile contains one column called row. It contains 
        //    /// the row number.
        //    /// </remarks>
        //    /// <param name="filename">The filename to write to (minus the .shp extension).</param>
        //    /// <param name="featureLayer">The IFeatureLayer to write.</param>
        //    /// <exception cref="System.ArgumentNullException">The filename parameter cannot be null.</exception>
        //    /// <exception cref="System.ArgumentNullException">The BaseGeometries parameter cannot be null.</exception>
        //    public virtual void Write(string filename, IFeatureLayer featureLayer)
        //    {
        //        if (filename == null)
        //        {
        //            ArgumentNullException ex = new ArgumentNullException("The filename parameter cannot be null.");
        //            LogManager.Exception(ex);
        //            throw ex;
        //        }
        //        if (featureLayer == null)
        //        {
        //            ArgumentNullException ex = new ArgumentNullException("The featureLayer parameter cannot be null.");
        //            LogManager.Exception(ex);
        //            throw ex;
        //        }

        //        System.IO.FileStream shpStream = new System.IO.FileStream(filename + ".shp", System.IO.FileMode.Create);
        //        System.IO.FileStream shxStream = new System.IO.FileStream(filename + ".shx", System.IO.FileMode.Create);
        //        BigEndianBinaryWriter shpBinaryWriter = new BigEndianBinaryWriter(shpStream);
        //        BigEndianBinaryWriter shxBinaryWriter = new BigEndianBinaryWriter(shxStream);



        //        // assumes
        //        ShapeHandler handler = Shapefile.GetShapeHandler(featureLayer.ShapeGeometryType);

        //        IBasicGeometry body;
        //        int numShapes = featureLayer.Features.Count;
        //        // calc the length of the shp file, so it can put in the header.
        //        int shpLength = 50;

        //        for (int i = 0; i < numShapes; i++)
        //        {
        //            // TO DO: FIX THIS TO WORK WITH NEW FEATURES
        //           //  body = featureLayer.Features[i].Geometry
        //            shpLength += 4; // length of header in WORDS
        //            shpLength += handler.GetLength(body); // length of shape in WORDS
        //        }

        //        int shxLength = 50 + (4 * numShapes);

        //        // write the .shp header
        //        ShapefileHeader ShapeHeader = new ShapefileHeader();
        //        ShapeHeader.FileLength = shpLength;

        //        // get envelope in external coordinates

        //        IEnvelope bounds = ShapeHandler.GetEnvelopeExternal(new PrecisionModel(geometryFactory.PrecisionModel), env);
        //        ShapeHeader.Bounds = bounds;

        //        // assumes Geometry type of the first item will the same for all other items
        //        // in the collection.
        //        ShapeHeader.ShapeType = Shapefile.GetShapeType(geometryCollection.Geometries[0]);
        //        ShapeHeader.Write(shpBinaryWriter);

        //        // write the .shx header
        //        ShapefileHeader shxHeader = new ShapefileHeader();
        //        shxHeader.FileLength = shxLength;
        //        shxHeader.Bounds = ShapeHeader.Bounds;

        //        // assumes Geometry type of the first item will the same for all other items in the collection.
        //        shxHeader.ShapeType = Shapefile.GetShapeType(geometryCollection.Geometries[0]);
        //        shxHeader.Write(shxBinaryWriter);

        //        // write the individual records.
        //        int _pos = 50; // header length in WORDS
        //        for (int i = 0; i < numShapes; i++)
        //        {
        //            body = geometryCollection.Geometries[i];
        //            int recordLength = handler.GetLength(body);
        //            shpBinaryWriter.WriteIntBE(i + 1);
        //            shpBinaryWriter.WriteIntBE(recordLength);

        //            shxBinaryWriter.WriteIntBE(_pos);
        //            shxBinaryWriter.WriteIntBE(recordLength);

        //            _pos += 4; // length of header in WORDS
        //            handler.Write(body, shpBinaryWriter, geometryFactory);
        //            _pos += recordLength; // length of shape in WORDS
        //        }

        //        shxBinaryWriter.Flush();
        //        shxBinaryWriter.Close();
        //        shpBinaryWriter.Flush();
        //        shpBinaryWriter.Close();

        //        // WriteDummyDbf(filename + ".dbf", numShapes);	
        //    }

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="filename"></param>
        //    /// <param name="recordCount"></param>
        //    public static void WriteDummyDbf(string filename, int recordCount)
        //    {
        //        dBaseHeader dbfHeader = new dBaseHeader();

        //        dbfHeader.AddColumn("Description",'C',20,0);

        //        dBaseWriter dbfWriter = new dBaseWriter(filename);
        //        dbfWriter.Write(dbfHeader);
        //        for (int i=0; i < recordCount; i++)
        //        {
        //            ArrayList columnValues = new ArrayList();
        //            columnValues.Add((double)i);
        //            dbfWriter.Write(columnValues);
        //        }
        //        dbfWriter.Close();
        //    }
        //}
    }
}
