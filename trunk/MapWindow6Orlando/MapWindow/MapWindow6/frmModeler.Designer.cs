namespace MapWindow
{
    partial class frmModeler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModeler));
            this.modeler1 = new MapWindow.Tools.Modeler();
            this.modelerToolStrip1 = new MapWindow.Tools.ModelerToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.modelerToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // modeler1
            // 
            this.modeler1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.modeler1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.modeler1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.modeler1.DataColor = System.Drawing.Color.LightGreen;
            this.modeler1.DataFont = new System.Drawing.Font("Tahoma", 8F);
            this.modeler1.DataShape = MapWindow.Tools.ModelShapes.Ellipse;
            this.modeler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modeler1.DrawingQuality = System.Drawing.Drawing2D.SmoothingMode.Default;
            this.modeler1.IsInitialized = true;
            this.modeler1.Location = new System.Drawing.Point(0, 0);
            this.modeler1.Name = "modeler1";
            this.modeler1.ShowWaterMark = true;
            this.modeler1.Size = new System.Drawing.Size(625, 431);
            this.modeler1.TabIndex = 0;
            this.modeler1.ToolColor = System.Drawing.Color.Khaki;
            this.modeler1.ToolFont = new System.Drawing.Font("Tahoma", 8F);
            this.modeler1.ToolManager = null;
            this.modeler1.ToolShape = MapWindow.Tools.ModelShapes.Rectangle;
            this.modeler1.ZoomFactor = 1F;
            // 
            // modelerToolStrip1
            // 
            this.modelerToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.modelerToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.modelerToolStrip1.Name = "modelerToolStrip1";
            this.modelerToolStrip1.Size = new System.Drawing.Size(625, 25);
            this.modelerToolStrip1.TabIndex = 1;
            this.modelerToolStrip1.Text = "modelerToolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // frmModeler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 431);
            this.Controls.Add(this.modelerToolStrip1);
            this.Controls.Add(this.modeler1);
            this.Name = "frmModeler";
            this.Text = "frmModeler";
            this.modelerToolStrip1.ResumeLayout(false);
            this.modelerToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapWindow.Tools.Modeler modeler1;
        private MapWindow.Tools.ModelerToolStrip modelerToolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}