//********************************************************************************************************
// Product Name: MapWindow.Tools.ToolDialog
// Description:  The tool dialog to be used by tools. It get populated by DialogElements once it is created
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
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    /// <summary>
    /// A generic form that works with the various dialog elements in order to create a fully working process.
    /// </summary>
    internal class ToolDialog : Form
    {
        #region ---------------------- Class Variables
        
        //Required by Designer
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelOkCancel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panelElementContainer;
                
        //Class Variables
        private ITool _tool;
        private Panel panelToolName;
        private Label lblToolName;
        private Panel panelHelp;
        private Panel panelToolIcon;
        private LinkLabel helpHyperlink;
        private RichTextBox rtbHelp;
        private Panel helpImage;
        private int _elementHeight = 3;
        private List<DataSetArray> _dataSets = new List<DataSetArray>();
        private List<DialogElement> _listOfDialogElements = new List<DialogElement>();

        #endregion

        #region ---------------------- Constructor

        /// <summary>
        /// The constructor for the ToolDialog
        /// </summary>
        /// <param name="Tool">The ITool to create the dialog box for</param>
        /// <param name="DataSets">The list of available DataSets available</param>
        public ToolDialog(ITool Tool, List<DataSetArray> DataSets)
        {
            //Required by the designer
            InitializeializeializeComponent();

            _dataSets = DataSets;

            Initialize(Tool);
        }

        /// <summary>
        /// The constructor for the ToolDialog
        /// </summary>
        /// <param name="Tool">The ITool to create the dialog box for</param>
        /// <param name="ModelElements">A list of all model elements</param>
        public ToolDialog(ITool Tool, List<ModelElement> ModelElements)
        {
            //Required by the designer
            InitializeializeializeComponent();

            //We store all the element names here and extract the datasets
            foreach (ModelElement me in ModelElements)
            {
                if (me as DataElement != null)
                {
                    Boolean addData = true;
                    foreach (Parameter par in Tool.OutputParameters)
                    {
                        if (par.ModelName == (me as DataElement).Parameter.ModelName)
                            addData = false; break;
                    }
                    if (addData)
                        _dataSets.Add(new DataSetArray(me.Name, (me as DataElement).Parameter.Value as MapWindow.Data.IDataSet));
                }
            }

            Initialize(Tool);
        }

        /// <summary>
        /// The constructor for the ToolDialog
        /// </summary>
        /// <param name="Tool">The ITool to create the dialog box for</param>
        private void Initialize(ITool Tool)
        {
            this.SuspendLayout();

            //Generates the form based on what inputs the ITool has
            _tool = Tool;
            this.Text = Tool.Name;
            this.lblToolName.Text = Tool.Name;

            //Sets up the help link
            if (Tool.HelpURL == "")
                this.helpHyperlink.Visible = false;
            else
            {
                this.helpHyperlink.Links[0].LinkData = Tool.HelpURL;
                this.helpHyperlink.Links.Add(0, this.helpHyperlink.Text.Length, Tool.HelpURL);
            }

            //Sets-up the icon for the Dialog
            this.Icon = MapWindow.Images.HammerSmall;
            if (Tool.Icon != null)
                this.panelToolIcon.BackgroundImage = Tool.Icon;
            else
                this.panelToolIcon.BackgroundImage = MapWindow.Images.Hammer;

            DialogSpacerElement inputSpacer = new DialogSpacerElement();
            inputSpacer.Text = MessageStrings.Input;
            AddElement(inputSpacer);

            //Populates the dialog with input elements
            PopulateInputElements();

            DialogSpacerElement outputSpacer = new DialogSpacerElement();
            outputSpacer.Text =MessageStrings.Ouput;
            AddElement(outputSpacer);

            //Populates the dialog with output elements
            PopulateOutputElements();

            //Populate the help text
            PopulateHelp(_tool.Name, _tool.HelpText, _tool.HelpImage);

            this.ResumeLayout();
        }

        #endregion

        #region ---------------------- Private Functions

        /// <summary>
        /// Adds Elements to the dialog based on what output Parameter the ITool contains 
        /// </summary>
        private void PopulateOutputElements()
        {
            //Loops through all the Parameter in the tool and generated their element
            foreach (Parameter param in _tool.OutputParameters)
            {
                //We add an event handler that fires if the parameter is changed
                param.ValueChanged += new EventHandlerValueChanged(param_ValueChanged);

                //Retrieve the dialog element from the parameter and add it to the dialog
                AddElement(param.OutputDialogElement(DataSets));
            }
        }

        /// <summary>
        /// Adds Elements to the dialog based on what input Parameter the ITool contains
        /// </summary>
        private void PopulateInputElements()
        {
            //Loops through all the Parameter in the tool and generated their element
            foreach (Parameter param in _tool.InputParameters)
            {
                //We make sure that the input parameter is defined
                if (param == null)
                    continue;

                //We add an event handler that fires if the parameter is changed
                param.ValueChanged += new EventHandlerValueChanged(param_ValueChanged);

                //Retrieve the dialog element from the parameter and add it to the dialog
                AddElement(param.InputDialogElement(DataSets));
            }
        }

        /// <summary>
        /// This adds the Elements to the form incrementaly lower down
        /// </summary>
        /// <param name="element">The element to add</param>
        private void AddElement(DialogElement element)
        {
            _listOfDialogElements.Add(element);
            this.panelElementContainer.Controls.Add(element);
            element.Clicked += new EventHandler(element_Clicked);
            element.Location = new System.Drawing.Point(5,_elementHeight );
            _elementHeight = element.Height + _elementHeight;
        }

        /// <summary>
        /// This adds a Bitmap to the help section
        /// </summary>
        /// <param name="Body">The title to appear in the help box</param>
        /// <param name="Title">The text to appear in the help box</param>
        /// <param name="Image">The bitmap to appear at the bottom of the help box</param>
        private void PopulateHelp(String Title, String Body, Bitmap Image)
        {
            rtbHelp.Text = "";
            rtbHelp.Size = new Size(0, 0);

            //Add the Title
            Font fBold = new Font("Tahoma", 14, FontStyle.Bold);
            rtbHelp.SelectionFont = fBold;
            rtbHelp.SelectionColor = Color.Black;
            rtbHelp.SelectedText = Title + "\r\n\r\n";

            //Add the text body
            fBold = new Font("Tahoma", 8, FontStyle.Bold);
            rtbHelp.SelectionFont = fBold;
            rtbHelp.SelectionColor = Color.Black;
            rtbHelp.SelectedText = Body;
            rtbHelp.Size = new Size(rtbHelp.Width, rtbHelp.GetPositionFromCharIndex(rtbHelp.Text.Length).Y + 30);

            //Add the image to the bottom
            if (Image != null)
            {
                helpImage.Visible = true;
                if (Image.Size.Width > 250)
                {
                    double Height = Image.Size.Height;
                    double Width = Image.Size.Width;
                    int newHeight = Convert.ToInt32(250 * (Height / Width));
                    helpImage.BackgroundImage = new Bitmap(Image, new Size(250, newHeight));
                    helpImage.Size = new Size(250, newHeight);
                }
                else
                {
                    helpImage.BackgroundImage = Image;
                    helpImage.Size = Image.Size;
                }
            }
            else
            {
                helpImage.Visible = false;
                helpImage.BackgroundImage = null;
                helpImage.Size = new Size(0, 0);
            }
        }

        #endregion

        #region ---------------------- Properties

        /// <summary>
        /// Gets the status of the tool
        /// </summary>
        public ToolStatus ToolStatus
        {
            get
            {
                foreach (DialogElement de in _listOfDialogElements)
                {
                    if (de.Status != ToolStatus.Ok)
                        return ToolStatus.Error;
                }
                return ToolStatus.Ok;
            }
        }

        /// <summary>
        /// Returns a list of IDataSet that are available in the ToolDialog excluding any of its own outputs.
        /// </summary>
        public List<DataSetArray> DataSets
        {
            get { return _dataSets; }
            set { _dataSets = value; }
        }

        #endregion

        #region ---------------------- Events

        /// <summary>
        /// Fires when a parameter is changed
        /// </summary>
        /// <param name="sender"></param>
        private void param_ValueChanged(Parameter sender)
        {
            _tool.ParameterChanged(sender);
        }

        /// <summary>
        /// When the user clicks OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// When the user clicks Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            return;
        }

        /// <summary>
        /// When one of the DialogElements is clicked this event fires to populate the help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void element_Clicked(object sender, EventArgs e)
        {
            DialogElement element = sender as DialogElement;
            if (element == null)
                PopulateHelp(_tool.Name, _tool.HelpText, _tool.HelpImage);
            else if (element.Param == null)
                PopulateHelp(_tool.Name, _tool.HelpText, _tool.HelpImage);
            else if (element.Param.HelpText == "")
                PopulateHelp(_tool.Name, _tool.HelpText, _tool.HelpImage);
            else
                PopulateHelp(element.Param.Name, element.Param.HelpText, element.Param.HelpImage);
        }

        /// <summary>
        /// If the user clicks out side of one of the tool elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otherElement_Click(object sender, EventArgs e)
        {
            PopulateHelp(_tool.Name, _tool.HelpText, _tool.HelpImage);
        }

        /// <summary>
        /// When the size of the help panel changes this event fires to move stuff around.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelHelp_SizeChanged(object sender, EventArgs e)
        {
            rtbHelp.Size = new Size(rtbHelp.Width, rtbHelp.GetPositionFromCharIndex(rtbHelp.Text.Length).Y + 30);
        }

        /// <summary>
        /// When the hyperlink is clicked this event fires.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpHyperlink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Determine which link was clicked within the LinkLabel.
            this.helpHyperlink.Links[helpHyperlink.Links.IndexOf(e.Link)].Visited = true;

            // Display the appropriate link based on the value of the 
            // LinkData property of the Link object.
            string target = e.Link.LinkData as string;

            System.Diagnostics.Process.Start(target);
        }

        #endregion

        #region ---------------------- Windows Form Designer generated code

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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeializeializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelElementContainer = new System.Windows.Forms.Panel();
            this.panelToolName = new System.Windows.Forms.Panel();
            this.panelToolIcon = new System.Windows.Forms.Panel();
            this.lblToolName = new System.Windows.Forms.Label();
            this.panelOkCancel = new System.Windows.Forms.Panel();
            this.helpHyperlink = new System.Windows.Forms.LinkLabel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelHelp = new System.Windows.Forms.Panel();
            this.helpImage = new System.Windows.Forms.Panel();
            this.rtbHelp = new System.Windows.Forms.RichTextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelToolName.SuspendLayout();
            this.panelOkCancel.SuspendLayout();
            this.panelHelp.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelElementContainer);
            this.splitContainer1.Panel1.Controls.Add(this.panelToolName);
            this.splitContainer1.Panel1.Controls.Add(this.panelOkCancel);
            this.splitContainer1.Panel1MinSize = 450;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.AutoScrollMinSize = new System.Drawing.Size(85, 0);
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.panelHelp);
            this.splitContainer1.Size = new System.Drawing.Size(792, 476);
            this.splitContainer1.SplitterDistance = 527;
            this.splitContainer1.TabIndex = 2;
            // 
            // panelElementContainer
            // 
            this.panelElementContainer.AutoScroll = true;
            this.panelElementContainer.AutoScrollMinSize = new System.Drawing.Size(250, 0);
            this.panelElementContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelElementContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelElementContainer.Location = new System.Drawing.Point(0, 40);
            this.panelElementContainer.Name = "panelElementContainer";
            this.panelElementContainer.Size = new System.Drawing.Size(527, 402);
            this.panelElementContainer.TabIndex = 2;
            this.panelElementContainer.Click += new System.EventHandler(this.otherElement_Click);
            // 
            // panelToolName
            // 
            this.panelToolName.Controls.Add(this.panelToolIcon);
            this.panelToolName.Controls.Add(this.lblToolName);
            this.panelToolName.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolName.Location = new System.Drawing.Point(0, 0);
            this.panelToolName.Name = "panelToolName";
            this.panelToolName.Size = new System.Drawing.Size(527, 40);
            this.panelToolName.TabIndex = 3;
            this.panelToolName.Click += new System.EventHandler(this.otherElement_Click);
            // 
            // panelToolIcon
            // 
            this.panelToolIcon.BackgroundImage = global::MapWindow.Images.Hammer;
            this.panelToolIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelToolIcon.Location = new System.Drawing.Point(4, 4);
            this.panelToolIcon.Name = "panelToolIcon";
            this.panelToolIcon.Size = new System.Drawing.Size(32, 32);
            this.panelToolIcon.TabIndex = 2;
            this.panelToolIcon.Click += new System.EventHandler(this.otherElement_Click);
            // 
            // lblToolName
            // 
            this.lblToolName.AutoSize = true;
            this.lblToolName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolName.Location = new System.Drawing.Point(42, 9);
            this.lblToolName.Name = "lblToolName";
            this.lblToolName.Size = new System.Drawing.Size(131, 23);
            this.lblToolName.TabIndex = 0;
            this.lblToolName.Text = "lblToolName";
            this.lblToolName.Click += new System.EventHandler(this.otherElement_Click);
            // 
            // panelOkCancel
            // 
            this.panelOkCancel.AutoSize = true;
            this.panelOkCancel.Controls.Add(this.helpHyperlink);
            this.panelOkCancel.Controls.Add(this.btnCancel);
            this.panelOkCancel.Controls.Add(this.btnOK);
            this.panelOkCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOkCancel.Location = new System.Drawing.Point(0, 442);
            this.panelOkCancel.Name = "panelOkCancel";
            this.panelOkCancel.Size = new System.Drawing.Size(527, 34);
            this.panelOkCancel.TabIndex = 1;
            this.panelOkCancel.Click += new System.EventHandler(this.otherElement_Click);
            // 
            // helpHyperlink
            // 
            this.helpHyperlink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpHyperlink.AutoSize = true;
            this.helpHyperlink.Location = new System.Drawing.Point(459, 13);
            this.helpHyperlink.Name = "helpHyperlink";
            this.helpHyperlink.Size = new System.Drawing.Size(53, 13);
            this.helpHyperlink.TabIndex = 2;
            this.helpHyperlink.TabStop = true;
            this.helpHyperlink.Text = "Tool Help";
            this.helpHyperlink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpHyperlink_LinkClicked);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(83, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(7, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panelHelp
            // 
            this.panelHelp.AutoScroll = true;
            this.panelHelp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelHelp.Controls.Add(this.helpImage);
            this.panelHelp.Controls.Add(this.rtbHelp);
            this.panelHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHelp.Location = new System.Drawing.Point(0, 0);
            this.panelHelp.Name = "panelHelp";
            this.panelHelp.Size = new System.Drawing.Size(261, 476);
            this.panelHelp.TabIndex = 0;
            this.panelHelp.SizeChanged += new System.EventHandler(this.panelHelp_SizeChanged);
            // 
            // helpImage
            // 
            this.helpImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.helpImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.helpImage.Location = new System.Drawing.Point(0, 250);
            this.helpImage.Name = "helpImage";
            this.helpImage.Size = new System.Drawing.Size(257, 161);
            this.helpImage.TabIndex = 1;
            // 
            // rtbHelp
            // 
            this.rtbHelp.BackColor = System.Drawing.SystemColors.Window;
            this.rtbHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtbHelp.Location = new System.Drawing.Point(0, 0);
            this.rtbHelp.Margin = new System.Windows.Forms.Padding(5);
            this.rtbHelp.Name = "rtbHelp";
            this.rtbHelp.ReadOnly = true;
            this.rtbHelp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbHelp.Size = new System.Drawing.Size(257, 250);
            this.rtbHelp.TabIndex = 0;
            this.rtbHelp.Text = "";
            // 
            // ToolDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(792, 476);
            this.Controls.Add(this.splitContainer1);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "ToolDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ToolDialog";
            this.Click += new System.EventHandler(this.otherElement_Click);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panelToolName.ResumeLayout(false);
            this.panelToolName.PerformLayout();
            this.panelOkCancel.ResumeLayout(false);
            this.panelOkCancel.PerformLayout();
            this.panelHelp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       #endregion

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolDialog));
            this.SuspendLayout();
            // 
            // ToolDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Font = null;
            this.Icon = null;
            this.Name = "ToolDialog";
            this.ResumeLayout(false);

        }

       

   }
}