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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/10/2009 2:13:31 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MapWindow.Components
{


    /// <summary>
    /// PluginToken
    /// </summary>
    public class PluginToken
    {
        #region Private Variables

        private string _name;
        private string _author;
        private string _version;
        private string _assemblyPath;
        private string _uniqueName;
        private Type _pluginType;
        private bool _enabled;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PluginToken
        /// </summary>
        public PluginToken(MapWindowPluginAttribute attribute, string assemblyPath, Type pluginType)
        {
            _name = attribute.Name;
            _version = attribute.Version;
            _author = attribute.Author;
            _assemblyPath = assemblyPath;
            _pluginType = pluginType;

        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Creates a new plugin of type TPlugin.  This can also return
        /// an interface that represents the specified plugin/provider.
        /// </summary>
        /// <typeparam name="TPlugin">The type to attempt to cast the instance member to</typeparam>
        /// <returns>An instance of the class/interface requested by TPlugin, or else null if the instance could not be created.</returns>
        public TPlugin CreateInstance<TPlugin>() where TPlugin: class
        {
            if(File.Exists(_assemblyPath) == false)
            {
                string tryPath = Application.StartupPath + "\\" + _assemblyPath;
                if (!File.Exists(tryPath))
                {
                    MessageBox.Show("Could not load this assembly: " + _assemblyPath + ", or " + tryPath);
                    return null;
                }
                _assemblyPath = tryPath;
            }
            Assembly asm = Assembly.LoadFrom(_assemblyPath);
            object result = asm.CreateInstance(_pluginType.FullName);
            return result as TPlugin;
        }

        /// <summary>
        /// Gets a boolean that is true if the specified type can become the specified interface.  This does not force
        /// an instantiation and therefore should be faster than instantiating the member and doing a type check.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns>Boolean, true if this token can Create an Instance of hte specified interface type.</returns>
        public bool CanBecome(Type interfaceType)
        {
            Type[] interfaces = PluginType.GetInterfaces();
            foreach (Type t in interfaces)
            {
                if (t == interfaceType)
                {
                    return true;
                }
            }
            return false;
        }
        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string author 
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// Gets or sets whether the current item is enabled.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        

        /// <summary>
        /// Gets or sets the name of the token
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the System.Type that can be instantiated by this token
        /// </summary>
        public Type PluginType
        {
            get { return _pluginType; }
            set { _pluginType = value; }
        }

        /// <summary>
        /// Gets or sets the string that should uniquely identify this plugin
        /// from any other similar plugins.  This is mainly used to save
        /// and load models with the correct tool.
        /// </summary>
        public string UniqueName
        {
            get { return _uniqueName; }
            set { _uniqueName = value; }
        }
        

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        

        

        #endregion



    }
}
