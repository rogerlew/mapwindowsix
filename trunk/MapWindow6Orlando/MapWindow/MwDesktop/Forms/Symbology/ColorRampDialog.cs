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
// The Initial Developer of this Original Code is Ted Dunsford. Created February 28, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
// 9/21/09: Chris Wilson - reformatted form for consistency
//********************************************************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Main;
using MapWindow.Drawing;
using MapWindow.Components;
namespace MapWindow.Forms
{
    /// <summary>
    /// A form for selecting colors
    /// </summary>
    public class RampColorDialog : Form
    {

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Designer variables
        private System.Windows.Forms.GroupBox grpLowColor;
        private System.Windows.Forms.GroupBox grpHighColor;
        private System.Windows.Forms.Label lblOrFrom1;
        private System.Windows.Forms.LinkLabel llLowColor;
        private System.Windows.Forms.Label lblLowColor;
        private System.Windows.Forms.LinkLabel llHighColor;
        private System.Windows.Forms.Label lblHighColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.Label lblPreview;
        private Label lblNumBreaks;
        private TextBox txtNumBreaks;
        private CheckBox chkContinuous;
        private ToolTip tipColorRamp;
        private TabControl tabRampStyle;
        private TabPage tabTwoColorPage;
        private TabPage tabHSB;
        private RadioButton radBrightness;
        private RadioButton radSaturation;
        private RadioButton radHue;
        private GroupBox grpBaseColor;
        private Label label2;
        private LinkLabel llBaseColor;
        private Label lblBaseColor;
        private RadioButton radDescending;
        private RadioButton radAscending;
        private GroupBox grbOrder;
        private MapWindow.Components.ColorDropDown cddLowColor;
        private MapWindow.Components.ColorDropDown cddHighColor;
        private MapWindow.Components.ColorDropDown cddBaseColor;
        private Label lblMaximum;
        private Label lblMinimum;
        private DialogButtons dialogButtons1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;

        private IColorRampBuilder _colorRampBuilder;
        
       

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of this form.
        /// </summary>
        public RampColorDialog()
        {
            InitializeComponent();

            _colorRampBuilder = new RasterSymbolizer(); // Even if 

            lblBaseColor.BackColor = Color.Gray;
            lblLowColor.BackColor = Color.DarkGreen;
            lblHighColor.BackColor = Color.Wheat;

            lblLowColor.Click += lblLowColor_Click;
            lblHighColor.Click += lblHighColor_Click;
            dialogButtons1.OkClicked += btnOk_Click;
            dialogButtons1.CancelClicked += btnCancel_Click;
            dialogButtons1.ApplyClicked += btnApply_Click;
            lblPreview.Paint += lblPreview_Paint;
            cddLowColor.SelectedIndexChanged += cddLowColor_SelectedIndexChanged;
            cddHighColor.SelectedIndexChanged += cddHighColor_SelectedIndexChanged;
            cddBaseColor.SelectedIndexChanged += CddBaseColorSelectedIndexChanged;
           
            lblBaseColor.Click += lblBaseColor_Click;
            
        }

       

      

        /// <summary>
        /// Constructs a new instance and sets it up for a specific color break
        /// </summary>
        /// <param name="colorRampBuilder"></param>
        public RampColorDialog(IColorRampBuilder colorRampBuilder)
            : this()
        {
            _colorRampBuilder = colorRampBuilder;
            if(_colorRampBuilder.ColorBreaks == null)
            {
                _colorRampBuilder.ColorBreaks = new ColorBreakList();
            }



            if (_colorRampBuilder.ColorBreaks.Count > 0)
            {
                lblBaseColor.BackColor = _colorRampBuilder.ColorBreaks[0].LowColor;
                lblLowColor.BackColor = _colorRampBuilder.ColorBreaks[0].LowColor;
                if (_colorRampBuilder.ColorBreaks.Count > 1)
                {
                    lblHighColor.BackColor = _colorRampBuilder.ColorBreaks[_colorRampBuilder.ColorBreaks.Count - 1].HighColor;
                }
                else
                {
                    lblHighColor.BackColor = lblLowColor.BackColor;
                }
            }
            else
            {
                lblBaseColor.BackColor = Color.Gray;
                lblLowColor.BackColor = Color.DarkGreen;
                lblHighColor.BackColor = Color.Wheat;
            }
           
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RampColorDialog));
            this.grpLowColor = new System.Windows.Forms.GroupBox();
            this.cddLowColor = new MapWindow.Components.ColorDropDown();
            this.lblOrFrom1 = new System.Windows.Forms.Label();
            this.llLowColor = new System.Windows.Forms.LinkLabel();
            this.lblLowColor = new System.Windows.Forms.Label();
            this.grpHighColor = new System.Windows.Forms.GroupBox();
            this.cddHighColor = new MapWindow.Components.ColorDropDown();
            this.label1 = new System.Windows.Forms.Label();
            this.llHighColor = new System.Windows.Forms.LinkLabel();
            this.lblHighColor = new System.Windows.Forms.Label();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.lblMaximum = new System.Windows.Forms.Label();
            this.lblMinimum = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();
            this.lblNumBreaks = new System.Windows.Forms.Label();
            this.txtNumBreaks = new System.Windows.Forms.TextBox();
            this.chkContinuous = new System.Windows.Forms.CheckBox();
            this.tipColorRamp = new System.Windows.Forms.ToolTip(this.components);
            this.radHue = new System.Windows.Forms.RadioButton();
            this.radSaturation = new System.Windows.Forms.RadioButton();
            this.radAscending = new System.Windows.Forms.RadioButton();
            this.radDescending = new System.Windows.Forms.RadioButton();
            this.tabRampStyle = new System.Windows.Forms.TabControl();
            this.tabTwoColorPage = new System.Windows.Forms.TabPage();
            this.tabHSB = new System.Windows.Forms.TabPage();
            this.grbOrder = new System.Windows.Forms.GroupBox();
            this.radBrightness = new System.Windows.Forms.RadioButton();
            this.grpBaseColor = new System.Windows.Forms.GroupBox();
            this.cddBaseColor = new MapWindow.Components.ColorDropDown();
            this.label2 = new System.Windows.Forms.Label();
            this.llBaseColor = new System.Windows.Forms.LinkLabel();
            this.lblBaseColor = new System.Windows.Forms.Label();
            this.dialogButtons1 = new MapWindow.Components.DialogButtons();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpLowColor.SuspendLayout();
            this.grpHighColor.SuspendLayout();
            this.grpPreview.SuspendLayout();
            this.tabRampStyle.SuspendLayout();
            this.tabTwoColorPage.SuspendLayout();
            this.tabHSB.SuspendLayout();
            this.grbOrder.SuspendLayout();
            this.grpBaseColor.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpLowColor
            // 
            this.grpLowColor.Controls.Add(this.cddLowColor);
            this.grpLowColor.Controls.Add(this.lblOrFrom1);
            this.grpLowColor.Controls.Add(this.llLowColor);
            this.grpLowColor.Controls.Add(this.lblLowColor);
            this.grpLowColor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpLowColor.Location = new System.Drawing.Point(6, 6);
            this.grpLowColor.Name = "grpLowColor";
            this.grpLowColor.Size = new System.Drawing.Size(295, 87);
            this.grpLowColor.TabIndex = 0;
            this.grpLowColor.TabStop = false;
            this.grpLowColor.Text = "Low Value Color";
            // 
            // cddLowColor
            // 
            this.cddLowColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cddLowColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cddLowColor.FormattingEnabled = true;
            this.cddLowColor.Items.AddRange(new object[] {
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.Color.Empty});
            this.cddLowColor.Location = new System.Drawing.Point(108, 50);
            this.cddLowColor.Name = "cddLowColor";
            this.cddLowColor.Size = new System.Drawing.Size(181, 22);
            this.cddLowColor.TabIndex = 3;
            this.cddLowColor.Value = System.Drawing.Color.Empty;
            // 
            // lblOrFrom1
            // 
            this.lblOrFrom1.AutoSize = true;
            this.lblOrFrom1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrFrom1.Location = new System.Drawing.Point(112, 34);
            this.lblOrFrom1.Name = "lblOrFrom1";
            this.lblOrFrom1.Size = new System.Drawing.Size(93, 13);
            this.lblOrFrom1.TabIndex = 2;
            this.lblOrFrom1.Text = "or select by name:";
            // 
            // llLowColor
            // 
            this.llLowColor.AutoSize = true;
            this.llLowColor.Location = new System.Drawing.Point(112, 18);
            this.llLowColor.Name = "llLowColor";
            this.llLowColor.Size = new System.Drawing.Size(124, 13);
            this.llLowColor.TabIndex = 0;
            this.llLowColor.TabStop = true;
            this.llLowColor.Text = "Select from color palette";
            this.tipColorRamp.SetToolTip(this.llLowColor, "Left Click to edit Start Color");
            this.llLowColor.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llLowColor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llLowColor_LinkClicked);
            // 
            // lblLowColor
            // 
            this.lblLowColor.BackColor = System.Drawing.Color.White;
            this.lblLowColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLowColor.Location = new System.Drawing.Point(26, 28);
            this.lblLowColor.Name = "lblLowColor";
            this.lblLowColor.Size = new System.Drawing.Size(42, 39);
            this.lblLowColor.TabIndex = 1;
            this.tipColorRamp.SetToolTip(this.lblLowColor, "Right Click to edit.");
            // 
            // grpHighColor
            // 
            this.grpHighColor.Controls.Add(this.cddHighColor);
            this.grpHighColor.Controls.Add(this.label1);
            this.grpHighColor.Controls.Add(this.llHighColor);
            this.grpHighColor.Controls.Add(this.lblHighColor);
            this.grpHighColor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpHighColor.Location = new System.Drawing.Point(6, 112);
            this.grpHighColor.Name = "grpHighColor";
            this.grpHighColor.Size = new System.Drawing.Size(295, 87);
            this.grpHighColor.TabIndex = 1;
            this.grpHighColor.TabStop = false;
            this.grpHighColor.Text = "High Value Color";
            // 
            // cddHighColor
            // 
            this.cddHighColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cddHighColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cddHighColor.FormattingEnabled = true;
            this.cddHighColor.Items.AddRange(new object[] {
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.Color.Empty});
            this.cddHighColor.Location = new System.Drawing.Point(108, 50);
            this.cddHighColor.Name = "cddHighColor";
            this.cddHighColor.Size = new System.Drawing.Size(181, 22);
            this.cddHighColor.TabIndex = 3;
            this.cddHighColor.Value = System.Drawing.Color.Empty;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(112, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "or select by name:";
            // 
            // llHighColor
            // 
            this.llHighColor.AutoSize = true;
            this.llHighColor.Location = new System.Drawing.Point(112, 18);
            this.llHighColor.Name = "llHighColor";
            this.llHighColor.Size = new System.Drawing.Size(124, 13);
            this.llHighColor.TabIndex = 0;
            this.llHighColor.TabStop = true;
            this.llHighColor.Text = "Select from color palette";
            this.llHighColor.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llHighColor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llHighColor_LinkClicked);
            // 
            // lblHighColor
            // 
            this.lblHighColor.BackColor = System.Drawing.Color.White;
            this.lblHighColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHighColor.Location = new System.Drawing.Point(26, 31);
            this.lblHighColor.Name = "lblHighColor";
            this.lblHighColor.Size = new System.Drawing.Size(42, 39);
            this.lblHighColor.TabIndex = 1;
            // 
            // grpPreview
            // 
            this.grpPreview.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grpPreview.Controls.Add(this.lblMaximum);
            this.grpPreview.Controls.Add(this.lblMinimum);
            this.grpPreview.Controls.Add(this.lblPreview);
            this.grpPreview.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPreview.Location = new System.Drawing.Point(327, 3);
            this.grpPreview.Name = "grpPreview";
            this.tableLayoutPanel1.SetRowSpan(this.grpPreview, 3);
            this.grpPreview.Size = new System.Drawing.Size(111, 303);
            this.grpPreview.TabIndex = 2;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Preview";
            // 
            // lblMaximum
            // 
            this.lblMaximum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaximum.Location = new System.Drawing.Point(6, 277);
            this.lblMaximum.Name = "lblMaximum";
            this.lblMaximum.Size = new System.Drawing.Size(60, 16);
            this.lblMaximum.TabIndex = 1;
            this.lblMaximum.Text = "100";
            this.lblMaximum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblMinimum
            // 
            this.lblMinimum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinimum.Location = new System.Drawing.Point(6, 29);
            this.lblMinimum.Name = "lblMinimum";
            this.lblMinimum.Size = new System.Drawing.Size(60, 16);
            this.lblMinimum.TabIndex = 0;
            this.lblMinimum.Text = "0";
            this.lblMinimum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPreview
            // 
            this.lblPreview.BackColor = System.Drawing.Color.White;
            this.lblPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPreview.Location = new System.Drawing.Point(72, 29);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(32, 264);
            this.lblPreview.TabIndex = 2;
            this.tipColorRamp.SetToolTip(this.lblPreview, "This shows what the color ramp will look like.");
            // 
            // lblNumBreaks
            // 
            this.lblNumBreaks.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNumBreaks.AutoSize = true;
            this.lblNumBreaks.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumBreaks.Location = new System.Drawing.Point(9, 6);
            this.lblNumBreaks.Name = "lblNumBreaks";
            this.lblNumBreaks.Size = new System.Drawing.Size(96, 13);
            this.lblNumBreaks.TabIndex = 0;
            this.lblNumBreaks.Text = "&Number of Breaks:";
            // 
            // txtNumBreaks
            // 
            this.txtNumBreaks.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtNumBreaks.Location = new System.Drawing.Point(129, 3);
            this.txtNumBreaks.Name = "txtNumBreaks";
            this.txtNumBreaks.Size = new System.Drawing.Size(76, 20);
            this.txtNumBreaks.TabIndex = 1;
            this.tipColorRamp.SetToolTip(this.txtNumBreaks, "Specify the integer number of color breaks.");
            // 
            // chkContinuous
            // 
            this.chkContinuous.AutoSize = true;
            this.chkContinuous.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkContinuous.Location = new System.Drawing.Point(10, 35);
            this.chkContinuous.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.chkContinuous.Name = "chkContinuous";
            this.chkContinuous.Size = new System.Drawing.Size(122, 17);
            this.chkContinuous.TabIndex = 1;
            this.chkContinuous.Text = "&Continuous Coloring";
            this.tipColorRamp.SetToolTip(this.chkContinuous, "If this is checked, each colorbreak will be a range\r\nof values.  If left unchecke" +
                    "d, each colorbreak\r\nwill hold a single color.\r\n");
            this.chkContinuous.UseVisualStyleBackColor = true;
            // 
            // radHue
            // 
            this.radHue.AutoSize = true;
            this.radHue.Checked = true;
            this.radHue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radHue.Location = new System.Drawing.Point(6, 12);
            this.radHue.Name = "radHue";
            this.radHue.Size = new System.Drawing.Size(74, 17);
            this.radHue.TabIndex = 0;
            this.radHue.TabStop = true;
            this.radHue.Text = "&Hue Ramp";
            this.tipColorRamp.SetToolTip(this.radHue, "Range is spread across all hues, starting with the color in Base Color");
            this.radHue.UseVisualStyleBackColor = true;
            // 
            // radSaturation
            // 
            this.radSaturation.AutoSize = true;
            this.radSaturation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radSaturation.Location = new System.Drawing.Point(6, 35);
            this.radSaturation.Name = "radSaturation";
            this.radSaturation.Size = new System.Drawing.Size(105, 17);
            this.radSaturation.TabIndex = 1;
            this.radSaturation.Text = "&Saturation Ramp";
            this.tipColorRamp.SetToolTip(this.radSaturation, "Colors range from fully saturated to unsaturaed\r\n");
            this.radSaturation.UseVisualStyleBackColor = true;
            // 
            // radAscending
            // 
            this.radAscending.AutoSize = true;
            this.radAscending.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radAscending.Location = new System.Drawing.Point(22, 29);
            this.radAscending.Name = "radAscending";
            this.radAscending.Size = new System.Drawing.Size(74, 17);
            this.radAscending.TabIndex = 0;
            this.radAscending.Text = "&Ascending";
            this.tipColorRamp.SetToolTip(this.radAscending, "Increasing Values have Increasing HSB");
            this.radAscending.UseVisualStyleBackColor = true;
            // 
            // radDescending
            // 
            this.radDescending.AutoSize = true;
            this.radDescending.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radDescending.Location = new System.Drawing.Point(22, 55);
            this.radDescending.Name = "radDescending";
            this.radDescending.Size = new System.Drawing.Size(80, 17);
            this.radDescending.TabIndex = 1;
            this.radDescending.Text = "&Descending";
            this.tipColorRamp.SetToolTip(this.radDescending, "Decreasing values have increasing HSB");
            this.radDescending.UseVisualStyleBackColor = true;
            // 
            // tabRampStyle
            // 
            this.tabRampStyle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabRampStyle.Controls.Add(this.tabTwoColorPage);
            this.tabRampStyle.Controls.Add(this.tabHSB);
            this.tabRampStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabRampStyle.Location = new System.Drawing.Point(4, 63);
            this.tabRampStyle.Name = "tabRampStyle";
            this.tabRampStyle.SelectedIndex = 0;
            this.tabRampStyle.Size = new System.Drawing.Size(315, 248);
            this.tabRampStyle.TabIndex = 3;
            // 
            // tabTwoColorPage
            // 
            this.tabTwoColorPage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabTwoColorPage.Controls.Add(this.grpLowColor);
            this.tabTwoColorPage.Controls.Add(this.grpHighColor);
            this.tabTwoColorPage.Location = new System.Drawing.Point(4, 22);
            this.tabTwoColorPage.Name = "tabTwoColorPage";
            this.tabTwoColorPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabTwoColorPage.Size = new System.Drawing.Size(307, 222);
            this.tabTwoColorPage.TabIndex = 0;
            this.tabTwoColorPage.Text = "Color Range";
            // 
            // tabHSB
            // 
            this.tabHSB.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabHSB.Controls.Add(this.grbOrder);
            this.tabHSB.Controls.Add(this.radBrightness);
            this.tabHSB.Controls.Add(this.radSaturation);
            this.tabHSB.Controls.Add(this.radHue);
            this.tabHSB.Controls.Add(this.grpBaseColor);
            this.tabHSB.Location = new System.Drawing.Point(4, 22);
            this.tabHSB.Name = "tabHSB";
            this.tabHSB.Padding = new System.Windows.Forms.Padding(3);
            this.tabHSB.Size = new System.Drawing.Size(307, 222);
            this.tabHSB.TabIndex = 1;
            this.tabHSB.Text = "Hue Saturation Brightness";
            // 
            // grbOrder
            // 
            this.grbOrder.Controls.Add(this.radAscending);
            this.grbOrder.Controls.Add(this.radDescending);
            this.grbOrder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbOrder.Location = new System.Drawing.Point(162, 6);
            this.grbOrder.Name = "grbOrder";
            this.grbOrder.Size = new System.Drawing.Size(139, 89);
            this.grbOrder.TabIndex = 3;
            this.grbOrder.TabStop = false;
            this.grbOrder.Text = "Sequence";
            // 
            // radBrightness
            // 
            this.radBrightness.AutoSize = true;
            this.radBrightness.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radBrightness.Location = new System.Drawing.Point(6, 58);
            this.radBrightness.Name = "radBrightness";
            this.radBrightness.Size = new System.Drawing.Size(75, 17);
            this.radBrightness.TabIndex = 2;
            this.radBrightness.Text = "&Brightness";
            this.radBrightness.UseVisualStyleBackColor = true;
            // 
            // grpBaseColor
            // 
            this.grpBaseColor.Controls.Add(this.cddBaseColor);
            this.grpBaseColor.Controls.Add(this.label2);
            this.grpBaseColor.Controls.Add(this.llBaseColor);
            this.grpBaseColor.Controls.Add(this.lblBaseColor);
            this.grpBaseColor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBaseColor.Location = new System.Drawing.Point(6, 113);
            this.grpBaseColor.Name = "grpBaseColor";
            this.grpBaseColor.Size = new System.Drawing.Size(295, 89);
            this.grpBaseColor.TabIndex = 4;
            this.grpBaseColor.TabStop = false;
            this.grpBaseColor.Text = "Base Color";
            // 
            // cddBaseColor
            // 
            this.cddBaseColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cddBaseColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cddBaseColor.FormattingEnabled = true;
            this.cddBaseColor.Items.AddRange(new object[] {
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.KnownColor.ActiveBorder,
            System.Drawing.KnownColor.ActiveCaption,
            System.Drawing.KnownColor.ActiveCaptionText,
            System.Drawing.KnownColor.AppWorkspace,
            System.Drawing.KnownColor.Control,
            System.Drawing.KnownColor.ControlDark,
            System.Drawing.KnownColor.ControlDarkDark,
            System.Drawing.KnownColor.ControlLight,
            System.Drawing.KnownColor.ControlLightLight,
            System.Drawing.KnownColor.ControlText,
            System.Drawing.KnownColor.Desktop,
            System.Drawing.KnownColor.GrayText,
            System.Drawing.KnownColor.Highlight,
            System.Drawing.KnownColor.HighlightText,
            System.Drawing.KnownColor.HotTrack,
            System.Drawing.KnownColor.InactiveBorder,
            System.Drawing.KnownColor.InactiveCaption,
            System.Drawing.KnownColor.InactiveCaptionText,
            System.Drawing.KnownColor.Info,
            System.Drawing.KnownColor.InfoText,
            System.Drawing.KnownColor.Menu,
            System.Drawing.KnownColor.MenuText,
            System.Drawing.KnownColor.ScrollBar,
            System.Drawing.KnownColor.Window,
            System.Drawing.KnownColor.WindowFrame,
            System.Drawing.KnownColor.WindowText,
            System.Drawing.KnownColor.Transparent,
            System.Drawing.KnownColor.AliceBlue,
            System.Drawing.KnownColor.AntiqueWhite,
            System.Drawing.KnownColor.Aqua,
            System.Drawing.KnownColor.Aquamarine,
            System.Drawing.KnownColor.Azure,
            System.Drawing.KnownColor.Beige,
            System.Drawing.KnownColor.Bisque,
            System.Drawing.KnownColor.Black,
            System.Drawing.KnownColor.BlanchedAlmond,
            System.Drawing.KnownColor.Blue,
            System.Drawing.KnownColor.BlueViolet,
            System.Drawing.KnownColor.Brown,
            System.Drawing.KnownColor.BurlyWood,
            System.Drawing.KnownColor.CadetBlue,
            System.Drawing.KnownColor.Chartreuse,
            System.Drawing.KnownColor.Chocolate,
            System.Drawing.KnownColor.Coral,
            System.Drawing.KnownColor.CornflowerBlue,
            System.Drawing.KnownColor.Cornsilk,
            System.Drawing.KnownColor.Crimson,
            System.Drawing.KnownColor.Cyan,
            System.Drawing.KnownColor.DarkBlue,
            System.Drawing.KnownColor.DarkCyan,
            System.Drawing.KnownColor.DarkGoldenrod,
            System.Drawing.KnownColor.DarkGray,
            System.Drawing.KnownColor.DarkGreen,
            System.Drawing.KnownColor.DarkKhaki,
            System.Drawing.KnownColor.DarkMagenta,
            System.Drawing.KnownColor.DarkOliveGreen,
            System.Drawing.KnownColor.DarkOrange,
            System.Drawing.KnownColor.DarkOrchid,
            System.Drawing.KnownColor.DarkRed,
            System.Drawing.KnownColor.DarkSalmon,
            System.Drawing.KnownColor.DarkSeaGreen,
            System.Drawing.KnownColor.DarkSlateBlue,
            System.Drawing.KnownColor.DarkSlateGray,
            System.Drawing.KnownColor.DarkTurquoise,
            System.Drawing.KnownColor.DarkViolet,
            System.Drawing.KnownColor.DeepPink,
            System.Drawing.KnownColor.DeepSkyBlue,
            System.Drawing.KnownColor.DimGray,
            System.Drawing.KnownColor.DodgerBlue,
            System.Drawing.KnownColor.Firebrick,
            System.Drawing.KnownColor.FloralWhite,
            System.Drawing.KnownColor.ForestGreen,
            System.Drawing.KnownColor.Fuchsia,
            System.Drawing.KnownColor.Gainsboro,
            System.Drawing.KnownColor.GhostWhite,
            System.Drawing.KnownColor.Gold,
            System.Drawing.KnownColor.Goldenrod,
            System.Drawing.KnownColor.Gray,
            System.Drawing.KnownColor.Green,
            System.Drawing.KnownColor.GreenYellow,
            System.Drawing.KnownColor.Honeydew,
            System.Drawing.KnownColor.HotPink,
            System.Drawing.KnownColor.IndianRed,
            System.Drawing.KnownColor.Indigo,
            System.Drawing.KnownColor.Ivory,
            System.Drawing.KnownColor.Khaki,
            System.Drawing.KnownColor.Lavender,
            System.Drawing.KnownColor.LavenderBlush,
            System.Drawing.KnownColor.LawnGreen,
            System.Drawing.KnownColor.LemonChiffon,
            System.Drawing.KnownColor.LightBlue,
            System.Drawing.KnownColor.LightCoral,
            System.Drawing.KnownColor.LightCyan,
            System.Drawing.KnownColor.LightGoldenrodYellow,
            System.Drawing.KnownColor.LightGray,
            System.Drawing.KnownColor.LightGreen,
            System.Drawing.KnownColor.LightPink,
            System.Drawing.KnownColor.LightSalmon,
            System.Drawing.KnownColor.LightSeaGreen,
            System.Drawing.KnownColor.LightSkyBlue,
            System.Drawing.KnownColor.LightSlateGray,
            System.Drawing.KnownColor.LightSteelBlue,
            System.Drawing.KnownColor.LightYellow,
            System.Drawing.KnownColor.Lime,
            System.Drawing.KnownColor.LimeGreen,
            System.Drawing.KnownColor.Linen,
            System.Drawing.KnownColor.Magenta,
            System.Drawing.KnownColor.Maroon,
            System.Drawing.KnownColor.MediumAquamarine,
            System.Drawing.KnownColor.MediumBlue,
            System.Drawing.KnownColor.MediumOrchid,
            System.Drawing.KnownColor.MediumPurple,
            System.Drawing.KnownColor.MediumSeaGreen,
            System.Drawing.KnownColor.MediumSlateBlue,
            System.Drawing.KnownColor.MediumSpringGreen,
            System.Drawing.KnownColor.MediumTurquoise,
            System.Drawing.KnownColor.MediumVioletRed,
            System.Drawing.KnownColor.MidnightBlue,
            System.Drawing.KnownColor.MintCream,
            System.Drawing.KnownColor.MistyRose,
            System.Drawing.KnownColor.Moccasin,
            System.Drawing.KnownColor.NavajoWhite,
            System.Drawing.KnownColor.Navy,
            System.Drawing.KnownColor.OldLace,
            System.Drawing.KnownColor.Olive,
            System.Drawing.KnownColor.OliveDrab,
            System.Drawing.KnownColor.Orange,
            System.Drawing.KnownColor.OrangeRed,
            System.Drawing.KnownColor.Orchid,
            System.Drawing.KnownColor.PaleGoldenrod,
            System.Drawing.KnownColor.PaleGreen,
            System.Drawing.KnownColor.PaleTurquoise,
            System.Drawing.KnownColor.PaleVioletRed,
            System.Drawing.KnownColor.PapayaWhip,
            System.Drawing.KnownColor.PeachPuff,
            System.Drawing.KnownColor.Peru,
            System.Drawing.KnownColor.Pink,
            System.Drawing.KnownColor.Plum,
            System.Drawing.KnownColor.PowderBlue,
            System.Drawing.KnownColor.Purple,
            System.Drawing.KnownColor.Red,
            System.Drawing.KnownColor.RosyBrown,
            System.Drawing.KnownColor.RoyalBlue,
            System.Drawing.KnownColor.SaddleBrown,
            System.Drawing.KnownColor.Salmon,
            System.Drawing.KnownColor.SandyBrown,
            System.Drawing.KnownColor.SeaGreen,
            System.Drawing.KnownColor.SeaShell,
            System.Drawing.KnownColor.Sienna,
            System.Drawing.KnownColor.Silver,
            System.Drawing.KnownColor.SkyBlue,
            System.Drawing.KnownColor.SlateBlue,
            System.Drawing.KnownColor.SlateGray,
            System.Drawing.KnownColor.Snow,
            System.Drawing.KnownColor.SpringGreen,
            System.Drawing.KnownColor.SteelBlue,
            System.Drawing.KnownColor.Tan,
            System.Drawing.KnownColor.Teal,
            System.Drawing.KnownColor.Thistle,
            System.Drawing.KnownColor.Tomato,
            System.Drawing.KnownColor.Turquoise,
            System.Drawing.KnownColor.Violet,
            System.Drawing.KnownColor.Wheat,
            System.Drawing.KnownColor.White,
            System.Drawing.KnownColor.WhiteSmoke,
            System.Drawing.KnownColor.Yellow,
            System.Drawing.KnownColor.YellowGreen,
            System.Drawing.KnownColor.ButtonFace,
            System.Drawing.KnownColor.ButtonHighlight,
            System.Drawing.KnownColor.ButtonShadow,
            System.Drawing.KnownColor.GradientActiveCaption,
            System.Drawing.KnownColor.GradientInactiveCaption,
            System.Drawing.KnownColor.MenuBar,
            System.Drawing.KnownColor.MenuHighlight,
            System.Drawing.Color.Empty});
            this.cddBaseColor.Location = new System.Drawing.Point(108, 50);
            this.cddBaseColor.Name = "cddBaseColor";
            this.cddBaseColor.Size = new System.Drawing.Size(174, 22);
            this.cddBaseColor.TabIndex = 3;
            this.cddBaseColor.Value = System.Drawing.Color.Empty;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(105, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "or select by name:";
            // 
            // llBaseColor
            // 
            this.llBaseColor.AutoSize = true;
            this.llBaseColor.Location = new System.Drawing.Point(105, 18);
            this.llBaseColor.Name = "llBaseColor";
            this.llBaseColor.Size = new System.Drawing.Size(124, 13);
            this.llBaseColor.TabIndex = 0;
            this.llBaseColor.TabStop = true;
            this.llBaseColor.Text = "Select from color palette";
            this.llBaseColor.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llBaseColor.Click += new System.EventHandler(this.llBaseColor_Click);
            // 
            // lblBaseColor
            // 
            this.lblBaseColor.BackColor = System.Drawing.Color.White;
            this.lblBaseColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBaseColor.Location = new System.Drawing.Point(26, 28);
            this.lblBaseColor.Name = "lblBaseColor";
            this.lblBaseColor.Size = new System.Drawing.Size(42, 39);
            this.lblBaseColor.TabIndex = 1;
            // 
            // dialogButtons1
            // 
            this.dialogButtons1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dialogButtons1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.dialogButtons1, 2);
            this.dialogButtons1.Location = new System.Drawing.Point(179, 322);
            this.dialogButtons1.Name = "dialogButtons1";
            this.dialogButtons1.Size = new System.Drawing.Size(259, 29);
            this.dialogButtons1.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.grpPreview, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabRampStyle, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dialogButtons1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkContinuous, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(441, 354);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.txtNumBreaks);
            this.panel1.Controls.Add(this.lblNumBreaks);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(208, 26);
            this.panel1.TabIndex = 0;
            // 
            // RampColorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 354);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RampColorDialog";
            this.ShowInTaskbar = false;
            this.Text = "Choose Colors";
            this.grpLowColor.ResumeLayout(false);
            this.grpLowColor.PerformLayout();
            this.grpHighColor.ResumeLayout(false);
            this.grpHighColor.PerformLayout();
            this.grpPreview.ResumeLayout(false);
            this.tabRampStyle.ResumeLayout(false);
            this.tabTwoColorPage.ResumeLayout(false);
            this.tabHSB.ResumeLayout(false);
            this.tabHSB.PerformLayout();
            this.grbOrder.ResumeLayout(false);
            this.grbOrder.PerformLayout();
            this.grpBaseColor.ResumeLayout(false);
            this.grpBaseColor.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

     

        #endregion



        #region Properties

        /// <summary>
        /// Gets or sets the end color for this dialog
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the end color for this dialog")]
        public Color HighColor
        {
            get { return lblHighColor.BackColor; }
            set
            {
                lblHighColor.BackColor = value;
                if (Visible) UpdatePreview();
            }
        }

        /// <summary>
        /// Gets or sets the start color for this dialog
        /// </summary>
        [Category("Appearance"),Description("Gets or sets the start color for this dialog")]
        public Color LowColor
        {
            get { return lblLowColor.BackColor; }
            set 
            { 
                lblLowColor.BackColor = value;
                if (Visible) UpdatePreview();
            }
        }


        /// <summary>
        /// Gets or sets the RasterSymbolizer that is being described by this dialog.
        /// </summary>
        [Category("Data"), Description("Gets or sets the raster symbolizer for this dialog.")]
        public IColorRampBuilder ColorRampBuilder
        {
            get { return _colorRampBuilder; }
            set { _colorRampBuilder = value; }
        }

        /// <summary>
        /// Gets a boolean that is true if the Hue, Saturation, Brightness mode is engaged.
        /// </summary>
        public bool IsHueSatBright
        {
            get
            {
                if (tabRampStyle.SelectedIndex == 0) return false;
                return true;
            }

        }

        #endregion

        #region Event Handlers

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }


        void cddHighColor_SelectedIndexChanged(object sender, EventArgs e)
        {
           lblHighColor.BackColor = Color.FromKnownColor((KnownColor)cddHighColor.SelectedItem);
           UpdatePreview();
        }

        void cddLowColor_SelectedIndexChanged(object sender, EventArgs e)
        {
           lblLowColor.BackColor = Color.FromKnownColor((KnownColor)cddLowColor.SelectedItem);
           UpdatePreview();
        }

        void CddBaseColorSelectedIndexChanged(object sender, EventArgs e)
        {
            lblBaseColor.BackColor = Color.FromKnownColor((KnownColor)cddBaseColor.SelectedItem);
            UpdatePreview();
        }


        void  lblBaseColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = lblBaseColor.BackColor;
            dlg.AllowFullOpen = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lblBaseColor.BackColor = dlg.Color;
                UpdatePreview();
            }
        }

        void lblHighColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = lblHighColor.BackColor;
            dlg.AllowFullOpen = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lblHighColor.BackColor = dlg.Color;
                UpdatePreview();
            }
        }

        void lblPreview_Paint(object sender, PaintEventArgs e)
        {
            UpdatePreview();
        }

        void lblLowColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = lblLowColor.BackColor;
            dlg.AllowFullOpen = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lblLowColor.BackColor = dlg.Color;
                UpdatePreview();
            }
        }

        void llBaseColor_Click(object sender, EventArgs e)
        {
            lblLowColor_Click(sender, new EventArgs());
        }


        private void llLowColor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblLowColor_Click(sender, new EventArgs());
        }

        private void llHighColor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblHighColor_Click(sender, new EventArgs());
        }


        #endregion


        #region Private Methods

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
        /// Updates the preview based on whatever the colorbreaks have... but the apply button is used to
        /// change the values in the color breaks.
        /// </summary>
        private void UpdatePreview()
        {
            Graphics g = lblPreview.CreateGraphics();
            if (_colorRampBuilder == null)
            {
                g.FillRectangle(Brushes.White, lblPreview.ClientRectangle);
                return;
            }
            if (_colorRampBuilder.ColorBreaks.Count == 0)
            {
                g.FillRectangle(Brushes.White, lblPreview.ClientRectangle);
                return;
            }
            Brush cbBrush;
            int i = 0;
            foreach(IColorCategory cb in _colorRampBuilder.ColorBreaks)
            {
                double verticalStep = ((double)lblPreview.ClientRectangle.Height) / _colorRampBuilder.ColorBreaks.Count;
                Rectangle brkRect = new Rectangle(lblPreview.ClientRectangle.X, lblPreview.ClientRectangle.Top + Convert.ToInt32(verticalStep * i), lblPreview.ClientRectangle.Width, Convert.ToInt32(verticalStep));
                if (cb.IsBiValue)
                {
                    cbBrush = new System.Drawing.Drawing2D.LinearGradientBrush(brkRect, cb.LowColor, cb.HighColor, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                }
                else
                {
                    cbBrush = new SolidBrush(cb.LowColor);
                }
                g.FillRectangle(cbBrush, brkRect);
                i++;
                // only dispose the brush here because we are creating a new instance of one.  
                if (cbBrush != null) cbBrush.Dispose();
            }
            
            // don't dispose cbBrush here because it is an existing system brush.
            
            g.Dispose();
            

        }

       

        #endregion

        
        private void btnApply_Click(object sender, EventArgs e)
        {
            int numBreaks;
            // Validate NumBreaks
            if (Global.IsInteger(txtNumBreaks.Text))
            {
                numBreaks = Global.GetInteger(txtNumBreaks.Text);
            }
            else
            {
                // For logging purposes only.  At this level, go ahead and handle the exception.
                NonNumericLogException ex = new NonNumericLogException("NumBreaks");
                LogManager.DefaultLogManager.LogMessageBox(this, "The value given as count could not be read as an integer.  Please try using values like '5'.", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (IsHueSatBright)
            {
                if (radBrightness.Checked)
                {
                    _colorRampBuilder.CreateShadingRamp(numBreaks, lblBaseColor.BackColor, chkContinuous.Checked, radAscending.Checked);
                }
                else if (radHue.Checked)
                {
                    _colorRampBuilder.CreateHueRamp(numBreaks, lblBaseColor.BackColor, chkContinuous.Checked, radAscending.Checked);
                }
                else
                {
                    _colorRampBuilder.CreateSaturationRamp(numBreaks, lblBaseColor.BackColor, chkContinuous.Checked, radAscending.Checked);
                }
            }
            else
            {
                _colorRampBuilder.CreateRamp(numBreaks, LowColor, HighColor, chkContinuous.Checked);
            }
        }

       

       


    }
}