using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
  public class TEXP
    {
      public Global.TypeSymbol UL;
      public string Val_STR;
      public double Val_NB;
      public TPVar Val_Var;
      public TCall Val_Call;
      public TEXP next;
      public TEXP prev;
      public TEXP index = null;
      public static void Free(TEXP Exp_Free)
      {
          Exp_Free.UL = 0;
          Exp_Free.Val_STR = null;
          Exp_Free.Val_NB = 0;
          Exp_Free.Val_Var = null; 
          Exp_Free.Val_Call = null;
          Exp_Free.next = null;
          Exp_Free.prev = null;
          Exp_Free.index = null;
          Exp_Free = null;
          GC.Collect();
      }
    }
}
