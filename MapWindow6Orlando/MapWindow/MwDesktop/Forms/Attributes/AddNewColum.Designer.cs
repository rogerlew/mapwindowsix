namespace MapWindow.Forms
{
    /// <summary>
    /// Presumably a class for adding a new column to the attribute Table editor
    /// </summary>
    partial class AddNewColum
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewColum));
            this.lblName = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.nudSize = new System.Windows.Forms.NumericUpDown();
            this.dialogButtons1 = new MapWindow.Components.DialogButtons();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AccessibleDescription = null;
            this.lblName.AccessibleName = null;
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Font = null;
            this.lblName.Name = "lblName";
            // 
            // lblType
            // 
            this.lblType.AccessibleDescription = null;
            this.lblType.AccessibleName = null;
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Font = null;
            this.lblType.Name = "lblType";
            // 
            // lblSize
            // 
            this.lblSize.AccessibleDescription = null;
            this.lblSize.AccessibleName = null;
            resources.ApplyResources(this.lblSize, "lblSize");
            this.lblSize.Font = null;
            this.lblSize.Name = "lblSize";
            // 
            // txtName
            // 
            this.txtName.AccessibleDescription = null;
            this.txtName.AccessibleName = null;
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.BackgroundImage = null;
            this.txtName.Font = null;
            this.txtName.Name = "txtName";
            // 
            // cmbType
            // 
            this.cmbType.AccessibleDescription = null;
            this.cmbType.AccessibleName = null;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.BackgroundImage = null;
            this.cmbType.Font = null;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2")});
            this.cmbType.Name = "cmbType";
            // 
            // nudSize
            // 
            this.nudSize.AccessibleDescription = null;
            this.nudSize.AccessibleName = null;
            resources.ApplyResources(this.nudSize, "nudSize");
            this.nudSize.Font = null;
            this.nudSize.Name = "nudSize";
            this.nudSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
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
            // AddNewColum
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.dialogButtons1);
            this.Controls.Add(this.nudSize);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblName);
            this.Font = null;
            this.Name = "AddNewColum";
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.NumericUpDown nudSize;
        private MapWindow.Components.DialogButtons dialogButtons1;
    }
}