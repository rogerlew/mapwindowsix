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
namespace MapWindow.Analysis.Topology.Operation.Linemerge
{
    /// <summary>
    /// A sequence of <c>LineMergeDirectedEdge</c>s forming one of the lines that will
    /// be output by the line-merging process.
    /// </summary>
    public class EdgeString
    {
        private IGeometryFactory factory;
        private IList directedEdges = new ArrayList();
        private Coordinate[] coordinates = null;

        /// <summary>
        /// Constructs an EdgeString with the given factory used to convert this EdgeString
        /// to a LineString.
        /// </summary>
        /// <param name="factory"></param>
        public EdgeString(IGeometryFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Adds a directed edge which is known to form part of this line.
        /// </summary>
        /// <param name="directedEdge"></param>
        public virtual void Add(LineMergeDirectedEdge directedEdge)
        {
            directedEdges.Add(directedEdge);
        }

        /// <summary>
        /// 
        /// </summary>
        private Coordinate[] Coordinates
        {
            get
            {
                if (coordinates == null)
                {
                    int forwardDirectedEdges = 0;
                    int reverseDirectedEdges = 0;
                    CoordinateList coordinateList = new CoordinateList();
                    IEnumerator i = directedEdges.GetEnumerator();
                    while (i.MoveNext()) 
                    {
                        LineMergeDirectedEdge directedEdge = (LineMergeDirectedEdge)i.Current;
                        if (directedEdge.EdgeDirection)                        
                             forwardDirectedEdges++;                        
                        else reverseDirectedEdges++;                        
                        coordinateList.Add(((LineMergeEdge)directedEdge.Edge).Line.Coordinates, false, directedEdge.EdgeDirection);
                    }
                    coordinates = coordinateList.ToCoordinateArray();
                    if (reverseDirectedEdges > forwardDirectedEdges)
                        CoordinateArrays.Reverse(coordinates);                    
                }
                return coordinates;
            }
        }

        /// <summary>
        /// Converts this EdgeString into a LineString.
        /// </summary>
        public virtual ILineString ToLineString()
        {
            return factory.CreateLineString(Coordinates);
        }
    }
}
