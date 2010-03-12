//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core assembly for the MapWindow 6.0 distribution.
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/30/2009 8:55:03 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Windows.Forms;
using MapWindow.Components;
using MapWindow.Main;

namespace MapWindow.Forms
{
    /// <summary>
    /// CollectionPropertyGrid
    /// </summary>
    public class CollectionPropertyGrid : Form
    {
        #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        /// <summary>
        /// Occurs whenever the add item is clicked.  This is because the Collection Property Grid
        /// doesn't necessarilly know how to create a default item.  (An alternative would be
        /// to send in a factory, but I think this will work just as well.)
        /// </summary>
        public event EventHandler AddItemClicked;

        #endregion

        private Panel panel1;
        private SplitContainer splitContainer1;
        private CollectionControl ccItems;
        private PropertyGrid propertyGrid1;
        private DialogButtons dialogButtons1;


        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #endregion


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionPropertyGrid));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ccItems = new MapWindow.Components.CollectionControl();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dialogButtons1 = new MapWindow.Components.DialogButtons();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.AccessibleDescription = null;
            this.splitContainer1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BackgroundImage = null;
            this.splitContainer1.Font = null;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AccessibleDescription = null;
            this.splitContainer1.Panel1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackgroundImage = null;
            this.splitContainer1.Panel1.Controls.Add(this.ccItems);
            this.splitContainer1.Panel1.Font = null;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AccessibleDescription = null;
            this.splitContainer1.Panel2.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.BackgroundImage = null;
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Panel2.Font = null;
            // 
            // ccItems
            // 
            this.ccItems.AccessibleDescription = null;
            this.ccItems.AccessibleName = null;
            resources.ApplyResources(this.ccItems, "ccItems");
            this.ccItems.BackgroundImage = null;
            this.ccItems.Font = null;
            this.ccItems.Name = "ccItems";
            this.ccItems.SelectedName = null;
            this.ccItems.SelectedObject = null;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AccessibleDescription = null;
            this.propertyGrid1.AccessibleName = null;
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.BackgroundImage = null;
            this.propertyGrid1.Font = null;
            this.propertyGrid1.Name = "propertyGrid1";
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.dialogButtons1);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // dialogButtons1
            // 
            this.dialogButtons1.AccessibleDescription = null;
            this.dialogButtons1.AccessibleName = null;
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.BackgroundImage = null;
            this.dialogButtons1.Font = null;
            this.dialogButtons1.Name = "dialogButtons1";
            // 
            // CollectionPropertyGrid
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.Icon = null;
            this.Name = "CollectionPropertyGrid";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CollectionPropertyGrid
        /// </summary>
        public CollectionPropertyGrid()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new instance of CollectionPropertyGrid
        /// </summary>
        /// <param name="list">The INamedList to display</param>
        public CollectionPropertyGrid(INamedList list)
        {  
            InitializeComponent();
            NamedList = list;
            ccItems.AddClicked += ccItems_AddClicked;
            ccItems.SelectedItemChanged += ccItems_SelectedItemChanged;
            ccItems_SelectedItemChanged(ccItems, new EventArgs());
            ccItems.RemoveClicked += ccItems_RemoveClicked;
            Configure();
        }

        private void Configure()
        {
            dialogButtons1.OkClicked += btnOk_Click;
            dialogButtons1.CancelClicked += btnCancel_Click;
            dialogButtons1.ApplyClicked += btnApply_Click;
        }

        void ccItems_RemoveClicked(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ccItems.SelectedObject;
        }

        
       
        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tool that connects each item with a string name.
        /// </summary>
        public INamedList NamedList
        {
            get 
            { 
                if (ccItems != null) return ccItems.ItemNames;
                return null;
            }
            set 
            {
                ccItems.ItemNames = value;
            }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        void ccItems_AddClicked(object sender, EventArgs e)
        {
            OnAddClicked();
        }

        void ccItems_SelectedItemChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ccItems.SelectedObject;
        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
            Close();
        }

        

        #endregion

        #region Protected Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Occurs when the add button is clicked
        /// </summary>
        protected virtual void OnAddClicked()
        {
            if (AddItemClicked != null) AddItemClicked(this, new EventArgs());
        }

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            if (ChangesApplied != null) ChangesApplied(this, new EventArgs());
        }

        #endregion        
    }
}