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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2008 1:07:43 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Geometries
{


    /// <summary>
    /// IMatrix
    /// </summary>
    public interface IMatrix
    {
      

        #region Methods

        /// <summary>
        /// Performs the matrix multiplication against the specified matrix
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        IMatrix Multiply(IMatrix Matrix);

       


        #endregion

        #region Properties


        /// <summary>
        /// Gets the number of rows
        /// </summary>
        int NumRows
        {
            get;
        }

        /// <summary>
        /// Gets the number of columns
        /// </summary>
        int NumColumns
        {
            get;
        }


        #endregion



    }
}
