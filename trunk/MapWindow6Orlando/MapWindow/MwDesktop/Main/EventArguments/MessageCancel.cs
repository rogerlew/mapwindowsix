//********************************************************************************************************
// Product Name: MapWindow.Interfaces Alpha
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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February 2008
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace MapWindow.Main
{
    /// <summary>
    /// A set of PaintEventArgs that can be used before a drawing function in order to cancel an event.
    /// </summary>
    public class MessageCancel : System.EventArgs
    {
       
        private bool _cancel; // decides to cancel something
        private string _message; // a message of what is happening.

        /// <summary>
        /// Creates a new instance of the MessageCancel Event Arguments
        /// </summary>
        /// <param name="message">A string message to convey with this event.</param>
        public MessageCancel(string message)
        {
            _cancel = false;
            _message = message;
        }

        /// <summary>
        /// Returns a boolean specifying whether the action that caused this event should be canceled.
        /// </summary>
        public virtual bool Cancel
        {
            get
            {
                return _cancel;
            }
            set
            {
                _cancel = value;
            }
        }

        /// <summary>
        /// The message allowing someone to decide whether or not the process should be cancelled.  For instance,
        /// when writing a new file, a message might show "The file C:\bob.txt already exists, overwrite it?"
        /// </summary>
        public virtual string Message
        {
            get
            {
                return _message;
            }
            protected set
            {
                _message = value;
            }

        }



    }
}
