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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/10/2009 1:39:44 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MapWindow.Plugins;

namespace MapWindow.Components
{


    /// <summary>
    /// This is the base manager that handles the plug-in tokenization for the various
    /// plug-ins that are offered through MapWindow.
    /// </summary>
    public class TokenManager 
    {
        #region Private Variables

        private List<string> _directory;
        private List<string> _assembly;
        private List<PluginToken> _tokens;

        #endregion

        

        #region Methods

        /// <summary>
        /// Searches the directory in an effort to find providers/plugins of type TPlugin
        /// </summary>
        public virtual void UpdateStore()
        {
            _tokens = new List<PluginToken>();
            foreach (string directory in Directories)
            {
                UpdateDirectory(directory);
            }
        }

        /// <summary>
        /// Obtains a list of all the plugins that instantiate classes that match the 
        /// specified input type.
        /// </summary>
        public List<TPlugin> GetPlugins<TPlugin>() where TPlugin: class
        {
            List<TPlugin> result = new List<TPlugin>();
            foreach (PluginToken token in _tokens)
            {
                if (token.PluginType is TPlugin)
                {
                    result.Add(token.CreateInstance<TPlugin>());
                }
            }
            return result;
        }

        /// <summary>
        /// Includes plugins from the specified directory
        /// </summary>
        /// <param name="directoryPath"></param>
        public void UpdateDirectory(string directoryPath)
        {
            string actualDir = directoryPath;
            if (Directory.Exists(directoryPath) == false)
            {
                actualDir = Application.StartupPath + Path.DirectorySeparatorChar + directoryPath;
                if(Directory.Exists(actualDir) == false)return;
            }
            string[] files = Directory.GetFiles(actualDir, "*.dll", SearchOption.AllDirectories);
            foreach (string path in files)
            {
                UpdateAssembly(path);
            }

        }

        /// <summary>
        /// Includes plugins from the specified assembly
        /// </summary>
        /// <param name="assemblyPath"></param>
        public void UpdateAssembly(string assemblyPath)
        {
            if (_tokens == null) _tokens = new List<PluginToken>();
            if (Path.GetExtension(assemblyPath) != ".dll") return;
            if (assemblyPath.Contains("Interop")) return;
            if (assemblyPath.Contains("MapWindow.dll")) return;

            try
            {
                try
                {
                    AssemblyName.GetAssemblyName(assemblyPath);
                }
                catch (BadImageFormatException)
                {
                    System.Diagnostics.Debug.WriteLine("Skipping non .Net library: " + assemblyPath);
                    return;
                }
                Assembly asm = Assembly.LoadFrom(assemblyPath);
                var attributes = asm.GetCustomAttributes(typeof(MapWindowPluginAssembly), false);
                if (attributes.Length == 0) return; // don't bother with a dll unless it marks itself as a MW plugin assembly
                Type[] classes = asm.GetTypes();
                try
                {
                    foreach (Type cls in classes)
                    {
                        if (cls.ContainsGenericParameters || !cls.IsClass || cls.IsAbstract) continue;
                        if (cls.IsCOMObject && Type.GetType("Mono.Runtime") != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Mono cannot work with COM objects.");
                            break; // if one is a com object, they will all be
                        }
                        object[] atts = cls.GetCustomAttributes(typeof(MapWindowPluginAttribute), true);
                        if (atts.Length == 0) continue;
                        _tokens.Add(new PluginToken(atts[0] as MapWindowPluginAttribute, assemblyPath, cls));
                    }
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            catch(Exception ex2)
            {
                System.Diagnostics.Debug.WriteLine(ex2.ToString());
            }

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the full paths of the specific assembly to attempt to load.
        /// </summary>
        public List<string> Assemblies
        {
            get { return _assembly; }
            set { _assembly = value; }
        }


        /// <summary>
        /// Gets or sets the directories to search for the specified plugin provider.
        /// </summary>
        public List<string> Directories
        {
            get { return _directory; }
            set { _directory = value; }
        }


        /// <summary>
        /// Gets the list of plugin tokens
        /// </summary>
        public List<PluginToken> Tokens
        {
            get { return _tokens; }
        }

      
        #endregion



    }
}
