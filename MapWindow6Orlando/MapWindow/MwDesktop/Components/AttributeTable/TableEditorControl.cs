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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in Fall 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
// Kandasamy Prasanna (2009-09-11) main contributor to most of the functionality
// Jiri Kadlec (2009-10-30) Changed the form into a user control, improved selection when sorting
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Map;
using MapWindow.Geometries;
using MapWindow.Forms;
using MapWindow.Forms.Attributes;

namespace MapWindow.Components.AttributeTable
{
    /// <summary>
    /// A Table editor user control. This may be used for displaying attributes of a feature layer.
    /// </summary>
    [ToolboxBitmap(typeof(TableEditorControl), "UserControl.ico")]
    public class TableEditorControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs whenever the user selects, de-selects or in any way updates the row selections
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Occurs whenever the user click SelectioinZoom button.
        /// </summary>
        public event EventHandler SelectionZoom;

        /// <summary>
        /// occurs whenever the user click RefreshMap button.
        /// </summary>
        public event EventHandler MapRefreshed;

        /// <summary>
        /// This will fire when user press ZoomToShapeBeingEdited button.
        /// </summary>
        public event EventHandler ZoomToShapeBeingEdited;

        #endregion

        #region Private Variables

        private IMapFeatureLayer _featureLayer;
        
        private DataTable _table;
        private bool _ignoreSelectionChanged;
        private bool _ignoreTableSelectionChanged;
        private bool _isEditable = true;
        private bool _showOnlySelectedRows;
        private bool _virtualHooked;
        private List<int> _selectionIndices;
        private DataTable _selection;
        private List<int> _selectedRows;
        private bool _loaded;
        private string _fidField;

        #region Windows Form Designer generated code

        private Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private System.Windows.Forms.ToolStripMenuItem mnuSelection;
        private System.Windows.Forms.ToolStripMenuItem mnuTools;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbtnZoomToSelected;
        private System.Windows.Forms.ToolStripButton tsbtnShowSelected;
        private System.Windows.Forms.ToolStripButton tsbtnImportFieldsFromDBF;
        private System.Windows.Forms.ToolStripButton tsbtnFieldCalculator;
        private System.Windows.Forms.ToolStripButton tsbtnRefreshMap;
        private System.Windows.Forms.ToolStripButton tsbtnRefresh;
        private System.Windows.Forms.ToolStripButton tsbtnQuery;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.ComponentModel.IContainer components = null;
        private ToolStripMenuItem addFieldToolStripMenuItem;
        private ToolStripMenuItem removeFieldToolStripMenuItem;
        private ToolStripMenuItem renameFieldToolStripMenuItem;
        private ToolStripMenuItem showOnlySelectedShapesToolStripMenuItem;
        private ToolStripMenuItem zoomToSelectedShapesToolStripMenuItem;
        private ToolStripMenuItem zoomToShapeBeingEditedToolStripMenuItem;
        private ToolStripMenuItem flashSelectedShapesToolStripMenuItem;
        private ToolStripMenuItem queryToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem selectNoneToolStripMenuItem;
        private ToolStripMenuItem invertSelectionToolStripMenuItem;
        private ToolStripMenuItem exportSelectedFeaturesToolStripMenuItem;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem importFieldDefinitionsFromDBFToolStripMenuItem;
        private ToolStripMenuItem fieldCalculatorToolToolStripMenuItem;
        private ToolStripMenuItem generateOrUpdateMWShapeIDFieldsToolStripMenuItem;
        private ToolStripMenuItem copyShapeIDsToSpecifiedFieldToolStripMenuItem;
        private Label lblSelectedNumber;
        private ToolStripButton tsbtnSaveEdits;
        private ToolStripMenuItem enableEditingToolStripMenuItem;
        private ToolStripMenuItem saveEditsToolStripMenuItem;
        private Label lblFilePath;
        private AttributeCache _attributeCache;
        private bool _bound;
        #endregion

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblSelectedNumber = new System.Windows.Forms.Label();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.addFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableEditingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveEditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this.showOnlySelectedShapesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToSelectedShapesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToShapeBeingEditedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flashSelectedShapesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSelectedFeaturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFieldDefinitionsFromDBFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldCalculatorToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbtnSaveEdits = new System.Windows.Forms.ToolStripButton();
            this.tsbtnZoomToSelected = new System.Windows.Forms.ToolStripButton();
            this.tsbtnShowSelected = new System.Windows.Forms.ToolStripButton();
            this.tsbtnImportFieldsFromDBF = new System.Windows.Forms.ToolStripButton();
            this.tsbtnFieldCalculator = new System.Windows.Forms.ToolStripButton();
            this.tsbtnRefreshMap = new System.Windows.Forms.ToolStripButton();
            this.tsbtnRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbtnQuery = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.lblSelectedNumber);
            this.panel1.Controls.Add(this.lblFilePath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 264);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 36);
            this.panel1.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(80, 6);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(196, 23);
            this.progressBar.TabIndex = 6;
            // 
            // lblSelectedNumber
            // 
            this.lblSelectedNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectedNumber.AutoSize = true;
            this.lblSelectedNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedNumber.Location = new System.Drawing.Point(281, 9);
            this.lblSelectedNumber.Name = "lblSelectedNumber";
            this.lblSelectedNumber.Size = new System.Drawing.Size(93, 15);
            this.lblSelectedNumber.TabIndex = 9;
            this.lblSelectedNumber.Text = "0 Row Selected";
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilePath.Location = new System.Drawing.Point(10, 9);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(64, 15);
            this.lblFilePath.TabIndex = 10;
            this.lblFilePath.Text = "File Name";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEdit,
            this.mnuView,
            this.mnuSelection,
            this.mnuTools});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(400, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFieldToolStripMenuItem,
            this.removeFieldToolStripMenuItem,
            this.renameFieldToolStripMenuItem,
            this.enableEditingToolStripMenuItem,
            this.saveEditsToolStripMenuItem});
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(39, 20);
            this.mnuEdit.Text = "Edit";
            // 
            // addFieldToolStripMenuItem
            // 
            this.addFieldToolStripMenuItem.Name = "addFieldToolStripMenuItem";
            this.addFieldToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.addFieldToolStripMenuItem.Text = "Add Field";
            this.addFieldToolStripMenuItem.Click += new System.EventHandler(this.addFieldToolStripMenuItem_Click);
            // 
            // removeFieldToolStripMenuItem
            // 
            this.removeFieldToolStripMenuItem.Name = "removeFieldToolStripMenuItem";
            this.removeFieldToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.removeFieldToolStripMenuItem.Text = "Remove Field";
            this.removeFieldToolStripMenuItem.Click += new System.EventHandler(this.removeFieldToolStripMenuItem_Click);
            // 
            // renameFieldToolStripMenuItem
            // 
            this.renameFieldToolStripMenuItem.Name = "renameFieldToolStripMenuItem";
            this.renameFieldToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.renameFieldToolStripMenuItem.Text = "Rename Field";
            this.renameFieldToolStripMenuItem.Click += new System.EventHandler(this.renameFieldToolStripMenuItem_Click);
            // 
            // enableEditingToolStripMenuItem
            // 
            this.enableEditingToolStripMenuItem.Checked = true;
            this.enableEditingToolStripMenuItem.CheckOnClick = true;
            this.enableEditingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableEditingToolStripMenuItem.Name = "enableEditingToolStripMenuItem";
            this.enableEditingToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.enableEditingToolStripMenuItem.Text = "Enable Editing";
            // 
            // saveEditsToolStripMenuItem
            // 
            this.saveEditsToolStripMenuItem.Name = "saveEditsToolStripMenuItem";
            this.saveEditsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.saveEditsToolStripMenuItem.Text = "Save Edits";
            this.saveEditsToolStripMenuItem.Visible = false;
            this.saveEditsToolStripMenuItem.Click += new System.EventHandler(this.saveEditsToolStripMenuItem_Click);
            // 
            // mnuView
            // 
            this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOnlySelectedShapesToolStripMenuItem,
            this.zoomToSelectedShapesToolStripMenuItem,
            this.zoomToShapeBeingEditedToolStripMenuItem,
            this.flashSelectedShapesToolStripMenuItem});
            this.mnuView.Name = "mnuView";
            this.mnuView.Size = new System.Drawing.Size(44, 20);
            this.mnuView.Text = "View";
            // 
            // showOnlySelectedShapesToolStripMenuItem
            // 
            this.showOnlySelectedShapesToolStripMenuItem.CheckOnClick = true;
            this.showOnlySelectedShapesToolStripMenuItem.Name = "showOnlySelectedShapesToolStripMenuItem";
            this.showOnlySelectedShapesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showOnlySelectedShapesToolStripMenuItem.Text = "Show only Selected Shapes";
            this.showOnlySelectedShapesToolStripMenuItem.Click += new System.EventHandler(this.showOnlySelectedShapesToolStripMenuItem_Click);
            // 
            // zoomToSelectedShapesToolStripMenuItem
            // 
            this.zoomToSelectedShapesToolStripMenuItem.Name = "zoomToSelectedShapesToolStripMenuItem";
            this.zoomToSelectedShapesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.zoomToSelectedShapesToolStripMenuItem.Text = "Zoom to Selected Shapes";
            this.zoomToSelectedShapesToolStripMenuItem.Click += new System.EventHandler(this.zoomToSelectedShapesToolStripMenuItem_Click);
            // 
            // zoomToShapeBeingEditedToolStripMenuItem
            // 
            this.zoomToShapeBeingEditedToolStripMenuItem.Name = "zoomToShapeBeingEditedToolStripMenuItem";
            this.zoomToShapeBeingEditedToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.zoomToShapeBeingEditedToolStripMenuItem.Text = "Zoom to Shape Being Edited";
            this.zoomToShapeBeingEditedToolStripMenuItem.Click += new System.EventHandler(this.zoomToShapeBeingEditedToolStripMenuItem_Click);
            // 
            // flashSelectedShapesToolStripMenuItem
            // 
            this.flashSelectedShapesToolStripMenuItem.Enabled = false;
            this.flashSelectedShapesToolStripMenuItem.Name = "flashSelectedShapesToolStripMenuItem";
            this.flashSelectedShapesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.flashSelectedShapesToolStripMenuItem.Text = "Flash Selected Shapes";
            this.flashSelectedShapesToolStripMenuItem.Click += new System.EventHandler(this.flashSelectedShapesToolStripMenuItem_Click);
            // 
            // mnuSelection
            // 
            this.mnuSelection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queryToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem,
            this.invertSelectionToolStripMenuItem,
            this.exportSelectedFeaturesToolStripMenuItem});
            this.mnuSelection.Name = "mnuSelection";
            this.mnuSelection.Size = new System.Drawing.Size(67, 20);
            this.mnuSelection.Text = "Selection";
            // 
            // queryToolStripMenuItem
            // 
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            this.queryToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.queryToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.queryToolStripMenuItem.Text = "Query";
            this.queryToolStripMenuItem.Click += new System.EventHandler(this.queryToolStripMenuItem_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.selectNoneToolStripMenuItem_Click);
            // 
            // invertSelectionToolStripMenuItem
            // 
            this.invertSelectionToolStripMenuItem.Name = "invertSelectionToolStripMenuItem";
            this.invertSelectionToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.invertSelectionToolStripMenuItem.Text = "Invert Selection";
            this.invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.invertSelectionToolStripMenuItem_Click);
            // 
            // exportSelectedFeaturesToolStripMenuItem
            // 
            this.exportSelectedFeaturesToolStripMenuItem.Name = "exportSelectedFeaturesToolStripMenuItem";
            this.exportSelectedFeaturesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.exportSelectedFeaturesToolStripMenuItem.Text = "Export Selected Features";
            this.exportSelectedFeaturesToolStripMenuItem.Click += new System.EventHandler(this.exportSelectedFeaturesToolStripMenuItem_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.importFieldDefinitionsFromDBFToolStripMenuItem,
            this.fieldCalculatorToolToolStripMenuItem,
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem,
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem});
            this.mnuTools.Name = "mnuTools";
            this.mnuTools.Size = new System.Drawing.Size(48, 20);
            this.mnuTools.Text = "Tools";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // importFieldDefinitionsFromDBFToolStripMenuItem
            // 
            this.importFieldDefinitionsFromDBFToolStripMenuItem.Name = "importFieldDefinitionsFromDBFToolStripMenuItem";
            this.importFieldDefinitionsFromDBFToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.importFieldDefinitionsFromDBFToolStripMenuItem.Text = "Append Field Definitions from DBF";
            this.importFieldDefinitionsFromDBFToolStripMenuItem.Click += new System.EventHandler(this.importFieldDefinitionsFromDBFToolStripMenuItem_Click);
            // 
            // fieldCalculatorToolToolStripMenuItem
            // 
            this.fieldCalculatorToolToolStripMenuItem.Name = "fieldCalculatorToolToolStripMenuItem";
            this.fieldCalculatorToolToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.fieldCalculatorToolToolStripMenuItem.Text = "Field Calculator Tool";
            this.fieldCalculatorToolToolStripMenuItem.Click += new System.EventHandler(this.fieldCalculatorToolToolStripMenuItem_Click);
            // 
            // generateOrUpdateMWShapeIDFieldsToolStripMenuItem
            // 
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem.Enabled = false;
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem.Name = "generateOrUpdateMWShapeIDFieldsToolStripMenuItem";
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem.Text = "Generate or Update MWShapeID Fields";
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem.Click += new System.EventHandler(this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem_Click);
            // 
            // copyShapeIDsToSpecifiedFieldToolStripMenuItem
            // 
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem.Name = "copyShapeIDsToSpecifiedFieldToolStripMenuItem";
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem.Text = "Copy Shape IDs to Specified Field...";
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem.Click += new System.EventHandler(this.copyShapeIDsToSpecifiedFieldToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.toolStrip);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(400, 26);
            this.panel2.TabIndex = 3;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSaveEdits,
            this.tsbtnZoomToSelected,
            this.tsbtnShowSelected,
            this.tsbtnImportFieldsFromDBF,
            this.tsbtnFieldCalculator,
            this.tsbtnRefreshMap,
            this.tsbtnRefresh,
            this.tsbtnQuery});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(400, 25);
            this.toolStrip.TabIndex = 8;
            this.toolStrip.Text = "ToolStrip";
            // 
            // tsbtnSaveEdits
            // 
            this.tsbtnSaveEdits.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSaveEdits.Image = global::MapWindow.Images.Disk;
            this.tsbtnSaveEdits.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSaveEdits.Name = "tsbtnSaveEdits";
            this.tsbtnSaveEdits.Size = new System.Drawing.Size(23, 22);
            this.tsbtnSaveEdits.Text = "toolStripButton1";
            this.tsbtnSaveEdits.ToolTipText = "Save Edits";
            this.tsbtnSaveEdits.Click += new System.EventHandler(this.tsbtnSaveEdits_Click);
            // 
            // tsbtnZoomToSelected
            // 
            this.tsbtnZoomToSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnZoomToSelected.Image = global::MapWindow.Properties.Resources.zoom;
            this.tsbtnZoomToSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnZoomToSelected.Name = "tsbtnZoomToSelected";
            this.tsbtnZoomToSelected.Size = new System.Drawing.Size(23, 22);
            this.tsbtnZoomToSelected.Text = "ZoomToSelected";
            this.tsbtnZoomToSelected.ToolTipText = "Zoom to Selected";
            this.tsbtnZoomToSelected.Click += new System.EventHandler(this.tsbtnZoomToSelected_Click);
            // 
            // tsbtnShowSelected
            // 
            this.tsbtnShowSelected.CheckOnClick = true;
            this.tsbtnShowSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnShowSelected.Image = global::MapWindow.Properties.Resources.table_edit;
            this.tsbtnShowSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnShowSelected.Name = "tsbtnShowSelected";
            this.tsbtnShowSelected.Size = new System.Drawing.Size(23, 22);
            this.tsbtnShowSelected.Text = "ShowSelected";
            this.tsbtnShowSelected.ToolTipText = "Show Selected";
            this.tsbtnShowSelected.Click += new System.EventHandler(this.tsbtnShowSelected_Click);
            // 
            // tsbtnImportFieldsFromDBF
            // 
            this.tsbtnImportFieldsFromDBF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnImportFieldsFromDBF.Image = global::MapWindow.Properties.Resources.down;
            this.tsbtnImportFieldsFromDBF.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnImportFieldsFromDBF.Name = "tsbtnImportFieldsFromDBF";
            this.tsbtnImportFieldsFromDBF.Size = new System.Drawing.Size(23, 22);
            this.tsbtnImportFieldsFromDBF.Text = "AppendFieldsFromDBF";
            this.tsbtnImportFieldsFromDBF.ToolTipText = "Append Fields From DBF";
            this.tsbtnImportFieldsFromDBF.Click += new System.EventHandler(this.tsbtnImportFieldsFromDBF_Click);
            // 
            // tsbtnFieldCalculator
            // 
            this.tsbtnFieldCalculator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnFieldCalculator.Image = global::MapWindow.Properties.Resources.calculator;
            this.tsbtnFieldCalculator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnFieldCalculator.Name = "tsbtnFieldCalculator";
            this.tsbtnFieldCalculator.Size = new System.Drawing.Size(23, 22);
            this.tsbtnFieldCalculator.Text = "FieldCalculator";
            this.tsbtnFieldCalculator.ToolTipText = "Field Calculator";
            this.tsbtnFieldCalculator.Click += new System.EventHandler(this.tsbtnFieldCalculator_Click);
            // 
            // tsbtnRefreshMap
            // 
            this.tsbtnRefreshMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRefreshMap.Image = global::MapWindow.Properties.Resources.color_scheme;
            this.tsbtnRefreshMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRefreshMap.Name = "tsbtnRefreshMap";
            this.tsbtnRefreshMap.Size = new System.Drawing.Size(23, 22);
            this.tsbtnRefreshMap.Text = "Refresh coloring scheme";
            this.tsbtnRefreshMap.Click += new System.EventHandler(this.tsbtnRefreshMap_Click);
            // 
            // tsbtnRefresh
            // 
            this.tsbtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRefresh.Image = global::MapWindow.Properties.Resources.refresh;
            this.tsbtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRefresh.Name = "tsbtnRefresh";
            this.tsbtnRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsbtnRefresh.Text = "Refresh Records to original";
            this.tsbtnRefresh.Click += new System.EventHandler(this.tsbtnRefresh_Click);
            // 
            // tsbtnQuery
            // 
            this.tsbtnQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnQuery.Image = global::MapWindow.Properties.Resources.script;
            this.tsbtnQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnQuery.Name = "tsbtnQuery";
            this.tsbtnQuery.Size = new System.Drawing.Size(23, 22);
            this.tsbtnQuery.Text = "Query";
            this.tsbtnQuery.Click += new System.EventHandler(this.tsbtnQuery_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridView1.Location = new System.Drawing.Point(0, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(400, 214);
            this.dataGridView1.TabIndex = 4;
            // 
            // TableEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "TableEditorControl";
            this.Size = new System.Drawing.Size(400, 300);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void dataGridView1_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (_attributeCache.EditRowIndex != dataGridView1.CurrentCell.RowIndex)
            {
                _attributeCache.EditRowIndex = dataGridView1.CurrentCell.RowIndex;
            }
            _attributeCache.EditRow[dataGridView1.Columns[e.ColumnIndex].Name] = e.Value;

        }

      

     

        void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if(_attributeCache.EditRow != null)
            {
                _attributeCache.SaveChanges();
            }
            _attributeCache.EditRowIndex = -1; // this also sets the EditRow to null;
        }

      
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Table editor control without any data
        /// </summary>
        public TableEditorControl()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new Table editor control for editing a feature layer's attribute values. This allows interaction
        /// with the map. If a row is selected in the Table the corresponding row is selected in the map
        /// </summary>
        /// <param name="layer">The symbolizer on the map</param>
        public TableEditorControl(IMapFeatureLayer layer)
        {
            FeatureLayer = layer;
 
            Configure();
        }

        private void Configure()
        {            
            InitializeComponent();
            _selectedRows = new List<int>();
            Load += TableEditorControlLoad;
            enableEditingToolStripMenuItem.CheckedChanged += EnableEditingToolStripMenuItemCheckedChanged;
          
            
        }

        void ParentForm_Shown(object sender, EventArgs e)
        {
            SetSelectionFromLayer();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            
        }

        void ParentForm_Closing(object sender, CancelEventArgs e)
        {
            if(_fidField != null)_featureLayer.DataSet.DataTable.Columns.Remove(_fidField);
        }

       


      
      
       
        void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = _attributeCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }


        //when first loaded
        void TableEditorControlLoad(object sender, EventArgs e)
        {
            if (ParentForm == null) return;
            ParentForm.FormClosed += ParentFormFormClosed;
        }

        //when the parent form is closed
        void ParentFormFormClosed(object sender, FormClosedEventArgs e)
        {
            _featureLayer = null;
            _table = null;
            Dispose();
        }

        

        //when the selected features are changed on the feature layer
        void SelectedFeaturesChanged(object sender, EventArgs e)
        {
            SetSelectionFromLayer();
            
        }

        private void SetSelectionFromLayer()
        {
            dataGridView1.SuspendLayout();
            if (_featureLayer == null) return;
            if (_ignoreSelectionChanged) return;
            _ignoreSelectionChanged = true;
            _ignoreTableSelectionChanged = true;
            if(!_featureLayer.EditMode)
            {
                FastDrawnState[] states = _featureLayer.DrawnStates;
                if (_featureLayer.DataSet.AttributesPopulated)
                {
                    _ignoreSelectionChanged = true;
                    _ignoreTableSelectionChanged = true;
                    dataGridView1.SuspendLayout();
                    if(states == null)
                    {
                        _ignoreSelectionChanged = false;
                        _ignoreTableSelectionChanged = false;
                        return;
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        int fid = (int) row.Cells[_fidField].Value;
                        row.Selected = states[fid].Selected;
                    }
                    dataGridView1.ResumeLayout();
                    _ignoreSelectionChanged = false;
                    _ignoreTableSelectionChanged = false;
                }
                else
                {
                    if (states == null)
                    {
                        return;
                    }
                    foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                    {
                        for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                        {
                            dataGridView1.Rows[row].Selected = states[row].Selected;
                        }
                    }

                }
            }
            else
            {
                IFeatureSelection fs = _featureLayer.Selection as IFeatureSelection;
                if (fs == null) return;
                if (_featureLayer.DataSet.AttributesPopulated)
                {
                    _ignoreSelectionChanged = true;
                    _ignoreTableSelectionChanged = true;
                    dataGridView1.SuspendLayout();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        int fid = (int)row.Cells[_fidField].Value;
                        row.Selected = fs.Filter.DrawnStates[_featureLayer.DataSet.Features[fid]].IsSelected;
                    }
                    dataGridView1.ResumeLayout();
                    _ignoreSelectionChanged = false;
                    _ignoreTableSelectionChanged = false;
                }
                else
                {
                    foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                    {
                        for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                        {
                            dataGridView1.Rows[row].Selected = fs.Filter.DrawnStates[_featureLayer.DataSet.Features[row]].IsSelected;
                        }
                    }

                }
               
            }
            _ignoreSelectionChanged = false;
            _ignoreTableSelectionChanged = false;
           dataGridView1.ResumeLayout();
            
        }

     


       
        

        #endregion

        #region Methods

        /// <summary>
        /// Zoom to selected rows (features)
        /// </summary>
        public void ZoomToSelected()
        {
            if (_featureLayer == null) return;
            if (_featureLayer.Selection.Count == 0) return;

            _featureLayer.ZoomToSelectedFeatures();
        }

        /// <summary>
        /// Zooms to the row which is being edited
        /// </summary>
        public void ZoomToEditedRow()
        {
            if (dataGridView1.CurrentRow == null) return;
            DataRowView drv = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;
            IFeature currentFeature = _featureLayer.DataSet.FeatureFromRow(drv.Row);
            MapFrame frame = _featureLayer.ParentMapFrame() as MapFrame;
            if (frame == null) return;
            IEnvelope env = currentFeature.Envelope.Copy();
                    
            if (env.Width == 0 || env.Height == 0)
            {
                env.ExpandBy(2.0, 2.0);
            }
            frame.Extents = env;
        }

        /// <summary>
        /// This assumes that the datarows displayed correspond to features in the data Table.
        /// </summary>
        /// <param name="features"></param>
        public void SelectFeatures(IEnumerable<IFeature> features)
        {
            // Prevent infinite recursion by checking to ensure that the update did not originate from here.
            if (_ignoreSelectionChanged) return;
            List<int> rows = new List<int>();
            foreach (IFeature f in features)
            {
                rows.Add(f.FID);
            }
            _ignoreSelectionChanged = true;
            dataGridView1.ClearSelection();
            foreach (int row in rows)
            {
                //dataGridView1.SelectedRows[row].Selected = true;
                //It should be full row of collectiion
                dataGridView1.Rows[row].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = row;
                
            }
            DisplaySelectedRowNumberLable(rows.Count);
            dataGridView1.Refresh();
            _ignoreSelectionChanged = false;
        }

        /// <summary>
        /// This will update the Label of Selected Number of rows.
        /// </summary>
        /// <param name="numRows"></param>
        public void DisplaySelectedRowNumberLable(int numRows)
        {
            lblSelectedNumber.Text = (numRows + " of " + (dataGridView1.RowCount-1) + " Selected");
        }

        /// <summary>
        /// This will update the Label of File path.
        /// </summary>
        /// <param name="file"></param>
        public void DisplayFilePathLabel(string file)
        {
            lblFilePath.Text = file;
        }

        #endregion

        #region Properties

        /// <summary>
        /// If set to true, only the selected rows are displayed.
        /// If set to false, all rows are displayed.
        /// </summary>
        public bool ShowSelectedRowsOnly
        {
            get 
            {
                return _showOnlySelectedRows;   
            }
            set
            {
                if (value == _showOnlySelectedRows) return;

                //if we are changing the property..
                if (value)
                {
                    ShowOnlySelectedRows();
                }
                else
                {
                    ShowAllRows();
                }
            }
        }

        /// <summary>
        /// Gets the source data Table
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTable SourceDataTable
        {
            get
            {
                return _table;
            }
        }

        

        private void FeatureLayerSetup()
        {
            if (_featureLayer == null) return;
            if (_featureLayer.DataSet.NumRows() < 50000)
            {
                _featureLayer.DataSet.FillAttributes();
            }
            
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                dataGridView1.VirtualMode = false;
                dataGridView1.AllowUserToOrderColumns = true;
                dataGridView1.DataSource = _featureLayer.DataSet.DataTable;
                if (_virtualHooked)
                {
                    dataGridView1.CellValueNeeded -= dataGridView1_CellValueNeeded;
                    dataGridView1.CellValuePushed -= dataGridView1_CellValuePushed;
                    dataGridView1.RowValidated -= dataGridView1_RowValidated;
                }
                AddFid(_featureLayer.DataSet.DataTable);
            }
            else
            {
                dataGridView1.VirtualMode = true;
                dataGridView1.AllowUserToOrderColumns = false;
                if (!_virtualHooked)
                {
                    dataGridView1.CellValueNeeded += dataGridView1_CellValueNeeded;
                    dataGridView1.CellValuePushed += dataGridView1_CellValuePushed;
                    dataGridView1.RowValidated += dataGridView1_RowValidated;
                    _virtualHooked = true;
                }
                _attributeCache = new AttributeCache(FeatureLayer.DataSet, 16);
                DataColumn[] columns = _featureLayer.DataSet.GetColumns();
                foreach (DataColumn field in columns)
                {
                    dataGridView1.Columns.Add(field.ColumnName, field.ColumnName);
                }
                dataGridView1.RowCount = _featureLayer.DataSet.NumRows();
            }
            _featureLayer.SelectionChanged += SelectedFeaturesChanged;
            
        }
        
     

        private void AddFid(DataTable table)
        {
            const string name = "FID";
            int i = 0;
            while(table.Columns.Contains(name + i))
            {
                i++;
            }
            _fidField = name + i;
            table.Columns.Add(_fidField, typeof (int));
            for(int row = 0; row < table.Rows.Count; row++)
            {
                table.Rows[row][_fidField] = row;
            }
        }

       

        /// <summary>
        /// Gets or sets the feature layer used by this data Table
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMapFeatureLayer FeatureLayer
        {
            get { return _featureLayer; }
            set
            {
                _featureLayer = value;
                FeatureLayerSetup();
                if (ParentForm == null || _loaded) return;
                ParentForm.Shown += ParentForm_Shown;
                ParentForm.Closing += ParentForm_Closing;
                _loaded = true;
            }
        }

      

        void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (_ignoreTableSelectionChanged) return;
            _ignoreSelectionChanged = true;
            
            if(_featureLayer.DataSet.AttributesPopulated)
            {
                if(!_featureLayer.EditMode)
                {
                    FastDrawnState[] states = _featureLayer.DrawnStates;
                    if(states == null)
                    {
                        _featureLayer.DrawnStatesNeeded = true;
                    }
                    states = _featureLayer.DrawnStates;
                    if(string.IsNullOrEmpty(_fidField) || !_featureLayer.DataSet.DataTable.Columns.Contains(_fidField))
                    {
                        _ignoreSelectionChanged = false;
                        return;
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        
                        int fid = (int)row.Cells[_fidField].Value;
                        states[fid].Selected = row.Selected;
                    }
                }
                else
                {
                    IFeatureSelection fs = _featureLayer.Selection as IFeatureSelection;
                    if(fs == null) return;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        int fid = (int)row.Cells[_fidField].Value;
                        IFeature f = _featureLayer.DataSet.Features[fid];
                        fs.Filter.DrawnStates[f].IsSelected = row.Selected;
                    }
                    
                }
                
               
            }
            else
            {
                List<int> adds = new List<int>();
                List<int> removes = _selectedRows.ToList();
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if(!_selectedRows.Contains(row.Index))
                    {
                        adds.Add(row.Index);
                    }
                    removes.Remove(row.Index);
                }
                _featureLayer.Select(adds);
                _featureLayer.UnSelect(removes);
            }
           
                
           
            _featureLayer.Invalidate();
            _ignoreSelectionChanged = false;
            
        }

        /// <summary>
        /// Gets the collection of selected data rows.  The row indices are
        /// 1 based instead of zero based, so be sure to subtract one before matching with a feature.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewSelectedRowCollection Selection
        {
            get { return dataGridView1.SelectedRows; }
        }

        /// <summary>
        /// Gets or sets the boolean that controls whether or not this form will throw an
        /// event during the selection changed process.
        /// </summary>
        public bool IgnoreSelectionChanged
        {
            get { return _ignoreSelectionChanged; }
            set { _ignoreSelectionChanged = value; }
        }

        /// <summary>
        /// set or get the relavant full featureset
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFeatureSet FeatureSetData
        {
            get { return _featureLayer.DataSet; }
        }

        /// <summary>
        /// gets or sets the visibility of the main menu strip
        /// </summary>
        public bool ShowMenuStrip
        {
            get { return menuStrip1.Visible; }
            set { menuStrip1.Visible = value; }
        }

        /// <summary>
        /// gets or sets the visibility of the main tool strip
        /// </summary>
        public bool ShowToolStrip
        {
            get { return toolStrip.Visible; }
            set { toolStrip.Visible = value; }
        }

        /// <summary>
        /// Gets or sets if the file path is shown in the status bar
        /// </summary>
        public bool ShowFileName
        {
            get 
            { 
                return lblFilePath.Visible; 
            }
            set 
            {
                if (value)
                {
                    progressBar.Visible = false;
                    lblFilePath.Visible = true;
                }
                else
                {
                    progressBar.Visible = true;
                    lblFilePath.Visible = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets if the progress bar is visible
        /// </summary>
        public bool ShowProgressBar
        {
            get { return progressBar.Visible; }
            set 
            {
                if (value == progressBar.Visible) return;
                if (value)
                {
                    progressBar.Visible = true;
                    lblFilePath.Visible = false;
                }
                else
                {
                    progressBar.Visible = false;
                    lblFilePath.Visible = true;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether the Table is editable by the user
        /// </summary>
        public bool IsEditable
        {
            get { return _isEditable; }
            set 
            {
                _isEditable = value;
                SetEditableIcons();
            }
        }

        #endregion

        #region Event Handlers

        //when the 'Enable editing' menu is checked or unchecked
        void EnableEditingToolStripMenuItemCheckedChanged(object sender, EventArgs e)
        {
            if (enableEditingToolStripMenuItem.Checked)
            {
                IsEditable = true;
                //dataGridView1.ReadOnly = false;
            }
            else
            {
                IsEditable = false;
                //dataGridView1.ReadOnly = true;
            }
        }


        //execute query
        void tsbtnQuery_Click(object sender, EventArgs e)
        {
            if (QueryExe() == false)
                MessageBox.Show("Could not Execute the Query");
        }

        //reload the data source
        void tsbtnRefresh_Click(object sender, EventArgs e)
        {
            //ReloadDataSource();
        }

        //refresh the map
        void tsbtnRefreshMap_Click(object sender, EventArgs e)
        {
            OnRefreshMap();
        }

        //start field calculator
        void tsbtnFieldCalculator_Click(object sender, EventArgs e)
        {
            FieldCalculationExecute();
        }

        void tsbtnImportFieldsFromDBF_Click(object sender, EventArgs e)
        {
            if (ImportFieldsFromDbf() == false)
                MessageBox.Show("Could not Import Column fields");
        }

        //limit the display to selected rows only
        void tsbtnShowSelected_Click(object sender, EventArgs e)
        {
            if (Equals(tsbtnShowSelected.Checked, true))
            {
                ShowOnlySelectedRows();
            }
            else
            {
                ShowAllRows();
            }
        }

        //zoom to selected rows
        void tsbtnZoomToSelected_Click(object sender, EventArgs e)
        {
            ZoomToSelected();
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the SelectionChanged event whenver the selection on this dialog has been altered
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            if (_ignoreSelectionChanged) return;

            //when selection is changed by the user..

            
            _ignoreSelectionChanged = true;
            ShowSelectedRowCount();

            if (SelectionChanged != null)
            {
                SelectionChanged(this, new EventArgs());
            }
            _ignoreSelectionChanged = false;
        }

        /// <summary>
        /// Fires the SelectioinZoom event whenver the selectionZoom button click
        /// </summary>
        protected virtual void OnSelectionZoom()
        {
            if (_ignoreSelectionChanged) return;

            _ignoreSelectionChanged = true;
            //if (tsbtnZoomToSelected.Click != null) 
            SelectionZoom(this, new EventArgs());
            _ignoreSelectionChanged = false;
        }


        /// <summary>
        /// Fires the OnRefreshMap event whenver the RefreshMap button click
        /// </summary>
        protected virtual void OnRefreshMap()
        {
            if (_ignoreSelectionChanged) return;
            _ignoreSelectionChanged = true;

            //Call the event Handler
            MapRefreshed(this, new EventArgs());
            _ignoreSelectionChanged = false;
        }


        /// <summary>
        /// Fires the OnFieldCalculation event whenver the zoomToShapeBeingEdited menu click
        /// </summary>
        protected virtual void OnzoomToShapeBeingEdited()
        {
            if (_ignoreSelectionChanged) return;
            _ignoreSelectionChanged = true;

            //Call the event Handeler

            ZoomToShapeBeingEdited(this, new EventArgs());
            _ignoreSelectionChanged = false;
        }

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

        #region ToolStripMenuItem_Click Events

        //add field
        private void addFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateNewColumn() == false)
            {
                MessageBox.Show("Could not create new field");
            }
            SelectNone();
        }

        //remove field
        private void removeFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> field = new List<string>();
            List<string> selectedField = new List<string>();

            //collect the field
            DataTable dt = _table;
            if (dt == null) return;
            foreach (DataColumn dc in dt.Columns)
            {
                field.Add(dc.ToString());
            }
            frmDeleteField frmDel = new frmDeleteField(field);
            if (frmDel.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show(MessageStrings.RemoveFields, MessageStrings.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    selectedField = frmDel.SelectedFieldIdList;
            }
            dataGridView1.SuspendLayout();
            foreach (string fi in selectedField)
            {
                dt.Columns.Remove(fi);
            }
            
            dataGridView1.ResumeLayout();
            SelectNone();
        }

        //rename field
        private void renameFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //collect the field
            List<string> field = new List<string>();
            DataTable dt = _table;
            if (dt == null) return;
            foreach (DataColumn dc in dt.Columns)
                field.Add(dc.ToString());

            ReNameField reNameField = new ReNameField(field);
            if (reNameField.ShowDialog() != DialogResult.OK) return;
            int index = dt.Columns.IndexOf(reNameField.ResultCombination[0]);
            dt.Columns[index].ColumnName = reNameField.ResultCombination[1];//rename column
            SelectNone();
        }

        //show only selected rows (selected features)
        private void showOnlySelectedShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showOnlySelectedShapesToolStripMenuItem.Checked)
            {
                ShowOnlySelectedRows();
            }
            else
            {
                ShowAllRows();
            }
        }

        //zoom to selected shapes
        private void zoomToSelectedShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomToSelected();
        }

        //zoom to shape being edited (corresponding to current row)
        private void zoomToShapeBeingEditedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            ZoomToEditedRow();
        }

        private void flashSelectedShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void queryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (QueryExe() == false)
                MessageBox.Show(MessageStrings.CouldNotExecuteQuery);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
            OnSelectionChanged();
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectNone();
            OnSelectionChanged();
        }

        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvertSelection();
            OnSelectionChanged();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox inbox = new InputBox(MessageStrings.EnterFindString, MessageStrings.Find);
            if (inbox.ShowDialog() != DialogResult.OK) return;
           
            string exp = inbox.Result;
            if (exp == null) return;
            if (FindString(exp) == false)
                MessageBox.Show(MessageStrings.NoMatch);

        }

        //Export selected features
        private void exportSelectedFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sdlg = new SaveFileDialog();
            sdlg.Filter = "Shapefiles (*.shp )|*.SHP";
            if (sdlg.ShowDialog() == DialogResult.OK)
            {
                _featureLayer.ExportSelection(sdlg.FileName);
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Replace frmReplace = new Replace();
            if (frmReplace.ShowDialog() != DialogResult.OK) return;
            string findString = frmReplace.FindString;
            string replaceString = frmReplace.ReplaceString;
            if (ReplaceString(findString, replaceString) == false)
                MessageBox.Show(MessageStrings.CouldNotFindReplace);
        }

        private void importFieldDefinitionsFromDBFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImportFieldsFromDbf() == false)
                MessageBox.Show(MessageStrings.FieldImportFailed);
        }

        private void fieldCalculatorToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FieldCalculationExecute();
        }

        private void generateOrUpdateMWShapeIDFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void copyShapeIDsToSpecifiedFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> field = new List<string>();
            
            bool isNew = false;
            if (_table == null) return;
            foreach (DataColumn dc in _table.Columns)
                field.Add(dc.ToString());
            SelectField frmSeFie = new SelectField(field);
            if (frmSeFie.ShowDialog() == DialogResult.OK)
            {
                string fieldName = frmSeFie.FieldName;
                int index = _table.Columns.IndexOf(frmSeFie.FieldName);
                if (index == -1)
                    isNew = true;
                if(CopyFid(fieldName,isNew)==false)
                    MessageBox.Show("Could not copy FID");
            }
        }

        //save edits
        private void tsbtnSaveEdits_Click(object sender, EventArgs e)
        {
            SaveEdits();
        }

        private void saveEditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveEdits();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// when 'IsEditable' is set to false, some toolbar icons are
        /// hidden
        /// </summary>
        private void SetEditableIcons()
        {
            if (_isEditable)
            {
                tsbtnSaveEdits.Visible = true;
                tsbtnFieldCalculator.Visible = true;
                tsbtnImportFieldsFromDBF.Visible = true;
                tsbtnRefreshMap.Visible = true;

                saveEditsToolStripMenuItem.Enabled = true;
                addFieldToolStripMenuItem.Enabled = true;
                removeFieldToolStripMenuItem.Enabled = true;
                renameFieldToolStripMenuItem.Enabled = true;
                fieldCalculatorToolToolStripMenuItem.Enabled = true;
                importFieldDefinitionsFromDBFToolStripMenuItem.Enabled = true;
                copyShapeIDsToSpecifiedFieldToolStripMenuItem.Enabled = true;

                dataGridView1.ReadOnly = false;
            }
            else
            {
                tsbtnSaveEdits.Visible = false;
                tsbtnFieldCalculator.Visible = false;
                tsbtnImportFieldsFromDBF.Visible = false;
                tsbtnRefreshMap.Visible = false;

                saveEditsToolStripMenuItem.Enabled = false;
                addFieldToolStripMenuItem.Enabled = false;
                removeFieldToolStripMenuItem.Enabled = false;
                renameFieldToolStripMenuItem.Enabled = false;
                fieldCalculatorToolToolStripMenuItem.Enabled = false;
                importFieldDefinitionsFromDBFToolStripMenuItem.Enabled = false;
                copyShapeIDsToSpecifiedFieldToolStripMenuItem.Enabled = false;

                dataGridView1.ReadOnly = true;
            }
        }

       

        //Shows all rows (both selected and unselected)
        private void ShowAllRows()
        {
            _showOnlySelectedRows = false;
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                _ignoreTableSelectionChanged = true;
                _ignoreSelectionChanged = true;
                dataGridView1.DataSource = _featureLayer.DataSet.DataTable;
                foreach (int row in _selectedRows)
                {
                    dataGridView1.Rows[row].Selected = true;
                }
                _ignoreTableSelectionChanged = false;
                _ignoreSelectionChanged = false;
            }
            else
            {
                _attributeCache = new AttributeCache(_featureLayer.DataSet, 16);
                dataGridView1.RowCount = _featureLayer.DrawnStates.Length;
            }
            
            Refresh();
        }

       
        
        //Limits the displayed rows only to rows which are selected
        private void ShowOnlySelectedRows()
        {
            if(_featureLayer.DataSet.AttributesPopulated)
            {
                int numRows = _featureLayer.DataSet.DataTable.Rows.Count;
                dataGridView1.SuspendLayout();
                _selection = new DataTable();
                _selection.Columns.AddRange(_featureLayer.DataSet.GetColumns());
                _selection.Columns.Add(_fidField, typeof (int));
                if(_selectionIndices == null)_selectionIndices = new List<int>();
                _selectionIndices.Clear();
                for (int row = 0; row < numRows; row++)
                {
                    if (!_featureLayer.DrawnStates[row].Selected) continue;
                    DataRow dr = _selection.NewRow();
                    dr.ItemArray = _featureLayer.DataSet.DataTable.Rows[row].ItemArray;
                    _selection.Rows.Add(dr);
                    _selectionIndices.Add(row);
                }
                dataGridView1.DataSource = _selection;
                dataGridView1.SelectAll();
                dataGridView1.ResumeLayout();
            }
            else
            {
                _attributeCache = new AttributeCache(_featureLayer.Selection, 16);
                dataGridView1.Rows.Clear(); // without this setting rowCount takes a looooong time
                dataGridView1.RowCount = _featureLayer.Selection.Count;
            }
            _showOnlySelectedRows = true;
            Refresh();
        }

        private void FieldCalculationExecute()
        {
            AttributeCalculator attributeCal = new AttributeCalculator();
            attributeCal.Show();
            attributeCal.FeatureSet = _featureLayer.DataSet;
            List<string> fieldList = new List<string>();
            foreach (DataColumn dataCol in _featureLayer.DataSet.DataTable.Columns)
                fieldList.Add(dataCol.ToString());

            attributeCal.LoadTableField(fieldList);//Load all fields
            attributeCal.NewFieldAdded += AttributeCalNewFieldAdded;//when user click new field to put the calulated values.

        }

        void AttributeCalNewFieldAdded(object sender, EventArgs e)
        {
            AttributeCalculator attributeCal = sender as AttributeCalculator;
            if (attributeCal != null) _featureLayer.DataSet = attributeCal.FeatureSet;
        }

        //shows the number of selected rows
        private void ShowSelectedRowCount()
        {
            if(_featureLayer == null) return;
            int numTotal = _featureLayer.DataSet.NumRows();
            int numSelected = _featureLayer.Selection.Count;
            string msg = "Selected: " + numSelected + " / " + numTotal;
            lblSelectedNumber.Text = msg;
        }

        //saves edits to the data Table
        private void SaveEdits()
        {
            try
            {
                dataGridView1.SuspendLayout();

                //remove temporary columns
               
                _featureLayer.DataSet.Save();

                dataGridView1.ResumeLayout();

                //ReloadDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save edits. " + ex.Message);
            }
        }

        private bool ImportFieldsFromDbf()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "DBase Files (*.dbf)|*.DBF";
            FeatureSet fs = new FeatureSet();
            FeatureSet fsTemp = new FeatureSet();
            fsTemp.CopyFeatures(_featureLayer.DataSet, true);
            if (dlg.ShowDialog() != DialogResult.OK)
                return false;
            string shapeFilePath2 = dlg.FileName;
            int count = shapeFilePath2.Length;
            shapeFilePath2 = shapeFilePath2.Remove(count - 4, 4);//remove the extension of the file
            shapeFilePath2 = shapeFilePath2 + ".shp";//add 
            fs.Open(shapeFilePath2);
            
            int noOfCol = fs.DataTable.Columns.Count;
            //Add all column header
            for (int i = 0; i < noOfCol; i++)
            {
                DataColumn dtcol = new DataColumn(fs.DataTable.Columns[i].ColumnName, fs.DataTable.Columns[i].DataType);
                if (fsTemp.DataTable.Columns.Contains(fs.DataTable.Columns[i].ColumnName) == false)
                    fsTemp.DataTable.Columns.Add(dtcol);
            }
            dataGridView1.DataSource = fsTemp.DataTable;
            return true;
        }


        //Executes a query
        private bool QueryExe()
        {      
            QueryDialog sqlFrm = new QueryDialog();
            sqlFrm.Table = _table;//Set Table
            
            string resultExpresion = null;
            if (sqlFrm.ShowDialog() == DialogResult.OK)
            {
                resultExpresion = sqlFrm.Expression;//get experession
            }
            if (resultExpresion != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                _featureLayer.SelectByAttribute(resultExpresion);//execute expression
                Cursor.Current = Cursors.Default;
            }
            else
            {
                return false;
            }

            return true;
        }

        //add a new field (column)
        private bool CreateNewColumn()
        {
            if (_table == null) return false;
            
            AddNewColum addCol = new AddNewColum();
            
            if (addCol.ShowDialog() != DialogResult.OK) return false;
            _table.Columns.Add(addCol.Name, addCol.Type);
            dataGridView1.ClearSelection();
            return true;
        }

        //select all features in the Table and map
        private void SelectAll()
        {
            if(_showOnlySelectedRows)
            {
                if (_featureLayer != null)
                {
                    if (_featureLayer.DataSet.AttributesPopulated)
                    {
                        _ignoreSelectionChanged = true;
                        _ignoreTableSelectionChanged = true;
                        List<int> fids = new List<int>();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            row.Selected = true;
                            fids.Add((int)row.Cells[_fidField].Value);
                        }
                        _featureLayer.Select(fids);
                        _ignoreSelectionChanged = false;
                        _ignoreTableSelectionChanged = false;

                    }
                    else
                    {
                        if (_showOnlySelectedRows) dataGridView1.RowCount = _featureLayer.DataSet.NumRows();
                        foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                        {
                            for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                            {
                                dataGridView1.Rows[row].Selected = true;
                            }
                        }
                    }
                }

                return;
            }
            _ignoreSelectionChanged = true;
            _selectedRows = new List<int>();
            for (int i = 0; i < _featureLayer.DataSet.NumRows(); i++ )
            {
                _selectedRows.Add(i);
            }
            if (_featureLayer != null)
            {
                _featureLayer.SelectAll();
                _featureLayer.Invalidate();
                 _ignoreTableSelectionChanged = true;
                if(_featureLayer.DataSet.AttributesPopulated)
                {
                   
                    dataGridView1.SelectAll();
                   
                }
                else
                {
                    if (_showOnlySelectedRows) dataGridView1.RowCount = _featureLayer.DataSet.NumRows();
                    foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                    {
                        for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                        {
                            dataGridView1.Rows[row].Selected = true;
                        }
                    }
                }
                 _ignoreTableSelectionChanged = false;
            }
            
            _ignoreSelectionChanged = false;
            
        }

        //unselect all features in the Table and map
        private void SelectNone()
        {
            
            _ignoreSelectionChanged = true;
            _selectedRows = new List<int>();
            _ignoreTableSelectionChanged = true;
            if (_featureLayer != null)
            {
                
                _featureLayer.ClearSelection();
                if(!_featureLayer.DataSet.AttributesPopulated)
                {
                    foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                    {
                        for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                        {
                            dataGridView1.Rows[row].Selected = false;
                        }
                    }
                }
                else
                {
                    dataGridView1.ClearSelection();
                }
                
            }
            _ignoreTableSelectionChanged = false;
            _ignoreSelectionChanged = false;
            
        }

        //invert selection
        private void InvertSelection()
        {
            if (_ignoreSelectionChanged) return;

            _ignoreSelectionChanged = true;
            if (_featureLayer != null)
            {
                if (_featureLayer.DataSet.AttributesPopulated)
                {
                    if(string.IsNullOrEmpty(_fidField) || !_featureLayer.DataSet.DataTable.Columns.Contains(_fidField))return; 
                    
                    _ignoreTableSelectionChanged = true;
                    List<int> adds = new List<int>();
                    List<int> removes = new List<int>();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        bool sel = !row.Selected;
                        row.Selected = sel;
                        if(sel)
                        {
                            adds.Add((int)row.Cells[_fidField].Value);
                        }
                        else
                        {
                            removes.Add((int)row.Cells[_fidField].Value);
                        }
                    }
                    _featureLayer.Selection.SuspendChanges();
                    _featureLayer.Select(adds);
                    _featureLayer.UnSelect(removes);
                    _featureLayer.Selection.ResumeChanges();
                    _ignoreTableSelectionChanged = false;
                }
                else
                {
                    _featureLayer.InvertSelection();
                    _featureLayer.Invalidate();
                    foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                    {
                        for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                        {
                            dataGridView1.Rows[row].Selected = _featureLayer.DrawnStates[row].Selected;
                        }
                    }
                }

                
            }
            
            _ignoreSelectionChanged = false;
 
        }

        /// <summary>
        /// Will find the string in the attribute Table (Search Operation)
        /// </summary>
        /// <param name="exp">the string to be found</param>
        /// <returns>true if the string was found, false otherwise</returns>
        private bool FindString(string exp)
        {
            if (exp == null) return false;

            exp.Trim();
            exp = exp.ToLower();

            string expression = BuildFindExpression(exp);
            _featureLayer.SelectByAttribute(expression);

            return _featureLayer.Selection.Count != 0;
        }

        /// <summary>
        /// Builds a 'find' select expression to find a string
        /// </summary>
        /// <param name="findString"></param>
        /// <returns></returns>
        private string BuildFindExpression(string findString)
        {
            List<string> conditions = new List<string>();
            DataColumn[] columns = _featureLayer.DataSet.GetColumns();
            //for each column in the data grid view
            foreach (Field col in columns)
            {
                //if it's a string column, enclose it in double quotes and
                //use the Like expression
                if (col.DataType != typeof (string)) continue;
                string condition = "[" + col.ColumnName + "] LIKE '" + findString + "'";
                conditions.Add(condition);
                //else
                //{
                //    string condition = "[" + col.ColumnName + "] = '" + findString + "'";
                //    conditions.Add(condition);
                //}
            }

            StringBuilder stb = new StringBuilder();
            for(int i=0; i< conditions.Count; i++)
            {
                stb.Append(conditions[i]);
                if (i < conditions.Count - 1)
                {
                    stb.Append(" OR ");
                }
            }

            return stb.ToString();
        }


        /// <summary>
        /// Will find the string in the dataGridView and replace
        /// </summary>
        /// <param name="exp">Find expression string</param>
        /// <param name="expReplace">replace expression string</param>
        /// <returns></returns>
        private bool ReplaceString(string exp, string expReplace)
        {
            if (exp == null) return false;
            exp.Trim();
            exp = exp.ToLower();
            bool rowFiended = false;
            string dgExp;
            int numRow = dataGridView1.RowCount;
            int numCol = dataGridView1.ColumnCount;
            progressBar.Visible = true;
            progressBar.Minimum = 0;
            progressBar.Maximum = numRow - 1;
            progressBar.Value = 1;
            progressBar.Step = 1;

            int category;
            if (exp.IndexOf("*", 0) == 0)//stating with "*"
            {
                category = 1;
                exp = exp.Remove(0, 1);
            }
            else if (exp.IndexOf("*", exp.Length - 1) == exp.Length - 1)//ending with "*"
            {
                category = 2;
                exp = exp.Remove(exp.Length - 1, 1);
            }
            else//take as normal case
                category = 0;

            for (int r = 0; r < numRow; r++)
            {
                //******Progress Bar
                progressBar.PerformStep();

                for (int c = 0; c < numCol; c++)
                {
                    if (dataGridView1[c, r].Value == null) continue;
                    dgExp = dataGridView1[c, r].Value.ToString();//cell value
                    dgExp = dgExp.ToLower();
                    bool itemReplaced = false;
                    if (category == 1)//stating with "*"
                    {
                        if (dgExp.EndsWith(exp))//check it occur at the end
                        {
                            dataGridView1.Rows[r].Selected = true;
                            rowFiended = true;
                            itemReplaced = true;
                        }
                    }
                    else if (category == 2)//ending with "*"
                    {
                        if (dgExp.StartsWith(exp))//check it occur at the begining
                        {
                            dataGridView1.Rows[r].Selected = true;
                            rowFiended = true;
                            itemReplaced = true;
                        }
                    }
                    else//take as normal case
                    {
                        if (dgExp == exp)//check it occur exacly same work
                        {
                            dataGridView1.Rows[r].Selected = true;
                            rowFiended = true;
                            itemReplaced = true;
                        }
                    }
                    //Replace the values
                    if (itemReplaced)
                    {
                        if (dataGridView1[c, r].ValueType == typeof(string))
                            dataGridView1[c, r].Value = expReplace;
                        else if (dataGridView1[c, r].ValueType == typeof(double))
                            dataGridView1[c, r].Value = Convert.ToDouble(expReplace);
                        else if (dataGridView1[c, r].ValueType == typeof(int))
                            dataGridView1[c, r].Value = Convert.ToInt32(expReplace);
                        else
                        {
                            MessageBox.Show("Unfamilar data type to replace");
                            return false;
                        }
                    }
                }
            }
            //OnSelectionChanged();
            dataGridView1.Refresh();
            return rowFiended;
        }

        /// <summary>
        /// This will copy the FID of features to given column
        /// </summary>
        /// <param name="field">the field where FID should be copied</param>
        /// <param name="isNewField">if isNewField is true, a new field will be added</param>
        /// <returns>true if successful</returns>
        private bool CopyFid(string field, bool isNewField)
        {
            if (field == null) return false;
            int colIndex = -1;
            if (isNewField) //create new field
            {
                
                _table.Columns.Add(field, typeof(int));
                colIndex = _table.Columns.Count - 1;
                dataGridView1.Refresh();
            }
            else
            {
                //use existing field (column)
                foreach (DataColumn dtCol in _table.Columns)//Take Column Index
                {
                    if (field.ToLower() == dtCol.ColumnName.ToLower())
                    {
                        colIndex = dtCol.Ordinal;
                    }
                }
                if (colIndex == -1)
                {
                    MessageBox.Show("Could not find the field in the attribute Table colums");
                    return false;
                }
            }

            IFeatureSet fs = _featureLayer.DataSet;
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                //assign the values
                DataRow r = _table.Rows[i];
                IFeature f = fs.FeatureFromRow(r);
                _table.Rows[i][colIndex] = f.FID;
            }
            SelectNone();

            return true;
        }

        
        #endregion

     

    }
}

