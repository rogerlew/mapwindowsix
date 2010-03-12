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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/14/2009 9:24:40 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;

namespace MapWindow.Drawing
{


    /// <summary>
    /// IColorSymbol
    /// </summary>
    public interface IColorable
    {
      
        #region Properties

        /// <summary>
        /// Gets or sets the Color
        /// </summary>
        Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the opacity
        /// </summary>
        float Opacity
        {
            get;
            set;
        }


        #endregion



    }
}
