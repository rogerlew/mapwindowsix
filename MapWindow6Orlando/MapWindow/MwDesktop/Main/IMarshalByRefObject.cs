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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Permissions;
namespace MapWindow.Main
{
    /// <summary>
    /// The easiest way to implement these elements is to inherit System.MarshalByRefObject
    /// </summary>
    public interface IMarshalByRefObject: IObject
    {
        /// <summary>
        /// Creates an object that contains all the relevant information required to generate a proxy used to communicate with a remote object
        /// </summary>
        /// <param name="requestedType"><I>requestedType</I>: The System.Type of the object that the new System.Runtime.Remoting.ObjRef will reference.</param>
        /// <returns>Information required to generate a proxy</returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        System.Runtime.Remoting.ObjRef CreateObjRef(System.Type requestedType);

        /// <summary>
        /// Retrieves the current lifetime service object that controls the lifetime policy for this instance
        /// </summary>
        /// <returns>An object of type System.Runtime.Remoting.Lifetiem.ILease used to control the lifetime policy for this instance.</returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        object GetLifetimeService();

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>An object of type System.Runtime.Remoting.Lifetime.ILease used to control the lifetime policy for this instance.  This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseMannagerPollTime property.</returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        object InitializeLifetimeService();


    }
}
