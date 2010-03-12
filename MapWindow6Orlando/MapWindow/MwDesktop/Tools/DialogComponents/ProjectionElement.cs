//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2009 1:41:29 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System.Windows.Forms;
using MapWindow.Forms;

namespace MapWindow.Tools
{


    /// <summary>
    /// ProjectionElement
    /// </summary>
    internal class ProjectionElement : DialogElement
    {
        private Label lblProjection;
        private Button cmdSelect;
        private ToolTip _tthelp;
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProjectionElement
        /// </summary>
        public ProjectionElement(ProjectionParam value)
        {
            
            _tthelp = new ToolTip();
            base.Param = value;
            InitializeComponent();
            if (value == null) return;
            base.GroupBox.Text = value.Name;
            if(value.Value != null)
            {
                lblProjection.Text = value.Value.ToProj4String();
            }
            value.ValueChanged += _value_ValueChanged;
        }

        void _value_ValueChanged(Param.Parameter sender)
        {
            if (Param == null) return;
            if (Param.Value == null)
            {
                lblProjection.Text = "";
                return;
            }
            lblProjection.Text = Param.Value.ToProj4String();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new ProjectionParam Param
        {
            get { return (ProjectionParam)base.Param; }
            set
            {
                Status = ToolStatus.Empty;
                LightTipText = MessageStrings.ParameterInvalid;
                base.Param = value;
                if(value == null) return;
                if (value.Value == null) return;
                lblProjection.Text = value.Value.ToProj4String();
                Status = ToolStatus.Ok;
                LightTipText = MessageStrings.ParameterValid;
                Invalidate();
            }
        }


        #endregion

        private void InitializeComponent()
        {
            this.lblProjection = new System.Windows.Forms.Label();
            this.cmdSelect = new System.Windows.Forms.Button();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.GroupBox.Controls.Add(this.lblProjection);
            this.GroupBox.Controls.Add(this.cmdSelect);
            this.GroupBox.Controls.SetChildIndex(this.cmdSelect, 0);
            this.GroupBox.Controls.SetChildIndex(this.lblProjection, 0);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            // 
            // lblProjection
            // 
            this.lblProjection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProjection.BackColor = System.Drawing.Color.White;
            this.lblProjection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProjection.Location = new System.Drawing.Point(39, 16);
            this.lblProjection.Name = "lblProjection";
            this.lblProjection.Size = new System.Drawing.Size(405, 20);
            this.lblProjection.TabIndex = 2;
            this.lblProjection.Text = "Press the button to select a projection";
            // 
            // cmdSelect
            // 
            this.cmdSelect.Location = new System.Drawing.Point(450, 15);
            this.cmdSelect.Name = "cmdSelect";
            this.cmdSelect.Size = new System.Drawing.Size(36, 23);
            this.cmdSelect.TabIndex = 3;
            this.cmdSelect.Text = "...";
            this.cmdSelect.UseVisualStyleBackColor = true;
            this.cmdSelect.Click += new System.EventHandler(this.cmdSelect_Click);
            // 
            // ProjectionElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "ProjectionElement";
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       

        private void cmdSelect_Click(object sender, System.EventArgs e)
        {
            ProjectionSelectDialog dlg = new ProjectionSelectDialog();
            if (dlg.ShowDialog() != DialogResult.OK) return;
            if(Param == null)
            {
                Param = new ProjectionParam("dest", dlg.SelectedCoordinateSystem);
            }
            else
            {
                Param.Value = dlg.SelectedCoordinateSystem;
            }
            lblProjection.Text = Param.Value.ToProj4String();
            _tthelp.SetToolTip(lblProjection, Param.Value.ToProj4String());
            

        }

        public override void Refresh()
        {
            Status = ToolStatus.Empty;
            LightTipText = MessageStrings.ParameterInvalid;
            if (Param == null) return;
            if (Param.Value == null) return;
            Status = ToolStatus.Ok;
            LightTipText = MessageStrings.ParameterValid;
        }


    }
}
