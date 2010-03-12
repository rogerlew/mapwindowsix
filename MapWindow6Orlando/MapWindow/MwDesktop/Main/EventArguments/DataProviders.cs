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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2008 4:50:33 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using MapWindow.Plugins;
namespace MapWindow.Main
{


    /// <summary>
    /// DataProviders
    /// </summary>
    public class DataProviders : EventArgs
    {
        #region Private Variables

        private List<IDataProvider> _providers;

       

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DataProviders
        /// </summary>
        /// <param name="providers">Specifies a list of IDataProviders</param>
        public DataProviders(List<IDataProvider> providers)
        {
            _providers = providers;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties


        /// <summary>
        /// Gets the list of providers for this event.
        /// </summary>
        public virtual List<IDataProvider> Providers
        {
            get { return _providers; }
            protected set { _providers = value; }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        #region Private Helper Functions

        #endregion

    }
}
