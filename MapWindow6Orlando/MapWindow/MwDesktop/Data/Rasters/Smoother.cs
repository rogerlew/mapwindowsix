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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/14/2009 2:31:49 PM
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
using System.Drawing.Imaging;

namespace MapWindow.Data
{


    /// <summary>
    /// Smoother
    /// </summary>
    public class Smoother
    {

        private struct ARGB
        {
            public int A;
            public int R;
            public int G;
            public int B;
            public byte ToByte(int val)
            {
                if (val > 255) val = 255;
                if (val < 0) val = 0;
                return Convert.ToByte(val);
            }
        }

        

        #region Private Variables

        private BitmapData _bmpData;
        private byte[] _rgbData;
        private ProgressMeter pm;
        private byte[] _result;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Smoother
        /// </summary>
        public Smoother(BitmapData inBmpData, byte[] inRgbData, IProgressHandler progHandler)
        {
            _bmpData = inBmpData;
            _rgbData = inRgbData;
            _result = new byte[inRgbData.Length];
            pm = new ProgressMeter(progHandler, "Smoothing Image", inBmpData.Height);
        }

       

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the smoothing by cycling through the values 
        /// </summary>
        /// <returns></returns>
        public byte[] Smooth()
        {
            for (int row = 0; row < _bmpData.Height; row++)
            {
                for (int col = 0; col < _bmpData.Width; col++)
                {
                    DoSmooth(row, col);
                }
                pm.CurrentValue = row;
            }
            return _result;
        }


        #endregion

        #region Properties



        #endregion

        private void DoSmooth(int row, int col)
        {
            ARGB sum = new ARGB();
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    ARGB inValue = GetColor(row + y, col + x);
                    sum.A += inValue.A;
                    sum.B += inValue.B;
                    sum.G += inValue.G;
                    sum.R += inValue.R;
                }
            }
            sum.A = sum.A / 9;
            sum.B = sum.B / 9;
            sum.G = sum.G / 9;
            sum.R = sum.R / 9;
            PutColor(row, col, sum);
        }

        private ARGB GetColor(int row, int col)
        {
            ARGB result = new ARGB();
            if (row < 0) row = 0;
            if (col < 0) col = 0;
            if (row >= _bmpData.Height) row = _bmpData.Height-1;
            if (col >= _bmpData.Width) col = _bmpData.Width-1;
            int offset = row * _bmpData.Stride + col * 4;
            result.B = (int)_rgbData[offset];
            result.G = (int)_rgbData[offset + 1];
            result.R = (int)_rgbData[offset + 2];
            result.A = (int)_rgbData[offset + 3];
            return result;
        }

        private void PutColor(int row, int col, ARGB color)
        {
            int offset = row * _bmpData.Stride + col * 4;
            _result[offset] = color.ToByte(color.B);
            _result[offset + 1] = color.ToByte(color.G);
            _result[offset + 2] = color.ToByte(color.R);
            _result[offset + 3] = color.ToByte(color.A);
        }


    }
}
