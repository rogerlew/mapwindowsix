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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2008 5:19:51 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Geometries
{


    /// <summary>
    /// Operations on 3D vectors can be carried out using a 4D Matrix.  This interface
    /// provides access to methods that are specific to 3D vector opperations.
    /// </summary>
    public interface IMatrix4: IMatrixD
    {

        #region Methods


        /// <summary>
        /// Specifies amount to rotate 
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        IMatrix4 RotateZ(double degrees);
        
        /// <summary>
        /// Multiplies the current matrix by a rotation matrix corresponding
        /// to the specified angle to create rotation in the Z direction.
        /// </summary>
        /// <param name="degrees">The angle to rotate in degrees.</param>
        /// <returns></returns>
        IMatrix4 RotateX(double degrees);
      
        /// <summary>
        /// Rotates the current matrix around the Y axis by multiplying the
        /// current matrix by a rotation matrix. 
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        IMatrix4 RotateY(double degrees);


        /// <summary>
        /// Translates the matrix by the specified amount in each of the directions
        /// by multiplying by a translation matrix created from the specified values.
        /// </summary>
        /// <param name="x">The translation in the X coordinate</param>
        /// <param name="y">The translation in the Y coordinate</param>
        /// <param name="z">The translation in the Z coordinate</param>
        /// <returns></returns>
        IMatrix4 Translate(double x, double y, double z);

        #endregion

        #region Properties



        #endregion

        


    }
}
