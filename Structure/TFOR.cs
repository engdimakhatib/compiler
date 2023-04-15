using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public  class TFOR
    {
       public TPVar V;//increase or decrement
       public TInstruction L_inst;
       public TEXP Exp_Begin;
       public TEXP Exp_End;
       public Boolean Is_Down;

       public static void Free(TFOR For_Free)
       {
           TPVar.Free(For_Free.V);
           TInstruction.Free(For_Free.L_inst);
           TEXP.Free(For_Free.Exp_Begin);
           TEXP.Free(For_Free.Exp_End);
           For_Free.Is_Down = false;
           For_Free = null;
           GC.Collect();
       }
    }
}
