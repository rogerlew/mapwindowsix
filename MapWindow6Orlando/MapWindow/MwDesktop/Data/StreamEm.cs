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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2010 11:54:49 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.IO;

namespace MapWindow.Data
{


    /// <summary>
    /// StreamEm
    /// </summary>
    public static class StreamEm
    {
       
        /// <summary>
        /// Writes the integer as big endian
        /// </summary>
        /// <param name="stream">The IO stream </param>
        /// <param name="value"></param>
        public static void WriteBe(this Stream stream, int value)
        {
            
            byte[] val = BitConverter.GetBytes(value);
            if(BitConverter.IsLittleEndian)Array.Reverse(val);
            stream.Write(val, 0, 4);
        }

        /// <summary>
        /// Writes the endian as little endian
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void WriteLe(this Stream stream, int value)
        {
            byte[] val = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(val);
            stream.Write(val, 0, 4);
        }

        /// <summary>
        /// Writes the specified double value to the stream as little endian
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void WriteLe(this Stream stream, double value)
        {
            byte[] val = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(val);
            stream.Write(val, 0, 8);
        }

        /// <summary>
        /// Checks that the endian order is ok for doubles and then writes
        /// the entire array to the stream.
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="values">The double values to write in little endian form</param>
        /// <param name="startIndex">The integer start index in the double array to begin writing</param>
        /// <param name="count">The integer count of doubles to write.</param>
        public static void WriteLe(this Stream stream, double[] values, int startIndex, int count)
        {
            byte[] bytes = new byte[count*8];
            Buffer.BlockCopy(values, startIndex*8, bytes, 0, bytes.Length);
            if(!BitConverter.IsLittleEndian)
            {
                // Reverse to little endian if this is a big endian machine
                byte[] temp = bytes;
                bytes = new byte[temp.Length];
                Array.Reverse(temp);
                for (int i = 0; i < count; i++)
                {
                    Array.Copy(temp, i*8, bytes, (count-i-1)*8, 8);
                }
            }
            stream.Write(bytes, 0, bytes.Length);            
        }


    }
}
