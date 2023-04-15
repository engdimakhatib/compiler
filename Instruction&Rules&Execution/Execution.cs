using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Compiler_Compiler
{
   public class Execution
    {
       public static TEXP Copy_Expression(TEXP Exp)
       {
           TEXP FST_EXP = null;
           TEXP Last_Exp = null;
           while (Exp != null)
           {
               TEXP Exp_New = new TEXP();
               Exp_New.UL = Exp.UL;
               Exp_New.Val_Call = Exp.Val_Call;
               Exp_New.Val_NB = Exp.Val_NB;
               Exp_New.Val_STR = Exp.Val_STR;
               Exp_New.Val_Var = Exp.Val_Var;
               Exp_New.index = Exp.index;
               Exp_New.next = null;
               if (FST_EXP == null)
               {
                   FST_EXP = Exp_New;
               }
               else
               {
                   Last_Exp.next = Exp_New;
                   Exp_New.prev = Last_Exp;
               }
               Last_Exp = Exp_New;

               Exp = Exp.next;
           }
           return FST_EXP;
       }

        #region Execute_Methods

        public static Global.TypeSymbol Execute_List_Of_Instruction(TInstruction LInst)
        {
            while (LInst != null)
            {
                if (LInst.INS is TIF)
                {
                    #region IF
                    TIF If_Aux = LInst.INS as TIF;
                    if (Execution.Evalute_COND(If_Aux.cond))
                        Global.UL = Execution.Execute_List_Of_Instruction(If_Aux.INS_IF);
                    else
                        Global.UL = Execution.Execute_List_Of_Instruction(If_Aux.INS_ELSE);
                    if (Global.UL == Global.TypeSymbol.u_HALT || Global.UL == Global.TypeSymbol.u_EXIT ||
                        Global.UL == Global.TypeSymbol.u_BREAK || Global.UL == Global.TypeSymbol.u_RETURN)
                        return Global.UL;
                    #endregion
                }
                else
                    if (LInst.INS is TWHILE)
                    {
                        #region While
                        TWHILE While_Aux = LInst.INS as TWHILE;
                        while (Execution.Evalute_COND(While_Aux.cond))
                        {
                            Global.UL = Execution.Execute_List_Of_Instruction(While_Aux.L_inst);
                            if (Global.UL == Global.TypeSymbol.u_HALT || Global.UL == Global.TypeSymbol.u_EXIT ||
                                Global.UL == Global.TypeSymbol.u_RETURN)
                            {
                                return Global.UL;
                            }
                            if (Global.UL == Global.TypeSymbol.u_BREAK)
                            {
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                        if (LInst.INS is TFOR)
                        {
                            #region for
                            TFOR For_Aux = LInst.INS as TFOR;
                            TEXP Exp_B = Execution.Evalute_Exp(For_Aux.Exp_Begin);
                            TEXP Exp_E = Execution.Evalute_Exp(For_Aux.Exp_End);
                            Execution.Assign(For_Aux.V, 0, Exp_B);
                            if (!For_Aux.Is_Down)
                            {
                                while (Exp_B.Val_NB <= Exp_E.Val_NB)
                                {
                                    Global.UL = Execution.Execute_List_Of_Instruction(For_Aux.L_inst);
                                    if (Global.UL == Global.TypeSymbol.u_BREAK)
                                    {
                                        break;
                                    }
                                    if (Global.UL == Global.TypeSymbol.u_HALT || Global.UL == Global.TypeSymbol.u_EXIT ||
                                        Global.UL == Global.TypeSymbol.u_RETURN)
                                    {
                                        return Global.UL;
                                    }
                                    Exp_B.Val_NB = Exp_B.Val_NB + 1;
                                    Execution.Assign(For_Aux.V, 0, Exp_B);
                                }
                            }//end for up
                            else
                            {
                                while (Exp_B.Val_NB >= Exp_E.Val_NB)
                                {
                                    Global.UL = Execution.Execute_List_Of_Instruction(For_Aux.L_inst);
                                    if (Global.UL == Global.TypeSymbol.u_RETURN || Global.UL == Global.TypeSymbol.u_HALT
                                        || Global.UL == Global.TypeSymbol.u_EXIT)
                                    {
                                        return Global.UL;
                                    }
                                    if (Global.UL == Global.TypeSymbol.u_BREAK)
                                    {
                                        break;
                                    }
                                    Exp_B.Val_NB = Exp_B.Val_NB - 1;
                                    Execution.Assign(For_Aux.V, 0, Exp_B);
                                }
                            }//end is down
                            TEXP.Free(Exp_B);
                            TEXP.Free(Exp_E);
                            #endregion
                        }
                        else
                            if (LInst.INS is TBreak)
                            {
                                #region break
                                TBreak Break_Aux = LInst.INS as TBreak;
                                return Break_Aux.ul;
                                #endregion
                            }
                            else
                                if (LInst.INS is TReturn)
                                {
                                    #region return

                                    TReturn Return_Aux = LInst.INS as TReturn;
                                    Global.G_Return = Return_Aux;
                                    return Global.TypeSymbol.u_RETURN;

                                    #endregion return
                                }
                                else
                                    if (LInst.INS is TAssign)
                                    {
                                        #region assign
                                        TAssign Assign_Aux = LInst.INS as TAssign;
                                        TEXP Exp_Second = Execution.Evalute_Exp(Assign_Aux.exp);
                                        if (Assign_Aux.index == null)
                                        {
                                            Execution.Assign(Assign_Aux.V, 0, Exp_Second);
                                        }
                                        else
                                        {
                                            TEXP Index_Exp = Execution.Evalute_INT(Assign_Aux.index);
                                            int index = Convert.ToInt32(Index_Exp.Val_NB);
                                            if (index<1)
                                            {
                                                Global.Message_Wrong = Error.Get_Error(49) + "\t" + Error.Get_Type_Error(2);
                                                throw new Exception();
                                            }
                                            Execution.Assign(Assign_Aux.V,Convert.ToInt32( Index_Exp.Val_NB), Exp_Second);
                                            TEXP.Free(Index_Exp);
                                        }
                                        //TEXP.Free(Exp_Second);
                                        #endregion
                                    }
                                    else
                                        if (LInst.INS is TCall)
                                        {
                                            #region Call
                                            //exceute procedure take it from inst
                                            TCall Call_Aux = LInst.INS as TCall;
                                            Global.UL = Execution.Execute_Call(Call_Aux);
                                            if (Global.UL == Global.TypeSymbol.u_RETURN)
                                            {
                                                Global.Message_Wrong = Error.Get_Error(38) + "\t" + Error.Get_Type_Error(2);
                                                throw new Exception();
                                            }
                                            if (Global.UL == Global.TypeSymbol.u_HALT)
                                            {
                                                return Global.UL;
                                            }

                                            #endregion Call
                                        }
                                        else
                                            if (LInst.INS is TRead)
                                            {
                                                #region Execute Read inst

                                                TRead Read_Aux = LInst.INS as TRead;
                                                String Value_From_INput = "";
                                                INPUT Input_Form = new INPUT();
                                                while (Value_From_INput=="")
                                                {
                                                    Input_Form.ShowDialog();
                                                    Value_From_INput=(Input_Form.TextBox1.Text).Trim();
                                                }//end while

                                                TEXP Exp = new TEXP();
                                                if (Global.IS_INPUT_INTEGER)
                                                {
                                                    Exp.UL = Global.TypeSymbol.u_CST_INT;
                                                    Exp.Val_NB = Convert.ToInt32(Value_From_INput);
                                                }
                                                else
                                                {
                                                    Exp.UL = Global.TypeSymbol.u_CST_STR;
                                                    Exp.Val_STR = Value_From_INput;
                                                }
                                                Exp.next = null;

                                                int i;
                                                if (Read_Aux.Index == null)
                                                {
                                                    i = 0;
                                                }
                                                else
                                                {
                                                    TEXP Exp_Aux = Execution.Evalute_INT(Read_Aux.Index);
                                                    i = Convert.ToInt32(Exp_Aux.Val_NB);
                                                    if (i<1)
                                                    {
                                                        Global.Message_Wrong = Error.Get_Error(49) + "\t" + Error.Get_Type_Error(2);
                                                        throw new Exception();
                                                    }
                                                    TEXP.Free(Exp_Aux);
                                                }
                                                Execution.Assign(Read_Aux.v, i, Exp);
                                                TEXP.Free(Exp);

                                                #endregion Execute Read inst
                                            }
                                            else
                                                if (LInst.INS is TWRITE)
                                                {
                                                    #region Execute write inst

                                                    
                                                    TWRITE Write_Aux = LInst.INS as TWRITE;
                                                    string Write_In_Output = "";
                                                    TEXP Exp = Execution.Evalute_Exp(Write_Aux.exp);
                                                    while (Exp!=null)
                                                    {

                                                        #region Write value

                                                        if (Exp.UL == Global.TypeSymbol.u_CST_INT)
                                                        {
                                                            Write_In_Output = Write_In_Output + Exp.Val_NB.ToString();
                                                        }
                                                        else
                                                            if (Exp.UL == Global.TypeSymbol.u_CST_REAL)
                                                            {
                                                                Write_In_Output = Write_In_Output + Exp.Val_NB.ToString();
                                                            }
                                                            else
                                                                if (Exp.UL == Global.TypeSymbol.u_CST_STR)
                                                                {
                                                                    Write_In_Output = Write_In_Output + Exp.Val_STR;
                                                                }
                                                                else
                                                                    if (Exp.UL == Global.TypeSymbol.u_TRUE)
                                                                    {
                                                                        Write_In_Output = Write_In_Output + "True";
                                                                    }
                                                                    else
                                                                        if (Exp.UL == Global.TypeSymbol.u_FALSE)
                                                                        {
                                                                            Write_In_Output = Write_In_Output + "False";
                                                                        }
                                                                        else
                                                                        {
                                                                            Write_In_Output = Write_In_Output + "Unknown";
                                                                        }

                                                        #endregion Write value

                                                        TEXP Exp_Aux = Exp;
                                                        Exp = Exp.next;
                                                        TEXP.Free(Exp_Aux);

                                                        if (Write_Aux.Is_ln)
                                                        {
                                                            Write_In_Output += "\n";
                                                        }
                                                        
                                                    }//end while
                                                   // OUTPUT Output_Write_Form = new OUTPUT();
                                                   Global.Output_Write_Form.RichTextBox1.Text += Write_In_Output.ToString();
                                                   Global.Output_Write_Form.Show();
                                                    //if (Global.Is_Closed)
                                                    //{
                                                    //    Output_Write_Form = null;
                                                    //}
                                                    #endregion Execute Write inst
                                                }
                LInst = LInst.next;
            }//end while
            
            return Global.TypeSymbol.u_NORMAL;
        }

        public static Global.TypeSymbol Execute_Call(TCall Call_Aux)
        {
            #region input
            TEXP Exp0 = Execution.Evalute_Exp(Call_Aux.PIN);
            while (Exp0 != null && Exp0.next != null)
            {
                Exp0 = Exp0.next;
            }//حتى تشير إلى الذيل لمقارنتها مع الدخل للتصريح حيث الإضافة للرأس
            TPVar Var_Aux = Call_Aux.F.P_In;
            while (Var_Aux != null && Exp0 != null)
            {
                TEXP Exp_Aux = Exp0.prev;
                if(Exp_Aux!=null)
                Exp_Aux.next = null;
                Exp0.prev = null;
                Execution.Assign(Var_Aux, 0, Exp0);//إسناد الاستدعاء للتصريح
                TEXP.Free(Exp0);
                Exp0 = Exp_Aux;
                Var_Aux = (TPVar)Var_Aux.next;
            }
            if (Var_Aux != null || Exp0 != null)
            {
                Global.Message_Wrong = Error.Get_Error(36) + "\t" + Error.Get_Type_Error(2);
                throw new Exception();
            }
            #endregion

            #region output
            TList_Var LV = Call_Aux.POUT;//الاستدعاء
            Var_Aux = Call_Aux.F.P_Out;//التصريح
            if (Var_Aux==null)
            {
                Var_Aux = Call_Aux.F.P_In;
            }
            while (Var_Aux != Call_Aux.F.P_In && LV != null)
            {
                Execution.Assign_Var(Var_Aux,LV.V);//إسناد التصريح للاستدعاء///////////////////////////////////////////////////////////
                LV = LV.next;
                Var_Aux = (TPVar)Var_Aux.next;
            }
            if (LV != null || Var_Aux != Call_Aux.F.P_In)
            {
                Global.Message_Wrong = Error.Get_Error(37) + "\t" + Error.Get_Type_Error(2);
                throw new Exception(); 
            }
            #endregion

            Global.UL = Execution.Execute_List_Of_Instruction(Call_Aux.F.List);

            #region Output
            Var_Aux = Call_Aux.F.P_Out;
            LV = Call_Aux.POUT;
            if (Var_Aux == null)
            {
                Var_Aux = Call_Aux.F.P_In;
            }
            while (Var_Aux != Call_Aux.F.P_In && LV != null)
            {
                Execution.Assign_Var(LV.V,Var_Aux);/////////////////إسناد الاستدعاء للتصريح
                Var_Aux = (TPVar)Var_Aux.next;
                LV = LV.next;
            } 
            #endregion

            return Global.UL;
        }

        #endregion Execute_Methods

        #region Evalute_Methods

       public static Boolean Evalute_COND(TEXP Cond)
       {
           TEXP Exp = Execution.Evalute_Exp(Cond);
           if( (Exp.UL!=Global.TypeSymbol.u_TRUE)&&(Exp.UL!=Global.TypeSymbol.u_FALSE))
           {
               Global.Message_Wrong = Error.Get_Error(41) + "\t" + Error.Get_Type_Error(2);
               throw new Exception();
           }
           Boolean Result = (Exp.UL == Global.TypeSymbol.u_TRUE);
           Free_Class.Free_EXP(Exp);
           return Result;
       }

       public static TEXP Evalute_INT(TEXP exp)
       {
           TEXP Exp = Execution.Evalute_Exp(exp);
           if (Exp.UL!=Global.TypeSymbol.u_CST_INT)
           {
               Global.Message_Wrong = Error.Get_Error(50) + "\t" + Error.Get_Type_Error(2);
               throw new Exception();
           }
           return Exp;
       }

       public static TEXP Evalute_Exp(TEXP exp)
       {
           TEXP Exp0 = Execution.Copy_Expression(exp);
           TEXP Cur_Exp = Exp0;
           while (Cur_Exp != null)
           {
               if (Cur_Exp.UL == Global.TypeSymbol.u_CST_INT || Cur_Exp.UL == Global.TypeSymbol.u_CST_REAL ||
                   Cur_Exp.UL == Global.TypeSymbol.u_FALSE || Cur_Exp.UL == Global.TypeSymbol.u_TRUE ||
                   Cur_Exp.UL == Global.TypeSymbol.u_CST_STR || Cur_Exp.UL == Global.TypeSymbol.u_VAR_DEFINE)
               {
                   Cur_Exp = Cur_Exp.next;
               }
               else
                   if (Cur_Exp.UL == Global.TypeSymbol.u_VAR)
                   {
                       #region Var

                       int i;

                       if (Cur_Exp.index != null)
                       {
                           TEXP Exp_Aux = Execution.Evalute_INT(Cur_Exp.index);
                           i = Convert.ToInt32(Exp_Aux.Val_NB);
                           if (i < 1)
                           {
                               Global.Message_Wrong = Error.Get_Error(49) + "\t" + Error.Get_Type_Error(2);
                               throw new Exception();
                           }
                           TEXP.Free(Exp_Aux);
                       }
                       else
                           i = 1;
                       TItem Item_Aux = Cur_Exp.Val_Var.items;
                       while (Item_Aux != null && i > 1)
                       {
                           i = i - 1;
                           Item_Aux = Item_Aux.next;
                       }
                       if (Item_Aux == null)
                       {
                           Cur_Exp.UL = Global.TypeSymbol.u_UNKNOWN;
                       }
                       else
                       {
                           Cur_Exp.UL = Item_Aux.kind;
                           Cur_Exp.Val_NB = Item_Aux.Val_NB;
                           Cur_Exp.Val_STR = Item_Aux.Val_STR;
                       }
                       Cur_Exp = Cur_Exp.next;

                       #endregion var
                   }
                   else
                       if (Cur_Exp.UL == Global.TypeSymbol.u_VAR_PROC)//////////////////////////////////////////////////////
                       {
                           #region Call

                           //function (exp.valcall)
                           TCall Call_Aux = Cur_Exp.Val_Call;
                           Global.UL = Execution.Execute_Call(Call_Aux);

                           if (Global.UL != Global.TypeSymbol.u_RETURN)
                           {
                               Global.Message_Wrong = Error.Get_Error(39) + "\t" + Error.Get_Type_Error(2);
                               throw new Exception();
                           }

                           TEXP Exp_Aux = Execution.Evalute_Exp(Global.G_Return.exp);
                           Cur_Exp.UL = Exp_Aux.UL;
                           Cur_Exp.Val_NB = Exp_Aux.Val_NB;
                           Cur_Exp.Val_STR = Exp_Aux.Val_STR;
                           Free_Class.Free_EXP(Exp_Aux);
                           Cur_Exp = Cur_Exp.next;

                           #endregion call
                       }
                       else
                           if (Cur_Exp.UL == Global.TypeSymbol.u_UNARY_MINUS || Cur_Exp.UL == Global.TypeSymbol.u_UNARY_PLUS
                               || Cur_Exp.UL == Global.TypeSymbol.u_NOT || Cur_Exp.UL == Global.TypeSymbol.u_SIN
                               || Cur_Exp.UL == Global.TypeSymbol.u_COS || Cur_Exp.UL == Global.TypeSymbol.u_TAN
                               || Cur_Exp.UL == Global.TypeSymbol.u_ATAN || Cur_Exp.UL == Global.TypeSymbol.u_STRING_TO_INT
                               || Cur_Exp.UL == Global.TypeSymbol.u_LENGTH || Cur_Exp.UL == Global.TypeSymbol.u_INT_TO_STRING
                               || Cur_Exp.UL == Global.TypeSymbol.u_LOG || Cur_Exp.UL == Global.TypeSymbol.u_LN ||
                                  Cur_Exp.UL == Global.TypeSymbol.u_SQRT || Cur_Exp.UL == Global.TypeSymbol.u_ABS)
                           {
                               #region Function with one parameter

                               #region condition for type one parameter

                               if ((Cur_Exp.UL == Global.TypeSymbol.u_UNARY_MINUS || Cur_Exp.UL == Global.TypeSymbol.u_SIN
                                   || Cur_Exp.UL == Global.TypeSymbol.u_TAN || Cur_Exp.UL == Global.TypeSymbol.u_ATAN
                                   || Cur_Exp.UL == Global.TypeSymbol.u_COS
                                   || Cur_Exp.UL == Global.TypeSymbol.u_UNARY_PLUS || Cur_Exp.UL == Global.TypeSymbol.u_LOG
                                   || Cur_Exp.UL == Global.TypeSymbol.u_LN || Cur_Exp.UL == Global.TypeSymbol.u_SQRT || Cur_Exp.UL == Global.TypeSymbol.u_ABS) &&
                                   !(Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_INT || Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_REAL))
                               {
                                   Global.Message_Wrong = Error.Get_Error(40) + "\t" + Error.Get_Type_Error(2);
                                   throw new Exception();
                               }

                               if ((Cur_Exp.UL == Global.TypeSymbol.u_LENGTH || Cur_Exp.UL == Global.TypeSymbol.u_STRING_TO_INT) &&
                                   (Cur_Exp.prev.UL != Global.TypeSymbol.u_CST_STR))
                               {
                                   Global.Message_Wrong = Error.Get_Error(18) + "\t" + Error.Get_Type_Error(2);
                                   throw new Exception();
                               }

                               if (Cur_Exp.UL == Global.TypeSymbol.u_INT_TO_STRING && Cur_Exp.prev.UL != Global.TypeSymbol.u_CST_INT)
                               {
                                   Global.Message_Wrong = Error.Get_Error(50) + "\t" + Error.Get_Type_Error(2);
                                   throw new Exception();
                               }

                               if (Cur_Exp.UL == Global.TypeSymbol.u_NOT &&
                                  !(Cur_Exp.prev.UL == Global.TypeSymbol.u_TRUE || Cur_Exp.prev.UL == Global.TypeSymbol.u_FALSE))
                               {
                                   Global.Message_Wrong = Error.Get_Error(41) + "\t" + Error.Get_Type_Error(2);
                                   throw new Exception();
                               }

                               #endregion condition for type one parameter

                               #region Evalute function one parameter

                               if (Cur_Exp.UL == Global.TypeSymbol.u_SIN)//////////////////////
                               {
                                   Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                   Cur_Exp.prev.Val_NB = Math.Sin(Math.PI * Cur_Exp.prev.Val_NB / 180);
                               }
                               else
                                   if (Cur_Exp.UL == Global.TypeSymbol.u_COS)
                                   {
                                       Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                       Cur_Exp.prev.Val_NB = Math.Cos(Math.PI * Cur_Exp.prev.Val_NB / 180);
                                   }
                                   else
                                       if (Cur_Exp.UL == Global.TypeSymbol.u_TAN)//////////////
                                       {
                                           double RESULT_DIV = Cur_Exp.prev.Val_NB / 90;
                                           if ((RESULT_DIV % 2 == 1) || (RESULT_DIV % 2 == -1))
                                           {
                                               Global.Message_Wrong = Error.Get_Error(42) + "\t" + Error.Get_Type_Error(2);
                                               throw new Exception();
                                           }
                                           Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                           Cur_Exp.prev.Val_NB = Math.Tan(Math.PI * Cur_Exp.prev.Val_NB / 180);
                                       }
                                       else
                                           if (Cur_Exp.UL == Global.TypeSymbol.u_ATAN)
                                           {
                                               Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                               Cur_Exp.prev.Val_NB = Math.Atan(Cur_Exp.prev.Val_NB);
                                           }
                                           else
                                               if (Cur_Exp.UL == Global.TypeSymbol.u_LENGTH)
                                               {
                                                   Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_INT;
                                                   Cur_Exp.prev.Val_NB = (Cur_Exp.prev.Val_STR).Length;
                                               }
                                               else
                                                   if (Cur_Exp.UL == Global.TypeSymbol.u_NOT)
                                                   {
                                                       if (Cur_Exp.prev.UL == Global.TypeSymbol.u_TRUE)
                                                       {
                                                           Cur_Exp.prev.UL = Global.TypeSymbol.u_FALSE;
                                                       }
                                                       else
                                                       {
                                                           Cur_Exp.prev.UL = Global.TypeSymbol.u_TRUE;
                                                       }
                                                   }
                                                   else
                                                       if (Cur_Exp.UL == Global.TypeSymbol.u_UNARY_MINUS)
                                                       {
                                                           Cur_Exp.prev.Val_NB = -1 * (Cur_Exp.prev.Val_NB);
                                                       }
                                                       else
                                                           if (Cur_Exp.UL == Global.TypeSymbol.u_STRING_TO_INT)
                                                           {
                                                               Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_INT;
                                                               try
                                                               {
                                                                   Cur_Exp.prev.Val_NB = Convert.ToInt32(Cur_Exp.prev.Val_STR);
                                                               }
                                                               catch 
                                                               {
                                                                   Global.Message_Wrong = Error.Get_Error(55) + "\t" + Error.Get_Type_Error(2);
                                                                   throw new Exception();   
                                                               }
                                                           }
                                                           else
                                                               if (Cur_Exp.UL == Global.TypeSymbol.u_INT_TO_STRING)
                                                               {
                                                                   try
                                                                   {
                                                                       Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_STR;
                                                                       Cur_Exp.prev.Val_STR = (Cur_Exp.prev.Val_NB).ToString();
                                                                   }
                                                                   catch 
                                                                   {
                                                                       Global.Message_Wrong = Error.Get_Error(56) + "\t" + Error.Get_Type_Error(2);
                                                                       throw new Exception();
                                                                   }
                                                                   
                                                               }
                                                               else
                                                                   if (Cur_Exp.UL == Global.TypeSymbol.u_LOG)
                                                                   {
                                                                       Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                                                       Cur_Exp.prev.Val_NB = Math.Log10(Cur_Exp.prev.Val_NB);
                                                                   }
                                                                   else
                                                                       if (Cur_Exp.UL == Global.TypeSymbol.u_LN)
                                                                       {
                                                                           Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                                                           Cur_Exp.prev.Val_NB = Math.Log(Cur_Exp.prev.Val_NB);
                                                                       }
                                                                       else
                                                                           if (Cur_Exp.UL == Global.TypeSymbol.u_UNARY_PLUS)
                                                                           {
                                                                               Cur_Exp.prev.Val_NB = +1 * (Cur_Exp.prev.Val_NB);
                                                                           }
                                                                           else
                                                                           if (Cur_Exp.UL == Global.TypeSymbol.u_SQRT)
                                                                           {
                                                                               if (Cur_Exp.prev.Val_NB<0)
                                                                               {
                                                                                   Global.Message_Wrong = Error.Get_Error(53) + "\t" + Error.Get_Type_Error(2);
                                                                                   throw new Exception();
                                                                               }
                                                                               Cur_Exp.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                                                               Cur_Exp.prev.Val_NB = Math.Sqrt(Cur_Exp.prev.Val_NB);
                                                                           }
                                                                           else
                                                                               if (Cur_Exp.UL == Global.TypeSymbol.u_ABS)
                                                                               {
                                                                                   if (Cur_Exp.prev.Val_NB < 0)
                                                                                   {
                                                                                       Cur_Exp.prev.Val_NB = -1 * Cur_Exp.prev.Val_NB;
                                                                                   }
                                                                               }
                                                                               

                               #endregion Evalute function one parameter

                               if (Cur_Exp.next != null)
                               {
                                   Cur_Exp.next.prev = Cur_Exp.prev;
                               }

                               Cur_Exp.prev.next = Cur_Exp.next;

                               TEXP Aux = Cur_Exp.next;
                               TEXP.Free(Cur_Exp);
                               Cur_Exp = Aux;

                               #endregion Function with one parameter
                           }
                           else
                               if (Cur_Exp.UL == Global.TypeSymbol.u_AND || Cur_Exp.UL == Global.TypeSymbol.u_OR
                                  || Cur_Exp.UL == Global.TypeSymbol.u_PLUS || Cur_Exp.UL == Global.TypeSymbol.u_MINUS
                                  || Cur_Exp.UL == Global.TypeSymbol.u_MULTI || Cur_Exp.UL == Global.TypeSymbol.u_EQUAL
                                  || Cur_Exp.UL == Global.TypeSymbol.u_DIV || Cur_Exp.UL == Global.TypeSymbol.u_IDIV
                                  || Cur_Exp.UL == Global.TypeSymbol.u_GT || Cur_Exp.UL == Global.TypeSymbol.u_LT
                                  || Cur_Exp.UL == Global.TypeSymbol.u_NOT_EQUAL || Cur_Exp.UL == Global.TypeSymbol.u_GE
                                  || Cur_Exp.UL == Global.TypeSymbol.u_LE || Cur_Exp.UL == Global.TypeSymbol.u_MOD)
                               {
                                   #region Function with two parameter

                                   #region condition Function with two parameter

                                   if ((Cur_Exp.UL == Global.TypeSymbol.u_MINUS  || Cur_Exp.UL == Global.TypeSymbol.u_MULTI
                                       || Cur_Exp.UL == Global.TypeSymbol.u_DIV  || Cur_Exp.UL == Global.TypeSymbol.u_LE
                                       || Cur_Exp.UL == Global.TypeSymbol.u_IDIV || Cur_Exp.UL == Global.TypeSymbol.u_GT
                                       || Cur_Exp.UL == Global.TypeSymbol.u_LT   || Cur_Exp.UL == Global.TypeSymbol.u_MOD
                                       || Cur_Exp.UL == Global.TypeSymbol.u_GE)  &&
                                       !((Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_INT    || Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_REAL) &&
                                       (Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_CST_INT || Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_CST_REAL)))
                                   {
                                       Global.Message_Wrong = Error.Get_Error(40) + "\t" + Error.Get_Type_Error(2);
                                       throw new Exception();
                                   }

                                   if ((Cur_Exp.UL == Global.TypeSymbol.u_AND || Cur_Exp.UL == Global.TypeSymbol.u_OR) &&
                                       !((Cur_Exp.prev.UL == Global.TypeSymbol.u_FALSE || Cur_Exp.prev.UL == Global.TypeSymbol.u_TRUE)
                                       && (Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_FALSE || Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_TRUE)))
                                   {
                                       Global.Message_Wrong = Error.Get_Error(43) + "\t" + Error.Get_Type_Error(2);
                                       throw new Exception();
                                   }

                                   if ((Cur_Exp.UL == Global.TypeSymbol.u_EQUAL || Cur_Exp.UL == Global.TypeSymbol.u_NOT_EQUAL) &&
                                       !((((Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_INT || Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_REAL) &&
                                        (Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_CST_INT || Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_CST_REAL)) ||
                                        ((Cur_Exp.prev.UL == Global.TypeSymbol.u_TRUE || Cur_Exp.prev.UL == Global.TypeSymbol.u_FALSE) &&
                                        (Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_TRUE || Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_FALSE))) ||
                                        (Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_STR && Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_CST_STR)))
                                   {
                                       Global.Message_Wrong = Error.Get_Error(44) + "\t" + Error.Get_Type_Error(2);
                                       throw new Exception();
                                   }

                                   if (Cur_Exp.UL == Global.TypeSymbol.u_PLUS &&
                                      !((Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_STR && Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_CST_STR) ||
                                      ((Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_INT || Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_REAL) &&
                                      (Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_CST_INT || Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_CST_REAL))))
                                   {
                                       Global.Message_Wrong = Error.Get_Error(45) + "\t" + Error.Get_Type_Error(2);
                                       throw new Exception();
                                   }

                                   #endregion condition Function with two parameter

                                   #region excute Function with two parameter

                                   if (Cur_Exp.UL == Global.TypeSymbol.u_AND)
                                   {
                                       if (Cur_Exp.prev.UL == Global.TypeSymbol.u_FALSE || Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_FALSE)
                                       {
                                           Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;
                                       }
                                       else
                                           Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                   }
                                   else if (Cur_Exp.UL == Global.TypeSymbol.u_OR)
                                   {
                                       if (Cur_Exp.prev.UL == Global.TypeSymbol.u_TRUE || Cur_Exp.prev.prev.UL == Global.TypeSymbol.u_TRUE)
                                       {
                                           Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                       }
                                       else
                                           Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;
                                   }
                                   else
                                       if (Cur_Exp.UL == Global.TypeSymbol.u_PLUS)
                                       {
                                           if (Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_STR)
                                           {
                                               Cur_Exp.prev.prev.Val_STR = Cur_Exp.prev.prev.Val_STR + Cur_Exp.prev.Val_STR;
                                           }
                                           else
                                           {
                                               Cur_Exp.prev.prev.Val_NB = Cur_Exp.prev.prev.Val_NB + Cur_Exp.prev.Val_NB;
                                               if (Cur_Exp.prev.prev.UL != Cur_Exp.prev.UL)
                                               {
                                                   Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                               }
                                           }
                                       }
                                       else
                                           if (Cur_Exp.UL == Global.TypeSymbol.u_MINUS)
                                           {
                                               Cur_Exp.prev.prev.Val_NB = Cur_Exp.prev.prev.Val_NB - Cur_Exp.prev.Val_NB;
                                               if (Cur_Exp.prev.prev.UL != Cur_Exp.prev.UL)
                                               {
                                                   Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                               }
                                           }
                                           else
                                               if (Cur_Exp.UL == Global.TypeSymbol.u_MULTI)
                                               {
                                                   Cur_Exp.prev.prev.Val_NB = Cur_Exp.prev.prev.Val_NB * Cur_Exp.prev.Val_NB;
                                                   if (Cur_Exp.prev.prev.UL != Cur_Exp.prev.UL)
                                                   {
                                                       Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                                   }
                                               }
                                               else
                                                   if (Cur_Exp.UL == Global.TypeSymbol.u_DIV)
                                                   {
                                                       if (Cur_Exp.prev.Val_NB == 0)
                                                       {
                                                           Global.Message_Wrong = Error.Get_Error(46) + "\t" + Error.Get_Type_Error(2);
                                                           throw new Exception();
                                                       }

                                                       Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_CST_REAL;
                                                       Cur_Exp.prev.prev.Val_NB = Cur_Exp.prev.prev.Val_NB / Cur_Exp.prev.Val_NB;
                                                   }
                                                   else
                                                       if (Cur_Exp.UL == Global.TypeSymbol.u_IDIV)
                                                       {
                                                           if (Cur_Exp.prev.Val_NB == 0)
                                                           {
                                                               Global.Message_Wrong = Error.Get_Error(46) + "\t" + Error.Get_Type_Error(2);
                                                               throw new Exception();
                                                           }
                                                           Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_CST_INT;
                                                           Cur_Exp.prev.prev.Val_NB = Convert.ToInt32(Cur_Exp.prev.prev.Val_NB / Cur_Exp.prev.Val_NB);
                                                       }
                                                       else
                                                           if (Cur_Exp.UL == Global.TypeSymbol.u_MOD)
                                                           {
                                                               if (Cur_Exp.prev.Val_NB == 0)
                                                               {
                                                                   Global.Message_Wrong = Error.Get_Error(46) + "\t" + Error.Get_Type_Error(2);
                                                                   throw new Exception();
                                                               }
                                                               Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_CST_INT;
                                                               Cur_Exp.prev.prev.Val_NB = Convert.ToInt32(Cur_Exp.prev.prev.Val_NB % Cur_Exp.prev.Val_NB);
                                                           }
                                                           else
                                                               if (Cur_Exp.UL == Global.TypeSymbol.u_GT)
                                                               {
                                                                   if (Cur_Exp.prev.prev.Val_NB > Cur_Exp.prev.Val_NB)
                                                                   {
                                                                       Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                   }
                                                                   else
                                                                       Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;

                                                               }
                                                               else
                                                                   if (Cur_Exp.UL == Global.TypeSymbol.u_GE)
                                                                   {
                                                                       if (Cur_Exp.prev.prev.Val_NB >= Cur_Exp.prev.Val_NB)
                                                                       {
                                                                           Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                       }
                                                                       else
                                                                           Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;

                                                                   }
                                                                   else
                                                                       if (Cur_Exp.UL == Global.TypeSymbol.u_LT)
                                                                       {
                                                                           if (Cur_Exp.prev.prev.Val_NB < Cur_Exp.prev.Val_NB)
                                                                           {
                                                                               Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                           }
                                                                           else
                                                                               Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;

                                                                       }
                                                                       else
                                                                           if (Cur_Exp.UL == Global.TypeSymbol.u_LE)
                                                                           {
                                                                               if (Cur_Exp.prev.prev.Val_NB <= Cur_Exp.prev.Val_NB)
                                                                               {
                                                                                   Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                               }
                                                                               else
                                                                                   Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;
                                                                           }
                                                                           else
                                                                               if (Cur_Exp.UL == Global.TypeSymbol.u_EQUAL)
                                                                               {
                                                                                   if (Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_INT || Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_REAL)
                                                                                   {
                                                                                       if (Cur_Exp.prev.prev.Val_NB == Cur_Exp.prev.Val_NB)
                                                                                       {
                                                                                           Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                                       }
                                                                                       else
                                                                                           Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;
                                                                                   }
                                                                                   else
                                                                                       if (Cur_Exp.prev.UL == Global.TypeSymbol.u_TRUE || Cur_Exp.prev.UL == Global.TypeSymbol.u_FALSE)
                                                                                       {
                                                                                           if (Cur_Exp.prev.prev.UL == Cur_Exp.prev.UL)

                                                                                               Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                                           else
                                                                                               Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;
                                                                                       }
                                                                                       else
                                                                                       {
                                                                                           if (Cur_Exp.prev.Val_STR == Cur_Exp.prev.prev.Val_STR)
                                                                                           {
                                                                                               Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                                           }
                                                                                           else
                                                                                               Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;
                                                                                       }
                                                                               }
                                                                               else
                                                                                   if (Cur_Exp.UL == Global.TypeSymbol.u_NOT_EQUAL)
                                                                                   {
                                                                                       if (Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_INT || Cur_Exp.prev.UL == Global.TypeSymbol.u_CST_REAL)
                                                                                       {
                                                                                           if (Cur_Exp.prev.prev.Val_NB != Cur_Exp.prev.Val_NB)
                                                                                           {
                                                                                               Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                                           }
                                                                                           else
                                                                                               Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;
                                                                                       }
                                                                                       else
                                                                                           if (Cur_Exp.prev.UL == Global.TypeSymbol.u_FALSE || Cur_Exp.prev.UL == Global.TypeSymbol.u_TRUE)
                                                                                           {
                                                                                               if (Cur_Exp.prev.prev.UL != Cur_Exp.prev.UL)

                                                                                                   Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                                               else
                                                                                                   Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;
                                                                                           }
                                                                                           else
                                                                                           {
                                                                                               if (Cur_Exp.prev.Val_STR != Cur_Exp.prev.prev.Val_STR)
                                                                                               {
                                                                                                   Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_TRUE;
                                                                                               }
                                                                                               else Cur_Exp.prev.prev.UL = Global.TypeSymbol.u_FALSE;
                                                                                           }
                                                                                   }
                                   #endregion excute Function with two parameter

                                   TEXP Exp_Aux = Cur_Exp.prev.prev;
                                   if (Cur_Exp.next != null)
                                   {
                                       Cur_Exp.next.prev = Exp_Aux;
                                   }
                                   Exp_Aux.next = Cur_Exp.next;

                                   TEXP.Free(Cur_Exp.prev);
                                   TEXP.Free(Cur_Exp);

                                   Cur_Exp = Exp_Aux.next;

                                   #endregion
                               }
           }
                              
           return Exp0;
       }

        #endregion Evalute_Methods

        #region Assign_Methods

       public static void Assign_Var(TPVar A_Var, TPVar B_Var)
       {
           TPVar.Free(A_Var);
           A_Var.items = null;
           TItem Cur_Item = B_Var.items;
           TItem Last_Item = null;
           while (Cur_Item!=null)
           {
               TItem New_Item = new TItem();
               New_Item.next = null;
               New_Item.kind = Cur_Item.kind;
               New_Item.Val_NB = Cur_Item.Val_NB;
               New_Item.Val_STR = Cur_Item.Val_STR;
               if (A_Var.items == null)
               {
                   A_Var.items = New_Item;
               }
               else
               {
                   Last_Item.next = New_Item;
               }
               Last_Item = New_Item;
               Cur_Item = Cur_Item.next;
           }
       }

       public static void Assign(TPVar Var, int Index, TEXP exp)
       {
           int i;

           #region index!=0

           if (Index != 0)
           {
               i = 0;
               TItem Item_Aux = Var.items;
               TItem Last_Item = null;
               while (i < Index)
               {
                   if (Item_Aux == null)
                   {
                       TItem New_Item = new TItem();
                       New_Item.next = null;
                       New_Item.kind = Global.TypeSymbol.u_UNKNOWN;
                       if (Last_Item == null)
                       {
                           Var.items = New_Item;
                       }
                       else
                       {
                           Last_Item.next = New_Item;
                       }
                       Last_Item = New_Item;
                   }
                   else
                   {
                       Last_Item = Item_Aux;
                       Item_Aux = Item_Aux.next;
                   }
                   i = i + 1;
               }//end while

               Last_Item.kind = exp.UL;
               Last_Item.Val_STR = exp.Val_STR;
               Last_Item.Val_NB = exp.Val_NB;
               
               Last_Item.next = null;//////////////////////////////////////////////////////////////////////////

               //TEXP.Free(exp);
           }
           #endregion

           else
           {
               #region index==0

               TPVar.Free(Var);
               Var.items = null;
               TItem Last_Item = null;
               while (exp != null)
               {
                   TItem Item_Aux = new TItem();
                   Item_Aux.next = null;
                   Item_Aux.kind = exp.UL;
                   Item_Aux.Val_NB = exp.Val_NB;
                   Item_Aux.Val_STR = exp.Val_STR;
                   if (Var.items == null)
                   {
                       Var.items = Item_Aux;
                   }
                   else
                   {
                       Last_Item.next = Item_Aux;
                   }
                   Last_Item = Item_Aux;

                   TEXP Exp_Aux = exp;
                   exp = exp.next;
                 //  TEXP.Free(Exp_Aux);
               } 

               #endregion
           }
       }

       #endregion Assign_Methods

        
    }
}