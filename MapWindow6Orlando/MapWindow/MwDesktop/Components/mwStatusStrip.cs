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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/12/2009 10:46:15 AM
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
    /// A pre-configured status strip with a thread safe Progress function
    /// </summary>
    [ToolboxBitmap(typeof(mwStatusStrip), "Components.mwStatusStrip.ico")]
    public partial class mwStatusStrip : StatusStrip, IProgressHandler
    {
        /// <summary>
        /// Creates a new instance of the StatusStrip which has a built in, thread safe Progress handler
        /// </summary>
        public mwStatusStrip()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This method is thread safe so that people calling this method don't cause a cross-thread violation
        /// by updating the progress indicator from a different thread
        /// </summary>
        /// <param name="key">A string message with just a description of what is happening, but no percent completion information</param>
        /// <param name="percent">The integer percent from 0 to 100</param>
        /// <param name="message">A message</param>
        public void Progress(string key, int percent, string message)
        {
            if(InvokeRequired)
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
            bool valueUpdated = false;
            bool messageUpdated = false;
            foreach(ToolStripItem itm in Items)
            {
                ToolStripProgressBar pb = itm as ToolStripProgressBar;
                if (pb != null)
                {
                    if (valueUpdated) continue;
                    // We found a progress bar, so update the progress
                    pb.Value = percent;
                    valueUpdated = true;
                }
                ToolStripStatusLabel sl = itm as ToolStripStatusLabel;
                if (sl != null)
                {
                    if (messageUpdated) continue;
                    sl.Text = message;
                    messageUpdated = true;
                }
            }

        }
    }
}
