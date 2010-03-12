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
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/2/2009 9:26:05 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MapWindow.Projections
{


    /// <summary>
    /// Normally, cloning an object starts with MemberwiseClone, which 
    /// creates a shallow copy of the object.  For any members that
    /// derive from the Descriptor, however, any public properties
    /// or fields that implement ICloneable are copied (deep copy behavior).
    /// This is not always desirable, even if the member CAN be copied.
    /// This attribute causes the deep copy behavior to skip over
    /// properties marked with this attribute.
    /// </summary>
    public class ProjShallowCopy : Attribute
    {
    
       

    }
}
