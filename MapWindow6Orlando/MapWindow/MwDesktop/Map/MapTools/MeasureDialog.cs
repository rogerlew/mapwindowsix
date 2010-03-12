//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core assembly for the MapWindow 6.0 distribution.
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/19/2009 11:03:57 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Windows.Forms;


namespace MapWindow.Map
{
    /// <summary>
    /// MeasureDialog
    /// </summary>
    public class MeasureDialog : Form
    {
        /// <summary>
        /// Occurs when the measuring mode has been changed.
        /// </summary>
        public event EventHandler MeasureModeChanged;
      

        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem distanceToolStripMenuItem;
        private ToolStripMenuItem kilometersToolStripMenuItem;
        private ToolStripMenuItem metersToolStripMenuItem;
        private ToolStripMenuItem decimetersToolStripMenuItem;
        private ToolStripMenuItem centimetersToolStripMenuItem;
        private ToolStripMenuItem milimetersToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem milesToolStripMenuItem;
        private ToolStripMenuItem nauticalMilesToolStripMenuItem;
        private ToolStripMenuItem yardsToolStripMenuItem;
        private ToolStripMenuItem feetToolStripMenuItem;
        private ToolStripMenuItem inchesToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem decimalDegreesToolStripMenuItem;
        private ToolStripMenuItem areaToolStripMenuItem;
        private ToolTip ttHelp;
        private ToolStripMenuItem kilometersToolStripMenuItem1;
        private ToolStripMenuItem hectaresToolStripMenuItem;
        private ToolStripMenuItem metersToolStripMenuItem1;
        private ToolStripMenuItem areaToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem milesToolStripMenuItem1;
        private ToolStripMenuItem acresToolStripMenuItem;
        private ToolStripMenuItem feetToolStripMenuItem1;
        private ToolStripMenuItem yardsToolStripMenuItem1;
        private Label label1;
        private Label lblMeasure;
        private Label lblPartialValue;
        private Label lblTotalValue;

        private double _distFromMeters = 1;
        private double _areaFromMeters = 1;
        private ToolStripMenuItem _currentDist;
        private ToolStripMenuItem _currentArea;
        private double _distance;
        private double _area;
        private double _totalDistance;
        private double _totalArea;
        private MeasureModes _measureMode;
        private bool _angularDistance;
        private string _distUnits;
        private string _areaUnits;
        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #endregion


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasureDialog));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.distanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kilometersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimetersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centimetersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.milimetersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.milesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nauticalMilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.feetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.decimalDegreesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.areaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kilometersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hectaresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.areaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.milesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.acresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.feetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.yardsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lblMeasure = new System.Windows.Forms.Label();
            this.lblPartialValue = new System.Windows.Forms.Label();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = null;
            this.toolStrip1.AccessibleName = null;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackgroundImage = null;
            this.toolStrip1.Font = null;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripDropDownButton1});
            this.toolStrip1.Name = "toolStrip1";
            this.ttHelp.SetToolTip(this.toolStrip1, resources.GetString("toolStrip1.ToolTip"));
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AccessibleDescription = null;
            this.toolStripButton1.AccessibleName = null;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.BackgroundImage = null;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::MapWindow.Images.Line;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.AccessibleDescription = null;
            this.toolStripButton2.AccessibleName = null;
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.BackgroundImage = null;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::MapWindow.Images.Area;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AccessibleDescription = null;
            this.toolStripDropDownButton1.AccessibleName = null;
            resources.ApplyResources(this.toolStripDropDownButton1, "toolStripDropDownButton1");
            this.toolStripDropDownButton1.BackgroundImage = null;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.distanceToolStripMenuItem,
            this.areaToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::MapWindow.Images.ScaleBar;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            // 
            // distanceToolStripMenuItem
            // 
            this.distanceToolStripMenuItem.AccessibleDescription = null;
            this.distanceToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.distanceToolStripMenuItem, "distanceToolStripMenuItem");
            this.distanceToolStripMenuItem.BackgroundImage = null;
            this.distanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kilometersToolStripMenuItem,
            this.metersToolStripMenuItem,
            this.decimetersToolStripMenuItem,
            this.centimetersToolStripMenuItem,
            this.milimetersToolStripMenuItem,
            this.toolStripMenuItem1,
            this.milesToolStripMenuItem,
            this.nauticalMilesToolStripMenuItem,
            this.yardsToolStripMenuItem,
            this.feetToolStripMenuItem,
            this.inchesToolStripMenuItem,
            this.toolStripMenuItem2,
            this.decimalDegreesToolStripMenuItem});
            this.distanceToolStripMenuItem.Name = "distanceToolStripMenuItem";
            this.distanceToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // kilometersToolStripMenuItem
            // 
            this.kilometersToolStripMenuItem.AccessibleDescription = null;
            this.kilometersToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.kilometersToolStripMenuItem, "kilometersToolStripMenuItem");
            this.kilometersToolStripMenuItem.BackgroundImage = null;
            this.kilometersToolStripMenuItem.Name = "kilometersToolStripMenuItem";
            this.kilometersToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.kilometersToolStripMenuItem.Click += new System.EventHandler(this.kilometersToolStripMenuItem_Click);
            // 
            // metersToolStripMenuItem
            // 
            this.metersToolStripMenuItem.AccessibleDescription = null;
            this.metersToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.metersToolStripMenuItem, "metersToolStripMenuItem");
            this.metersToolStripMenuItem.BackgroundImage = null;
            this.metersToolStripMenuItem.Checked = true;
            this.metersToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.metersToolStripMenuItem.Name = "metersToolStripMenuItem";
            this.metersToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.metersToolStripMenuItem.Click += new System.EventHandler(this.metersToolStripMenuItem_Click);
            // 
            // decimetersToolStripMenuItem
            // 
            this.decimetersToolStripMenuItem.AccessibleDescription = null;
            this.decimetersToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.decimetersToolStripMenuItem, "decimetersToolStripMenuItem");
            this.decimetersToolStripMenuItem.BackgroundImage = null;
            this.decimetersToolStripMenuItem.Name = "decimetersToolStripMenuItem";
            this.decimetersToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.decimetersToolStripMenuItem.Click += new System.EventHandler(this.decimetersToolStripMenuItem_Click);
            // 
            // centimetersToolStripMenuItem
            // 
            this.centimetersToolStripMenuItem.AccessibleDescription = null;
            this.centimetersToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.centimetersToolStripMenuItem, "centimetersToolStripMenuItem");
            this.centimetersToolStripMenuItem.BackgroundImage = null;
            this.centimetersToolStripMenuItem.Name = "centimetersToolStripMenuItem";
            this.centimetersToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.centimetersToolStripMenuItem.Click += new System.EventHandler(this.centimetersToolStripMenuItem_Click);
            // 
            // milimetersToolStripMenuItem
            // 
            this.milimetersToolStripMenuItem.AccessibleDescription = null;
            this.milimetersToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.milimetersToolStripMenuItem, "milimetersToolStripMenuItem");
            this.milimetersToolStripMenuItem.BackgroundImage = null;
            this.milimetersToolStripMenuItem.Name = "milimetersToolStripMenuItem";
            this.milimetersToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.milimetersToolStripMenuItem.Click += new System.EventHandler(this.milimetersToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.AccessibleDescription = null;
            this.toolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // milesToolStripMenuItem
            // 
            this.milesToolStripMenuItem.AccessibleDescription = null;
            this.milesToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.milesToolStripMenuItem, "milesToolStripMenuItem");
            this.milesToolStripMenuItem.BackgroundImage = null;
            this.milesToolStripMenuItem.Name = "milesToolStripMenuItem";
            this.milesToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.milesToolStripMenuItem.Click += new System.EventHandler(this.milesToolStripMenuItem_Click);
            // 
            // nauticalMilesToolStripMenuItem
            // 
            this.nauticalMilesToolStripMenuItem.AccessibleDescription = null;
            this.nauticalMilesToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.nauticalMilesToolStripMenuItem, "nauticalMilesToolStripMenuItem");
            this.nauticalMilesToolStripMenuItem.BackgroundImage = null;
            this.nauticalMilesToolStripMenuItem.Name = "nauticalMilesToolStripMenuItem";
            this.nauticalMilesToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.nauticalMilesToolStripMenuItem.Click += new System.EventHandler(this.nauticalMilesToolStripMenuItem_Click);
            // 
            // yardsToolStripMenuItem
            // 
            this.yardsToolStripMenuItem.AccessibleDescription = null;
            this.yardsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.yardsToolStripMenuItem, "yardsToolStripMenuItem");
            this.yardsToolStripMenuItem.BackgroundImage = null;
            this.yardsToolStripMenuItem.Name = "yardsToolStripMenuItem";
            this.yardsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.yardsToolStripMenuItem.Click += new System.EventHandler(this.yardsToolStripMenuItem_Click);
            // 
            // feetToolStripMenuItem
            // 
            this.feetToolStripMenuItem.AccessibleDescription = null;
            this.feetToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.feetToolStripMenuItem, "feetToolStripMenuItem");
            this.feetToolStripMenuItem.BackgroundImage = null;
            this.feetToolStripMenuItem.Name = "feetToolStripMenuItem";
            this.feetToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.feetToolStripMenuItem.Click += new System.EventHandler(this.feetToolStripMenuItem_Click);
            // 
            // inchesToolStripMenuItem
            // 
            this.inchesToolStripMenuItem.AccessibleDescription = null;
            this.inchesToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.inchesToolStripMenuItem, "inchesToolStripMenuItem");
            this.inchesToolStripMenuItem.BackgroundImage = null;
            this.inchesToolStripMenuItem.Name = "inchesToolStripMenuItem";
            this.inchesToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.inchesToolStripMenuItem.Click += new System.EventHandler(this.inchesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.AccessibleDescription = null;
            this.toolStripMenuItem2.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // decimalDegreesToolStripMenuItem
            // 
            this.decimalDegreesToolStripMenuItem.AccessibleDescription = null;
            this.decimalDegreesToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.decimalDegreesToolStripMenuItem, "decimalDegreesToolStripMenuItem");
            this.decimalDegreesToolStripMenuItem.BackgroundImage = null;
            this.decimalDegreesToolStripMenuItem.Name = "decimalDegreesToolStripMenuItem";
            this.decimalDegreesToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.decimalDegreesToolStripMenuItem.Click += new System.EventHandler(this.decimalDegreesToolStripMenuItem_Click);
            // 
            // areaToolStripMenuItem
            // 
            this.areaToolStripMenuItem.AccessibleDescription = null;
            this.areaToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.areaToolStripMenuItem, "areaToolStripMenuItem");
            this.areaToolStripMenuItem.BackgroundImage = null;
            this.areaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kilometersToolStripMenuItem1,
            this.hectaresToolStripMenuItem,
            this.metersToolStripMenuItem1,
            this.areaToolStripMenuItem1,
            this.toolStripMenuItem3,
            this.milesToolStripMenuItem1,
            this.acresToolStripMenuItem,
            this.feetToolStripMenuItem1,
            this.yardsToolStripMenuItem1});
            this.areaToolStripMenuItem.Name = "areaToolStripMenuItem";
            this.areaToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // kilometersToolStripMenuItem1
            // 
            this.kilometersToolStripMenuItem1.AccessibleDescription = null;
            this.kilometersToolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.kilometersToolStripMenuItem1, "kilometersToolStripMenuItem1");
            this.kilometersToolStripMenuItem1.BackgroundImage = null;
            this.kilometersToolStripMenuItem1.Name = "kilometersToolStripMenuItem1";
            this.kilometersToolStripMenuItem1.ShortcutKeyDisplayString = null;
            this.kilometersToolStripMenuItem1.Click += new System.EventHandler(this.kilometersToolStripMenuItem1_Click);
            // 
            // hectaresToolStripMenuItem
            // 
            this.hectaresToolStripMenuItem.AccessibleDescription = null;
            this.hectaresToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.hectaresToolStripMenuItem, "hectaresToolStripMenuItem");
            this.hectaresToolStripMenuItem.BackgroundImage = null;
            this.hectaresToolStripMenuItem.Name = "hectaresToolStripMenuItem";
            this.hectaresToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.hectaresToolStripMenuItem.Click += new System.EventHandler(this.hectaresToolStripMenuItem_Click);
            // 
            // metersToolStripMenuItem1
            // 
            this.metersToolStripMenuItem1.AccessibleDescription = null;
            this.metersToolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.metersToolStripMenuItem1, "metersToolStripMenuItem1");
            this.metersToolStripMenuItem1.BackgroundImage = null;
            this.metersToolStripMenuItem1.Checked = true;
            this.metersToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.metersToolStripMenuItem1.Name = "metersToolStripMenuItem1";
            this.metersToolStripMenuItem1.ShortcutKeyDisplayString = null;
            this.metersToolStripMenuItem1.Click += new System.EventHandler(this.metersToolStripMenuItem1_Click);
            // 
            // areaToolStripMenuItem1
            // 
            this.areaToolStripMenuItem1.AccessibleDescription = null;
            this.areaToolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.areaToolStripMenuItem1, "areaToolStripMenuItem1");
            this.areaToolStripMenuItem1.BackgroundImage = null;
            this.areaToolStripMenuItem1.Name = "areaToolStripMenuItem1";
            this.areaToolStripMenuItem1.ShortcutKeyDisplayString = null;
            this.areaToolStripMenuItem1.Click += new System.EventHandler(this.areaToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.AccessibleDescription = null;
            this.toolStripMenuItem3.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            // 
            // milesToolStripMenuItem1
            // 
            this.milesToolStripMenuItem1.AccessibleDescription = null;
            this.milesToolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.milesToolStripMenuItem1, "milesToolStripMenuItem1");
            this.milesToolStripMenuItem1.BackgroundImage = null;
            this.milesToolStripMenuItem1.Name = "milesToolStripMenuItem1";
            this.milesToolStripMenuItem1.ShortcutKeyDisplayString = null;
            this.milesToolStripMenuItem1.Click += new System.EventHandler(this.milesToolStripMenuItem1_Click);
            // 
            // acresToolStripMenuItem
            // 
            this.acresToolStripMenuItem.AccessibleDescription = null;
            this.acresToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.acresToolStripMenuItem, "acresToolStripMenuItem");
            this.acresToolStripMenuItem.BackgroundImage = null;
            this.acresToolStripMenuItem.Name = "acresToolStripMenuItem";
            this.acresToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.acresToolStripMenuItem.Click += new System.EventHandler(this.acresToolStripMenuItem_Click);
            // 
            // feetToolStripMenuItem1
            // 
            this.feetToolStripMenuItem1.AccessibleDescription = null;
            this.feetToolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.feetToolStripMenuItem1, "feetToolStripMenuItem1");
            this.feetToolStripMenuItem1.BackgroundImage = null;
            this.feetToolStripMenuItem1.Name = "feetToolStripMenuItem1";
            this.feetToolStripMenuItem1.ShortcutKeyDisplayString = null;
            this.feetToolStripMenuItem1.Click += new System.EventHandler(this.feetToolStripMenuItem1_Click);
            // 
            // yardsToolStripMenuItem1
            // 
            this.yardsToolStripMenuItem1.AccessibleDescription = null;
            this.yardsToolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.yardsToolStripMenuItem1, "yardsToolStripMenuItem1");
            this.yardsToolStripMenuItem1.BackgroundImage = null;
            this.yardsToolStripMenuItem1.Name = "yardsToolStripMenuItem1";
            this.yardsToolStripMenuItem1.ShortcutKeyDisplayString = null;
            this.yardsToolStripMenuItem1.Click += new System.EventHandler(this.yardsToolStripMenuItem1_Click);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            this.ttHelp.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // lblMeasure
            // 
            this.lblMeasure.AccessibleDescription = null;
            this.lblMeasure.AccessibleName = null;
            resources.ApplyResources(this.lblMeasure, "lblMeasure");
            this.lblMeasure.Font = null;
            this.lblMeasure.Name = "lblMeasure";
            this.ttHelp.SetToolTip(this.lblMeasure, resources.GetString("lblMeasure.ToolTip"));
            // 
            // lblPartialValue
            // 
            this.lblPartialValue.AccessibleDescription = null;
            this.lblPartialValue.AccessibleName = null;
            resources.ApplyResources(this.lblPartialValue, "lblPartialValue");
            this.lblPartialValue.BackColor = System.Drawing.Color.White;
            this.lblPartialValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPartialValue.Font = null;
            this.lblPartialValue.Name = "lblPartialValue";
            this.ttHelp.SetToolTip(this.lblPartialValue, resources.GetString("lblPartialValue.ToolTip"));
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AccessibleDescription = null;
            this.lblTotalValue.AccessibleName = null;
            resources.ApplyResources(this.lblTotalValue, "lblTotalValue");
            this.lblTotalValue.BackColor = System.Drawing.Color.White;
            this.lblTotalValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTotalValue.Font = null;
            this.lblTotalValue.Name = "lblTotalValue";
            this.ttHelp.SetToolTip(this.lblTotalValue, resources.GetString("lblTotalValue.ToolTip"));
            // 
            // MeasureDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.lblTotalValue);
            this.Controls.Add(this.lblPartialValue);
            this.Controls.Add(this.lblMeasure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MeasureDialog";
            this.ShowIcon = false;
            this.ttHelp.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.TopMost = true;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of MeasureDialog
        /// </summary>
        public MeasureDialog()
        {
            InitializeComponent();
            _measureMode = MeasureModes.Distance;
            _currentDist = metersToolStripMenuItem;
            _currentArea = metersToolStripMenuItem1;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        /// <summary>
        /// Gets the distance in meters of just one segment
        /// </summary>
        public double Distance
        {
            get { return _distance; }
            set 
            { 
                _distance = value;
                if (_measureMode == MeasureModes.Distance)
                {
                    double d = _distance*_distFromMeters;
                    if(d < 1000)
                    {
                        lblPartialValue.Text = d + " " + _distUnits;
                    }
                    else
                    {
                        lblPartialValue.Text = d.ToString("#,###") + " " + _distUnits;
                    }
                    
                }
               
            }
        }

       
      


        /// <summary>
        /// The total distance across all segments in meters
        /// </summary>
        public double TotalDistance
        {
            get { return _totalDistance; }
            set
            {
                _totalDistance = value;
                if (_measureMode == MeasureModes.Distance)
                {
                    double d = _totalDistance*_distFromMeters;
                    if(d < 1000)
                    {
                        lblTotalValue.Text = d + " " + _distUnits;
                    }
                    else
                    {
                        lblTotalValue.Text = (d).ToString("#,###") + " " + _distUnits; 
                    }
                    
                }
             
            }
        }

        /// <summary>
        /// Gets the current area being calculated in square meters
        /// </summary>
        public double Area
        {
            get { return _area; }
            set
            {
                _area = value;
                if(_measureMode == MeasureModes.Area)
                {
                    double a = _area*_areaFromMeters;
                    if(a < 1000)
                    {
                        lblPartialValue.Text = a.ToString(); 
                    }
                    else
                    {
                        lblPartialValue.Text = (_area*_areaFromMeters).ToString("#,###") + " " + _areaUnits;
                    } 
                    
                }
            }
        }

        /// <summary>
        /// Gets or sets the total area in square meters
        /// </summary>
        public double TotalArea
        {
            get { return _totalArea;  }
            set
            {
                _totalArea = value;
                if (_measureMode == MeasureModes.Area)
                {
                    lblTotalValue.Text = (_totalArea * _areaFromMeters).ToString("#,###");
                }
            }
        }

       

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        #region Private Functions

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
        /// Gets or sets whether to display the distances or areas.
        /// </summary>
        public MeasureModes MeasureMode
        {
            get { return _measureMode; }
            set
            {
                _measureMode = value;
                lblMeasure.Text = _measureMode == MeasureModes.Distance ? "Distance:" : "Area:";
            }
        }

        #endregion

        private void UpdateDistance(ToolStripMenuItem newItem, double fromMeters)
        {
            if (_currentDist == newItem) return;
            if(_currentDist != null)_currentDist.Checked = false;
            _currentDist = newItem;
            newItem.Checked = true;
            _distFromMeters = fromMeters;
        }

        private void kilometersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Kilometers";
            UpdateDistance(kilometersToolStripMenuItem, .001);
        }
        private void metersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Meters";
            UpdateDistance(metersToolStripMenuItem, 1);
        }
        private void decimetersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Decimeters";
            UpdateDistance(decimetersToolStripMenuItem, 10);
        }
        private void centimetersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Centimeters";
            UpdateDistance(centimetersToolStripMenuItem, 100);
        }
        private void milimetersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Milimeters";
            UpdateDistance(milimetersToolStripMenuItem, 1000);
        }
        private void milesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Miles";
            UpdateDistance(milesToolStripMenuItem, 0.000621371192);
        }
        private void nauticalMilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Nautical Miles";
            UpdateDistance(nauticalMilesToolStripMenuItem, 0.000539956803);
        }
        private void yardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Yards";
            UpdateDistance(yardsToolStripMenuItem, 1.0936133);
        }
        private void feetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _angularDistance = false;
            _distUnits = "Feet";
            UpdateDistance(feetToolStripMenuItem, 3.2808399);
        }
        private void inchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Inches";
            UpdateDistance(inchesToolStripMenuItem, 39.3700787);
        }
        // This should maybe be improved later to take latitude into account horizontally.
        private void decimalDegreesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _distUnits = "Decimal Degrees";
            UpdateDistance(decimalDegreesToolStripMenuItem, 8.983152098E-6);
        }

        private void UpdateArea(ToolStripMenuItem newItem, double fromMeters)
        {
            if (_currentArea == newItem) return;
            if (_currentArea != null) _currentArea.Checked = false;
            _currentArea = newItem;
            newItem.Checked = true;
            _areaFromMeters = fromMeters;
        }

        private void kilometersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _areaUnits = "Square Kilometers";
            UpdateArea(kilometersToolStripMenuItem1, 1E-6);
        }

        private void hectaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _areaUnits = "Hectares";
            UpdateArea(hectaresToolStripMenuItem, 0.0001);
        }
        private void metersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _areaUnits = "Square Meters";
            UpdateArea(metersToolStripMenuItem1, 1);
        }
        private void areaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _areaUnits = "Ares";
            UpdateArea(metersToolStripMenuItem1, .01);
        }
        private void milesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _areaUnits = "Square Miles";
            UpdateArea(milesToolStripMenuItem1, 3.86102159E-7);
        }
        private void acresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _areaUnits = "Acres";
            UpdateArea(acresToolStripMenuItem, 0.000247105381);
        }
        private void feetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _areaUnits = "Feet";
            UpdateArea(feetToolStripMenuItem1, 10.7639104);
        }
        private void yardsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _areaUnits = "Yards";
            UpdateArea(yardsToolStripMenuItem1, 1.19599005);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if(_measureMode != MeasureModes.Area)
            {
                MeasureMode = MeasureModes.Area;
                OnMeasureModeChanged();
                
            }
            
        }



        private void OnMeasureModeChanged()
        {
            if(MeasureModeChanged != null)MeasureModeChanged(this, new EventArgs());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(_measureMode != MeasureModes.Distance)
            {
               MeasureMode = MeasureModes.Distance;
                OnMeasureModeChanged();
            }
        }


       
    }
}