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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/5/2009 9:37:56 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MapWindow.Data;
using MapWindow.Geometries;
using MapWindow.Tools;

namespace MapWindow.Drawing
{


    /// <summary>
    /// IndexSelection
    /// </summary>
    public class IndexSelection : Changeable, IIndexSelection
    {
      
        /// <summary>
        /// This class cycles through the members
        /// </summary>
        public class IndexSelectionEnumerator : IEnumerator<int>
        {
            private readonly FastDrawnState[] _states;
            private int _current;
            private readonly bool _selectionState;

            /// <summary>
            /// Creates a new instance of IndexSelectionEnumerator
            /// </summary>
            /// <param name="states"></param>
            /// <param name="selectionState"></param>
            public IndexSelectionEnumerator(FastDrawnState[] states, bool selectionState)
            {
                _states = states;
                _selectionState = selectionState;
                _current = -1;
            }

            #region IEnumerator<int> Members

            /// <summary>
            /// The current integer index
            /// </summary>
            public int Current
            {
                get { return _current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
               
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Attempts to move to the next member in the drawn states that shares the selection state.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                do
                {
                    _current++;
                } while (_current < _states.Length && _states[_current].Selected != _selectionState);
                return !(_current == _states.Length);
            }

            /// <summary>
            /// Resets the index to the starting point
            /// </summary>
            public void Reset()
            {
                _current = -1;
            }

            #endregion
        }

        #region Private Variables

        private readonly IFeatureLayer _layer;
        private readonly List<ShapeRange> _shapes;
        private SelectionModes _selectionMode;
        private bool _selectionState;
        private IFeatureCategory _regionCategory;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of IndexSelection
        /// </summary>
        public IndexSelection(IFeatureLayer layer)
        {
            _layer = layer;
            _shapes = layer.DataSet.ShapeIndices;
            SelectionMode = SelectionModes.IntersectsExtent;
            _selectionState = true; 
        }

        #endregion

        /// <summary>
        /// Adds all of the specified index values to the selection
        /// </summary>
        /// <param name="indices">The indices to add</param>
        public void AddRange(IEnumerable<int> indices)
        {
            foreach (int index in indices)
            {
                _layer.DrawnStates[index].Selected = _selectionState;
            }
            OnChanged();
        }

        /// <summary>
        /// Add REgion
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public bool AddRegion(IEnvelope region, out IEnvelope affectedArea)
        {
            bool added = false;
            SuspendChanges();
            Extent affected = new Extent();
            IPolygon reg = region.ToPolygon();
           
            Stopwatch sw =new Stopwatch();
            sw.Start();
            for (int shp = 0; shp < _layer.DrawnStates.Length; shp++)
            {
                if (_regionCategory != null && _layer.DrawnStates[shp].Category != _regionCategory) continue;
                bool doAdd = false;
                ShapeRange shape = _shapes[shp];
                if (_selectionMode == SelectionModes.Intersects)
                {
                    // Prevent geometry creation (which is slow) and use ShapeRange instead
                    ShapeRange env = new ShapeRange(region);
                    if(env.Intersects(shape))
                    {
                        _layer.DrawnStates[shp].Selected = _selectionState;
                        affected.ExpandToInclude(shape.Extent);
                        added = true;
                        OnChanged();
                    }
                }
                else if (_selectionMode == SelectionModes.IntersectsExtent)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        _layer.DrawnStates[shp].Selected = _selectionState;
                        affected.ExpandToInclude(shape.Extent);
                        added = true;
                        OnChanged();
                    }
                }
                else if (_selectionMode == SelectionModes.ContainsExtent)
                {
                    if (shape.Extent.Within(region))
                    {
                        _layer.DrawnStates[shp].Selected = _selectionState;
                        affected.ExpandToInclude(shape.Extent);
                        added = true;
                        OnChanged();
                    }
                }
                else if(_selectionMode == SelectionModes.Disjoint)
                {
                    if(shape.Extent.Intersects(region))
                    {
                        IBasicGeometry g = _layer.DataSet.Features[shp].BasicGeometry;
                        IGeometry geom = Geometry.FromBasicGeometry(g);
                        if (reg.Disjoint(geom)) doAdd = true;
                    }
                    else
                    {
                        doAdd = true;
                    }
                }
                else
                {
                    if (!shape.Extent.Intersects(region)) continue;
                    IBasicGeometry g = _layer.DataSet.GetFeature(shp).BasicGeometry;
                    IGeometry geom = Geometry.FromBasicGeometry(g);
                    switch (_selectionMode)
                    {
                        case SelectionModes.Contains:
                            if (shape.Extent.Within(region))
                            {
                                doAdd = true;
                            }
                            else if (shape.Extent.Intersects(region))
                            {
                                if (reg.Contains(geom)) doAdd = true;
                            }
                            break;
                        case SelectionModes.CoveredBy:
                            if (reg.CoveredBy(geom)) doAdd = true;
                            break;
                        case SelectionModes.Covers:
                            if (reg.Covers(geom)) doAdd = true;
                            break;
                        case SelectionModes.Intersects:
                            if (shape.Extent.Within(region))
                            {
                                doAdd = true;
                            }
                            else if (shape.Extent.Intersects(region))
                            {
                                if (reg.Intersects(geom)) doAdd = true;
                            }
                            break;
                        case SelectionModes.Overlaps:
                            if (reg.Overlaps(geom)) doAdd = true;
                            break;
                        case SelectionModes.Touches:
                            if (reg.Touches(geom)) doAdd = true;
                            break;
                        case SelectionModes.Within:
                            if (reg.Within(geom)) doAdd = true;
                            break;
                    }

                }
                if (!doAdd) continue;
                OnChanged();
                _layer.DrawnStates[shp].Selected = _selectionState;
                affected.ExpandToInclude(shape.Extent);
                added = true;
            }
            sw.Stop();
            Debug.WriteLine("Selection: " + sw.ElapsedMilliseconds);
            ResumeChanges();
            affectedArea = affected.ToEnvelope();
            return added;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public bool InvertSelection(IEnvelope region, out IEnvelope affectedArea)
        {
            SuspendChanges();
            bool flipped = false;
            Extent affected = new Extent();
            IPolygon reg = region.ToPolygon();
            for (int shp = 0; shp < _layer.DrawnStates.Length; shp++)
            {
                if (_regionCategory != null && _layer.DrawnStates[shp].Category != _regionCategory) continue;
                bool doFlip = false;
                ShapeRange shape = _shapes[shp];

                if (_selectionMode == SelectionModes.IntersectsExtent)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        _layer.DrawnStates[shp].Selected = !_layer.DrawnStates[shp].Selected;
                        affected.ExpandToInclude(shape.Extent);
                        flipped = true;
                    }
                }
                if (_selectionMode == SelectionModes.ContainsExtent)
                {
                    if (shape.Extent.Within(region))
                    {
                        _layer.DrawnStates[shp].Selected = !_layer.DrawnStates[shp].Selected;
                        affected.ExpandToInclude(shape.Extent);
                        flipped = true;
                    }
                }
                if (_selectionMode == SelectionModes.Disjoint)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        IBasicGeometry g = _layer.DataSet.Features[shp].BasicGeometry;
                        IGeometry geom = Geometry.FromBasicGeometry(g);
                        if (reg.Disjoint(geom)) doFlip = true;
                    }
                    else
                    {
                        doFlip = true;
                    }
                }
                else
                {
                    if (!shape.Extent.Intersects(region)) continue;
                    IFeature f = _layer.DataSet.Features[shp]; // only get this if envelopes intersect
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
                }
                if (!doFlip) continue;
                flipped = true;
                _layer.DrawnStates[shp].Selected = !_layer.DrawnStates[shp].Selected;
                affected.ExpandToInclude(shape.Extent);
            }
            affectedArea = affected.ToEnvelope();
            ResumeChanges();
            return flipped;
        }

        /// <summary>
        /// Attempts to remove all the members from the collection.  If
        /// one of the specified indices is outside the range of possible
        /// values, this returns false, even if others were successfully removed.
        /// This will also return false if none of the states were changed.
        /// </summary>
        /// <param name="indices">The indices to remove</param>
        /// <returns></returns>
        public bool RemoveRange(IEnumerable<int> indices)
        {
            bool problem = false;
            bool changed = false;
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            foreach (int index in indices)
            {
                if (index < 0 || index > drawnStates.Length)
                {
                    problem = true;
                }
                else
                {
                    if (drawnStates[index].Selected != !_selectionState) changed = true;
                    drawnStates[index].Selected = !_selectionState;
                }
                
            }
            return !problem && changed;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public bool RemoveRegion(IEnvelope region, out IEnvelope affectedArea)
        {
            bool removed = false;
            SuspendChanges();

            Extent affected = new Extent();
            IPolygon reg = region.ToPolygon();
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            for (int shp = 0; shp < drawnStates.Length; shp++)
            {
                if (_regionCategory != null && drawnStates[shp].Category != _regionCategory) continue;
                bool doRemove = false;
                ShapeRange shape = _shapes[shp];
                if (_selectionMode == SelectionModes.IntersectsExtent)
                {
                    if (shape.Extent.Intersects(region))
                    {
                        drawnStates[shp].Selected = !_selectionState;
                        affected.ExpandToInclude(shape.Extent);
                        removed = true;
                    }
                }
                else if (_selectionMode == SelectionModes.ContainsExtent)
                {
                    if (shape.Extent.Within(region))
                    {
                        drawnStates[shp].Selected = !_selectionState;
                        affected.ExpandToInclude(shape.Extent);
                        removed = true;
                    }
                }
                if(_selectionMode == SelectionModes.Disjoint)
                {
                    if(shape.Extent.Intersects(region))
                    {
                        IBasicGeometry g = _layer.DataSet.Features[shp].BasicGeometry;
                        IGeometry geom = Geometry.FromBasicGeometry(g);
                        if (reg.Disjoint(geom)) doRemove = true;
                    }
                    else
                    {
                        doRemove = true;
                    }
                }
                else
                {
                    if (!shape.Extent.Intersects(region)) continue;
                    IBasicGeometry g = _layer.DataSet.Features[shp].BasicGeometry;
                    IGeometry geom = Geometry.FromBasicGeometry(g);
                    switch (_selectionMode)
                    {
                        case SelectionModes.Contains:
                            if (shape.Extent.Within(region))
                            {
                                doRemove = true;
                            }
                            else if (shape.Extent.Intersects(region))
                            {
                                if (reg.Contains(geom)) doRemove = true;
                            }
                            break;
                        case SelectionModes.CoveredBy:
                            if (reg.CoveredBy(geom)) doRemove = true;
                            break;
                        case SelectionModes.Covers:
                            if (reg.Covers(geom)) doRemove = true;
                            break;
                        case SelectionModes.Intersects:
                            if (shape.Extent.Within(region))
                            {
                                doRemove = true;
                            }
                            else if (shape.Extent.Intersects(region))
                            {
                                if (reg.Intersects(geom)) doRemove = true;
                            }
                            break;
                        case SelectionModes.Overlaps:
                            if (reg.Overlaps(geom)) doRemove = true;
                            break;
                        case SelectionModes.Touches:
                            if (reg.Touches(geom)) doRemove = true;
                            break;
                        case SelectionModes.Within:
                            if (reg.Within(geom)) doRemove = true;
                            break;
                    }

                }
                if (!doRemove) continue;
                drawnStates[shp].Selected = !_selectionState;
                affected.ExpandToInclude(shape.Extent);
                removed = true;
            }
            affectedArea = affected.ToEnvelope();
            ResumeChanges();
            return removed;
        }

        /// <summary>
        /// Returns a new featureset based on the features in this collection
        /// </summary>
        /// <returns></returns>
        public FeatureSet ToFeatureSet()
        {
            FeatureSet fs = new FeatureSet(ToFeatureList());
            fs.Projection = _layer.DataSet.Projection;
            return fs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IFeature> ToFeatureList()
        {
            List<IFeature> result = new List<IFeature>();
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            for(int shp = 0; shp < drawnStates.Length; shp++)
            {
                if(drawnStates[shp].Selected == _selectionState)
                {
                    result.Add(_layer.DataSet.Features[shp]);
                }
            }
            return result;
        }

        /// <summary>
        /// Calculates the envelope of this collection
        /// </summary>
        public IEnvelope Envelope
        {
            get
            {
                if (_layer.DrawnStatesNeeded == false) return new Envelope();
                Extent ext = new Extent();
                FastDrawnState[] drawnStates = _layer.DrawnStates;
                for (int shp = 0; shp < drawnStates.Length; shp++)
                {
                    if (drawnStates[shp].Selected == _selectionState)
                    {
                        ext.ExpandToInclude(_shapes[shp].Extent);
                    }
                }
                return ext.ToEnvelope();
            }
        }

        /// <summary>
        /// Selection Mode controls how envelopes are treated when working with geometries.
        /// </summary>
        public SelectionModes SelectionMode
        {
            get
            {
                return _selectionMode;
            }
            set
            {
                _selectionMode = value;
            }
        }

        /// <summary>
        /// Gets or sets whether this should work as "Selected" indices (true) or 
        /// "UnSelected" indices (false).
        /// </summary>
        public bool SelectionState
        {
            get { return _selectionState; }
            set { _selectionState = value;}
        }

        #region IDataPageRetriever Members


        public DataTable GetAttributes(int startIndex, int numRows)
        {
            AttributeCache c = new AttributeCache(_layer.DataSet, numRows);
            DataTable result = new DataTable();
            result.Columns.AddRange(_layer.DataSet.GetColumns());
            int i = 0;
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            for (int fid = 0; fid < drawnStates.Length; fid++ )
            {
                if (drawnStates[fid].Selected)
                {
                    i++;
                    if (i < startIndex) continue;
                    DataRow dr = result.NewRow();
                    Dictionary<string, object> vals = c.RetrieveElement(fid);
                    foreach (KeyValuePair<string, object> pair in vals)
                    {
                        dr[pair.Key] = pair.Value;
                    }
                    result.Rows.Add(dr);
                    if (i > startIndex + numRows) break;
                }
            }
            return result;
        }

        public void SetAttributes(int startIndex, DataTable values)
        {
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            int sind = -1;
            for (int fid = 0; fid < drawnStates.Length; fid++)
            {
                if(drawnStates[fid].Selected)
                {
                    sind++;
                    if(sind > startIndex + values.Rows.Count)
                    {
                        break;
                    }
                    if(sind >= startIndex)
                    {
                        _layer.DataSet.Edit(fid, values.Rows[sind]);
                    }
                }
            }
        }

        public int NumRows()
        {
            return Count;
        }

        public DataColumn[] GetColumns()
        {
            { return _layer.DataSet.GetColumns(); }
        }

        public DataColumn GetColumn(string name)
        {
            return _layer.DataSet.GetColumn(name);
        }

        #endregion

        #region ICollection<int> Members

        public void Add(int index)
        {

            if (index < 0 || index >= _layer.DrawnStates.Length) return;
            if (_layer.DrawnStates[index].Selected == _selectionState) return;
            _layer.DrawnStates[index].Selected = _selectionState;
            OnChanged();
        }

        public void Clear()
        {
            for (int shp = 0; shp < _layer.DrawnStates.Length; shp++)
            {
                _layer.DrawnStates[shp].Selected = !_selectionState;
            }
            OnChanged();
        }

        public bool Contains(int index)
        {
            return (_layer.DrawnStates[index].Selected == _selectionState);
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            int index = arrayIndex;
            foreach (int i in this)
            {
                array[index] = i;
                index++;
            }
        }
      
        public int Count
        {
            get
            {
                int count = 0;
                if (_layer.DrawnStates == null) return 0;
                for (int i = 0; i < _layer.DrawnStates.Length; i++)
                {
                    if (_layer.DrawnStates[i].Selected == _selectionState) count++;
                }
                return count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(int index)
        {
            if (index < 0 || index >= _layer.DrawnStates.Length) return false;
            if (_layer.DrawnStates[index].Selected != _selectionState) return false;
            _layer.DrawnStates[index].Selected = !_selectionState;
            OnChanged();
            return true;
        }

        /// <summary>
        /// Setting this to a specific category will only allow selection by 
        /// region to affect the features that are within the specified category.
        /// </summary>
        public IFeatureCategory RegionCategory
        {
            get { return _regionCategory; }
            set { _regionCategory = value; }
        }

        #endregion

        #region IEnumerable<IFeature> Members

        public IEnumerator<int> GetEnumerator()
        {
            return new IndexSelectionEnumerator(_layer.DrawnStates, _selectionState);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion



        #region IDataPageRetriever Members

        /// <summary>
        /// Adding a row via the selection just adds a new data row in the source file.  This will not in any
        /// way affect the selection itself or even the in-ram representation of features.
        /// </summary>
        /// <param name="values"></param>
        public void AddRow(Dictionary<string, object> values)
        {
            // Don't worry about the index in this case.
            _layer.DataSet.AddRow(values);
        }

        // <summary>
        /// Adding a row via the selection just adds a new data row in the source file.  This will not in any
        /// way affect the selection itself or even the in-ram representation of features.
        /// </summary>
        /// <param name="values"></param>
        public void AddRow(DataRow values)
        {
            // Don't worry about the index in this case.
            _layer.DataSet.AddRow(values);
        }


        public void Edit(int index, Dictionary<string, object> values)
        {
            int sourceIndex = GetSourceIndex(index);
            _layer.DataSet.Edit(sourceIndex, values);
        }

        public void Edit(int index, DataRow values)
        {
            int sourceIndex = GetSourceIndex(index);
            _layer.DataSet.Edit(sourceIndex, values);
        }

        private int GetSourceIndex(int selectedIndex)
        {
            // For instance, the 0 index member of the selection might in fact
            // be the 10th member of the featureset.  But we want to edit the 10th member
            // and not the 0 member.
            int count = 0;
            foreach (int i in this)
            {
                if (count == selectedIndex) return i;
            }
            throw new IndexOutOfRangeException("Index requested was: " + selectedIndex + " but the selection only has " + count + " members");
        }

        #endregion

        #region IAttributeSource Members


        public int[] GetCounts(string[] expressions, ICancelProgressHandler progressHandler, int maxSampleSize)
        {
            int numSelected = Count;
            int[] counts = new int[expressions.Length];
            bool requiresRun = false;
            for (int ie = 0; ie < expressions.Length; ie++)
            {
                if (string.IsNullOrEmpty(expressions[ie]))
                {
                    counts[ie] = numSelected;
                }
                else
                {
                    requiresRun = true;
                }
            }
            if (!requiresRun) return counts;
            AttributePager ap = new AttributePager(_layer.DataSet, 100000);
            int numinTable = 0;
            DataTable result = new DataTable();
            result.Columns.AddRange(_layer.DataSet.GetColumns());
            FastDrawnState[] drawnStates = _layer.DrawnStates;
            for (int shp = 0; shp < drawnStates.Length; shp++)
            {
                if(drawnStates[shp].Selected)
                {
                    result.Rows.Add(ap.Row(shp).ItemArray);
                    numinTable++;
                    if(numinTable > 100000)
                    {
                        for (int ie = 0; ie < expressions.Length; ie++)
                        {
                            if (string.IsNullOrEmpty(expressions[ie])) continue;
                            counts[ie] += result.Select(expressions[ie]).Length;
                        }
                        result.Clear();
                    }
                }
            }
            for (int ie = 0; ie < expressions.Length; ie++)
            {
                if (string.IsNullOrEmpty(expressions[ie])) continue;
                counts[ie] += result.Select(expressions[ie]).Length;
            }
            result.Clear();
            return counts; 
        }

        #endregion
    }
}
