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

namespace MapWindow.GeoMap
{


    /// <summary>
    ///
    /// </summary>
    public class GeoDrawVerifyArgs : MapDrawArgs
    {
        #region Private Variables

        private bool _handled;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoHandledDrawArgs
        /// </summary>
        public GeoDrawVerifyArgs(MapDrawArgs args)
            : base(args.Graphics, args.ClipRectangle, args.GeoGraphics)
        {
           
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the handled property
        /// </summary>
        public bool Handled
        {
            get { return _handled; }
            set { _handled = value; }
        }


        #endregion



    }
}
