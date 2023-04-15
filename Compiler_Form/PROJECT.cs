using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Compiler_Compiler
{
    public partial class PROJECT : Form
    {
        public PROJECT()
        {
            InitializeComponent();
        }

        public static Boolean IsCompile = false;

        private void cOMPILEToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Free_Class.Free_ALL();
                Global.ReadKeyWord();
                Error.Read_File_Error();
                Rules_Class.Compile_Main_Program(Path.GetFullPath(File_Name));

                IsCompile = true;
                richTextBox2.Text = "\n"+"Build Successed";
            }
            catch
            {
                richTextBox2.Text = "\n" + Global.Message_Wrong.ToString();
            }
            finally
            {
                Global.CF.Close();
            }
        }

        private void rUNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsCompile)
                {
                    richTextBox2.Text = "";
                }
                TProcedure Pass = Global.G_Var_Proc;
                while (Pass != null)
                {
                    if (!Pass.Is_Define)
                    {
                        Global.Message_Wrong = Error.Get_Error(54) + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    Pass = (TProcedure)Pass.next;
                }

                Execution.Execute_List_Of_Instruction(Global.Main_INST);
                Free_Class.INTIAl_VARS();
            }
            catch (Exception)
            {
                richTextBox2.Text = "\n" + Global.Message_Wrong.ToString();
            }
            finally
            { }
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            {
                {
                    StreamReader Reader = null;
                    openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.InitialDirectory = @"D:\Compiler_Compiler_finalL\Compiler_Compiler\bin\Debug";
                   // openFileDialog1.Filter = "(*.aub)|*.aub";
                    openFileDialog1.RestoreDirectory = true;

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            tabControl1.Visible = true;
                            File_Name = openFileDialog1.SafeFileName;

                            tabControl1.TabPages[0].Text = File_Name;
                            Reader = new StreamReader(openFileDialog1.FileName);
                            while (!(Reader.EndOfStream))
                            {
                                richTextBox1.Text += Reader.ReadLine() + "\n";
                            }
                        }
                        catch
                        {
                            richTextBox2.Text ="\n"+ Global.Message_Wrong.ToString();
                        }
                        finally
                        {
                            Reader.Close();
                        }

                    }
                }
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (File_Name != "")
            {
                richTextBox1.SaveFile(File_Name, RichTextBoxStreamType.PlainText);
            }
        }
        public static string File_Name = "";

        private void PROJECT_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aBOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_US About = new About_US();
            About.ShowDialog();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void cOMPILEToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tabControl1.Visible = true;
            if (File_Name == "")
            {
                File_Name = "Temp.aub";
            }
            else
            {
                richTextBox1.SaveFile(File_Name, RichTextBoxStreamType.PlainText);
                richTextBox1.Clear();
                File_Name = "Temp.aub";
                richTextBox1.SaveFile(File_Name, RichTextBoxStreamType.PlainText);

            }
           
            tabControl1.TabPages[0].Text = File_Name;
        }
    }
}
