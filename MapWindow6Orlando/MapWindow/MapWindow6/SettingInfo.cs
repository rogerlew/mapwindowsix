using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
namespace MapWindow
{
    /// <summary>
    /// A Class to store the preferences for this application
    /// </summary>
    [Serializable]
    public class SettingInfo
    {
        /// <summary>
        /// The Preferred Culture
        /// </summary>
        public CultureInfo PreferredCulture;

        /// <summary>
        /// Creates a new instance of this class
        /// </summary>
        public SettingInfo()
        {
            PreferredCulture = System.Globalization.CultureInfo.CurrentCulture;
           
        }


    }


}
