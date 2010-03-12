//********************************************************************************************************
// Product Name: MapWindow.exe Alpha
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
// The Original Code is from MapWindow.exe version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/03/2008 5:44:01 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace MapWindow
{
    /// <summary>
    /// A class to store the setting information to a file.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Creates a new setting situation by loading from the filename if it exists.  Otherwise a default is created.
        /// </summary>
        /// <param name="filename"></param>
        public Settings(string filename)
        {
            _filename = filename;
            if (_filename != null && File.Exists(_filename))
            {
                _settingInfo = Load(_filename);
            }
            else
            {
                _settingInfo = new SettingInfo();
            }
            
        }

        private static string _filename;
        private static SettingInfo _settingInfo = null;

        /// <summary>
        /// Gets or sets the setting info.  Setting this will also save a copy.
        /// </summary>
        public static SettingInfo Info
        {
            get
            {
                if (_settingInfo == null)
                {
                    if (_filename != null && File.Exists(_filename))
                    {
                        _settingInfo = Load(_filename);
                    }
                    else
                    {
                        _settingInfo = new SettingInfo();
                    }
                    
                }
                return _settingInfo;
            }
            set
            {
                _settingInfo = value;
                if (_filename != null)
                {
                    Save(value, _filename);   
                }

            }
        }

        /// <summary>
        /// Gets or sets the filename where the settings are stored.
        /// </summary>
        public static string Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }

        const int VERSION = 1;
        private static void Save(SettingInfo settings, string fileName)
        {
            Stream stream = null;
            try
            {
                if(Directory.Exists(Path.GetDirectoryName(fileName)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, VERSION);
                formatter.Serialize(stream, settings);
            }
            catch(Exception ex)
            {
                Debug.Write(ex.ToString());
                // do nothing, just ignore any possible errors
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
        }

        private static SettingInfo Load(string fileName)
        {
            Stream stream = null;
            SettingInfo settings = null;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                int version = (int)formatter.Deserialize(stream);
                Debug.Assert(version == VERSION);
                settings = (SettingInfo)formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                // do nothing, just ignore any possible errors
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
            return settings;
        }
    }
    
}
