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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/15/2009 4:32:44 PM
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
using System.Windows.Forms;

namespace MapWindow.Data
{


    /// <summary>
    /// ScaleBarInfo
    /// </summary>
    public class ScaleBarInfo
    {
        #region Private Variables

        private bool _visible;
        private ScaleBarPositions _position;
        private string _unit;
        private Color _foreColor;
        private Color _backColor;
        

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ScaleBarInfo
        /// </summary>
        public ScaleBarInfo()
        {

        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the backcolor
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Gets or sets the forecolor
        /// </summary>
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        /// <summary>
        /// Gets or sets what position that the scalebar should load in
        /// </summary>
        public ScaleBarPositions Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Gets or sets the unit for the scalebar.
        /// </summary>
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        /// <summary>
        /// Gets or sets whether to show the scale bar.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        #endregion



    }
}
