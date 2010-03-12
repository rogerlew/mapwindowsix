//********************************************************************************************************
// Product Name: MapWindow.Interfaces Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;


namespace MapWindow.Geometries
{
    /// <summary>
    /// An object that knows how to build a particular implementation of
    /// <c>ICoordinateSequence</c> from an array of Coordinates.
    /// </summary>
    /// <seealso cref="ICoordinateSequence" />
    public interface ICoordinateSequenceFactory
    {
        /// <summary>
        /// Returns a CoordinateSequence based on the given array; whether or not the
        /// array is copied is implementation-dependent.
        /// </summary>
        /// <param name="coordinates">Coordinates array, which may not be null nor contain null elements</param>
        ICoordinateSequence Create(IEnumerable<Coordinate> coordinates);

        /// <summary>
        /// Constructs a new coordinate sequence using a single coordinate.
        /// </summary>
        /// <param name="coord">A single coordinate to be used in a coordinate sequence factory.</param>
        /// <returns>A valid ICoordinateSequence.</returns>
        ICoordinateSequence Create(Coordinate coord);

        /// <summary>
        /// Constructs either an array with 1 member or an empty list, depending on the implementation.
        /// </summary>
        /// <returns>A new seqeunce</returns>
        ICoordinateSequence Create();
        
    }
}
