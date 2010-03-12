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

namespace MapWindow.Analysis.Topology.Utilities
{
    /// <summary>
    /// A utility for making programming assertions.
    /// </summary>
    public class Assert
    {
        /// <summary>
        /// Only static methods!
        /// </summary>
        private Assert() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assertion"></param>
        public static void IsTrue(bool assertion)
        {
            IsTrue(assertion, null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assertion"></param>
        /// <param name="message"></param>
        public static void IsTrue(bool assertion, string message)
        {
            if (!assertion)
            {
                if (message == null)               
                     throw new AssertionFailedException();                
                else throw new AssertionFailedException(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedValue"></param>
        /// <param name="actualValue"></param>
       
        public static void IsEquals(Object expectedValue, Object actualValue)
        {
            IsEquals(expectedValue, actualValue, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedValue"></param>
        /// <param name="actualValue"></param>
        /// <param name="message"></param>
        public static void IsEquals(Object expectedValue, Object actualValue, string message)
        {
            if (!actualValue.Equals(expectedValue))
                throw new AssertionFailedException("Expected " + expectedValue + " but encountered "
                            + actualValue + (message != null ? ": " + message : String.Empty));            
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ShouldNeverReachHere()
        {
            ShouldNeverReachHere(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void ShouldNeverReachHere(string message)
        {
            throw new AssertionFailedException("Should never reach here"
                + (message != null ? ": " + message : String.Empty));
        }
    }
}
