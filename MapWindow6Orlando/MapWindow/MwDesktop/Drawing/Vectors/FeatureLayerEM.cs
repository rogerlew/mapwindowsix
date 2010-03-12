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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/23/2009 4:23:30 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// FeatureLayerEM
    /// </summary>
    public static class FeatureLayerEM
    {
      
        #region Methods

        /// <summary>
        /// Inverts the selection
        /// </summary>
        /// <param name="featureLayer"></param>
        public static void InvertSelection(this IFeatureLayer featureLayer)
        {
            IEnvelope ignoreMe;
            featureLayer.InvertSelection(featureLayer.Envelope, featureLayer.Envelope, SelectionModes.IntersectsExtent,
                                         out ignoreMe);
        }

       

       
     

        #endregion

        #region Properties



        #endregion



    }
}
