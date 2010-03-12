//********************************************************************************************************
// Product Name: MapWindow.Tools.ToolManagerToolInfo
// Description:  A class which contains information about a tool specifically for the ToolManger
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

namespace MapWindow.Tools
{
    class ToolManagerToolInfo: ToolInfo
    {
        private IToolProvider _ToolProviderAssembly;

        /// <summary>
        /// Creates an instance of ToolManagerToolInfo
        /// </summary>
        /// <param name="Info">The tool info</param>
        /// <param name="ToolProviderAssembly">The IToolProvider that can instantiate the tool manager info represents</param>
        public ToolManagerToolInfo(ToolInfo Info, IToolProvider ToolProviderAssembly)
        {
            base.Name = Info.Name;
            base.UniqueName = Info.UniqueName;
            base.Description = Info.Description;
            base.ToolTip = Info.ToolTip;
            base.Category = Info.Category;
            base.Icon = Info.Icon;
            _ToolProviderAssembly = ToolProviderAssembly;
        }

        /// <summary>
        /// Get the full filename that the tool's assembly is in
        /// </summary>
        public IToolProvider ToolProviderAssembly
        {
            get { return _ToolProviderAssembly; }
        }

    }
}
