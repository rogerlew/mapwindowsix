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
using System.Data;
using System.Diagnostics;
using System.Text;

namespace MapWindow.GoingOut
{
    /// <summary>
    /// Stores all attributes associated with a single <c>Geometry</c> feature.
    /// </summary>
    [Serializable]
    public class AttributesTable : IAttributesTable
    {        
        private const string IndexField = "_NTS_ID_";
        private const int IndexValue = 0;
        
        private Hashtable attributes = new Hashtable();

        /// <summary>
        /// Initialize a new attribute Table.
        /// </summary>
        public AttributesTable() 
        {                       
            // Add ID with fixed value of 0
            // AddAttribute(IndexField, typeof(Int32));
            // this[IndexField] = IndexValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Count
        {
            get
            {
                return attributes.Count;
            }
        }

        /// <summary>
        /// Returns a <c>string</c> array containing 
        /// all names of present attributes.
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetNames()
        {
            int index = 0;
            string[] names = new string[attributes.Count];
            foreach (string name in attributes.Keys)
                names[index++] = name;
            return names;
           
        }

        /// <summary>
        /// Returns a <c>object</c> array containing 
        /// all values of present attributes.
        /// </summary>
        /// <returns></returns>
        public virtual object[] GetValues()
        {
            int index = 0;
            object[] values = new object[attributes.Count];
            foreach (object val in attributes.Values)
                values[index++] = val;
            return values;
        }

        /// <summary>
        /// Verifies if attribute specified already exists.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public virtual bool Exists(string attributeName)
        {
            return attributes.ContainsKey(attributeName);
        }

        /// <summary>
        /// Build a field with the given value and add it to attributes Table.        
        /// </summary>
        /// <param name="attributeName">Name of the new attribute.</param>        
        /// <param name="attributeValue">Value for attribute (can be null).</param>
        /// <exception cref="ArgumentException">If attribute already exists.</exception>
        public virtual void AddAttribute(string attributeName, object attributeValue)
        {
            if(Exists(attributeName))
                throw new ArgumentException("Attribute " + attributeName + " already exists!");
            attributes.Add(attributeName, attributeValue);
        }        

        /// <summary>
        /// Delete the specified attribute from the Table.
        /// </summary>
        /// <param name="attributeName"></param>       
        public virtual void DeleteAttribute(string attributeName)
        {
            if (!Exists(attributeName))
                throw new ArgumentException("Attribute " + attributeName + " not exists!");
            attributes.Remove(attributeName);
        }

        /// <summary>
        /// Return the <c>System.Type</c> of the specified attribute, 
        /// useful for casting values retrieved with GetValue methods.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public virtual Type GetType(string attributeName)
        {
            if (!Exists(attributeName))
                throw new ArgumentException("Attribute " + attributeName + " not exists!");
            return attributes[attributeName].GetType();
        }

        /// <summary>
        /// Get the value of the specified attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        protected virtual object GetValue(string attributeName)
        {
            if (!Exists(attributeName))
                throw new ArgumentException("Attribute " + attributeName + " not exists!");
            return attributes[attributeName];
        }

        /// <summary>
        /// Set the value of the specified attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        protected virtual void SetValue(string attributeName, object attributeValue)
        {
            if (!Exists(attributeName))
                throw new ArgumentException("Attribute " + attributeName + " not exists!");
            attributes[attributeName] = attributeValue;
        }

        /// <summary>
        /// Get / Set the value of the specified attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public virtual object this[string attributeName]
        {
            get
            {
                return GetValue(attributeName);
            }
            set
            {
                SetValue(attributeName, value);
            }
        }
    }
}
