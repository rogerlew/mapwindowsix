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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/14/2009 8:50:58 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.Drawing;
using MapWindow.Main;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// VectorEditorSettings
    /// </summary>
    [Serializable]
    public class FeatureEditorSettings : EditorSettings
    {
        #region Private Variables

        private ClassificationTypes _classificationType;
        private string _fieldName;
        private bool _useGradient;
        private int _gradientAngle;
        private string _normField;
        private double _startSize;
        private double _endSize;
        private IFeatureSymbolizer _templateSymbolizer;
        private bool _useSizeRange;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of VectorEditorSettings
        /// </summary>
        public FeatureEditorSettings()
        {
            _useGradient = true;
            _gradientAngle = -45;
            _startSize = 5;
            _endSize = 20;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the unique values.
        /// </summary>
        [Serialize("ClassificationType")]
        public ClassificationTypes ClassificationType
        {
            get { return _classificationType; }
            set { _classificationType = value; }
        }

       
        /// <summary>
        /// Gets or sets the double size for the last item in the range
        /// </summary>
        [Serialize("EndSize")]
        public double EndSize
        {
            get { return _endSize; }
            set { _endSize = value; }
        }

      

        /// <summary>
        /// Gets or sets the field name that categories are based on
        /// </summary>
        [Serialize("FieldName")]
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        /// <summary>
        /// Gets or sets the normalization field
        /// </summary>
        [Serialize("NormField")]
        public string NormField
        {
            get { return _normField; }
            set { _normField = value; }
        }


        /// <summary>
        /// Gets or sets the gradient angle if use gradient is true
        /// and the shape is a polygon shape.
        /// </summary>
        [Serialize("GradientAngle")]
        public int GradientAngle
        {
            get { return _gradientAngle; }
            set { _gradientAngle = value; }
        }


        /// <summary>
        /// Gets or sets the double start size for point or line size ranges
        /// </summary>
        [Serialize("StartSize")]
        public double StartSize
        {
            get { return _startSize; }
            set { _startSize = value; }
        }

        /// <summary>
        /// Gets or sets the feature symbolizer that acts as a template for
        /// any characteristics not covered by the size and color ranges.
        /// </summary>
        [Serialize("TemplateSymbolizer")]
        public IFeatureSymbolizer TemplateSymbolizer
        {
            get { return _templateSymbolizer; }
            set { _templateSymbolizer = value; }
        }

      
        /// <summary>
        /// Gets or sets a boolean indicating whether or not to
        /// use a gradient when randomly calculating polygon
        /// forms.
        /// </summary>
        [Serialize("UseGradient")]
        public bool UseGradient
        {
            get { return _useGradient; }
            set { _useGradient = value; }
        }


        /// <summary>
        /// Gets or sets a boolean indicating whether the size range should be used instead of
        /// the size specified by the template.
        /// </summary>
        [Serialize("UseSizeRange")]
        public bool UseSizeRange
        {
            get { return _useSizeRange; }
            set { _useSizeRange = value; }
        }
       
        
        #endregion



    }
}
