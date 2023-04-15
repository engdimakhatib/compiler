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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Global.ReadKeyWord();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            //TSymbol temp = new TSymbol();
            //temp = Global.GSymbol;
            //while (temp != null)
            //{
            //    richTextBox1.Text += temp.name + "\n";
            //    richTextBox2.Text += temp.ul.ToString() + "\n";
            //    richTextBox2.Text +=Convert.ToInt32 (temp.ul).ToString() + "\n";
            //    temp = temp.next;
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //string text = "";
            //text = textBox1.Text.ToUpper();
            //TSymbol Retu = Global.FindSymbol(text);
            //if (Retu == null)
            //{ textBox1.Text += "\t" + "Not Found"; }
            //else
            //    textBox1.Text += "\t" + Retu.ul.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //richTextBox3.Clear();
            //string File_Name = "";
            //File_Name = textBox2.Text;
            //File_Name = File_Name.ToUpper();
            //Global.AddFile(File_Name);
            //TTFile temp;
            //temp = Global.GFile;
            //while (temp != null)
            //{
            //    richTextBox3.Text += temp.name + "\n";
            //    temp = temp.next;
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Global.GFile.next = null;//test
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //DialogResult result = saveFileDialog1.ShowDialog();

            //// saveFileDialog1.InitialDirectory="D:\Compiler_Compiler - modulation\Compiler_Compiler\bin\Debug\test.txt";
            //if (result == DialogResult.OK)
            //{
            //    richTextBox4.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            //}

            //Global.CF = new StreamReader("test.txt");
            //richTextBox4.Clear();
            //Lexical.Read_New_Line();
            //while (!Global.CF.EndOfStream)
            //{

            //    Boolean j = Lexical.Skip_Spaces_And_Comment();
            //    if (j)
            //    {
            //        richTextBox4.Text += Global.CL[Global.CI].ToString();
            //        Global.CI++;
            //    }
                   
            //}
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //richTextBox6.Clear();
            //richTextBox4.SaveFile("example1.txt", RichTextBoxStreamType.PlainText);
            //Global.CF = new StreamReader("example1.txt");
            //Lexical.Read_New_Line();
            //try
            //{
            //    while (!Global.CF.EndOfStream || Global.CI < Global.CL.Length)
            //    {
                  
            //        //while (Global.CI < Global.CL.Length)
            //       // {
            //            Global.TypeSymbol ul=Lexical.Lexical_Unit();
            //            richTextBox6.Text += ul.ToString() + "\t\t";
            //            richTextBox6.Text += Global.buffer_Temp + "\n";
            //            Global.buffer_Temp = "";
            //            if (ul == Global.TypeSymbol.u_EOF)
            //                break;
            //        //}

            //    }
            //}
            //catch
            //{
            //    MessageBox.Show(Global.Message_Wrong+Global.buffer_Temp);
            //}
            //finally//على كل الأحوال سيدخل
            //    {
            //        Global.CF.Close();
            //    }
        }

        private void button7_Click(object sender, EventArgs e)
        {
           // Error.Read_File_Error();
            // Error.Get_Error(2);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //string s = richTextBox6.Text.ToUpper();
            //richTextBox6.Text += Global.FindSy(s,ref Global.GSymbol).name.ToString()+"\n";
            //richTextBox6.Text += Global.GSymbol.name.ToString();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            //string s = richTextBox4.Text.ToUpper();
            //richTextBox4.Text += Global.FindS(s, Global.GSymbol).name.ToString() + "\n";
            //richTextBox4.Text += Global.GSymbol.name.ToString();


            
        }

        private void button8_Click(object sender, EventArgs e)
        {
           

           // TIdentif temp= Global.G_Var;
           // Global.Add_IDentif(richTextBox6.Text.ToUpper(),ref temp,Global.TypeSymbol.u_VAR);
           //Global.G_Var = (TPVar)temp;
           // richTextBox4.Text = Global.G_Var.name + "\t";
           // Global.G_Var =(TPVar) Global.G_Var.next;
           // while (Global.G_Var != null)
           // {
           //     richTextBox4.Text += Global.G_Var.name+"\t";
           //     Global.G_Var =(TPVar) Global.G_Var.next;
           // }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Global.G_Var = new TPVar();
            //Global.G_Var.name = "first_put_in_GVar";
            //Global.G_Var.next = null;
            //Global.G_Var.ul = Global.TypeSymbol.u_AND;
            //Global.G_Var.items.kind = Global.TypeSymbol.u_ASSIGN;
            //Global.G_Var.items.next = null;
            //Global.G_Var.items.Val_NB = 0;
            //Global.G_Var.items.Val_STR = "fir";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Global.G_Var_Define = null;
            //Global.G_Var_Proc=null;
            //Global.G_Var=null;
            //richTextBox6.Clear();
            //try
            //{
            //    richTextBox4.SaveFile("example1.txt", RichTextBoxStreamType.PlainText);
            //    Global.G_Cur_File.name = "example1.txt";
            //    Rules_Class.Compile_Current_File();
            //    TTFile file = Global.GFile;
            //    richTextBox6.Text += "Files_Include" + "\n";
            //    while (file != null)
            //    {
            //        richTextBox6.Text += file.name.ToString();
            //        file = file.next;
            //    }
            //    richTextBox6.Text += "\n";
            //    TDenfine tempd = Global.G_Var_Define;
            //    richTextBox6.Text += "Define"+"\n";
            //    while (tempd != null)
            //    {
            //        richTextBox6.Text += tempd.name.ToString();
            //        tempd = (TDenfine)tempd.next;
            //    }
            //    richTextBox6.Text += "\n";
            //   TProcedure tempp = Global.G_Var_Proc;
            //   richTextBox6.Text += "Procedure" + "\n";
            //    while (tempp != null)
            //    {
            //        richTextBox6.Text += tempp.name.ToString() + "\n";
            //        TPVar input = tempp.P_In;
            //        TPVar output = tempp.P_Out;
            //        TPVar local = tempp.L_Var;
            //        richTextBox6.Text += "Input_Procedure" + "\n";
            //        while (input != null)
            //        {
            //            richTextBox6.Text += "\t" + input.name;
            //            input =(TPVar) input.next;
            //        }
            //        richTextBox6.Text += "\n";
            //        richTextBox6.Text += "Output_Procedure" + "\n";
            //        while (output != null && output != tempp.P_In)
            //        {
            //            richTextBox6.Text += "\t" + output.name;
            //            output = (TPVar)output.next;
            //        }
            //        richTextBox6.Text += "\n";
            //        richTextBox6.Text += "Local_Var_Procedure" + "\n";
            //        while (local != null && local != tempp.P_Out&&local!=tempp.P_In)
            //        {
            //            richTextBox6.Text += "\t" + local.name;
            //            local = (TPVar)local.next;
            //        }
            //        tempp =(TProcedure) tempp.next;
            //    }
            //   TPVar tempv = Global.G_Var;
            //   richTextBox6.Text += "\n";
            //   richTextBox6.Text += "G_Var" + "\n";
            //   while ( tempv != null)
            //    {
            //        richTextBox6.Text += tempv.name.ToString();
            //        tempv =(TPVar) tempv.next;
            //    }

            //}
            //catch { MessageBox.Show(Global.Message_Wrong); }
            //finally { Global.CF.Close(); }

        }
       
        private void richTextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
                {
                    //richTextBox6.Clear();
                    //try
                    //{
                    //    richTextBox4.SaveFile("example.txt", RichTextBoxStreamType.PlainText);
                    //    Global.CF = new StreamReader("example.txt");
                    //    TEXP last = null;
                    //    TEXP head = new TEXP();
                    //    Global.UL = Lexical.Lexical_Unit();
                    //    head = Instruction_Class.Read_Expression(ref last);
                    //    while (head != null)
                    //    {
                    //        richTextBox6.Text += head.UL.ToString() + "\t" + head.Val_NB.ToString()+"\n";
                    //        head = head.next;
                    //    }
                    //}
                    //catch { MessageBox.Show(Global.Message_Wrong); }
                    //finally { Global.CF.Close(); }
                }

        private void button7_Click_2(object sender, EventArgs e)
        {
            //Global.G_Var_Define = null;
            //Global.G_Var_Proc = null;
            //Global.G_Var = null;
            //Global.GFile = null;
            //richTextBox6.Clear();
            //try
            //{
            //    Global.G_Cur_File.name = "example1.txt";
            //    Rules_Class.Compile_Main_Program(Path.GetFullPath("example1.txt"));
            //    TTFile file = Global.GFile;
            //    richTextBox6.Text += "Files_Include" + "\n";
            //    while (file != null)
            //    {
            //        richTextBox6.Text += file.name.ToString();
            //        file = file.next;
            //    }
            //}
            //catch { MessageBox.Show(Global.Message_Wrong); }
            //finally { Global.CF.Close(); }

        }

        private void cOMPILEToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Free_Class.Free_ALL();
            //    Global.ReadKeyWord();
            //    Error.Read_File_Error();
            //    Rules_Class.Compile_Main_Program(Path.GetFullPath(File_Name));
            //}
            //catch
            //{
            //    richTextBox2.Text = Global.Message_Wrong.ToString();
            //    //MessageBox.Show(Global.Message_Wrong);
            //}
            //finally
            //{
            //    Global.CF.Close();
            //}
        }

       
        private void oPENToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //    StreamReader Reader = null;
        //    openFileDialog1 = new OpenFileDialog();
        //    openFileDialog1.InitialDirectory = @"D:\NOW\Compiler_Compiler_final\Compiler_Compiler\bin\Debug";
        //    openFileDialog1.Filter = "Fourth Year (*.aub)|*.aub";
        //    openFileDialog1.RestoreDirectory = true;

        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        try
        //        {

        //            tabControl1.Visible = true;
        //            File_Name = openFileDialog1.SafeFileName;

        //            tabControl1.TabPages[0].Text = File_Name;
        //            Reader = new StreamReader(openFileDialog1.FileName);
        //            while (!(Reader.EndOfStream))
        //            {
        //                richTextBox1.Text += Reader.ReadLine() + "\n";
        //            }
                   
        //        }
        //        catch
        //        {
        //            MessageBox.Show(e.ToString());

        //        }
        //        finally
        //        {
        //           Reader.Close();
        //        }

        //    } 
        }

        private void sAVEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (File_Name != "")
            //{
            //    richTextBox1.SaveFile(File_Name, RichTextBoxStreamType.PlainText);
            //}
        }

        private void sAVEASToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rUNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Execution.Execute_List_Of_Instruction(Global.Main_INST);
                Free_Class.INTIAl_VARS();
            }
            catch (Exception)
            {
                MessageBox.Show(Global.Message_Wrong);
            }
            finally
            { }
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cLOSEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        //    {
        //        StreamReader Reader = null;
        //        openFileDialog1 = new OpenFileDialog();
        //        openFileDialog1.InitialDirectory = @"D:\NOW\Compiler_Compiler_final\Compiler_Compiler\bin\Debug";
        //        openFileDialog1.Filter = "Fourth Year (*.aub)|*.aub";
        //        openFileDialog1.RestoreDirectory = true;

        //        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //        {
        //            try
        //            {
        //                tabControl1.Visible = true;
        //                File_Name = openFileDialog1.SafeFileName;

        //                tabControl1.TabPages[0].Text = File_Name;
        //                Reader = new StreamReader(openFileDialog1.FileName);
        //                while (!(Reader.EndOfStream))
        //                {
        //                    richTextBox1.Text += Reader.ReadLine() + "\n";
        //                }
        //            }
        //            catch
        //            {
        //                richTextBox2.Text = Global.Message_Wrong.ToString();
        //                //MessageBox.Show(e.ToString());

        //            }
        //            finally
        //            {
        //                Reader.Close();
        //            }

        //        }
        //    }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {//
            //if (File_Name != "")
            //{
            //    richTextBox1.SaveFile(File_Name, RichTextBoxStreamType.PlainText);
            //}
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
