using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TIdentif
    {
       public string name = "";
       public Global.TypeSymbol ul;
       public TIdentif next;

       public static void Free(TIdentif Tiden_Free)
       {
           Tiden_Free.name = null;
           Tiden_Free.ul = 0;
           Tiden_Free.next = null;
           Tiden_Free = null;
           GC.Collect();
       }
    }
}
