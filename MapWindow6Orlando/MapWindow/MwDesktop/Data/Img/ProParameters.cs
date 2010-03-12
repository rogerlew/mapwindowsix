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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 3:02:36 PM
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
    /// ProParameters
    /// </summary>
    public class ProParameters
    {
        private ProType _proType;
        private int _proNumber;
        private string _proExeName;
        private string _proName;
        private int _proZone;
        private double[] _proParams;
        private Spheroid _proSpheroid;

    
        /// <summary>
        /// Creates a new instance of the ProParameters class
        /// </summary>
        public ProParameters()
        {
            _proParams = new double[15];
        }


        /// <summary>
        /// Gets or sets the string exectable name for external projectiosn
        /// </summary>
        public string ProExeName
        {
            get { return _proExeName; }
            set { _proExeName = value; }
        }

        /// <summary>
        /// Gets or sets the string projection name
        /// </summary>
        public string ProName
        {
            get { return _proName; }
            set { _proName = value; }
        }

        /// <summary>
        /// Gets or sets the projection number for internal projections
        /// </summary>
        public int ProNumber
        {
            get { return _proNumber; }
            set { _proNumber = value; }
        }

        /// <summary>
        /// Projection parameters array in the GCTP form
        /// </summary>
        public double[] ProParams
        {
            get { return _proParams; }
            set { _proParams = value; }
        }

        /// <summary>
        /// Gets or sets the projection type
        /// </summary>
        public ProType ProType
        {
            get { return _proType; }
            set { _proType = value; }
        }

        /// <summary>
        /// Gets or sets the projection zone (UTM, SP only)
        /// </summary>
        public int ProZone
        {
            get { return _proZone; }
            set { _proZone = value; }
        }

        /// <summary>
        /// The projection spheroid
        /// </summary>
        public Spheroid ProSpheroid
        {
            get { return _proSpheroid; }
            set { _proSpheroid = value; }
        }

        
        

    }
}
