using System;
using MapWindow.Tools.Param;
using System.Windows.Forms;
using System.IO;

namespace MapWindow.Tools.DialogComponents
{
    public class OpenFileElement : DialogElement
    {
        #region Class Variables
        private ComboBox comboFile;
        private System.Windows.Forms.Button btnAddData;
        private Boolean _refreshCombo = true;
        private string _addedValue;
        private string _fileName;
        #endregion

        #region Constructor
        /// <summary>
        /// Create an instance of the dialog
        /// </summary>
        /// <param name="param"></param>
        public OpenFileElement(OpenFileParam param, string text)
        {
            InitializeComponent();
            base.Param = param;
            _fileName = text;
            // textBox1.Text = param.Value;
            GroupBox.Text = param.Name;
            Refresh();

            //Populates the dialog with the default parameter value
            if (param.Value != null && param.DefaultSpecified == true)
            {
                _fileName = param.ModelName;
                base.Status = ToolStatus.Ok;
                base.LightTipText = MapWindow.MessageStrings.FeaturesetValid;
            }
        }

        #endregion

        /// <summary>
        /// updates the param if something's been changed
        /// </summary>
        public override void Refresh()
        {
            //Disable the combo box temporarily
            _refreshCombo = false;

            //We set the combo boxes status to empty to start
            base.Status = ToolStatus.Empty;
            base.LightTipText = MapWindow.MessageStrings.FeaturesetMissing;
             comboFile.Items.Clear();

            //If the user added a feature set
            if (_addedValue != null)
            {
                 comboFile.Items.Add(_addedValue);
                if (base.Param.Value != null && base.Param.DefaultSpecified == true)
                {
                    if (_addedValue == base.Param.Value.ToString())
                    {
                         comboFile.SelectedItem = _addedValue;
                        base.Status = ToolStatus.Ok;
                        base.LightTipText = MapWindow.MessageStrings.FeaturesetValid;
                    }
                }
            }

            //Add all the dataSets back to the combo box
            if (_fileName != null)
            {
                 comboFile.Items.Add(_fileName);

                if (base.Param.Value != null && base.Param.DefaultSpecified == true)
                {
                    if (_fileName == base.Param.Value.ToString())
                    {
                         comboFile.SelectedItem = _fileName;
                        base.Status = ToolStatus.Ok;
                        base.LightTipText = MapWindow.MessageStrings.FeaturesetValid;
                    }
                }

            }

            _refreshCombo = true;
        }

      
        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAddData = new System.Windows.Forms.Button();
            this.comboFile = new System.Windows.Forms.ComboBox();
            this._groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _groupBox1
            // 
            this._groupBox1.Controls.Add(this.btnAddData);
            this._groupBox1.Controls.Add(this.comboFile);
            this._groupBox1.Controls.SetChildIndex(this.comboFile, 0);
            this._groupBox1.Controls.SetChildIndex(this.btnAddData, 0);
            // 
            // btnAddData
            // 
            this.btnAddData.Image = global::MapWindow.Images.AddLayer;
            this.btnAddData.Location = new System.Drawing.Point(450, 10);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(26, 26);
            this.btnAddData.TabIndex = 9;
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // comboFile
            // 
            this.comboFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFile.FormattingEnabled = true;
            this.comboFile.Location = new System.Drawing.Point(34, 12);
            this.comboFile.Name = "comboFile";
            this.comboFile.Size = new System.Drawing.Size(410, 21);
            this.comboFile.TabIndex = 10;
            this.comboFile.SelectedValueChanged += new System.EventHandler(this.comboFile_SelectedValueChanged);
            // 
            // OpenFileElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "OpenFileElement";
            this._groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

      

       

        private void btnAddData_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select File Name";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _addedValue = dialog.FileName;
            }
            this.Param.Value = _addedValue;
            Refresh();
            base.Status = ToolStatus.Ok;
        }

        private void comboFile_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_refreshCombo)
            {
                string name =  comboFile.SelectedItem as string;
                if (name != null)
                {
                    base.Param.Value = name;
                    return;
                }
            }
        }


    }
}
