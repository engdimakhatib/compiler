using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class Instruction_Class
    {
       public static TInstruction Read_List_Of_Instruction()
       {
           if (Global.UL != Global.TypeSymbol.u_BEGIN)
           {
               Global.Message_Wrong = Error.Get_Error(23) +"\t"+ Error.Get_Type_Error(3);
               throw new Exception();
           }
           Global.UL = Lexical.Lexical_Unit(); 
           TInstruction FST_INST = null;
           TInstruction LAST_INST = null;
           while (Global.UL != Global.TypeSymbol.u_END)
           {
               TInstruction NEW_INST = new TInstruction();
               NEW_INST.INS = Instruction_Class.Read_One_Instruction();
               NEW_INST.next = null;
               if (LAST_INST == null)
               {
                   FST_INST = NEW_INST;
               }
               else
               {
                   LAST_INST.next = NEW_INST;
               }
               LAST_INST = NEW_INST;
           }
           Global.UL = Lexical.Lexical_Unit();
           return FST_INST;
       }

       public static TInstruction Read_One_Or_List_Of_Instruction()
       {
           Global.UL = Lexical.Lexical_Unit();
           if (Global.UL == Global.TypeSymbol.u_BEGIN)
           {
               return Instruction_Class.Read_List_Of_Instruction();
           }
           TInstruction New_INST = new TInstruction();
           New_INST.INS = Instruction_Class.Read_One_Instruction();
           New_INST.next = null;
           return New_INST;
       }

       public static object Read_One_Instruction()
       {
           if (Global.UL == Global.TypeSymbol.u_VAR || Global.UL == Global.TypeSymbol.u_UNKNOWN)
           {
               #region ASSIGN
               TAssign Assign_Aux = new TAssign();
               if (Global.UL == Global.TypeSymbol.u_UNKNOWN)
               {
                   TIdentif temp = Global.G_Var;
                   Global.Add_IDentif(Global.G_Cur_STR, ref temp, Global.TypeSymbol.u_VAR);
                   Global.G_Var = (TPVar)temp;
                   Global.G_Cur_Id = Global.G_Var;
               }
               Assign_Aux.V = (TPVar)Global.G_Cur_Id;
               Global.UL = Lexical.Lexical_Unit();
               Assign_Aux.index = null;
               if (Global.UL == Global.TypeSymbol.u_OPENB)
               {
                   Global.UL = Lexical.Lexical_Unit();
                   TEXP last = null;
                   Assign_Aux.index = Instruction_Class.Read_Expression(ref last);
                   if (Global.UL != Global.TypeSymbol.u_CLOSEB)
                   {
                       Global.Message_Wrong = Error.Get_Error(25) + "\t" + Error.Get_Type_Error(3);
                       throw new Exception();
                   }
                   Global.UL = Lexical.Lexical_Unit();
               }//end index
               if (Global.UL != Global.TypeSymbol.u_ASSIGN)
               {
                   Global.Message_Wrong = Error.Get_Error(26) + "\t" + Error.Get_Type_Error(3);
                   throw new Exception();
               }
               Global.UL = Lexical.Lexical_Unit();
               TEXP last1 = null;
               Assign_Aux.exp = Instruction_Class.Read_Expression(ref last1);
               if (Global.UL != Global.TypeSymbol.u_SEMICOLON)
               {
                   Global.Message_Wrong = Error.Get_Error(22) + "\t" + Error.Get_Type_Error(3);
                   throw new Exception();
               }
               Global.UL = Lexical.Lexical_Unit();
               return Assign_Aux;

               #endregion
           }
           else
               if (Global.UL == Global.TypeSymbol.u_IF)
               {
                   #region IF

                   TIF If_Aux = new TIF();
                   Global.UL = Lexical.Lexical_Unit();
                   TEXP last = null;
                   If_Aux.cond = Instruction_Class.Read_Condition(ref last);
                   if (Global.UL != Global.TypeSymbol.u_THEN)
                   {
                       Global.Message_Wrong = Error.Get_Error(27) + "\t" + Error.Get_Type_Error(3);
                       throw new Exception();
                   }

                   If_Aux.INS_IF = Instruction_Class.Read_One_Or_List_Of_Instruction();
                   If_Aux.INS_ELSE = null;
                   if (Global.UL == Global.TypeSymbol.u_ELSE)
                   {
                       If_Aux.INS_ELSE = Instruction_Class.Read_One_Or_List_Of_Instruction();
                   }
                   return If_Aux;
                   #endregion
               }
               else
                   if (Global.UL == Global.TypeSymbol.u_WHILE)
                   {
                       #region  WHILE
                       TWHILE While_Aux = new TWHILE();
                       Global.UL = Lexical.Lexical_Unit();
                       TEXP last = null;
                       While_Aux.cond = Instruction_Class.Read_Condition(ref last);
                       if (Global.UL != Global.TypeSymbol.u_DO)
                       {
                           Global.Message_Wrong = Error.Get_Error(28) + "\t" + Error.Get_Type_Error(3);
                           throw new Exception();
                       }
                       While_Aux.L_inst = Instruction_Class.Read_One_Or_List_Of_Instruction();
                       return While_Aux;
                       #endregion
                   }

                   else
                       if (Global.UL == Global.TypeSymbol.u_FOR)
                       {
                           #region  FOR
                           TFOR For_Aux = new TFOR();
                           Global.UL = Lexical.Lexical_Unit();
                           if (Global.UL == Global.TypeSymbol.u_UNKNOWN)
                           {
                               TIdentif temp = Global.G_Var;
                               Global.Add_IDentif(Global.G_Cur_STR, ref temp, Global.TypeSymbol.u_VAR);
                               Global.G_Var = (TPVar)temp;
                               Global.G_Cur_Id = Global.G_Var;
                           }
                           else
                           {
                               if (Global.UL != Global.TypeSymbol.u_VAR)
                               {
                                   Global.Message_Wrong = Error.Get_Error(12) + "\t" + Error.Get_Type_Error(3);
                                   throw new Exception();
                               }
                           }
                           For_Aux.V = (TPVar)Global.G_Cur_Id;
                           if (Lexical.Lexical_Unit() != Global.TypeSymbol.u_ASSIGN)
                           {
                               Global.Message_Wrong = Error.Get_Error(26) + "\t" + Error.Get_Type_Error(3);
                               throw new Exception();
                           }
                           Global.UL = Lexical.Lexical_Unit();
                           TEXP last = null;
                           For_Aux.Exp_Begin = Instruction_Class.Read_Expression(ref last);
                           if (Global.UL != Global.TypeSymbol.u_TO && Global.UL != Global.TypeSymbol.u_DOWN_TO)
                           {
                               Global.Message_Wrong = Error.Get_Error(29) + "\t" + Error.Get_Type_Error(3);
                               throw new Exception();
                           }
                           For_Aux.Is_Down = (Global.UL == Global.TypeSymbol.u_DOWN_TO);
                           Global.UL = Lexical.Lexical_Unit();
                           TEXP last1 = null;
                           For_Aux.Exp_End = Instruction_Class.Read_Expression(ref last1);
                           if (Global.UL != Global.TypeSymbol.u_DO)
                           {
                               Global.Message_Wrong = Error.Get_Error(28) + "\t" + Error.Get_Type_Error(3);
                               throw new Exception();
                           }
                           For_Aux.L_inst = Instruction_Class.Read_One_Or_List_Of_Instruction();
                           return For_Aux;
                           #endregion
                       }
                       else
                           if (Global.UL == Global.TypeSymbol.u_CALL)
                           {
                               #region Call 
                               //reading procedure and not function
                               Global.UL = Lexical.Lexical_Unit();
                               if (Global.UL != Global.TypeSymbol.u_VAR_PROC && Global.UL != Global.TypeSymbol.u_UNKNOWN)
                               {
                                   Global.Message_Wrong = Error.Get_Error(32) + "\t" + Error.Get_Type_Error(3);
                                   throw new Exception();
                               }
                               if (Global.UL == Global.TypeSymbol.u_UNKNOWN)
                               {
                                   TIdentif temp = Global.G_Var_Proc;
                                   Global.Add_IDentif(Global.G_Cur_STR, ref temp, Global.TypeSymbol.u_VAR_PROC);
                                   Global.G_Var_Proc = (TProcedure)temp;
                                   Global.G_Var_Proc.Is_Function = false;
                                   Global.G_Cur_Id = Global.G_Var_Proc;
                               }
                               if (((TProcedure)Global.G_Cur_Id).Is_Function)
                               {
                                   Global.Message_Wrong = Error.Get_Error(30) + "\t" + Error.Get_Type_Error(3);
                                   throw new Exception();
                               }
                               TProcedure Proc_Aux = (TProcedure)Global.G_Cur_Id;
                               Global.UL = Lexical.Lexical_Unit();
                               TCall Call_Aux = Instruction_Class.Read_Call(Proc_Aux);
                               if (Global.UL != Global.TypeSymbol.u_SEMICOLON)
                               {
                                   Global.Message_Wrong = Error.Get_Error(22) + "\t" + Error.Get_Type_Error(3);
                                   throw new Exception();
                               }
                               Global.UL = Lexical.Lexical_Unit();

                               return Call_Aux;

                               #endregion call
                           }
                           else
                               if (Global.UL == Global.TypeSymbol.u_WRITE || Global.UL == Global.TypeSymbol.u_WRITELN)
                               {
                                   #region WRITE
                                   TWRITE Write_Aux = new TWRITE();
                                   Write_Aux.Is_ln = (Global.UL == Global.TypeSymbol.u_WRITELN);
                                   Global.UL = Lexical.Lexical_Unit();
                                   if (Global.UL!=Global.TypeSymbol.u_OPENP)
                                   {
                                       Global.Message_Wrong = Error.Get_Error(19) + "\t" + Error.Get_Type_Error(3);
                                       throw new Exception();
                                   }
                                   Global.UL = Lexical.Lexical_Unit();
                                   Write_Aux.exp = null;
                                   TEXP Last=null;
                                   TEXP Last1 = new TEXP();
                                   while (Global.UL != Global.TypeSymbol.u_CLOSEP)
                                   {
                                       TEXP Exp_New = Instruction_Class.Read_Expression(ref Last);
                                       if (Write_Aux.exp == null)
                                       {
                                           Write_Aux.exp = Exp_New;
                                       }
                                       else
                                       {
                                           Last1.next = Exp_New;
                                           Exp_New.prev = Last1;
                                       }
                                       Last1 = Last;
                                       if(Global.UL!=Global.TypeSymbol.u_COMMA&&Global.UL!=Global.TypeSymbol.u_CLOSEP)
                                       {
                                           Global.Message_Wrong = Error.Get_Error(31) + "or" + Error.Get_Error(21) + "\t" + Error.Get_Type_Error(3);
                                           throw new Exception();
                                       }
                                       if (Global.UL == Global.TypeSymbol.u_COMMA)
                                       {
                                           Global.UL = Lexical.Lexical_Unit();
                                           if (Global.UL==Global.TypeSymbol.u_CLOSEP)
                                           {
                                               Global.Message_Wrong = Error.Get_Error(18) + "\t" + Error.Get_Type_Error(3);
                                               throw new Exception();
                                           }
                                       }
                                   }//end while
                                   if(!Write_Aux.Is_ln && Write_Aux.exp==null)
                                   {
                                       Global.Message_Wrong ="WARNING    "+ Error.Get_Error(19) + "\t" + Error.Get_Type_Error(3);
                                       throw new Exception();
                                   }
                                   if (Lexical.Lexical_Unit()!=Global.TypeSymbol.u_SEMICOLON)
                                   {
                                       Global.Message_Wrong = Error.Get_Error(22) +"\t" + Error.Get_Type_Error(3);
                                       throw new Exception();
                                   }
                                   Global.UL = Lexical.Lexical_Unit();
                                   return Write_Aux;
                                   #endregion
                               }
                               else
                                   if (Global.UL == Global.TypeSymbol.u_READ)
                                   {
                                       #region READ

                                       TRead Read_Aux = new TRead();
                                       Global.UL = Lexical.Lexical_Unit();
                                       if (Global.UL != Global.TypeSymbol.u_OPENP)
                                       {
                                           Global.Message_Wrong = Error.Get_Error(19) + "\t" + Error.Get_Type_Error(3);
                                           throw new Exception();
                                       }
                                       Global.UL = Lexical.Lexical_Unit();
                                       if (Global.UL == Global.TypeSymbol.u_UNKNOWN)
                                       {
                                           TIdentif temp = Global.G_Var;
                                           Global.Add_IDentif(Global.G_Cur_STR, ref temp, Global.TypeSymbol.u_VAR);
                                           Global.G_Var =( TPVar)temp;
                                           Read_Aux.v = Global.G_Var;
                                       }
                                       else
                                           if (Global.UL == Global.TypeSymbol.u_VAR)
                                           {
                                               Read_Aux.v = (TPVar)Global.G_Cur_Id;
                                           }
                                           else
                                           {
                                               Global.Message_Wrong = Error.Get_Error(12) + "\t" + Error.Get_Type_Error(3);
                                               throw new Exception();
                                           }
                                       Global.UL = Lexical.Lexical_Unit();
                                       if (Global.UL==Global.TypeSymbol.u_OPENB)
                                       {
                                           Global.UL = Lexical.Lexical_Unit();
                                           TEXP last=null;
                                           Read_Aux.Index = Instruction_Class.Read_Expression(ref last);
                                           if (Global.UL!=Global.TypeSymbol.u_CLOSEB)
                                           {
                                               Global.Message_Wrong = Error.Get_Error(25) + "\t" + Error.Get_Type_Error(3);
                                               throw new Exception();
                                           }
                                           Global.UL = Lexical.Lexical_Unit();
                                       }
                                       if (Global.UL!=Global.TypeSymbol.u_CLOSEP)
                                       {
                                           Global.Message_Wrong = Error.Get_Error(21) + "\t" + Error.Get_Type_Error(3);
                                           throw new Exception();
                                       }
                                       Global.UL = Lexical.Lexical_Unit();
                                       if (Global.UL!=Global.TypeSymbol.u_SEMICOLON)
                                       {
                                           Global.Message_Wrong = Error.Get_Error(22) + "\t" + Error.Get_Type_Error(3);
                                           throw new Exception();
                                       }
                                       Global.UL = Lexical.Lexical_Unit();
                                       return Read_Aux;
                                       #endregion
                                   }
                                   else
                                       if (Global.UL == Global.TypeSymbol.u_RETURN)
                                       {
                                           #region RETURN
                                           TReturn Return_Aux = new TReturn();
                                           Global.UL = Lexical.Lexical_Unit();
                                           TEXP last=null;
                                           Return_Aux.exp = Instruction_Class.Read_Expression(ref last);
                                           if (Global.UL!=Global.TypeSymbol.u_SEMICOLON)
                                           {
                                               Global.Message_Wrong = Error.Get_Error(22) + "\t" + Error.Get_Type_Error(3);
                                               throw new Exception();
                                           }
                                           Global.UL = Lexical.Lexical_Unit();
                                           return Return_Aux;
                                           #endregion
                                       }
                                       else
                                           if (Global.UL == Global.TypeSymbol.u_BREAK || Global.UL == Global.TypeSymbol.u_HALT
                                               || Global.UL == Global.TypeSymbol.u_EXIT)
                                           {
                                               #region break halt exit
                                               TBreak Break_Aux = new TBreak();
                                               Break_Aux.ul = Global.UL;
                                               Global.UL = Lexical.Lexical_Unit();
                                               if (Global.UL!=Global.TypeSymbol.u_SEMICOLON)
                                               {
                                                   Global.Message_Wrong = Error.Get_Error(22) + "\t" + Error.Get_Type_Error(3);
                                                   throw new Exception();
                                               }
                                               Global.UL = Lexical.Lexical_Unit();
                                               return Break_Aux;
                                               #endregion
                                           }
                                           else
                                           {
                                               Global.Message_Wrong = Error.Get_Error(33) + "\t" + Error.Get_Type_Error(3);
                                               throw new Exception();
                                           }
                          
       }

       public static TCall Read_Call(TProcedure Procedure)
       {
           TCall Call_Aux = new TCall();
           Call_Aux.F = Procedure;
           if (Global.UL != Global.TypeSymbol.u_OPENP)
           {
               Global.Message_Wrong = Error.Get_Error(19) + "\t" + Error.Get_Type_Error(3);
               throw new Exception();
           }
           Global.UL = Lexical.Lexical_Unit();
           Call_Aux.PIN = null;
           Call_Aux.POUT = null;
           TEXP Last1 = null;
           TEXP last = null;
           if (Global.UL == Global.TypeSymbol.u_INPUT)
           {
                                  
               #region input
               Global.UL = Lexical.Lexical_Unit();
               while (true)
               {

                   TEXP exp1 = Instruction_Class.Read_Expression(ref Last1);
                   if (exp1 == null)////
                   {
                       Global.Message_Wrong = Error.Get_Error(18) + "\t" + Error.Get_Type_Error(3);
                       throw new Exception();
                   }
                   if (Call_Aux.PIN == null)//أول دخول
                   {
                       Call_Aux.PIN = exp1;
                   }
                   else
                   {
                       last.next = exp1;
                       exp1.prev = last;
                   }
                   last = Last1;
                   if (Global.UL == Global.TypeSymbol.u_OUTPUT || Global.UL == Global.TypeSymbol.u_CLOSEP)
                   {
                       break;
                   }
                   if (Global.UL != Global.TypeSymbol.u_COMMA)
                   {
                       Global.Message_Wrong = Error.Get_Error(31) + "\t" + Error.Get_Type_Error(3);
                       throw new Exception();
                   }
                   Global.UL = Lexical.Lexical_Unit();
               }//end while true
               #endregion
           }//end input
           if (Global.UL == Global.TypeSymbol.u_OUTPUT)
           {
               #region output
               Global.UL = Lexical.Lexical_Unit();
               while (true)
               {
                   if (Global.UL == Global.TypeSymbol.u_UNKNOWN)
                   {
                       TIdentif temp = Global.G_Var;
                       Global.Add_IDentif(Global.G_Cur_STR, ref temp, Global.TypeSymbol.u_VAR);
                       Global.G_Var = (TPVar)temp;
                       Global.G_Cur_Id = Global.G_Var;
                   }
                   else if (Global.UL != Global.TypeSymbol.u_VAR)
                   {
                       Global.Message_Wrong = Error.Get_Error(12) + "\t" + Error.Get_Type_Error(3);
                       throw new Exception();
                   }
                   TList_Var NewLVar = new TList_Var();
                   NewLVar.V = (TPVar)Global.G_Cur_Id;
                   NewLVar.next = Call_Aux.POUT;
                   Call_Aux.POUT = NewLVar;
                   Global.UL = Lexical.Lexical_Unit();
                   if (Global.UL == Global.TypeSymbol.u_CLOSEP)
                   {
                       break;
                   }
                   else if (Global.UL != Global.TypeSymbol.u_COMMA)
                   {
                       Global.Message_Wrong = Error.Get_Error(31) + "\t" + Error.Get_Type_Error(3);//////
                       throw new Exception();
                   }
                   Global.UL = Lexical.Lexical_Unit();
               }//end while true output
               #endregion
           }
           if (Global.UL != Global.TypeSymbol.u_CLOSEP)
           {
               Global.Message_Wrong = Error.Get_Error(21) + "\t" + Error.Get_Type_Error(3);
               throw new Exception();
           }
           //if (Lexical.Lexical_Unit() != Global.TypeSymbol.u_SEMICOLON)
           //{
           //    Global.Message_Wrong = Error.Get_Error(22) + "\t" + Error.Get_Type_Error(3);
           //    throw new Exception();
           //}
           Global.UL = Lexical.Lexical_Unit();
           return Call_Aux;
       }

       public static TEXP Read_Condition(ref TEXP Last)
       {
           TEXP last1=null;
           TEXP Exp0 = Instruction_Class.Read_CTerm(ref Last);
           while (Global.UL == Global.TypeSymbol.u_OR)
           {
               TEXP Exp_New = new TEXP();
               Exp_New.UL = Global.UL;
               Global.UL = Lexical.Lexical_Unit();
               TEXP Exp1 = Instruction_Class.Read_CTerm(ref last1);
               Last.next = Exp1;
               Exp1.prev = Last;
               last1.next = Exp_New;
               Exp_New.prev = last1;
               Exp_New.next = null;
               Last = Exp_New;
           }
           return Exp0;
       }

       public static TEXP Read_CTerm(ref TEXP Last)
       {
           TEXP last1 = null;
           TEXP Exp0 = Instruction_Class.Read_CFactor(ref Last);
           while (Global.UL == Global.TypeSymbol.u_AND)
           {
               TEXP Exp_New = new TEXP();
               Exp_New.UL = Global.UL;
               Global.UL = Lexical.Lexical_Unit();
               TEXP Exp1 = Instruction_Class.Read_CFactor(ref last1);
               Last.next = Exp1;
               Exp1.prev = Last;
               last1.next = Exp_New;
               Exp_New.prev = last1;
               Exp_New.next = null;
               Last = Exp_New;
           }
           return Exp0;
       }

       public static TEXP Read_CFactor(ref TEXP Last)
       {
           TEXP last1 = null;
           TEXP Exp0 = Instruction_Class.Read_Expression(ref Last);
           while (Global.UL == Global.TypeSymbol.u_GE || Global.UL == Global.TypeSymbol.u_GT || Global.UL == Global.TypeSymbol.u_LE ||
               Global.UL == Global.TypeSymbol.u_LT || Global.UL == Global.TypeSymbol.u_EQUAL || Global.UL == Global.TypeSymbol.u_NOT_EQUAL)
           {
               TEXP Exp_New = new TEXP();
               Exp_New.UL = Global.UL;
               Global.UL = Lexical.Lexical_Unit();
               TEXP Exp1 = Instruction_Class.Read_Expression(ref last1);
               Last.next = Exp1;
               Exp1.prev = Last;
               last1.next = Exp_New;
               Exp_New.prev = last1;
               Exp_New.next = null;
               Last = Exp_New;
           }
           return Exp0;
       }

       public static TEXP Read_Expression(ref TEXP Last)
       {
           TEXP Last1=null;
           TEXP Exp0 = Instruction_Class.Read_Term(ref Last);
           while (Global.UL == Global.TypeSymbol.u_MINUS || Global.UL == Global.TypeSymbol.u_PLUS)
           {
               TEXP Exp_New = new TEXP();
               Exp_New.UL = Global.UL;
               Global.UL = Lexical.Lexical_Unit();
               TEXP Exp1 = Instruction_Class.Read_Term(ref Last1);
               Last.next = Exp1;
               Exp1.prev = Last;
               Last1.next = Exp_New;
               Exp_New.prev = Last1;
               Exp_New.next = null;
               Last = Exp_New;
           }
           return Exp0;
       }

       public static TEXP Read_Term(ref TEXP Last)
       {
           TEXP Last1 = null;
           TEXP Exp0 = Instruction_Class.Read_Factor(ref Last);
           while (Global.UL == Global.TypeSymbol.u_MULTI|| Global.UL == Global.TypeSymbol.u_DIV||
                  Global.UL == Global.TypeSymbol.u_MOD|| Global.UL == Global.TypeSymbol.u_IDIV)
           {
               TEXP Exp_New = new TEXP();
               Exp_New.UL = Global.UL;
               Global.UL = Lexical.Lexical_Unit();
               TEXP Exp1 = Instruction_Class.Read_Factor(ref Last1);
               Last.next = Exp1;
               Exp1.prev = Last;
               Last1.next = Exp_New;
               Exp_New.prev = Last1;
               Exp_New.next = null;
               Last = Exp_New;
           }
           return Exp0;
       }

       public static TEXP Read_Factor(ref TEXP Last)
       {
           TEXP Last1 = null;
           TEXP Exp0 = Instruction_Class.Read_Fact(ref Last);
           TEXP Last_Aux = Last;
           while (Global.UL == Global.TypeSymbol.u_POWER_SIGHN)
           {
               TEXP Exp_New = new TEXP();
               Exp_New.UL = Global.UL;
               Global.UL = Lexical.Lexical_Unit();
               TEXP Exp1 = Instruction_Class.Read_Fact(ref Last1);
               Exp_New.next=Last_Aux.next;
               if (Last_Aux.next != null)
               {
                   Last_Aux.next.prev = Exp_New;
               }
               else
               {
                   Last = Exp_New; //أول دخول
               }
               Last_Aux.next = Exp1;
               Exp1.prev = Last_Aux;
               Last1.next = Exp_New;
               Exp_New.prev = Last1;
               Last_Aux = Last1;
           }
           return Exp0;
       }
      
       public static TEXP Read_Fact(ref TEXP Last)
       {
           TEXP exp;
           if (Global.UL == Global.TypeSymbol.u_CST_STR || Global.UL == Global.TypeSymbol.u_CST_REAL ||
               Global.UL == Global.TypeSymbol.u_CST_INT || Global.UL == Global.TypeSymbol.u_TRUE || Global.UL == Global.TypeSymbol.u_FALSE)
           {
               #region Constant (int str real true false)

               exp = new TEXP();
               exp.UL = Global.UL;
               if (Global.UL == Global.TypeSymbol.u_CST_STR)
               {
                   exp.Val_STR = Global.G_Cur_STR;
               }
               else 
                   if ( Global.UL == Global.TypeSymbol.u_CST_REAL ||
                        Global.UL == Global.TypeSymbol.u_CST_INT)
               { 
                   exp.Val_NB = Global.G_Cur_NB; 
               }
               exp.next = null;
               exp.prev = null;
               Last = exp;
               Global.UL = Lexical.Lexical_Unit();
               return exp;

               #endregion Constant (int str real true false)
           }
           else
           {
               if (Global.UL == Global.TypeSymbol.u_OPENP)
               {
                   #region (exp)
                   Global.UL = Lexical.Lexical_Unit();
                   exp = Instruction_Class.Read_Condition(ref Last);
                   if (Global.UL != Global.TypeSymbol.u_CLOSEP)
                   {
                       Global.Message_Wrong = Error.Get_Error(21) + "\t" + Error.Get_Type_Error(3);
                       throw new Exception();
                   }
                   Global.UL = Lexical.Lexical_Unit();
                   #endregion
                   return exp;
               }
               else 
               {
                   if (Global.UL == Global.TypeSymbol.u_VAR || Global.UL == Global.TypeSymbol.u_UNKNOWN ||
                       Global.UL == Global.TypeSymbol.u_VAR_DEFINE || Global.UL == Global.TypeSymbol.u_VAR_PROC)
                   {
                       #region Variable function

                       exp = new TEXP();
                       exp.next = null;
                       exp.prev = null;
                       if (Global.UL == Global.TypeSymbol.u_VAR_DEFINE)
                       {
                           TDenfine Def_Aux = (TDenfine)Global.G_Cur_Id;
                           exp.UL = Def_Aux.kind;
                           exp.Val_STR = Def_Aux.Val_STR;
                           exp.Val_NB = Def_Aux.Val_NB;
                           Global.UL = Lexical.Lexical_Unit();
                           Last = exp;//نهاية التعبير يجب اسنادها للأخير
                           return exp;
                       }
                       else
                       {
                           #region unknown(var,function)

                           Global.TypeSymbol UL_AUX;
                           TPVar VAR_AUX = null;
                           TProcedure PROC_Aux = null;
                           if (Global.UL == Global.TypeSymbol.u_UNKNOWN)
                           {
                               string buffer = Global.G_Cur_STR;
                               Global.UL = Lexical.Lexical_Unit();
                               if (Global.UL == Global.TypeSymbol.u_OPENP)
                               {
                                   TIdentif temp = Global.G_Var_Proc;
                                   Global.Add_IDentif(buffer, ref temp, Global.TypeSymbol.u_VAR_PROC);
                                   Global.G_Var_Proc = (TProcedure)temp;
                                   PROC_Aux = Global.G_Var_Proc;
                                   Global.G_Var_Proc.Is_Function = true;
                                   UL_AUX = Global.TypeSymbol.u_VAR_PROC;
                               }
                               else 
                               {
                                   TIdentif temp = Global.G_Var;
                                   Global.Add_IDentif(buffer, ref temp, Global.TypeSymbol.u_VAR);
                                   Global.G_Var = (TPVar)temp;
                                   VAR_AUX = Global.G_Var;
                                   UL_AUX = Global.TypeSymbol.u_VAR;
                               }

                           }
                           else
                           {
                               UL_AUX = Global.UL;
                               if (Global.UL == Global.TypeSymbol.u_VAR)
                               {
                                   VAR_AUX = (TPVar)Global.G_Cur_Id;
                               }
                               else
                               {
                                   PROC_Aux = (TProcedure)Global.G_Cur_Id;
                               }
                               Global.UL = Lexical.Lexical_Unit();
                           }
                           exp.UL = UL_AUX;
                           if (UL_AUX == Global.TypeSymbol.u_VAR_PROC)
                           {
                               if (!PROC_Aux.Is_Function)
                               {
                                   Global.Message_Wrong = Error.Get_Error(34) + "\t" + Error.Get_Type_Error(3);
                                   throw new Exception();
                               }
                               exp.Val_Call = Instruction_Class.Read_Call(PROC_Aux);
                               //reading function and not procedure
                           }
                           else
                           {
                               exp.Val_Var = VAR_AUX;
                               if (Global.UL == Global.TypeSymbol.u_OPENB)
                               {
                                   Global.UL = Lexical.Lexical_Unit();
                                   TEXP last = null;
                                   exp.index = Instruction_Class.Read_Expression(ref last);
                                   if (Global.UL != Global.TypeSymbol.u_CLOSEB)
                                   {
                                       Global.Message_Wrong = Error.Get_Error(25) + "\t" + Error.Get_Type_Error(3);
                                       throw new Exception();
                                   }
                                   Global.UL = Lexical.Lexical_Unit();
                               }
                               else
                               {
                                   exp.index = null;
                               }
                           }

                           #endregion unknown(var,function)

                           Last = exp;//نهاية التعبير يجب اسنادها للأخير
                           return exp;
                       }

                       #endregion Variable function
                   }
                   else
                   {
                       #region (MATH FUNCTION)
                       if (Global.UL == Global.TypeSymbol.u_POWER || Global.UL == Global.TypeSymbol.u_SIN || Global.UL == Global.TypeSymbol.u_COS ||
                           Global.UL == Global.TypeSymbol.u_LN || Global.UL == Global.TypeSymbol.u_LOG || Global.UL == Global.TypeSymbol.u_ATAN ||
                           Global.UL == Global.TypeSymbol.u_SQRT || Global.UL == Global.TypeSymbol.u_TAN||Global.UL==Global.TypeSymbol.u_LENGTH
                           || Global.UL == Global.TypeSymbol.u_STRING_TO_INT || Global.UL == Global.TypeSymbol.u_INT_TO_STRING 
                           ||Global.UL==Global.TypeSymbol.u_ABS)
                       {
                           TEXP exp_NEW = new TEXP();
                           exp_NEW.UL = Global.UL;
                           exp_NEW.next = null;
                           if (Lexical.Lexical_Unit()!=Global.TypeSymbol.u_OPENP)
                           {
                                Global.Message_Wrong = Error.Get_Error(19) + "\t" + Error.Get_Type_Error(3);
                                throw new Exception();
                           }
                           Global.UL = Lexical.Lexical_Unit();
                           exp = Instruction_Class.Read_Expression(ref Last);
                           if (Global.UL!=Global.TypeSymbol.u_CLOSEP)
                           {
                               Global.Message_Wrong = Error.Get_Error(21) + "\t" + Error.Get_Type_Error(3);
                                throw new Exception();
                           }
                           Last.next = exp_NEW;
                           exp_NEW.prev = Last;
                           //**********************************
                           Last = exp_NEW;//نهاية التعبير يجب اسنادها للأخير
                           //*******************************
                           Global.UL = Lexical.Lexical_Unit();
                           return exp;
                       #endregion

                       }
                       else 
                       {
                           if (Global.UL == Global.TypeSymbol.u_MINUS || Global.UL == Global.TypeSymbol.u_PLUS || 
                               Global.UL == Global.TypeSymbol.u_NOT)
                           {
                               TEXP exp_NEW = new TEXP();
                               exp_NEW.next = null;
                               if (Global.UL == Global.TypeSymbol.u_PLUS)
                               {
                                   exp_NEW.UL = Global.TypeSymbol.u_UNARY_PLUS;
                               }
                               else
                                   if (Global.UL == Global.TypeSymbol.u_MINUS)
                                   {
                                       exp_NEW.UL = Global.TypeSymbol.u_UNARY_MINUS;
                                   }
                                   else
                                       exp_NEW.UL = Global.UL;
                               Global.UL = Lexical.Lexical_Unit();
                               exp = Instruction_Class.Read_Fact(ref Last);
                               Last.next = exp_NEW;
                               exp_NEW.prev = Last;
                               //**********************************
                               Last = exp_NEW;//نهاية التعبير يجب اسنادها للأخير
                               //*******************************
                               return exp;
                           }
                           else
                           {
                               Global.Message_Wrong = Error.Get_Error(34) + "\t" + Error.Get_Type_Error(3);
                               throw new Exception();
                           }
                       }
                   }
               }
           }
           
       }

    }
}