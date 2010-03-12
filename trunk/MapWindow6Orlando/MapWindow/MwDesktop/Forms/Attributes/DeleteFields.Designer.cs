namespace MapWindow.Forms
{
    partial class frmDeleteField
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDeleteField));
            this.lblHeading = new System.Windows.Forms.Label();
            this.clb = new System.Windows.Forms.CheckedListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeading
            // 
            this.lblHeading.AccessibleDescription = null;
            this.lblHeading.AccessibleName = null;
            resources.ApplyResources(this.lblHeading, "lblHeading");
            this.lblHeading.Font = null;
            this.lblHeading.Name = "lblHeading";
            // 
            // clb
            // 
            this.clb.AccessibleDescription = null;
            this.clb.AccessibleName = null;
            resources.ApplyResources(this.clb, "clb");
            this.clb.BackgroundImage = null;
            this.clb.CheckOnClick = true;
            this.clb.FormattingEnabled = true;
            this.clb.Name = "clb";
            this.clb.SelectedIndexChanged += new System.EventHandler(this.clb_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.Font = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackgroundImage = null;
            this.btnOK.Font = null;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmDeleteField
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.clb);
            this.Controls.Add(this.lblHeading);
            this.Font = null;
            this.Icon = null;
            this.Name = "frmDeleteField";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frmDeleteField_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// This event will fire when user select the item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clb_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.clb.SelectedItems.Count > 0)
            {
                if (this.btnOK.Enabled == false)
                    this.btnOK.Enabled = true;
            }
            else
                this.btnOK.Enabled = false;
        }

        #endregion

        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.CheckedListBox clb;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}