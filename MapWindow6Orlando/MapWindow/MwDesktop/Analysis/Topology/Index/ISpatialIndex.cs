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

namespace MapWindow.Analysis.Topology.Index
{
    /// <summary> 
    /// The basic insertion and query operations supported by classes
    /// implementing spatial index algorithms.
    /// A spatial index typically provides a primary filter for range rectangle queries. A
    /// secondary filter is required to test for exact intersection. Of course, this
    /// secondary filter may consist of other tests besides intersection, such as
    /// testing other kinds of spatial relationships.
    /// </summary>
    public interface ISpatialIndex
    {
        /// <summary>
        /// Adds a spatial item with an extent specified by the given <c>Envelope</c> to the index.
        /// </summary>
        void Insert(IEnvelope itemEnv, object item);

        /// <summary> 
        /// Queries the index for all items whose extents intersect the given search <c>Envelope</c> 
        /// Note that some kinds of indexes may also return objects which do not in fact
        /// intersect the query envelope.
        /// </summary>
        /// <param name="searchEnv">The envelope to query for.</param>
        /// <returns>A list of the items found by the query.</returns>
        IList Query(IEnvelope searchEnv);

        /// <summary>
        /// Queries the index for all items whose extents intersect the given search <see cref="Envelope" />,
        /// and applies an <see cref="IItemVisitor" /> to them.
        /// Note that some kinds of indexes may also return objects which do not in fact
        /// intersect the query envelope.
        /// </summary>
        /// <param name="searchEnv">The envelope to query for.</param>
        /// <param name="visitor">A visitor object to apply to the items found.</param>
        void Query(IEnvelope searchEnv, IItemVisitor visitor);

        /// <summary> 
        /// Removes a single item from the tree.
        /// </summary>
        /// <param name="itemEnv">The Envelope of the item to remove.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns> <c>true</c> if the item was found.</returns>
        bool Remove(IEnvelope itemEnv, object item);
    }
}
