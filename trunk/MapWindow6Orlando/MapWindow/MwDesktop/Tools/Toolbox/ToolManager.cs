//********************************************************************************************************
// Product Name: MapWindow.Tools.ToolManager
// Description:  Deals with loading and finding tools from a specific folder
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
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml;
using MapWindow.Components;
using MapWindow.Drawing;
using MapWindow.Forms;

namespace MapWindow.Tools
{
    /// <summary>
    /// This class provides a ToolManager for loading tools from .dll's
    /// </summary>
    public class ToolManager : TreeView
    {
        #region ------------------ Private Variables

        private readonly ToolTip _toolTipTree;
        private readonly Dictionary<string, ToolManagerToolInfo> _toolInfoList;
        private List<string> _toolDirectories;
        private readonly List<string> _loadedToolProviderIDs;
        private readonly XmlDocument _xmlSettings = new XmlDocument();
        private readonly char _PS = System.IO.Path.DirectorySeparatorChar;
        private ILegend _legend;
        private string _tempPath;
 
        #endregion

        #region ------------------ Constructor
        
        /// <summary>
        /// Creates a new instance of the ToolManager, scans the executables root path\tools
        /// </summary>
        public ToolManager()
        {
            //Sets up some initial variables
            _toolTipTree = new ToolTip();
            _toolInfoList = new Dictionary<string, ToolManagerToolInfo>();
            _toolDirectories = new List<string>();
            _loadedToolProviderIDs = new List<string>();
            _xmlSettings = new XmlDocument();
            
        }

        /// <summary>
        /// Occurs when the control is created and a refresh is needed
        /// </summary>
        protected override void OnCreateControl()
        {
            RefreshTree();
        }

       


      
 
        #endregion

        #region ------------------ Public Properties

        /// <summary>
        /// Sets the data that are available by default to tools
        /// </summary>
        [Category("ToolManager Appearance"), Description("Sets the data that are available by default to tools")]
        public List<DataSetArray> DataSets
        {
            get 
            {
                List<DataSetArray> dataSets = new List<DataSetArray>();
                for (int i = 0; i < _legend.RootNodes.Count; i++)
                {
                    IFrame mf = _legend.RootNodes[i] as IFrame;
                    if (mf != null)
                    {
                        foreach (ILayer ml in mf)
                        {
                            if (ml.DataSet != null)
                                dataSets.Add(new DataSetArray(ml.LegendText, ml.DataSet));
                        }
                    }
                }
                return dataSets;
            }
        }

        /// <summary>
        /// Gets or Sets the legend object. This is needed to automatically populate the list of data layers in tool dialogs.
        /// </summary>
        [Category("ToolManager Appearance"), Description("Gets or Sets the legend object. This is needed to automatically populate the list of data layers in tool dialogs.")]
        public ILegend Legend
        {
            get { return _legend; }
            set
            {
                _legend = value;
            }

        }

        /// <summary>
        /// Gets or sets the list of paths that is searched for IToolProviders this path is also passed along to IToolProviders when they are found
        /// </summary>
        [Category("ToolManager Appearance"), 
        Description("Gets or sets the list of paths that is searched for IToolProviders this path is also passed along to IToolProviders when they are found."),
        Editor(typeof(StringCollectionEditor), typeof(CollectionEditor))]
        public List<string> ToolDirectories
        {
            get { return _toolDirectories; }
            set { _toolDirectories = value; }
        }

        /// <summary>
        /// Gets the list of unique tool names
        /// </summary>
        [Category("ToolManager Appearance"), Description("Gets the list of unique tool names.")]
        public List<ToolInfo> ToolList
        {
            get
            {
                List<ToolInfo> myList = new List<ToolInfo>();
                foreach (ToolInfo info in _toolInfoList.Values)
                    myList.Add(info);
                return (myList);
            }
        }

        /// <summary>
        /// Gets or sets the string temp path
        /// </summary>
        [Category("ToolManager Appearance"), Description("Path where tools can store their temporary files")]
        public string TempPath
        {
            get { return _tempPath; }
            set { _tempPath = value; }
        }

        #endregion

        #region ------------------ Public Methods

        /// <summary>
        /// Loads the settings for all the ToolManager and all ToolProviders
        /// </summary>
        /// <returns></returns>
        public void LoadSettings()
        {
            string exeFile = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + _PS + exeFile;
            if (System.IO.File.Exists(path + _PS + "ToolManagerSettings.xml") == false)
            {
                if (System.IO.Directory.Exists(path) == false) System.IO.Directory.CreateDirectory(path);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(path + _PS + "ToolManagerSettings.xml");
                sw.WriteLine("<ToolManagerSetting version=\"1.0\">\r\n" +
                             "  <Setting>\r\n" +
                             "  </Setting>\r\n" +
                             "    <Provider Name =\"DefaultToolProvider\">\r\n" +
                             "      <Settings>\r\n" +
                             "        <Path Value =\"" + Application.StartupPath + _PS + "Tools\"></Path>\r\n" +
                             "      </Settings>\r\n" +
                             "    </Provider>\r\n" +
                             "</ToolManagerSetting>\r\n");
                sw.Close();
            }
            _xmlSettings.Load(path + _PS + "ToolManagerSettings.xml");
        }

        /// <summary>
        /// Locates a tool by its name in the tree and highlights it
        /// </summary>
        /// <param name="toolName"></param>
        public void HighlightTool(string toolName)
        {
            CollapseAll();
            if (toolName == "")
                return;

            for (int i = 0; i < Nodes.Count; i++)
            {
                for (int j = 0; j < Nodes[i].Nodes.Count; j++)
                {
                    if (Nodes[i].Nodes[j].Text.ToLower().Contains(toolName.ToLower()))
                    {
                        Nodes[i].Nodes[j].Expand();
                        SelectedNode = Nodes[i].Nodes[j];
                        return;
                    }
                }
            }
            CollapseAll();
            return;
        }

        /// <summary>
        /// Highlights the next tool
        /// </summary>
        /// <param name="toolName"></param>
        public void HighlightNextTool(string toolName)
        {
            TreeNode selectedNode = SelectedNode;
            CollapseAll();            

            if (toolName == "" || selectedNode == null)
                return;

            bool foundSelected = false;
            for (int i = 0; i < Nodes.Count; i++)
            {
                for (int j = 0; j < Nodes[i].Nodes.Count; j++)
                {
                    if (Nodes[i].Nodes[j] == selectedNode)
                    {
                        foundSelected = true;
                        continue;
                    }
                    if (foundSelected && Nodes[i].Nodes[j].Text.ToLower().Contains(toolName.ToLower()))
                    {
                        Nodes[i].Nodes[j].Expand();
                        SelectedNode = Nodes[i].Nodes[j];
                        return;
                    }
                }
            }

            HighlightTool(toolName);
        }

        /// <summary>
        /// Returns true if the Tool Manager can create the tool specified by the uniqueName
        /// </summary>
        /// <param name="uniqueName">The unique name of a tool</param>
        /// <returns>true if the tool can be created otherwise false</returns>
        public bool CanCreateTool(string uniqueName)
        {
            return _toolInfoList.ContainsKey(uniqueName);
        }

        /// <summary>
        /// Creates a new instance of a tool based on its uniqueName
        /// </summary>
        /// <param name="uniqueName">The unique name of the tool</param>
        /// <returns>Returns an new instance of the tool or NULL if the tools unique name doesn't exist in the manager</returns>
        public ITool NewTool(string uniqueName)
        {
            if (_toolInfoList.ContainsKey(uniqueName))
            {
                //Gets the ToolProvider that contains the Tool
                IToolProvider tProvider = _toolInfoList[uniqueName].ToolProviderAssembly;

                //Returns the tool generated by the tool provider
                return tProvider.NewTool(uniqueName);
            }
            return null;
        }

        /// <summary>
        /// This clears the list of available tools and loads them from file again
        /// </summary>
        public virtual void RefreshTree()
        {
            //We clear the list of tools and providers
            LoadSettings();
            _toolInfoList.Clear();
            _loadedToolProviderIDs.Clear();
            Nodes.Clear();

            //Rescans for providers
            LoadDefaultToolProvider();
            LoadToolProviders();

            //Re-populate the tool treeview
            ImageList = new ImageList();
            ImageList.Images.Add("Hammer", Images.HammerSmall);

            foreach (KeyValuePair<string, ToolManagerToolInfo> kvp in _toolInfoList)
            {
                ToolInfo myTool = kvp.Value;

                //If the tool's category doesn't exist we add it
                if (Nodes[myTool.Category] == null)
                    Nodes.Add(myTool.Category, myTool.Category);

                //we add the tool with the default icon
                Nodes[myTool.Category].Nodes.Add(myTool.UniqueName, myTool.Name, "Hammer", "Hammer");
            }
        }

        #endregion

        #region ------------------ Private Methods

        /// <summary>
        /// Loads the default tool provider
        /// </summary>
        private void LoadDefaultToolProvider()
        {
            //creates an instance of the default tool provider and initializes it
            DefaultToolProvider toolProvider = new DefaultToolProvider();
            if (_xmlSettings.ChildNodes.Count == 0) return;
            foreach (XmlNode child in _xmlSettings.ChildNodes[0].ChildNodes)
            {
                for (int i = 0; i < child.Attributes.Count; i++)
                {
                    if (child.Attributes[i].Value == "DefaultToolProvider")
                    {
                        XmlDocument providerSettings = new XmlDocument();
                        providerSettings.LoadXml(child.InnerXml);
                        toolProvider.Initialize(providerSettings);
                        
                        _loadedToolProviderIDs.Add(toolProvider.UniqueID);

                        //Loads each of the tools from the info list generated by the provider
                        foreach (ToolInfo info in toolProvider.ToolInfoList)
                        {
                            _toolInfoList.Add(info.UniqueName, new ToolManagerToolInfo(info, toolProvider));
                        }

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// This should be called once all the permitted directories have been set in the code.
        /// </summary>
        private void LoadToolProviders()
        {
            foreach (string directory in _toolDirectories)
            {
                if (System.IO.Directory.Exists(directory))
                {
                    foreach (string file in System.IO.Directory.GetFiles(directory, "*.dll", System.IO.SearchOption.AllDirectories))
                    {
                        LoadToolProvidersFromAssembly(file);
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Given a string filename for the "*.dll" file, this will attempt to load a ITool from the .dll
        /// </summary>
        /// <param name="filename">The string path of the assembly to load from.</param>
        private void LoadToolProvidersFromAssembly(string filename)
        {
            if (System.IO.Path.GetExtension(filename) != ".dll") return; //makes sure its a dll
            if (System.IO.Path.GetFileName(filename) == "MapWindow.dll") return; //If they forget to turn "copy local" to false.
            if (filename.Contains("Interop")) return;

            Assembly asm = Assembly.LoadFrom(filename);

            try
            {
                Type[] coClassList = asm.GetTypes();
                foreach (Type coClass in coClassList)
                {
                    Type[] infcList = coClass.GetInterfaces();
                    foreach (Type infc in infcList)
                    {
                        try
                        {
                            IToolProvider toolProvider = asm.CreateInstance(coClass.FullName) as IToolProvider;
                            if (toolProvider != null)
                            {
                                //Makes sure this tool providers unique ID hasn't been used
                                if (_loadedToolProviderIDs.Contains(toolProvider.UniqueID))
                                    LogManager.DefaultLogManager.LogMessageBox("The ToolManager already contains a ToolProvider called: " + toolProvider.UniqueID + ". The ToolProvider with this name in file: " + filename + " will not be loaded!", "Duplicate ToolProvider", MessageBoxButtons.OK);
                                else
                                {
                                    _loadedToolProviderIDs.Add(toolProvider.UniqueID);
                                    //////////This code needs to be update to work with other toolproviders

                                    //Initializeializeializes the toolProvider

                                    //Loads each of the tools from the info list generated by the provider
                                    foreach (ToolInfo info in toolProvider.ToolInfoList)
                                    {
                                        _toolInfoList.Add(info.UniqueName,new ToolManagerToolInfo(info,toolProvider));
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // this object didn't work, but keep looking
                        }
                    }
                }
            }
            catch
            {
                // We will fail frequently.
            }
        }


        #endregion
            
        #region ------------------ Events

        /// <summary>
        /// Runs when and item gets dragged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            
            TreeNode theNode = e.Item as TreeNode;
            if (theNode != null)
            {
                // Verify that the tag property is not "null".
                if ((theNode.Parent != null) && _toolInfoList.ContainsKey(theNode.Name))
                {
                    DoDragDrop("ITool: " + theNode.Name, DragDropEffects.Copy);
                }
            }
        }

        /// <summary>
        /// Thie event fires when the mouse moves to change the ToolTip
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Get the node at the current mouse pointer location.
            TreeNode theNode = GetNodeAt(e.X, e.Y);

            // Set a ToolTip only if the mouse pointer is actually paused on a node.
            if ((theNode != null))
            {
                // Verify that the tag property is not "null".
                if (_toolInfoList.ContainsKey(theNode.Name))
                {
                    // Change the ToolTip only if the pointer moved to a new node.
                    if (_toolInfoList[theNode.Name].ToolTip != _toolTipTree.GetToolTip(this))
                    {
                        _toolTipTree.SetToolTip(this, _toolInfoList[theNode.Name].ToolTip);
                    }
                }
                else
                {
                    _toolTipTree.SetToolTip(this, "");
                }
            }
            else     // Pointer is not over a node so clear the ToolTip.
            {
                _toolTipTree.SetToolTip(this, "");
            }
        }

        /// <summary>
        /// When the user double clicks one of the tools this event fires
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            // Get the node at the current mouse pointer location.
            TreeNode theNode = GetNodeAt(PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y);
           
            // Checks if the user double clicked a node and not a white spot
            if ((theNode != null))
            {
                // checks if the user clicked a tool or a category
                if (_toolInfoList.ContainsKey(theNode.Name))
                {

                    //We Generate an instance of the tool and dialog box to go with it
                    ITool toolToExecute = NewTool(theNode.Name);
                    toolToExecute.WorkingPath = _tempPath;
                    ToolDialog td = new ToolDialog(toolToExecute, DataSets);
                    DialogResult tdResult = td.ShowDialog(this);
                    while (tdResult == DialogResult.OK && td.ToolStatus != ToolStatus.Ok)
                    {
                        MessageBox.Show(MessageStrings.ToolSetupIncorectly);
                        tdResult = td.ShowDialog(this);
                    }
                    if (tdResult == DialogResult.OK && td.ToolStatus == ToolStatus.Ok)
                    {
                        //This fires when the user clicks the "OK" button on a tool dialog
                        //First we create the progress form
                        ToolProgress progForm = new ToolProgress(1);

                        //We create a background worker thread to execute the tool
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += bw_DoWork;
                        bw.RunWorkerCompleted += bw_RunWorkerCompleted;

                        object[] threadParamerter = new object[2];
                        threadParamerter[0] = toolToExecute;
                        threadParamerter[1] = progForm;

                        // Show the progress dialog and kick off the Async thread
                        progForm.Show(this);
                        bw.RunWorkerAsync(threadParamerter);
                    }
                }
            }
        }

        private static void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] threadParamerter = e.Argument as object[];
            if (threadParamerter == null) return;
            ITool toolToExecute = threadParamerter[0] as ITool;
            ToolProgress progForm = threadParamerter[1] as ToolProgress;
            
            //This code runs the 
            if (progForm == null) return;
            progForm.Progress("", 0, "==================");
            if (toolToExecute != null) progForm.Progress("", 0, "Executing Tool: " + toolToExecute.Name);
            progForm.Progress("", 0, "==================");
            if (toolToExecute != null) toolToExecute.Execute(progForm);
            progForm.executionComplete();
            progForm.Progress("", 0, "==================");
            if (toolToExecute != null) progForm.Progress("", 0, "Done Executing Tool: " + toolToExecute.Name);
            progForm.Progress("", 0, "==================");
        }

        private static void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        #endregion

       
    }
}
