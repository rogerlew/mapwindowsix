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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2008 9:56:23 AM
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
    /// ILabel
    /// </summary>
    public interface ILabel
    {

        #region Events

        /// <summary>
        /// Occurs when the Symbolizer for this label is changed.
        /// </summary>
        event EventHandler<TextSymbolChangedEventArgs> SymbolChanged;

        #endregion


        #region Methods



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic position for the anchoring the label.  The relationship
        /// between this and the text depends on the horizontal alignment.
        /// </summary>
        Coordinate AnchorPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the integer index for this label
        /// </summary>
        int Index
        {
            get;
        }

        /// <summary>
        /// Gets the Symbol group that currently contains this label.
        /// </summary>
        ITextSymbolGroup Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text that appears on this layer.
        /// </summary>
        string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbolizer for this specific label.  This allows customization for
        /// individual labels, but at the same time can allow many labels to point to one set 
        /// of symbolic characteristics.
        /// </summary>
        ILabelSymbolizer Symbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbolizer that is being used.  Symbol groups are defined by the
        /// original symbolizer, and there is only one selection symbolizer per group.  This
        /// simply is a shortcut to accessing the symbolgroup's selection symbolizer.
        /// </summary>
        ILabelSymbolizer SelectionSymbolizer
        {
            get;
            set;
        }

       


        #endregion



    }
}
