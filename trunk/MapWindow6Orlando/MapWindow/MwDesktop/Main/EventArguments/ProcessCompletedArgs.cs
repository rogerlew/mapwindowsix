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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2008 10:18:21 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.ComponentModel;
namespace MapWindow.Main
{


    /// <summary>
    /// This is partly for logging, but also partly for asynchronous processes.
    /// </summary>
    public class ProcessCompletedArgs: AsyncCompletedEventArgs
    {
        #region Private Variables

        private string _name;
        private DateTime _time;
        private object _result;
        private bool _resultIsValid;

        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProcessEndedArgs
        /// </summary>
        /// <param name="userState">A unique identifier for this process</param>
        protected ProcessCompletedArgs(object userState):base(null, false, userState)
        {
            _time = DateTime.Now;
            _name = "Unspecified";
           
         
         
        }

        /// <summary>
        /// Creates a ProcessCompletedArgs for the specified process name.
        /// </summary>
        /// <param name="userState">A unique identifier for this process</param>
        /// <param name="name">The name of the process that is ending.</param>
        public ProcessCompletedArgs(object userState, string name):base(null, false, userState)
        {
            _time = DateTime.Now;
            _name = name;
           
        }

        /// <summary>
        /// Creates a ProcessCompletedArgs for the specific process name
        /// </summary>
        /// <param name="userState">A unique identifier for this process</param>
        /// <param name="name">The name of the process which is completed</param>
        /// <param name="cancelled">Boolean, true if the process was cancelled.</param>
        public ProcessCompletedArgs(object userState, string name, bool cancelled):base(null, cancelled, userState)
        {
            _time = DateTime.Now;
            _name = name;
           
        }

        /// <summary>
        /// Creates a ProcessCompletedArgs for process name.  In this case, it assumes
        /// that there was no exception was thrown, and the process was not cancelled.
        /// </summary>
        /// <param name="userState">A unique identifier for this process</param>
        /// <param name="name">The name of the process which is completed</param>
        /// <param name="result">The result, if any, that was created by the process</param>
        public ProcessCompletedArgs(object userState, string name, object result):base(null, false, userState)
        {
            _time = DateTime.Now;
            _name = name;
            _result = result;
        }

        /// <summary>
        /// Creates a ProcessCompletedArgs where this process threw an exception.
        /// </summary>
        /// <param name="userState">A unique identifier for this process</param>
        /// <param name="name">The string name of the process.</param>
        /// <param name="error">The exception that caused the termination of the process.</param>
        public ProcessCompletedArgs(object userState, string name, Exception error):base(error, false, userState)
        {
            _name = name;
           
        }




        #endregion

       

        #region Properties


        /// <summary>
        /// Gets the name of the process that was completed
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        /// <summary>
        /// Gets the object result from the process, if any.  Attempting to access
        /// this after an exception or cancellation will throw an InvalidOperationException
        /// </summary>
        public object Result
        {
            get 
            {
                // Test to see if this operation failed or was cancelled, this will rase the appropriate exception.
                RaiseExceptionIfNecessary();

                return _result; 
            }
            protected set { _result = value; }
        }

        /// <summary>
        /// Gets a boolean indicating whether the process completed corrected and was not
        /// cancelled and did not throw an error.  
        /// </summary>
        public bool ResultIsValid
        {
            get { return _resultIsValid; }
            protected set { _resultIsValid = value; }
        }


        /// <summary>
        /// Gets the time when this event occured
        /// </summary>
        public virtual DateTime Time
        {
            get { return _time; }
            protected set { _time = value; }
        }

        

        


        #endregion



    }
}
