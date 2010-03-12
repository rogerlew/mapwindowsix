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
namespace MapWindow.Geometries
{
   
        

        /// <summary>
        /// The internal representation of a list of coordinates inside a Geometry.
        /// <para>
        /// There are some cases in which you might want Geometries to store their
        /// points using something other than the NTS Coordinate class. For example, you
        /// may want to experiment with another implementation, such as an array of x’s
        /// and an array of y’s. or you might want to use your own coordinate class, one
        /// that supports extra attributes like M-values.
        /// </para>
        /// <para>
        /// You can do this by implementing the ICoordinateSequence and
        /// ICoordinateSequenceFactory interfaces. You would then create a
        /// GeometryFactory parameterized by your ICoordinateSequenceFactory, and use
        /// this GeometryFactory to create new Geometries. All of these new Geometries
        /// will use your ICoordinateSequence implementation.
        /// A note on performance. If your ICoordinateSequence is not based on an array
        /// of Coordinates, it may incur a performance penalty when its ToArray() method
        /// is called because the array needs to be built from scratch. 
        /// </para>
        /// </summary>
        public interface ICoordinateSequence : ICollection<Coordinate>, ICloneable
        {
            /// <summary>
            /// Returns the dimension (number of ordinates in each coordinate) for this sequence.
            /// </summary>
            int Dimension { get; }
           
            /// <summary>
            /// Returns the ordinate of a coordinate in this sequence.
            /// Ordinate indices 0 and 1 are assumed to be X and Y.
            /// Ordinates indices greater than 1 have user-defined semantics
            /// (for instance, they may contain other dimensions or measure values).         
            /// </summary>
            /// <param name="index">The coordinate index in the sequence.</param>
            /// <param name="ordinate">The ordinate index in the coordinate (in range [0, dimension-1]).</param>
            /// <returns></returns>       
            double GetOrdinate(int index, Ordinates ordinate);

            /// <summary>
            /// Sets the value for a given ordinate of a coordinate in this sequence.       
            /// </summary>
            /// <param name="index">The coordinate index in the sequence.</param>
            /// <param name="ordinate">The ordinate index in the coordinate (in range [0, dimension-1]).</param>
            /// <param name="value">The new ordinate value.</param>       
            void SetOrdinate(int index, Ordinates ordinate, double value);

            /// <summary>
            /// Returns (possibly copies of) the Coordinates in this collection.
            /// Whether or not the Coordinates returned are the actual underlying
            /// Coordinates or merely copies depends on the implementation. Note that
            /// if this implementation does not store its data as an array of Coordinates,
            /// this method will incur a performance penalty because the array needs to
            /// be built from scratch.
            /// </summary>
            /// <returns></returns>
            Coordinate[] ToCoordinateArray();

            /// <summary>
            /// Expands the given Envelope to include the coordinates in the sequence.
            /// Does not modify the original envelope, but rather returns a copy of the
            /// original envelope after being modified.
            /// </summary>
            /// <param name="env">The envelope use as the starting point for expansion.  This envelope will not be modified.</param>
            /// <returns>The newly created envelope that is the expanded version of the original.</returns> 
            IEnvelope ExpandEnvelope(IEnvelope env);

            /// <summary>
            /// Direct accessor to an ICoordinate
            /// </summary>
            /// <param name="Index">An integer index in this array sequence</param>
            /// <returns>An ICoordinate for the specified index</returns>
            Coordinate this[int Index]
            {
                get;
                set;
            }

            /// <summary>
            /// Every time a member is added to, subtracted from, or edited in the sequence,
            /// this VersionID is incremented.  This doesn't uniquely separate this sequence
            /// from other sequences, but rather acts as a way to rapidly track if changes
            /// have occured against a cached version.
            /// </summary>
            int VersionID
            {
                get;
            }
        }
    
}
