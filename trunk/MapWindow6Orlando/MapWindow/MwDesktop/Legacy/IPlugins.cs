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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:50:01 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Legacy
{


    /// <summary>
    /// Interface for manipulating plugins of the IPlugin type.
    /// This differs from "CondensedPlugins" in that the Item ([]) property returns IPlugin
    /// instead of IPluginDetails. The original "Plugins" (this one) cannot be changed without breaking
    /// the backward compatibility of the interface.
    /// </summary>
    public interface IPlugins : System.Collections.IEnumerable
    {
        /// <summary>
        /// clears all plugins from the list of available plugins, but doesn't unload loaded plugins
        /// </summary>
        void Clear();
        /// <summary>
        /// Add a plugin from a file
        /// </summary>
        /// <param name="Path">path to the plugin</param>
        /// <returns>true on success, false on failure</returns>
        bool AddFromFile(string Path);
        /// <summary>
        /// Adds any compatible plugins from a directory(recursive into subdirs)
        /// </summary>
        /// <param name="Path">path to the directory</param>
        /// <returns>true on success, false otherwise</returns>
        bool AddFromDir(string Path);
        /// <summary>
        /// Loads a plugin from an instance of an object
        /// </summary>
        /// <param name="Plugin">the Plugin object to load</param>
        /// <param name="PluginKey">The Key by which this plugin can be identified at a later time</param>
        /// <param name="SettingsString">A string that contains any settings that should be passed to the plugin after it is loaded into the system</param>
        /// <returns>true on success, false otherwise</returns>
        bool LoadFromObject(IPlugin Plugin, string PluginKey, string SettingsString);
        /// <summary>
        /// Loads a plugin from an instance of an object
        /// </summary>
        /// <param name="Plugin">the Plugin object to load</param>
        /// <param name="PluginKey">The Key by which this plugin can be identified at a later time</param>
        /// <returns>true on success, false otherwise</returns>
        bool LoadFromObject(IPlugin Plugin, string PluginKey);
        /// <summary>
        /// Starts (loads) a specified plugin
        /// </summary>
        /// <param name="Key">Identifying key for the plugin to start</param>
        /// <returns>true on success, false otherwise</returns>
        bool StartPlugin(string Key);
        /// <summary>
        /// Stops (unloads) a specified plugin
        /// </summary>
        /// <param name="Key">Identifying key for the plugin to stop</param>
        void StopPlugin(string Key);
        /// <summary>
        /// number of available plugins
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets an IPlugin object from the list of all loaded plugins
        /// <param name="Index">0-based index into the list of plugins</param>
        /// </summary>
        IPlugin this[int Index] { get; }

        /// <summary>
        /// Removes a plugin from the list of available plugins and unloads the plugin if loaded
        /// </summary>
        /// <param name="IndexOrKey">0-based integer index or string key for the plugin to remove</param>
        void Remove(object IndexOrKey);

        /// <summary>
        /// Gets or Sets the default folder where plugins are loaded from 
        /// </summary>
        string PluginFolder { get; set; }

        /// <summary>
        /// Checks to see if a plugin is currently loaded (running)
        /// </summary>
        /// <param name="Key">Unique key identifying the plugin</param>
        /// <returns>true if loaded, false otherwise</returns>
        bool PluginIsLoaded(string Key);

        /// <summary>
        /// Shows the dialog for loading/starting/stopping plugins
        /// </summary>
        void ShowPluginDialog();

        /// <summary>
        /// Sends a broadcast message to all loaded plugins
        /// </summary>
        /// <param name="Message">The message that should be sent</param>
        void BroadcastMessage(string Message);

        /// <summary>
        /// Returns the key belonging to a plugin with the given name. An empty string is returned if the name is not found.
        /// </summary>
        /// <param name="PluginName">The name of the plugin</param>
        string GetPluginKey(string PluginName);
    }
}
