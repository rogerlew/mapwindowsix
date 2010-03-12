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

namespace MapWindow.GeometriesGraph
{
    /// <summary>
    /// 
    /// </summary>
    public enum Positions
    {
        /// <summary>
        ///  An indicator that a Location is <c>on</c> a GraphComponent (0)
        /// </summary>
        On = 0,

        /// <summary>
        /// An indicator that a Location is to the <c>left</c> of a GraphComponent (1)
        /// </summary>
        Left = 1,

        /// <summary> 
        /// An indicator that a Location is to the <c>right</c> of a GraphComponent (2)
        /// </summary> 
        Right = 2,

        /// <summary> 
        /// An indicator that a Location is <c>is parallel to x-axis</c> of a GraphComponent (-1)
        /// /// </summary> 
        Parallel = -1,
    }

    /// <summary> 
    /// A Position indicates the position of a Location relative to a graph component
    /// (Node, Edge, or Area).
    /// </summary>
    public class Position 
    {
        /// <summary> 
        /// Returns Positions.Left if the position is Positions.Right, 
        /// Positions.Right if the position is Left, or the position
        /// otherwise.
        /// </summary>
        /// <param name="position"></param>
        public static Positions Opposite(Positions position)
        {
            if (position == Positions.Left)
                return Positions.Right;
            if (position == Positions.Right)
                return Positions.Left;
            return position;
        }
    }
}
