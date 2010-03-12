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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2008 9:33:56 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Components
{
    /// <summary>
    /// This component allows customization of how processes are handled
    /// </summary>
    public class ProcessManager
    {
        #region Private Variables

        private ILogManager _logManager;

       

       

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// This variable returns the default process manager for the entire project.  There is only
        /// one of these, and if it is requested before it is assigned, a new ProcessManager will be created.
        /// </summary>
        public ProcessManager DefaultProcessManager = new ProcessManager();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the ProcessManager
        /// </summary>
        public ProcessManager()
        {

            InitializeComponent();
            // When someone creates a process manager it automatically assumes that it should be the default.
            // If this is not the case, then DefaultProcessManager should be set at a different time.
            DefaultProcessManager = this;

            // By default, this gets set to the default log manager
            _logManager = LogManager.DefaultLogManager;
        }

        
       


        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the implementation of ILogManager that this ProcessManager should use.  This does not
        /// necessarilly have to be the same log manager as the default, but if no log manager is specified,
        /// the default is used.
        /// </summary>
        public ILogManager MyLogManager
        {
            get { return _logManager; }
            set { _logManager = value; }
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
