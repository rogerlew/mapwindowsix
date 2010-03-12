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

namespace MapWindow.Analysis.Topology.Index.Chain
{
    /// <summary>
    /// The action for the internal iterator for performing
    /// envelope select queries on a MonotoneChain.
    /// </summary>
    public class MonotoneChainSelectAction
    {
        /// <summary>
        /// These envelopes are used during the MonotoneChain search process.
        /// </summary>
        public Envelope TempEnv1 = new Envelope();

        /// <summary>
        /// 
        /// </summary>
        public LineSegment SelectedSegment = new LineSegment();

        /// <summary> 
        /// This function can be overridden if the original chain is needed.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="start"></param>
        public virtual void Select(MonotoneChain mc, int start)
        {
            mc.GetLineSegment(start, ref SelectedSegment);
            Select(SelectedSegment);
        }

        /// <summary>
        /// This is a convenience function which can be overridden to obtain the actual
        /// line segment which is selected.
        /// </summary>
        /// <param name="seg"></param>
        public virtual void Select(LineSegment seg) { }
    }
}
