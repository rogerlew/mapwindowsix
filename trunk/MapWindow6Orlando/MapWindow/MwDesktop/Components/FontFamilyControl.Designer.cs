namespace MapWindow.Components
{
    /// <summary>
    /// A control that is specifically designed to allow choosing a font family name
    /// </summary>
    partial class FontFamilyControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontFamilyControl));
            this.ffdNames = new MapWindow.Components.FontFamilyDropDown();
            this.SuspendLayout();
            // 
            // ffdNames
            // 
            this.ffdNames.AccessibleDescription = null;
            this.ffdNames.AccessibleName = null;
            resources.ApplyResources(this.ffdNames, "ffdNames");
            this.ffdNames.BackgroundImage = null;
            this.ffdNames.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ffdNames.Font = null;
            this.ffdNames.FormattingEnabled = true;
            this.ffdNames.Name = "ffdNames";
            // 
            // FontFamilyControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.ffdNames);
            this.Font = null;
            this.Name = "FontFamilyControl";
            this.ResumeLayout(false);

        }

        #endregion

        private FontFamilyDropDown ffdNames;
    }
}
