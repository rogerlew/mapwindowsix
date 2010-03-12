//********************************************************************************************************
// Product Name: MapWindow.Tools.ToolInfo
// Description:  A class which contains information about a tool
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
    /// A class which holds the information about a specific tool
    /// </summary>
    public class ToolInfo
    {
        #region ---------------------- private variables

        private string _name;
        private string _uniqueName;
        private string _description;
        private string _toolTip;
        private string _category;
        private System.Drawing.Bitmap _icon;

        #endregion 

        #region ---------------------- Constructor

        /// <summary>
        /// Creates a empty ToolInfo so it can be inherited
        /// </summary>
        protected ToolInfo()
        { 
        }
        
        /// <summary>
        /// Creates an instance of the ToolInfo 
        /// </summary>
        /// <param name="name">name of the tool</param>
        /// <param name="uniqueName">unique name of the tool</param>
        /// <param name="description">description of the tool</param>
        /// <param name="toolTip">toolTip that appears when the user mouses over the treeview</param>
        /// <param name="category">category the tool should be placed in on the treeview</param>
        /// <param name="icon">the tools icon</param>
        public ToolInfo(string name, string uniqueName, string description, string toolTip, string category, System.Drawing.Bitmap icon)
        {
            _name = name;
            _uniqueName = uniqueName;
            _description = description;
            _toolTip = toolTip;
            _category = category;
            _icon = icon;
        }

        #endregion

        #region ---------------------- Public Attributes

        /// <summary>
        /// Gets the name of the tool as it appears in the tool treeview and tool dialog
        /// </summary>
        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        /// <summary>
        /// Gets the unique name for the tool, must be unique
        /// </summary>
        public string UniqueName
        {
            get { return _uniqueName; }
            protected set { _uniqueName = value; }
        }

        /// <summary>
        /// Gets the the description of the tool as it will appear in the tool dialog
        /// </summary>
        public string Description
        {
            get { return _description; }
            protected set { _description = value; }
        }

        /// <summary>
        /// Gets the tooltip which will appear over the tool in the treeview
        /// </summary>
        public string ToolTip
        {
            get { return _toolTip; }
            protected set { _toolTip = value; }
        }

        /// <summary>
        /// Gets the category the tool should be place in the tree view
        /// </summary>
        public string Category
        {
            get { return _category; }
            protected set { _category = value; }
        }

        /// <summary>
        /// Gets the icon for the tool that will appear in the tree view and tool dialog
        /// </summary>
        public System.Drawing.Bitmap Icon
        {
            get { return _icon; }
            protected set { _icon = value; }
        }

        #endregion
    }
}
