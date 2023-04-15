using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
    public static class Lexical
    {

        public static Boolean Read_New_Line()
        {

            while (!Global.CF.EndOfStream)
            {
                Global.CL = Global.CF.ReadLine().Trim();
                if (Global.CL.Length > 0 && Global.CL != " ")
                {
                    Global.CI = 0;
                    return true;
                }
            } 
            return false;
        }

        public static Boolean Skip_Spaces_And_Comment()
        {
            while (true)
            {
                while (Global.CI < Global.CL.Length && Global.CL[Global.CI] == ' ')//مناقشة الفراغات
                {
                    Global.CI++;
                }
                if (Global.CI == Global.CL.Length)
                    if (!Read_New_Line())
                        return false;
                if (Global.CI + 1 == Global.CL.Length)
                {
                    return true;
                }
                if (Global.CI + 1 < Global.CL.Length && Global.CL[Global.CI] == '/' && Global.CL[Global.CI + 1] == '/')
                {
                    if (!Read_New_Line())
                        return false;
                }
                else
                {
                    if (Global.CI + 1 < Global.CL.Length && Global.CL[Global.CI] == '/' && Global.CL[Global.CI + 1] == '*')
                    {
                        Global.CI += 2;
                        while (true)
                        {
                            while (Global.CI + 1 < Global.CL.Length && !(Global.CL[Global.CI] == '*' && Global.CL[Global.CI + 1] == '/'))
                                Global.CI++;

                            if (Global.CI + 1 == Global.CL.Length || Global.CI == Global.CL.Length)
                            {
                                if (!Read_New_Line())
                                {
                                   Global.Message_Wrong = Error.Get_Error(9)+"\t"+Error.Get_Type_Error(1);
                                   //return false;
                                   throw new Exception();
                                }
                            }
                            else
                            {
                                Global.CI += 2;
                                break; 
                            }
                        }
                    }
                    else
                        return true;
                }
            }
        }

        public static void Skip_Spaces()
        {
            while (Global.CI < Global.CL.Length && Global.CL[Global.CI] == ' ')
                Global.CI++;
        }

        public static Global.TypeSymbol Lexical_Unit()
        {
            Double Dec = 0;
            Double p = 10;
            Boolean Is_Real = false;
            Boolean Single_Point = true;
            if (!Lexical.Skip_Spaces_And_Comment())
            { return Global.TypeSymbol.u_EOF; }
            
            if (char.IsDigit(Global.CL[Global.CI]) || Global.CL[Global.CI] == '.')
            {

                #region number
                Global.G_Cur_NB = 0;
                while (Global.CI < Global.CL.Length && char.IsDigit(Global.CL[Global.CI]))
                {
                    Single_Point = false;
                    Global.G_Cur_NB = Global.G_Cur_NB * 10 + Convert.ToInt32(Global.CL[Global.CI]) - Convert.ToInt32('0');
                    Global.CI++;
                }
                if (Global.CI < Global.CL.Length && (Global.CL[Global.CI]) == '.')
                {
                    Is_Real = true;
                    Global.CI++;
                    while (Global.CI < Global.CL.Length && char.IsDigit(Global.CL[Global.CI]))
                    {
                        Single_Point = false;
                        Dec = Dec + (Convert.ToInt32(Global.CL[Global.CI]) - Convert.ToInt32('0')) / p;
                        p = p * 10;
                        Global.CI++;
                    }
                    Global.G_Cur_NB = Global.G_Cur_NB + Dec;
                }
                if (Single_Point)
                {
                    Global.Message_Wrong = Error.Get_Error(2) + "\t" + Error.Get_Type_Error(1);
                    throw new Exception();
                }
                if (Global.CI + 1 < Global.CL.Length && (Global.CL[Global.CI] == 'e' || Global.CL[Global.CI] == 'E') &&
                    (char.IsDigit(Global.CL[Global.CI + 1]) || Global.CL[Global.CI + 1] == '+' || Global.CL[Global.CI + 1] == '-'))//if e
                {
                    Is_Real = true;
                    if(( Global.CL[Global.CI + 1] == '+' || Global.CL[Global.CI + 1] == '-')
                        &&!(Global.CI+2<Global.CL.Length&&char.IsDigit(Global.CL[Global.CI+2])))
                    {
                        Global.Message_Wrong = Error.Get_Error(3) + "\t" + Error.Get_Type_Error(1);
                        throw new Exception();
                    }
                    p=+1;
                    if(Global.CL[Global.CI+1]=='-')
                    {
                        Global.CI=Global.CI+2;
                        p=-1;
                    }
                    else if(Global.CL[Global.CI+1]=='+')
                        Global.CI=Global.CI+2;
                    else
                        Global.CI++;
                    Dec=0;
                    while(Global.CI<Global.CL.Length&&char.IsDigit(Global.CL[Global.CI]))
                    {
                        Dec = Dec * 10 + Convert.ToInt32(Global.CL[Global.CI]) - Convert.ToInt32('0');
                        Global.CI++;
                    }
                    Global.G_Cur_NB = Global.G_Cur_NB * Math.Pow(10, p * Dec);
                }//end if e E
                Global.buffer_Temp = Global.G_Cur_NB.ToString();/////
                if (Is_Real)
                    return Global.TypeSymbol.u_CST_REAL;
                else return Global.TypeSymbol.u_CST_INT;

            }//end if Number
            
            #endregion


            else
                if (Global.CL[Global.CI] == '\'')
                {

                    #region string
                    Global.CI++;//skip first '
                    Global.G_Cur_STR = "";
                    while (Global.CI < Global.CL.Length && Global.CL[Global.CI] != '\'')
                    {
                        if (Global.CL[Global.CI] == '\\')
                        {
                            // \\3 \3
                            //   \\\3 error
                            if (Global.CI + 1 < Global.CL.Length && Global.CL[Global.CI + 1] == '\\' || Global.CL[Global.CI + 1] == '\'')
                            {
                                Global.CI++;//skip first \
                            }
                            else 
                            {
                                Global.Message_Wrong = Error.Get_Error(4) + "\t" + Error.Get_Type_Error(1);
                               throw new Exception();
                            }                                
                        }
                        Global.G_Cur_STR = Global.G_Cur_STR + Global.CL[Global.CI];
                        Global.CI++;
                    }
                    if (Global.CI == Global.CL.Length)
                    {
                        Global.Message_Wrong = Error.Get_Error(5) + "\t" + Error.Get_Type_Error(1);
                        throw new Exception (); 
                    }
                    Global.CI++;//skip second '
                    Global.buffer_Temp = Global.G_Cur_STR;//////
                    return Global.TypeSymbol.u_CST_STR;

                    #endregion

                }//end if string
                else
                {
                    if(char.IsLetter(Global.CL[Global.CI])||Global.CL[Global.CI]=='_')
                    {

                   #region Identifire

                        Global.buffer = "";
                        while(Global.CI<Global.CL.Length
                            &&(char.IsLetter(Global.CL[Global.CI])||Global.CL[Global.CI]=='_'||char.IsDigit(Global.CL[Global.CI])))
                        {
                                Global.buffer = Global.buffer + Global.CL[Global.CI];
                                Global.CI++;
                        }
                        TSymbol Temp;
                        Temp= Global.FindSymbol(Global.buffer.ToUpper());
                        if (Temp != null)
                        {
                            Global.buffer_Temp = Temp.name;
                            return Temp.ul;
                        }
                        else
                        {
                            TIdentif GID;
                            GID = Global.Find_IDentif(Global.buffer.ToUpper(), Global.G_Var);
                            if (GID != null)
                            {
                                Global.buffer_Temp = GID.name;
                                Global.G_Cur_Id = GID;
                                return GID.ul;
                            }
                            else
                            {
                                GID = Global.Find_IDentif(Global.buffer.ToUpper(), Global.G_Var_Proc);
                                if (GID != null)
                                {
                                    Global.buffer_Temp = GID.name;
                                    Global.G_Cur_Id = GID;
                                    return GID.ul;
                                }
                                else
                                {
                                    GID = Global.Find_IDentif(Global.buffer.ToUpper(), Global.G_Var_Define);
                                    if (GID != null)
                                    {
                                        Global.buffer_Temp = GID.name;
                                        Global.G_Cur_Id = GID;
                                        return GID.ul;
                                    }
                                    else
                                    {
                                        Global.buffer_Temp = Global.buffer;
                                        Global.G_Cur_STR = Global.buffer.ToUpper();
                                        return Global.TypeSymbol.u_UNKNOWN;
                                    }
                                }

                            }
                        }

                   #endregion Identifire

                    }
                    else
                    {

                     #region  Sign
                    
                        if(Global.CL[Global.CI]==':')
                        {
                            Global.CI++;
                            if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '=')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_ASSIGN;
                            }
                            else
                            {
                                Global.Message_Wrong = Error.Get_Error(0) + "\t" + Error.Get_Type_Error(1);/////message
                               throw new Exception(); 
                            }
                        }
                            if(Global.CL[Global.CI]=='+')
                            {
                                Global.CI++;
                                Global.buffer_Temp = "+";
                                return Global.TypeSymbol.u_PLUS;
                            }
                            if (Global.CL[Global.CI] == '=')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_EQUAL;
                            }
                            if (Global.CL[Global.CI] == '-')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_MINUS;
                            }
                            if (Global.CL[Global.CI] == '!')
                            {
                                Global.CI++;
                                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '=')
                                {
                                    Global.CI++;
                                    return Global.TypeSymbol.u_NOT_EQUAL;
                                }
                                else
                                    return Global.TypeSymbol.u_NOT;
                            }
                            if (Global.CL[Global.CI] == '*')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_MULTI;
                            }
                            if (Global.CL[Global.CI] == '/')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_DIV;
                            }
                            if (Global.CL[Global.CI] == '^')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_POWER_SIGHN;
                            }
                            if (Global.CL[Global.CI] == '&')
                            {
                                Global.CI++;
                                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '&')
                                {
                                    Global.CI++;
                                    return Global.TypeSymbol.u_AND;
                                }
                                else
                                {
                                    Global.Message_Wrong = Error.Get_Error(6) + "\t" + Error.Get_Type_Error(1);
                                   throw new Exception();
                                }
                            }
                            if (Global.CL[Global.CI] == '|')
                            {
                                Global.CI++;

                                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '|')
                                {
                                    Global.CI++;
                                    return Global.TypeSymbol.u_OR;
                                }
                                else
                                {
                                   Global.Message_Wrong = Error.Get_Error(7)+"\t"+Error.Get_Type_Error(1);
                                   throw new Exception(); 
                                }
                            }
                            if (Global.CL[Global.CI] == '<')
                            {
                                Global.CI++;
                                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '=')
                                {
                                    Global.CI++;
                                    return Global.TypeSymbol.u_LE;
                                }
                                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '>')
                                {
                                    Global.CI++;
                                    return Global.TypeSymbol.u_NOT_EQUAL;
                                }
                                return Global.TypeSymbol.u_LT;
                            }
                            if (Global.CL[Global.CI] == '>')
                            {
                                Global.CI++;
                                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '=')
                                {
                                    Global.CI++;
                                    return Global.TypeSymbol.u_GE;
                                }
                                return Global.TypeSymbol.u_GT;
                            }
                            if (Global.CL[Global.CI] == '(')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_OPENP;
                            }
                            if (Global.CL[Global.CI] == ')')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_CLOSEP;
                            }
                            if (Global.CL[Global.CI] == '[')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_OPENB;
                            }
                            if (Global.CL[Global.CI] == ']')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_CLOSEB;
                            } if (Global.CL[Global.CI] == '{')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_BEGIN;
                            }
                            if (Global.CL[Global.CI] == '}')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_END;
                            }
                            if (Global.CL[Global.CI] == ';')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_SEMICOLON;
                            } 
                            if (Global.CL[Global.CI] == ',')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_COMMA;
                            }
                            //if (Global.CL[Global.CI] == '.')
                            //{
                            //    Global.CI++;
                            //    return Global.TypeSymbol.u_FULLSTOP;
                            //} if (Global.CL[Global.CI] == '#')
                            //{
                            //    Global.CI++;
                            //    return Global.TypeSymbol.u_SHARP;
                            //} 
                        if (Global.CL[Global.CI] == '%')
                            {
                                Global.CI++;
                                return Global.TypeSymbol.u_MOD;
                            }

                     #endregion

                            else
                            {
                                {
                                    Global .Message_Wrong =Error.Get_Error(8)+"\t"+Error.Get_Type_Error(1);
                                    Global.CI++;
                                    throw new Exception();
                                }
                            }
                    }

                }

        }
    }
}