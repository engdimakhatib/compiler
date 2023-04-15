using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TList_Var
    {
       public TPVar V;
       public TList_Var next;

       public static void Free(TList_Var List_Var_Free)
       {
          // TPVar.Free(List_Var_Free.V);
           List_Var_Free.V = null;
           List_Var_Free.next = null;
           List_Var_Free = null;
           GC.Collect();
       }
    }
}
