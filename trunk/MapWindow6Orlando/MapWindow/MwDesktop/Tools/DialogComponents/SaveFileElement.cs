using System;
using MapWindow.Tools.Param;
using System.Windows.Forms;
using System.IO;

namespace MapWindow.Tools.DialogComponents
{
    public class SaveFileElement : DialogElement
    {
        public SaveFileElement(SaveFilePram param)
        {
            InitializeComponent();
            this.Param = param;
            txtDataTable.Text = param.Value;
            GroupBox.Text = param.Name;

            Refresh();

            //Populates the dialog with the default parameter value
            if (param.Value != null && param.DefaultSpecified == true)
            {
                txtDataTable.Text = param.ModelName;
                base.Status = ToolStatus.Ok;
                base.LightTipText = MapWindow.MessageStrings.FeaturesetValid;
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtDataTable = new System.Windows.Forms.TextBox();
            this.btnAddData = new System.Windows.Forms.Button();
            this._groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _groupBox1
            // 
            this._groupBox1.Controls.Add(this.btnAddData);
            this._groupBox1.Controls.SetChildIndex(this.btnAddData, 0);
            // 
            // txtDataTable
            // 
            this.txtDataTable.Location = new System.Drawing.Point(45, 13);
            this.txtDataTable.Name = "txtDataTable";
            this.txtDataTable.Size = new System.Drawing.Size(356, 20);
            this.txtDataTable.TabIndex = 9;
            // 
            // btnAddData
            // 
            this.btnAddData.Image = global::MapWindow.Images.AddLayer;
            this.btnAddData.Location = new System.Drawing.Point(432, 10);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(26, 26);
            this.btnAddData.TabIndex = 8;
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // SaveFileElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDataTable);
            this.Name = "SaveFileElement";
            this.Controls.SetChildIndex(this._groupBox1, 0);
            this.Controls.SetChildIndex(this.txtDataTable, 0);
            this._groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDataTable;
        private System.Windows.Forms.Button btnAddData;

        private void btnAddData_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Select File Name";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtDataTable.Text = dialog.FileName;
            }
            this.Param.Value = txtDataTable.Text;

            Refresh();
            base.Status = ToolStatus.Ok;
        }

    }
}
