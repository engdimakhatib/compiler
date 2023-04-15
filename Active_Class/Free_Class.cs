using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
  public  class Free_Class
    {
        public static void INTIAl_VARS()
        {
            TPVar Var_Aux = Global.G_Var;
            while (Var_Aux != null)
            {
                TPVar.Free(Var_Aux);
                Var_Aux.items = null;
                Var_Aux = (TPVar)Var_Aux.next;
                GC.Collect();
            }
            TProcedure Proc_Aux = Global.G_Var_Proc;
            while (Proc_Aux != null)
            {
                Var_Aux = Proc_Aux.L_Var;
                while (Var_Aux != null)
                {
                    TPVar.Free(Var_Aux);
                    Var_Aux.items = null;
                    Var_Aux = (TPVar)Var_Aux.next;
                    GC.Collect();
                }
                Proc_Aux = (TProcedure)Proc_Aux.next;
            }
        }

        public static void Free_ALL()
        {
            Free_Class.INTIAl_VARS();
            TInstruction.Free(Global.Main_INST);
            Global.Main_INST = null;
            Free_Class.Free_GVAR(Global.G_Var);
            Global.G_Var = null;
            while (Global.G_Var_Proc != null)
            {
                TProcedure Proc_Aux = (TProcedure)Global.G_Var_Proc.next;
                Free_Class.Free_GVAR(Global.G_Var_Proc.L_Var);//pin pout 
                TInstruction.Free(Global.G_Var_Proc.List);
                TProcedure.Free(Global.G_Var_Proc);
                Global.G_Var_Proc = Proc_Aux;
            }

            while (Global.G_Var_Define != null)//////////////////////////////////////////////////////
            {
                TDenfine Define_Aux = (TDenfine)Global.G_Var_Define.next;
                TDenfine.Free(Global.G_Var_Define);
                Global.G_Var_Define = Define_Aux;
            }

            while (Global.GSymbol != null)
            {
                TSymbol Symbol_Aux = Global.GSymbol.next;
                TSymbol.Free(Global.GSymbol);
                Global.GSymbol = Symbol_Aux;
            }

            while (Global.GFile!=null)
            {
                TTFile Aux = Global.GFile.next;
                TTFile.Free(Global.GFile);
                Global.GFile = Aux;
            }

        }

        public static void Free_GVAR(TPVar GV_Free)
        {
            while (GV_Free != null)
            {
                TPVar Var_Aux = (TPVar)GV_Free.next;
                TPVar.Free(GV_Free);
                GV_Free = Var_Aux;
            }
            GV_Free = null;
            GC.Collect();
        }

        public static void Free_EXP(TEXP EXP_Free)
        {
            while (EXP_Free != null)
            {
                TEXP Exp_Aux = EXP_Free.next;
                if (EXP_Free.UL == Global.TypeSymbol.u_CALL)//function
                {
                    Free_Class.Free_Call(EXP_Free.Val_Call);
                }
                EXP_Free.next = null;
                TEXP.Free(EXP_Free);
                EXP_Free = Exp_Aux;
            }
        }

        public static void Free_Call(TCall Call_Free)
        {
            Free_Class.Free_EXP(Call_Free.PIN);
            while (Call_Free.POUT != null)
            {
                TList_Var LV_Aux = Call_Free.POUT.next;
                TList_Var.Free(Call_Free.POUT);
                Call_Free.POUT = LV_Aux;
            }
            TCall.Free(Call_Free);
        }

    }
}
