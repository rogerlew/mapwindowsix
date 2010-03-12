using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapWindow.Components
{
    /// <summary>
    /// A Control that can be used to create custom gradients.
    /// </summary>
    [ToolboxBitmap(typeof(GradientControl), "UserControl.ico"),
    DefaultEvent("GradientChanging")]
    public class GradientControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the gradient is updated to its final value when the sliders or levers are released.
        /// </summary>
        public event EventHandler GradientChanged;

        /// <summary>
        /// Occurs when the gradient changes as the result of a sliding action, either from the dragging of a slider or else
        /// the dragging of a lever.
        /// </summary>
        public event EventHandler GradientChanging;

        #endregion

        #region Private Variables

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ColorLever leverMinimum;
        private GradientSlider gradientSlider1;
        private ColorLever leverMaximum;

        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GradientControl));
            this.gradientSlider1 = new MapWindow.Components.GradientSlider();
            this.leverMinimum = new MapWindow.Components.ColorLever();
            this.leverMaximum = new MapWindow.Components.ColorLever();
            this.SuspendLayout();
            // 
            // gradientSlider1
            // 
            this.gradientSlider1.LeftHandle.Color = System.Drawing.Color.SteelBlue;
            this.gradientSlider1.LeftHandle.Position = 0.2F;
            this.gradientSlider1.LeftHandle.RoundingRadius = 4;
            this.gradientSlider1.LeftHandle.Visible = true;
            this.gradientSlider1.LeftHandle.Width = 10;
            resources.ApplyResources(this.gradientSlider1, "gradientSlider1");
            this.gradientSlider1.Maximum = 1F;
            this.gradientSlider1.MaximumColor = System.Drawing.Color.Blue;
            this.gradientSlider1.Minimum = 0F;
            this.gradientSlider1.MinimumColor = System.Drawing.Color.Lime;
            this.gradientSlider1.Name = "gradientSlider1";
            this.gradientSlider1.RightHandle.Color = System.Drawing.Color.SteelBlue;
            this.gradientSlider1.RightHandle.Position = 0.8F;
            this.gradientSlider1.RightHandle.RoundingRadius = 4;
            this.gradientSlider1.RightHandle.Visible = true;
            this.gradientSlider1.RightHandle.Width = 10;
            this.gradientSlider1.PositionChanging += new System.EventHandler(this.gradientSlider1_PositionChanging);
            this.gradientSlider1.PositionChanged += new System.EventHandler(this.gradientSlider1_PositionChanged);
            // 
            // leverMinimum
            // 
            this.leverMinimum.Angle = 0;
            this.leverMinimum.BackColor = System.Drawing.SystemColors.Control;
            this.leverMinimum.BarLength = 5;
            this.leverMinimum.BarWidth = 5;
            this.leverMinimum.BorderWidth = 5;
            this.leverMinimum.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.leverMinimum.Flip = true;
            this.leverMinimum.KnobColor = System.Drawing.Color.SteelBlue;
            this.leverMinimum.KnobRadius = 7;
            resources.ApplyResources(this.leverMinimum, "leverMinimum");
            this.leverMinimum.Name = "leverMinimum";
            this.leverMinimum.Opacity = 0F;
            this.leverMinimum.ColorChanged += new System.EventHandler(this.leverMinimum_ColorChanged);
            this.leverMinimum.ColorChanging += new System.EventHandler(this.leverMinimum_ColorChanging);
            // 
            // leverMaximum
            // 
            this.leverMaximum.Angle = 0;
            this.leverMaximum.BackColor = System.Drawing.SystemColors.Control;
            this.leverMaximum.BarLength = 5;
            this.leverMaximum.BarWidth = 5;
            this.leverMaximum.BorderWidth = 5;
            this.leverMaximum.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.leverMaximum.Flip = false;
            this.leverMaximum.KnobColor = System.Drawing.Color.SteelBlue;
            this.leverMaximum.KnobRadius = 7;
            resources.ApplyResources(this.leverMaximum, "leverMaximum");
            this.leverMaximum.Name = "leverMaximum";
            this.leverMaximum.Opacity = 1F;
            this.leverMaximum.ColorChanged += new System.EventHandler(this.leverMaximum_ColorChanged);
            this.leverMaximum.ColorChanging += new System.EventHandler(this.leverMaximum_ColorChanging);
            // 
            // GradientControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gradientSlider1);
            this.Controls.Add(this.leverMinimum);
            this.Controls.Add(this.leverMaximum);
            this.Name = "GradientControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        
       
        #endregion

        /// <summary>
        /// Creates a new instance of a Gradient Control
        /// </summary>
        public GradientControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the position of the minimum handle
        /// </summary>
        public float StartValue
        {
            get { return gradientSlider1.LeftHandle.Position; }
            set { gradientSlider1.LeftHandle.Position = value; }
        }

        /// <summary>
        /// Gets or sets the position of the maximum handle
        /// </summary>
        public float EndValue
        {
            get { return gradientSlider1.RightHandle.Position; }
            set { gradientSlider1.RightHandle.Position = value; }
        }
    
        /// <summary>
        /// Gets or sets the minimum color
        /// </summary>
        [Description("Gets or sets the color associated with the minimum side of the gradient slider.")]
        public Color MinimumColor
        {
            get { return gradientSlider1.MinimumColor; }
            set 
            { 
                gradientSlider1.MinimumColor = value;
                leverMinimum.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum color
        /// </summary>
        [Description("Gets or sets the color associated with the maximum side of the gradient slider.")]
        public Color MaximumColor
        {
            get { return gradientSlider1.MaximumColor; }
            set 
            { 
                gradientSlider1.MaximumColor = value;
                leverMaximum.Color = value;
            }

        }

        /// <summary>
        /// Gets or sets a boolean that determine whether the sliders are enabled on the gradient control.
        /// Disabling the sliders will prevent them from being drawn or changed, and will automatically
        /// set the values to minimum and maximumum.
        /// </summary>
        [Description("Gets or sets a boolean that determine whether the sliders are enabled on the gradient control. Disabling the sliders will prevent them from being drawn or changed, and will automatically set the values to minimum and maximumum.")]
        public bool SlidersEnabled
        {
            get { return gradientSlider1.Enabled; }
            set 
            {
                if (value == false)
                {
                    gradientSlider1.LeftHandle.Position = gradientSlider1.Minimum;
                    gradientSlider1.RightHandle.Position = gradientSlider1.Maximum;
                }
                gradientSlider1.Enabled = value;
                Invalidate();
            }
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
        /// Fires the Gradient Changed event
        /// </summary>
        protected virtual void OnGradientChanged()
        {
            if (GradientChanged != null) GradientChanged(this, new EventArgs());
        }

        /// <summary>
        /// Fires the Gradient Changing event
        /// </summary>
        protected virtual void OnGradientChanging()
        {
            if (GradientChanging != null) GradientChanging(this, new EventArgs());
        }


        #endregion

        private void leverMinimum_ColorChanging(object sender, EventArgs e)
        {
            gradientSlider1.MinimumColor = leverMinimum.Color;
            OnGradientChanging();
        }

        private void leverMaximum_ColorChanging(object sender, EventArgs e)
        {
            gradientSlider1.MaximumColor = leverMaximum.Color;
            OnGradientChanging();
        }

        void leverMaximum_ColorChanged(object sender, EventArgs e)
        {
            gradientSlider1.MaximumColor = leverMaximum.Color;
            OnGradientChanged();
        }

        void leverMinimum_ColorChanged(object sender, EventArgs e)
        {
            gradientSlider1.MinimumColor = leverMinimum.Color;
            OnGradientChanged();
        }

        void gradientSlider1_PositionChanging(object sender, EventArgs e)
        {
            OnGradientChanging();
        }

        void gradientSlider1_PositionChanged(object sender, EventArgs e)
        {
            OnGradientChanged();
        }

       

    }
}
