using System;
using System.Windows.Forms;
namespace MapWindow
{
    
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (frmMapWindow6 frm = new frmMapWindow6())
            {
                // Show our form and initialize our graphics engine
                frm.Show();
                Application.Run(frm);
            }
        }
    }
}