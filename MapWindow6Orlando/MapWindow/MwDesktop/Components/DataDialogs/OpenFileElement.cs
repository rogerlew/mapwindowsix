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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 1:15:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;


namespace MapWindow.Components
{


    /// <summary>
    /// The OpenFileElement can be added directly to a form and supports all of the
    /// basic dialog options that are important for browsing vector/raster/image
    /// data.
    /// </summary>
    internal class OpenFileElement : UserControl
    {
        private ImageList imlImages;
        private IContainer components;
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of OpenFileElement
        /// </summary>
        public OpenFileElement()
        {

        }

        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenFileElement));
            this.imlImages = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imlImages
            // 
            this.imlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.imlImages, "imlImages");
            this.imlImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // OpenFileElement
            // 
            this.Name = "OpenFileElement";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
