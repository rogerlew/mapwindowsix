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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 4:23:05 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapWindow.Main;
using MapWindow.Drawing;
using System.Drawing.Drawing2D;
namespace MapWindow.Components
{
    /// <summary>
    /// This is designed to automatically have add, subtract, up and down arrows for working with a simple collection of items.
    /// </summary>
    [ToolboxBitmap(typeof(DecorationCollectionControl), "UserControl.ico")]
    internal class DecorationCollectionControl : UserControl 
    {
        #region Events

        /// <summary>
        /// Occurs when someone clicks the add button.
        /// </summary>
        public event EventHandler AddClicked;

        /// <summary>
        /// Occurs when someone selects one of the items in the list box
        /// </summary>
        public event EventHandler SelectedItemChanged;

        /// <summary>
        /// Occurs when either the Promote or Demote function has been used,
        /// changing the order.
        /// </summary>
        public event EventHandler OrderChanged;

        /// <summary>
        /// Occurs when the remove button has been clicked
        /// </summary>
        public event EventHandler RemoveClicked;

        /// <summary>
        /// Occurs when the list has been added, removed, or re-ordered in any way.
        /// </summary>
        public event EventHandler ListChanged;

        #endregion

        #region Private Variables

        private ListBox lbxItems;
        private Panel panel1;
        private Button btnAdd;
        private Button btnDown;
        private Button btnUp;
        private Button btnRemove;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private IList<ILineDecoration> _decorations;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Collection Control
        /// </summary>
        public DecorationCollectionControl()
        {
            InitializeComponent();
            _decorations = new List<ILineDecoration>();
            lbxItems.DrawItem += new DrawItemEventHandler(lbxItems_DrawItem);
        }

        void lbxItems_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle outer = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, outer);
            }
            else
            {
                Brush b = new SolidBrush(BackColor);
                e.Graphics.FillRectangle(b, outer);
                b.Dispose();
            }
            Rectangle inner = new Rectangle(e.Bounds.X + 5, e.Bounds.Y + 1, e.Bounds.Width - 10, e.Bounds.Height - 3);
            e.Graphics.FillRectangle(Brushes.White, inner);
            e.Graphics.DrawRectangle(Pens.Black, inner);
            int index = (e.Index < 0) ? 0 : e.Index;
            if (lbxItems.Items.Count == 0) return;
            ILineDecoration lineDecoration = lbxItems.Items[index] as ILineDecoration;
            if (lineDecoration == null) return;
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(e.Bounds.X + 10, e.Bounds.Y + e.Bounds.Height / 2), new Point(e.Bounds.Width - 10, e.Bounds.Y + e.Bounds.Height / 2));
            lineDecoration.Draw(e.Graphics, gp, 1);
            gp.Dispose();
        }

        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DecorationCollectionControl));
            this.lbxItems = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxItems
            // 
            this.lbxItems.AccessibleDescription = null;
            this.lbxItems.AccessibleName = null;
            resources.ApplyResources(this.lbxItems, "lbxItems");
            this.lbxItems.BackgroundImage = null;
            this.lbxItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbxItems.Font = null;
            this.lbxItems.FormattingEnabled = true;
            this.lbxItems.Name = "lbxItems";
            this.lbxItems.SelectedIndexChanged += new System.EventHandler(this.lbxItems_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // btnDown
            // 
            this.btnDown.AccessibleDescription = null;
            this.btnDown.AccessibleName = null;
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.BackgroundImage = null;
            this.btnDown.Font = null;
            this.btnDown.Image = global::MapWindow.Images.down;
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.AccessibleDescription = null;
            this.btnUp.AccessibleName = null;
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.BackgroundImage = null;
            this.btnUp.Font = null;
            this.btnUp.Image = global::MapWindow.Images.up;
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.AccessibleDescription = null;
            this.btnRemove.AccessibleName = null;
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.BackgroundImage = null;
            this.btnRemove.Font = null;
            this.btnRemove.Image = global::MapWindow.Images.mnuLayerClear;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AccessibleDescription = null;
            this.btnAdd.AccessibleName = null;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.BackgroundImage = null;
            this.btnAdd.Font = null;
            this.btnAdd.Image = global::MapWindow.Images.mnuLayerAdd;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // DecorationCollectionControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.lbxItems);
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.Name = "DecorationCollectionControl";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void lbxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        /// <summary>
        /// Fires the SelectedItemChanged event
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            if (SelectedItemChanged != null) SelectedItemChanged(this, new EventArgs());
        }


        #endregion

        #region Methods

       
        /// <summary>
        /// Refreshes the items in the list to accuratly reflect the current collection
        /// </summary>
        public void RefreshList()
        {
            lbxItems.SuspendLayout();
            lbxItems.Items.Clear();
            foreach (ILineDecoration lineDecoration in _decorations)
            {
                lbxItems.Items.Insert(0, lineDecoration);
            }
            lbxItems.ResumeLayout();
        }


        #endregion

        #region Properties

       

        /// <summary>
        /// Gets the selected item cast as an object.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILineDecoration SelectedDecoration
        {
            get
            {
                if (lbxItems == null) return null;
                if (lbxItems.SelectedItem == null) return null;
                return lbxItems.SelectedItem as ILineDecoration;
            }
            set
            {
                if (lbxItems == null) return;
                lbxItems.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the core list of lineDecorations that will be drawn here.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<ILineDecoration> Decorations
        {
            get { return _decorations; }
            set 
            { 
                _decorations = value;
                RefreshList();
            }
        }


      

        #endregion


        #region Event Handlers

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OnAdd();
        }


        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            ILineDecoration lineDecoration = lbxItems.SelectedItem as ILineDecoration;
            if (lineDecoration == null) return;
            _decorations.DecreaseIndex<ILineDecoration>(lineDecoration);
            RefreshList();
            lbxItems.SelectedItem = lineDecoration;
            OnOrderChanged();
        }

       
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            ILineDecoration lineDecoration = lbxItems.SelectedItem as ILineDecoration;
            int index = _decorations.IndexOf(lineDecoration);
            _decorations.Remove(lineDecoration);
            RefreshList();
            if(_decorations.Count == 0)return;
            if (index >= _decorations.Count)
            {
                index -= 1;
            }
            lbxItems.SelectedIndex = index;
            OnRemoveClick();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            ILineDecoration lineDecoration = lbxItems.SelectedItem as ILineDecoration;
            if (lineDecoration == null) return;
            _decorations.IncreaseIndex<ILineDecoration>(lineDecoration);
            RefreshList();
            lbxItems.SelectedItem = lineDecoration;
            OnOrderChanged();
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
        /// Fires the AddClicked event
        /// </summary>
        protected virtual void OnAdd()
        {
            if (AddClicked != null) AddClicked(this, new EventArgs());
            OnListChanged();
        }



        /// <summary>
        /// Fires the ListChanged event
        /// </summary>
        protected virtual void OnListChanged()
        {
            if (ListChanged != null) ListChanged(this, new EventArgs());
        }

        /// <summary>
        /// Fires the RemoveCLicked event
        /// </summary>
        protected virtual void OnRemoveClick()
        {
            if (RemoveClicked != null) RemoveClicked(this, new EventArgs());
            OnListChanged();
        }

        /// <summary>
        /// Fires the OnOrderChanged event
        /// </summary>
        protected virtual void OnOrderChanged()
        {
            if (OrderChanged != null) OrderChanged(this, new EventArgs());
            OnListChanged();
        }

        #endregion

    }
}
