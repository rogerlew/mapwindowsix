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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 2:57:50 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Data.Img
{


    /// <summary>
    /// Spheroid
    /// </summary>
    public class Spheroid
    {
        #region Private Variables

        private string _sphereName;
        private double _a;
        private double _b;
        private double _eSquared;
        private double _radius;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Spheroid
        /// </summary>
        public Spheroid()
        {

        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the semi-major axis of the ellipsoid
        /// </summary>
        public double A
        {
            get { return _a; }
            set { _a = value; }
        }

        /// <summary>
        /// Gets or sets the semi-minor axis of the ellipsoid
        /// </summary>
        public double B
        {
            get { return _b; }
            set { _b = value; }
        }

        /// <summary>
        /// Gets or sets the eccentricity squared
        /// </summary>
        public double ESquared
        {
            get { return _eSquared; }
            set { _eSquared = value; }
        }

        /// <summary>
        /// Gets or sets the radius of the sphere
        /// </summary>
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>
        /// Gets or sets the string name o the ellipsoid
        /// </summary>
        public string SphereName
        {
            get { return _sphereName; }
            set { _sphereName = value; }
        }

        #endregion



    }
}
