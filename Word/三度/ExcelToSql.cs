using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlText
{
    class ExcelToSql
    {
        #region 读取Excel文件为datatable，并返回+DataTable ExcelToDT(string fileName, out string back)
        /// <summary>
        /// 读取Excel文件为datatable，并返回
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="back">错误信息</param>
        /// <returns></returns>
        public static DataTable ExcelToDT(string fileName, out string back)
        {

            back = "OK";

            string Path = "F:\\桌面\\" + fileName + ".xlsx";
            string strConn = "Provider=Microsoft.Ace.OLEDB.12.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            try
            {
                OleDbDataAdapter db = new OleDbDataAdapter("select * from [sheet2$]", conn);
                DataSet ds = new DataSet();
                db.Fill(ds, "Tab");
                conn.Close();
                DataTable dt = ds.Tables["Tab"];
                return dt;
            }
            catch
            {

                back = "读取Excel错误，请查看上传文件要求，注意表名！";
            }

            return null;
        } 
        #endregion
    }
}
