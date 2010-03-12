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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/5/2009 12:14:19 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;

namespace MapWindow.Drawing
{


    /// <summary>
    /// SnapShotEventArgs
    /// </summary>
    public class SnapShotEventArgs : EventArgs
    {
        #region Private Variables

        private Bitmap _picture;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SnapShotEventArgs
        /// </summary>
        public SnapShotEventArgs(Bitmap inPicture)
        {
            _picture = inPicture;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the picture that was taken by the snapshot
        /// </summary>
        public Bitmap Picture
        {
            get { return _picture; }
            protected set { _picture = value; }
        }


        #endregion



    }
}
