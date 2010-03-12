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


namespace MapWindow.Geometries
{
    

    /// <summary>
    /// Class containing static methods for conversions
    /// between Dimension values and characters.
    /// </summary>
    public class Dimension
    {
        private Dimension() { }

        /// <summary>
        /// Converts the dimension value to a dimension symbol,
        /// for example, <c>True => 'T'</c>
        /// </summary>
        /// <param name="dimensionValue">Number that can be stored in the <c>IntersectionMatrix</c>.
        /// Possible values are <c>True, False, Dontcare, 0, 1, 2</c>.</param>
        /// <returns>Character for use in the string representation of an <c>IntersectionMatrix</c>.
        /// Possible values are <c>T, F, * , 0, 1, 2</c>.</returns>
        public static char ToDimensionSymbol(Dimensions dimensionValue)
        {
            switch (dimensionValue)
            {
                case Dimensions.False:
                    return 'F';
                case Dimensions.True:
                    return 'T';
                case Dimensions.Dontcare:
                    return '*';
                case Dimensions.Point:
                    return '0';
                case Dimensions.Curve:
                    return '1';
                case Dimensions.Surface:
                    return '2';
                default:
                    throw new ArgumentOutOfRangeException
                        ("Unknown dimension value: " + dimensionValue);
            }
        }

        /// <summary>
        /// Converts the dimension symbol to a dimension value,
        /// for example, <c>'*' => Dontcare</c>
        /// </summary>
        /// <param name="dimensionSymbol">Character for use in the string representation of an <c>IntersectionMatrix</c>.
        /// Possible values are <c>T, F, * , 0, 1, 2</c>.</param>
        /// <returns>Number that can be stored in the <c>IntersectionMatrix</c>.
        /// Possible values are <c>True, False, Dontcare, 0, 1, 2</c>.</returns>
        public static Dimensions ToDimensionValue(char dimensionSymbol)
        {
            switch (Char.ToUpper(dimensionSymbol))
            {
                case 'F':
                    return Dimensions.False;
                case 'T':
                    return Dimensions.True;
                case '*':
                    return Dimensions.Dontcare;
                case '0':
                    return Dimensions.Point;
                case '1':
                    return Dimensions.Curve;
                case '2':
                    return Dimensions.Surface;
                default:
                    throw new ArgumentOutOfRangeException
                        ("Unknown dimension symbol: " + dimensionSymbol);
            }
        }
    }
}
