//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://www.gnu.org/licenses/lgpl.html and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Text;

using MapWindow.Analysis.Topology.Utilities;

namespace MapWindow.Analysis.Topology.Precision
{
    /// <summary> 
    /// Determines the maximum number of common most-significant
    /// bits in the mantissa of one or numbers.
    /// Can be used to compute the double-precision number which
    /// is represented by the common bits.
    /// If there are no common bits, the number computed is 0.0.
    /// </summary>
    public class CommonBits
    {
        /// <summary>
        /// Computes the bit pattern for the sign and exponent of a
        /// double-precision number.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>The bit pattern for the sign and exponent.</returns>
        public static long SignExpBits(long num)
        {
            return num >> 52;
        }

        /// <summary>
        /// This computes the number of common most-significant bits in the mantissas
        /// of two double-precision numbers.
        /// It does not count the hidden bit, which is always 1.
        /// It does not determine whether the numbers have the same exponent - if they do
        /// not, the value computed by this function is meaningless.
        /// </summary>
        /// <param name="num1"></param>
        /// /// <param name="num2"></param>
        /// <returns>The number of common most-significant mantissa bits.</returns>
        public static int NumCommonMostSigMantissaBits(long num1, long num2)
        {
            int count = 0;
            for (int i = 52; i >= 0; i--)
            {
                if (GetBit(num1, i) != GetBit(num2, i))
                    return count;
                count++;
            }
            return 52;
        }

        /// <summary>
        /// Zeroes the lower n bits of a bitstring.
        /// </summary>
        /// <param name="bits">The bitstring to alter.</param>
        /// <param name="nBits">the number of bits to zero.</param>
        /// <returns>The zeroed bitstring.</returns>
        public static long ZeroLowerBits(long bits, int nBits)
        {
            long invMask = (1L << nBits) - 1L;
            long mask = ~invMask;
            long zeroed = bits & mask;
            return zeroed;
        }

        /// <summary>
        /// Extracts the i'th bit of a bitstring.
        /// </summary>
        /// <param name="bits">The bitstring to extract from.</param>
        /// <param name="i">The bit to extract.</param>
        /// <returns>The value of the extracted bit.</returns>
        public static int GetBit(long bits, int i)
        {
            long mask = (1L << i);
            return (bits & mask) != 0 ? 1 : 0;
        }

        private bool isFirst = true;
        private int commonMantissaBitsCount = 53;
        private long commonBits = 0;
        private long commonSignExp;

        /// <summary>
        /// 
        /// </summary>
        public CommonBits() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        public virtual void Add(double num)
        {
            long numBits = System.BitConverter.DoubleToInt64Bits(num);            
            if (isFirst)
            {
                commonBits = numBits;
                commonSignExp = SignExpBits(commonBits);
                isFirst = false;
                return;
            }

            long numSignExp = SignExpBits(numBits);
            if (numSignExp != commonSignExp)
            {
                commonBits = 0;
                return;
            }            
            commonMantissaBitsCount = NumCommonMostSigMantissaBits(commonBits, numBits);
            commonBits = ZeroLowerBits(commonBits, 64 - (12 + commonMantissaBitsCount));
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double Common
        {
            get
            {
                return System.BitConverter.Int64BitsToDouble(commonBits);
            }
        }

        /// <summary>
        /// A representation of the Double bits formatted for easy readability
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public virtual string ToString(long bits)
        {
            double x = System.BitConverter.Int64BitsToDouble(bits);
            string numStr = HexConverter.ConvertAny2Any(bits.ToString(), 10, 2);            
            string padStr = "0000000000000000000000000000000000000000000000000000000000000000" + numStr;
            string bitStr = padStr.Substring(padStr.Length - 64);
            string str = bitStr.Substring(0, 1) + "  " + bitStr.Substring(1, 12) + "(exp) "
                         + bitStr.Substring(12) + " [ " + x + " ]";
            return str;
        }
    }
}
