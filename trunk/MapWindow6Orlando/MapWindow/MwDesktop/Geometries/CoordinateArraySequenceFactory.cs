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
using System.Collections.Generic;   
namespace MapWindow.Geometries
{
    /// <summary>
    /// Creates CoordinateSequences represented as an array of Coordinates.
    /// </summary>
    [Serializable]
    public sealed class CoordinateArraySequenceFactory : ICoordinateSequenceFactory
    {
        private static readonly CoordinateArraySequenceFactory _instance = new CoordinateArraySequenceFactory();

        /// <summary>
        /// 
        /// </summary>
        private CoordinateArraySequenceFactory() { }

      
        /// <summary>
        /// Returns the singleton _instance of CoordinateArraySequenceFactory.
        /// </summary>
        /// <returns></returns>
        public static CoordinateArraySequenceFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        ///  Returns a CoordinateArraySequence based on the given array (the array is not copied).
        /// </summary>
        /// <param name="coordinates">the coordinates, which may not be null nor contain null elements.</param>
        /// <returns></returns>
        public  ICoordinateSequence Create(IEnumerable<Coordinate> coordinates) 
        {
            return new CoordinateArraySequence(coordinates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordSeq"></param>
        /// <returns></returns>
        public  ICoordinateSequence Create(ICoordinateSequence coordSeq) 
        {
            return new CoordinateArraySequence(coordSeq);
        }

       

        /// <summary>
        /// Constructs a new coordinate sequence using a single coordinate.
        /// </summary>
        /// <param name="coord">A single coordinate to be used in a coordinate sequence factory.</param>
        /// <returns>A valid ICoordinateSequence.</returns>
        public  ICoordinateSequence Create(Coordinate coord)
        {
            return new CoordinateArraySequence(coord);
        }

        /// <summary>
        /// Constructs either an array with 1 member or an empty list, depending on the implementation.
        /// </summary>
        /// <returns>A new seqeunce</returns>
        public  ICoordinateSequence Create()
        {
            return new CoordinateArraySequence();
        }

       
    }
}
