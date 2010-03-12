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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/14/2009 11:22:04 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Drawing;
namespace MapWindow.Components
{
    /// <summary>
    /// A user control that is specifically designed to control the point sizes
    /// </summary>
    [DefaultEvent("SelectedSizeChanged"),
    ToolboxBitmap(typeof(SizeControl), "UserControl.ico")]
    public class SizeControl : UserControl
    {

        #region Events

        /// <summary>
        /// Occurs when the size changes on this control, ether through applying changes in the dialog or else
        /// by selecting one of the pre-configured sizes.
        /// </summary>
        public event EventHandler SelectedSizeChanged;

        #endregion

        #region private variables

        private System.Windows.Forms.GroupBox grpSize;
        private System.Windows.Forms.Button btnEdit;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private SymbolSizeChooser scSizes;
        private Size2DDialog _editDialog;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the size control
        /// </summary>
        public SizeControl()
        {
            InitializeComponent();
            _editDialog = new Size2DDialog();
            _editDialog.ChangesApplied += new EventHandler(_editDialog_ChangesApplied);
            scSizes.SelectedSizeChanged += new EventHandler(scSizes_SelectedSizeChanged);
        }

      
       


        #endregion
       
        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SizeControl));
            this.grpSize = new System.Windows.Forms.GroupBox();
            this.scSizes = new MapWindow.Components.SymbolSizeChooser();
            this.btnEdit = new System.Windows.Forms.Button();
            this.grpSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scSizes)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSize
            // 
            this.grpSize.AccessibleDescription = null;
            this.grpSize.AccessibleName = null;
            resources.ApplyResources(this.grpSize, "grpSize");
            this.grpSize.BackgroundImage = null;
            this.grpSize.Controls.Add(this.scSizes);
            this.grpSize.Controls.Add(this.btnEdit);
            this.grpSize.Font = null;
            this.grpSize.Name = "grpSize";
            this.grpSize.TabStop = false;
            // 
            // scSizes
            // 
            this.scSizes.AccessibleDescription = null;
            this.scSizes.AccessibleName = null;
            resources.ApplyResources(this.scSizes, "scSizes");
            this.scSizes.BackgroundImage = null;
            this.scSizes.BoxBackColor = System.Drawing.SystemColors.Control;
            this.scSizes.BoxSelectionColor = System.Drawing.SystemColors.Highlight;
            this.scSizes.BoxSize = new System.Drawing.Size(36, 36);
            this.scSizes.Font = null;
            this.scSizes.Name = "scSizes";
            this.scSizes.NumBoxes = 4;
            this.scSizes.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.scSizes.RoundingRadius = 6;
            // 
            // btnEdit
            // 
            this.btnEdit.AccessibleDescription = null;
            this.btnEdit.AccessibleName = null;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.BackgroundImage = null;
            this.btnEdit.Font = null;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // SizeControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.grpSize);
            this.Font = null;
            this.Name = "SizeControl";
            this.grpSize.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scSizes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        #region Methods

       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbol to use when drawing the various sizes
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISymbol Symbol
        {
            get
            {
                return scSizes.Symbol;
            }
            set
            {
                scSizes.Symbol = value;
                _editDialog.Symbol = value;
            }
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
        /// Fires the SizeChanged event
        /// </summary>
        protected virtual void OnSelectedSizeChanged()
        {
            if (SelectedSizeChanged != null) SelectedSizeChanged(this, new EventArgs());
        }


        #endregion

        #region Event Handlers

    
        private void btnEdit_Click(object sender, EventArgs e)
        {
            _editDialog.ShowDialog();
        }

        void _editDialog_ChangesApplied(object sender, EventArgs e)
        {
            scSizes.Invalidate();
            OnSelectedSizeChanged();
        }

        void scSizes_SelectedSizeChanged(object sender, EventArgs e)
        {
            // Even though this is a reference type, and the "symbol" property is
            // up to date on the edit dialog, we need to force it to regenerate 
            // the actual text values in the text boxes.
            _editDialog.Symbol = scSizes.Symbol;
            OnSelectedSizeChanged();
        }

        #endregion

    }
}
