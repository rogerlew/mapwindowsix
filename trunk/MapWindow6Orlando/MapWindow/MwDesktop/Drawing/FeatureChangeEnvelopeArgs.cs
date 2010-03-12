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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2008 11:15:25 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// FeatureChangeEnvelopeArgs
    /// </summary>
    public class FeatureChangeEnvelopeArgs : FeatureChangeArgs
    {
        #region Private Variables
        
        private IEnvelope _envelope;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureChangeEnvelopeArgs
        /// </summary>
        public FeatureChangeEnvelopeArgs(List<int> inChangedFeatures, IEnvelope inEnvelope):base(inChangedFeatures)
        {
            _envelope = inEnvelope;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the geographic envelope for the most recent selection event.
        /// </summary>
        public IEnvelope Envelope
        {
            get { return _envelope; }
            protected set { _envelope = value; }
        }

        #endregion



    }
}
