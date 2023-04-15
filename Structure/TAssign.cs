using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public  class TAssign
    {
       public TPVar V;
       public TEXP index;
       public TEXP exp;
       
       public static void Free(TAssign Assign_Free)
       {
           Free_Class.Free_GVAR(Assign_Free.V);
           Free_Class.Free_EXP(Assign_Free.exp);
           Free_Class.Free_EXP(Assign_Free.index);
           Assign_Free = null;
           GC.Collect();
       }
    }
}
