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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2008 10:05:50 AM
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

namespace MapWindow.Main
{


    /// <summary>
    /// ProcessStartedArgs Event Arguments, mainly used for logging purposes
    /// </summary>
    public class ProcessStartedArgs : EventArgs
    {
        #region Private Variables

       
        private string _name;
        private List<string> _ParameterNames;
        private List<string> _ParameterValues;

       

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProcessStarted, but doesn't set any of the properties
        /// </summary>
        protected ProcessStartedArgs()
        {
            _name = "Unspecified";
            _ParameterNames = new List<string>();
            _ParameterValues = new List<string>();

        }

        /// <summary>
        /// Creates a new instance of ProcessStartedArgs for parameterless processes
        /// </summary>
        /// <param name="name">The name of the process</param>
        public ProcessStartedArgs(string name)
        {
            _name = name;
            _ParameterNames = new List<string>();
            _ParameterValues = new List<string>();
        }

        /// <summary>
        /// Creates a new instance of ProcessStartedArgs class for processes that have several parameters
        /// </summary>
        /// <param name="name">The name of the process to start</param>
        /// <param name="parameterNames">The list of the names of the parameters</param>
        /// <param name="parameterValues">The list of the string equivalent of the values of the parameters.</param>
        public ProcessStartedArgs(string name, List<string> parameterNames, List<string> parameterValues)
        {
            _name = name;
            _ParameterNames = parameterNames;
            _ParameterValues = parameterValues;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the name of the parameters
        /// </summary>
        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        /// <summary>
        /// Gets the names for each parameter for this process
        /// </summary>
        public List<string> ParameterNames
        {
            get { return _ParameterNames; }
            protected set { _ParameterNames = value; }
        }

        /// <summary>
        /// Gets a string version of the value for each parameter in this process
        /// </summary>
        public List<string> ParameterValues
        {
            get { return _ParameterValues; }
            protected set { _ParameterValues = value; }
        }


        #endregion



    }
}
