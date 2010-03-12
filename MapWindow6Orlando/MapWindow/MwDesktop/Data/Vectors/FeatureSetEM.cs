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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/16/2009 2:15:12 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using System.Data;
using MapWindow.Geometries;
using MapWindow.Main;
namespace MapWindow.Data
{


    /// <summary>
    /// FeatureSetEM contains extension methods that should work for any IFeatureSet
    /// </summary>
    public static class FeatureSetEM
    {

        /// <summary>
        /// Creates a new polygon featureset that is created by buffering each of the individual shapes.
        /// </summary>
        /// <param name="self">The IFeatureSet to buffer</param>
        /// <param name="distance">The double distance to buffer</param>
        /// <param name="copyAttributes">Boolean, if this is true, then the new featureset will have 
        /// the same attributes as the original.</param>
        /// <returns>The newly created IFeatureSet</returns>
        public static IFeatureSet Buffer(this IFeatureSet self, double distance, bool copyAttributes)
        {
            // Dimension the new, output featureset.  Buffered shapes are polygons, even if the 
            // original geometry is a point or a line.
            IFeatureSet result = new FeatureSet(FeatureTypes.Polygon);


            // Cycle through the features, and buffer each one separately.
            foreach (IFeature original in self.Features)
            {
                // Actually calculate the buffer geometry.
                IFeature buffer = original.Buffer(distance);

                // If copyAttributes is true, then this will copy those attributes from the original.
                if(copyAttributes)
                {
                    // Accessing the attributes should automatically load them from the datasource if
                    // they haven't been loaded already.
                    buffer.CopyAttributes(original);
                }

                // Add the resulting polygon to the featureset
                result.Features.Add(buffer);
            }
            return result;
        }
        
        
        /// <summary>
        /// Generates an empty featureset that has the combined fields from this featureset
        /// and the specified featureset.
        /// </summary>
        /// <param name="self">This featureset</param>
        /// <param name="other">The other featureset to combine fields with.</param>
        /// <returns></returns>
        public static IFeatureSet CombinedFields(this IFeatureSet self, IFeatureSet other)
        {
            IFeatureSet result = new FeatureSet(self.FeatureType);
            Dictionary<string, DataColumn> resultColumns = new Dictionary<string, DataColumn>();
            foreach (DataColumn dc in self.DataTable.Columns)
            {
                string name = dc.ColumnName;
                int i = 1;
                while (resultColumns.ContainsKey(name))
                {
                    name = dc.ColumnName + i;
                    i++;
                }
                resultColumns.Add(name, dc);
            }
            foreach (DataColumn dc in other.DataTable.Columns)
            {
                string name = dc.ColumnName;
                int i = 1;
                while (resultColumns.ContainsKey(name))
                {
                    name = dc.ColumnName + i;
                    i++;
                }
                resultColumns.Add(name, dc);
            }
            foreach (KeyValuePair<string, DataColumn> pair in resultColumns)
            {
                result.DataTable.Columns.Add(new DataColumn(pair.Key, pair.Value.DataType));
            }
            return result;
        }

        /// <summary>
        /// Calculates a union of any features that have a common value in the specified field.
        /// The output will only contain the specified field.  Example: Disolving a county 
        /// shapefile based on state name to produce a single polygon shaped like the state.
        /// </summary>
        /// <param name="self">The original featureSet to disolve the features of</param>
        /// <param name="fieldName">The string field name to use for the disolve operation</param>
        /// <returns>A featureset where the geometries of features with the same attribute in the specified field have been combined.</returns>
        public static IFeatureSet Dissolve(this IFeatureSet self, string fieldName)
        {
            Dictionary<object, IFeature> resultFeatures = new Dictionary<object, IFeature>();
            IFeatureSet result = new FeatureSet(self.FeatureType);
            result.DataTable.Columns.Add(fieldName, self.DataTable.Columns[fieldName].DataType);

            foreach (IFeature feature in self.Features)
            {
                object value = feature.DataRow[fieldName];
                if(resultFeatures.ContainsKey(value))
                {
                    IFeature union = resultFeatures[value].Union(feature);
                    union.DataRow = result.DataTable.NewRow();
                    union.DataRow[fieldName] = value;
                    resultFeatures[value] = union;
                }
                else
                {
                    IFeature union = feature.Copy();
                    union.DataRow = result.DataTable.NewRow();
                    union.DataRow[fieldName] = value;
                    resultFeatures.Add(value, feature);
                }
            }
           
            foreach (IFeature feature in resultFeatures.Values)
            {
                result.Features.Add(feature);
            }
            return result;
        }
        
        /// <summary>
        /// Tests to see if this feature intersects with the specified envelope
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="env">The envelope to test</param>
        /// <returns>Boolean, true if the intersection occurs</returns>
        public static bool Intersects(this IFeature self, IEnvelope env)
        {
            return self.Envelope.Intersects(env) && Geometry.FromBasicGeometry(self.BasicGeometry).Intersects(env.ToPolygon());
        }


        /// <summary>
        /// This tests each feature of the input  
        /// </summary>
        /// <param name="self">This featureSet</param>
        /// <param name="other">The featureSet to perform intersection with</param>
        /// <param name="joinType">The attribute join type</param>
        /// <param name="progHandler">A progress handler for status messages</param>
        /// <returns>An IFeatureSet with the intersecting features, broken down based on the join Type</returns>
        public static IFeatureSet Intersection(this IFeatureSet self, IFeatureSet other, FieldJoinType joinType, IProgressHandler progHandler)
        {
          
            IFeatureSet result = null;
            ProgressMeter pm = new ProgressMeter(progHandler, "Calculating Intersection", self.Features.Count);
            if (joinType == FieldJoinType.All)
            {
                result = CombinedFields(self, other);
                // Intersection is symetric, so only consider I X J where J <= I
                int i=0;
                foreach(IFeature selfFeature in self.Features)
                {
                    List<IFeature> potentialOthers = other.Select(selfFeature.Envelope);
                    foreach (IFeature otherFeature in potentialOthers)
                    {
                        selfFeature.Intersection(otherFeature, result, joinType);
                        
                    }
                    pm.CurrentValue = i;
                    i++;
                }
                pm.Reset();
            }
            if (joinType == FieldJoinType.LocalOnly)
            {
                result = new FeatureSet();
                result.CopyTableSchema(self);
                result.FeatureType = self.FeatureType;
                IFeature union;
                pm = new ProgressMeter(progHandler, "Calculating Union", other.Features.Count);
                if(other.Features != null && other.Features.Count > 0)
                {
                    union = other.Features[0];
                    for(int i = 1; i < other.Features.Count; i++)
                    {
                        union = union.Union(other.Features[i]);
                        pm.CurrentValue = i;
                    }
                    pm.Reset();
                    pm = new ProgressMeter(progHandler, "Calculating Intersections", self.NumRows());
                    Extent otherEnvelope = new Extent(union.Envelope);
                    for (int shp = 0; shp < self.ShapeIndices.Count; shp++)
                    {
                        if (!self.ShapeIndices[shp].Extent.Intersects(otherEnvelope)) continue;
                        IFeature selfFeature = self.GetFeature(shp);
                        selfFeature.Intersection(union, result, joinType);
                        pm.CurrentValue = shp;
                    }
                    pm.Reset();
                }

                
            }
            if (joinType == FieldJoinType.ForeignOnly)
            {
                result = new FeatureSet();
                result.CopyTableSchema(other);
                IFeature union;
                if (self.Features != null && self.Features.Count > 0)
                {
                    pm = new ProgressMeter(progHandler, "Calculating Union", self.Features.Count);
                    union = self.Features[0];
                    for (int i = 1; i < self.Features.Count; i++)
                    {
                        union = union.Union(self.Features[i]);
                        pm.CurrentValue = i;
                    }
                    pm.Reset();
                    if (other.Features != null)
                    {
                        pm = new ProgressMeter(progHandler, "Calculating Intersection", other.Features.Count);
                        int j = 0;
                        foreach (IFeature otherFeature in other.Features)
                        {
                            IFeature test = otherFeature.Intersection(union, result, joinType);
                            if (test.BasicGeometry != null)
                            {
                                result.Features.Add(test);
                            }
                            pm.CurrentValue = j;
                            j++;
                        }
                    }
                    pm.Reset();
                }
                
                
            }
            return result;

        }

        

    }
}
