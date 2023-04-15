using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
    public class TWHILE
    {
        public TEXP cond;
        public TInstruction L_inst;

        public static void Free(TWHILE While_Free)
        {
            Free_Class.Free_EXP(While_Free.cond);
            TInstruction.Free(While_Free.L_inst);
            While_Free = null;
            GC.Collect();
        }
    }
}
