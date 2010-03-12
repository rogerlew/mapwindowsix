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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/31/2008 10:11:25 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Analysis.Topology.KDTree
{


    /// <summary>
    /// NeighborsOutOfRangeException
    /// </summary>
    public class NeighborsOutOfRangeException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Creates a new NeighborsOutOfRangeException 
        /// </summary>
        public NeighborsOutOfRangeException()
            : base(MessageStrings.ArgumentOutOfRangeException_S.Replace("%S", "numNeighbors"))
        {
        }


    }
}
