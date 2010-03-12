//********************************************************************************************************
// Product Name: MapWindow.Layout.LayoutPropertyGrid
// Description:  A property grid designed to work along with the layout engine
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
// The Original Code is MapWindow.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
//********************************************************************************************************

using System;
using System.Windows.Forms;
using MapWindow.Layout.Elements;
using System.ComponentModel;

namespace MapWindow.Layout
{
    /// <summary>
    /// This is a control that allows users to easilly modify the various aspects of many different layout components
    /// </summary>
    [ToolboxItem(false)]
    public class LayoutPropertyGrid : UserControl
    {
        private PropertyGrid _propertyGrid;
        private LayoutControl _layoutControl;

        /// <summary>
        /// Creates a new instance of the Layout Property Grid
        /// </summary>
        public LayoutPropertyGrid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the layout control associated with this property grid
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set 
            {
                _layoutControl = value;
                if (_layoutControl == null) return;
                _layoutControl.SelectionChanged += LayoutControlSelectionChanged;
            }
        }

        #region ------------------- Event Handlers

        /// <summary>
        /// If the selection changes this event is called
        /// </summary>
        void LayoutControlSelectionChanged(object sender, EventArgs e)
        {
            //This code is so that the property grid gets updates if one of the properties changes
            foreach (LayoutElement selecteElement in _layoutControl.LayoutElements)
                selecteElement.Invalidated -= SelecteElementInvalidated;
            foreach (LayoutElement selecteElement in _layoutControl.SelectedLayoutElements)
                selecteElement.Invalidated += SelecteElementInvalidated;
                    

            //If there is no selection get the layoutControls properties otherwise show the selected elements properties
            if (_layoutControl.SelectedLayoutElements.Count > 0)
                _propertyGrid.SelectedObjects = _layoutControl.SelectedLayoutElements.ToArray();
            else
                _propertyGrid.SelectedObjects = null;
        }

        void SelecteElementInvalidated(object sender, EventArgs e)
        {
            //If there is no selection get the layoutControls properties otherwise show the selected elements properties
            _propertyGrid.Refresh();
        }


        void LayoutPropertyGrid_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    _layoutControl.DeleteSelected();
                    break;
                case Keys.F5:
                    _layoutControl.RefreshElements();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutPropertyGrid));
            this._propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // _propertyGrid
            // 
            resources.ApplyResources(this._propertyGrid, "_propertyGrid");
            this._propertyGrid.Name = "_propertyGrid";
            // 
            // LayoutPropertyGrid
            // 
            this.Controls.Add(this._propertyGrid);
            this.Name = "LayoutPropertyGrid";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LayoutPropertyGrid_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
