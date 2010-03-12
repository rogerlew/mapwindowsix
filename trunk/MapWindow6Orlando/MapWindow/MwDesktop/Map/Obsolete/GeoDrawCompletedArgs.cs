//********************************************************************************************************
// Product Name: GeoMap.Drawing.exe 
// Description:  An open source drawing pad that is super simple, but extendable
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from GeoMap.Drawing.exe
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/30/2008 1:16:28 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
namespace MapWindow.GeoMap
{


    /// <summary>
    ///
    /// </summary>
    public class GeoDrawCompletedArgs : MapDrawArgs
    {
        #region Private Variables

      
        private Exception _exception;
        private bool _cancelled;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoHandledDrawArgs
        /// </summary>
        public GeoDrawCompletedArgs(MapDrawArgs args, bool inCancelled)
            : base(args.Graphics, args.ClipRectangle, args.GeoGraphics)
        {
            _cancelled = inCancelled;
          

        }
        /// <summary>
        /// Creates a new instance of GeoHandledDrawArgs
        /// </summary>
        public GeoDrawCompletedArgs(MapDrawArgs args, Exception inException)
            : base(args.Graphics, args.ClipRectangle, args.GeoGraphics)
        {
         
            _exception = inException;

        }
        

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// If the drawing was cancelled then this will be true
        /// </summary>
        public bool Cancelled
        {
            get { return _cancelled; }
            protected set { _cancelled = value; }
        }

        /// <summary>
        /// If an exception was thrown then this will not be null
        /// </summary>
        public Exception Exception
        {
            get { return _exception; }
            protected set { _exception = value; }
        }

        #endregion



    }
}
