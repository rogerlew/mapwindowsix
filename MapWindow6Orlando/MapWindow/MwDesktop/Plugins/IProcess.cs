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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2008 10:04:01 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using MapWindow.Main;
namespace MapWindow.Plugins
{


    /// <summary>
    /// IProcess
    /// </summary>
    public interface IProcess: IProgressHandler
    {
      
        #region Methods


        /// <summary>
        /// If any parameters are required, this is where the Process should provide
        /// a dialog to obtain those parameters from the user.
        /// </summary>
        /// <returns></returns>
        IAsyncResult StartProcess();

        /// <summary>
        /// The parameters object is a dictionary with a key that is the string, parameter name and the value being
        /// an object which is the value of the parameter.  The types should match the types listed for each parameter
        /// in the Parameters property.
        /// </summary>
        /// <param name="parameters">A Dictionary &lt;string, object&gt; of object parameters to pass to this process.</param>
        /// <returns>An IAsyncResult interface which can be used for working with the asynchronous process. </returns>
        IAsyncResult StartProcess(object parameters);

        /// <summary>
        /// Sends a message to the process that it has been cancelled.  This should abort the process
        /// and cause the process to throw the processCompleted event.
        /// </summary>
        void Cancel(IAsyncResult status);
      
        /// <summary>
        /// This blocks further code execution until this process has finished.
        /// </summary>
        object EndProcess(IAsyncResult status);
        


        #endregion



        #region Properties

        /// <summary>
        /// This should return true if the process should be run as a background process, or false if the process should
        /// be modal.  If the process is Asynchronous then the ProcessCompleted event returns the result.
        /// </summary>
        bool IsAsynchronous
        {
            get;
            set;
        }

        /// <summary>
        /// If IsAsynchronous is true, but multiple, simultaneous invokations are not allowed, IsBusy can be used
        /// to indicate that the process is not ready for another call.
        /// </summary>
        bool IsBusy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the unique ID identifying this process.  This will be assigned to the process by the
        /// ProcessManager.
        /// </summary>
        object UserState
        {
            get;
            set;
        }

        /// <summary>
        /// Describes the parameters necessary for this process.  The key is the string parameter name, and the
        /// value is the type for the parameter.
        /// </summary>
        /// <remarks>This part is purely speculative.  The model-builder will force activities here.</remarks>
        Dictionary<string, Type> Parameters
        {
            get;
        }

       


        #endregion

        #region Event

        /// <summary>
        /// Occurs when this process is actually started.  This is mostly useful for logging.
        /// </summary>
       event EventHandler<ProcessStartedArgs> ProcessStarted;

        /// <summary>
        /// Occurs when this process has either been cancelled or exits normally.
        /// </summary>
        event EventHandler<ProcessCompletedArgs> ProcessCompleted;


        /// <summary>
        /// Occurs when some progress has been made by this process
        /// </summary>
        event EventHandler<ProgressChangedArgs> ProgressChanged;


        #endregion

    }
}
