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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 2:42:50 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************



namespace MapWindow.Data.Img
{


    /// <summary>
    /// MapInfo
    /// </summary>
    public class MapInfo
    {
        #region Private Variables

        
        private string _proName;
        private Coordinate _upperLeftCenter;
        private Coordinate _lowerRightCenter;
        private Size _pixelSize;
        private string _units;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of MapInfo
        /// </summary>
        public MapInfo()
        {

        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map coordinats of the center of the lower right pixel
        /// </summary>
        public Coordinate LowerRightCenter
        {
            get { return _lowerRightCenter; }
            set { _lowerRightCenter = value; }
        }

        /// <summary>
        /// Gets or sets the size of a single pixel in map units
        /// </summary>
        public Size PixelSize
        {
            get { return _pixelSize; }
            set { _pixelSize = value; }
        }

        /// <summary>
        /// Gets or sets the string Projection Name
        /// </summary>
        public string ProName
        {
            get { return _proName; }
            set { _proName = value; }
        }

        /// <summary>
        /// Gets or sets the map units
        /// </summary>
        public string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Gets or sets the map coordinates of center of upper left pixel
        /// </summary>
        public Coordinate UpperLeftCenter
        {
            get { return _upperLeftCenter; }
            set { _upperLeftCenter = value; }
        }



        #endregion



    }
}
