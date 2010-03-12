//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2008 6:55:40 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Components;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Main;

namespace MapWindow.Forms
{
    /// <summary>
    /// A ColorBreakEditor form for rasters
    /// </summary>
    public class RasterSymbolizerDialog : Form
    {
        #region Private Variables

        private IRasterSymbolizer _rasterSymbolizer;
        private IRasterSymbolizer _backupCopy;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components;

        // Designer Variables
        private Button btnOK;
        private Button btnApply;
        private Panel pnlHeader;
        private Panel pnlFooter;
        private Button btnCancel;
        private Panel pnlColorBreakEditor;
        private ColorBreakViewer csvEditor;
        private ToolStrip tsbMinus;
        private ToolStripButton tsbOpen;
        private ToolStripButton tsbSave;
        private ToolStripButton tsbAdd;
        private ToolStripButton tsbRemove;
        private CheckBox chkComputeHillshade;
        private ToolStripSplitButton tsbWizard;
        private ToolStripMenuItem mnuColorRamp;
        private ToolStripMenuItem mnuEqualBreaks;
        private ToolStripMenuItem mnuUniqueValues;
        private ToolStripMenuItem mnuPredefined;
        private ComboBox cmbGradientStyle;
        private Label lblGradient;
        private Button btnClassify;
        private ComboBox cmbNumberFormat;
        private Button btnShadedRelief;
        private ToolStripMenuItem mnuDeadSea;
        private ToolStripMenuItem mnuDesert;
        private ToolStripMenuItem mnuFallLeaves;
        private ToolStripMenuItem mnuGlaciers;
        private ToolStripMenuItem mnuHighway;
        private ToolStripMenuItem mnuMeadow;
        private ToolStripMenuItem mnuSummerMountains;
        private ToolStripMenuItem mnuValleyFires;
        private Label lblDecimalCount;
        private TextBox txtDecimalCount;
        private ToolTip ttInfo;
        private mwStatusStrip mwStatusStrip1;
        private ToolStripStatusLabel statusText;
        private ToolStripProgressBar statusProgressBar;
        private TextBox txtOpacity;
        private Label lblOpacity;
        private CheckBox chkDrapeLayers;
        private CheckBox chkSmooth;
        private Label lblNumberFormat;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ColorbreakEditor form
        /// </summary>
        public RasterSymbolizerDialog()
        {
            _rasterSymbolizer = new RasterSymbolizer(); // a new symbolizer to hold values
            Configure();
        }


        /// <summary>
        /// Loads this form with an existing rasterSymbolizer
        /// </summary>
        /// <param name="rasterSymbolizer">A Raster Symbolizer</param>
        public RasterSymbolizerDialog(IRasterSymbolizer rasterSymbolizer)
        {
            _rasterSymbolizer = rasterSymbolizer;
            Configure();
            if ((int) _rasterSymbolizer.NumberFormat < 6)
                cmbNumberFormat.SelectedIndex = (int) _rasterSymbolizer.NumberFormat;
        }


       
        private void Configure()
        {
            _backupCopy = _rasterSymbolizer.Copy();
            InitializeComponent();
            csvEditor.IntializeControl(_rasterSymbolizer.ColorBreaks);
            csvEditor.LayerType = LayerTypes.Raster;
            txtDecimalCount.Text = "0";
            cmbNumberFormat.SelectedIndex = 4;
            cmbGradientStyle.SelectedIndex = 0;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the raster symbolizer that should be displayed in this form.
        /// </summary>
        public IRasterSymbolizer RasterSymbolizer
        {
            get { return _rasterSymbolizer; }
            set
            {
                _rasterSymbolizer = value;
                _backupCopy = _rasterSymbolizer.Copy();

                DisplaySymbolizerValues(); // adjust the form properties to match the new symbolizer
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when Changes have been applied to the colorscheme either through the ok button or 
        /// else by pressing the ApplyChanges button.
        /// </summary>
        public event EventHandler<RasterSymbolizerArgs> ChangesApplied;

        /// <summary>
        /// Fires the ChangesApplied event.
        /// </summary>
        public virtual void OnChangesApplied()
        {
            if (ChangesApplied != null)
                ChangesApplied(this, new RasterSymbolizerArgs(_rasterSymbolizer));
        }

        #endregion

        #region EventHandlers

        private void cmbNumberFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            _rasterSymbolizer.NumberFormat = (NumberFormats) cmbNumberFormat.SelectedIndex;
            csvEditor.Invalidate();
        }


        private void mnuColorRamp_Click(object sender, EventArgs e)
        {
            IRasterSymbolizer temp = _rasterSymbolizer.Copy();
            RampColorDialog rmpcolor = new RampColorDialog(temp);
            if (rmpcolor.ShowDialog(this) != DialogResult.OK) return;
            _rasterSymbolizer = temp;
            csvEditor.Invalidate();
        }

        private void mnuEqualBreaks_Click(object sender, EventArgs e)
        {
            string result;
            LogManager.DefaultLogManager.LogInputBox(this, "How many color breaks do you want?", "Enter a Value:",
                                                     ValidationTypes.PositiveInteger, out result);
            int count = Global.GetInteger(result);
            _rasterSymbolizer.CreateRandomBreaks(count);
            csvEditor.Invalidate();
            Focus();
        }


        private void mnuUniqueValues_Click(object sender, EventArgs e)
        {
            if (_rasterSymbolizer.Raster == null)
            {
                // log exception  
                LogManager.DefaultLogManager.LogMessageBox(ExceptionMessages.Raster_RasterUndefined,
                                                           "Invalid Opperation.");
                return;
            }
            int numRows = _rasterSymbolizer.Raster.NumRows;
            int numCols = _rasterSymbolizer.Raster.NumColumns;
            bool notified = false;

            // --------------------------- INTEGER -------------------------------------------
            if (_rasterSymbolizer.Raster.DataType == typeof(int))
            {
                List<int> intList = new List<int>();


                IntRaster intRaster = _rasterSymbolizer.Raster as IntRaster;
                if (intRaster == null)
                {
                    LogManager.DefaultLogManager.LogMessageBox(ExceptionMessages.Raster_RasterUndefined,
                                                               "Invalid Opperation.");
                    return;
                }
                int intNoData = intRaster.IntNoDataValue;
                for (int row = 0; row < numRows; row++)
                {
                    for (int col = 0; col < numCols; col++)
                    {
                        if (intList.Contains(intRaster.Data[row][col]) == false)
                        {
                            intList.Add(intRaster.Data[row][col]);
                            if (intList.Count > 100 && notified == false)
                            {
                                if (
                                    LogManager.DefaultLogManager.LogMessageBox(MessageStrings.MoreThan100UniqueValues,
                                                                               "Slow Process Alert",
                                                                               MessageBoxButtons.YesNo,
                                                                               MessageBoxIcon.Question) ==
                                    DialogResult.No)
                                    return;
                                notified = true;
                            }
                            if (intList.Count > 1000)
                            {
                                LogManager.DefaultLogManager.LogMessageBox(MessageStrings.TooManyUniqueValues,
                                                                           "Too many values.", MessageBoxButtons.OK,
                                                                           MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                }
                _rasterSymbolizer.ColorBreaks.Clear();
                foreach (int value in intList)
                {
                    if (value != intNoData)
                    {
                        IColorCategory cb = _rasterSymbolizer.AddColorBreak(value);
                        Color rand = Global.RandomTranslucent(_rasterSymbolizer.Opacity);
                        cb.LowColor = rand;
                        cb.HighColor = rand;
                    }
                }
            }
            // --------------------------- Float -------------------------------------------

            if (_rasterSymbolizer.Raster.DataType == typeof(float))
            {
                List<float> floatList = new List<float>();


                FloatRaster floatRaster = _rasterSymbolizer.Raster as FloatRaster;
                if (floatRaster == null)
                {
                    LogManager.DefaultLogManager.LogMessageBox(ExceptionMessages.Raster_RasterUndefined,
                                                               "Invalid Opperation.");
                    return;
                }
                float floatNoData = floatRaster.FloatNoDataValue;
                for (int row = 0; row < numRows; row++)
                {
                    for (int col = 0; col < numCols; col++)
                    {
                        if (floatList.Contains(floatRaster.Data[row][col]) == false)
                        {
                            floatList.Add(floatRaster.Data[row][col]);
                            if (floatList.Count > 100)
                            {
                                if (
                                    LogManager.DefaultLogManager.LogMessageBox(MessageStrings.MoreThan100UniqueValues,
                                                                               "Slow Process Alert",
                                                                               MessageBoxButtons.YesNo,
                                                                               MessageBoxIcon.Question) ==
                                    DialogResult.No)
                                    return;
                            }
                            if (floatList.Count > 1000)
                            {
                                LogManager.DefaultLogManager.LogMessageBox(MessageStrings.TooManyUniqueValues,
                                                                           "Too many values.", MessageBoxButtons.OK,
                                                                           MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                }
                _rasterSymbolizer.ColorBreaks.Clear();
                foreach (float value in floatList)
                {
                    if (value != floatNoData)
                    {
                        IColorCategory cb = _rasterSymbolizer.AddColorBreak(value);
                        Color rand = Global.RandomTranslucent(_rasterSymbolizer.Opacity);
                        cb.LowColor = rand;
                        cb.HighColor = rand;
                    }
                }
            }

            // --------------------------- Double -------------------------------------------
            if (_rasterSymbolizer.Raster.DataType == typeof(double))
            {
                List<double> doubleList = new List<double>();


                DoubleRaster doubleRaster = _rasterSymbolizer.Raster as DoubleRaster;
                
                if (doubleRaster == null)
                {
                    LogManager.DefaultLogManager.LogMessageBox(ExceptionMessages.Raster_RasterUndefined,
                                                               "Invalid Opperation.");
                    return;
                }
                double doubleNoData = doubleRaster.DoubleNoDataValue;
                for (int row = 0; row < numRows; row++)
                {
                    for (int col = 0; col < numCols; col++)
                    {
                        if (doubleList.Contains(doubleRaster.Data[row][col]) == false)
                        {
                            doubleList.Add(doubleRaster.Data[row][col]);
                            if (doubleList.Count > 100)
                            {
                                if (
                                    LogManager.DefaultLogManager.LogMessageBox(MessageStrings.MoreThan100UniqueValues,
                                                                               "Slow Process Alert",
                                                                               MessageBoxButtons.YesNo,
                                                                               MessageBoxIcon.Question) ==
                                    DialogResult.No)
                                    return;
                            }
                            if (doubleList.Count > 1000)
                            {
                                LogManager.DefaultLogManager.LogMessageBox(MessageStrings.TooManyUniqueValues,
                                                                           "Too many values.", MessageBoxButtons.OK,
                                                                           MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                }
                _rasterSymbolizer.ColorBreaks.Clear();
                foreach (double value in doubleList)
                {
                    if (value != doubleNoData)
                    {
                        IColorCategory cb = _rasterSymbolizer.AddColorBreak(value);
                        Color rand = Global.RandomTranslucent(_rasterSymbolizer.Opacity);
                        cb.LowColor = rand;
                        cb.HighColor = rand;
                    }
                }
            }
            // --------------------------- Short -------------------------------------------
            if (_rasterSymbolizer.Raster.DataType == typeof(short))
            {
                List<short> shortList = new List<short>();
                ShortRaster shortRaster = _rasterSymbolizer.Raster as ShortRaster;
                if (shortRaster == null)
                {
                    LogManager.DefaultLogManager.LogMessageBox(ExceptionMessages.Raster_RasterUndefined,
                                                               "Invalid Opperation.");
                    return;
                } 
                short shortNoData = shortRaster.ShortNoDataValue;
                for (int row = 0; row < numRows; row++)
                {
                    for (int col = 0; col < numCols; col++)
                    {
                        if (shortList.Contains(shortRaster.Data[row][col]) == false)
                        {
                            shortList.Add(shortRaster.Data[row][col]);
                            if (shortList.Count > 100)
                            {
                                if (
                                    LogManager.DefaultLogManager.LogMessageBox(MessageStrings.MoreThan100UniqueValues,
                                                                               "Slow Process Alert",
                                                                               MessageBoxButtons.YesNo,
                                                                               MessageBoxIcon.Question) ==
                                    DialogResult.No)
                                    return;
                            }
                            if (shortList.Count > 1000)
                            {
                                LogManager.DefaultLogManager.LogMessageBox(MessageStrings.TooManyUniqueValues,
                                                                           "Too many values.", MessageBoxButtons.OK,
                                                                           MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                }
                _rasterSymbolizer.ColorBreaks.Clear();
                foreach (short value in shortList)
                {
                    if (value != shortNoData)
                    {
                        IColorCategory cb = _rasterSymbolizer.AddColorBreak(value);
                        Color rand = Global.RandomTranslucent(_rasterSymbolizer.Opacity);
                        cb.LowColor = rand;
                        cb.HighColor = rand;
                    }
                }
            }
            csvEditor.Invalidate();
        }


        private void txtOpacity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsPunctuation(e.KeyChar))
                e.Handled = true;
        }


        private void TryValidateOpacity()
        {
            if (ValidateOpacity())
            {
                // it passed validate, so it is either one or the other
                if (Global.IsFloat(txtOpacity.Text))
                    _rasterSymbolizer.Opacity = Global.GetFloat(txtOpacity.Text);
                else
                    _rasterSymbolizer.Opacity = float.Parse(txtOpacity.Text);
                int alpha = Convert.ToInt32((255F*_rasterSymbolizer.Opacity));
                if (alpha > 255) alpha = 255;
                if (alpha < 0) alpha = 0;

                foreach (IColorCategory cb in _rasterSymbolizer.ColorBreaks)
                {
                    cb.HighColor = Color.FromArgb(alpha, cb.HighColor);
                    cb.LowColor = Color.FromArgb(alpha, cb.LowColor);
                }

                csvEditor.Invalidate();
            }
        }

        private void txtOpacity_Validating(object sender, CancelEventArgs e)
        {
            TryValidateOpacity();
        }

        private void txtDecimalCount_Validating(object sender, CancelEventArgs e)
        {
            if (ValidateDecimalCount())
            {
                _rasterSymbolizer.DecimalCount = Global.GetInteger(txtDecimalCount.Text);
                csvEditor.Invalidate();
            }
        }

        private void txtDecimalCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits.  No periods etc.
            if (char.IsDigit(e.KeyChar) != true && char.IsControl(e.KeyChar) != true)
                e.Handled = true;
        }

        private void csvEditor_StatusUpdated(object sender, EventArgs e)
        {
            ShowStatus();
        }


        private void txtDecimalCount_TextChanged(object sender, EventArgs e)
        {
            if (ValidateDecimalCount())
            {
                _rasterSymbolizer.DecimalCount = int.Parse(txtDecimalCount.Text);
                csvEditor.Invalidate();
            }
        }


        private void chkComputeHillshade_CheckedChanged(object sender, EventArgs e)
        {
            // Only enable the ShadedRelief button if we are using a hillshade
            btnShadedRelief.Enabled = chkComputeHillshade.Checked;

            if (_rasterSymbolizer == null) _rasterSymbolizer = new RasterSymbolizer();

            if (_rasterSymbolizer.ShadedRelief == null)
                _rasterSymbolizer.ShadedRelief = new ShadedRelief();

            _rasterSymbolizer.ShadedRelief.IsUsed = chkComputeHillshade.Checked;
            OnDataChanged();
        }

        private void btnShadedRelief_Click(object sender, EventArgs e)
        {
            PropertyDialog frm = new PropertyDialog();
            IShadedRelief newCopy = _rasterSymbolizer.ShadedRelief.Copy();
            frm.PropertyGrid.SelectedObject = newCopy;
            if (frm.ShowDialog(this) != DialogResult.OK) return;
            _rasterSymbolizer.ShadedRelief = newCopy;
            OnDataChanged();
        }

        private void frmRasterSymbolizer_Load(object sender, EventArgs e)
        {
            cmbNumberFormat.SelectedIndex = 3; // Auto
            DisplaySymbolizerValues();
        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            _rasterSymbolizer = _backupCopy;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (btnApply.Enabled) ApplyChanges();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void csvEditor_DataChanged(object sender, EventArgs e)
        {
            OnDataChanged();
        }

        private void cmbGradientStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            _rasterSymbolizer.GradientModel = (GradientModels) cmbGradientStyle.SelectedIndex;
        }

        #region Predefined Colorschemes

        private void mnuDeadSea_Click(object sender, EventArgs e)
        {
            SetPrebuiltScheme(ColorSchemes.DeadSea);
        }

        private void mnuDesert_Click(object sender, EventArgs e)
        {
            SetPrebuiltScheme(ColorSchemes.Desert);
        }

        private void mnuFallLeaves_Click(object sender, EventArgs e)
        {
            SetPrebuiltScheme(ColorSchemes.FallLeaves);
        }

        private void mnuGlaciers_Click(object sender, EventArgs e)
        {
            SetPrebuiltScheme(ColorSchemes.Glaciers);
        }

        private void mnuHighway_Click(object sender, EventArgs e)
        {
            SetPrebuiltScheme(ColorSchemes.Highway);
        }

        private void mnuMeadow_Click(object sender, EventArgs e)
        {
            SetPrebuiltScheme(ColorSchemes.Meadow);
        }

        private void mnuSummerMountains_Click(object sender, EventArgs e)
        {
            SetPrebuiltScheme(ColorSchemes.Summer_Mountains);
        }

        private void mnuValleyFires_Click(object sender, EventArgs e)
        {
            SetPrebuiltScheme(ColorSchemes.Valley_Fires);
        }

        #endregion

        #region ToolStripButtons

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            IColorCategory cb = new ColorCategory();
            cb.GradientModel = _rasterSymbolizer.GradientModel;
            cb.LowValue = _rasterSymbolizer.Minimum;
            cb.HighValue = _rasterSymbolizer.Maximum;
            cb.LowColor = Color.DarkGray;
            cb.HighColor = Color.WhiteSmoke;
            cb.GradientModel = _rasterSymbolizer.GradientModel;
            cb.NumberFormat = _rasterSymbolizer.NumberFormat;
            cb.DecimalCount = _rasterSymbolizer.DecimalCount;
            _rasterSymbolizer.ColorBreaks.Add(cb);
            csvEditor.Invalidate();
            OnDataChanged();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MapWindow Coloring Schemes (*.mwleg)|*.mwleg";
            ofd.DefaultExt = "mwleg";
            ofd.Multiselect = false;
            if (ofd.ShowDialog(this) != DialogResult.OK) return;
            _rasterSymbolizer.Open(ofd.FileName);
            csvEditor.Invalidate();
            OnDataChanged();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MapWindow Coloring Schemes (*.mwleg)|*.mwleg";
            sfd.DefaultExt = "mwleg";
            if (sfd.ShowDialog(this) != DialogResult.OK) return;
            _rasterSymbolizer.Save(sfd.FileName);
        }


        private void tsbRemove_Click(object sender, EventArgs e)
        {
            RemoveSelected();
        }

        private void RemoveSelected()
        {
            IList<IColorCategory> colorBreaks = csvEditor.ColorBreaks;
            for (int index = colorBreaks.Count - 1; index >= 0; index--)
            {
                if (colorBreaks[index].IsSelected) colorBreaks.RemoveAt(index);
            }
            csvEditor.ClearSelection();
            csvEditor.Invalidate();
            OnDataChanged();
        }

        #endregion

        #region Wizard

        #endregion

        #endregion

        #region Private Functions

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Assigns the values form the symbolizer to the various combo-boxes and controls on this form.
        /// </summary>
        private void DisplaySymbolizerValues()
        {
            if (_rasterSymbolizer == null) _rasterSymbolizer = new RasterSymbolizer();
            if (_rasterSymbolizer.ShadedRelief == null) _rasterSymbolizer.ShadedRelief = new ShadedRelief();
            if (cmbGradientStyle.Items.Contains(_rasterSymbolizer.GradientModel.ToString()))
                cmbGradientStyle.SelectedItem = _rasterSymbolizer.GradientModel;
            else
                cmbGradientStyle.SelectedItem = 0;
            chkComputeHillshade.Checked = _rasterSymbolizer.ShadedRelief.IsUsed;
            csvEditor.ColorBreaks = _rasterSymbolizer.ColorBreaks;
        }


        private void ApplyChanges()
        {
            if (_rasterSymbolizer.ShadedRelief.IsUsed)
            {
                if (_rasterSymbolizer.ShadedRelief.HasChanged || _rasterSymbolizer.HillShade == null)
                    _rasterSymbolizer.CreateHillShade(mwStatusStrip1);
            }
            if (_rasterSymbolizer.ColorSchemeHasChanged)
            {
                _rasterSymbolizer.ParentLayer.WriteBitmap(mwStatusStrip1);
            }
            _backupCopy = _rasterSymbolizer.Copy();
            OnChangesApplied();
            btnApply.Enabled = false;
        }

        /// <summary>
        /// This just enables the apply button
        /// </summary>
        protected void OnDataChanged()
        {
            btnApply.Enabled = true;
        }

        private void SetPrebuiltScheme(ColorSchemes scheme)
        {
            _rasterSymbolizer.ColorScheme = scheme;
            csvEditor.Refresh();
            OnDataChanged();
        }

        private void ShowStatus()
        {
            bool hasError = false;
            if (txtDecimalCount.Tag != null)
                if ((string) txtDecimalCount.Tag == "Error") hasError = true;
            if (txtOpacity.Tag != null)
                if ((string) txtOpacity.Tag == "Error") hasError = true;
            if (csvEditor.HasErrors)
                hasError = true;
            if (hasError)
            {
                btnApply.Enabled = false;
                btnOK.Enabled = false;
                statusText.Text = "Error.  Mouse over red items.";
            }
            else
            {
                btnApply.Enabled = true;
                btnOK.Enabled = true;
                statusText.Text = "Ready.";
            }
        }

        private bool ValidateOpacity()
        {
            float val;
            bool hasError = false;

            if (Global.IsFloat(txtOpacity.Text) == false)
            {
                if (float.TryParse(txtOpacity.Text, out val) == false)
                    hasError = true;
                else if (val < 0 || val > 1) hasError = true;
            }
            else
            {
                val = Global.GetFloat(txtDecimalCount.Text);
                if (val < 0 || val > 1) hasError = true;
            }
            if (hasError)
            {
                ttInfo.SetToolTip(txtOpacity, MessageStrings.OpacityInvalid);
                txtOpacity.Tag = "Error";
                txtOpacity.BackColor = Color.Salmon;
            }
            else
            {
                ttInfo.SetToolTip(txtOpacity, MessageStrings.Opacity);
                txtOpacity.Tag = null;
                txtOpacity.BackColor = Color.White;
            }
            ShowStatus();
            return !hasError;
        }

        private bool ValidateDecimalCount()
        {
            bool hasError = false;

            if (Global.IsInteger(txtDecimalCount.Text) == false) hasError = true;
            else
            {
                int val = Global.GetInteger(txtDecimalCount.Text);
                if (val < 0 || val > 15) hasError = true;
            }
            if (hasError)
            {
                ttInfo.SetToolTip(txtDecimalCount, MessageStrings.DecimalCountInvalid);
                txtDecimalCount.Tag = "Error";
                txtDecimalCount.BackColor = Color.Salmon;
            }
            else
            {
                ttInfo.SetToolTip(txtDecimalCount, MessageStrings.DecimalCount);
                txtDecimalCount.Tag = null;
                txtDecimalCount.BackColor = Color.White;
            }
            ShowStatus();
            return !hasError;
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof (RasterSymbolizerDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.chkDrapeLayers = new System.Windows.Forms.CheckBox();
            this.txtOpacity = new System.Windows.Forms.TextBox();
            this.lblOpacity = new System.Windows.Forms.Label();
            this.txtDecimalCount = new System.Windows.Forms.TextBox();
            this.lblDecimalCount = new System.Windows.Forms.Label();
            this.btnShadedRelief = new System.Windows.Forms.Button();
            this.cmbNumberFormat = new System.Windows.Forms.ComboBox();
            this.lblNumberFormat = new System.Windows.Forms.Label();
            this.cmbGradientStyle = new System.Windows.Forms.ComboBox();
            this.lblGradient = new System.Windows.Forms.Label();
            this.btnClassify = new System.Windows.Forms.Button();
            this.chkComputeHillshade = new System.Windows.Forms.CheckBox();
            this.tsbMinus = new System.Windows.Forms.ToolStrip();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbRemove = new System.Windows.Forms.ToolStripButton();
            this.tsbWizard = new System.Windows.Forms.ToolStripSplitButton();
            this.mnuColorRamp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEqualBreaks = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUniqueValues = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPredefined = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeadSea = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDesert = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFallLeaves = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGlaciers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHighway = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMeadow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSummerMountains = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuValleyFires = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.mwStatusStrip1 = new MapWindow.Components.mwStatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlColorBreakEditor = new System.Windows.Forms.Panel();
            this.csvEditor = new MapWindow.Components.ColorBreakViewer();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            this.chkSmooth = new System.Windows.Forms.CheckBox();
            this.pnlHeader.SuspendLayout();
            this.tsbMinus.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.mwStatusStrip1.SuspendLayout();
            this.pnlColorBreakEditor.SuspendLayout();
            this.csvEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(295, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(219, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(70, 23);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.chkSmooth);
            this.pnlHeader.Controls.Add(this.chkDrapeLayers);
            this.pnlHeader.Controls.Add(this.txtOpacity);
            this.pnlHeader.Controls.Add(this.lblOpacity);
            this.pnlHeader.Controls.Add(this.txtDecimalCount);
            this.pnlHeader.Controls.Add(this.lblDecimalCount);
            this.pnlHeader.Controls.Add(this.btnShadedRelief);
            this.pnlHeader.Controls.Add(this.cmbNumberFormat);
            this.pnlHeader.Controls.Add(this.lblNumberFormat);
            this.pnlHeader.Controls.Add(this.cmbGradientStyle);
            this.pnlHeader.Controls.Add(this.lblGradient);
            this.pnlHeader.Controls.Add(this.btnClassify);
            this.pnlHeader.Controls.Add(this.chkComputeHillshade);
            this.pnlHeader.Controls.Add(this.tsbMinus);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(362, 187);
            this.pnlHeader.TabIndex = 3;
            this.pnlHeader.Click += new System.EventHandler(this.pnlHeader_Click);
            // 
            // chkDrapeLayers
            // 
            this.chkDrapeLayers.AutoSize = true;
            this.chkDrapeLayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                                                               System.Drawing.FontStyle.Bold,
                                                               System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.chkDrapeLayers.Location = new System.Drawing.Point(15, 150);
            this.chkDrapeLayers.Name = "chkDrapeLayers";
            this.chkDrapeLayers.Size = new System.Drawing.Size(183, 17);
            this.chkDrapeLayers.TabIndex = 10;
            this.chkDrapeLayers.Text = "Drape Visible Vector Layers";
            this.chkDrapeLayers.UseVisualStyleBackColor = true;
            this.chkDrapeLayers.CheckedChanged += new System.EventHandler(this.chkDrapeLayers_CheckedChanged);
            // 
            // txtOpacity
            // 
            this.txtOpacity.Location = new System.Drawing.Point(72, 114);
            this.txtOpacity.Name = "txtOpacity";
            this.txtOpacity.Size = new System.Drawing.Size(79, 20);
            this.txtOpacity.TabIndex = 9;
            this.ttInfo.SetToolTip(this.txtOpacity,
                                   "Specifies transparency as a floating point decimal range of values between 0 and " +
                                   "1");
            this.txtOpacity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOpacity_KeyPress);
            this.txtOpacity.Validating += new System.ComponentModel.CancelEventHandler(this.txtOpacity_Validating);
            // 
            // lblOpacity
            // 
            this.lblOpacity.AutoSize = true;
            this.lblOpacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                                           System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblOpacity.Location = new System.Drawing.Point(12, 117);
            this.lblOpacity.Name = "lblOpacity";
            this.lblOpacity.Size = new System.Drawing.Size(54, 13);
            this.lblOpacity.TabIndex = 8;
            this.lblOpacity.Text = "Opacity:";
            // 
            // txtDecimalCount
            // 
            this.txtDecimalCount.Location = new System.Drawing.Point(290, 114);
            this.txtDecimalCount.Name = "txtDecimalCount";
            this.txtDecimalCount.Size = new System.Drawing.Size(52, 20);
            this.txtDecimalCount.TabIndex = 7;
            this.ttInfo.SetToolTip(this.txtDecimalCount, "The number of digits after the decimal.");
            this.txtDecimalCount.TextChanged += new System.EventHandler(this.txtDecimalCount_TextChanged);
            this.txtDecimalCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDecimalCount_KeyPress);
            this.txtDecimalCount.Validating +=
                new System.ComponentModel.CancelEventHandler(this.txtDecimalCount_Validating);
            // 
            // lblDecimalCount
            // 
            this.lblDecimalCount.AutoSize = true;
            this.lblDecimalCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                                                                System.Drawing.FontStyle.Bold,
                                                                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblDecimalCount.Location = new System.Drawing.Point(191, 117);
            this.lblDecimalCount.Name = "lblDecimalCount";
            this.lblDecimalCount.Size = new System.Drawing.Size(93, 13);
            this.lblDecimalCount.TabIndex = 6;
            this.lblDecimalCount.Text = "Decimal Count:";
            // 
            // btnShadedRelief
            // 
            this.btnShadedRelief.Location = new System.Drawing.Point(157, 30);
            this.btnShadedRelief.Name = "btnShadedRelief";
            this.btnShadedRelief.Size = new System.Drawing.Size(190, 23);
            this.btnShadedRelief.TabIndex = 2;
            this.btnShadedRelief.Text = "Edit &Shaded Relief";
            this.ttInfo.SetToolTip(this.btnShadedRelief, "Detailed control of shading applied to a texture.");
            this.btnShadedRelief.UseVisualStyleBackColor = true;
            this.btnShadedRelief.Click += new System.EventHandler(this.btnShadedRelief_Click);
            // 
            // cmbNumberFormat
            // 
            this.cmbNumberFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNumberFormat.FormattingEnabled = true;
            this.cmbNumberFormat.Items.AddRange(new object[]
                                                    {
                                                        "Currency",
                                                        "Exponential",
                                                        "Fixed Digit",
                                                        "General",
                                                        "Number",
                                                        "Percent"
                                                    });
            this.cmbNumberFormat.Location = new System.Drawing.Point(157, 88);
            this.cmbNumberFormat.Name = "cmbNumberFormat";
            this.cmbNumberFormat.Size = new System.Drawing.Size(190, 21);
            this.cmbNumberFormat.TabIndex = 5;
            this.ttInfo.SetToolTip(this.cmbNumberFormat, "Describes how the values are converted to strings.");
            this.cmbNumberFormat.SelectedIndexChanged +=
                new System.EventHandler(this.cmbNumberFormat_SelectedIndexChanged);
            // 
            // lblNumberFormat
            // 
            this.lblNumberFormat.AutoSize = true;
            this.lblNumberFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                                                                System.Drawing.FontStyle.Bold,
                                                                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblNumberFormat.Location = new System.Drawing.Point(59, 91);
            this.lblNumberFormat.Name = "lblNumberFormat";
            this.lblNumberFormat.Size = new System.Drawing.Size(96, 13);
            this.lblNumberFormat.TabIndex = 5;
            this.lblNumberFormat.Text = "Number Format:";
            // 
            // cmbGradientStyle
            // 
            this.cmbGradientStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradientStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                                                                 System.Drawing.FontStyle.Regular,
                                                                 System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.cmbGradientStyle.FormattingEnabled = true;
            this.cmbGradientStyle.Items.AddRange(new object[]
                                                     {
                                                         "Linear",
                                                         "Logorithmic",
                                                         "Exponential"
                                                     });
            this.cmbGradientStyle.Location = new System.Drawing.Point(157, 61);
            this.cmbGradientStyle.Name = "cmbGradientStyle";
            this.cmbGradientStyle.Size = new System.Drawing.Size(190, 21);
            this.cmbGradientStyle.TabIndex = 4;
            this.ttInfo.SetToolTip(this.cmbGradientStyle, "Describes how to ramp the color range on each break.");
            this.cmbGradientStyle.SelectedIndexChanged +=
                new System.EventHandler(this.cmbGradientStyle_SelectedIndexChanged);
            // 
            // lblGradient
            // 
            this.lblGradient.AutoSize = true;
            this.lblGradient.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                                            System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblGradient.Location = new System.Drawing.Point(12, 64);
            this.lblGradient.Name = "lblGradient";
            this.lblGradient.Size = new System.Drawing.Size(143, 13);
            this.lblGradient.TabIndex = 3;
            this.lblGradient.Text = "Bi-Value Color Gradient:";
            // 
            // btnClassify
            // 
            this.btnClassify.Location = new System.Drawing.Point(272, 3);
            this.btnClassify.Name = "btnClassify";
            this.btnClassify.Size = new System.Drawing.Size(75, 23);
            this.btnClassify.TabIndex = 3;
            this.btnClassify.Text = "C&lassify";
            this.ttInfo.SetToolTip(this.btnClassify, "Advanced Classification");
            this.btnClassify.UseVisualStyleBackColor = true;
            // 
            // chkComputeHillshade
            // 
            this.chkComputeHillshade.AutoSize = true;
            this.chkComputeHillshade.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                                                                    System.Drawing.FontStyle.Bold,
                                                                    System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.chkComputeHillshade.Location = new System.Drawing.Point(20, 34);
            this.chkComputeHillshade.Name = "chkComputeHillshade";
            this.chkComputeHillshade.Size = new System.Drawing.Size(131, 17);
            this.chkComputeHillshade.TabIndex = 1;
            this.chkComputeHillshade.Text = "Compute &Hillshade";
            this.ttInfo.SetToolTip(this.chkComputeHillshade,
                                   "If checked, the texture is shaded as though the data were elevation values.");
            this.chkComputeHillshade.UseVisualStyleBackColor = true;
            this.chkComputeHillshade.CheckedChanged += new System.EventHandler(this.chkComputeHillshade_CheckedChanged);
            // 
            // tsbMinus
            // 
            this.tsbMinus.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
                                             {
                                                 this.tsbOpen,
                                                 this.tsbSave,
                                                 this.tsbAdd,
                                                 this.tsbRemove,
                                                 this.tsbWizard
                                             });
            this.tsbMinus.Location = new System.Drawing.Point(0, 0);
            this.tsbMinus.Name = "tsbMinus";
            this.tsbMinus.Size = new System.Drawing.Size(362, 25);
            this.tsbMinus.TabIndex = 0;
            this.tsbMinus.Text = "toolStrip1";
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = ((System.Drawing.Image) (resources.GetObject("tsbOpen.Image")));
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbOpen.ToolTipText = "Open";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = ((System.Drawing.Image) (resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.ToolTipText = "Save";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbAdd
            // 
            this.tsbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAdd.Image = ((System.Drawing.Image) (resources.GetObject("tsbAdd.Image")));
            this.tsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAdd.Name = "tsbAdd";
            this.tsbAdd.Size = new System.Drawing.Size(23, 22);
            this.tsbAdd.Text = "toolStripButton1";
            this.tsbAdd.ToolTipText = "Add";
            this.tsbAdd.Click += new System.EventHandler(this.tsbAdd_Click);
            // 
            // tsbRemove
            // 
            this.tsbRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemove.Image = ((System.Drawing.Image) (resources.GetObject("tsbRemove.Image")));
            this.tsbRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemove.Name = "tsbRemove";
            this.tsbRemove.Size = new System.Drawing.Size(23, 22);
            this.tsbRemove.ToolTipText = "Remove";
            this.tsbRemove.Click += new System.EventHandler(this.tsbRemove_Click);
            // 
            // tsbWizard
            // 
            this.tsbWizard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbWizard.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
                                                      {
                                                          this.mnuColorRamp,
                                                          this.mnuEqualBreaks,
                                                          this.mnuUniqueValues,
                                                          this.mnuPredefined
                                                      });
            this.tsbWizard.Image = ((System.Drawing.Image) (resources.GetObject("tsbWizard.Image")));
            this.tsbWizard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWizard.Name = "tsbWizard";
            this.tsbWizard.Size = new System.Drawing.Size(32, 22);
            this.tsbWizard.ToolTipText = "Wizard";
            // 
            // mnuColorRamp
            // 
            this.mnuColorRamp.Name = "mnuColorRamp";
            this.mnuColorRamp.Size = new System.Drawing.Size(149, 22);
            this.mnuColorRamp.Text = "&Color Ramp";
            this.mnuColorRamp.Click += new System.EventHandler(this.mnuColorRamp_Click);
            // 
            // mnuEqualBreaks
            // 
            this.mnuEqualBreaks.Name = "mnuEqualBreaks";
            this.mnuEqualBreaks.Size = new System.Drawing.Size(149, 22);
            this.mnuEqualBreaks.Text = "&Equal Breaks";
            this.mnuEqualBreaks.Click += new System.EventHandler(this.mnuEqualBreaks_Click);
            // 
            // mnuUniqueValues
            // 
            this.mnuUniqueValues.Name = "mnuUniqueValues";
            this.mnuUniqueValues.Size = new System.Drawing.Size(149, 22);
            this.mnuUniqueValues.Text = "&Unique Values";
            this.mnuUniqueValues.Click += new System.EventHandler(this.mnuUniqueValues_Click);
            // 
            // mnuPredefined
            // 
            this.mnuPredefined.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
                                                          {
                                                              this.mnuDeadSea,
                                                              this.mnuDesert,
                                                              this.mnuFallLeaves,
                                                              this.mnuGlaciers,
                                                              this.mnuHighway,
                                                              this.mnuMeadow,
                                                              this.mnuSummerMountains,
                                                              this.mnuValleyFires
                                                          });
            this.mnuPredefined.Name = "mnuPredefined";
            this.mnuPredefined.Size = new System.Drawing.Size(149, 22);
            this.mnuPredefined.Text = "&Pre-defined";
            // 
            // mnuDeadSea
            // 
            this.mnuDeadSea.Name = "mnuDeadSea";
            this.mnuDeadSea.Size = new System.Drawing.Size(179, 22);
            this.mnuDeadSea.Text = "&Dead Sea";
            this.mnuDeadSea.Click += new System.EventHandler(this.mnuDeadSea_Click);
            // 
            // mnuDesert
            // 
            this.mnuDesert.Name = "mnuDesert";
            this.mnuDesert.Size = new System.Drawing.Size(179, 22);
            this.mnuDesert.Text = "D&esert";
            this.mnuDesert.Click += new System.EventHandler(this.mnuDesert_Click);
            // 
            // mnuFallLeaves
            // 
            this.mnuFallLeaves.Name = "mnuFallLeaves";
            this.mnuFallLeaves.Size = new System.Drawing.Size(179, 22);
            this.mnuFallLeaves.Text = "&Fall Leaves";
            this.mnuFallLeaves.Click += new System.EventHandler(this.mnuFallLeaves_Click);
            // 
            // mnuGlaciers
            // 
            this.mnuGlaciers.Name = "mnuGlaciers";
            this.mnuGlaciers.Size = new System.Drawing.Size(179, 22);
            this.mnuGlaciers.Text = "&Glaciers";
            this.mnuGlaciers.Click += new System.EventHandler(this.mnuGlaciers_Click);
            // 
            // mnuHighway
            // 
            this.mnuHighway.Name = "mnuHighway";
            this.mnuHighway.Size = new System.Drawing.Size(179, 22);
            this.mnuHighway.Text = "&Highway";
            this.mnuHighway.Click += new System.EventHandler(this.mnuHighway_Click);
            // 
            // mnuMeadow
            // 
            this.mnuMeadow.Name = "mnuMeadow";
            this.mnuMeadow.Size = new System.Drawing.Size(179, 22);
            this.mnuMeadow.Text = "&Meadow";
            this.mnuMeadow.Click += new System.EventHandler(this.mnuMeadow_Click);
            // 
            // mnuSummerMountains
            // 
            this.mnuSummerMountains.Name = "mnuSummerMountains";
            this.mnuSummerMountains.Size = new System.Drawing.Size(179, 22);
            this.mnuSummerMountains.Text = "&Summer Mountains";
            this.mnuSummerMountains.Click += new System.EventHandler(this.mnuSummerMountains_Click);
            // 
            // mnuValleyFires
            // 
            this.mnuValleyFires.Name = "mnuValleyFires";
            this.mnuValleyFires.Size = new System.Drawing.Size(179, 22);
            this.mnuValleyFires.Text = "&Valley Fires";
            this.mnuValleyFires.Click += new System.EventHandler(this.mnuValleyFires_Click);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.mwStatusStrip1);
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnApply);
            this.pnlFooter.Controls.Add(this.btnOK);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 531);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(362, 52);
            this.pnlFooter.TabIndex = 4;
            this.pnlFooter.Click += new System.EventHandler(this.pnlFooter_Click);
            // 
            // mwStatusStrip1
            // 
            this.mwStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
                                                   {
                                                       this.statusText,
                                                       this.statusProgressBar
                                                   });
            this.mwStatusStrip1.Location = new System.Drawing.Point(0, 30);
            this.mwStatusStrip1.Name = "mwStatusStrip1";
            this.mwStatusStrip1.Size = new System.Drawing.Size(362, 22);
            this.mwStatusStrip1.TabIndex = 9;
            this.mwStatusStrip1.Text = "mwStatusStrip1";
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(245, 17);
            this.statusText.Spring = true;
            this.statusText.Text = "Ready.";
            this.statusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusProgressBar
            // 
            this.statusProgressBar.Name = "statusProgressBar";
            this.statusProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlColorBreakEditor
            // 
            this.pnlColorBreakEditor.Controls.Add(this.csvEditor);
            this.pnlColorBreakEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColorBreakEditor.Location = new System.Drawing.Point(0, 187);
            this.pnlColorBreakEditor.Name = "pnlColorBreakEditor";
            this.pnlColorBreakEditor.Size = new System.Drawing.Size(362, 344);
            this.pnlColorBreakEditor.TabIndex = 5;
            // 
            // csvEditor
            // 
            // 
            // csvEditor.ColorPanel
            // 
            this.csvEditor.ColorPanel.ContentStyle = MapWindow.Components.ColorPanelStyle.Colors;
            this.csvEditor.ColorPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.csvEditor.ColorPanel.ErrorColor = System.Drawing.Color.Salmon;
            this.csvEditor.ColorPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F,
                                                                     System.Drawing.FontStyle.Regular,
                                                                     System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.csvEditor.ColorPanel.HasErrors = false;
            this.csvEditor.ColorPanel.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.csvEditor.ColorPanel.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.csvEditor.ColorPanel.HorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.csvEditor.ColorPanel.ItemSpacing = 2;
            this.csvEditor.ColorPanel.Location = new System.Drawing.Point(0, 30);
            this.csvEditor.ColorPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.csvEditor.ColorPanel.Name = "ColorPanel";
            this.csvEditor.ColorPanel.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.csvEditor.ColorPanel.Size = new System.Drawing.Size(64, 314);
            this.csvEditor.ColorPanel.StartIndex = 0;
            this.csvEditor.ColorPanel.TabIndex = 13;
            this.csvEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.csvEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular,
                                                          System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.csvEditor.HeaderBackgroundColor = System.Drawing.Color.FromArgb(((int) (((byte) (224)))),
                                                                                 ((int) (((byte) (224)))),
                                                                                 ((int) (((byte) (224)))));
            this.csvEditor.HeaderBorderVisible = true;
            this.csvEditor.HeaderFont = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular,
                                                                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.csvEditor.HeaderForeColor = System.Drawing.Color.Black;
            this.csvEditor.HeaderTextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.csvEditor.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.csvEditor.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.csvEditor.ItemHeight = 19;
            this.csvEditor.ItemSpacing = 2;
            this.csvEditor.Location = new System.Drawing.Point(0, 0);
            this.csvEditor.Margin = new System.Windows.Forms.Padding(5);
            this.csvEditor.Name = "csvEditor";
            this.csvEditor.ScrollBarWidth = 17;
            this.csvEditor.Size = new System.Drawing.Size(362, 344);
            this.csvEditor.TabIndex = 0;
            this.csvEditor.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // csvEditor.TextPanel
            // 
            this.csvEditor.TextPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.csvEditor.TextPanel.ContentStyle = MapWindow.Components.ColorPanelStyle.Captions;
            this.csvEditor.TextPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.csvEditor.TextPanel.ErrorColor = System.Drawing.Color.Salmon;
            this.csvEditor.TextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F,
                                                                    System.Drawing.FontStyle.Regular,
                                                                    System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.csvEditor.TextPanel.HasErrors = false;
            this.csvEditor.TextPanel.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.csvEditor.TextPanel.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.csvEditor.TextPanel.HorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.csvEditor.TextPanel.ItemSpacing = 2;
            this.csvEditor.TextPanel.Location = new System.Drawing.Point(149, 0);
            this.csvEditor.TextPanel.Margin = new System.Windows.Forms.Padding(5);
            this.csvEditor.TextPanel.Name = "TextPanel";
            this.csvEditor.TextPanel.Size = new System.Drawing.Size(149, 314);
            this.csvEditor.TextPanel.StartIndex = 0;
            this.csvEditor.TextPanel.TabIndex = 15;
            this.ttInfo.SetToolTip(this.csvEditor, "Lists individual color breaks.");
            // 
            // csvEditor.ValuePanel
            // 
            this.csvEditor.ValuePanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.csvEditor.ValuePanel.ContentStyle = MapWindow.Components.ColorPanelStyle.Values;
            this.csvEditor.ValuePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.csvEditor.ValuePanel.ErrorColor = System.Drawing.Color.Salmon;
            this.csvEditor.ValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F,
                                                                     System.Drawing.FontStyle.Regular,
                                                                     System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.csvEditor.ValuePanel.HasErrors = false;
            this.csvEditor.ValuePanel.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.csvEditor.ValuePanel.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.csvEditor.ValuePanel.HorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.csvEditor.ValuePanel.ItemSpacing = 2;
            this.csvEditor.ValuePanel.Location = new System.Drawing.Point(0, 0);
            this.csvEditor.ValuePanel.Margin = new System.Windows.Forms.Padding(5);
            this.csvEditor.ValuePanel.Name = "ValuePanel";
            this.csvEditor.ValuePanel.Size = new System.Drawing.Size(149, 314);
            this.csvEditor.ValuePanel.StartIndex = 0;
            this.csvEditor.ValuePanel.TabIndex = 14;
            this.csvEditor.StatusUpdated += new System.EventHandler(this.csvEditor_StatusUpdated);
            this.csvEditor.DataChanged += new System.EventHandler(this.csvEditor_DataChanged);
            // 
            // chkSmooth
            // 
            this.chkSmooth.AutoSize = true;
            this.chkSmooth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                                          System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.chkSmooth.Location = new System.Drawing.Point(260, 150);
            this.chkSmooth.Name = "chkSmooth";
            this.chkSmooth.Size = new System.Drawing.Size(82, 17);
            this.chkSmooth.TabIndex = 11;
            this.chkSmooth.Text = "Smoothed";
            this.chkSmooth.UseVisualStyleBackColor = true;
            this.chkSmooth.CheckedChanged += new System.EventHandler(this.chkSmooth_CheckedChanged);
            // 
            // frmRasterSymbolizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 583);
            this.Controls.Add(this.pnlColorBreakEditor);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Name = "frmRasterSymbolizer";
            this.Text = "Color Scheme Editor";
            this.Load += new System.EventHandler(this.frmRasterSymbolizer_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tsbMinus.ResumeLayout(false);
            this.tsbMinus.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.mwStatusStrip1.ResumeLayout(false);
            this.mwStatusStrip1.PerformLayout();
            this.pnlColorBreakEditor.ResumeLayout(false);
            this.csvEditor.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void pnlFooter_Click(object sender, EventArgs e)
        {
            TryValidateOpacity();
        }

        private void pnlHeader_Click(object sender, EventArgs e)
        {
            TryValidateOpacity();
        }

        #endregion

        private void chkDrapeLayers_CheckedChanged(object sender, EventArgs e)
        {
            _rasterSymbolizer.DrapeVectorLayers = chkDrapeLayers.Checked;
            btnApply.Enabled = true;
        }

        private void chkSmooth_CheckedChanged(object sender, EventArgs e)
        {
            _rasterSymbolizer.IsSmoothed = chkSmooth.Checked;
            btnApply.Enabled = true;
        }
    }
}