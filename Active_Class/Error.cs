using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Compiler_Compiler
{
   public class Error
    {
       public static void Read_File_Error()
       { 
           StreamReader File_Error=new StreamReader("Error.txt");
           string[] Line_Error;
           while(!File_Error.EndOfStream)
           {
               Line_Error = File_Error.ReadLine().Split(':');
               Global.Error_Message_NB.Insert(Convert.ToInt32( Line_Error[0]), Line_Error[1]);
           }
           File_Error.Close();
           StreamReader type_Err = new StreamReader("Type_Error.txt");
           string[] line;
           while (!type_Err.EndOfStream)
           {
               line = type_Err.ReadLine().Split(':');
               Global.Type_Error.Insert(Convert.ToInt32(line[0]), line[1]);
           }
           type_Err.Close();
       }

       public static string Get_Error(Int32 NB_Error)
       {
           return Global.Error_Message_NB[NB_Error].ToString();
       }

       public static string Get_Type_Error(int Num_Type_Error)
       {
           return Global.Type_Error[Num_Type_Error].ToString();
       }
    }
}
