//********************************************************************************************************
// Product Name: MapWindow.Tools.DefaultToolProvider
// Description:  The default tool provider. Scans a folder for .dlls that contain assemblies implementing
//               the ITool interface. Tools found are added to an internal list and can be instantiated
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
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Jan, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MapWindow.Tools
{
    /// <summary>
    /// Scans a folder for .dlls that contain assemblies implementing
    /// the ITool interface. If it finds any it adds them to an internal list.
    /// </summary>
    public class DefaultToolProvider : IToolProvider
    {
        private Dictionary<string, DefaultToolProviderToolInfo> _toolInfoList;
        private List<string> _toolDirectories;

        #region ------------------ public methods

        /// <summary>
        /// Creates an instance of the DLLToolProvider and scans for tools
        /// </summary>
        public void Initialize(XmlDocument ProviderSettings)
        {
            _toolInfoList = new Dictionary<string, DefaultToolProviderToolInfo>();
            _toolDirectories = new List<string>();
            _toolDirectories.Add(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "Tools");
            foreach (XmlNode child in ProviderSettings.ChildNodes[0].ChildNodes)
            {
                if (child.Name == "Path")
                {
                    for (int i = 0; i < child.Attributes.Count; i++)
                    {
                        if (child.Attributes[i].Name  == "Value")
                        {
                            if (! _toolDirectories.Contains(child.Attributes[i].Value)) _toolDirectories.Add(child.Attributes[i].Value);
                        }
                    }
                }
            }

            LoadToolsFromDirectories();
        }

        /// <summary>
        /// Returns the ToolProvider's Unique ID
        /// </summary>
        public string UniqueID
        {
            get { return "DefaultToolProvider"; }
        }

        /// <summary>
        /// Gets the list tools avaible from the ToolProvider in the form of ToolInfo
        /// </summary>
        /// 
        public List<ToolInfo> ToolInfoList
        {
            get 
            { 
                List<ToolInfo> tempToolList = new List<ToolInfo>();
                foreach (ToolInfo entry in _toolInfoList.Values)
                    tempToolList.Add(entry as ToolInfo);
                return tempToolList;
            }
        }

        /// <summary>
        /// Gets or Sets a string that holds the ToolProviders settings. The string is stored by the ToolManager and is populated when the ToolProvider is created
        /// and saved whenever the ChangeSettings method is called.
        /// </summary>
        public List<string> ProviderSettings
        {
            get;
            set;
        }

        /// <summary>
        /// This shows a simple dialog explaining that the default tool provider always scans the same folders as the ToolManager
        /// </summary>
        public void ChangeSettings()
        {
            System.Windows.Forms.MessageBox.Show(MapWindow.MessageStrings.DefaultToolproviderSettingsDialogText, MapWindow.MessageStrings.DefaultToolproviderSettingsDialogTitle, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Creates a new instance of a tool based on its UniqueName
        /// </summary>
        /// <param name="UniqueName">The unique name of the tool</param>
        /// <returns>Returns an new instance of the tool or NULL if the tools unique name doesn't exist in the manager</returns>
        public ITool NewTool(string UniqueName)
        {
            ITool tool = null;
            if (_toolInfoList.ContainsKey(UniqueName))
            {
                try
                {
                    System.Reflection.Assembly asm;
                    asm = System.Reflection.Assembly.LoadFrom(_toolInfoList[UniqueName].AssemblyFileName);
                    tool = asm.CreateInstance(_toolInfoList[UniqueName].ToolClassName) as ITool;
                    if (tool != null)
                    {
                        tool.Initialize();
                        return tool;
                    }
                }
                catch //If the assembly fails to be generated
                {
                    return null;
                }
            }
            return null;
        }

        #endregion

        #region ------------------ private methods

        /// <summary>
        /// This should be called once all the permitted directories have been set in the code.
        /// </summary>
        private void LoadToolsFromDirectories()
        {
            foreach (string directory in _toolDirectories)
            {
                if (System.IO.Directory.Exists(directory))
                {
                    string[] files = System.IO.Directory.GetFiles(directory, "*.dll", System.IO.SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        LoadToolsFromAssembly(file);
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Given a string filename for the "*.dll" file, this will attempt to load a ITool from the .dll
        /// </summary>
        /// <param name="filename">The string path of the assembly to load from.</param>
        private void LoadToolsFromAssembly(string filename)
        {
            if (System.IO.Path.GetExtension(filename) != ".dll") return; //makes sure its a dll
            if (System.IO.Path.GetFileName(filename) == "MapWindow.dll") return; //If they forget to turn "copy local" to false.
            if (filename.Contains("Interop")) return;

            Type[] CoClassList;
            Type[] InfcList;
            try
            {
                System.Reflection.Assembly asm;
                asm = System.Reflection.Assembly.LoadFrom(filename);
                DefaultToolProviderToolInfo toolSpecs;

                CoClassList = asm.GetTypes();
                foreach (Type CoClass in CoClassList)
                {
                    InfcList = CoClass.GetInterfaces();
                    foreach (Type Infc in InfcList)
                    {
                        try
                        {
                            ITool tool = asm.CreateInstance(CoClass.FullName) as ITool;
                            if (tool != null)
                            {
                                System.IO.FileInfo toolFileInfo = new System.IO.FileInfo(filename);
                                toolSpecs = new DefaultToolProviderToolInfo(tool, filename, CoClass.FullName, toolFileInfo.LastWriteTime);
                                if (_toolInfoList.ContainsKey(toolSpecs.UniqueName) == false)
                                    _toolInfoList.Add(toolSpecs.UniqueName, toolSpecs);
                            }
                        }
                        catch
                        {
                            // this object didn't work, but keep looking
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                // We will fail frequently.
            }
        }
        #endregion
    }
}
