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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/2/2009 9:46:34 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using MapWindow.Forms;
namespace MapWindow.Drawing
{


    /// <summary>
    /// ICharacterSymbol
    /// </summary>
    public interface ICharacterSymbol: ISymbol, IColorable
    {
       

        #region Methods

       

        /// <summary>
        /// Gets the string equivalent of the specified character code.
        /// </summary>
        /// <returns>A string version of the character</returns>
        string ToString();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unicode category for this character.
        /// </summary>
        [Description("Gets the unicode category for this character.")]
        System.Globalization.UnicodeCategory Category
        {
            get;
        }

        /// <summary>
        /// Gets or sets the character that this represents.
        /// </summary>
        [Description("Gets or sets the character for this symbol.")]
        char Character
        {
            get;
            set;
        }
        /// <summary>
        /// Unicode characters consist of 2 bytes.  This represents the first byte,
        /// which can be thought of as specifying a typeset.
        /// </summary>
        [Description("Gets or sets the upper unicode byte, or character set.")]
        byte CharacterSet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the byte code for the lower 256 values.  This represents the
        /// specific character in a given "typeset" range.
        /// </summary>
        [Description("Gets or sets the lower unicode byte or character ASCII code"),
        Editor(typeof(CharacterCodeEditor), typeof(UITypeEditor))]
        byte Code
        {
            get;
            set;
        }

       

        /// <summary>
        /// Gets or sets the string font family name to use for this character set.
        /// </summary>
        [Description("Gets or sets the font family name to use when building the font."),
        Editor(typeof(FontFamilyNameEditor), typeof(UITypeEditor))]
        string FontFamilyName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font style to use for this character layer.
        /// </summary>
        [Description("Gets or sets the font style to use for this character layer.")]
        FontStyle Style
        {
            get;
            set;
        }

     


        #endregion



    }
}
