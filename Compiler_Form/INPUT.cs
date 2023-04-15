using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compiler_Compiler
{
    public partial class INPUT : Form
    {
        public INPUT()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Global.IS_INPUT_INTEGER = true;
            }
            this.Close();
        }
    }
}
