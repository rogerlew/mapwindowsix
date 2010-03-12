namespace MapWindow
{
    /// <summary>
    /// Test class
    /// </summary>
    partial class frmTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTest));
            this.labelAlignmentControl1 = new MapWindow.Components.LabelAlignmentControl();
            this.SuspendLayout();
            // 
            // labelAlignmentControl1
            // 
            this.labelAlignmentControl1.AccessibleDescription = null;
            this.labelAlignmentControl1.AccessibleName = null;
            resources.ApplyResources(this.labelAlignmentControl1, "labelAlignmentControl1");
            this.labelAlignmentControl1.BackgroundImage = null;
            this.labelAlignmentControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelAlignmentControl1.Font = null;
            this.labelAlignmentControl1.Name = "labelAlignmentControl1";
            this.labelAlignmentControl1.Value = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmTest
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.labelAlignmentControl1);
            this.Font = null;
            this.Icon = null;
            this.Name = "frmTest";
            this.ResumeLayout(false);

        }

        #endregion

        private MapWindow.Components.LabelAlignmentControl labelAlignmentControl1;





    }
}