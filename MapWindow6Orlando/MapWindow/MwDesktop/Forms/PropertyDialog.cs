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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in Fall 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using MapWindow.Drawing;
namespace MapWindow.Forms
{
   
    
    /// <summary>
    /// A Dialog that can be useful for showing properties
    /// </summary>
    public class PropertyDialog : Form
    {

        #region Private Variables


        IWindowsFormsEditorService _dialogProvider;

        // Designer variables
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdApply;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #endregion

        #region Constructors

        /// <summary>
        /// This creates a new instance of the Dialog
        /// </summary>
        public PropertyDialog()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// This creates a new instance of the Dialog
        /// </summary>
        /// <param name="dialogProvider">A Dialog provider... this doesn't seem to do anything yet</param>
        public PropertyDialog(IWindowsFormsEditorService dialogProvider):this()
        {
            _dialogProvider = dialogProvider;
           
           
        }

      

       

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyDialog));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            // cmdOK
            // 
            this.cmdOK.AccessibleDescription = null;
            this.cmdOK.AccessibleName = null;
            resources.ApplyResources(this.cmdOK, "cmdOK");
            this.cmdOK.BackgroundImage = null;
            this.cmdOK.Font = null;
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.AccessibleDescription = null;
            this.cmdCancel.AccessibleName = null;
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackgroundImage = null;
            this.cmdCancel.Font = null;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_click);
            // 
            // cmdApply
            // 
            this.cmdApply.AccessibleDescription = null;
            this.cmdApply.AccessibleName = null;
            resources.ApplyResources(this.cmdApply, "cmdApply");
            this.cmdApply.BackgroundImage = null;
            this.cmdApply.Font = null;
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_click);
            // 
            // PropertyDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.cmdApply);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.propertyGrid1);
            this.Font = null;
            this.Icon = null;
            this.Name = "PropertyDialog";
            this.ResumeLayout(false);

        }

        #endregion

        #region Properties

        /// <summary>
        /// This provides access to the property grid on this dialog
        /// </summary>
        public PropertyGrid PropertyGrid
        {
            get
            {
                return propertyGrid1;
            }
            set
            {
                propertyGrid1 = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// This event occurs when someone presses the apply button
        /// </summary>
        public virtual event EventHandler ChangesApplied;

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnChangesApplied()
        {
            
           
            if (ChangesApplied != null)
            {
                ChangesApplied(this, new System.EventArgs());
            }
        }

        #endregion

        #region EventHandlers


        private void cmdApply_click(object sender, EventArgs e)
        {
            OnChangesApplied();
        }

        private void cmdOK_click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            OnChangesApplied();
            this.Close();
        }

        private void cmdCancel_click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region Private Functions

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