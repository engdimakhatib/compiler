using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TCall
    {
       public TProcedure F;
       public TEXP PIN;
       public TList_Var POUT;

       public static void Free(TCall Call_Free)
       {
           Call_Free.F = null;
           Call_Free = null;
           GC.Collect();
       }
    }
}
