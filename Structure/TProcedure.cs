using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TProcedure:TIdentif
    {
       public TPVar P_In;
       public TPVar P_Out;
       public TPVar L_Var;
       public Boolean Is_Define;
       public Boolean Is_Function;
       public TInstruction List;
       public TProcedure()
       {
           Is_Define = false;
           Is_Function = false;
           P_In = null;
           P_Out = null;
           L_Var = null;
           List = null;
       }

       public static void Free(TProcedure Procedure_Free)
       {
           Procedure_Free.Is_Define = false;
           Procedure_Free.Is_Function = false;
           Procedure_Free = null;
           GC.Collect();
       }
    }
}
