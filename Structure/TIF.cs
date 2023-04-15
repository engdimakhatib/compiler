using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TIF
    {
       public TEXP cond;
       public TInstruction INS_IF;
       public TInstruction INS_ELSE;

       public static void Free(TIF If_Free)
       {
           Free_Class.Free_EXP(If_Free.cond);
           TInstruction.Free(If_Free.INS_IF);
           TInstruction.Free(If_Free.INS_ELSE);
           If_Free = null;
           GC.Collect();
       }
    }
}
