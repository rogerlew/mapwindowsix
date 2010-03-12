using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace MapWindow.Analysis.DataManagement.Shapefile.dBase
{
	/// <summary>
	/// Class that allows records in a dbase file to be enumerated.
	/// </summary>
	public class dBaseReader  : IEnumerable
	{

        private dBaseHeader _header = null;
		private string _filename;

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the DbaseFileReader class.
		/// </summary>
		public dBaseReader(string filename) 
		{
			if (filename==null)
			{
				throw new ArgumentNullException(filename);
			}
			// check for the file existing here, otherwise we will not get an error
			//until we read the first record or read the header.
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException(String.Format("Could not find file \"{0}\"",filename));
			}
			_filename = filename;
			
		}
		
		#endregion

		#region Methods

		/// <summary>
		/// Gets the header information for the dbase file.
		/// </summary>
		/// <returns>DbaseFileHeader contain header and field information.</returns>
		public dBaseHeader GetHeader() 
		{
			if (_header==null)
			{
				FileStream stream = new FileStream(_filename, System.IO.FileMode.Open);
				BinaryReader dbfStream = new BinaryReader(stream);

				_header = new dBaseHeader();
				// read the header
				_header.ReadHeader(dbfStream);

				dbfStream.Close();
				stream.Close();

			}
			return _header;
		}
		
		#endregion

        #region Properties

        /// <summary>
        /// Gets the filename being used by this reader
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            protected set { _filename = value; }
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
		/// Gets the object that allows iterating through the members of the collection.
		/// </summary>
		/// <returns>An object that implements the IEnumerator interface.</returns>
		public System.Collections.IEnumerator GetEnumerator()
		{
			return new DbaseFileEnumerator(this);
		}

		#endregion		
	}
}
