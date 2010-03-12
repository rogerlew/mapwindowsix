using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using MapWindow.Geometries;
using MapWindow.Data;
using MapWindow.Analysis.DataManagement.Shapefile.Shape;

using MapWindow.Analysis.DataManagement.Shapefile.dBase;
namespace MapWindow.Analysis.DataManagement.Shapefile
{
    /// <summary>
    /// A simple test class for write a complete (shp, shx and dbf) shapefile structure.
    /// </summary>
    public class ShapefileWriter
    {
        #region Static

  

        /// <summary>
        /// Gets the header from a dbf file.
        /// </summary>
        /// <param name="dbfFile">The DBF file.</param>
        /// <returns></returns>
        public static dBaseHeader GetHeader(string dbfFile)
        {
            if (!File.Exists(dbfFile))
                throw new FileNotFoundException(dbfFile + " not found");
            dBaseHeader header = new dBaseHeader();
            header.ReadHeader(new BinaryReader(new FileStream(dbfFile, FileMode.Open, FileAccess.Read, FileShare.Read)));
            return header;
        }

        #endregion

        private const int DoubleLength = 18;
        private const int DoubleDecimals = 8;
        private const int IntLength = 10;
        private const int IntDecimals = 0;
        private const int StringLength = 254;
        private const int StringDecimals = 0;

        private string shpFile = String.Empty;
        private string shxFile = String.Empty;
        private string dbfFile = String.Empty;

        private ShapeWriter shapeWriter = null;
        private dBaseWriter dbaseWriter = null;

        private dBaseHeader header = null;

        /// <summary>
        /// Gets or sets the header of the shapefile.
        /// </summary>
        /// <value>The header.</value>
        public dBaseHeader Header
        {
            get { return header; }
            set { header = value; }
        }

        private GeometryFactory geometryFactory = null;

        /// <summary>
        /// Gets or sets the geometry factory.
        /// </summary>
        /// <value>The geometry factory.</value>
        protected virtual GeometryFactory GeometryFactory
        {
            get { return geometryFactory; }
            set { geometryFactory = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ShapefileDataWriter"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file with or without any extension.</param>
        public ShapefileWriter(string fileName) : this(fileName, new GeometryFactory()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ShapefileDataWriter"/> class.
        /// </summary>
        /// <param name="fileName">File path without any extension</param>
        /// <param name="geometryFactory"></param>
        public ShapefileWriter(string fileName, GeometryFactory geometryFactory)
        {
            this.geometryFactory = geometryFactory;

            // Files            
            shpFile = fileName;
            dbfFile = fileName + ".dbf";

            // Writers
            shapeWriter = new ShapeWriter(geometryFactory);
            dbaseWriter = new dBaseWriter(dbfFile);
        }

        /// <summary>
        /// Writes the specified feature collection.
        /// </summary>
        /// <param name="featurelayer">The feature collection.</param>
        public virtual void Write(IFeatureSet featurelayer)
        {
            // Test if the Header is initialized
            if (Header == null)
                throw new ApplicationException("Header must be set first!");

            try
            {
                // Write shp and shx  
                IBasicGeometry[] geometries = new IBasicGeometry[featurelayer.Features.Count];
                int index = 0;
                // putting these into an array effectively re-indexes the members, so by putting 
                // them into an array, we ensure the order will match the order of the dbf.
                foreach (IFeature feature in featurelayer.Features)
                {
                    geometries[index++] = feature.BasicGeometry;
                    //dbaseWriter.Write(feature.Value.DataRow, );
                }
                shapeWriter.Write(shpFile, new GeometryCollection(geometries, geometryFactory));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                // Close dbf writer
                dbaseWriter.Close();
            }
        }
    }
}
