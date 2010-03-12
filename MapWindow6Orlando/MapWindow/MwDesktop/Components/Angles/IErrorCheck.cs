//********************************************************************************************************
// Product Name: SketchPad.Components.exe 
// Description:  An open source drawing pad that is super simple, but extendable
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from SketchPad.Components.exe
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/10/2008 11:13:03 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MapWindow.Components
{


    /// <summary>
    ///
    /// </summary>
    public interface IErrorCheck
    {
      
        #region Properties

        /// <summary>
        /// Boolean, true if there is an error on this device.
        /// </summary>
        bool HasError
        {
            get;
        }

        /// <summary>
        /// Specifies the current error message.
        /// </summary>
        string ErrorMessage
        {
            get;
        }

        /// <summary>
        /// Gets the cleanly formatted name for this control for an error message
        /// </summary>
        string MessageName
        {
            get;
        }

        #endregion



    }
}
