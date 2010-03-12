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
using System.Collections;
using System.IO;
using MapWindow.Main;
using System.Globalization;
using MapWindow.Analysis.DataManagement.Shapefile.Shape;
using MapWindow.Geometries;
using MapWindow.Data;
namespace MapWindow.Scrap
{
    /// <summary>
    /// This class supports complete shapefile editing capabilities, not
    /// restricting the user in any way.  Think of this as more of a 
    /// file editor, while the FeatureLayer is what you would normally
    /// open for display purposes.  This is optimized for versitility 
    /// rather than speed and does not support database style Append,
    /// Delete and Edit, but rather rewrites the entire shapefile every
    /// time the shapefile is saved.
    /// </summary>
    public sealed class ShapefileFeatureSet: FeatureSet
    {
        #region Variables

        int _numRecords;
        DateTime _updateDate;
        int _headerLength;
        int _recordLength;
        int _numFields;
        readonly List<Field> _columns;
        byte _fileType;
        BinaryWriter _writer;

        // Constant for the size of a record
        private const int FileDescriptorSize = 32;

        #endregion

        /// <summary>
        /// Don't actually open the shapefile in this case, just initialize the
        /// major variables.
        /// </summary>
        public ShapefileFeatureSet()
        {
            //Fields = new Dictionary<string, Field>();
            _columns = new List<Field>();
            DataTable = new System.Data.DataTable();
            Features = new FeatureList(this);
            FeatureType = FeatureTypes.Unspecified;
        }

        /// <summary>
        /// Constructor for the ShapefileFeatureLayer class with no status messages.
        /// </summary>
        /// <param name="Filename">The filename to open.</param>
        public ShapefileFeatureSet(string Filename): this(Filename, null)
        {
            // This simply forwards the constructor to with a default null value to progressHandler.
        }

        /// <summary>
        /// Constructor that also accepts a progres handler to show the progress of loading the values into memory.
        /// </summary>
        /// <param name="filename">The filename to open.</param>
        /// <param name="progressHandler">Any valid implementation of the IProgressHandler interface.</param>
        public ShapefileFeatureSet(string filename, IProgressHandler progressHandler)
        {
            //Fields = new Dictionary<string, Field>();
            _columns = new List<Field>();
            DataTable = new System.Data.DataTable();
            Features = new FeatureList(this);
            // Since they have passed a filename go ahead and open it
            Filename = filename;
            FeatureType = FeatureTypes.Unspecified; // this will get set when queried the first time
            Name = Path.GetFileNameWithoutExtension(filename);
            Open(progressHandler);
        }


        #region Methods

        /// <summary>
        /// Reads all the information from the file, including the vector shapes and the database component.
        /// </summary>
        public override void Open()
        {
            Open(null);
        }

        /// <summary>
        /// Reads all the information from the specified file.  This also sends status messages through progressHandler.
        /// </summary>
        public void Open(IProgressHandler progressHandler)
        {
            
            IFeature currentFeature;
            string dbfFile = Path.ChangeExtension(Filename, ".dbf");
            FileStream myStream = new FileStream(dbfFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader myReader = new BinaryReader(myStream);
            ReadTableHeader(myReader); // based on the header, set up the fields information etc.

            ProgressMeter pm = new ProgressMeter(progressHandler, "Opening " + Path.GetFileName(Filename), _numRecords);
             
           
            
            ShapeReader myShapeReader = new ShapeReader(Filename);
            IEnumerator en = myShapeReader.GetEnumerator();
            en.MoveNext();
            // Reading the Table elements as well as the shapes in a single progress loop.
            for (int row = 0; row < _numRecords; row++)
            {
                // --------- DATABASE --------- CurrentFeature = ReadTableRow(myReader);
                // rem this line if the DATABASE is turned back on
                currentFeature = new Feature();

                currentFeature.BasicGeometry = (IBasicGeometry)en.Current;
                en.MoveNext();
                Features.Add(currentFeature);

             
                // --------- DATABASE ---------  _Table.Rows.Add(CurrentFeature.DataRow);

                // If a progress message needs to be updated, this will handle that.
                
                pm.CurrentValue = row;
                

            }
            Envelope = null; // invalidate the envelope so that it will be re-calculated from all the points
            
            pm.Reset(); // Shows the basic "Ready." message indicating that we are done with this step.
            

        }

        /// <summary>
        /// Attempts to save the file to the path specified by the Filename property.
        /// This should be the .shp extension.
        /// </summary>
        private void DoSave()
        {
            string dbfFile = Path.ChangeExtension(Filename, ".dbf");
            ShapeWriter myShapeWriter = new ShapeWriter();
            myShapeWriter.Write(Filename, this);

            _writer = new BinaryWriter(File.Open(dbfFile, FileMode.Create));
            WriteHeader(_writer);
            WriteTable();
            _writer.Close();
        }

        /// <summary>
        /// Saves this thingy
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="overwrite"></param>
        public override void SaveAs(string filename, bool overwrite)
        {
            if (Filename == filename)
            {
                DoSave();
                return;
            }
            if (File.Exists(Filename))
            {
                if (overwrite == false) return;
                File.Delete(Filename);
            }
            Filename = filename;
            Save();
        }

        /// <summary>
        /// This appends the content of one datarow to a dBase file.
        /// </summary>
        /// <exception cref="ArgumentNullException">The columnValues parameter was null</exception>
        /// <exception cref="InvalidOperationException">Header records need to be written first.</exception>
        /// <exception cref="InvalidDataException">Table property of columnValues parameter cannot be null.</exception>
        public void WriteTable()
        {
            if (DataTable == null) return;
           
            _writer.Write((byte)0x20); // the deleted flag

            for (int row = 0; row < Features.Count; row++)
            {
                int len = _recordLength;
                for (int fld = 0; fld < _columns.Count; fld++)
                {

                    object columnValue = DataTable.Rows[row][fld];
                    if (columnValue == null || columnValue is DBNull)
                        WriteSpaces(_columns[fld].Length+1);
                    if (columnValue is double)
                        Write((double)columnValue, _columns[fld].Length, _columns[fld].DecimalCount);
                    else if (columnValue is float)
                        Write((float)columnValue);
                    else if (columnValue is int || columnValue is Int16 || columnValue is Int64)
                        Write(Convert.ToInt32(columnValue), _columns[fld].Length);
                    else if (columnValue is bool)
                        Write((bool)columnValue);
                    else if (columnValue is string)
                    {
                        int length = _columns[fld].Length;
                        Write((string)columnValue, length);
                    }
                    else if (columnValue is DateTime)
                        Write((DateTime)columnValue);
                    else
                    {
                        Write((string)columnValue, _columns[fld].Length);
                    }
                    len -= _columns[fld].Length;
                }
                // If, for some reason the column lengths don't add up to the total record length, fill with spaces.
                if(len>0)WriteSpaces(len);

            }

        }

        /// <summary>
        /// Writes a number of spaces equal to numspaces
        /// </summary>
        /// <param name="numspaces">The integer number of spaces to write</param>
        public void WriteSpaces(int numspaces)
        {
            for (int I = 0; I < numspaces; I++)
            {
                _writer.Write(' ');
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <param name="decimalCount"></param>
        public void Write(double number, int length, int decimalCount)
        {
            // write with 19 chars.
            string format = "{0:";
            for (int i = 0; i < decimalCount; i++)
            {
                if (i == 0)
                    format = format + "0.";
                format = format + "0";
            }
            format = format + "}";
            string str = String.Format(format, number);
            for (int i = 0; i < length - str.Length; i++)
                _writer.Write((byte)0x20);
            foreach (char c in str)
                _writer.Write(c);
        }

        /// <summary>
        /// Writes an integer so that it is formatted for dbf.  This is still buggy since it is possible to lose info here.
        /// </summary>
        /// <param name="number">The integer value</param>
        /// <param name="length">The length of the field.</param>
        public void Write(int number, int length)
        {
            string str = number.ToString();
            if(str.Length > length)
            {
                str = str.Substring(str.Length - length, length);
            }
            for (int i = 0; i < length - str.Length; i++)
                _writer.Write((byte)0x20);
            foreach (char c in str)
                _writer.Write(c);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        public void Write(float number)
        {
            _writer.Write(String.Format("{0:000000000.000000000}", number));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        public void Write(string text, int length)
        {
            // ensure string is not too big
            text = text.PadRight(length, ' ');
            string dbaseString = text.Substring(0, length);

            // will extra chars get written??
            foreach (char c in dbaseString)
                _writer.Write(c);

            int extraPadding = length - dbaseString.Length;
            for (int i = 0; i < extraPadding; i++)
                _writer.Write((byte)0x20);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        public void Write(DateTime date)
        {
            _writer.Write(date.Year - 1900);
            _writer.Write(date.Month);
            _writer.Write(date.Day);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        public void Write(bool flag)
        {
            if (flag)
                _writer.Write("T");
            else _writer.Write("F");
        }


        #endregion

        #region Properties


        /// <summary>
        /// This is the list of columns in the order that they will be stored.
        /// </summary>
        public List<Field> Columns
        {
            get { return _columns; }
        }

       

       

       
        #endregion


        /// <summary>
        /// Write the header data to the DBF file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteHeader(BinaryWriter writer)
        {
            // write the output file type.
            writer.Write(_fileType);

            writer.Write((byte)(_updateDate.Year - 1900));
            writer.Write((byte)_updateDate.Month);
            writer.Write((byte)_updateDate.Day);

            // write the number of records in the datafile.
            writer.Write(_numRecords);

            // write the length of the header structure.
            writer.Write((short)_headerLength);

            // write the length of a record
            writer.Write((short)_recordLength);

            // write the reserved bytes in the header
            for (int i = 0; i < 20; i++)
                writer.Write((byte)0);

            // write all of the header records
            Field currentField; 
            for (int i = 0; i < _columns.Count; i++)
            {
                currentField = _columns[i];
                // write the field name
                for (int j = 0; j < 11; j++)
                {

                    if (currentField.ColumnName.Length > j)
                        writer.Write((byte)currentField.ColumnName[j]);
                    else writer.Write((byte)0);
                }

                // write the field type
                writer.Write(currentField.TypeCharacter);

                // write the field data address, offset from the start of the record.
                writer.Write(0);

                // write the length of the field.
                writer.Write(currentField.Length);

                // write the decimal count.
                writer.Write(currentField.DecimalCount);

                // write the reserved bytes.
                for (int j = 0; j < 14; j++) writer.Write((byte)0);
            }

            // write the end of the field definitions marker
            writer.Write((byte)0x0D);
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
            int year = reader.ReadByte();
            int month = reader.ReadByte();
            int day = reader.ReadByte();
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
            _numFields = (_headerLength - FileDescriptorSize - 1) / FileDescriptorSize;

            // read all of the header records
            //Fields = new Dictionary<string, Field>();
            
            for (int i = 0; i < _numFields; i++)
            {
                
               
                // read the field name				
                char[] buffer = reader.ReadChars(11);
                string name = new string(buffer);
                int nullPoint = name.IndexOf((char)0);
                if (nullPoint != -1)
                    name = name.Substring(0, nullPoint);
               

                // read the field type
                char code = (char)reader.ReadByte();

                // read the field data address, offset from the start of the record.
                int dataAddress = reader.ReadInt32();

                // read the field length in bytes
                byte tempLength = reader.ReadByte();
               
                

                // read the field decimal count in bytes
                byte decimalcount = reader.ReadByte();

                // read the reserved bytes.
                //reader.skipBytes(14);
                reader.ReadBytes(14);
                Field myField = new Field(name, code, tempLength, decimalcount);
                myField.DataAddress = dataAddress; // not sure what this does yet
                //Fields.Add(name, myField); // Store fields accessible by string column name
                _columns.Add(myField); // Store fields accessible by an index
                DataTable.Columns.Add(myField);
            }

            // Last byte is a marker for the end of the field definitions.
            reader.ReadBytes(1);
        }

        /// <summary>
        /// Read a single dbase record
        /// </summary>
        /// <returns>Returns an IFeature with information appropriate for the current row in the Table</returns>
        private IFeature ReadTableRow(BinaryReader reader)
        {

            int tempRecordLength;
            bool foundRecord = false;
            Feature result = new Feature();
            result.DataRow = DataTable.NewRow();

            // Scan through deleted records until we reach the next record
            while (!foundRecord)
            {
                // read the deleted flag
                char tempDeleted = reader.ReadChar();
                if (tempDeleted != '*')
                {
                    foundRecord = true;
                    break;
                }
                reader.BaseStream.Seek(_recordLength - 1, SeekOrigin.Current);

            }
            tempRecordLength = 1;
            int tempFieldLength = _recordLength;
            
            // read the Fields
            IEnumerator en = _columns.GetEnumerator();
            int j = 0;
            while(en.MoveNext())
            {

                // find the length of the field.
                Field CurrentField = (Field)en.Current;

                // find the field type
                char tempFieldType = CurrentField.TypeCharacter;

                // read the data.
                object tempObject = null;
                tempRecordLength = tempRecordLength + CurrentField.Length;
                switch (tempFieldType)
                {
                    case 'L':   // logical data type, one character (T,t,F,f,Y,y,N,n)
                        
                        char tempChar = (char)reader.ReadByte();
                        if ((tempChar == 'T') || (tempChar == 't') || (tempChar == 'Y') || (tempChar == 'y'))
                            tempObject = true;
                        else tempObject = false;
                        break;

                    case 'C':   // character record.
                        
                        char[] sbuffer = new char[CurrentField.Length];
                        sbuffer = reader.ReadChars(CurrentField.Length);
                        // use an encoding to ensure all 8 bits are loaded
                        // tempObject = new string(sbuffer, "ISO-8859-1").Trim();								

                        //HACK: this can be made more efficient
                        tempObject = new string(sbuffer).Trim().Replace("\0", String.Empty);   //.ToCharArray();
                        break;

                    case 'D':   // date data type.
                       
                        char[] ebuffer = new char[8];
                        ebuffer = reader.ReadChars(8);
                        if (IsNull(ebuffer))
                        {
                            tempObject = DBNull.Value;
                        }
                        else
                        {

                            string tempString = new string(ebuffer, 0, 4);
                            int year = Int32.Parse(tempString, CultureInfo.InvariantCulture);
                            tempString = new string(ebuffer, 4, 2);
                            int month = Int32.Parse(tempString, CultureInfo.InvariantCulture);
                            tempString = new string(ebuffer, 6, 2);
                            int day = Int32.Parse(tempString, CultureInfo.InvariantCulture);
                            tempObject = new DateTime(year, month, day);

                        }
                        break;

                    case 'N': // number - ESRI uses N for doubles and floats
                    case 'F': // floating point number - reading non-esri floats

                        char[] fbuffer = new char[CurrentField.Length];
                        fbuffer = reader.ReadChars(CurrentField.Length);
                        string tempStr = new string(fbuffer);
                        // Check for DBNULL
                        if (IsNull(fbuffer))
                        {
                            tempObject = DBNull.Value;
                        }
                        else
                        {
                            try
                            {
                                tempObject = Double.Parse(tempStr.Trim(), CultureInfo.InvariantCulture);
                            }
                            catch (FormatException)
                            {

                                // Returning string here will create trouble with specific types.
                                // return null.
                                // if we can't format the number, just save it as a string
                                // instead of saving as string, check for null
                                // tempObject = tempString;
                                tempObject = DBNull.Value;


                            }
                        }
                        break;

                    default:
                        throw new NotSupportedException("Do not know how to parse Field type " + tempFieldType);
                }
                result.DataRow[j] = tempObject;
                j++;
            }


            // ensure that the full record has been read.

            if (tempRecordLength < _recordLength)
            {
                reader.BaseStream.Seek(_recordLength - tempRecordLength, SeekOrigin.Current);
            }

            

            return result;
        }

        private bool IsNull(char[] CharArray)
        {
            for (int I = 0; I < CharArray.Length; I++)
            {
                if (CharArray[I] != ' ' && CharArray[I] != '\0') return false;
            }
            return true;
        }

       
    }
}
