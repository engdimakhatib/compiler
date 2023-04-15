using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler_Compiler
{
   public class TTFile
    {
       public string name;
       public TTFile next;
       public TDenfine Local_Gdefine;

       public static void Free_GDfine(TTFile File_Define)
       {
           while (File_Define.Local_Gdefine != null)
           {
               TDenfine Aux = (TDenfine)File_Define.Local_Gdefine.next;
               TDenfine.Free(File_Define.Local_Gdefine);
               File_Define.Local_Gdefine = Aux;
           }
       }

       public static void Free(TTFile File_Free)
       {
           File_Free.name = null;
           File_Free.next = null;
           Free_GDfine(File_Free);
           File_Free = null;
           GC.Collect();
       }
    }
}
