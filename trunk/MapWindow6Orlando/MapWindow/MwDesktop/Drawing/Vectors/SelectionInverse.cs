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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2009 3:48:20 PM
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
    /// Selection
    /// </summary>
    public class SelectionInverse : FeatureSelection, ISelection
    {
       
        #region Constructors

        /// <summary>
        /// Creates a new instance of Selection
        /// </summary>
        public SelectionInverse(IFeatureSet fs, IDrawingFilter inFilter):base(fs, inFilter, FilterTypes.Selection)
        {
            Selected = false;
            UseSelection = true;
            UseCategory = false;
            UseVisibility = false;
            UseChunks = false;
            SelectionMode = SelectionModes.IntersectsExtent;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inverts the selection based on the current SelectionMode
        /// </summary>
        /// <param name="region">The geographic region to reverse the selected state</param>
        /// <param name="affectedArea">The affected area to invert</param>
        public bool InvertSelection(IEnvelope region, out IEnvelope affectedArea)
        {
            SuspendChanges();
            const bool flipped = false;
            affectedArea = new Envelope();



            IDictionary<IFeature, IDrawnState> states = Filter.DrawnStates;
            foreach (KeyValuePair<IFeature, IDrawnState> kvp in states)
            {
                bool doFlip = false;
                IFeature f = kvp.Key;
                if (SelectionMode == SelectionModes.IntersectsExtent)
                {
                    if (region.Intersects(f.Envelope))
                    {
                        kvp.Value.IsSelected = !kvp.Value.IsSelected;
                        affectedArea.ExpandToInclude(f.Envelope);
                    }
                }
                else if (SelectionMode == SelectionModes.ContainsExtent)
                {
                    if (region.Contains(f.Envelope))
                    {
                        kvp.Value.IsSelected = !kvp.Value.IsSelected;
                        affectedArea.ExpandToInclude(f.Envelope);
                    }
                }
                IPolygon reg = region.ToPolygon();
                IGeometry geom = Geometry.FromBasicGeometry(f.BasicGeometry);
                switch (SelectionMode)
                {
                    case SelectionModes.Contains:
                        if (region.Intersects(f.Envelope))
                        {
                            if (reg.Contains(geom)) doFlip = true;
                        }
                        break;
                    case SelectionModes.CoveredBy:
                        if (reg.CoveredBy(geom)) doFlip = true;
                        break;
                    case SelectionModes.Covers:
                        if (reg.Covers(geom)) doFlip = true;
                        break;
                    case SelectionModes.Disjoint:
                        if (reg.Disjoint(geom)) doFlip = true;
                        break;
                    case SelectionModes.Intersects:
                        if (region.Intersects(f.Envelope))
                        {
                            if (reg.Intersects(geom)) doFlip = true;
                        }
                        break;
                    case SelectionModes.Overlaps:
                        if (reg.Overlaps(geom)) doFlip = true;
                        break;
                    case SelectionModes.Touches:
                        if (reg.Touches(geom)) doFlip = true;
                        break;
                    case SelectionModes.Within:
                        if (reg.Within(geom)) doFlip = true;
                        break;
                }
                if (doFlip)
                {
                    kvp.Value.IsSelected = !kvp.Value.IsSelected;
                    affectedArea.ExpandToInclude(f.Envelope);
                }
            }
            ResumeChanges();
            return flipped;
        }


        #endregion
    }
}
