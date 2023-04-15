using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
  public  class TWRITE
    {
     public TEXP exp;
     public Boolean Is_ln = false;

     public static void Free(TWRITE Write_Aux)
     {
         Free_Class.Free_EXP(Write_Aux.exp);
         Write_Aux.Is_ln = false;
         Write_Aux = null;
         GC.Collect();
     }

    }
}
