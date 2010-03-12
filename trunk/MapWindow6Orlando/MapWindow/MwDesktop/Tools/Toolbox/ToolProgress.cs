//********************************************************************************************************
// Product Name: MapWindow.Tools.ToolProgress
// Description:  A form used to show the progress of a tool
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Feb, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Name                  |   Date            |            Comments
// ----------------------|-------------------|-----------------------------------------------------
// Ted Dunsford          | 8/24/2009         |  Used Re-sharper to clean up a few unnecessary accessors        
//********************************************************************************************************

using System;
using System.Windows.Forms;

namespace MapWindow.Tools
{
    /// <summary>
    /// A form which shows the progress of a tool
    /// </summary>
    public class ToolProgress : Form, ICancelProgressHandler
    {
        #region ------------------ Class Variables

        private ProgressBar _progressBarTool;
        private ProgressBar progressBarTotal;
        private Label lblTotal;
        private Label lblTool;
        private TextBox txtBoxStatus;
        private Button btnCancel;
        private int _toolProgressCount;
        private bool _cancelPressed;
        private bool _executionComplete;

        #endregion

        #region ------------------ constructor

        /// <summary>
        /// Creates an instance of the Tool Progress forms and hands over an array of tools which will then be executed
        /// </summary>
        /// <param name="numTools">The number of tools that are going to be executed</param>
        public ToolProgress(int numTools)
        {
            InitializeComponent();
            progressBarTotal.Maximum = numTools * 100;
            progressBarTotal.Minimum = 0;
            _progressBarTool.Maximum = 100;
            _progressBarTool.Minimum = 0;
            _executionComplete = false;
        }

        #endregion

        #region ------------------- Methods

        /// <summary>
        /// This method should be called when the process has been completed
        /// </summary>
        public void executionComplete()
        {
            if (InvokeRequired)
            {
                updateExecComp uec = updateExecutionComplete;
                BeginInvoke(uec);
            }
            else
            {
                updateExecutionComplete();
            }
        }

        private delegate void updateExecComp();

        private void updateExecutionComplete()
        {
            btnCancel.Text = "Close";
            _executionComplete = true;
        }

        /// <summary>
        /// Handles the progress method necessary to implement IProgress
        /// </summary>
        /// <param name="Key">This a message with no percentage information..this is ignored</param>
        /// <param name="Percent">The integer percentage from 0 to 100 that is used to control the progress bar</param>
        /// <param name="Message">The actual complete message to show..this is also ignored</param>
        public void Progress(string Key, int Percent, string Message)
        {
            if (InvokeRequired)
            {
                UpdateProg prg = UpdateProgress;
                BeginInvoke(prg, new object[] { Key, Percent, Message });
            }
            else
            {
                UpdateProgress(Key, Percent, Message);
            }
        }

        private delegate void UpdateProg(string Key, int Percent, string Message);

        private void UpdateProgress(string Key, int Percent, string Message)
        {
            try
            {
                if (Percent < 0) Percent = 0;
                if (Percent > 100) Percent = 100;
                _progressBarTool.Value = Percent;
                progressBarTotal.Value = (_toolProgressCount) * 100 + Percent;
                txtBoxStatus.AppendText("\r\n" + DateTime.Now + ": " + Message);
            }
            catch (Exception)
            {
               
                throw;
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_executionComplete)
                Close();
            else
            {
                _cancelPressed = true;
                return;
            }
        }

        #endregion

        #region ---------------------- Properties

        /// <summary>
        /// Gets or sets the number of tools that have been succesfully executed
        /// </summary>
        public int ToolProgressCount
        {
            get { return _toolProgressCount; }
            set { _toolProgressCount = value; }
        }

        /// <summary>
        /// Returns true if the cancel button was pressed
        /// </summary>
        public bool Cancel
        {
            get { return _cancelPressed; }
        }

        #endregion

        #region  Windows Form Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        protected System.ComponentModel.IContainer components;

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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _progressBarTool = new ProgressBar();
            progressBarTotal = new ProgressBar();
            lblTotal = new Label();
            lblTool = new Label();
            txtBoxStatus = new TextBox();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // progressBarTool
            // 
            _progressBarTool.Anchor = (AnchorStyles.Top | AnchorStyles.Left)
                                      | AnchorStyles.Right;
            _progressBarTool.Location = new System.Drawing.Point(13, 77);
            _progressBarTool.Name = "_progressBarTool";
            _progressBarTool.Size = new System.Drawing.Size(494, 23);
            _progressBarTool.TabIndex = 0;
            // 
            // progressBarTotal
            // 
            progressBarTotal.Anchor = (AnchorStyles.Top | AnchorStyles.Left)
                                      | AnchorStyles.Right;
            progressBarTotal.Location = new System.Drawing.Point(13, 33);
            progressBarTotal.Name = "progressBarTotal";
            progressBarTotal.Size = new System.Drawing.Size(494, 23);
            progressBarTotal.TabIndex = 1;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new System.Drawing.Point(12, 15);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(75, 13);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "Total Progress";
            // 
            // lblTool
            // 
            lblTool.AutoSize = true;
            lblTool.Location = new System.Drawing.Point(12, 59);
            lblTool.Name = "lblTool";
            lblTool.Size = new System.Drawing.Size(72, 13);
            lblTool.TabIndex = 3;
            lblTool.Text = "Tool Progress";
            // 
            // txtBoxStatus
            // 
            txtBoxStatus.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
                                   | AnchorStyles.Left)
                                  | AnchorStyles.Right;
            txtBoxStatus.Location = new System.Drawing.Point(13, 130);
            txtBoxStatus.Multiline = true;
            txtBoxStatus.Name = "txtBoxStatus";
            txtBoxStatus.ScrollBars = ScrollBars.Vertical;
            txtBoxStatus.Size = new System.Drawing.Size(494, 247);
            txtBoxStatus.TabIndex = 4;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(431, 383);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // ToolProgress
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(519, 418);
            Controls.Add(btnCancel);
            Controls.Add(txtBoxStatus);
            Controls.Add(lblTool);
            Controls.Add(lblTotal);
            Controls.Add(progressBarTotal);
            Controls.Add(_progressBarTool);
            Name = "ToolProgress";
            Text = "ToolProgress";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

    }
}