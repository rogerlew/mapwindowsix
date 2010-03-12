//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created $time$
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

using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Data
{


    /// <summary>
    /// FloatBuffer
    /// </summary>
    internal class FloatValueGrid : IValueGrid
    {
        #region Private Variables

        private FloatRaster _sourceRaster;
        private int _topRow;
        private bool _updated;
        private float[][] _rowBuffer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new, completely empty datablock. 
        /// </summary>
        public FloatValueGrid()
        {
            
        }

        #endregion

        #region Methods


        /// <summary>
        /// Constructing an ObjectGrid this way assumes the values are not in ram and will
        /// simply buffer 3 rows.
        /// </summary>
        /// <param name="sourceRaster"></param>
        public FloatValueGrid(FloatRaster sourceRaster)
        {
            _sourceRaster = sourceRaster;

            if (sourceRaster.IsInRam == true)
            {
                // we will just be forwarding requests to the data array
            }
            else
            {
                _rowBuffer = new float[3][];
                _topRow = 0;
                for (int row = 0; row < 3; row++)
                {
                    _rowBuffer[row] = _sourceRaster.ReadRow(row);   
                }
            }

        }

        #endregion

        #region Properties

        // float to double
        private double AsDouble(float val)
        {
            if (val == _sourceRaster.FloatNoDataValue) // to handle noise when converting from float to double
            {
                return _sourceRaster.NoDataValue;
            }
            return (double)val;
        }

        // double to float
        private float AsFloat(double val)
        {
            if (val == _sourceRaster.NoDataValue)
            {
                return _sourceRaster.FloatNoDataValue;
            }
            if (val > (double)float.MaxValue) return float.MaxValue;
            if (val < (double)float.MinValue) return float.MinValue;
            return Convert.ToSingle(val);
        }
        /// <summary>
        /// Gets or sets a value at the 0 row, 0 column index.
        /// </summary>
        /// <param name="row">The 0 based vertical row index from the top</param>
        /// <param name="column">The 0 based horizontal column index from the left</param>
        /// <returns>An object reference to the actual value in the data member.</returns>
        public double this[int row, int column]
        {
            get
            {
                if (_sourceRaster.IsInRam)
                {
                    return AsDouble(_sourceRaster.Data[row][column]);
                }
                else
                {
                    int shift = row - _topRow;
                    if (shift >= 0 && shift < 3)
                    {
                        return AsDouble(_rowBuffer[row - _topRow][column]);
                    }
                    // the value was not found in the buffer.  If the value is below the row buffer, load the new row as the bottom row.
                    if (shift == 3)
                    {
                        _rowBuffer[0] = _rowBuffer[1];
                        _rowBuffer[1] = _rowBuffer[2];
                        _rowBuffer[2] = _sourceRaster.ReadRow(row);
                        return AsDouble(_rowBuffer[2][column]);
                    }
                    if (shift == 4)
                    {
                        _rowBuffer[0] = _rowBuffer[2];
                        _rowBuffer[1] = _sourceRaster.ReadRow(row - 1);
                        _rowBuffer[2] = _sourceRaster.ReadRow(row);
                        return  AsDouble(_rowBuffer[2][column]);
                    }
                    if (shift > 4)
                    {
                        _rowBuffer[0] = _sourceRaster.ReadRow(row - 2);
                        _rowBuffer[1] = _sourceRaster.ReadRow(row - 1);
                        _rowBuffer[2] = _sourceRaster.ReadRow(row);
                        return AsDouble(_rowBuffer[2][column]);
                    }
                    if (shift == -1)
                    {
                        _rowBuffer[0] = _sourceRaster.ReadRow(row);
                        _rowBuffer[1] = _rowBuffer[0];
                        _rowBuffer[2] = _rowBuffer[1];
                        return AsDouble(_rowBuffer[0][column]);
                        
                    }
                    if (shift == -2)
                    {
                        _rowBuffer[0] = _sourceRaster.ReadRow(row);
                        _rowBuffer[1] = _sourceRaster.ReadRow(row + 1);
                        _rowBuffer[2] = _rowBuffer[0];
                        return AsDouble(_rowBuffer[0][column]);
                    }
                    if (shift < -2)
                    {
                        _rowBuffer[0] = _sourceRaster.ReadRow(row);
                        _rowBuffer[1] = _sourceRaster.ReadRow(row + 1);
                        _rowBuffer[2] = _sourceRaster.ReadRow(row + 2);
                        return AsDouble(_rowBuffer[0][column]);
                    }
                    // this should never happen
                    throw new ApplicationException(MessageStrings.IndexingErrorIn_S.Replace("%S", "Raster.Value[" + row + ", " + column + "]"));
                }
               
            }
            set
            {
                _updated = true;
                if (_sourceRaster.IsInRam)
                {
                    _sourceRaster.Data[row][column] = AsFloat(value);
                }
                else
                {
                    _sourceRaster.WriteValue(row, column, AsFloat(value));
                   
                }
            }
        }

        /// <summary>
        /// This is just a boolean flag that is set to true when the values 
        /// are updated.  It is the responsibility of the user to set this
        /// value to false again when the situation warents it.
        /// </summary>
        public bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }

        #endregion

      

    }
}
