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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2008 2:00:57 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// EnvelopeArgs
    /// </summary>
    public class EnvelopeArgs: EventArgs
    {
        #region Private Variables

        private IEnvelope _envelope;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of EnvelopeArgs
        /// </summary>
        public EnvelopeArgs(IEnvelope inEnvelope)
        {
            _envelope = inEnvelope;
        }

        #endregion

      
        #region Properties

        /// <summary>
        /// Gets the envelope specific to this event.
        /// </summary>
        public IEnvelope Envelope
        {
            get { return _envelope; }
            protected set { _envelope = value; }
        }


        #endregion



    }
}
