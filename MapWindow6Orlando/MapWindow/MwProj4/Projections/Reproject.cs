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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/24/2009 9:10:23 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;

namespace MapWindow.Projections
{


    /// <summary>
    /// Reproject
    /// </summary>
    public static class Reproject 
    {
        #region Private Variables

        private const double Eps = 1E-12;
        private const int X = 0;
        private const int Y = 1;
        private const int Lam = 0;
        private const int Phi = 1;

        #endregion

        #region Constructors

      

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public static void ReprojectPoints(double[] xy, double[] z, ProjectionInfo source, ProjectionInfo dest, int startIndex, int numPoints)
        {
            

            double toMeter = source.Unit.Meters;
          
            // Geocentric coordinates are centered at the core of the earth.  Z is up toward the north pole.
            // The X axis goes from the center of the earth through greenwich.
            // The Y axis passes through 90E. 
            // This section converts from geocentric coordinates to geodetic ones if necessary.
            if (source.IsGeocentric)  
            {
                if (z == null)
                {
                    throw new ProjectionException(45);
                }
                for (int i = startIndex; i < numPoints; i++)
                {
                    if(toMeter != 1)
                    {
                        xy[i * 2] *= toMeter;
                        xy[i * 2 + 1] *= toMeter;
                    }
                }
                GeocentricGeodetic g = new GeocentricGeodetic(source.GeographicInfo.Datum.Spheroid);
                g.GeocentricToGeodetic(xy, z, startIndex, numPoints);
            }

            // Transform source points to lam/phi if they are not already
            ConvertToLatLon(source, xy, startIndex, numPoints);
            
            double fromGreenwich = source.GeographicInfo.Meridian.Longitude * source.GeographicInfo.Unit.Radians;
            if(fromGreenwich != 0)
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (xy[2* i] != double.PositiveInfinity)
                        xy[2 * i] += fromGreenwich;
                }
            }
            // DATUM TRANSFORM IF NEEDED

            if(!source.GeographicInfo.Datum.Matches(dest.GeographicInfo.Datum))
            {
                DatumTransform(source, dest, xy, z, startIndex, numPoints);
            }
            


            // Adjust to new prime meridian if there is one in the destination cs
            fromGreenwich = dest.GeographicInfo.Meridian.Longitude*dest.GeographicInfo.Unit.Radians;
            if(fromGreenwich != 0)
            {
                for(int i = startIndex; i < numPoints; i++)
                {
                    if(xy[i * 2] != double.PositiveInfinity)
                    {
                        xy[i * 2 +1] -= fromGreenwich;
                    }
                }
            }

            if(dest.IsGeocentric)
            {
                if (z == null)
                {
                    throw new ProjectionException(45);
                }
                GeocentricGeodetic g = new GeocentricGeodetic(dest.GeographicInfo.Datum.Spheroid);
                g.GeodeticToGeocentric(xy, z, startIndex, numPoints);
                double frmMeter = 1/dest.Unit.Meters;
                if(frmMeter != 1)
                {
                    for (int i = startIndex; i < numPoints; i++)
                    {
                        if (xy[i * 2] != double.PositiveInfinity)
                        {
                            xy[i * 2] *= frmMeter;
                            xy[i * 2+1] *= frmMeter;
                        }
                    }
                }
            }
            else
            {
                ConvertToProjected(dest, xy, startIndex, numPoints);
            }    
        }

        private static void ConvertToProjected(ProjectionInfo dest, double[] xy, int startIndex, int numPoints)
        {
            double frmMeter = 1/dest.Unit.Meters;
            bool geoc = dest.Geoc;
            double lam0 = dest.GetLam0();
            double roneEs = 1/(1 - dest.GeographicInfo.Datum.Spheroid.EccentricitySquared());
            bool over = dest.Over;
            double x0 = 0;
            double y0 = 0;
            if(dest.FalseEasting != null) x0 = dest.FalseEasting.Value;
            if(dest.FalseNorthing != null) y0 =  dest.FalseNorthing.Value;
            double a = dest.GeographicInfo.Datum.Spheroid.EquatorialRadius;
            for(int i = startIndex; i < numPoints; i++)
            {
                double lam = xy[2*i];
                double phi = xy[2*i + 1];
                double t = Math.Abs(phi - Math.PI/2);
                if (t < Eps || Math.Abs(lam) > 10)
                {
                    xy[2 * i] = double.PositiveInfinity;
                    xy[2 * i + 1] = double.PositiveInfinity;
                    continue;
                }
                if (Math.Abs(t) <= Eps)
                {
                    xy[2 * i + 1] = phi < 0 ? -Math.PI / 2 : Math.PI / 2;
                }
                else if(geoc)
                {
                    xy[2 * i + 1] = Math.Atan(roneEs * Math.Tan(phi));
                }
                xy[2 * i] -= lam0;
                if (!over)
                {
                    xy[2 * i] = Adjlon(xy[2 * i]);
                }
                
            }

            // break this out because we don't want a chatty call to extension transforms
            dest.Transform.Forward(xy, startIndex, numPoints);
            
            for (int i = startIndex; i < numPoints; i++)
            {

                xy[2 * i] = frmMeter * (a * xy[2 * i] + x0);
                xy[2 * i + 1] = frmMeter * (a * xy[2 * i + 1] + y0);
                
            }
        }

        private static void DatumTransform(ProjectionInfo source, ProjectionInfo dest, double[] xy, double[] z, int startIndex, int numPoints)
        {
            Spheroid wgs84 = new Spheroid(Proj4Ellipsoids.WGS_1984);
            Datum sDatum = source.GeographicInfo.Datum;
            Datum dDatum = dest.GeographicInfo.Datum;
            
            /* -------------------------------------------------------------------- */
            /*      We cannot do any meaningful datum transformation if either      */
            /*      the source or destination are of an unknown datum type          */
            /*      (ie. only a +ellps declaration, no +datum).  This is new        */
            /*      behavior for PROJ 4.6.0.                                        */
            /* -------------------------------------------------------------------- */
            if( sDatum.DatumType == DatumTypes.Unknown ||
                dDatum.DatumType == DatumTypes.Unknown) return;

            /* -------------------------------------------------------------------- */
            /*      Short cut if the datums are identical.                          */
            /* -------------------------------------------------------------------- */
            if (sDatum.Matches(dDatum)) return;

            double srcA = sDatum.Spheroid.EquatorialRadius;
            double srcEs = sDatum.Spheroid.EccentricitySquared();

            double dstA = dDatum.Spheroid.EquatorialRadius;
            double dstEs = dDatum.Spheroid.EccentricitySquared();



            /* -------------------------------------------------------------------- */
            /*      Create a temporary Z value if one is not provided.              */
            /* -------------------------------------------------------------------- */
            if (z == null)
            {
                z = new double[xy.Length / 2];
            }

            /* -------------------------------------------------------------------- */
            /*	If this datum requires grid shifts, then apply it to geodetic   */
            /*      coordinates.                                                    */
            /* -------------------------------------------------------------------- */
            if( sDatum.DatumType == DatumTypes.GridShift)
            {

//        pj_apply_gridshift( pj_param(srcdefn->params,"snadgrids").s, 0, 
//                            point_count, point_offset, x, y, z );

                GridShift.Apply(source.GeographicInfo.Datum.NadGrids, false, xy, startIndex, numPoints);

                srcA = wgs84.EquatorialRadius;
                srcEs = wgs84.EccentricitySquared();
            }

            if (dDatum.DatumType == DatumTypes.GridShift)
            {
                dstA = wgs84.EquatorialRadius;
                dstEs = wgs84.EccentricitySquared();
            }

            /* ==================================================================== */
            /*      Do we need to go through geocentric coordinates?                */
            /* ==================================================================== */

            if( srcEs != dstEs || srcA != dstA
                || sDatum.DatumType == DatumTypes.Param3 
                || sDatum.DatumType == DatumTypes.Param7
                || dDatum.DatumType == DatumTypes.Param3 
                || dDatum.DatumType == DatumTypes.Param7)
            {
                /* -------------------------------------------------------------------- */
                /*      Convert to geocentric coordinates.                              */
                /* -------------------------------------------------------------------- */
               
                GeocentricGeodetic gc = new GeocentricGeodetic(sDatum.Spheroid);
                gc.GeodeticToGeocentric(xy, z, startIndex, numPoints);


                /* -------------------------------------------------------------------- */
                /*      Convert between datums.                                         */
                /* -------------------------------------------------------------------- */

                if( sDatum.DatumType == DatumTypes.Param3 || sDatum.DatumType == DatumTypes.Param7)
                {
                    
                    // pj_geocentric_to_wgs84( srcdefn, point_count, point_offset,x,y,z);
   
                }
                if( dDatum.DatumType == DatumTypes.Param3 || dDatum.DatumType == DatumTypes.Param7)
                {
                    // pj_geocentric_from_wgs84( dstdefn, point_count,point_offset,x,y,z);
                }

                /* -------------------------------------------------------------------- */
                /*      Convert back to geodetic coordinates.                           */
                /* -------------------------------------------------------------------- */
                
                gc = new GeocentricGeodetic(dDatum.Spheroid);
                gc.GeodeticToGeocentric(xy, z, startIndex, numPoints);

            }

            /* -------------------------------------------------------------------- */
            /*      Apply grid shift to destination if required.                    */
            /* -------------------------------------------------------------------- */
            if(dDatum.DatumType == DatumTypes.GridShift)
            {
                //        pj_apply_gridshift( pj_param(dstdefn->params,"snadgrids").s, 1,
                //                            point_count, point_offset, x, y, z );
                GridShift.Apply(dest.GeographicInfo.Datum.NadGrids, true, xy, startIndex, numPoints);
            }

            
        }

        private static void ConvertToLatLon(ProjectionInfo source, double[] xy, int startIndex, int numPoints)
        {
            double toMeter = 1.0;
            if (source.Unit != null) toMeter = source.Unit.Meters;
            double oneEs = 1 - source.GeographicInfo.Datum.Spheroid.EccentricitySquared();
            double ra = 1 / source.GeographicInfo.Datum.Spheroid.EquatorialRadius;
            double x0 = 0;
            if (source.FalseEasting != null) x0 = source.FalseEasting.Value;
            double y0 = 0;
            if (source.FalseNorthing != null) y0 = source.FalseNorthing.Value;
            for(int i = startIndex; i < numPoints; i++)
            {
                if (xy[i * 2] == double.PositiveInfinity || xy[i * 2 + 1] == double.PositiveInfinity)
                {
                    // This might be error worthy, but not sure if we want to throw an exception here
                    continue;

                }
                // descale and de-offset
                xy[i * 2] = (xy[i * 2] * toMeter - x0) * ra;
                xy[i * 2 + 1] = (xy[i * 2 + 1] * toMeter - y0) * ra;

            }

          
            source.Transform.Inverse(xy, startIndex, numPoints);

            for (int i = startIndex; i < numPoints; i++)
            {
                if (source.CentralMeridian != null) xy[i * 2] += source.CentralMeridian.Value * source.GeographicInfo.Unit.Radians;
                xy[i * 2] = Adjlon(xy[i * 2]);
                if (source.Geoc && Math.Abs(Math.Abs(xy[i * 2 + 1]) - Math.PI / 2) > Eps)
                {
                    xy[i * 2 + 1] = Math.Atan(oneEs * Math.Tan(xy[i * 2 + 1]));
                }
                
            }
        }

        private static double Adjlon(double lon)
        {
            if (Math.Abs(lon) <= Math.PI) return (lon);
            lon += Math.PI;  /* adjust to 0..2pi rad */
            lon -= 2 * Math.PI * Math.Floor(lon / (2 * Math.PI)); /* remove integral # of 'revolutions'*/
            lon -= Math.PI;  /* adjust back to -pi..pi rad */
            return (lon);
        }

       

        #endregion

        #region Properties



        #endregion




      
    }
}
