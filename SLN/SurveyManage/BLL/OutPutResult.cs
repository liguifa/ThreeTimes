using Common.Models;
using DAL;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Reflection;
using System.IO;
using System.IO.Compression;
namespace BLL
{
    public class OutPutResult
    {
        public string OutPut(string id,string rootPath,out string zipPath)
        {
            zipPath = null;
            try
            {
                bool complete = new Company().Search(k => k.company_Id == id)[0].company_Status == 2;
                if (!complete)
                {
                    return "调查未完成，如需导出请手动结束调查！";
                }

                //查找出所有公司
                List<company> companys = new Companyhandling().SearchSonCompany(id);

                //1.0 根据公司id找到问卷
                long testInfoId = new TestInfo().Search(k => k.testInfo_OwnComPanyId == id && k.testInfo_IsDel == false)[0].testInfo_Id;
                //1.1 根据问卷找到题目

                List<test> tests = new Test().Search(k => k.test_TestInfoId == testInfoId, u => u.test_Id);

                List<int> plags = new List<int>();
                for (int i = 0; i < tests.Count; i++)
                {
                    if (tests[i].question.@class.class_Id == 1)
                    {
                        plags.Add(i);
                    }
                }

                string directoryPath = rootPath + @"\" + id;
                if (Directory.Exists(directoryPath))
                {
                    //如果文件夹存在
                    Directory.Delete(directoryPath,true);
                  

                }
                Directory.CreateDirectory(directoryPath);
                foreach (company c in companys)
                {
                    string cid = c.company_Id;
                    //2.0 由公司Id找到所有员工 、spare字段不为 1(有效问卷的员工)
                    List<staff> staffs = new Staff().Search(k => k.staff_OwnCompanyId == cid && k.staff_IsDel == false && string.IsNullOrEmpty(k.staff_Spare) && k.staff_IsWrite == true);
                    //3.0 由员工找到 答案,此答案与问卷表中问题对应
                    key[] keys = new key[staffs.Count];
                    System.Data.DataTable dt = new System.Data.DataTable("答案");
                    DataColumn dc1 = new DataColumn("序号", typeof(int));
                    DataColumn dc2 = new DataColumn("问题", typeof(string));
                    dt.Columns.Add(dc1);
                    dt.Columns.Add(dc2);

                    for (int s = 0; s < staffs.Count; s++)
                    {
                        long sid = staffs[s].staff_Id;
                        keys[s] = new Key().Search(k => k.key_IsDel == false && k.key_StaffId == sid)[0];
                        DataColumn dc = new DataColumn(staffs[s].staff_TrueName, typeof(string));
                        dt.Columns.Add(dc);
                    }
                    DataRow dr = dt.NewRow();
                    dr["问题"] = "一、基础问题";
                    dt.Rows.Add(dr);
                    for (int i = 0; i < plags.Count; i++)
                    {
                        DataRow dr1 = dt.NewRow();
                        dr1["问题"] = tests[plags[i]].question.qusetion_Stem;
                        dr1["序号"] = i + 1;
                        for (int s = 0; s < staffs.Count; s++)
                        {
                            Type myData = typeof(key);
                            string value = "key_key" + (plags[i] + 1);

                            foreach (PropertyInfo info in myData.GetProperties())
                            {
                                if (info.Name == value)
                                {
                                    dr1[staffs[s].staff_TrueName] = info.GetValue(keys[s]);
                                }
                            }


                        }
                        dt.Rows.Add(dr1);

                    }
                    DataRow dr2 = dt.NewRow();
                    dr2["问题"] = "二、专业问题";
                    dt.Rows.Add(dr2);
                    int j = 1;
                    for (int i = 0; i < tests.Count; i++)
                    {

                        if (!plags.Contains(i))
                        {
                            DataRow dr1 = dt.NewRow();
                            dr1["序号"] = j++;
                            dr1["问题"] = tests[i].question.qusetion_Stem;

                            for (int s = 0; s < staffs.Count; s++)
                            {
                                Type myData = typeof(key);
                                string value = "key_key" + (i + 1);

                                foreach (PropertyInfo info in myData.GetProperties())
                                {
                                    if (info.Name == value)
                                    {
                                        dr1[staffs[s].staff_TrueName] = info.GetValue(keys[s]);
                                    }
                                }

                            }
                            dt.Rows.Add(dr1);
                        }
                    }

                    //4.0 导出到Excel表中，随机命名；
                    //定义一个COM中空类型的对象
                    object missing = System.Reflection.Missing.Value;
                    Excel.Application app = new Excel.Application();
                    app.Application.Workbooks.Add(true);
                    Excel.Workbook book = (Excel.Workbook)app.ActiveWorkbook;
                    Excel.Worksheet sheet = (Excel.Worksheet)book.ActiveSheet;
                    sheet.Cells[1, 1] = "序号";
                    sheet.Cells[1, 2] = "问题";
                    for (int i = 0; i < staffs.Count; i++)
                    {
                        sheet.Cells[1, i + 3] = staffs[i].staff_TrueName;

                    }
                    //将DataTable赋值给excel
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {


                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            sheet.Cells[k + 2, i + 1] = dt.Rows[k][i];

                        }
                    }
                    //保存excel文件
                    string filename = directoryPath + "\\" + c.company_Id + ".xlsx";
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    book.SaveCopyAs(filename);
                    //关闭文件
                    book.Close(false, missing, missing);
                    //退出excel
                    app.Quit();

                }
                 zipPath = rootPath + "\\" + id + ".zip";
                 if (File.Exists(zipPath)) {
                     File.Delete(zipPath);
                 }
                
                ZipFile.CreateFromDirectory(directoryPath, zipPath);
            }
            catch {
                zipPath = null;
                return "未知错误！";
            }
            return "ok";
        }
    }
}
