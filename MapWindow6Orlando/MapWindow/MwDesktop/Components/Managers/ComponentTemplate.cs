//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  Low level interfaces that allow separate components to use objects that are defined
//               in completely independant libraries so long as they satisfy the basic interface requirements.
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created $time$
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MapWindow.PluginInterfaces;
using MapWindow.Main;

namespace MapWindow.Components
{
    /// <summary>
    /// This component allows customization of how log messages are sent
    /// </summary>
    public partial class $safeitemname$ : Component
    {
        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the $safeitemname$
        /// </summary>
        public $safeitemname$()
        {

            InitializeComponent();
        }

        /// <summary>
        /// Creates a new $safeitemname$ and adds it to the specified container.
        /// </summary>
        /// <param name="container"></param>
        public $safeitemname$(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            
        }

        #endregion


        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

       
    }
}
