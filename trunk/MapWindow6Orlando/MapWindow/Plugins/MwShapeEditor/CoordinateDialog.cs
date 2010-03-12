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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/11/2009 9:24:49 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.ComponentModel;
using System.Windows.Forms;
using MapWindow.Components;
using MapWindow.Geometries;
namespace MapWindow.ShapeEditor
{
    /// <summary>
    /// CoordinateDialog
    /// </summary>
    public class CoordinateDialog : Form
    {

        /// <summary>
        /// Occurs when the ok button is clicked
        /// </summary>
        public event EventHandler CoordinateAdded;
        private Button btnOk;
        private Button btnClose;
        private bool _showM;
        private bool _showZ;
        private ToolTip ttHelp;
        private DoubleBox dbxX;
        private DoubleBox dbxY;
        private DoubleBox dbxZ;
        private DoubleBox dbxM;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordinateDialog));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.dbxM = new MapWindow.Components.DoubleBox();
            this.dbxZ = new MapWindow.Components.DoubleBox();
            this.dbxY = new MapWindow.Components.DoubleBox();
            this.dbxX = new MapWindow.Components.DoubleBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.AccessibleDescription = null;
            this.btnOk.AccessibleName = null;
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.BackgroundImage = null;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Font = null;
            this.btnOk.Name = "btnOk";
            this.ttHelp.SetToolTip(this.btnOk, resources.GetString("btnOk.ToolTip"));
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleDescription = null;
            this.btnClose.AccessibleName = null;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackgroundImage = null;
            this.btnClose.Font = null;
            this.btnClose.Name = "btnClose";
            this.ttHelp.SetToolTip(this.btnClose, resources.GetString("btnClose.ToolTip"));
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dbxM
            // 
            this.dbxM.AccessibleDescription = null;
            this.dbxM.AccessibleName = null;
            resources.ApplyResources(this.dbxM, "dbxM");
            this.dbxM.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxM.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxM.BackgroundImage = null;
            this.dbxM.Caption = "M:";
            this.dbxM.Font = null;
            this.dbxM.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxM.IsValid = true;
            this.dbxM.Name = "dbxM";
            this.dbxM.NumberFormat = null;
            this.dbxM.RegularHelp = "Enter a double precision floating point value.";
            this.ttHelp.SetToolTip(this.dbxM, resources.GetString("dbxM.ToolTip"));
            this.dbxM.Value = 0;
            this.dbxM.ValidChanged += new System.EventHandler(this.Coordinate_ValidChanged);
            // 
            // dbxZ
            // 
            this.dbxZ.AccessibleDescription = null;
            this.dbxZ.AccessibleName = null;
            resources.ApplyResources(this.dbxZ, "dbxZ");
            this.dbxZ.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxZ.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxZ.BackgroundImage = null;
            this.dbxZ.Caption = "Z:";
            this.dbxZ.Font = null;
            this.dbxZ.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxZ.IsValid = true;
            this.dbxZ.Name = "dbxZ";
            this.dbxZ.NumberFormat = null;
            this.dbxZ.RegularHelp = "Enter a double precision floating point value.";
            this.ttHelp.SetToolTip(this.dbxZ, resources.GetString("dbxZ.ToolTip"));
            this.dbxZ.Value = 0;
            // 
            // dbxY
            // 
            this.dbxY.AccessibleDescription = null;
            this.dbxY.AccessibleName = null;
            resources.ApplyResources(this.dbxY, "dbxY");
            this.dbxY.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxY.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxY.BackgroundImage = null;
            this.dbxY.Caption = "Y:";
            this.dbxY.Font = null;
            this.dbxY.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxY.IsValid = true;
            this.dbxY.Name = "dbxY";
            this.dbxY.NumberFormat = null;
            this.dbxY.RegularHelp = "Enter a double precision floating point value.";
            this.ttHelp.SetToolTip(this.dbxY, resources.GetString("dbxY.ToolTip"));
            this.dbxY.Value = 0;
            this.dbxY.ValidChanged += new System.EventHandler(this.Coordinate_ValidChanged);
            // 
            // dbxX
            // 
            this.dbxX.AccessibleDescription = null;
            this.dbxX.AccessibleName = null;
            resources.ApplyResources(this.dbxX, "dbxX");
            this.dbxX.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxX.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxX.BackgroundImage = null;
            this.dbxX.Caption = "X:";
            this.dbxX.Font = null;
            this.dbxX.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxX.IsValid = true;
            this.dbxX.Name = "dbxX";
            this.dbxX.NumberFormat = null;
            this.dbxX.RegularHelp = "Enter a double precision floating point value.";
            this.ttHelp.SetToolTip(this.dbxX, resources.GetString("dbxX.ToolTip"));
            this.dbxX.Value = 0;
            this.dbxX.ValidChanged += new System.EventHandler(this.Coordinate_ValidChanged);
            // 
            // CoordinateDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.dbxM);
            this.Controls.Add(this.dbxZ);
            this.Controls.Add(this.dbxY);
            this.Controls.Add(this.dbxX);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoordinateDialog";
            this.ShowIcon = false;
            this.ttHelp.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.TopMost = true;
            this.ResumeLayout(false);

        }

       

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CoordinateDialog
        /// </summary>
        public CoordinateDialog()
        {
            InitializeComponent();
            _showM = true;
            _showZ = true;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets a coordinate based on the current values.
        /// </summary>
        public Coordinate Coordinate
        {
            get
            {
                Coordinate c;
                if (_showZ)
                {
                    c = new Coordinate(dbxX.Value, dbxY.Value, dbxZ.Value);
                }
                else
                {
                    c = new Coordinate(dbxX.Value, dbxY.Value);
                }
                if (_showM)
                {
                    c.M = dbxM.Value;
                }
                return c;
            }
        }

        /// <summary>
        /// Gets or sets whether or not to show M values
        /// </summary>
        public bool ShowMValues
        {
            get { return _showM; }
            set
            {
                if (_showM != value)
                {
                    if (value == false)
                    {
                        dbxM.Visible = false;
                        Height -= 20;
                    }
                    else
                    {
                        dbxM.Visible = true;
                        Height += 20;
                    }
                }
                _showM = value;
            }
        }

        /// <summary>
        /// Gets or sets whether or not to show Z values
        /// </summary>
        public bool ShowZValues
        {
            get { return _showZ; }
            set
            {
                if (_showZ != value)
                {
                    if (value == false)
                    {
                        dbxZ.Visible = false;
                        dbxM.Top -= 20;
                        Height -= 20;
                    }
                    else
                    {
                        dbxZ.Visible = true;
                        dbxM.Top += 20;
                        Height += 20;
                    }
                }
                _showZ = value;
            }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers
        private void Coordinate_ValidChanged(object sender, EventArgs e)
        {
            UpdateOk();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// prevents disposing this form when the user closes it.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
            base.OnClosing(e);
        }

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

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            if (CoordinateAdded != null) CoordinateAdded(this, new EventArgs());
            Hide();
        }
        /// <summary>
        /// Gets or sets the X value
        /// </summary>
        public double X
        {
            get { return dbxX.Value; }
            set { dbxX.Text = value.ToString(); }
        }
        /// <summary>
        /// Gets or sets the Y value
        /// </summary>
        public double Y
        {
            get { return dbxY.Value; }
            set { dbxY.Text = value.ToString(); }
        }
        /// <summary>
        /// Gets or sets the Z value
        /// </summary>
        public double Z
        {
            get { return dbxZ.Value; }
            set { dbxZ.Text = value.ToString(); }
        }
        /// <summary>
        /// Gets or sets the M vlaue
        /// </summary>
        public double M
        {
            get { return dbxM.Value; }
            set { dbxM.Text = value.ToString(); }
        }

        

        private void UpdateOk()
        {
            bool isValid = true;
            if (dbxX.IsValid == false) isValid = false;
            if (dbxY.IsValid == false) isValid = false;
            if (_showZ)
            {
                if (dbxZ.IsValid == false) isValid = false;
            }
            if (_showM)
            {
                if (dbxM.IsValid == false) isValid = false;
            }
            if (isValid == false)
            {
                btnOk.Enabled = false;
            }
            else
            {
                btnOk.Enabled = true;
            }

        }

    
       

        

     
    }
}