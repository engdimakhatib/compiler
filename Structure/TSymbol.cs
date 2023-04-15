using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
  public  class TSymbol
    {
      public string name;
      public Global.TypeSymbol ul;
      public TSymbol next;

      public static void Free(TSymbol Symbol_Free)
      {
          Symbol_Free.name = null;
          Symbol_Free.ul = 0;
          Symbol_Free.next = null;
          Symbol_Free = null;
          GC.Collect();
      }
    }
}
