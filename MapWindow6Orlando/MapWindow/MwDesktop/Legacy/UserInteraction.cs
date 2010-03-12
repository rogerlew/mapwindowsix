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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/21/2009 3:51:16 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
namespace MapWindow.Legacy
{


    /// <summary>
    /// UserInteraction
    /// </summary>
    public class UserInteraction : IUserInteraction
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of UserInteraction
        /// </summary>
        public UserInteraction()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        ///  Prompt the user to select a projection, and return the PROJ4 representation of this
        ///  projection. Specify the dialog caption and an optional default projection ("" for none).
        /// </summary>
        /// <param name="DialogCaption">The text to be displayed on the dialog, e.g. "Please select a projection."</param>
        /// <param name="DefaultProjection">The PROJ4 projection string of the projection to default to, "" for none.</param>
        /// <returns></returns>
        public string GetProjectionFromUser(string DialogCaption, string DefaultProjection)
        {
            bool TO_DO_PROJECTION_FORM = true;
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Retrieve a color ramp, defined by a start and end color, from the user.
        /// </summary>
        /// <param name="suggestedStart">The start color to initialize the dialog with.</param>
        /// <param name="suggestedEnd">The end color to initialize the dialog with.</param>
        /// <param name="selectedEnd">The end color that the user selected.</param>
        /// <param name="selectedStart">The start color that the user selected.</param>
        /// <returns></returns>
        public bool GetColorRamp(Color suggestedStart, Color suggestedEnd, out Color selectedStart, out Color selectedEnd)
        {
            bool TO_DO_SHOW_COLOR_RAMP_DIALOG = true;
            throw new NotImplementedException();
        }

        #endregion

       



    }
}
