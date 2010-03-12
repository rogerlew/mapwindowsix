using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using MapWindow.Analysis.Logging;
using MapWindow.Components;
namespace MapWindow.Analysis.DataManagement.Shapefile.dBase
{
	/// <summary>
	/// This class aids in the writing of Dbase IV files. 
	/// </summary>
	/// <remarks>
	/// Attribute information of an ESRI Shapefile is written using Dbase IV files.
	/// </remarks>
	public class dBaseWriter
	{
		string _filename;
		BinaryWriter _writer;
		bool headerWritten = false;
		bool recordsWritten = false;
		dBaseHeader _header;

		/// <summary>
		/// Initializes a new instance of the DbaseFileWriter class.
		/// </summary>
        /// <param name="filename"></param>
		public dBaseWriter(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			_filename = filename;
			FileStream filestream  = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Write);
			_writer = new BinaryWriter(filestream);
		}	

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
		public void Write(dBaseHeader header)
		{
			if (header == null)
				throw new ArgumentNullException("header");
			if (recordsWritten)
				throw new InvalidOperationException("Records have already been written. Header file needs to be written first.");
			headerWritten = true;
			header.WriteHeader(_writer);
			_header = header;			
		}

        /// <summary>
        /// This appends the content of one datarow to a dBase file.
        /// </summary>
        /// <param name="columnValues">A DataRow to append to the dBase file</param>
        /// <param name="Table">A DataTable that gives some column information</param>
        /// <exception cref="ArgumentNullException">The columnValues parameter was null</exception>
        /// <exception cref="InvalidOperationException">Header records need to be written first.</exception>
        /// <exception cref="InvalidDataException">Table property of columnValues parameter cannot be null.</exception>
		public void Write(System.Data.DataRow columnValues, System.Data.DataTable Table)
		{

			if (columnValues == null)
            {
                ArgumentNullException ex = new ArgumentNullException("The columnValues parameter was null");
                LogManager.DefaultLogManager.Exception(ex);
				throw ex;
            }
			if (!headerWritten)
            {
				InvalidOperationException ex = new InvalidOperationException("Header records need to be written first.");
                LogManager.DefaultLogManager.Exception(ex);
				throw ex;
            }
            if (columnValues.Table == null)
            {
                InvalidDataException ex = new InvalidDataException("Table property of columnValues parameter cannot be null.");
                LogManager.DefaultLogManager.Exception(ex);
				throw ex;
            }
                
			_writer.Write((byte)0x20); // the deleted flag

            for (int i = 0; i < _header.NumFields; i++)
            {
              
                object columnValue = columnValues[_header.Fields[i].Name];
                if (columnValue == null || columnValue is DBNull)
                    WriteSpaces(_header.Fields[i].Length);
                if (columnValue is double)
                    Write((double)columnValue, _header.Fields[i].Length, _header.Fields[i].DecimalCount);
                else if (columnValue is float)
                    Write((float)columnValue, _header.Fields[i].Length, _header.Fields[i].DecimalCount);
                else if (columnValue is bool)
                    Write((bool)columnValue);
                else if (columnValue is string)
                {
                    int length = _header.Fields[i].Length;
                    Write((string)columnValue, length);
                }
                else if (columnValue is DateTime)
                    Write((DateTime)columnValue);
            }
			
		}

        /// <summary>
        /// Writes a set number of spaces
        /// </summary>
        /// <param name="numspaces"></param>
        public void WriteSpaces(int numspaces)
        {
            for (int I = 0; I < numspaces; I++)
            {
                _writer.Write(' ');
            }
        }
        /// <summary>
        /// Writes the entire contents of a data Table to the file.
        /// This will create a header from the Table information.
        /// </summary>
        /// <param name="Table">The Data Table to write.</param>
        public void Write(System.Data.DataTable Table)
        {
            _header = new dBaseHeader();
            
            foreach(System.Data.DataRow dr in Table.Rows)
            {
                Write(dr, Table);
            }
        }

        /// <summary>
        /// This appends an array of values as a new row.  Care should be taken to 
        /// ensure that the list has the correct number of columns and that the
        /// datatype from each column matches the header.
        /// </summary>
        /// <param name="columnValues">A System.ArrayList of values for one row to append to the dBase file</param>
        /// <exception cref="ArgumentNullException">The columnValues parameter was null</exception>
        /// <exception cref="InvalidOperationException">Header records need to be written first.</exception>
        /// <exception cref="InvalidDataException">The number of values does not match the number of columns for this dbf.</exception>
        public void Write(ArrayList columnValues)
        {
            if (columnValues == null)
            {
                ArgumentNullException ex = new ArgumentNullException("The columnValues parameter was null");
                LogManager.DefaultLogManager.Exception(ex);
                throw ex;
            }
            if (!headerWritten)
            {
                InvalidOperationException ex = new InvalidOperationException("Header records need to be written first.");
                LogManager.DefaultLogManager.Exception(ex);
                throw ex;
            }
            if (columnValues.Count != this._header.NumFields)
            {
                InvalidDataException ex = new InvalidDataException("The number of values does not match the number of columns for this dbf.");
                LogManager.DefaultLogManager.Exception(ex);
                throw ex;
            }

            _writer.Write((byte)0x20); // the deleted flag
            int i = 0;
            foreach (object columnValue in columnValues)
            {
                if (columnValue is double)
                    Write((double)columnValue, _header.Fields[i].Length, _header.Fields[i].DecimalCount);
                else if (columnValue is float)
                    Write((float)columnValue, _header.Fields[i].Length, _header.Fields[i].DecimalCount);
                else if (columnValue is bool)
                    Write((bool)columnValue);
                else if (columnValue is string)
                {
                    int length = _header.Fields[i].Length;
                    Write((string)columnValue, length);
                }
                else if (columnValue is DateTime)
                    Write((DateTime)columnValue);
                i++;
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
			string format="{0:";
			for(int i = 0; i < decimalCount;i++)
			{
				if (i==0)
					format = format + "0.";
				format = format + "0";
			}
			format=format + "}";
			string str = String.Format(format,number);
			for (int i=0; i< length-str.Length; i++)
				_writer.Write((byte)0x20);		
			foreach(char c in str)
				_writer.Write(c);			
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <param name="decimalCount"></param>
		public void Write(float number, int length, int decimalCount)
		{
			_writer.Write(String.Format("{0:000000000.000000000}",number));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
		public void Write(string text, int length)
		{
			// ensure string is not too big
			text = text.PadRight(length,' ');
			string dbaseString = text.Substring(0,length);

			// will extra chars get written??
			foreach(char c in dbaseString)
				_writer.Write(c);

			int extraPadding = length - dbaseString.Length;
			for(int i=0; i < extraPadding; i++)
				_writer.Write((byte)0x20);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
		public void Write(DateTime date)
		{
			_writer.Write(date.Year-1900);
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

        /// <summary>
        /// 
        /// </summary>
		public void Close()
		{
			_writer.Close();
		}
	}
}
