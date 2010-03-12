namespace MapWindow
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.cmbCultureInfo = new System.Windows.Forms.ComboBox();
            this.lblCulturePreferences = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbCultureInfo
            // 
            this.cmbCultureInfo.AccessibleDescription = null;
            this.cmbCultureInfo.AccessibleName = null;
            resources.ApplyResources(this.cmbCultureInfo, "cmbCultureInfo");
            this.cmbCultureInfo.BackgroundImage = null;
            this.cmbCultureInfo.Font = null;
            this.cmbCultureInfo.FormattingEnabled = true;
            this.cmbCultureInfo.Name = "cmbCultureInfo";
            this.cmbCultureInfo.SelectedIndexChanged += new System.EventHandler(this.cmbCultureInfo_SelectedIndexChanged);
            // 
            // lblCulturePreferences
            // 
            this.lblCulturePreferences.AccessibleDescription = null;
            this.lblCulturePreferences.AccessibleName = null;
            resources.ApplyResources(this.lblCulturePreferences, "lblCulturePreferences");
            this.lblCulturePreferences.Name = "lblCulturePreferences";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackgroundImage = null;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = null;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnApply
            // 
            this.btnApply.AccessibleDescription = null;
            this.btnApply.AccessibleName = null;
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.BackgroundImage = null;
            this.btnApply.Font = null;
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSettings
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblCulturePreferences);
            this.Controls.Add(this.cmbCultureInfo);
            this.Font = null;
            this.Icon = null;
            this.Name = "frmSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCultureInfo;
        private System.Windows.Forms.Label lblCulturePreferences;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
    }
}