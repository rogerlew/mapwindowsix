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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/5/2009 2:06:00 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Drawing;
using MapWindow.Forms;


namespace MapWindow.Components
{


    /// <summary>
    /// PointSizeRangeControl
    /// </summary>
    [DefaultEvent("SizeRangeChanged"),
    ToolboxBitmap(typeof(FeatureSizeRangeControl), "UserControl.ico")]
    public class FeatureSizeRangeControl : UserControl
    {
        /// <summary>
        /// Occurs when either the sizes or the template has changed.
        /// </summary>
        public event EventHandler<SizeRangeEventArgs> SizeRangeChanged;


        private NumericUpDown nudStart;
        private NumericUpDown nudEnd;
        private Label label1;
        private GroupBox grpSizeRange;
        private Label label2;
        private CheckBox chkSizeRange;
        private Button btnEdit;
        #region Private Variables

        private IContainer components;
        private TrackBar trkEnd;
        private TrackBar trkStart;
        private IFeatureScheme _scheme;

        private PointSymbolView psvEnd;
        private PointSymbolView psvStart;

        private LineSymbolView lsvEnd;
        private LineSymbolView lsvStart;

        private FeatureSizeRange _sizeRange;
        private bool _ignore;
        private DetailedPointSymbolDialog _pointDialog;
        private DetailedLineSymbolDialog _lineDialog;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PointSizeRangeControl
        /// </summary>
        public FeatureSizeRangeControl()
        {
            InitializeComponent();
            _pointDialog = new DetailedPointSymbolDialog();
            _pointDialog.ChangesApplied += new System.EventHandler(_pointDialog_ChangesApplied);
            _lineDialog = new DetailedLineSymbolDialog();
            _lineDialog.ChangesApplied += new EventHandler(_lineDialog_ChangesApplied);
        }

        void _lineDialog_ChangesApplied(object sender, EventArgs e)
        {
            if (_sizeRange == null) return;
            _sizeRange.Symbolizer = _lineDialog.Symbolizer;
            UpdateControls();
        }

        void _pointDialog_ChangesApplied(object sender, System.EventArgs e)
        {
            if (_sizeRange == null) return;
            _sizeRange.Symbolizer = _pointDialog.Symbolizer;
            UpdateControls();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        /// <summary>
        /// Gets or sets the point scheme to work with.
        /// </summary>
        public IFeatureScheme Scheme
        {
            get { return _scheme; }
            set
            {
                _scheme = value;
                IPointScheme ps = _scheme as IPointScheme;
                if(ps != null)
                {
                    if (ps.Categories.Count > 0)
                    {
                        _sizeRange.Symbolizer = ps.Categories[0].Symbolizer;
                    }
                }
                ILineScheme ls = _scheme as ILineScheme;
                if(ls != null)
                {
                    if(ls.Categories.Count > 0)
                    {
                        _sizeRange.Symbolizer = ls.Categories[0].Symbolizer;
                    }
                }
                
                
            
                UpdateControls();
            }
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Handles the dispose situation.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (components != null && disposing)
            {
                foreach (Control control in components.Components)
                {
                    control.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
       

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSizeRangeControl));
            this.nudStart = new System.Windows.Forms.NumericUpDown();
            this.nudEnd = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.grpSizeRange = new System.Windows.Forms.GroupBox();
            this.lsvStart = new MapWindow.Components.LineSymbolView();
            this.lsvEnd = new MapWindow.Components.LineSymbolView();
            this.trkEnd = new System.Windows.Forms.TrackBar();
            this.btnEdit = new System.Windows.Forms.Button();
            this.trkStart = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.chkSizeRange = new System.Windows.Forms.CheckBox();
            this.psvStart = new MapWindow.Components.PointSymbolView();
            this.psvEnd = new MapWindow.Components.PointSymbolView();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).BeginInit();
            this.grpSizeRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkStart)).BeginInit();
            this.SuspendLayout();
            // 
            // nudStart
            // 
            this.nudStart.AccessibleDescription = null;
            this.nudStart.AccessibleName = null;
            resources.ApplyResources(this.nudStart, "nudStart");
            this.nudStart.Font = null;
            this.nudStart.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nudStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStart.Name = "nudStart";
            this.nudStart.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudStart.ValueChanged += new System.EventHandler(this.nudStart_ValueChanged);
            // 
            // nudEnd
            // 
            this.nudEnd.AccessibleDescription = null;
            this.nudEnd.AccessibleName = null;
            resources.ApplyResources(this.nudEnd, "nudEnd");
            this.nudEnd.Font = null;
            this.nudEnd.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nudEnd.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEnd.Name = "nudEnd";
            this.nudEnd.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudEnd.ValueChanged += new System.EventHandler(this.nudEnd_ValueChanged);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // grpSizeRange
            // 
            this.grpSizeRange.AccessibleDescription = null;
            this.grpSizeRange.AccessibleName = null;
            resources.ApplyResources(this.grpSizeRange, "grpSizeRange");
            this.grpSizeRange.BackgroundImage = null;
            this.grpSizeRange.Controls.Add(this.lsvStart);
            this.grpSizeRange.Controls.Add(this.lsvEnd);
            this.grpSizeRange.Controls.Add(this.trkEnd);
            this.grpSizeRange.Controls.Add(this.btnEdit);
            this.grpSizeRange.Controls.Add(this.trkStart);
            this.grpSizeRange.Controls.Add(this.label2);
            this.grpSizeRange.Controls.Add(this.chkSizeRange);
            this.grpSizeRange.Controls.Add(this.nudStart);
            this.grpSizeRange.Controls.Add(this.nudEnd);
            this.grpSizeRange.Controls.Add(this.label1);
            this.grpSizeRange.Controls.Add(this.psvStart);
            this.grpSizeRange.Controls.Add(this.psvEnd);
            this.grpSizeRange.Font = null;
            this.grpSizeRange.Name = "grpSizeRange";
            this.grpSizeRange.TabStop = false;
            // 
            // lsvStart
            // 
            this.lsvStart.AccessibleDescription = null;
            this.lsvStart.AccessibleName = null;
            resources.ApplyResources(this.lsvStart, "lsvStart");
            this.lsvStart.BackColor = System.Drawing.Color.White;
            this.lsvStart.BackgroundImage = null;
            this.lsvStart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lsvStart.Font = null;
            this.lsvStart.Name = "lsvStart";
            // 
            // lsvEnd
            // 
            this.lsvEnd.AccessibleDescription = null;
            this.lsvEnd.AccessibleName = null;
            resources.ApplyResources(this.lsvEnd, "lsvEnd");
            this.lsvEnd.BackColor = System.Drawing.Color.White;
            this.lsvEnd.BackgroundImage = null;
            this.lsvEnd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lsvEnd.Font = null;
            this.lsvEnd.Name = "lsvEnd";
            // 
            // trkEnd
            // 
            this.trkEnd.AccessibleDescription = null;
            this.trkEnd.AccessibleName = null;
            resources.ApplyResources(this.trkEnd, "trkEnd");
            this.trkEnd.BackgroundImage = null;
            this.trkEnd.Font = null;
            this.trkEnd.Maximum = 128;
            this.trkEnd.Minimum = 1;
            this.trkEnd.Name = "trkEnd";
            this.trkEnd.TickFrequency = 16;
            this.trkEnd.Value = 20;
            this.trkEnd.Scroll += new System.EventHandler(this.trkEnd_Scroll);
            // 
            // btnEdit
            // 
            this.btnEdit.AccessibleDescription = null;
            this.btnEdit.AccessibleName = null;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.BackgroundImage = null;
            this.btnEdit.Font = null;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // trkStart
            // 
            this.trkStart.AccessibleDescription = null;
            this.trkStart.AccessibleName = null;
            resources.ApplyResources(this.trkStart, "trkStart");
            this.trkStart.BackgroundImage = null;
            this.trkStart.Font = null;
            this.trkStart.Maximum = 128;
            this.trkStart.Minimum = 1;
            this.trkStart.Name = "trkStart";
            this.trkStart.TickFrequency = 16;
            this.trkStart.Value = 5;
            this.trkStart.Scroll += new System.EventHandler(this.trkStart_Scroll);
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // chkSizeRange
            // 
            this.chkSizeRange.AccessibleDescription = null;
            this.chkSizeRange.AccessibleName = null;
            resources.ApplyResources(this.chkSizeRange, "chkSizeRange");
            this.chkSizeRange.BackgroundImage = null;
            this.chkSizeRange.Font = null;
            this.chkSizeRange.Name = "chkSizeRange";
            this.chkSizeRange.UseVisualStyleBackColor = true;
            this.chkSizeRange.CheckedChanged += new System.EventHandler(this.chkSizeRange_CheckedChanged);
            // 
            // psvStart
            // 
            this.psvStart.AccessibleDescription = null;
            this.psvStart.AccessibleName = null;
            resources.ApplyResources(this.psvStart, "psvStart");
            this.psvStart.BackColor = System.Drawing.Color.White;
            this.psvStart.BackgroundImage = null;
            this.psvStart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.psvStart.Font = null;
            this.psvStart.Name = "psvStart";
            // 
            // psvEnd
            // 
            this.psvEnd.AccessibleDescription = null;
            this.psvEnd.AccessibleName = null;
            resources.ApplyResources(this.psvEnd, "psvEnd");
            this.psvEnd.BackColor = System.Drawing.Color.White;
            this.psvEnd.BackgroundImage = null;
            this.psvEnd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.psvEnd.Font = null;
            this.psvEnd.Name = "psvEnd";
            // 
            // FeatureSizeRangeControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.grpSizeRange);
            this.Font = null;
            this.Name = "FeatureSizeRangeControl";
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).EndInit();
            this.grpSizeRange.ResumeLayout(false);
            this.grpSizeRange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkStart)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Initializes this point size range control
        /// </summary>
        /// <param name="args"></param>
        public void Initialize(SizeRangeEventArgs args)
        {
            if (_sizeRange == null) return;
            _sizeRange.Start = args.StartSize;
            _sizeRange.End = args.EndSize;
            _sizeRange.Symbolizer = args.Template;
            _sizeRange.UseSizeRange = args.UseSizeRange;
            IPointSymbolizer ps = args.Template as IPointSymbolizer;
            if(ps != null)
            {
                psvStart.Visible = true;
                psvEnd.Visible = true;
                lsvStart.Visible = false;
                lsvEnd.Visible = false;
            }
            ILineSymbolizer ls = args.Template as ILineSymbolizer;
            if(ls != null)
            {
                lsvStart.Visible = true;
                lsvEnd.Visible = true;
                psvStart.Visible = false;
                psvEnd.Visible = false;
            }
        }

        /// <summary>
        /// Gets or sets the point Size Range, which controls the symbolizer, 
        /// as well as allowing the creation of a dynamically sized version
        /// of the symbolizer.
        /// </summary>
        public FeatureSizeRange SizeRange
        {
            get { return _sizeRange; }
            set
            {
                _sizeRange = value;
                UpdateControls();
            }
        }

       

       

        private void nudStart_ValueChanged(object sender, System.EventArgs e)
        {
           
            if (_sizeRange == null) return;
            _sizeRange.Start = (int)nudStart.Value;
            UpdateControls();
        }

        /// <summary>
        /// Handles the inter-connectivity of the various controls and updates
        /// them all to match the latest value.
        /// </summary>
        public void UpdateControls()
        {
            if (_ignore) return;
            _ignore = true;
            
            if (_sizeRange == null)
            {
                _ignore = false;
                return;
            }
            chkSizeRange.Checked = _sizeRange.UseSizeRange; 
            trkStart.Value = (int)_sizeRange.Start;
            trkEnd.Value = (int) _sizeRange.End;
            IPointSymbolizer ps = _sizeRange.Symbolizer as IPointSymbolizer;
            if(ps != null)
            {
                Color color = ps.GetFillColor();
                if (_scheme != null && _scheme.EditorSettings.UseColorRange)
                {
                    color = _scheme.EditorSettings.StartColor;
                }
                if (chkSizeRange.Checked)
                {
                    psvStart.Symbolizer = _sizeRange.GetSymbolizer(_sizeRange.Start, color) as IPointSymbolizer;
                }
                else
                {
                    IPointSymbolizer sm = ps.Copy();
                    sm.SetFillColor(color);
                    psvStart.Symbolizer = sm;
                }
                if (_scheme != null && _scheme.EditorSettings.UseColorRange)
                {
                    color = _scheme.EditorSettings.EndColor;
                }
                if (chkSizeRange.Checked)
                {
                    psvEnd.Symbolizer = _sizeRange.GetSymbolizer(_sizeRange.End, color) as IPointSymbolizer;
                }
                else
                {
                    IPointSymbolizer sm = ps.Copy();
                    sm.SetFillColor(color);
                    psvEnd.Symbolizer = sm;
                }
            }

            ILineSymbolizer ls = _sizeRange.Symbolizer as ILineSymbolizer;
            if (ls != null)
            {
                Color color = ls.GetFillColor();
                if (_scheme != null && _scheme.EditorSettings.UseColorRange)
                {
                    color = _scheme.EditorSettings.StartColor;
                }
                if (chkSizeRange.Checked)
                {
                    lsvStart.Symbolizer = _sizeRange.GetSymbolizer(_sizeRange.Start, color) as ILineSymbolizer;
                }
                else
                {
                    ILineSymbolizer sm = ls.Copy();
                    sm.SetFillColor(color);
                    lsvStart.Symbolizer = sm;
                }
                if (_scheme != null && _scheme.EditorSettings.UseColorRange)
                {
                    color = _scheme.EditorSettings.EndColor;
                }
                if (chkSizeRange.Checked)
                {
                    lsvEnd.Symbolizer = _sizeRange.GetSymbolizer(_sizeRange.End, color) as ILineSymbolizer;
                }
                else
                {
                    ILineSymbolizer sm = ls.Copy();
                    sm.SetFillColor(color);
                    lsvEnd.Symbolizer = sm;
                }
            }
            
           
            nudStart.Value = (decimal)_sizeRange.Start;
            nudEnd.Value = (decimal)_sizeRange.End;
           
            _ignore = false;
            
            OnSizeRangeChanged();
        }

        private void nudEnd_ValueChanged(object sender, System.EventArgs e)
        {
            if (_sizeRange == null) return;
            _sizeRange.End = (int) nudEnd.Value;
            UpdateControls();
        }

        private void trkEnd_Scroll(object sender, System.EventArgs e)
        {
            if (_sizeRange == null) return;
            _sizeRange.End = trkEnd.Value;
            UpdateControls();
        }

        private void trkStart_Scroll(object sender, System.EventArgs e)
        {
            if (_sizeRange == null) return;
            _sizeRange.Start = trkStart.Value;
            UpdateControls();
        }

        private void chkSizeRange_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_sizeRange == null) return;
            _sizeRange.UseSizeRange = chkSizeRange.Checked;
            UpdateControls();
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (_sizeRange == null) return;
            IPointSymbolizer ps = _sizeRange.Symbolizer as IPointSymbolizer;
            if(ps != null)
            {
                _pointDialog.Symbolizer = _sizeRange.Symbolizer as IPointSymbolizer;
                _pointDialog.ShowDialog(this);
            }
            ILineSymbolizer ls = _sizeRange.Symbolizer as ILineSymbolizer;
            if(ls != null)
            {
                _lineDialog.Symbolizer = _sizeRange.Symbolizer as ILineSymbolizer;
                _lineDialog.ShowDialog(this);
            }
        }

    
        /// <summary>
        /// Fires the SizeRangeChanged event args
        /// </summary>
        protected virtual void OnSizeRangeChanged()
        {
            if (SizeRangeChanged != null) SizeRangeChanged(this, new SizeRangeEventArgs(_sizeRange));
        }
       
    }
}
