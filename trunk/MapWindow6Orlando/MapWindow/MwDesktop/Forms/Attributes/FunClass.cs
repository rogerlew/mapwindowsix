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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/11/09.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

namespace MapWindow.Forms
{
    class FunClass
    {
        private string _functon;
        private int _idx;
        private int _priorityLevel;
        private string _preArg;
        private string _nextArg;
        private double _value;
        private int _posInExpr;
        private int _funTok;
        private int _noOfArg;

        #region constructor
        /// <summary>
        /// Constuction
        /// </summary>
        public FunClass()
        {
            _idx = 0;
            _functon = null;
            _value = 0.0;
        }

        /// <summary>
        /// Constuction
        /// </summary>
        /// <param name="function">Function name</param>
        /// <param name="index">Index of the class</param>
        public FunClass(string function, int index)
        {
            _idx = index;
            _functon = function;
            _value = 0.0;
        }

        /// <summary>
        /// Constuction
        /// </summary>
        /// <param name="function">Function name</param>
        /// <param name="index">Index of the class</param>
        /// <param name="posInExp">Position in the expression</param>
        public FunClass(string function, int index, int posInExp)
        {
            _idx = index;
            _functon = function;
            _value = 0.0;
            _posInExpr = posInExp;
        }

        /// <summary>
        /// Constuction
        /// </summary>
        /// <param name="function">Function name</param>
        /// <param name="index">Index of the class</param>
        /// <param name="posInExp">Position in the expression</param>
        /// <param name="tokVal">Token value</param>
        public FunClass(string function, int index, int posInExp, int tokVal)
        {
            _idx = index;
            _functon = function;
            _value = 0.0;
            _posInExpr = posInExp;
            _funTok = tokVal;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or Set the Fuction string
        /// </summary>
        public string FunctionName
        {
            set { _functon = value; }
            get { return _functon; }
        }

        /// <summary>
        /// Get or Set the Index
        /// </summary>
        public int Index
        {
            set { _idx = value; }
            get { return _idx; }
        }

        /// <summary>
        /// get or set the PriorityLevel
        /// </summary>
        public int PriorityLevel
        {
            set { _priorityLevel = value; }
            get { return _priorityLevel; }
        }

        /// <summary>
        /// get or set Previous Argument
        /// </summary>
        public string PreviousArgument
        {
            set { _preArg = value; }
            get { return _preArg; }
        }

        /// <summary>
        /// get or det the Next argument
        /// </summary>
        public string NextArgument
        {
            set { _nextArg = value; }
            get { return _nextArg; }
        }

        /// <summary>
        /// get or set the value
        /// </summary>
        public double Value
        {
            set { _value = value; }
            get { return _value; }
        }

        /// <summary>
        /// get or set Position In Expression
        /// </summary>
        public int PositionInExpression
        {
            set { _posInExpr = value; }
            get { return _posInExpr; }
        }

        /// <summary>
        /// set or get token value
        /// </summary>
        public int TokenVal
        {
            set { _funTok = value; }
            get { return _funTok; }
        }

        /// <summary>
        /// set or get the no Of arg fuction type
        /// either one or two or more than two 
        /// </summary>
        public int NoOfArg
        {
            set { _noOfArg = value; }
            get { return _noOfArg; }
        }

        #endregion

        #region methode
        #endregion
    }
}
