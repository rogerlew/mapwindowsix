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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using MapWindow.Data.IO;
using System.Collections;
using MapWindow.Analysis.DataManagement.Shapefile.Shape;
using MapWindow.Analysis.DataManagement.Shapefile;
using System.IO;
using MapWindow.Data;
namespace MapWindow.Scrap
{
    /// <summary>
    /// A Feature data source that reads and writes to shapefile formats
    /// </summary>
    public class ShapeFileDataSource : IFeatureDataSource
    {
        #region Variables
        /// <summary>
        /// Filename or connection string
        /// </summary>
        private string _filenameOrConnection;

        /// <summary>
        /// Layers
        /// </summary>
        private MapWindow.Main.IEventDictionary<int, IFeatureSet> _layers = null;

        /// <summary>
        /// Boolean, true if this should not be allowed to save
        /// </summary>
        private bool _readOnly;

        //string _filename;
        private int _numRecords;
        private DateTime _updateDate;
        private int _headerLength;
        private int _recordLength;
        private int _numFields;
        private Dictionary<string, Field> _fields;
        private byte _fileType;
        //System.Data.DataTable _Table;
        //Dictionary<int, IFeature> _features;
        // Constant for the size of a record
        private int _fileDescriptorSize = 32;

        #endregion


        #region IFeatureDataSource Members

        /// <summary>
        /// FeatureLayers
        /// </summary>
        public MapWindow.Main.IEventDictionary<int, IFeatureSet> FeatureLayers
        {
            get
            {
                return _layers;
            }
        }

        #endregion

        #region IDataSource Members

        /// <summary>
        /// .shp
        /// </summary>
        public string DialogFilterRead
        {
            get { return "Shapefiles (*.shp) |.shp"; }
        }

        /// <summary>
        /// .shp
        /// </summary>
        public string DialogFilterWrite
        {
            get { return "Shapefiles (*.shp) |.shp"; }
        }

        /// <summary>
        /// .shp
        /// </summary>
        public string Extension
        {
            get { return ".shp"; }
        }

        /// <summary>
        /// A filename or connection string
        /// </summary>
        public string FilenameOrConnection
        {
            get
            {
                return _filenameOrConnection;
            }
          
        }

        /// <summary>
        /// Number of layers for database connections
        /// </summary>
        public int NumLayers
        {
            get { return _layers.Count; }
        }

        /// <summary>
        /// Opens a shapefile.  This obtains an inram copy of the vector geometries, but
        /// uses a database style link to the dbf fields so that it will obtain specific
        /// field values upon request.  Once a local copy exists, it will be stored in
        /// the Layers.
        /// </summary>
        /// <param name="FileOrConnection"></param>
        public void Open(string FileOrConnection)
        {
            _filenameOrConnection = FileOrConnection;
            ShapeReader sfReader = new ShapeReader(FileOrConnection);
            IEnumerator Shape = sfReader.GetEnumerator();
           // FeatureSet myShapefileLayer = new FeatureSet(this);
            //IFeature CurrentFeature;
            while (Shape.MoveNext())
            {
               // TO DO
            }
        }

        /// <summary>
        /// Opens a new file or connection
        /// </summary>
        /// <param name="FileOrConnection"></param>
        /// <param name="ReadOnly"></param>
        public void Open(string FileOrConnection, bool ReadOnly)
        {
            _filenameOrConnection = FileOrConnection;
            _readOnly = ReadOnly;
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool ReadOnly
        {
            get { return _readOnly; }
        }

        /// <summary>
        /// Saves any modifications to the shapefile over the existing files.
        /// </summary>
        /// <exception cref="System.NullReferenceException">Occurs if FilenameOrConnection is null: This Shapefile DataSource has no file or connection associated with it.</exception>
        public void Save()
        {
            if (_filenameOrConnection == null)
            {
                throw new NullReferenceException("This Shapefile DataSource has no file or connection associated with it.");
            }
        }

        /// <summary>
        /// Saves any modifications of the original shapefile to a new path location.
        /// </summary>
        /// <param name="Filename">The String path or connection string to overwrite</param>
        /// <param name="Overwrite">Boolean, if this is true this method will overwrite any existing files for the shapefile.</param>
        public void SaveAs(string Filename, bool Overwrite)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Read the header data from the DBF file.
        /// </summary>
        /// <param name="reader">BinaryReader containing the header.</param>
        private void ReadTableHeader(BinaryReader reader)
        {
            // type of reader.
            _fileType = reader.ReadByte();
            if (_fileType != 0x03)
                throw new NotSupportedException("Unsupported DBF reader Type " + _fileType);

            // parse the update date information.
            int year = (int)reader.ReadByte();
            int month = (int)reader.ReadByte();
            int day = (int)reader.ReadByte();
            _updateDate = new DateTime(year + 1900, month, day);

            // read the number of records.
            _numRecords = reader.ReadInt32();

            // read the length of the header structure.
            _headerLength = reader.ReadInt16();

            // read the length of a record
            _recordLength = reader.ReadInt16();

            // skip the reserved bytes in the header.
            //in.skipBytes(20);
            reader.ReadBytes(20);
            // calculate the number of Fields in the header
            _numFields = (_headerLength - _fileDescriptorSize - 1) / _fileDescriptorSize;

            // read all of the header records
            _fields = new Dictionary<string, Field>();

            for (int i = 0; i < _numFields; i++)
            {


                // read the field name				
                char[] buffer = new char[11];
                buffer = reader.ReadChars(11);
                string name = new string(buffer);
                int nullPoint = name.IndexOf((char)0);
                if (nullPoint != -1)
                    name = name.Substring(0, nullPoint);


                // read the field type
                char Code = (char)reader.ReadByte();

                // read the field data address, offset from the start of the record.
                int dataAddress = reader.ReadInt32();

                // read the field length in bytes
                byte tempLength = reader.ReadByte();



                // read the field decimal count in bytes
                byte decimalcount = reader.ReadByte();

                // read the reserved bytes.
                //reader.skipBytes(14);
                reader.ReadBytes(14);
                Field myField = new Field(name, Code, tempLength, decimalcount);
                myField.DataAddress = dataAddress; // not sure what this does yet
                _fields.Add(name, myField); // Store fields accessible by index and string column name

              //  _Table.Columns.Add(myField);
            }

            // Last byte is a marker for the end of the field definitions.
            reader.ReadBytes(1);
        }

        #endregion
    }
}
