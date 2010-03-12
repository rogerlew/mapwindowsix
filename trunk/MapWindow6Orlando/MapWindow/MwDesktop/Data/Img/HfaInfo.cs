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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 3:18:45 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;


namespace MapWindow.Data.Img
{


    /// <summary>
    /// HfaInfo
    /// </summary>
    public class HfaInfo
    {
        #region Private Variables

        private string _path;
        private string _filename;
        private string _igeFilename;
        private HfaAccess _access;
        private long _endOfFile; // using long instead of guint32
        private long _rootPos;
        private long _dictionaryPos;
        private short _entryHeaderLength;
        private int _version;
        private Boolean _treeDirty;
        private FileStream _fp;
        private Dictionary<string, HfaType> _dictionary;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HfaInfo
        /// </summary>
        public HfaInfo()
        {
            _dictionary = new Dictionary<string, HfaType>();
        }

        #endregion

        #region Methods

        


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the access type
        /// </summary>
        public HfaAccess Access
        {
            get { return _access; }
            set { _access = value; }
        }

        /// <summary>
        /// Gets or sets a dictionary for looking up types based on string type names.
        /// </summary>
        public Dictionary<string, HfaType> Dictionary
        {
            get { return _dictionary; }
            set { _dictionary = value; }
        }

        /// <summary>
        /// The directory path
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        
        /// <summary>
        /// The string filename sans path
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// sans path
        /// </summary>
        public string IgeFilename
        {
            get { return _igeFilename; }
            set { _igeFilename = value; }
        }

        public long EndOfFile
        {
            get { return _endOfFile; }
            set { _endOfFile = value; }
        }

        public long RootPos
        {
            get { return _rootPos; }
            set { _rootPos = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public long DictionaryPos
        {
            get { return _dictionaryPos; }
            set { _dictionaryPos = value; }
        }

        public short EntryHeaderLength
        {
            get { return _entryHeaderLength; }
            set { _entryHeaderLength = value; }
        }

        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public bool TreeDirty
        {
            get { return _treeDirty; }
            set { _treeDirty = value; }
        }

        /// <summary>
        /// The file stream
        /// </summary>
        public FileStream Fp
        {
            get { return _fp; }
            set { _fp = value; }
        }

        #endregion



    }
}
