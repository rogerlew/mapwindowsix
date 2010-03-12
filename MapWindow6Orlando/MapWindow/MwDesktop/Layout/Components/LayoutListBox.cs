//********************************************************************************************************
// Product Name: MapWindow.Layout.LayoutListBox
// Description:  Shows a list of all the items in the layout control
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 4:23:05 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MapWindow.Layout.Elements;

namespace MapWindow.Layout
{
    /// <summary>
    /// This is designed to automatically have add, subtract, up and down arrows for working with a simple collection of items.
    /// </summary>
    [ToolboxItem(false)]
    public class LayoutListBox : UserControl
    {

        #region ---------------- Class Variables

        //Internal Variables
        private ListBox _lbxItems;
        private Panel _btnPanel;
        private Button _btnDown;
        private Button _btnUp;
        private Button _btnRemove;
        private Panel _listPanel;
        private LayoutControl _layoutControl;
        private bool _suppressSelectionChange;

        // Required designer variable.
        private System.ComponentModel.IContainer components = null;

        #endregion

        #region ---------------- Properties

        /// <summary>
        /// Gets or sets the layoutControl
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set 
            {
                if (value == null) return;
                _layoutControl = value;
                _layoutControl.SelectionChanged += new EventHandler(_layoutControl_SelectionChanged);
                _layoutControl.ElementsChanged += new EventHandler(_layoutControl_ElementsChanged);
                RefreshList();
            }
        }

        #endregion

        #region ---------------- Constructors

        /// <summary>
        /// Creates a new instance of the Collection Control
        /// </summary>
        public LayoutListBox()
        {
            InitializeComponent();
            _suppressSelectionChange = false;
            _lbxItems.DrawItem += new DrawItemEventHandler(lbxItems_DrawItem);
        }

        #endregion

        #region ---------------- Drawing Code

        void lbxItems_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
            Rectangle outer = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            SolidBrush textBrush;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, outer);
                textBrush = new SolidBrush(Color.White);
            }
            else
            {
                textBrush = new SolidBrush(Color.Black);
                e.Graphics.FillRectangle(Brushes.White, outer);
            }
            Rectangle thumbRect = new Rectangle(outer.X + 3, outer.Y + 3, 32
                , 32);
            e.Graphics.FillRectangle(Brushes.White, thumbRect);
            if ((_lbxItems.Items[e.Index] as LayoutElement).ThumbNail != null)
                e.Graphics.DrawImage((_lbxItems.Items[e.Index] as LayoutElement).ThumbNail, thumbRect);
            thumbRect.X--; thumbRect.Y--; thumbRect.Width++; thumbRect.Height++;
            e.Graphics.DrawRectangle(Pens.Black, thumbRect);

            SizeF TextSize = e.Graphics.MeasureString((_lbxItems.Items[e.Index] as LayoutElement).Name, this.Font);
            Rectangle textRectangle = new Rectangle(outer.X + 40, outer.Y, outer.Width - 40, outer.Height);
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Near;
            drawFormat.FormatFlags = StringFormatFlags.NoWrap;
            drawFormat.LineAlignment = StringAlignment.Center;
            drawFormat.Trimming = StringTrimming.EllipsisCharacter;
            e.Graphics.DrawString((_lbxItems.Items[e.Index] as LayoutElement).Name, this.Font, textBrush, textRectangle, drawFormat);
            
            //Little clean up code
            drawFormat.Dispose();
            textBrush.Dispose();
        }

        #endregion

        #region ---------------- Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutListBox));
            this._lbxItems = new System.Windows.Forms.ListBox();
            this._btnPanel = new System.Windows.Forms.Panel();
            this._btnDown = new System.Windows.Forms.Button();
            this._btnUp = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._listPanel = new System.Windows.Forms.Panel();
            this._btnPanel.SuspendLayout();
            this._listPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lbxItems
            // 
            this._lbxItems.AccessibleDescription = null;
            this._lbxItems.AccessibleName = null;
            resources.ApplyResources(this._lbxItems, "_lbxItems");
            this._lbxItems.BackgroundImage = null;
            this._lbxItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._lbxItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._lbxItems.Font = null;
            this._lbxItems.FormattingEnabled = true;
            this._lbxItems.Name = "_lbxItems";
            this._lbxItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._lbxItems.SelectedIndexChanged += new System.EventHandler(this.lbxItems_SelectedIndexChanged);
            // 
            // _btnPanel
            // 
            this._btnPanel.AccessibleDescription = null;
            this._btnPanel.AccessibleName = null;
            resources.ApplyResources(this._btnPanel, "_btnPanel");
            this._btnPanel.BackgroundImage = null;
            this._btnPanel.Controls.Add(this._btnDown);
            this._btnPanel.Controls.Add(this._btnUp);
            this._btnPanel.Controls.Add(this._btnRemove);
            this._btnPanel.Font = null;
            this._btnPanel.Name = "_btnPanel";
            // 
            // _btnDown
            // 
            this._btnDown.AccessibleDescription = null;
            this._btnDown.AccessibleName = null;
            resources.ApplyResources(this._btnDown, "_btnDown");
            this._btnDown.BackgroundImage = null;
            this._btnDown.Font = null;
            this._btnDown.Image = global::MapWindow.Images.down;
            this._btnDown.Name = "_btnDown";
            this._btnDown.UseVisualStyleBackColor = true;
            this._btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // _btnUp
            // 
            this._btnUp.AccessibleDescription = null;
            this._btnUp.AccessibleName = null;
            resources.ApplyResources(this._btnUp, "_btnUp");
            this._btnUp.BackgroundImage = null;
            this._btnUp.Font = null;
            this._btnUp.Image = global::MapWindow.Images.up;
            this._btnUp.Name = "_btnUp";
            this._btnUp.UseVisualStyleBackColor = true;
            this._btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.AccessibleDescription = null;
            this._btnRemove.AccessibleName = null;
            resources.ApplyResources(this._btnRemove, "_btnRemove");
            this._btnRemove.BackgroundImage = null;
            this._btnRemove.Font = null;
            this._btnRemove.Image = global::MapWindow.Images.mnuLayerClear;
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // _listPanel
            // 
            this._listPanel.AccessibleDescription = null;
            this._listPanel.AccessibleName = null;
            resources.ApplyResources(this._listPanel, "_listPanel");
            this._listPanel.BackColor = System.Drawing.Color.White;
            this._listPanel.BackgroundImage = null;
            this._listPanel.Controls.Add(this._lbxItems);
            this._listPanel.Font = null;
            this._listPanel.Name = "_listPanel";
            // 
            // LayoutListBox
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this._listPanel);
            this.Controls.Add(this._btnPanel);
            this.Font = null;
            this.Name = "LayoutListBox";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LayoutListBox_KeyUp);
            this._btnPanel.ResumeLayout(false);
            this._listPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region ---------------- Public Methods

        /// <summary>
        /// Refreshes the items in the list to accuratly reflect the current collection
        /// </summary>
        public void RefreshList()
        {
            _lbxItems.SuspendLayout();

            //We clear the old list
            _lbxItems.Items.Clear();

            //Updates the list of elements
            foreach (LayoutElement le in _layoutControl.LayoutElements.ToArray())
            {
                _lbxItems.Items.Add(le);
                le.ThumbnailChanged += new EventHandler(le_ThumbnailChanged);
            }
            
            //Updates the selection list            
            foreach (LayoutElement le in _layoutControl.SelectedLayoutElements.ToArray())
                _lbxItems.SelectedItems.Add(le);

            _lbxItems.ResumeLayout();
        }

        #endregion

        #region ---------------- Event Handlers

        void le_ThumbnailChanged(object sender, EventArgs e)
        {
            _lbxItems.Invalidate();
        }

        void _layoutControl_ElementsChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange == true) return;
            _suppressSelectionChange = true;
            RefreshList();
            _suppressSelectionChange = false;
        }

        void _layoutControl_SelectionChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange == true) return;
            _suppressSelectionChange = true;
            RefreshList();
            _suppressSelectionChange = false;
        }

        void lbxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange == true) return;
            _suppressSelectionChange = true;
            _layoutControl.SuspendLayout();
            _layoutControl.ClearSelection();
            _layoutControl.AddToSelection(new List<LayoutElement>(_lbxItems.SelectedItems.OfType<LayoutElement>()));
            _layoutControl.ResumeLayout();
            _suppressSelectionChange = false;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionDown();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionUp();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _layoutControl.DeleteSelected();
        }

        void LayoutListBox_KeyUp(object sender, KeyEventArgs e)
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

        #region ---------------- Protected Methods

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

        #endregion

    }
}
