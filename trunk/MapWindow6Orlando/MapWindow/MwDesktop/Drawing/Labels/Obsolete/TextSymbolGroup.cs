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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/18/2008 9:01:45 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// SymbolGroup
    /// </summary>
    public class TextSymbolGroup: LegendItem, ITextSymbolGroup
    {
    

        #region Private Variables

        private ILabelSymbolizer _selectionSymbolizer;
        private ILabelSymbolizer _symbolizer;
        private IList<int> _regularLabels;
        private IList<int> _selectedLabels;
        private int _count;




        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new empty instance of the symbol group, with the expectation of having members added to it.
        /// </summary>
        public TextSymbolGroup()
        {
            Configure();
        }

        /// <summary>
        /// Creates a TextSymbolGroup that will work with the specified count instead of working based
        /// on the list of actual labels.
        /// </summary>
        /// <param name="count">An integer representing how many label indices should be tracked.</param>
        public TextSymbolGroup(int count)
        {
            _count = count;
            Configure();
        }

        

        private void Configure()
        {
            base.IsDragable = false;
            _selectedLabels = new List<int>();
            _regularLabels = new List<int>();
            if (_count > 0)
            {
                for (int i = 0; i < _count; i++)
                {
                    _regularLabels.Add(i);

                }
            }
            _symbolizer = new LabelSymbolizer();
            _selectionSymbolizer = new LabelSymbolizer();
            _selectionSymbolizer.BackColor = Color.LightCyan;
            base.LegendSymbolMode = SymbolModes.Symbol;
        }



        #endregion

        #region Methods

        public override Size GetLegendSymbolSize()
        {
            return new Size(16, 16);
        }

        /// <summary>
        /// Add a new integer into this group.
        /// </summary>
        /// <param name="label">The integer index to be added to the group.</param>
        /// <returns>Boolean, false if the integer was already in this group.</returns>
        public bool Add(ILabel label)
        {
            if (RegularLabels.Contains(label.Index)) return false;
            if (SelectedLabels.Contains(label.Index)) return false;
            RegularLabels.Add(label.Index);
            label.Parent = this;
            return true;
        }

        /// <summary>
        /// Specifies tht 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool CanReceiveItem(ILegendItem item)
        {
            base.CanReceiveItem(item);
            return false;
        }
       
        /// <summary>
        /// Removes all the selected indices and returns them to the regular indices.
        /// This returns a list of the indices that were changed.
        /// </summary>
        /// <returns>Returns a list of label indices that were changed</returns>
        public IList<int> Clear()
        {
            List<int> changeList = new List<int>();
            for (int I = _selectedLabels.Count - 1; I >= 0; I--)
            {
                int lbl = _selectedLabels[I];
                _selectedLabels.RemoveAt(I);
                if (_regularLabels.Contains(lbl)) continue;
                _regularLabels.Add(lbl);
                changeList.Add(lbl);
            }
            return changeList;
           
        }

        
        /// <summary>
        /// Selects a single label 
        /// </summary>
        /// <param name="label">The integer label to select</param>
        public void Select(int label)
        {
            // Use public accessors in case they are overridden, but cache the reference to avoid repeatedly using the accessors
            IList<int> reg = RegularLabels;
            IList<int> sel = SelectedLabels;
            if (reg.Contains(label))
            {
                reg.Remove(label);
            }
            if (sel.Contains(label) == false)
            {
                sel.Add(label);
            }
        }

        /// <summary>
        /// Unselect a single label in this group.
        /// </summary>
        /// <param name="label">The integer label</param>
        public void UnSelect(int label)
        {
            IList<int> reg = RegularLabels;
            IList<int> sel = SelectedLabels;
            if (sel.Contains(label) == false)
            {
                sel.Add(label);
            }
            if (reg.Contains(label))
            {
                reg.Remove(label);
            }
        }


        #endregion

        #region Properties

    

        /// <summary>
        /// Gets or sets the list of regular labels based on their index in the actual list of labels
        /// </summary>
        public virtual IList<int> RegularLabels
        {
            get { return _regularLabels; }
            set { _regularLabels = value; }
        }

        /// <summary>
        /// Gets or sets the list of selected labels based on their index in the actual list of labels
        /// </summary>
        public virtual IList<int> SelectedLabels
        {
            get { return _selectedLabels; }
            set { _selectedLabels = value; }
        }

        /// <summary>
        /// Gets or sets the set of symbolic characteristics that should be used for drawing selected labels.
        /// </summary>
        public virtual ILabelSymbolizer SelectionSymbolizer
        {
            get { return _selectionSymbolizer; }
            set { _selectionSymbolizer = value; }
        }
        /// <summary>
        /// Gets or sets the text symbolizer for this group of labels
        /// </summary>
        public virtual ILabelSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            set { _symbolizer = value; }
        }

        #endregion

        /// <summary>
        ///  Temporarilly handle list duplication here.  This may be replaced by more general 
        ///  duplication extension methods for IList.
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Descriptor copy)
        {
            base.OnCopy(copy);

            TextSymbolGroup tsg = copy as TextSymbolGroup;
            List<int> selectedLabels = new List<int>();
            foreach (int value in _selectedLabels)
            {
                selectedLabels.Add(value);
            }
            tsg.SelectedLabels = selectedLabels;
            List<int> unselectedLabels = new List<int>();
            foreach (int value in _regularLabels)
            {
                unselectedLabels.Add(value);
            }
            tsg.RegularLabels = unselectedLabels;

            
        }

        #region ILegendItem Members

        /// <summary>
        /// Handles the drawing method for the legend symbol
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="box">The box to draw a symbol in</param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            Brush b = new SolidBrush(_symbolizer.FontColor);
            g.DrawString("Label", _symbolizer.GetFont(), b, new PointF((float)box.Left, (float)box.Top));
            b.Dispose();
        }

   
    
        #endregion


      

    }
}
