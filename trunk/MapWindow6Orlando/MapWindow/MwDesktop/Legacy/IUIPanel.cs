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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:38:33 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Windows.Forms;

namespace MapWindow.Legacy
{
    /// <summary>
    /// A function that is called upon close of a panel. The
    /// name (caption) of the closed panel is passed into the
    /// OnPanelClose function.
    /// </summary>
    public delegate void OnPanelClose(string Caption);

    /// <summary>
    /// UIPanel
    /// </summary>
    public interface IUIPanel
    {
      

        #region Methods

        /// <summary>
        /// Adds a function (OnCloseFunction) which
        /// is called when the panel specified by Caption is closed.
        /// </summary>
        void AddOnCloseHandler(string Caption, OnPanelClose OnCloseFunction);

        /// <summary>
        /// Returns a System.Windows.Forms.Panel that can be used to add dockable content to MapWindow.
        /// </summary>
        Panel CreatePanel(string Caption, MapWindowDockStyles DockStyle);

        /// <summary>
        /// Returns a System.Windows.Forms.Panel that can be used to add dockable content to MapWindow.
        /// </summary>
        Panel CreatePanel(string Caption, DockStyle DockStyle);

        /// <summary>
        /// Deletes the specified panel.
        /// </summary>
        void DeletePanel(string Caption);

        /// <summary>
        /// Hides or shows a panel without necessarily deleting it.
        /// </summary>
        void SetPanelVisible(string Caption, bool Visible);

       

        #endregion

     
       

    }
}
