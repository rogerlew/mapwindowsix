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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/15/2009 1:28:47 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************



namespace MapWindow.Plugins
{


    /// <summary>
    /// A provider is the base class that enables plug-ins to work with the Application Manager.
    /// This is true whether it is a plug-in or a data provider or some other extension.
    /// </summary>
    public class Extension
    {
        #region Private Variables

        private bool _enabled;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Provider
        /// </summary>
        public Extension()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Activates this provider
        /// </summary>
        public void Activate()
        {
            OnActivate();
        }

        /// <summary>
        /// Deactivates this provider
        /// </summary>
        public void Deactivate()
        {
            OnDeactivate();
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean that is true if the extension is active and running.
        /// </summary>
        public bool IsEnabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        
        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets or sets whether or not this provider is active
        /// </summary>
        protected virtual void OnActivate()
        {
            _enabled = true;
        }

        /// <summary>
        /// Gets or sets whether or not this 
        /// </summary>
        protected virtual void OnDeactivate()
        {
            _enabled = false;
        }

        #endregion
    }
}
