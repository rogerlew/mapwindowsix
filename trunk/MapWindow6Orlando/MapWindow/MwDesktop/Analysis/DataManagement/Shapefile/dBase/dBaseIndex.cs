//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://www.gnu.org/licenses/lgpl.html and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Analysis.DataManagement.Shapefile.dBase
{
    /// <summary>
    /// Scans through a dbf file and peaks at each row to see if it has been deleted.
    /// If not, then it reads the index value.
    /// </summary>
    public class dBaseIndex : Object
    {

        #region Private Variables

        private List<long> _offsets;
        private string _filename;
        private dBaseHeader _header;

        #endregion

        #region Methods

        /// <summary>
        /// This will return a copy of the index, but also preserve
        /// a master copy in this object that is read-only.
        /// </summary>
        /// <param name="filename">The filename of the dbf file to index</param>
        /// <returns>returns a List of long offsets.  The index in the list is the row value.</returns>
        public List<long> GetIndex(string filename)
        {
            List<long> OffsetsCopy = new List<long>();
            _offsets = new List<long>();
            if (filename == null)
            {
                throw new ArgumentNullException(filename);
            }
            // check for the file existing here, otherwise we will not get an error
            //until we read the first record or read the header.
            if (!System.IO.File.Exists(filename))
            {
                throw new System.IO.FileNotFoundException(String.Format("Could not find file \"{0}\"", filename));
            }
            _filename = filename;

            _header = new dBaseHeader();
            // read the header
            System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader binaryreader = new System.IO.BinaryReader(stream);
            _header.ReadHeader(binaryreader);

            int iCurrentRecord = 0;
            long CurrentOffset = _header.HeaderLength;
            byte DeleteChar = Convert.ToByte('*');
            int RowLength = _header.RecordLength;
            while (iCurrentRecord < _header.NumRecords)
            {
                int Val = stream.ReadByte();
                if (stream.ReadByte() != DeleteChar)
                {
                    _offsets.Add(CurrentOffset);
                    OffsetsCopy.Add(CurrentOffset);
                    iCurrentRecord++;
                }
                stream.Seek(RowLength - 1, System.IO.SeekOrigin.Current);
                CurrentOffset += RowLength;
            }
            binaryreader.Close();
            stream.Close();
            binaryreader.Close();
            return OffsetsCopy;




        }


        #endregion

        #region Properties

        /// <summary>
        /// Returns the Long offset of the specified row in the database
        /// </summary>
        /// <param name="index">The index to edit</param>
        /// <returns></returns>
        public long this[int index]
        {
            get
            {
                return _offsets[index];
            }
        }

        #endregion

       

    }
}
