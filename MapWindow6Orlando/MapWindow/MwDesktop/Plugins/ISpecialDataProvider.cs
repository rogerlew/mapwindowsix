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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 9:20:39 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using MapWindow.Data;


namespace MapWindow.Plugins
{


    /// <summary>
    /// ISpecialDataProvider
    /// </summary>
    public interface ISpecialDataProvider
    {
        /// <summary>
        /// A Non-File based open.  If no DialogReadFilter is provided, MapWindow will call
        /// this method when this plugin is selected from the Add Other Data option in the
        /// file menu.
        /// </summary>
        /// <returns>An IDataSet created by this Open Method.</returns>
        IDataSet Open();

        /// <summary>
        /// Instructs the provider to do whatever it is that is necessary to create a new
        /// instance of the dataset.  This may involve dialogs or other user interface
        /// methods.
        /// </summary>
        /// <returns>An IDataSet that represents the newly created data member</returns>
        IDataSet CreateNew();
    }
}
