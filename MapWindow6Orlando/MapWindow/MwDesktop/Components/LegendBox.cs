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
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/3/2008 1:28:26 PM
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

namespace MapWindow.Components
{


    /// <summary>
    /// A Legend Box encapsulates the basic drawing information for an item.
    /// This will not capture information about sub-items.
    /// </summary>
    public class LegendBox
    {
        #region Private Variables

        private Rectangle _bounds;
        private ILegendItem _item;
        private Rectangle _checkbox;
        private Rectangle _expandBox;
        private Rectangle _textBox;
        private Rectangle _symbolBox;
        private int _indent;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LegendBox
        /// </summary>
        public LegendBox()
        {

        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bounds for this LegendBox
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets the actual item that this bounds is associated with
        /// </summary>
        public ILegendItem Item
        {
            get { return _item; }
            set { _item = value; }
        }

        /// <summary>
        /// If this item is in checkbox mode, then this is the physical location of the checkbox
        /// </summary>
        public Rectangle CheckBox
        {
            get { return _checkbox; }
            set { _checkbox = value; }
        }

        /// <summary>
        /// IF this item is a groupable item, this is the region for the expanding box for the item
        /// </summary>
        public Rectangle ExpandBox
        {
            get {return _expandBox; }
            set { _expandBox = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle that corresponds with text.
        /// </summary>
        public Rectangle Textbox
        {
            get { return _textBox; }
            set { _textBox = value; }
        }

        /// <summary>
        /// gets or sets the symbol box
        /// </summary>
        public Rectangle SymbolBox
        {
            get { return _symbolBox; }
            set { _symbolBox = value; }
        }

        /// <summary>
        /// Gets or sets the integer number of indentations.  This should be used
        /// in coordination with whatever the indentation amount is for the specific legend.
        /// </summary>
        public int Indent
        {
            get { return _indent; }
            set { _indent = value; }
        }

      

        #endregion



    }
}
