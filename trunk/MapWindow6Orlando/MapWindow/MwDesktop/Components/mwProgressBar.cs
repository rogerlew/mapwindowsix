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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 5:05:40 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using System.Windows.Forms;
using MapWindow.Main;


namespace MapWindow.Components
{


    /// <summary>
    /// mwProgressBar
    /// </summary>
    [ToolboxBitmap(typeof(mwToolStrip), "Components.mwProgressBar.ico")]
    public class mwProgressBar : ProgressBar, IProgressHandler
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of mwProgressBar
        /// </summary>
        public mwProgressBar()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is thread safe so that people calling this method don't cause a cross-thread violation
        /// by updating the progress indicator from a different thread
        /// </summary>
        /// <param name="key">A string message with just a description of what is happening, but no percent completion information</param>
        /// <param name="percent">The integer percent from 0 to 100</param>
        /// <param name="message">A message</param>
        public void Progress(string key, int percent, string message)
        {
            if (InvokeRequired)
            {
                UpdateProg prg = UpdateProgress;
                BeginInvoke(prg, new object[] { key, percent, message });
            }
            else
            {
                UpdateProgress(key, percent, message);
            }

        }

        private delegate void UpdateProg(string key, int percent, string message);

        private void UpdateProgress(string key, int percent, string message)
        {
            Value = percent;
        }

        #endregion

        #region Properties



        #endregion



    }
}
