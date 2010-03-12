//********************************************************************************************************
// Product Name: MapWindow.Tools.IToolProvider
// Description:  Interface for providing tools for the MapWindow toolbox
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jan, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MapWindow.Tools
{
    /// <summary>
    /// Interface for providing tools for the MapWindow toolbox
    /// </summary>
    public interface IToolProvider
    {
        /// <summary>
        /// This runs after the ToolProvider has been created and after the tool Manager has poled the tool provider for its Unique ID
        /// Once this method is called the ToolProvider should be to return a list of available tools when ToolInfoList is called
        /// </summary>
        /// <param name="ToolSettings">The ToolProvider's settings string</param>
        void Initialize(XmlDocument ToolSettings);

        /// <summary>
        /// Returns the ToolProvider's Unique ID
        /// </summary>
        string UniqueID
        {
            get;
        }

        /// <summary>
        /// Gets the list tools avaible from the ToolProvider in the form of ToolInfo
        /// </summary>
        List<ToolInfo> ToolInfoList
        {
            get;
        }

        /// <summary>
        /// Gets or Sets a string that holds the ToolProviders settings. The string is stored by the ToolManager and is populated when the ToolProvider is created
        /// and saved whenever the ChangeSettings method is called.
        /// </summary>
        List<string> ProviderSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Changes the settings string of the provider, This can be used to show a dialog box. It must display something to the user!
        /// </summary>
        void ChangeSettings();

        /// <summary>
        /// Creates a new instance of a tool based on its UniqueName
        /// </summary>
        /// <param name="UniqueName">The unique name of the tool</param>
        /// <returns>Returns an new instance of the tool or NULL if the tools unique name doesn't exist in the manager</returns>
        ITool NewTool(string UniqueName);
    }
}
