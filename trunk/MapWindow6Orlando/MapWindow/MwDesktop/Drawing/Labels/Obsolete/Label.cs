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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2008 9:22:54 AM
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

namespace MapWindow.Drawing
{


    /// <summary>
    /// Label
    /// </summary>
    public class MapLabel : ILabel
    {
        /// <summary>
        /// Occurs when the Symbolizer for this label is changed.
        /// </summary>
        public event EventHandler<TextSymbolChangedEventArgs> SymbolChanged;

        #region Private Variables

        private Coordinate _anchorPoint;
        private string _text;
        private ILabelSymbolizer _symbolizer;
        private ILabelSymbolizer _selectionSymbolizer;
        private int _index;
        private ITextSymbolGroup _parent;
       
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a Label
        /// </summary>
        public MapLabel()
        {

        }

       
        /// <summary>
        /// Creates a new label with the specified text, anchor point, and symbolizer
        /// </summary>
        /// <param name="inText">The string text for this label</param>
        /// <param name="inAnchorPoint">The ICoordinate specifying the geographic anchor point for this label</param>
        /// <param name="inSymbolizer">The symbolizer to use for this label.</param>
        public MapLabel(string inText, Coordinate inAnchorPoint, ILabelSymbolizer inSymbolizer)
        {
            _text = inText;
            _anchorPoint = inAnchorPoint;
            _symbolizer = inSymbolizer;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic position for the anchoring the label.  The relationship
        /// between this and the text depends on the horizontal alignment.
        /// </summary>
        public Coordinate AnchorPoint
        {
            get { return _anchorPoint; }
            set { _anchorPoint = value; }
        }

        /// <summary>
        /// Gets the integer index for this label
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        /// <summary>
        /// Gets the Symbol group that currently contains this label.
        /// </summary>
        public ITextSymbolGroup Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the text that appears on this layer.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets the symbolizer for this specific label.  This allows customization for
        /// individual labels, but at the same time can allow many labels to point to one set 
        /// of symbolic characteristics.
        /// </summary>
        public ILabelSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            set 
            {
                ILabelSymbolizer oldSymbol = _symbolizer;
                _symbolizer = value;
                OnSymbolChanged(oldSymbol, value);
            }
        }

        /// <summary>
        /// Gets or sets the symbolizer that is being used.  Symbol groups are defined by the
        /// original symbolizer, and there is only one selection symbolizer per group.  This
        /// simply is a shortcut to accessing the symbolgroup's selection symbolizer.
        /// </summary>
        public ILabelSymbolizer SelectionSymbolizer
        {
            get { return _selectionSymbolizer; }
            set 
            {
   
                _selectionSymbolizer = value;
            }
        }


        #endregion

        #region protected Methods

        /// <summary>
        /// Fires the SymbolChanged event
        /// </summary>
        /// <param name="oldSymbolizer">The old symbol</param>
        /// <param name="newSymbolizer">The new symbol</param>
        protected virtual void OnSymbolChanged(ILabelSymbolizer oldSymbolizer, ILabelSymbolizer newSymbolizer)
        {
            if (SymbolChanged != null) SymbolChanged(this, new TextSymbolChangedEventArgs(this, oldSymbolizer, newSymbolizer));
        }

        #endregion


    }
}
