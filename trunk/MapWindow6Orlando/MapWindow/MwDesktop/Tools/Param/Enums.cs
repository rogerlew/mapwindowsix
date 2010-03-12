//********************************************************************************************************
// Product Name: MapWindow.Tools.Enums
// Description:  An Enumeration defining all of the parameter types which can be passed back from a ITool
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Tools.Param
{
    /// <summary>
    /// Defines weather or not to add the parameter to the model
    /// </summary>
    public enum ShowParamInModel
    {
        /// <summary>
        /// Always add the parameter to the model
        /// </summary>
        Always,
        /// <summary>
        /// Show the parameter in the model
        /// </summary>
        Yes,
        /// <summary>
        /// Don't show the parameter in the model
        /// </summary>
        No
    }

    /// <summary>
    /// Defines the data types which can be parameters for a ITool
    /// </summary>
    public enum ParamEnum
    {
        /// <summary>
        /// Defines a parameter of type Int which takes a max/min and default value
        /// </summary>
        IntParam,
        /// <summary>
        /// Defines a parameter of type Double which takes a max/min and default value
        /// </summary>
        DoubleParam,
        /// <summary>
        /// Defines a parameter which specifies a raster 
        /// </summary>
        RasterParam,
        /// <summary>
        /// Defines a parameter which specifies a Feature Set 
        /// </summary>
        FeatureSetParam,
        /// <summary>
        /// Defines a parameter which specifies a Point Feature Set 
        /// </summary>
        PointFeatureSetParam,
        /// <summary>
        /// Defines a parameter which specifies a Line Feature Set 
        /// </summary>
        LineFeatureSetParam,
        /// <summary>
        /// Defines a parameter which specifies a Polygon Feature Set 
        /// </summary>
        PolygonFeatureSetParam,
        /// <summary>
        /// Defines a parameter which specifies a string 
        /// </summary>
        StringParam,
        /// <summary>
        /// Defines a parameter which specifies a boolean value
        /// </summary>
        BooleanParam,
        /// <summary>
        /// Defines a parameter that is presented by a comboBox in the form
        /// </summary>
        ListParam
    }
}
