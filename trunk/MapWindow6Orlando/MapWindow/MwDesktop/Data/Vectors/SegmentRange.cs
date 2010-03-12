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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2010 2:12:57 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using MapWindow.Geometries;

namespace MapWindow.Data
{


    /// <summary>
    /// SegmentSet
    /// </summary>
    public class SegmentRange : IEnumerable<Segment>
    {

        /// <summary>
        /// Cycles through the points, creating segments.  If the feature type is a polygon, then this will
        /// loop around again at the end of the part to create a segment from the first and last vertex.
        /// </summary>
        public class SegmentRangeEnumerator : IEnumerator<Segment>
        {
            private readonly SegmentRange _range;
            private Segment _current;
            private int _index;
            private readonly int _start;
            private readonly int _numVertices;
            private readonly double[] _verts;

            /// <summary>
            /// Creates a new enumerator given the SegmentRange
            /// </summary>
            /// <param name="parent"></param>
            public SegmentRangeEnumerator(SegmentRange parent)
            {
                _range = parent;
                _start = _range.Part.StartIndex;
                _numVertices = _range.Part.NumVertices;
                _index = -1;
                _verts = parent.Part.Vertices;
                
            }

            public Segment  Current
            {
	            get { return _current; }
            }
            object System.Collections.IEnumerator.Current
            {
                get { return _current; }
            }
            public void  Dispose()
            {
     	        
            }
            public bool  MoveNext()
            {
                _index++;
                if(_index == 0)
                {
                    Vertex p1 = new Vertex(_verts[_start * 2],_verts[_start * 2 + 1]);
                    Vertex p2 = new Vertex(_verts[_start * 2 + 2], _verts[_start * 2 + 3]);
                    _current = new Segment(p1, p2);
                    return true;
                }
                if (_index == _numVertices-1)
                {
                    // We have reached the last vertex, but if it is a polygon we wrap this around
                    if(_range.FeatureType != FeatureTypes.Polygon) return false;
                    Vertex p1 = _current.P2;
                    Vertex p2 = new Vertex(_verts[_start * 2], _verts[_start * 2 + 1]);
                    _current = new Segment(p1, p2);
                    return true;
                }
                if(_index > 0 && _index < _numVertices-1)
                {
                    Vertex p1 = _current.P2;
                    Vertex p2 = new Vertex(_verts[2 * (_start + _index + 1)], _verts[2 * (_start + _index + 1) + 1]);
                    _current = new Segment(p1, p2);
                    return true;
                }
                return false;
            }

            public void  Reset()
            {
                _current = null;
                _index = - 1;
            }

    
        }


        #region Private Variables

        private readonly FeatureTypes _featureType;
        private readonly PartRange _part;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SegmentSet
        /// </summary>
        public SegmentRange(PartRange part, FeatureTypes featureType)
        {
            _featureType = featureType;
            _part = part;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the feature type 
        /// </summary>
        public FeatureTypes FeatureType
        {
            get { return _featureType;}
        }

        /// <summary>
        /// Gets the part
        /// </summary>
        public PartRange Part
        {
            get { return _part; }
        }

        #endregion


        public IEnumerator<Segment> GetEnumerator()
        {
            return new SegmentRangeEnumerator(this);
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
