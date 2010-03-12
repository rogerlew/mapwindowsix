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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2008 11:46:40 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MapWindow.Map
{


    /// <summary>
    /// ChunkEventArgs
    /// </summary>
    public class ChunkEventArgs : EventArgs
    {
        #region Private Variables

        private int _currentChunk;
        private int _numChunks;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ChunkEventArgs
        /// </summary>
        public ChunkEventArgs(int inCurrentChunk, int inNumChunks)
        {
            _currentChunk = inCurrentChunk;
            _numChunks = inNumChunks;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current chunk for this event
        /// </summary>
        public int CurrentChunk
        {
            get { return _currentChunk; }
            protected set { _currentChunk = value; }
        }

        /// <summary>
        /// Gets the number of chunks
        /// </summary>
        public int NumChunks
        {
            get { return _numChunks; }
            protected set { _numChunks = value; }
        }

        #endregion



    }
}
