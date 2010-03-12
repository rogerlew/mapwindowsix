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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/15/2009 1:35:00 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************



namespace MapWindow.Plugins
{


    /// <summary>
    /// The IExtension interface represents the shared content between all providers and plugins.  This simply acts like
    /// an on-off switch for enabling or disabling the extension.
    /// </summary>
    public interface IExtension 
    {

        #region Methods

        /// <summary>
        /// Activates this extension
        /// </summary>
        void Activate();
       

        /// <summary>
        /// Deactivates this extension
        /// </summary>
        void Deactivate();
        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean that is true if the extension is active and running.
        /// </summary>
        bool IsEnabled
        {
            get;
            set;
        }


        #endregion



    }
}
