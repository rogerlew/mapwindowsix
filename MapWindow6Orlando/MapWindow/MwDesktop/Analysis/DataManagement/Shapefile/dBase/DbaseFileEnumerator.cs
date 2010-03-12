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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/6/2008 10:49:41 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.Collections;
using System.IO;
using System.Globalization;

namespace MapWindow.Analysis.DataManagement.Shapefile.dBase
{


    /// <summary>
    /// 
    /// </summary>
    public class DbaseFileEnumerator : IEnumerator
    {
        dBaseReader _parent;
        ArrayList _arrayList;
        int _iCurrentRecord = 0;
        private BinaryReader _dbfStream = null;
        private int _readPosition = 0;
        private dBaseHeader _header = null;
        //private string[] _fieldNames = null;
        private long _currentOffset;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public DbaseFileEnumerator(dBaseReader parent)
        {
            _parent = parent;
            FileStream stream = new FileStream(parent.Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            _dbfStream = new BinaryReader(stream);
            ReadHeader();
        }

        #region Implementation of IEnumerator

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// The current offset in the file
        /// </summary>
        public long CurrentOffset
        {
            get
            {
                return _currentOffset;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            _iCurrentRecord++;
            if (_iCurrentRecord <= _header.NumRecords)
            {
                _currentOffset = _dbfStream.BaseStream.Position;
                _arrayList = this.Read();

            }
            bool more = true;
            if (_iCurrentRecord > _header.NumRecords)
            {
                this._dbfStream.Close();
                more = false;
            }
            return more;
        }

        /// <summary>
        /// 
        /// </summary>
        public object Current
        {
            get
            {
                return _arrayList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ReadHeader()
        {
            _header = new dBaseHeader();
            // read the header
            _header.ReadHeader(_dbfStream);

            // how many records remain
            _readPosition = _header.HeaderLength;
        }


        /// <summary>
        /// Read a single dbase record
        /// </summary>
        /// <returns>return the read shapefile record or null if there are no more records</returns>
        private ArrayList Read()
        {
            ArrayList attrs = null;

            bool foundRecord = false;

            // Scan through deleted records until we reach the next record
            while (!foundRecord)
            {
                // read the deleted flag
                char tempDeleted = (char)_dbfStream.ReadChar();
                if (tempDeleted != '*')
                {
                    foundRecord = true;
                    break;
                }
                _dbfStream.BaseStream.Seek(_header.RecordLength - 1, SeekOrigin.Current);

            }

            // retrieve the record length
            int tempNumFields = _header.NumFields;

            // storage for the actual values
            attrs = new ArrayList(tempNumFields);

            // read the record length
            int tempRecordLength = 1; // for the deleted character just read.

            // read the Fields
            for (int j = 0; j < tempNumFields; j++)
            {

                // find the length of the field.
                int tempFieldLength = _header.Fields[j].Length;
                tempRecordLength = tempRecordLength + tempFieldLength;

                // find the field type
                char tempFieldType = _header.Fields[j].DbaseType;

                // read the data.
                object tempObject = null;

                switch (tempFieldType)
                {
                    case 'L':   // logical data type, one character (T,t,F,f,Y,y,N,n)
                        char tempChar = (char)_dbfStream.ReadByte();
                        if ((tempChar == 'T') || (tempChar == 't') || (tempChar == 'Y') || (tempChar == 'y'))
                            tempObject = true;
                        else tempObject = false;
                        break;

                    case 'C':   // character record.
                        char[] sbuffer = new char[tempFieldLength];
                        sbuffer = _dbfStream.ReadChars(tempFieldLength);
                        // use an encoding to ensure all 8 bits are loaded
                        // tempObject = new string(sbuffer, "ISO-8859-1").Trim();								

                        //HACK: this can be made more efficient
                        tempObject = new string(sbuffer).Trim().Replace("\0", String.Empty);   //.ToCharArray();
                        break;

                    case 'D':   // date data type.
                        char[] ebuffer = new char[8];
                        ebuffer = _dbfStream.ReadChars(8);
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
                        char[] fbuffer = new char[tempFieldLength];
                        fbuffer = _dbfStream.ReadChars(tempFieldLength);
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
                attrs.Add(tempObject);
            }


            // ensure that the full record has been read.

            if (tempRecordLength < _header.RecordLength)
            {
                _dbfStream.BaseStream.Seek(_header.RecordLength - tempRecordLength, SeekOrigin.Current);
            }


            return attrs;
        }

        #endregion

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
