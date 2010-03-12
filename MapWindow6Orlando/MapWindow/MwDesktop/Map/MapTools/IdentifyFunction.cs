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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/2/2009 12:18:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Forms;
namespace MapWindow.Map
{


    /// <summary>
    /// IdentifyFunction
    /// </summary>
    public class IdentifyFunction : MapFunction
    {
        #region Private Variables

        FeatureIdentifier frmFeatureIdentifier;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of IdentifyFunction
        /// </summary>
        public IdentifyFunction(IMap inMap)
            : base(inMap)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides the OnMouseUp event to handle the situation where we are tyring to
        /// identify the vector features in the specified area.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            Coordinate c1 = e.Map.PixelToProj(new System.Drawing.Point(e.X - 8, e.Y - 8));
            Coordinate c2 = e.Map.PixelToProj(new System.Drawing.Point(e.X + 8, e.Y + 8));
            Coordinate s1 = e.Map.PixelToProj(new System.Drawing.Point(e.X - 1, e.Y - 1));
            Coordinate s2 = e.Map.PixelToProj(new System.Drawing.Point(e.X + 1, e.Y + 1));
            Coordinate center = e.Map.PixelToProj(new System.Drawing.Point(e.X, e.Y));
            IEnvelope tolerant = new Envelope(c1, c2);
            IEnvelope strict = new Envelope(s1, s2);

            if (frmFeatureIdentifier == null)
            {
                frmFeatureIdentifier = new FeatureIdentifier();
            }
            frmFeatureIdentifier.SuspendLayout();
            frmFeatureIdentifier.Clear();
            Identify(e.Map.MapFrame.GetLayers(), strict, tolerant);
            frmFeatureIdentifier.ReSelect();
            frmFeatureIdentifier.ResumeLayout();
            frmFeatureIdentifier.Show();
            base.OnMouseUp(e);
        }

        private void Identify(IEnumerable<ILayer> layers, IEnvelope strict, IEnvelope tolerant)
        {
            foreach (IMapLayer layer in layers)
            {
                IGroup grp = layer as IGroup;
                if (grp != null)
                {
                    Identify(grp, strict, tolerant);
                }
                else
                {
                    IMapFeatureLayer gfl = layer as IMapFeatureLayer;
                    if (gfl != null)
                    {
                        if (gfl.DataSet.FeatureType == FeatureTypes.Polygon)
                        {
                            frmFeatureIdentifier.Add(gfl, strict);
                        }
                        else
                        {
                            frmFeatureIdentifier.Add(gfl, tolerant);
                        }
                    }
                }

            }
        }

        #endregion

        #region Properties



        #endregion



    }
}
