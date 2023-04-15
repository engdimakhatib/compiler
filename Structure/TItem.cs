using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TItem
    {
       public Global.TypeSymbol kind ;
       public Double Val_NB = 0;
       public string Val_STR = "";
       public TItem next;
       

       public static void Free(TItem Item_Free)
       {
           Item_Free.Val_NB = 0;
           Item_Free.Val_STR = null;
           Item_Free.kind = 0;
           Item_Free.next = null;
           Item_Free = null;
           GC.Collect();
       }
    }
}
