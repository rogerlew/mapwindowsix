using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapWindow
{
    public partial class frmTest : Form
    {
        /// <summary>
        /// Test form
        /// </summary>
        public frmTest()
        {
            InitializeComponent();
            this.Shown += new EventHandler(frmTest_Shown);
        }

        void frmTest_Shown(object sender, EventArgs e)
        {
            
        }
    }
}
