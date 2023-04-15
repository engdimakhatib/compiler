using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TInstruction
    {
       public object INS;
       public TInstruction next;

       public static void Free(TInstruction Inst_Free)
       {
           while (Inst_Free != null)
           {
               TInstruction Aux = Inst_Free.next;

               #region Free

               if (Inst_Free.INS is TAssign)
               {
                   TAssign.Free(Inst_Free.INS as TAssign);
               }
               else
                   if (Inst_Free.INS is TFOR)
                   {
                       TFOR.Free(Inst_Free.INS as TFOR);
                   }
                   else
                       if (Inst_Free.INS is TWHILE)
                       {
                           TWHILE.Free(Inst_Free.INS as TWHILE);
                       }
                       else
                           if (Inst_Free.INS is TIF)
                           {
                               TIF.Free(Inst_Free.INS as TIF);
                           }
                           else
                               if (Inst_Free.INS is TReturn)
                               {
                                   TReturn.Free(Inst_Free.INS as TReturn);
                               }
                               else
                                   if (Inst_Free.INS is TRead)
                                   {
                                       TRead.Free(Inst_Free.INS as TRead);
                                   }
                                   else
                                       if (Inst_Free.INS is TWRITE)
                                       {
                                           TWRITE.Free(Inst_Free.INS as TWRITE);
                                       }
                                       else
                                           if (Inst_Free.INS is TCall)
                                           {
                                               TCall.Free(Inst_Free.INS as TCall);
                                           }
                                           else
                                               if (Inst_Free.INS is TBreak)
                                               {
                                                   TBreak.Free(Inst_Free.INS as TBreak);
                                               }
                                               else
                                                   Inst_Free = null;

               #endregion free

               Inst_Free = Aux;
               GC.Collect();
           }//end while
           
       }
    }
}
