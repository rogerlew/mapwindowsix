//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 3/8/2010 9:28:29 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using MapWindow.Map;
using System.Drawing;

namespace MapWindow.Components
{
    /// <summary>
    /// The PanelManager can be used to add tab pages to the main application form
    /// </summary>
    public class PanelManager : Component
    {
        /// <summary>
        /// Creates a new instance of the PanelManager associated with the
        /// user-specified applicationManager.
        /// </summary>
        /// <param name="applicationManager">The main application manager</param>
        public PanelManager(ApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        //the collection of tabs of this manager
        private List<Control> _tabCollection = new List<Control>();

        private ApplicationManager _applicationManager = null;
        private bool _tabControlVisible = false;
        private ImageList _tabImageList = new ImageList();
        
        
        /// <summary>
        /// The main application manager
        /// </summary>
        public ApplicationManager ApplicationManager
        {
            get { return _applicationManager; }
        }

        /// <summary>
        /// The number of tabs in this tab control manager
        /// </summary>
        public int NumTabs
        {
            get { return _tabCollection.Count; }
        }

        /// <summary>
        /// Gets the collections of controls that are displayed in the tabs or
        /// panels.
        /// </summary>
        public IList<Control> PanelControlCollection
        {
            get { return _tabCollection; }
        }
        
        
        /// <summary>
        /// Adds the user control to the main application form as a new tab page.
        /// </summary>
        /// <param name="myControl">the user control to add</param>
        /// <param name="tabName">the tab</param>
        public void AddPanel(Control myControl, string tabName)
        {
            ToolStripContainer mainContainer = _applicationManager.ToolStripContainer;
            TabControl mainTabControl = null;
            
            if (_tabControlVisible == true)
            {
                //if there already is a main tab control, add a new tab page to the control
                if (_tabControlVisible == true)
                {
                    //find the 'tab control'
                    foreach (Control ctl in mainContainer.ContentPanel.Controls)
                    {
                        if (ctl is TabControl)
                        {
                            mainTabControl = ctl as TabControl;
                            break;
                        }
                    }

                    if (mainTabControl != null)
                    {
                        TabPage newTabPage = new TabPage();
                        newTabPage.Text = tabName;
                        newTabPage.Name = tabName;
                        newTabPage.Controls.Add(myControl);
                        _tabCollection.Add(myControl);
                        mainTabControl.TabPages.Add(newTabPage);
                    }
                }
            }
            else
            {
                Control controlToMove = null;

                if (mainContainer.ContentPanel.Controls.Count > 0)
                {
                    //we assume that the content panel of the main toolStripContainer
                    //only contains one control
                    controlToMove = mainContainer.ContentPanel.Controls[0];

                    try
                    {
                        mainTabControl = new TabControl();
                        mainTabControl.Dock = DockStyle.Fill;
                        
                        //temporarily remove the control
                        mainContainer.Controls.Remove(controlToMove);

                        
                        //create a new TabPage for the map content
                        TabPage mapTabPage = new TabPage();
                        mainTabControl.Controls.Add(mapTabPage);
                        mapTabPage.Text = "map"; //map should be moved to message strings
                        
                        //move the main content control to the tab page
                        mapTabPage.Controls.Add(controlToMove);

                        //create a new tabPage for the plugin content
                        TabPage newTabPage = new TabPage();

                        mainTabControl.TabPages.Add(newTabPage);
                        newTabPage.Name = tabName;
                        newTabPage.Text = tabName;
                        newTabPage.Controls.Add(myControl);
                        _tabCollection.Add(myControl);

                        //add the TabControl to the main content panel
                        mainContainer.ContentPanel.Controls.Add(mainTabControl);

                        _tabControlVisible = true;
                    }
                    catch
                    {
                        MessageBox.Show("Error adding tab page!");
                    }
                }
            }
        }


        /// <summary>
        /// Adds the user control to the main application form as a new tab page.
        /// </summary>
        /// <param name="myControl">the user control to add</param>
        /// <param name="panelName">the tab or panel name</param>
        /// <param name="panelIcon">the image or icon displayed on the tab or panel</param>
        public void AddPanel(Control myControl, string panelName, Image panelIcon)
        {
            ToolStripContainer mainContainer = _applicationManager.ToolStripContainer;
            TabControl mainTabControl = null;

            if (_tabControlVisible == true)
            {
                //if there already is a main tab control, add a new tab page to the control
                if (_tabControlVisible == true)
                {
                    //find the 'tab control'
                    foreach (Control ctl in mainContainer.ContentPanel.Controls)
                    {
                        if (ctl is TabControl)
                        {
                            mainTabControl = ctl as TabControl;
                            if (mainTabControl.ImageList == null)
                            {
                                mainTabControl.ImageList = _tabImageList;
                            }
                            break;
                        }
                    }

                    if (mainTabControl != null)
                    {
                        TabPage newTabPage = new TabPage();
                        newTabPage.Text = panelName;
                        newTabPage.Name = panelName;
                        newTabPage.Controls.Add(myControl);
                        _tabCollection.Add(myControl);
                        mainTabControl.TabPages.Add(newTabPage);

                        if (panelIcon != null)
                        {
                            _tabImageList.Images.Add(panelName, panelIcon);
                            newTabPage.ImageKey = panelName;
                            newTabPage.ImageIndex = _tabImageList.Images.Count - 1;
                        }
                    }
                }
            }
            else
            {
                Control controlToMove = null;

                if (mainContainer.ContentPanel.Controls.Count > 0)
                {
                    //we assume that the content panel of the main toolStripContainer
                    //only contains one control
                    controlToMove = mainContainer.ContentPanel.Controls[0];

                    try
                    {
                        mainTabControl = new TabControl();
                        mainTabControl.ImageList = _tabImageList;
                       
                        mainTabControl.Dock = DockStyle.Fill;

                        //temporarily remove the control
                        mainContainer.Controls.Remove(controlToMove);


                        //create a new TabPage for the map content
                        TabPage mapTabPage = new TabPage();
                        mainTabControl.Controls.Add(mapTabPage);
                        mapTabPage.Text = "map"; //map should be moved to message strings

                        //move the main content control to the tab page
                        mapTabPage.Controls.Add(controlToMove);

                        //create a new tabPage for the plugin content
                        TabPage newTabPage = new TabPage();
                        newTabPage.Name = panelName;
                        newTabPage.Text = panelName;
                        newTabPage.Controls.Add(myControl);

                        mainTabControl.TabPages.Add(newTabPage);

                        //add the tab icon if supplied by the user
                        if (panelIcon != null)
                        {
                            _tabImageList.Images.Add(panelName, panelIcon);
                            newTabPage.ImageKey = panelName;
                            newTabPage.ImageIndex = _tabImageList.Images.Count - 1;
                        }
                        
                        _tabCollection.Add(myControl);

                        //add the TabControl to the main content panel
                        mainContainer.ContentPanel.Controls.Add(mainTabControl);

                        _tabControlVisible = true;
                    }
                    catch
                    {
                        MessageBox.Show("Error adding tab page!");
                    }
                }
            }
        }


        /// <summary>
        /// Remove the panel or tab from the application.
        /// </summary>
        /// <param name="panelName">The name of the panel or tab page to be removed.</param>
        public void RemovePanel(string panelName)
        {
            //find the main tab control
            ToolStripContainer mainContainer = _applicationManager.ToolStripContainer;
            TabControl mainTabControl = null;

            //find the 'tab control'
            foreach (Control ctl in mainContainer.ContentPanel.Controls)
            {
                if (ctl is TabControl)
                {
                    mainTabControl = ctl as TabControl;
                    break;
                }
            }

            if (mainTabControl == null) return;
            
            if (_tabControlVisible == true)
            {
                if (NumTabs > 1)
                {
                    //only remove the tab page
                    TabPage pageToRemove = FindTabPage(mainTabControl, panelName);
                    if (pageToRemove != null)
                    {
                        if (pageToRemove.ImageKey != string.Empty)
                        {
                            _tabImageList.Images.RemoveByKey(pageToRemove.ImageKey);
                        }
                        
                        mainTabControl.TabPages.Remove(pageToRemove);
                        _tabCollection.Remove(pageToRemove.Controls[0]);
                    }
                }
                else
                {
                    //remove the tab page and also the tab control
                    TabPage pageToRemove = FindTabPage(mainTabControl, panelName);
                    if (pageToRemove != null)
                    {
                        mainTabControl.TabPages.Remove(pageToRemove);
                        _tabCollection.Remove(pageToRemove.Controls[0]);

                        if (mainTabControl.TabPages.Count == 1)
                        {
                            Control movedControl = mainTabControl.TabPages[0].Controls[0];
                            mainTabControl.TabPages.Remove(pageToRemove);
                            _tabCollection.Remove(movedControl);
                            if (_applicationManager.ToolStripContainer.ContentPanel.Contains(mainTabControl))
                            {
                                _applicationManager.ToolStripContainer.ContentPanel.Controls.Remove(mainTabControl);
                                _applicationManager.ToolStripContainer.ContentPanel.Controls.Add(movedControl);
                                mainTabControl = null;
                                _tabControlVisible = false;
                            }
                        }
                    }
                }
            }
        }

        private TabPage FindTabPage(TabControl tabControl, string tabName)
        {
            if (tabControl.TabPages.Count == 0)
            {
                _tabControlVisible = false;
                _tabCollection.Clear();
                return null;
            }
            else
            {
                foreach (TabPage page in tabControl.TabPages)
                {
                    if (page.Name == tabName)
                    {
                        return page;
                    }
                }
                return null;
            }
        }
    }
}
