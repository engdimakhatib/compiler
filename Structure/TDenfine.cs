using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TDenfine:TIdentif
    {
       public Global.TypeSymbol kind;
       public String Val_STR = "";
       public Double Val_NB = 0;

       public static void Free(TDenfine Define_Free)
       {
           Define_Free.kind = 0;
           Define_Free.Val_NB = 0;
           Define_Free.Val_STR = null;
           Define_Free = null;
           GC.Collect();
       }
    }
}

