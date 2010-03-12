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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/16/2009 2:27:45 PM
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

namespace MapWindow.Data
{


    /// <summary>
    /// FieldJoinType
    /// </summary>
    public enum FieldJoinType
    {
        /// <summary>
        /// Output datasets have all fields from both input and output sets.  Fields with duplicate field names will be appended with a number.
        /// Features from this dataset may appear more than once if more than one valid intersection occurs with the features from the
        /// other featureset.
        /// </summary>
        All,
        /// <summary>
        /// The fields will be created from the fields in the other featureset.  
        /// </summary>
        ForeignOnly,
        /// <summary>
        /// All the fields from this FeatureSet are used, and all of the features from the other featureset are considered
        /// to be a single geometry so that features from this set will appear no more than once in the output set.
        /// </summary>
        LocalOnly,
        /// <summary>
        /// No fields will be copied, but features from this featureset will be considered independantly and added as separate
        /// features to the output featureset.
        /// </summary>
        None,
    }
}
