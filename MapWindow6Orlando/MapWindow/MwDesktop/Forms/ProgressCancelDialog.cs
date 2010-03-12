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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/16/2009 1:18:11 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Windows.Forms;
using MapWindow.Components;
using MapWindow.Tools;

namespace MapWindow.Forms
{
    /// <summary>
    /// ProgressCancelDialog
    /// </summary>
    public class ProgressCancelDialog : Form, ICancelProgressHandler
    {

        /// <summary>
        /// Fires the canceled event.
        /// </summary>
        public event EventHandler Cancelled;

        private delegate void SetMess(string message);
        private mwProgressBar mwProgressBar1;
        private Button button1;
        private Label lblProgressText;
        private bool _cancelled;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressCancelDialog));
            this.mwProgressBar1 = new MapWindow.Components.mwProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mwProgressBar1
            // 
            this.mwProgressBar1.AccessibleDescription = null;
            this.mwProgressBar1.AccessibleName = null;
            resources.ApplyResources(this.mwProgressBar1, "mwProgressBar1");
            this.mwProgressBar1.BackgroundImage = null;
            this.mwProgressBar1.Font = null;
            this.mwProgressBar1.Name = "mwProgressBar1";
            // 
            // button1
            // 
            this.button1.AccessibleDescription = null;
            this.button1.AccessibleName = null;
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackgroundImage = null;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = null;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblProgressText
            // 
            this.lblProgressText.AccessibleDescription = null;
            this.lblProgressText.AccessibleName = null;
            resources.ApplyResources(this.lblProgressText, "lblProgressText");
            this.lblProgressText.Font = null;
            this.lblProgressText.Name = "lblProgressText";
            // 
            // ProgressCancelDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.lblProgressText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mwProgressBar1);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressCancelDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProgressCancelDialog
        /// </summary>
        public ProgressCancelDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

      

        #endregion

        #region Events

        #endregion

        #region Event Handlers

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

        #region IProgressHandler Members



        public void Progress(string Key, int Percent, string Message)
        {
            mwProgressBar1.Value = Percent;
            SetMess ms = SetMessage;
            if(InvokeRequired)
            {
                Invoke(ms, new object[] {Message});
            }
            else
            {
                SetMessage(Message);
            }
        }

        private void SetMessage(string text)
        {
            lblProgressText.Text = text;
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            _cancelled = true;
            if(Cancelled != null)Cancelled(this, new EventArgs());
            Hide();
        }

        #region ICancelProgressHandler Members

        public bool Cancel
        {
            get { return _cancelled; }
        }

        #endregion
    }
}