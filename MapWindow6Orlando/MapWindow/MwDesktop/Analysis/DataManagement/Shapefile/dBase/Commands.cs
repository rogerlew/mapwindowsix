using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Globalization;
namespace MapWindow.Analysis.DataManagement.Shapefile.dBase
{
    /// <summary>
    /// The original read write for the NTS topology suite features only the basic opperations of 
    /// reading or writing an entire file.  This is perfectly acceptable in the case of small shapefiles,
    /// but for huge shapefiles it would be nice to support extended functionality to include commands.
    /// 
    /// </summary>
    public class Commands
    {
        static dBaseHeader _Header;
        static string _Filename;
        BinaryReader _dbfStream;


        /// <summary>
        /// Creates a new instance of the commmand server from a file
        /// </summary>
        /// <param name="Filename">The filename of the dbf to read, edit or append to</param>
        public Commands(string Filename)
        {
            if (Filename == null)
            {
                throw new ArgumentNullException(Filename);
            }
            // check for the file existing here, otherwise we will not get an error
            //until we read the first record or read the header.
            if (!File.Exists(Filename))
            {
                throw new FileNotFoundException(String.Format("Could not find file \"{0}\"", Filename));
            }
            _Filename = Filename;

            dBaseReader dr = new dBaseReader(Filename);
            _Header = dr.GetHeader();
        }

        /// <summary>
        /// If the precise location in the file is known, this will read the values from one and
        /// only one row at the specified offset.  Since the precise offset generally can't be
        /// determined without reading the entire file using a Reader, this is mostly useful
        /// in the case where edits from a specific row need to be abandoned and reloaded.
        /// </summary>
        /// <param name="Offset">The byte offset within the dbf file where this row is located.</param>
        /// <param name="Filename"></param>
        /// <returns>An Arraylist containing the column information</returns>
        /// 
        public ArrayList Read(long Offset, string Filename)
        {
            if (_Header == null)
            {
                throw new InvalidOperationException("The Header for the file specified was null.");
            }
            FileStream stream = new FileStream(_Filename, System.IO.FileMode.Open);
            stream.Seek(Offset, SeekOrigin.Begin);
            _dbfStream = new BinaryReader(stream);
            ArrayList result = Read();
            _dbfStream.Close();
            stream.Close();
            return result;
        }

        /// <summary>
        /// Reading a row index is a challenge because there can be any number of deleted rows in the file
        /// that have '*' representing the deleted files.
        /// </summary>
        /// <param name="FID">The integer RowIndex where the data is stored</param>
        /// <returns>An ArrayList with the values from the specified row.</returns>
        public ArrayList Read(int FID)
        {
            long Offset = 0;
            int I = -1;
            if (_Header == null)
            {
                throw new InvalidOperationException("The Header for the file specified was null.");
            }
            FileStream stream = new FileStream(_Filename, System.IO.FileMode.Open);
            // It might seem like you could guess that the minimum would be the row length times the offset,
            // but the potential for deleted rows means we need to count the existing rows along the way.
            Offset = _Header.HeaderLength;
            stream.Seek(Offset, SeekOrigin.Begin);
            _dbfStream = new BinaryReader(stream);
            while (I < FID)
            {
                if (_dbfStream.PeekChar() != '*')
                {
                    I++;
                }
                stream.Seek(_Header.RecordLength, SeekOrigin.Current);
            }

          
            ArrayList result = Read();
            _dbfStream.Close();
            stream.Close();
            return result;
        }

        /// <summary>
		/// Read a single dbase record
		/// </summary>
		/// <returns>return the read shapefile record or null if there are no more records</returns>
		private ArrayList Read()  
		{
			ArrayList attrs = null;
   
			bool foundRecord = false;
			while (!foundRecord) 
			{
				// retrieve the record length
				int tempNumFields = _Header.NumFields;
        
				// storage for the actual values
				attrs = new ArrayList(tempNumFields);
        
				// read the deleted flag
				char tempDeleted = (char)_dbfStream.ReadChar();
        
				// read the record length
				int tempRecordLength = 1; // for the deleted character just read.
        
				// read the Fields
				for (int j = 0; j < tempNumFields; j++)
				{                
					// find the length of the field.
					int tempFieldLength = _Header.Fields[j].Length;
					tempRecordLength = tempRecordLength + tempFieldLength;
            
					// find the field type
					char tempFieldType = _Header.Fields[j].DbaseType;
            
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
							tempObject = new string(sbuffer).Trim().Replace("\0",String.Empty);   //.ToCharArray();
							break;
                    
						case 'D':   // date data type.
							char[] ebuffer = new char[8];
							ebuffer = _dbfStream.ReadChars(8);
							string tempString = new string(ebuffer, 0, 4);
							int year = Int32.Parse(tempString, CultureInfo.InvariantCulture);
							tempString = new string(ebuffer, 4, 2);
                            int month = Int32.Parse(tempString, CultureInfo.InvariantCulture);
							tempString = new string(ebuffer, 6, 2);
							int day = Int32.Parse(tempString, CultureInfo.InvariantCulture);
							tempObject = new DateTime(year, month, day);
							break;
                    
						case 'N': // number
						case 'F': // floating point number
							char[] fbuffer = new char[tempFieldLength];
							fbuffer = _dbfStream.ReadChars(tempFieldLength);
							tempString = new string(fbuffer);
							try 
							{ 
								tempObject = Double.Parse(tempString.Trim(), CultureInfo.InvariantCulture);
							}
							catch (FormatException) 
							{
								// if we can't format the number, just save it as a string
								tempObject = tempString;
							}
							break;
                    
						default:
							throw new NotSupportedException("Do not know how to parse Field type "+tempFieldType);
					}
					attrs.Add(tempObject);
				}
        
				// ensure that the full record has been read.
				if (tempRecordLength < _Header.RecordLength)
				{
					byte[] tempbuff = new byte[_Header.RecordLength-tempRecordLength];
					tempbuff = _dbfStream.ReadBytes(_Header.RecordLength-tempRecordLength);
				}
        
				// add the row if it is not deleted.
				if (tempDeleted != '*')
				{
					foundRecord = true;
				}
			}
			return attrs;
		}
       


        /// <summary>
        /// Gets the header information for the dbase file.
        /// </summary>
        /// <returns>DbaseFileHeader contain header and field information.</returns>
        public dBaseHeader GetHeader()
        {
            if (_Header == null)
            {
                FileStream stream = new FileStream(_Filename, System.IO.FileMode.Open);
                _dbfStream = new BinaryReader(stream);

                _Header = new dBaseHeader();
                // read the header
                _Header.ReadHeader(_dbfStream);

                _dbfStream.Close();
                stream.Close();

            }
            return _Header;
        }

        
    }
}
