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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 5:30:50 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using MapWindow.Plugins;
namespace MapWindow.Map
{


    /// <summary>
    /// GeoPlugin
    /// </summary>
    public interface IMapPlugin : IExtension
    {
       
        #region Methods

        /// <summary>
        /// Gives all the information about MapWindow that the plugin manager has.
        /// Since the availability of all of these aspects will vary based on
        /// what the developer has linked with his plugin manager as well as what
        /// controls are actually available on his project, it is possible for
        /// any of these items to be null.
        /// </summary>
        void Initialize(IMapPluginArgs args);

        #endregion




    }
}
