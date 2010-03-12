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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2008 1:59:52 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

namespace MapWindow.Geometries
{


    /// <summary>
    /// Vector
    /// </summary>
    public interface Vector : ICoordinate
    {

        #region Methods


        /// <summary>
        /// Adds each of the elements of V to the elements of this vector
        /// </summary>
        /// <param name="v">Vector, the vector to add to this vector</param>
        /// <returns>A vector result from the addition</returns>
        Vector Add(Vector v);

        /// <summary>
        /// Returns the cross product of this vector with the specified vector V
        /// </summary>
        /// <param name="v">The vector to perform a cross product against</param>
        /// <returns>A vector result from the inner product</returns>
        Vector Cross(Vector v);

        /// <summary>
        /// Returns the dot product of this vector with V2
        /// </summary>
        /// <param name="v">The vector to perform an inner product against</param>
        /// <returns>A Double result from the inner product</returns>
        double Dot(Vector v);

        /// <summary>
        /// Returns the scalar product of this vector against a scalar
        /// </summary>
        /// <param name="scalar">Double, a value to multiply against all the members of this vector</param>
        /// <returns>A vector multiplied by the scalar</returns>
        Vector Multiply(double scalar);


        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        void Normalize();

        /// <summary>
        /// Subtracts each element of V from each element of this vector
        /// </summary>
        /// <param name="v">Vector, the vector to subtract from this vector</param>
        /// <returns>A vector result from the subtraction</returns>
        Vector Subtract(Vector v);

       
        #endregion

        #region Properties

        /// <summary>
        /// The Euclidean distance from the origin to the tip of the 3 dimensional vector
        /// Setting the magntiude won't change the direction.
        /// </summary>
        double Length
        {
            get;
        }

       
      
     

        #endregion

    }
}
