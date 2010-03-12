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
// The Initial Developer of this Original Code is Darrel Brown. Created 9/10/2009
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

namespace MapWindow.Serialization
{
	/// <summary>
	/// Base class for formatters responsible for converting objects to and from string values.
	/// </summary>
	public abstract class SerializationFormatter
	{
		/// <summary>
		/// Converts an object to a string value.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The string representation of the given object.</returns>
		public abstract string ToString(object value);

		/// <summary>
		/// Converts a string representation of an object back into the original object form.
		/// </summary>
		/// <param name="value">The string representation of an object.</param>
		/// <returns>The object represented by the given string.</returns>
		public abstract object FromString(string value);
	}
}