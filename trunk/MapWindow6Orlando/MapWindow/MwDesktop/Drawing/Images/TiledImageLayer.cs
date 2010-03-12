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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/6/2010 11:44:38 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using MapWindow.Data;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// TiledImageLayer
    /// </summary>
    public class TiledImageLayer : Layer, ITiledImageLayer
    {
        #region Private Variables

        private ITiledImage _dataSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TiledImageLayer
        /// </summary>
        public TiledImageLayer(ITiledImage dataset)
        {
            _dataSet = dataset;
        }

        #endregion

        #region Methods

        



        #endregion

        #region Properties

        public override IEnvelope Envelope
        {
            get
            {
                return _dataSet.Envelope;
            }
            protected set
            {
                // nothing to set here.
            }
        }


        public new ITiledImage DataSet
        {
            get { return _dataSet; }
            set { _dataSet = value; }
        }

        #endregion



    }
}
