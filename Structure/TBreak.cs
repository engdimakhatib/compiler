using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TBreak
    {
     public Global.TypeSymbol ul;

     public static void Free(TBreak Break_Free)
     {
         Break_Free.ul = 0;
         Break_Free = null;
         GC.Collect();
     }

    }
}
