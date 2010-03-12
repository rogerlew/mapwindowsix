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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/21/2009 1:36:28 PM
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
    /// ILabelCategory
    /// </summary>
    public interface ILabelCategory : ICloneable
    {


        #region Methods

        /// <summary>
        /// Returns a shallow copy of this object cast as a LabelCategory.
        /// </summary>
        /// <returns>A shallow copy of this object.</returns>
        ILabelCategory Copy();
      

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string expression that controls the integration
        /// of field values into the label text.  This will not do calculations,
        /// but will allow multiple fields to be conjoined in a string expression,
        /// substituting a field value where each [FieldName] occurs.
        /// </summary>
        string Expression
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string filter expression that controls which features
        /// that this should apply itself to.
        /// </summary>
        string FilterExpression
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text symbolizer to use for this category
        /// </summary>
        ILabelSymbolizer Symbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text symbolizer to use for this category
        /// </summary>
        ILabelSymbolizer SelectionSymbolizer
        {
            get;
            set;
        }


        #endregion



    }
}
