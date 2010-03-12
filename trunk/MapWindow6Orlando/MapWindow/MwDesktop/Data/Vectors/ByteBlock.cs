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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2009 10:21:38 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using MapWindow.Data.IO;


namespace MapWindow.Data
{


    /// <summary>
    /// ByteBlock
    /// </summary>
    public class ByteBlock
    {
        #region Private Variables

        /// <summary>
        /// All the blocks
        /// </summary>
        public List<Byte[]> Blocks;

        /// <summary>
        /// The block size of the arrays
        /// </summary>
        public readonly int BlockSize;

        /// <summary>
        /// The offset
        /// </summary>
        public int Offset;

        /// <summary>
        /// The current block index
        /// </summary>
        public int CurrentBlock;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ByteBlock
        /// </summary>
        public ByteBlock(int blockSize)
        {
            BlockSize = blockSize;
            Blocks = new List<byte[]>();
            Blocks.Add(new byte[BlockSize]);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the number of bytes using the specified reader.
        /// This handles copying across blocks if necessary.
        /// </summary>
        /// <param name="numBytes"></param>
        /// <param name="reader"></param>
        public void Read(int numBytes, BufferedBinaryReader reader)
        {
            if(Offset + numBytes < BlockSize)
            {
                if (numBytes > 0)
                {
                    reader.Read(Blocks[CurrentBlock], Offset, numBytes);
                    Offset += numBytes;
                }
                return;
            }
            int firstLen = BlockSize - Offset;
            int secondLen = numBytes - firstLen;
            reader.Read(Blocks[CurrentBlock], Offset, firstLen);
            Offset = 0;
            CurrentBlock += 1;
            if(Blocks.Count <= CurrentBlock)Blocks.Add(new byte[BlockSize]);
            reader.Read(Blocks[CurrentBlock], Offset, secondLen);
            Offset += secondLen;
        }

        /// <summary>
        /// If the bytes were converted to a single, contiguous integer array, this returns
        /// the current offset in that array.
        /// </summary>
        /// <returns></returns>
        public int IntOffset()
        {
            return ((Blocks.Count - 1)*BlockSize + Offset)/4;
        }

        /// <summary>
        /// If the bytes were converted to a single contiguous double array, this returns
        /// the offset in that array.
        /// </summary>
        /// <returns></returns>
        public int DoubleOffset()
        {
            return ((Blocks.Count - 1) * BlockSize + Offset) / 8;
        }

        /// <summary>
        /// Resets the indices
        /// </summary>
        public void Reset()
        {
            Offset = 0;
            CurrentBlock = 0;
        }

        /// <summary>
        /// Combines all the blocks into a single array of the specified datatype
        /// </summary>
        /// <returns></returns>
        public int[] ToIntArray()
        {
            int[] result = new int[IntOffset()];
            for(int iblock = 0; iblock < CurrentBlock; iblock++)
            {
                Buffer.BlockCopy(Blocks[iblock], 0, result, (BlockSize * iblock), BlockSize);
            }
            Buffer.BlockCopy(Blocks[CurrentBlock], 0, result, (BlockSize * CurrentBlock), Offset);
            return result;
        }

        /// <summary>
        /// Combines all the blocks into a single array of the specified datatype
        /// </summary>
        /// <returns></returns>
        public double[] ToDoubleArray()
        {
            double[] result = new double[DoubleOffset()];
            for (int iblock = 0; iblock < CurrentBlock; iblock++)
            {
                Buffer.BlockCopy(Blocks[iblock], 0, result, (BlockSize * iblock), BlockSize);
            }
            Buffer.BlockCopy(Blocks[CurrentBlock], 0, result, (BlockSize * CurrentBlock), Offset);
            return result;
        }

        #endregion

        #region Properties



        #endregion



    }
}
