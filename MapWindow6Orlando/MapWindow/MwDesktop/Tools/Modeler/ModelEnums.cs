//********************************************************************************************************
// Product Name: MapWindow.Tools Enumerations for the Model
// Description:  Contains enumerations used in the model
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Tools
{
    /// <summary>
    /// Definitions for the shapes that components can have in the modeler
    /// </summary>
    public enum ModelShapes
    {
        /// <summary>
        /// Defines the Model Component as a Rectangle
        /// </summary>
        Rectangle,
        /// <summary>
        /// Defines the Model Component as a Triangle
        /// </summary>
        Triangle,
        /// <summary>
        /// Defines the Model Component as a Ellipse
        /// </summary>
        Ellipse,
        /// <summary>
        /// Defines an Arrow
        /// </summary>
        Arrow
    }

    /// <summary>
    /// Used internally to decided if a tool has executed, is done, or finished in error
    /// </summary>
    internal enum ToolExecuteStatus
    {
        /// <summary>
        /// The tool has not been run yet
        /// </summary>
        NotRun,

        /// <summary>
        /// The tool is currently executing
        /// </summary>
        Running,

        /// <summary>
        /// The tool finished running succesfully
        /// </summary>
        Done,

        /// <summary>
        /// The tool returned an error when executing
        /// </summary>
        Error
    }
}
