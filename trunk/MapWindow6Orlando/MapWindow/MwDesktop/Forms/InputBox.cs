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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/29/2008 3:48:03 PM
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

using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Components;
using MapWindow.Main;
namespace MapWindow.Forms
{
    /// <summary>
    /// frmInputBox
    /// </summary>
    public class InputBox : Form
    {
        

        #region Private Variables

        private Label lblMessageText;
        private TextBox txtInput;
        private Button cmdOk;
        private Button cmdCancel;

        private ValidationTypes _validation;

       

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputBox));
            this.lblMessageText = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMessageText
            // 
            this.lblMessageText.AccessibleDescription = null;
            this.lblMessageText.AccessibleName = null;
            resources.ApplyResources(this.lblMessageText, "lblMessageText");
            this.lblMessageText.Name = "lblMessageText";
            // 
            // txtInput
            // 
            this.txtInput.AccessibleDescription = null;
            this.txtInput.AccessibleName = null;
            resources.ApplyResources(this.txtInput, "txtInput");
            this.txtInput.BackgroundImage = null;
            this.txtInput.Font = null;
            this.txtInput.Name = "txtInput";
            // 
            // cmdOk
            // 
            this.cmdOk.AccessibleDescription = null;
            this.cmdOk.AccessibleName = null;
            resources.ApplyResources(this.cmdOk, "cmdOk");
            this.cmdOk.BackColor = System.Drawing.Color.Transparent;
            this.cmdOk.BackgroundImage = null;
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.UseVisualStyleBackColor = false;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.AccessibleDescription = null;
            this.cmdCancel.AccessibleName = null;
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackColor = System.Drawing.Color.Transparent;
            this.cmdCancel.BackgroundImage = null;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // InputBox
            // 
            this.AcceptButton = this.cmdOk;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.cmdCancel;
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblMessageText);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of frmInputBox
        /// </summary>
        public InputBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new instance of frmInputBox
        /// </summary>
        /// <param name="text">Sets the text of the message to show.</param>
        public InputBox(string text):this()
        {
            lblMessageText.Text = text;
            _validation = ValidationTypes.None;
        }

        /// <summary>
        /// Creates a new instance of frmInputBox
        /// </summary>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        public InputBox(string text, string caption):this()
        {
            lblMessageText.Text = text;
            _validation = ValidationTypes.None;
            Text = caption;
        }

        /// <summary>
        /// Creates a new instance of frmInputBox
        /// </summary>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        /// <param name="validation">A MapWindow.Main.ValidationType enumeration specifying acceptable validation to return OK.</param>
        public InputBox(string text, string caption, ValidationTypes validation):this()
        {
            lblMessageText.Text = text;
            _validation = validation;
            Text = caption;
        }

        /// <summary>
        /// Creates a new instance of frmInputBox
        /// </summary>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        /// <param name="validation">A MapWindow.Main.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="icon">Specifies an icon to appear on this messagebox.</param>
        public InputBox(string text, string caption, ValidationTypes validation, Icon icon)
            : this()
        {
            lblMessageText.Text = text;
            _validation = validation;
            Text = caption;
            ShowIcon = true;
            Icon = icon;
        }

       

        /// <summary>
        /// Creates a new instance of frmInputBox
        /// </summary>
        /// <param name="owner">Specifies the Form to set as the owner of this dialog.</param>
        /// <param name="text">Sets the text of the message to show.</param>
        public InputBox(Form owner, string text)
            : this()
        {
            this.Owner = owner;
           
            lblMessageText.Text = text;
        }

        /// <summary>
        /// Creates a new instance of frmInputBox
        /// </summary>
        /// <param name="owner">Specifies the Form  to set as the owner of this dialog.</param>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        public InputBox(Form owner, string text, string caption)
            : this()
        {
            this.Owner = owner;
            
            lblMessageText.Text = text;
            Text = caption;
        }

        /// <summary>
        /// Creates a new instance of frmInputBox
        /// </summary>
        /// <param name="owner">Specifies the Form to set as the owner of this dialog.</param>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        /// <param name="validation">A MapWindow.Main.ValidationType enumeration specifying acceptable validation to return OK.</param>
        public InputBox(Form owner, string text, string caption, ValidationTypes validation)
            : this()
        {
            this.Owner = owner;
           
            lblMessageText.Text = text;
            Text = caption;
            _validation = validation;
        }

        /// <summary>
        /// Creates a new instance of frmInputBox
        /// </summary>
        /// <param name="owner">Specifies the Form to set as the owner of this dialog.</param>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        /// <param name="validation">A MapWindow.Main.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="icon">Specifies an icon to appear on this messagebox.</param>
        public InputBox(Form owner, string text, string caption, ValidationTypes validation, Icon icon)
            : this()
        {
            this.Owner = owner;
            _validation = validation;
            lblMessageText.Text = text;
            Text = caption;
            ShowIcon = true;
            Icon = icon;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// The string result that was entered for this text box.
        /// </summary>
        public string Result
        {
            get
            {
                return txtInput.Text;
            }
        }

        /// <summary>
        /// Gets or sets the type of validation to force on the value before the OK option is permitted.
        /// </summary>
        public ValidationTypes Validation
        {
            get { return _validation; }
            set { _validation = value; }
        }

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

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            // Parse the value entered in the text box.  If the value doesn't match the criteria, don't
            // allow the user to exit the dialog by pressing ok.
            switch (_validation)
            {
                case ValidationTypes.Byte:
                    if (Global.IsByte(txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "byte"));
                        return;
                    }
                    break;
                case ValidationTypes.Double:
                    if (Global.IsDouble(txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "double"));
                        return;
                    }
                    break;
                case ValidationTypes.Float:
                    if (Global.IsFloat(txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "float"));
                        return;
                    }
                    break;
                case ValidationTypes.Integer:
                    if (Global.IsInteger(txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "integer"));
                        return;
                    }
                    break;
                case ValidationTypes.PositiveDouble:
                    if (Global.IsDouble(txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "double"));
                        return;
                    }
                    else
                    {
                        if (Global.GetDouble(txtInput.Text) < 0)
                        {
                            LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "positive double"));
                            return;
                        }
                    }
                    break;
                case ValidationTypes.PositiveFloat:
                    if (Global.IsFloat(txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "float"));
                        return;
                    }
                    else
                    {
                        if (Global.GetFloat(txtInput.Text) < 0)
                        {
                            LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "positive float"));
                            return;
                        }
                    }
                    break;
                case ValidationTypes.PositiveInteger:
                    if (Global.IsInteger(txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "integer"));
                        return;
                    }
                    else
                    {
                        if (Global.GetInteger(txtInput.Text) < 0)
                        {
                            LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "positive integer"));
                            return;
                        }
                    }
                    break;
                case ValidationTypes.PositiveShort:
                    if (Global.IsShort(txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "short"));
                        return;
                    }
                    else
                    {
                        if (Global.GetShort(txtInput.Text) < 0)
                        {
                            LogManager.DefaultLogManager.LogMessageBox(MessageStrings.ParseFailed_S.Replace("%S", "positive short"));
                            return;
                        }
                    }
                    break;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

      
    }
}