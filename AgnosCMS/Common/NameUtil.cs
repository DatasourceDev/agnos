using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgnosCMS.Common
{
   public class NameUtil
   {
      public class GenTmpLogsheet
      {
         public static string GenGroupID(int i, string name)
         {
            return "Tmp_Log_Group_Rows_" + i + "__" + name;
         }
         public static string GenGroupName(int i, string name)
         {
            return "Tmp_Log_Group_Rows[" + i + "]." + name;
         }
         public static string GenHeaderID(int i, int j, string name)
         {
            return "Tmp_Log_Group_Rows_" + i + "__Tmp_Log_Header_Rows_" + j + "__" + name;
         }

         public static string GenHeaderName(int i, int j, string name)
         {
            return "Tmp_Log_Group_Rows[" + i + "].Tmp_Log_Header_Rows[" + j + "]." + name;
         }

         public static string GenFieldID(int i, int j, string name)
         {
            return "Tmp_Log_Group_Rows_" + i + "__Tmp_Log_Field_Rows_" + j + "__" + name;
         }

         public static string GenFieldName(int i, int j, string name)
         {
            return "Tmp_Log_Group_Rows[" + i + "].Tmp_Log_Field_Rows[" + j + "]." + name;
         }

         public static string GenMapID(int i, int j, int k, string name)
         {
            return "Tmp_Log_Group_Rows_" + i + "__Tmp_Log_Header_Rows_" + j + "__Tmp_Log_Map_Rows_" + k + "__" + name;
         }

         public static string GenMapName(int i, int j, int k, string name)
         {
            return "Tmp_Log_Group_Rows[" + i + "].Tmp_Log_Header_Rows[" + j + "].Tmp_Log_Map_Rows[" + k + "]." + name;
         }


      }

      public class GenLogsheet
      {
         public static string GenGroupID(int i, string name)
         {
            return "Logsheet_Group_Rows_" + i + "__" + name;
         }
         public static string GenGroupName(int i, string name)
         {
            return "Logsheet_Group_Rows[" + i + "]." + name;
         }
         public static string GenHeaderID(int i, int j, string name)
         {
            return "Logsheet_Group_Rows_" + i + "__Logsheet_Header_Rows_" + j + "__" + name;
         }

         public static string GenHeaderName(int i, int j, string name)
         {
            return "Logsheet_Group_Rows[" + i + "].Logsheet_Header_Rows[" + j + "]." + name;
         }

         public static string GenFieldID(int i, int j, string name)
         {
            return "Logsheet_Group_Rows_" + i + "__Logsheet_Field_Rows_" + j + "__" + name;
         }

         public static string GenFieldName(int i, int j, string name)
         {
            return "Logsheet_Group_Rows[" + i + "].Logsheet_Field_Rows[" + j + "]." + name;
         }

         public static string GenMapID(int i, int j, int k, string name)
         {
            return "Logsheet_Group_Rows_" + i + "__Logsheet_Header_Rows_" + j + "__Logsheet_Map_Rows_" + k + "__" + name;
         }

         public static string GenMapName(int i, int j, int k, string name)
         {
            return "Logsheet_Group_Rows[" + i + "].Logsheet_Header_Rows[" + j + "].Logsheet_Map_Rows[" + k + "]." + name;
         }

      }


      public class GenCMSProduct
      {
         public static string GenMapID(int i, string name)
         {
            return "Product_Rows_" + i + "__" + name;
         }
         public static string GenMapName(int i, string name)
         {
            return "Product_Rows[" + i + "]." + name;
         }
      }
   }
}