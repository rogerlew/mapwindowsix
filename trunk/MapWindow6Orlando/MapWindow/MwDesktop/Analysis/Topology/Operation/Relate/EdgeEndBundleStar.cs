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
using MapWindow.GeometriesGraph;

namespace MapWindow.Analysis.Topology.Operation.Relate
{
    /// <summary>
    /// An ordered list of <c>EdgeEndBundle</c>s around a <c>RelateNode</c>.
    /// They are maintained in CCW order (starting with the positive x-axis) around the node
    /// for efficient lookup and topology building.
    /// </summary>
    public class EdgeEndBundleStar : EdgeEndStar
    {
        /// <summary>
        /// 
        /// </summary>
        public EdgeEndBundleStar() { }

        /// <summary>
        /// Insert a EdgeEnd in order in the list.
        /// If there is an existing EdgeStubBundle which is parallel, the EdgeEnd is
        /// added to the bundle.  Otherwise, a new EdgeEndBundle is created
        /// to contain the EdgeEnd.
        /// </summary>
        /// <param name="e"></param>
        public override void Insert(EdgeEnd e)
        {
            EdgeEndBundle eb = (EdgeEndBundle) EdgeMap[e];
            if (eb == null) 
            {
                eb = new EdgeEndBundle(e);
                InsertEdgeEnd(e, eb);
            }
            else 
                eb.Insert(e);
            
        }

        /// <summary>
        /// Update the IM with the contribution for the EdgeStubs around the node.
        /// </summary>
        /// <param name="im"></param>
        public virtual void UpdateIM(IntersectionMatrix im)
        {
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); ) 
            {
                EdgeEndBundle esb = (EdgeEndBundle) it.Current;
                esb.UpdateIM(im);
            }
        }
    }
}
