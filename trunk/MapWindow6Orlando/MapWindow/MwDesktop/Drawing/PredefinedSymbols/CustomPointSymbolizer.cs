//********************************************************************************************************
// Product Name: MapWindow.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for MapWindow.Drawing version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/22/2009 2:49:57 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.ComponentModel;
using MapWindow.Forms;
using System.Drawing.Design;
namespace MapWindow.Drawing
{


    /// <summary>
    /// CustomPointSymbolizer
    /// </summary>
    [Serializable]
    public class CustomPointSymbolizer : CustomSymbolizer
    {

        /// <summary>
        /// Creates a new CustomSymbolizer for symbolizing Points
        /// </summary>
        public CustomPointSymbolizer()
        {
            Symbolizer = new PointSymbolizer();
        }

        /// <summary>
        /// Creates a new Custom Point symbolizer with the specified properties
        /// </summary>
        /// <param name="uniqueName">the unique name</param>
        /// <param name="name">the name of the custom symbolizer</param>
        /// <param name="category">the map category of the custom symbolizer</param>
        /// <param name="symbolizer">the associated Point symbolizer</param>
        public CustomPointSymbolizer(string uniqueName, string name, string category, PointSymbolizer symbolizer)
        {
            base.UniqueName = uniqueName;
            base.Name = name;
            base.Category = category;
            base.Symbolizer = symbolizer;
        }

        /// <summary>
        /// Gets or sets the Point symbolizer
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(PointSymbolizerEditor), typeof(UITypeEditor))]
        public new IPointSymbolizer Symbolizer
        {
            get { return base.Symbolizer as IPointSymbolizer; }
            set { base.Symbolizer = value; }
        }



    }
}
