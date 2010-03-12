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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/4/2009 1:56:04 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Drawing;
namespace MapWindow.Components
{


    /// <summary>
    /// LineJoinControl
    /// </summary>
    [DefaultEvent("ValueChanged"), DefaultProperty("Value"),
    ToolboxBitmap(typeof(LineJoinControl), "UserControl.ico")]
    public class LineJoinControl : UserControl
    {

        #region Events

        /// <summary>
        /// Occurs when one of the radio buttons is clicked, changing the current value.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion


        #region Private Variables

        private GroupBox grpLineJoins;
        private RadioButton radBevel;
        private RadioButton radRound;
        private RadioButton radMiter;
        private LineJoinTypes _joinType;
        private IContainer components = null;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LineJoinControl
        /// </summary>
        public LineJoinControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineJoinControl));
            this.grpLineJoins = new System.Windows.Forms.GroupBox();
            this.radBevel = new System.Windows.Forms.RadioButton();
            this.radRound = new System.Windows.Forms.RadioButton();
            this.radMiter = new System.Windows.Forms.RadioButton();
            this.grpLineJoins.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpLineJoins
            // 
            this.grpLineJoins.AccessibleDescription = null;
            this.grpLineJoins.AccessibleName = null;
            resources.ApplyResources(this.grpLineJoins, "grpLineJoins");
            this.grpLineJoins.BackgroundImage = null;
            this.grpLineJoins.Controls.Add(this.radBevel);
            this.grpLineJoins.Controls.Add(this.radRound);
            this.grpLineJoins.Controls.Add(this.radMiter);
            this.grpLineJoins.Font = null;
            this.grpLineJoins.Name = "grpLineJoins";
            this.grpLineJoins.TabStop = false;
            this.grpLineJoins.Enter += new System.EventHandler(this.grpLineJoins_Enter);
            // 
            // radBevel
            // 
            this.radBevel.AccessibleDescription = null;
            this.radBevel.AccessibleName = null;
            resources.ApplyResources(this.radBevel, "radBevel");
            this.radBevel.BackgroundImage = null;
            this.radBevel.Font = null;
            this.radBevel.Name = "radBevel";
            this.radBevel.UseVisualStyleBackColor = true;
            this.radBevel.CheckedChanged += new System.EventHandler(this.radBevel_CheckedChanged);
            // 
            // radRound
            // 
            this.radRound.AccessibleDescription = null;
            this.radRound.AccessibleName = null;
            resources.ApplyResources(this.radRound, "radRound");
            this.radRound.BackgroundImage = null;
            this.radRound.Checked = true;
            this.radRound.Font = null;
            this.radRound.Name = "radRound";
            this.radRound.TabStop = true;
            this.radRound.UseVisualStyleBackColor = true;
            this.radRound.CheckedChanged += new System.EventHandler(this.radRound_CheckedChanged);
            // 
            // radMiter
            // 
            this.radMiter.AccessibleDescription = null;
            this.radMiter.AccessibleName = null;
            resources.ApplyResources(this.radMiter, "radMiter");
            this.radMiter.BackgroundImage = null;
            this.radMiter.Font = null;
            this.radMiter.Name = "radMiter";
            this.radMiter.UseVisualStyleBackColor = true;
            this.radMiter.CheckedChanged += new System.EventHandler(this.radMiter_CheckedChanged);
            // 
            // LineJoinControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.grpLineJoins);
            this.Font = null;
            this.Name = "LineJoinControl";
            this.grpLineJoins.ResumeLayout(false);
            this.grpLineJoins.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string text
        /// </summary>
        public override string Text
        {
            get
            {
                if(grpLineJoins != null)return grpLineJoins.Text;
                return null;
            }
            set
            {
                if(grpLineJoins != null) grpLineJoins.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the current line join type shown by the control.
        /// </summary>
        public LineJoinTypes Value
        {
            get { return _joinType; }
            set 
            { 
                _joinType = value;
                switch (value)
                {
                    case LineJoinTypes.Bevel: radBevel.Checked = true; break;
                    case LineJoinTypes.Mitre: radMiter.Checked = true; break;
                    case LineJoinTypes.Round: radRound.Checked = true; break;
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Disposes child elements as well
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Fires the on value changed event
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null) ValueChanged(this, new EventArgs());
        }

        #endregion

        private void radMiter_CheckedChanged(object sender, EventArgs e)
        {
            if (radMiter.Checked && _joinType != LineJoinTypes.Mitre)
            {
                _joinType = LineJoinTypes.Mitre;
                OnValueChanged();
            }
        }


        private void radRound_CheckedChanged(object sender, EventArgs e)
        {
            if (radRound.Checked && _joinType != LineJoinTypes.Round)
            {
                _joinType = LineJoinTypes.Round;
                OnValueChanged();
            }
        }

        private void radBevel_CheckedChanged(object sender, EventArgs e)
        {
            if (radBevel.Checked && _joinType != LineJoinTypes.Bevel)
            {
                _joinType = LineJoinTypes.Bevel;
                OnValueChanged();
            }
        }

        private void grpLineJoins_Enter(object sender, EventArgs e)
        {

        }

    }
}
