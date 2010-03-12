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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/18/2008 11:44:37 AM
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

namespace MapWindow.Map
{


    /// <summary>
    /// GeoDrawTextArgs
    /// </summary>
    public class MapDrawTextArgs
    {
        #region Private Variables

        private Brush _fontBrush;
        private Pen _borderPen;
        private Brush _backBrush;
        private ILabelSymbolizer _symbolizer;
        private MapDrawArgs _drawArgs;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoDrawTextArgs
        /// </summary>
        public MapDrawTextArgs(MapDrawArgs args, ILabelSymbolizer symbolizer)
        {
            _symbolizer = symbolizer;
            _drawArgs = args;
            _fontBrush = new SolidBrush(symbolizer.FontColor);
            _backBrush = new SolidBrush(symbolizer.BackColor);
            _borderPen = new Pen(symbolizer.BorderColor);

        }

        #endregion

        #region Methods

        /// <summary>
        /// Disposes the font brush, border pen and background brush
        /// </summary>
        public void Dispose()
        {
            _fontBrush.Dispose();
            _borderPen.Dispose();
            _backBrush.Dispose();
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets the TextSymbolizer for this 
        /// </summary>
        public ILabelSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            protected set { _symbolizer = value; }
        }
        
        /// <summary>
        /// Gets the GeoDrawArgs
        /// </summary>
        public MapDrawArgs DrawArgs
        {
            get { return _drawArgs; }
            protected set { _drawArgs = value; }
        }

        /// <summary>
        /// Gets the brush for drawing the background.
        /// </summary>
        public Brush BackBrush
        {
            get { return _backBrush; }
            protected set { _backBrush = value; }
        }

        /// <summary>
        /// Gets the border pen
        /// </summary>
        public Pen BorderPen
        {
            get { return _borderPen; }
            set { _borderPen = value; }
        }
        
        /// <summary>
        /// Gets the brush used for drawing fonts.
        /// </summary>
        public Brush FontBrush
        {
            get { return _fontBrush; }
            set { _fontBrush = value; }
        }

        #endregion



    }
}
