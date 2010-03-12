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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2008 1:03:44 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Geometries
{


    /// <summary>
    /// IMatrix4
    /// </summary>
    public interface IMatrixD : IMatrix
    {
       
        #region Methods


        /// <summary>
        /// Multiplies every value in the specified n x m matrix by the specified double inScalar.
        /// </summary>
        /// <param name="inScalar">The double precision floating point to multiply all the members against</param>
        /// <returns>A new n x m matrix</returns>
        IMatrixD Multiply(double inScalar);

        /// <summary>
        /// This replaces the underlying general multiplication with a more specific type.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        IMatrixD Multiply(IMatrixD matrix);
        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the values for this matrix of double precision coordinates
        /// </summary>
        double[,] Values
        {
            get;
            set;
        }


        #endregion



    }
}
