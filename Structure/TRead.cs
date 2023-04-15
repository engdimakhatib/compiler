using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TRead
    {
      public TPVar v;
      public TEXP Index;

      public static void Free(TRead Read_Free)
      {
          TPVar.Free(Read_Free.v);
          Free_Class.Free_EXP(Read_Free.Index);
          Read_Free = null;
          GC.Collect();
      }
    }
}
