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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 2:54:03 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Data.Img
{


    /// <summary>
    /// Datum
    /// </summary>
    public class Datum
    {
        #region Private Variables

        private string _name;
        private DatumType _type;
        private double[] _params;
        private string _gridname;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Datum
        /// </summary>
        public Datum()
        {
            _params = new double[7];
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the datum
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the datum type
        /// </summary>
        public DatumType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets or sets the parameters for the Parametric datum type
        /// </summary>
        public double[] Params
        {
            get { return _params; }
            set { _params = value; }
        }

        /// <summary>
        /// Gets or sets the string name of the grid file
        /// </summary>
        public string GridName
        {
            get { return _gridname; }
            set { _gridname = value; }
        }

        #endregion



    }
}
