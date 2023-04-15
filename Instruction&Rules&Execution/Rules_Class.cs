using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Compiler_Compiler
{
    public class Rules_Class
    {
        public static void Compile_Main_Program(String Name)
        {

            Global.GFile = new TTFile();
            Global.GFile.name = Name;
            Global.GFile.next = null;
            Global.G_Cur_File = Global.GFile;
            while (Global.G_Cur_File!=null)
            {
                Rules_Class.Compile_Current_File();
                Global.G_Cur_File.Local_Gdefine = Global.G_Var_Define;
                Global.G_Var_Define = null;
                Global.CF.Close();
                Global.G_Cur_File = Global.G_Cur_File.next;
            }

        }

        public static void Compile_Current_File()
        {

            Global.CF = new StreamReader(Global.G_Cur_File.name);
            if (!Lexical.Read_New_Line())
                return;
            if (!Lexical.Skip_Spaces_And_Comment())
                return;

            #region Include & Define 
            while (Global.CL[Global.CI] == '#')
            {
                #region read include or define

                if (Global.CI + 1 < Global.CL.Length &&
                    (Global.CL[Global.CI + 1] == 'i' || Global.CL[Global.CI + 1] == 'I' ||
                    Global.CL[Global.CI + 1] == 'd' || Global.CL[Global.CI + 1] == 'D'))
                {
                    Global.CI++;  //#
                    Global.UL = Lexical.Lexical_Unit();
                    if (Global.UL == Global.TypeSymbol.u_INCLUDE)
                    {
                        #region include

                        Lexical.Skip_Spaces();//لا يسمح بوجود تعليقات بينها و بين اسم الملف
                        if (!(Global.CI < Global.CL.Length && Global.CL[Global.CI] == '\''))
                        {
                            //#include 'file';
                            Global.Message_Wrong = Error.Get_Error(11) + "\t" + Error.Get_Type_Error(0);
                            throw new Exception();
                        }
                        Global.UL = Lexical.Lexical_Unit();
                        if (Global.UL != Global.TypeSymbol.u_CST_STR)
                        {
                            Global.Message_Wrong = Error.Get_Error(11) + "\t" + Error.Get_Type_Error(1);
                            throw new Exception();
                        }
                        
                        Boolean Exist = File.Exists(Global.G_Cur_STR);
                        if (!Exist)
                        {
                            Global.Message_Wrong = Global.G_Cur_STR+"\t"+Error.Get_Error(1) + "\t" + Error.Get_Type_Error(0);
                            throw new Exception();
                        }
                        else
                        Global.AddFile(Global.G_Cur_STR);
                       
                    }//end if include
                        #endregion
                    else if (Global.UL == Global.TypeSymbol.u_DEFINE)
                    {
                        #region define
                        Lexical.Skip_Spaces();
                        if (!(Global.CI < Global.CL.Length && (Global.CL[Global.CI] == '_' || char.IsLetter(Global.CL[Global.CI]))))
                        {
                            Global.Message_Wrong = Error.Get_Error(12)+"\t"+Error.Get_Type_Error(0);
                            throw new Exception();
                        }
                        Global.UL = Lexical.Lexical_Unit();
                        if (Global.UL != Global.TypeSymbol.u_UNKNOWN)
                        {
                            Global.Message_Wrong = Error.Get_Error(13)+"\t"+Error.Get_Type_Error(3);
                            throw new Exception();
                        }
                        TIdentif temp = Global.G_Var_Define;
                        Global.Add_IDentif(Global.G_Cur_STR, ref temp,Global.TypeSymbol.u_VAR_DEFINE);
                        Global.G_Var_Define = (TDenfine)temp;
                        TDenfine Def_Aux = Global.G_Var_Define;
                        Lexical.Skip_Spaces();
                        if (!(Global.CI < Global.CL.Length && (char.IsDigit(Global.CL[Global.CI]) || Global.CL[Global.CI] == '.' ||
                            Global.CL[Global.CI] == '\'')))
                        {
                            Global.Message_Wrong = Error.Get_Error(14)+"\t"+Error.Get_Type_Error(0);
                            throw new Exception();
                        }
                        Global.UL = Lexical.Lexical_Unit();
                        if (Global.UL == Global.TypeSymbol.u_CST_INT || Global.UL == Global.TypeSymbol.u_CST_REAL)
                        {
                            Def_Aux.Val_NB = Global.G_Cur_NB;
                        }
                        else
                        {
                            if (Global.UL == Global.TypeSymbol.u_CST_STR)
                            {
                                Def_Aux.Val_STR = Global.G_Cur_STR;
                            }
                            else
                            {
                                Global.Message_Wrong = Error.Get_Error(15)+"\t" + Error.Get_Type_Error(1);//lexical wrong in string or number
                                throw new Exception();
                            }

                        }
                        Def_Aux.kind = Global.UL;
                        
                        #endregion

                    }//end if define
                    else
                    {
                        Global.Message_Wrong = Error.Get_Error(16) + "\t" + Error.Get_Type_Error(0);
                        throw new Exception();
                    }
                }//end if include or define
                else 
                {
                    Global.Message_Wrong = Error.Get_Error(16) + "\t" + Error.Get_Type_Error(0);
                    throw new Exception();
                }
                #endregion
               Lexical.Skip_Spaces();

                if (Global.CI < Global.CL.Length)
                {
                    Global.Message_Wrong = Error.Get_Error(24) +"\t"+ Error.Get_Type_Error(0);
                    throw new Exception();
                }
                if (!Lexical.Read_New_Line())
                {
                    return;
                }
            }//end while # 
            #endregion

            Global.UL = Lexical.Lexical_Unit();

            #region Procedure & Function
 
            while (Global.UL == Global.TypeSymbol.u_PROCEDURE || Global.UL == Global.TypeSymbol.u_FUNCTION)
            {
                Global.TypeSymbol Aux_Ul = Global.UL;
                Global.UL = Lexical.Lexical_Unit();
                TProcedure Cur_Procedure;
                if (Global.UL == Global.TypeSymbol.u_UNKNOWN)
                {
                   TIdentif temp = Global.G_Var_Proc;
                    Global.Add_IDentif(Global.G_Cur_STR, ref temp,Global.TypeSymbol.u_VAR_PROC);
                   Global.G_Var_Proc = (TProcedure)temp;
                    Cur_Procedure = Global.G_Var_Proc;
                    Cur_Procedure.Is_Define = true;//عرّفت
                }
                else
                {
                    if (Global.UL == Global.TypeSymbol.u_VAR_PROC)
                    {
                        Cur_Procedure = (TProcedure)Global.G_Cur_Id;
                        if (Cur_Procedure.Is_Define)
                        {
                            Global.Message_Wrong = Error.Get_Error(17) + "\t" + Error.Get_Type_Error(3);
                            throw new Exception();//defin before
                        }
                        Cur_Procedure.Is_Define = true;
                    }
                    else
                    {
                        Global.Message_Wrong = Error.Get_Error(18) + "\t" + Error.Get_Type_Error(1);
                        throw new Exception();
                    }
                }
                Cur_Procedure.Is_Function = (Aux_Ul == Global.TypeSymbol.u_FUNCTION);
                Global.UL = Lexical.Lexical_Unit();
                if (Global.UL != Global.TypeSymbol.u_OPENP)
                {
                    Global.Message_Wrong = Error.Get_Error(19) + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Cur_Procedure.P_In = null;
                Cur_Procedure.P_Out = null;
                Global.UL = Lexical.Lexical_Unit();
                if (Global.UL == Global.TypeSymbol.u_INPUT)
                {
                    Global.UL = Lexical.Lexical_Unit();
                    while (true)
                    {
                        if (Global.UL != Global.TypeSymbol.u_UNKNOWN)
                        {
                            Global.Message_Wrong = Error.Get_Error(20)+" *OR*  "+Error.Get_Error(18) + "\t" + Error.Get_Type_Error(3);
                            throw new Exception();
                        }
                       TIdentif temp = Global.G_Var;
                        Global.Add_IDentif(Global.G_Cur_STR, ref temp,Global.TypeSymbol.u_VAR);
                       Global.G_Var = (TPVar)temp;
                        Global.UL = Lexical.Lexical_Unit();
                        if (Global.UL == Global.TypeSymbol.u_COMMA)
                            Global.UL = Lexical.Lexical_Unit();
                        else 
                            break;

                    }//end while true
                
                    Cur_Procedure.P_In = Global.G_Var;
                }//end if input
                    if (Global.UL == Global.TypeSymbol.u_OUTPUT)
                    {
                        Global.UL = Lexical.Lexical_Unit();
                        while (true)
                        {
                            if (Global.UL != Global.TypeSymbol.u_UNKNOWN)
                            {
                                Global.Message_Wrong = Error.Get_Error(20) + " *OR*  " + Error.Get_Error(18) + "\t" + Error.Get_Type_Error(3);
                                throw new Exception();
                            }
                            TIdentif temp = Global.G_Var;
                            Global.Add_IDentif(Global.G_Cur_STR, ref temp,Global.TypeSymbol.u_VAR);
                            Global.G_Var = (TPVar)temp;
                            Global.UL = Lexical.Lexical_Unit();
                            if (Global.UL == Global.TypeSymbol.u_COMMA)
                                Global.UL = Lexical.Lexical_Unit();
                            else break;

                        }//end while true
                        Cur_Procedure.P_Out = Global.G_Var;
                    }//end if output
                    if (Global.UL != Global.TypeSymbol.u_CLOSEP)
                    {
                        Global.Message_Wrong = Error.Get_Error(21) + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                    Global.UL = Lexical.Lexical_Unit();
                    if (Global.UL != Global.TypeSymbol.u_SEMICOLON) 
                    {
                        Global.Message_Wrong = Error.Get_Error(22) + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                    Global.UL = Lexical.Lexical_Unit();
                    Cur_Procedure.List = Instruction_Class.Read_List_Of_Instruction();
                    Cur_Procedure.L_Var = Global.G_Var;
                    Global.G_Var = null;
                }//end while proc or func

            #endregion procedure & function

            #region MAIN

            if (Global.UL==Global.TypeSymbol.u_MAIN)
            {
                Global.READ__Word_Main = true;
                if (Global.G_Cur_File!=Global.GFile)
                {
                    Global.Message_Wrong = Error.Get_Error(47) + "\t" + Error.Get_Type_Error(2);
                    throw new Exception();
                }
                Global.UL = Lexical.Lexical_Unit();
                Global.Main_INST = Instruction_Class.Read_List_Of_Instruction();
                if (Global.UL!=Global.TypeSymbol.u_EOF)
                {
                    Global.Message_Wrong = Error.Get_Error(48) + "\t" + Error.Get_Type_Error(2);
                    throw new Exception();
                }
            }//end if main

            #endregion MAIN

            if (Global.UL!=Global.TypeSymbol.u_EOF)
            {
                Global.Message_Wrong = Error.Get_Error(33) + "\t" + Error.Get_Type_Error(2);
                throw new Exception();
            }
            if (!Global.READ__Word_Main&&Global.G_Cur_File==Global.GFile)
            {
                Global.Message_Wrong = Error.Get_Error(52) + "\t" + Error.Get_Type_Error(2);
                throw new Exception();
            }

        }//end compile current file

            
    }   
        
}
