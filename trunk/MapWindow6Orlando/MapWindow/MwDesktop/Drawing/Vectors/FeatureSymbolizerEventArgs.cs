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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/20/2009 3:18:22 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MapWindow.Drawing
{


    /// <summary>
    /// FeatureSymbolizerEventArgs
    /// </summary>
    public class FeatureSymbolizerEventArgs : EventArgs
    {
        #region Private Variables

        private IFeatureSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureSymbolizerEventArgs
        /// </summary>
        public FeatureSymbolizerEventArgs(IFeatureSymbolizer symbolizer)
        {
            _symbolizer = symbolizer;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the feature symbolizer for this event
        /// </summary>
        public IFeatureSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            protected set { _symbolizer = value; }
        }


        #endregion



    }
}
