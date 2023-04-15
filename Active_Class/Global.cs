using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace Compiler_Compiler
{
   public class Global
   {
       
       #region Global variable
       public enum TypeSymbol
       {
           u_IF,
           u_DO,
           u_WHILE,
           u_FOR,
           u_EOF,
           u_CST_REAL,
           u_CST_INT,
           u_CST_STR,
           u_ERROR,
           u_VAR,
           u_VAR_PROC,
           u_DEFINE,
           u_UNKNOWN,
           u_EQUAL,
           u_NOT_EQUAL,
           u_PLUS,
           u_MINUS,
           u_NOT,
           u_MULTI,
           u_DIV,
           u_LT,
           u_GT,
           u_LE,
           u_GE,
           u_AND,
           u_OR,
           u_POWER_SIGHN,
           u_SEMICOLON,
           u_COMMA,
           u_OPENP,
           u_CLOSEP,
           u_CLOSEB,
           u_OPENB,
           u_BEGIN,
           u_END,
           u_FULLSTOP,
           u_SHARP,
           u_MOD,
           u_ASSIGN,
           u_INCLUDE,
           u_PROCEDURE,
           u_FUNCTION,
           u_INPUT,
           u_OUTPUT,
           u_THEN,
           u_DOWN_TO,
           u_TO,
           u_ELSE,
           u_IDIV,
           u_CALL,
           u_VAR_DEFINE,
           u_BREAK,
           u_RETURN,
           u_EXIT,
           u_HALT,
           u_READ,
           u_WRITE,
           u_WRITELN,
           u_FALSE,
           u_TRUE,
           u_SIN,
           u_COS,
           u_TAN,
           u_ATAN,
           u_LOG,
           u_LN,
           u_POWER,
           u_SQRT,
           u_UNARY_PLUS,
           u_UNARY_MINUS,
           u_NORMAL,
           u_LENGTH,
           u_INT_TO_STRING,
           u_STRING_TO_INT,
           u_MAIN,
           u_ABS
       };
       public static int CI = 0;
       public static string CL = "";
       public static StreamReader CF;
       public static Global.TypeSymbol UL;
       public static TSymbol GSymbol;
       public static TSymbol LastSymbol;
       public static TTFile GFile;//رأس لائحة الملفات
       public static TTFile G_Cur_File = new TTFile();//الملف الحالي
       public static Double G_Cur_NB = 0;
       public static String G_Cur_STR = "";
       public static TPVar G_Var;
       public static TIdentif G_Iden;
       public static TProcedure G_Var_Proc;
       public static TDenfine G_Var_Define;
       public static TIdentif G_Cur_Id;
       public static ArrayList Error_Message_NB = new ArrayList();
       public static ArrayList Type_Error = new ArrayList();
       public static string Message_Wrong = ""; 
       public static string buffer_Temp = "";
       public static string buffer = "";
       public static TReturn G_Return;
       public static TInstruction Main_INST;

       public static Boolean IS_INPUT_INTEGER = false;
       public static OUTPUT Output_Write_Form = new OUTPUT();
       public static Boolean  READ__Word_Main=false;

       #endregion

        #region  Global method

       public static void ReadKeyWord()
       {
           int i = 0;
           StreamReader txt = new StreamReader("keywords_Upper - Copy.INI");
           if (txt == null)
           {
              Global.Message_Wrong=Error.Get_Error(1);////massage
              throw new Exception();
           }
           else
           {
               string line = "";
               while ((line = txt.ReadLine()) != null)
               {
                   TSymbol SysAux = new TSymbol();
                   SysAux.name = line;
                   SysAux.ul = (Global.TypeSymbol)i;
                   SysAux.next = null;
                   if (Global.GSymbol == null)
                   {
                       Global.GSymbol = SysAux;
                   }
                   else
                   {
                       Global.LastSymbol.next = SysAux;
                   }
                   Global.LastSymbol = SysAux;
                   i++;
               }
           }
       }

       public static TSymbol FindSymbol(string SFind)
       {
           TSymbol temp;
           temp = Global.GSymbol;
           SFind = SFind.ToUpper();
           while (temp != null && temp.name != SFind)
           {
               temp = temp.next;
           }
           return temp;
       }

       public static TIdentif Find_IDentif(string bufer, TIdentif GID)
       {
           while (GID != null && GID.name!=bufer)
           {
               GID = GID.next;
           }
           return GID;
       }

       public static void Add_IDentif(string bufer, ref TIdentif GID,Global.TypeSymbol T_UL)//global input output
       {
           TIdentif Temp;
           if (T_UL==Global.TypeSymbol.u_VAR)
           {
               Temp = new TPVar();
           }
           else if (T_UL==Global.TypeSymbol.u_VAR_PROC)
           {
               Temp = new TProcedure();
           }
           else
               { 
                   Temp = new TDenfine(); 
               }
           Temp.ul = T_UL;
           Temp.name = bufer;
           Temp.next = GID;
           GID = Temp;


      }
 
       public static void AddFile(string FileName)
       {
           TTFile help1;
           TTFile help2;
           TTFile temp;
           help1 = Global.GFile;
           help2 = help1;
           while (help1 != null && help1.name != Path.GetFullPath(FileName))
           {
               help2 = help1;
               help1 = help1.next;
           }
           if (!File.Exists(Path.GetFullPath(FileName)))
           {
                Global.Message_Wrong = Error.Get_Error(1) + "\t" + Error.Get_Type_Error(2);
                throw new Exception();
           }
           if (help1 == null)
           {
              temp=new TTFile();
              temp.name = Path.GetFullPath(FileName);
              temp.next = null;
              if (help2 != null)
              {
                  help2.next = temp;// الدخولات الأخرى من أجل
              }
              else
              {
                  Global.GFile = temp;
              }//من أجل أول دخول
           }
       }

       #endregion 
    }
}