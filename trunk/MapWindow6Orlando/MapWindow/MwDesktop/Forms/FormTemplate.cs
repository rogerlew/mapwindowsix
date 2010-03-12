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
// The Initial Developer of this Original Code is Ted Dunsford. Created $time$
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Components;

namespace MapWindow.Forms
{
    /// <summary>
    /// $safeitemname$
    /// </summary>
    public class $safeitemname$: Form
    {
   
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "FormTemplate";
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of $safeitemname$
        /// </summary>
        public $safeitemname$()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

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

        #endregion
 }
}