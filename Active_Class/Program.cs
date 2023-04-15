using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Compiler_Compiler
{
    static class Program
    {
        public static PROJECT First_Form;
        //public static Form1 First_Form;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // First_Form = new Form1();
            First_Form = new PROJECT ();
            Application.Run(First_Form);
        }
    }
}
