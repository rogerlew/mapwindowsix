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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2009 4:16:28 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using MapWindow.Data;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// IFilterCollection
    /// </summary>
    public interface IFilterCollection : ICollection<IFeature>, IChangeable
    {

        

        #region Methods

    
      
       
        /// <summary>
        /// This uses extent checking (rather than full polygon intersection checking).  It will add
        /// any members that are either contained by or intersect with the specified region
        /// depending on the SelectionMode property.  The order of operation is the region
        /// acting on the feature, so Contains, for instance, would work with points.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea">The affected area of this addition</param>
        /// <returns>True if any item was actually added to the collection</returns>
        bool AddRegion(IEnvelope region, out IEnvelope affectedArea);

     
        /// <summary>
        /// Tests each member currently in the selected features based on 
        /// the SelectionMode.  If it passes, it will remove the feature from
        /// the selection.
        /// </summary>
        /// <param name="region">The geographic region to remove</param>
        /// <param name="affectedArea">A geographic area that was affected by this change.</param>
        /// <returns>Boolean, true if the collection was changed</returns>
        bool RemoveRegion(IEnvelope region, out IEnvelope affectedArea);
        

       /// <summary>
        /// As an example, choosing myFeatureLayer.SelectedFeatures.ToFeatureSet creates a new set.
        /// </summary>
        /// <returns>An in memory featureset that has not yet been saved to a file in any way.</returns>
        FeatureSet ToFeatureSet();

        /// <summary>
        /// Exports the members of this collection as a list of IFeature.
        /// </summary>
        /// <returns>A List of IFeature</returns>
        List<IFeature> ToList();
        


        #endregion

        #region Properties

        /// <summary>
        /// Gets the envelope that represents the features in this collection.
        /// If the collection changes, this will be invalidated automatically,
        /// and the next envelope request will re-calcualte the envelope.
        /// </summary>
        IEnvelope Envelope
        {
            get;
        }

      

        /// <summary>
        /// Gets or sets the selection mode to use when Adding or Removing features
        /// from a specified envelope region.
        /// </summary>
        SelectionModes SelectionMode
        {
            get;
            set;
        }

        #endregion
    }
}
