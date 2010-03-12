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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:51:46 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Legacy
{


   
    /// <summary>
    /// Used to manipulate the status bar at the bottom of Mapwindow.
    /// </summary>
    public interface IStatusBar
    {
        /// <summary>
        /// Gets/Sets the enabled state of the StatusBar
        /// </summary>
        bool Enabled { get; set; }
        /// <summary>
        /// Gets/Sets whether or not the StatusBar's ProgressBar should be shown
        /// </summary>
        bool ShowProgressBar { get; set; }
        /// <summary>
        /// Gets/Sets the value of the StatusBar's ProgressBar
        /// </summary>
        int ProgressBarValue { get; set; }

        /// <summary>
        /// Adds a new panel to the status bar.  This function has been deprecated.  Please use the 
        /// <c>AddPanel(Text)</c> overload.
        /// </summary>
        /// <returns>The StatusBarItem that was just added</returns>
        IStatusBarItem AddPanel();

        /// <summary>
        ///	Adds a new panel to the status bar.  This function has been deprecated.  Please use the 
        /// <c>AddPanel(Text)</c> overload.
        /// </summary>
        /// <param name="InsertAt">The index at which the panel should be added</param>
        /// <returns>The StatusBarItem that was just added</returns>
        IStatusBarItem AddPanel(int InsertAt);

        /// <summary>
        /// This is the preferred method to use to add a statusbar panel.
        /// </summary>
        /// <param name="Text">Text to display in the panel.</param>
        /// <param name="Position">Position to insert panel at.</param>
        /// <param name="Width">Width of the panel in pixels.</param>
        /// <param name="AutoSize">Panel <c>AutoSize</c> property.</param>
        /// <returns>A <c>System.Windows.Forms.StatusBarPanel</c> object.</returns>
        System.Windows.Forms.StatusBarPanel AddPanel(string Text, int Position, int Width, System.Windows.Forms.StatusBarPanelAutoSize AutoSize);

        /// <summary>
        /// Removes the specified Panel.  There must always be one panel.  If you remove the last panel, the <c>MapWindow</c> will automatically add one.
        /// </summary>
        /// <param name="Index">Zero-Based index of the panel to be removed</param>
        void RemovePanel(int Index);

        /// <summary>
        /// Removes the panel object specified.  There must always be one panel.  If you remove the last panel, the <c>MapWindow</c> will automatically add one.
        /// </summary>
        /// <param name="Panel"><c>StatusBarPanel</c> to remove.</param>
        void RemovePanel(ref System.Windows.Forms.StatusBarPanel Panel);

        /// <summary>
        /// Iterator for all panels within the StatusBar
        /// </summary>
        /// <param name="index">Index of the StatusBarItem to retrieve</param>
        IStatusBarItem this[int index] { get; }

        /// <summary>
        /// Returns the number of panels in the <c>StatusBar</c>.
        /// </summary>
        /// <returns>Number of panels in the <c>StatusBar</c>.</returns>
        int NumPanels { get; }

        /// <summary>
        /// This function makes the progress bar fit into the last panel of the status bar. Call this function whenever you change the size of ANY panel in the status bar.  You do not need to call this on <c>AddPanel</c> or <c>RemovePanel</c>.
        /// </summary>
        void ResizeProgressBar();

        
    }
}
