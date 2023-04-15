using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TPVar:TIdentif
    {
       public TItem items;

       public static void Free(TPVar Var_Free)
       {
           while(Var_Free.items!=null)
           {
               TItem Aux=Var_Free.items.next;
               TItem.Free(Var_Free.items);
               Var_Free.items = Aux;
           }
           GC.Collect();
       }
   }

}
