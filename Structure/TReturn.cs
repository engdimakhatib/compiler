using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
  public class TReturn
    {
     public TEXP exp;

     public static void Free(TReturn Return_Free)
     {
         Free_Class.Free_EXP(Return_Free.exp);
         Return_Free = null;
         GC.Collect();
     }
    }
}
