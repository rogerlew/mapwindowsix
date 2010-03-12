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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/10/2009 1:52:43 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
namespace MapWindow.Components
{


    /// <summary>
    /// IPlugin is the most basic interface possible, and describes each possible plugin
    /// </summary>
    public class MapWindowPluginAttribute : Attribute
    {
        #region Private Variables
        private string _version;
        private readonly string _name;
        private string _author;
        private string _uniqueName;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of IPlugin
        /// </summary>
        public MapWindowPluginAttribute(string name)
        {
            _name = name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a string from this attribute
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string value = "Name:" + _name;
            if (_author != null)
            {
                value += " Author:" + _author;
            }
            if (_version != null)
            {
                value += " Version:" + _version.ToString();
            }
            return value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the author of this plugin
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// Gets the name of this plugin
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets or sets the version of the attribute
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// Gets or sets the UniqueName for this plugin
        /// </summary>
        public string UniqueName
        {
            get { return _uniqueName; }
            set { _uniqueName = value; }
        }
       

        #endregion



    }
}
