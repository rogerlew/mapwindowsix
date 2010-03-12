//********************************************************************************************************
// Product Name: MapWindow.Tools.DefaultToolProviderToolInfo
// Description:  A class which contains information about a tool specifically for DefaultToolProvider
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
    internal class DefaultToolProviderToolInfo: ToolInfo
    {
        private string _assemblyFileName;
        private string _toolClassName;
        private DateTime _dateFileModified;

        /// <summary>
        /// Creates an instance of DLLToolInfo
        /// </summary>
        /// <param name="tool"></param>
        /// <param name="AssemblyFileName"></param>
        /// <param name="ToolClassName"></param>
        /// <param name="DateFileModified">The dane the file was modified</param>
        public DefaultToolProviderToolInfo(ITool tool, string AssemblyFileName, string ToolClassName, DateTime DateFileModified)
        {
            base.Name = tool.Name;
            base.UniqueName = tool.UniqueName;
            base.Description = tool.Description;
            base.ToolTip = tool.ToolTip;
            base.Category = tool.Category;
            base.Icon = tool.Icon;
            _assemblyFileName = AssemblyFileName;
            _toolClassName = ToolClassName;
            _dateFileModified = DateFileModified;
        }

        /// <summary>
        /// Get the full filename that the tool's assembly is in
        /// </summary>
        public string AssemblyFileName
        {
            get { return _assemblyFileName; }
        }

        /// <summary>
        /// Gets the name of the tools class 
        /// </summary>
        public string ToolClassName
        {
            get { return _toolClassName; }
        }
    }
}
