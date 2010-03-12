using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MapWindow.Components
{
    /// <summary>
    /// A user control for specifying angles
    /// </summary>
    [DefaultEvent("AngleChanged"),
    ToolboxBitmap(typeof(AngleControl), "Angles.AngleControl.ico")]
    public class AngleControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the angle changes either by the picker or the textbox.
        /// </summary>
        public event EventHandler AngleChanged;

        /// <summary>
        /// Occurs when the mouse up event occurs on the picker
        /// </summary>
        public event EventHandler AngleChosen;

        #endregion

        #region Private Variables

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private AnglePicker anglePicker1;
        private NumericUpDown nudAngle;
        private System.Windows.Forms.Label lblText;

        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AngleControl));
            this.lblText = new System.Windows.Forms.Label();
            this.nudAngle = new System.Windows.Forms.NumericUpDown();
            this.anglePicker1 = new MapWindow.Components.AnglePicker();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.AccessibleDescription = null;
            this.lblText.AccessibleName = null;
            resources.ApplyResources(this.lblText, "lblText");
            this.lblText.Font = null;
            this.lblText.Name = "lblText";
            // 
            // nudAngle
            // 
            this.nudAngle.AccessibleDescription = null;
            this.nudAngle.AccessibleName = null;
            resources.ApplyResources(this.nudAngle, "nudAngle");
            this.nudAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudAngle.Minimum = new decimal(new int[] {
            -360,
            0,
            0,
            -2147483648});
            this.nudAngle.Name = "nudAngle";
            this.nudAngle.ValueChanged += new System.EventHandler(this.nudAngle_ValueChanged);
            // 
            // anglePicker1
            // 
            this.anglePicker1.AccessibleDescription = null;
            this.anglePicker1.AccessibleName = null;
            resources.ApplyResources(this.anglePicker1, "anglePicker1");
            this.anglePicker1.Angle = 0;
            this.anglePicker1.BackgroundImage = null;
            this.anglePicker1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.anglePicker1.CircleBorderColor = System.Drawing.Color.LightGray;
            this.anglePicker1.CircleBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.anglePicker1.CircleFillColor = System.Drawing.Color.LightGray;
            this.anglePicker1.Clockwise = false;
            this.anglePicker1.Font = null;
            this.anglePicker1.KnobColor = System.Drawing.Color.Green;
            this.anglePicker1.KnobVisible = true;
            this.anglePicker1.Name = "anglePicker1";
            this.anglePicker1.PieFillColor = System.Drawing.Color.SteelBlue;
            this.anglePicker1.Snap = 3;
            this.anglePicker1.StartAngle = 0;
            this.anglePicker1.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.anglePicker1.AngleChanged += new System.EventHandler(this.anglePicker1_AngleChanged);
            // 
            // AngleControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = null;
            this.Controls.Add(this.nudAngle);
            this.Controls.Add(this.anglePicker1);
            this.Controls.Add(this.lblText);
            this.Font = null;
            this.Name = "AngleControl";
            ((System.ComponentModel.ISupportInitialize)(this.nudAngle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        #region Constructors
        /// <summary>
        /// A user control designed to allow an angle to be chosen
        /// </summary>
        public AngleControl()
        {
            InitializeComponent();
            Configure();
        }


        private void Configure()
        {
            anglePicker1.AngleChosen += new EventHandler(anglePicker1_AngleChosen);
        }

        void anglePicker1_AngleChosen(object sender, EventArgs e)
        {
            OnAngleChosen();
        }
    

        #endregion

        /// <summary>
        /// Gets or sets the integer angle in degrees.
        /// </summary>
        [Description("Gets or sets the integer angle in degrees")]
        public int Angle
        {
            get { return anglePicker1.Angle; }
            set
            {
                anglePicker1.Angle = value;
                nudAngle.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if the values should increase in the 
        /// clockwise direction instead of the counter clockwise direction
        /// </summary>
        [Description("Gets or sets a boolean indicating if the values should increase in the clockwise direction instead of the counter clockwise direction")]
        public bool Clockwise
        {
            get { return anglePicker1.Clockwise; }
            set
            {
                anglePicker1.Clockwise = value;
            }
        }



        /// <summary>
        /// Gets or sets the base Knob Color
        /// </summary>
        [Description("Gets or sets the color to use as the base color for drawing the knob.")]
        public Color KnobColor
        {
            get { return anglePicker1.KnobColor; }
            set { anglePicker1.KnobColor = value; }
        }

        /// <summary>
        /// Gets or sets the start angle in degrees measured counter clockwise from the X axis.
        /// For instance, for an azimuth angle that starts at the top, this should be set to 90.
        /// </summary>
        [Description("Gets or sets the start angle in degrees measured counter clockwise from the X axis.  For instance, for an azimuth angle that starts at the top, this should be set to 90.")]
        public int StartAngle
        {
            get { return anglePicker1.StartAngle; }
            set { anglePicker1.StartAngle = value; }
        }

        /// <summary>
        /// Gets or sets the string text for this control.
        /// </summary>
        [Localizable(true), Description("Gets or sets the string text for this control.")]
        public string Caption
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        #region Protected Methods

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

    
        /// <summary>
        /// Fires the angle changed event
        /// </summary>
        protected virtual void OnAngleChanged()
        {
            if (AngleChanged != null) AngleChanged(this, new EventArgs());
        }

        /// <summary>
        /// fires an event once the mouse has been released.
        /// </summary>
        protected virtual void OnAngleChosen()
        {
            if (AngleChosen != null) AngleChosen(this, new EventArgs());
        }


        #endregion

        
        private void nudAngle_ValueChanged(object sender, EventArgs e)
        {
            if (anglePicker1.Angle != nudAngle.Value)
            {
                anglePicker1.Angle = (int)nudAngle.Value;
                OnAngleChanged();
            }
        }

        private void anglePicker1_AngleChanged(object sender, EventArgs e)
        {
            if (nudAngle.Value != anglePicker1.Angle)
            {
                nudAngle.Value = anglePicker1.Angle;
                OnAngleChanged();
            }
        }


    }
}
