//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://www.gnu.org/licenses/lgpl.html and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Text;

using MapWindow.Geometries;

namespace MapWindow.Analysis.Topology.Noding
{

    /// <summary>
    /// Dissolves a noded collection of <see cref="SegmentString" />s to produce
    /// a set of merged linework with unique segments.
    /// A custom merging strategy can be applied when two identical (up to orientation)
    /// strings are dissolved together.
    /// The default merging strategy is simply to discard the merged string.
    ///<para>
    /// A common use for this class is to merge noded edges
    /// while preserving topological labelling.
    /// </para>
    /// </summary>
    public class SegmentStringDissolver
    {
        /// <summary>
        /// 
        /// </summary>
        public interface ISegmentStringMerger
        {
            /// <summary>
            /// Updates the context data of a <see cref="SegmentString" />
            /// when an identical (up to orientation) one is found during dissolving.
            /// </summary>
            /// <param name="mergeTarget">The segment string to update.</param>
            /// <param name="ssToMerge">The segment string being dissolved.</param>
            /// <param name="isSameOrientation">
            /// <c>true</c> if the strings are in the same direction,
            /// <c>false</c> if they are opposite.
            /// </param>
            void Merge(SegmentString mergeTarget, SegmentString ssToMerge, bool isSameOrientation);
        }

        private ISegmentStringMerger merger;
        private IDictionary ocaMap = new SortedList();
        
        /// <summary>
        /// Creates a dissolver with a user-defined merge strategy.
        /// </summary>
        /// <param name="merger"></param>
        public SegmentStringDissolver(ISegmentStringMerger merger)
        {
            this.merger = merger;
        }

        /// <summary>
        /// Creates a dissolver with the default merging strategy.
        /// </summary>
        public SegmentStringDissolver()
            : this(null) { }

        /// <summary>
        /// Dissolve all <see cref="SegmentString" />s in the input <see cref="ICollection"/>.
        /// </summary>
        /// <param name="segStrings"></param>
        public void Dissolve(ICollection segStrings)
        {
            foreach(object obj in segStrings)
                Dissolve((SegmentString)obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oca"></param>
        /// <param name="segString"></param>
        private void Add(OrientedCoordinateArray oca, SegmentString segString)
        {
            ocaMap.Add(oca, segString);
        }

        /// <summary>
        /// Dissolve the given <see cref="SegmentString" />.
        /// </summary>
        /// <param name="segString"></param>
        public void Dissolve(SegmentString segString)
        {
            OrientedCoordinateArray oca = new OrientedCoordinateArray(segString.Coordinates);
            SegmentString existing = FindMatching(oca, segString);
            if (existing == null)
                Add(oca, segString);            
            else
            {
                if (merger != null)
                {
                    bool isSameOrientation = CoordinateArrays.Equals(existing.Coordinates, segString.Coordinates);
                    merger.Merge(existing, segString, isSameOrientation);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oca"></param>
        /// <param name="segString"></param>
        /// <returns></returns>
        private SegmentString FindMatching(OrientedCoordinateArray oca, SegmentString segString)
        {
            return (SegmentString)ocaMap[oca];            
        }        

        /// <summary>
        /// Gets the collection of dissolved (i.e. unique) <see cref="SegmentString" />s
        /// </summary>
        public ICollection Dissolved
        {
            get
            {
                return ocaMap.Values;
            }
        }

    }
}
