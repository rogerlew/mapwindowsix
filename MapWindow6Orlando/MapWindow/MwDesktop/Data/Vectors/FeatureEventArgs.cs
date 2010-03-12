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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2009 1:27:58 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MapWindow.Data
{


    /// <summary>
    /// FeatureEvent
    /// </summary>
    public class FeatureEventArgs: EventArgs
    {
        #region Private Variables
        private IFeature _feature;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureEvent
        /// </summary>
        public FeatureEventArgs(IFeature inFeature)
        {
            _feature = inFeature;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the feature being referenced by this event.
        /// </summary>
        public IFeature Feature
        {
            get { return _feature; }
            protected set { _feature = value; }
        }

        #endregion



    }
}
