using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Diagnostics;
using System.IO;
using MapWindow.Components;
using MapWindow.Analysis.Logging;
using MapWindow.Geometries;
using MapWindow.Analysis.DataManagement.Shapefile.Shape;
using MapWindow.Analysis.DataManagement.Shapefile.dBase;
namespace MapWindow.Analysis.DataManagement.Shapefile
{
    /// <summary>
    /// This contains shapefile related data management functions
    /// </summary>
    public static class Shapefile
    {



        #region Copy
        /// <summary>
        /// Copies a shapefile and all associated files.
        /// </summary>
        /// <param name="sourceShapefileName">Full path to the original shapefile (including .shp extension).</param>
        /// <param name="destShapefileName">Full path to where the copy should be saved (including .shp extension).</param>
        /// <returns>False if an error was encoutered, true otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">The first parameter, oldShapefilePath, cannot be null.</exception>
        /// <exception cref="System.ArgumentNullException">The second parameter, newShapefilePath, cannot be null.</exception>
        public static bool Copy(string sourceShapefileName, string destShapefileName)
        {
            List<string> ParamList = new List<string>();
            ParamList.Add("sourceShapefileName: " + sourceShapefileName);
            ParamList.Add("destShapefileName: " + destShapefileName);
            LogManager.DefaultLogManager.PublicMethodEntered("MapWindow.Analysis.DataManagement.Shapefile", ParamList);
            if (sourceShapefileName == null)
            {
                ArgumentNullException ex = new ArgumentNullException("The first parameter, sourceShapefileName, cannot be null.");

                LogManager.DefaultLogManager.Exception(ex);
                throw ex;
                
            }
            if (destShapefileName == null)
            {
                ArgumentNullException ex = new ArgumentNullException("The second parameter, destShapefileName, cannot be null.");
                LogManager.DefaultLogManager.Exception(ex);
                throw ex;
            }

            Shapefile.Delete(destShapefileName);
            string[] Extensions = { ".shx", ".dbf", ".prj", ".spx", ".sbn", ".xml", ".shp.xml" };
            try
            {
                FileSystemInfo fl;
                for (int I = 0; I <= Extensions.GetUpperBound(0); I++)
                {
                    if (File.Exists(sourceShapefileName)) System.IO.File.Copy(sourceShapefileName, destShapefileName);
                    fl = new FileInfo(destShapefileName);
                    fl.Attributes = fl.Attributes & (System.IO.FileAttributes.Archive & System.IO.FileAttributes.ReadOnly);
                    sourceShapefileName = System.IO.Path.ChangeExtension(sourceShapefileName, Extensions[I]);
                    destShapefileName = System.IO.Path.ChangeExtension(destShapefileName, Extensions[I]);
                }
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogManager.Exception(ex);
                throw ex;
            }
            LogManager.DefaultLogManager.PublicMethodLeft("MapWindow.Analysis.DataManagement.Shapefile");

            return true;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes shapefile and associated files (.shx, .dbf, .prj).
        /// </summary>
        /// <param name="ShapefilePath">Full path to shapefile, including .shp extension</param>
        public static void Delete(string ShapefilePath)
        {
            List<string> ParamList = new List<string>();
            ParamList.Add("ShapefilePath: " + ShapefilePath);
            LogManager.DefaultLogManager.PublicMethodEntered("MapWindow.Analysis.DataManagement.Shapefile.Delete", ParamList);

            string Filename, Directory;
            if (ShapefilePath == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ShapefilePath cannot be null.");
                LogManager.DefaultLogManager.Exception(ex);
                throw ex;
            }
            Directory = System.IO.Path.GetDirectoryName(ShapefilePath);

            if (System.IO.Directory.Exists(Directory) == false)
            {
                System.IO.DirectoryNotFoundException ex = new System.IO.DirectoryNotFoundException("The specified directory: " + Directory + " does not exist.");
                LogManager.DefaultLogManager.Exception(ex);
                throw ex;
            }
            Filename = ShapefilePath;

            try
            {
                if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
                Filename = System.IO.Path.ChangeExtension(Filename, ".shx");
                if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
                Filename = System.IO.Path.ChangeExtension(Filename, ".dbf");
                if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
                Filename = System.IO.Path.ChangeExtension(Filename, ".prj");
                if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
                Filename = System.IO.Path.ChangeExtension(Filename, ".spx");
                if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
                Filename = System.IO.Path.ChangeExtension(Filename, ".sbn");
                if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
                Filename = System.IO.Path.ChangeExtension(Filename, ".xml");
                if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
                Filename = System.IO.Path.ChangeExtension(Filename, ".shp.xml");
                if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
            }
            catch (Exception Ex)
            {
                LogManager.DefaultLogManager.Exception(Ex);
                throw Ex;
            }



            LogManager.DefaultLogManager.PublicMethodLeft("MapWindow.Analysis.DataManagement.Shapefile.Delete");
            return;
        }

        #endregion

        /// <summary>
        /// Returns the appropriate class to convert a shaperecord to an OGIS geometry given the type of shape.
        /// </summary>
        /// <param name="type">The shapefile type.</param>
        /// <returns>An instance of the appropriate handler to convert the shape record to a Geometry object.</returns>
        public static ShapeHandler GetShapeHandler(ShapeGeometryTypes type)
        {
            switch (type)
            {
                case ShapeGeometryTypes.Point:
                case ShapeGeometryTypes.PointM:
                case ShapeGeometryTypes.PointZ:
                case ShapeGeometryTypes.PointZM:
                    return new PointHandler();

                case ShapeGeometryTypes.Polygon:
                case ShapeGeometryTypes.PolygonM:
                case ShapeGeometryTypes.PolygonZ:
                case ShapeGeometryTypes.PolygonZM:
                    return new PolygonHandler();

                case ShapeGeometryTypes.LineString:
                case ShapeGeometryTypes.LineStringM:
                case ShapeGeometryTypes.LineStringZ:
                case ShapeGeometryTypes.LineStringZM:
                    return new MultiLineHandler();

                case ShapeGeometryTypes.MultiPoint:
                case ShapeGeometryTypes.MultiPointM:
                case ShapeGeometryTypes.MultiPointZ:
                case ShapeGeometryTypes.MultiPointZM:
                    return new MultiPointHandler();

                default:
                    return null;
            }
        }

        internal const int ShapefileId = 9994;
        internal const int Version = 1000;
        
        /// <summary>
		/// Given a geomtery object, returns the equilivent shape file type.
		/// </summary>
		/// <param name="geom">A Geometry object.</param>
		/// <returns>The equilivent for the geometry object.</returns>
        public static ShapeGeometryTypes GetShapeType(IGeometry geom) 
		{
            if (geom is IPoint) 
                return ShapeGeometryTypes.Point;
            if (geom is IPolygon) 
                return ShapeGeometryTypes.Polygon;
            if (geom is IMultiPolygon) 			
                return ShapeGeometryTypes.Polygon;
            if (geom is ILineString) 
                return ShapeGeometryTypes.LineString;
            if (geom is IMultiLineString) 			
                return ShapeGeometryTypes.LineString;
            if (geom is IMultiPoint)
                return ShapeGeometryTypes.MultiPoint;
            return ShapeGeometryTypes.NullShape;
		}

        /// <summary>
        /// Given a geomtery object, returns the equilivent shape file type.
        /// </summary>
        /// <param name="geom">A Geometry object.</param>
        /// <returns>The equilivent for the geometry object.</returns>
        public static ShapeGeometryTypes GetShapeType(IBasicGeometry geom)
        {
            if (geom is IBasicPoint)
                return ShapeGeometryTypes.Point;
            if (geom is IBasicPolygon)
                return ShapeGeometryTypes.Polygon;
            if (geom is IBasicLineString)
                return ShapeGeometryTypes.LineString;
            return ShapeGeometryTypes.NullShape;
        }

		

		/// <summary>
		/// Returns an ShapefileDataReader representing the data in a shapefile.
		/// </summary>
		/// <param name="filename">The filename (minus the . and extension) to read.</param>
		/// <param name="geometryFactory">The geometry factory to use when creating the objects.</param>
		/// <returns>An ShapefileDataReader representing the data in the shape file.</returns>
		public static ShapefileReader CreateDataReader(string filename, GeometryFactory geometryFactory)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			if (geometryFactory == null)
				throw new ArgumentNullException("geometryFactory");
			ShapefileReader shpDataReader= new ShapefileReader(filename,geometryFactory);
			return shpDataReader;
		}

		/// <summary>
		/// Creates a DataTable representing the information in a shape file.
		/// </summary>
		/// <param name="filename">The filename (minus the . and extension) to read.</param>
		/// <param name="tableName">The name to give to the Table.</param>
		/// <param name="geometryFactory">The geometry factory to use when creating the objects.</param>
		/// <returns>DataTable representing the data </returns>
		public static DataTable CreateDataTable(string filename, string tableName, GeometryFactory geometryFactory)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			if (tableName == null)
				throw new ArgumentNullException("tableName");
			if (geometryFactory == null)
				throw new ArgumentNullException("geometryFactory");

			ShapefileReader shpfileDataReader= new ShapefileReader(filename, geometryFactory);
			DataTable table = new DataTable(tableName);
		
			// use ICustomTypeDescriptor to get the properies/ fields. This way we can get the 
			// length of the dbase char fields. Because the dbase char field is translated into a string
			// property, we lost the length of the field. We need to know the length of the
			// field when creating the Table in the database.

			IEnumerator enumerator = shpfileDataReader.GetEnumerator();
			bool moreRecords = enumerator.MoveNext();
			ICustomTypeDescriptor typeDescriptor  = (ICustomTypeDescriptor)enumerator.Current;
			foreach(PropertyDescriptor property in typeDescriptor.GetProperties())
			{
				ColumnStructure column = (ColumnStructure)property;
				Type fieldType = column.PropertyType;
				DataColumn datacolumn = new DataColumn(column.Name, fieldType);
				if (fieldType== typeof(string))
					// use MaxLength to pass the length of the field in the dbase file
					datacolumn.MaxLength=column.Length;
				table.Columns.Add( datacolumn );
			}

			// add the rows - need a do-while loop because we read one row in order to determine the fields
			int iRecordCount=0;
			object[] values = new object[shpfileDataReader.FieldCount];
			do
			{
				iRecordCount++;
				shpfileDataReader.GetValues(values);
				table.Rows.Add(values);
				moreRecords = enumerator.MoveNext();
			} 
            while (moreRecords);
			return table;
		}
	
		/// <summary>
		/// Imports a shapefile into a dababase Table.
		/// </summary>
		/// <remarks>
		/// This method assumes a Table has already been created in the database.
		/// Calling this method does not close the connection that is passed in.
		/// </remarks>
		/// <param name="filename"></param>
		/// <param name="connectionstring"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public static int ImportShapefile(string filename, string connectionstring, string tableName)
		{			
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                int rowsAdded = -1;
                PrecisionModel pm = new PrecisionModel();
                GeometryFactory geometryFactory = new GeometryFactory(pm, -1);

                DataTable shpDataTable = Shapefile.CreateDataTable(filename, tableName, geometryFactory);
                string createTableSql = CreateDbTable(shpDataTable, true);

                SqlCommand createTableCommand = new SqlCommand(createTableSql, connection);
                connection.Open();
                createTableCommand.ExecuteNonQuery();

                string sqlSelect = String.Format("select * from {0}", tableName);
                SqlDataAdapter selectCommand = new SqlDataAdapter(sqlSelect, connection);

                // use a data adaptor - saves donig the inserts ourselves
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlSelect, connection);
                SqlCommandBuilder custCB = new SqlCommandBuilder(dataAdapter);
                DataSet ds = new DataSet();

                // fill dataset
                dataAdapter.Fill(ds, shpDataTable.TableName);

                // copy rows from our datatable to the empty Table in the DataSet
                int i = 0;
                foreach (DataRow row in shpDataTable.Rows)
                {
                    DataRow newRow = ds.Tables[0].NewRow();
                    newRow.ItemArray = row.ItemArray;
                    //gotcha! - new row still needs to be added to the Table.
                    //NewRow() just creates a new row with the same schema as the Table. It does
                    //not add it to the Table.
                    ds.Tables[0].Rows.Add(newRow);
                    i++;
                }

                // update all the rows in batch
                rowsAdded = dataAdapter.Update(ds, shpDataTable.TableName);
                int iRows = shpDataTable.Rows.Count;
                Debug.Assert(rowsAdded != iRows, String.Format("{0} of {1} rows were added to the database.", rowsAdded, shpDataTable.Rows.Count));

                return rowsAdded;
            }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="deleteExisting"></param>
        /// <returns></returns>
		private static string CreateDbTable(DataTable table, bool deleteExisting)
		{
			StringBuilder sb = new StringBuilder();
			if (deleteExisting)
			{
				sb.AppendFormat("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\n",table.TableName);
				sb.AppendFormat("drop Table [dbo].[{0}]\n",table.TableName);
			}

			sb.AppendFormat("CREATE TABLE [dbo].[{0}] ( \n",table.TableName);
			for (int i=0; i < table.Columns.Count; i++)
			{
				string type = GetDbType(table.Columns[i].DataType, table.Columns[i].MaxLength );
				string columnName = table.Columns[i].ColumnName;
				if (columnName=="PRIMARY")
				{
					columnName="DBF_PRIMARY";
					Debug.Assert(false, "Shp2Db: Column PRIMARY renamed to PRIMARY.");
					Trace.WriteLine("Shp2Db: Column PRIMARY renamed to PRIMARY.");
				}
				sb.AppendFormat("[{0}] {1} ", columnName, type );
				
				// the unique id column cannot be null
				if (i==1)
					sb.Append(" NOT NULL ");
				if (i+1 != table.Columns.Count)
					sb.Append(",\n");
			}
			sb.Append(")\n");

			// the DataSet update stuff requires a unique column - so give it the row colum that we added
			//sb.AppendFormat("ALTER TABLE [dbo].[{0}] WITH NOCHECK ADD CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED ([{1}])  ON [PRIMARY]\n",Table.TableName, Table.Columns[1].ColumnName);
			return sb.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		
		private static string GetDbType(Type type, int length)
		{
			if (type == typeof(double))
				return "real";
			else if (type == typeof(float))
				return "float";
			else if (type == typeof(string))
				return String.Format("nvarchar({0}) ", length);
			else if (type == typeof(byte[]))
				return "image";
			else if (type == typeof(int))
				return "int";
			else if (type == typeof(char[]))
				return String.Format("nvarchar({0}) ", length);
			throw new NotSupportedException("Need to add the SQL type for "+type.Name);
		}
	}
}
