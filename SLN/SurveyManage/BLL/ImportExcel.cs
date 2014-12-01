using Common;
using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class ImportExcel
    {

        #region public List<object> ImportDataTable(bool isStaff, string filename, string id)
        public List<object> ImportDataTable(bool isStaff, string filename, string id)
        {
            List<object> status = new List<object>();
            try
            {
                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=Excel 8.0;";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("select * from [Sheet1$]", conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "table");
                conn.Close();
                DataTable table = ds.Tables["table"];
                if (isStaff)
                {
                    status = InspectionStaffData(table, id);
                }
                else
                {
                    status = InspectionCompanyData(table, id);
                }
                if (status[0].ToString() == "1")
                {
                    if (isStaff)
                    {
                        return WriteStaffData(table);
                    }
                    else
                    {
                        return WriteCompanyData(table);
                    }
                }
                else
                {
                    return status;
                }
            }
            catch
            {
                status.Add(0);
                status.Add("文件导入失败！请检查文件.....");
                return status;
            }

        }

        #endregion
        #region  private List<object> InspectionStaffData(DataTable table, string id)
        private List<object> InspectionStaffData(DataTable table, string id)
        {
            List<object> status = new List<object>();
            if (table.Rows.Count >= 1 && table.Columns.Count == 4)
            {
                List<company> list = new Companyhandling().SearchSonCompany(id);
                List<string> companyIdList = new List<string>();
                foreach (company c in list)
                {
                    companyIdList.Add(c.company_Id);
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (!String.IsNullOrEmpty(table.Rows[i][0].ToString()) && table.Rows[i][0].ToString().Length <= 25 && InspectionEmail(table.Rows[i][1].ToString()) && table.Rows[i][1].ToString().Length <= 25 && !String.IsNullOrEmpty(table.Rows[i][2].ToString()) && table.Rows[i][2].ToString().Length <= 25 && !String.IsNullOrEmpty(table.Rows[i][3].ToString()))
                    {
                        if (!companyIdList.Contains(table.Rows[i][3].ToString()))
                        {
                            status.Add(0);
                            status.Add("文件导入失败！请检查文件员工所属公司编号是否正确.....");
                            return status;
                        }
                    }
                    else
                    {
                        status.Add(0);
                        status.Add("文件导入失败！请检查员工信息是否正确.....");
                        return status;
                    }
                }
                status.Add(1);
                status.Add("正在写入文件......");
                return status;
            }
            else
            {
                status.Add(0);
                status.Add("文件导入失败！请检查文件格式是否正确.....");
                return status;
            }

        }

        #endregion
        #region private List<object> WriteStaffData(DataTable table)
        private List<object> WriteStaffData(DataTable table)
        {
            List<object> result = new List<object>();
            Random r = new Random();
            Staff s = new Staff();
            long num = s.SearchCount(d => d.staff_Id != 0);
            for (int i = 0; i < table.Rows.Count; i++)
            {

                staff sf = new staff()
                {
                    staff_Id = 1,
                    staff_TrueName = table.Rows[i][0].ToString(),
                    staff_Email = table.Rows[i][1].ToString(),
                    staff_Department = table.Rows[i][2].ToString(),
                    staff_IsWrite = false,
                    staff_IsDel = false,
                    staff_OwnCompanyId = table.Rows[i][3].ToString(),
                    staff_Username = table.Rows[i][3].ToString() + (num + i) + r.Next(10),
                    staff_Password = Md5.GetMd5Word("123456"),
                };
                if (s.SearchCount(d => d.staff_TrueName == sf.staff_TrueName && d.staff_Email == sf.staff_Email && d.staff_Department == sf.staff_Department && d.staff_OwnCompanyId == sf.staff_OwnCompanyId && d.staff_IsDel == false) == 0)
                {
                    try
                    {
                        s.Add(sf);
                    }
                    catch (Exception ex)
                    {
                        if (result.Count == 0)
                        {
                            result.Add(10);
                            result.Add("导入成功！但有点数据未被导入....");
                            result.Add("sf");
                        }
                        result.Add(sf.staff_TrueName);
                        result.Add(sf.staff_Email);
                        result.Add(sf.staff_Department);
                        result.Add(sf.staff_OwnCompanyId);
                        result.Add(ex.Message);
                    }
                }
                else
                {
                    if (result.Count == 0)
                    {
                        result.Add(10);
                        result.Add("导入完成！但有点数据未被导入....");
                        result.Add("sf");
                    }
                    result.Add(sf.staff_TrueName);
                    result.Add(sf.staff_Email);
                    result.Add(sf.staff_Department);
                    result.Add(sf.staff_OwnCompanyId);
                    result.Add("数据已存在！");
                }
            }

            if (result.Count == 0)
            {
                result.Add(11);
                result.Add("导入成功！");
            }
            return result;
        }
        #endregion
        #region    private string ProductionPassword(Random r)
        private string ProductionPassword(Random r)
        {
            char[] c = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            string pwd = String.Empty;
            for (int t = 1; t <= 6; t++)
            {
                pwd += c[r.Next(c.Length)];
            }
            return pwd;

        }
        #endregion
        #region  private bool InspectionEmail(string email)
        private bool InspectionEmail(string email)
        {
            Regex r = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return r.Match(email).Value == email;

        }

        #endregion
        #region   private List<object> InspectionCompanyData(DataTable table, string id)
        private List<object> InspectionCompanyData(DataTable table, string id)
        {
            List<object> status = new List<object>();
            status.Clear();
            if (table.Rows.Count >= 1 && table.Columns.Count == 5)
            {
                List<company> list = new Companyhandling().SearchSonCompany(id);
                List<string> companyIdList = new List<string>();
                foreach (company c in list)
                {
                    companyIdList.Add(c.company_Id);
                }
                Company com = new Company();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if ((!String.IsNullOrEmpty(table.Rows[i][0].ToString())) && InspectionCompanyId(table.Rows[i][0].ToString()) && (!String.IsNullOrEmpty(table.Rows[i][1].ToString())) && table.Rows[i][1].ToString().Length <= 32 && table.Rows[i][2].ToString().Length <= 12 && table.Rows[i][3].ToString().Length <= 12 && (!String.IsNullOrEmpty(table.Rows[i][4].ToString())) && InspectionCompanyId(table.Rows[i][4].ToString()))
                    {
                        string importId = table.Rows[i][0].ToString();
                        if (com.SearchCount(d => d.company_Id == importId && d.company_IsDel == false) != 0)
                        {
                            status.Add(0);
                            status.Add("文件导入失败！公司编号" + table.Rows[i][0] + "已被占用.....");
                            return status;
                        }
                        for (int k = 0; k < table.Rows.Count; k++)
                        {
                            companyIdList.Add(table.Rows[k][0].ToString());
                        }
                        if (!companyIdList.Contains(table.Rows[i][4].ToString()))
                        {
                            status.Add(0);
                            status.Add("文件导入失败！请检查所属公司编号是否正确.....");
                            return status;
                        }

                    }
                    else
                    {
                        status.Add(0);
                        status.Add("文件导入失败！请检查公司信息是否正确.....");
                        return status;
                    }
                }
                status.Add(1);
                status.Add("正在写入文件......");
                return status;
            }
            else
            {
                status.Add(0);
                status.Add("文件导入失败！请检查公司信息是否正确.....");
                return status;
            }
        }

        #endregion
        #region private List<object> WriteCompanyData(DataTable table)
        private List<object> WriteCompanyData(DataTable table)
        {
            List<object> result = new List<object>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                try
                {
                    Company c = new Company();
                    string parentId = table.Rows[i][4].ToString();
                    company parent = c.Search(d => d.company_Id == parentId && d.company_IsDel == false)[0];
                    company com = new company()
                    {
                        company_Id = table.Rows[i][0].ToString(),
                        company_IsSonIn = false,
                        company_IsDel = false,
                        company_Level = parent.company_Level + 1,
                        company_Name = table.Rows[i][1].ToString(),
                        company_OwnerCompany = table.Rows[i][4].ToString(),
                        company_Province = table.Rows[i][2].ToString(),
                        company_Status = 0,
                        company_Trade = table.Rows[i][3].ToString()
                    };
                    if (c.SearchCount(d => d.company_Id == com.company_Id && d.company_IsDel == false) == 0)
                    {
                        c.Add(com);
                        if (!parent.company_IsSonIn)
                        {
                            parent.company_IsSonIn = true;
                            c.Modify(parent, "company_IsSonIn");
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (result.Count == 0)
                    {
                        result.Add(10);
                        result.Add("导入完成！但有点数据未被导入....");
                        result.Add("cy");

                    }
                    result.Add(table.Rows[i][0].ToString());
                    result.Add(table.Rows[i][1].ToString());
                    result.Add(table.Rows[i][2].ToString());
                    result.Add(table.Rows[i][3].ToString());
                    result.Add(table.Rows[i][4].ToString());
                    result.Add(ex.Message);
                }
            }


            if (result.Count == 0)
            {
                result.Add(11);
                result.Add("导入成功!");
            }
            return result;
        }
        #endregion
        #region private bool InspectionCompanyId(string id)

        private bool InspectionCompanyId(string id)
        {
            Regex r = new Regex(@"^[A-Za-z0-9]{0,25}$");
            return r.Match(id).Value == id && id.Length <= 25;

        }
        #endregion

        public string ImportQusetions(string filename, string id)
        {
            long testInfoId = 0;

            try
            {
                testInfoId = new TestInfo().Search(u => u.testInfo_OwnComPanyId == id && u.testInfo_IsDel == false)[0].testInfo_Id;
            }
            catch
            {
                return "您还没有创建问卷！";
            }
            #region 打开文件，检查数据格式并转化为DataTable
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [Sheet1$]", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "table");
            conn.Close();
            DataTable table = ds.Tables["table"];
            string result = CheckDataTable(table);
            if (result != "ok")
            {
                return result;
            }
            #endregion

            int errorTests = 0;

            int errorQuestions = 0;
            Question Q = new Question();
            Test T = new Test();
            //遍历每行，将数据包装进rightTests和rightQuestions集合
            test t = new test();
            question q = new question();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                
                bool AllNull = string.IsNullOrEmpty(table.Rows[i][2].ToString()) && string.IsNullOrEmpty(table.Rows[i][3].ToString()) && string.IsNullOrEmpty(table.Rows[i][4].ToString()) && string.IsNullOrEmpty(table.Rows[i][5].ToString()) && string.IsNullOrEmpty(table.Rows[i][6].ToString()) && string.IsNullOrEmpty(table.Rows[i][7].ToString());
                if (AllNull)
                {
                    q.question_Option1 = "1";
                    q.question_Option2 = "2";
                    q.question_Option3 = "3";
                    q.question_Option4 = "4";
                    q.question_Option5 = "5";
                    q.question_Option6 = "6";
                    q.question_Option7 = "7";
                }
                else
                {
                    q.question_Option1 = table.Rows[i][2].ToString();
                    q.question_Option2 = table.Rows[i][3].ToString();
                    q.question_Option3 = table.Rows[i][4].ToString();
                    q.question_Option4 = table.Rows[i][5].ToString();
                    q.question_Option5 = table.Rows[i][6].ToString();
                    q.question_Option6 = table.Rows[i][7].ToString();
                    q.question_Option7 = table.Rows[i][8].ToString();
                }
                string stem = table.Rows[i][0].ToString();

                List<question> questions = Q.Search(k => k.qusetion_Stem == stem && k.question_IsDel == false);
                if (questions.Count > 0)
                {//题库中已存在此问题，不再插入，直接引用
                    t.test_QuestionId = questions[0].question_Id;
                    t.test_TestInfoId = testInfoId;
                    try {
                        long qid=t.test_QuestionId;
                        if (T.SearchCount(k => k.test_QuestionId == qid && k.test_TestInfoId == testInfoId&&k.question.question_IsDel==false) > 0)
                        {
                            continue;
                        }
                         T.Add(t);
                    }
                    catch
                    {
                        errorTests++;
                    }
                }
                else
                {
                    q.qusetion_Stem = stem;

                    string myclass = table.Rows[i][1].ToString();
                    List<@class> classes = new _Class().Search(k => k.class_Class == myclass);
                    if (classes.Count <= 0)
                    {
                        return "导入中止，请确定您所插入表中的问题类别在类别中已存在！<br/>请在题库类别管理中添加。";
                    }
                    else
                    {
                        q.question_ClassId = classes[0].class_Id;
                    }
                    try
                    {
                        Q.Add(q);
                    }
                    catch
                    {
                        errorQuestions++;
                    }
                    t.test_QuestionId = q.question_Id;
                    t.test_TestInfoId = testInfoId;
                    try
                    {
                        T.Add(t);
                    }
                    catch
                    {
                        errorTests++;
                    }
                }
            }

            if (errorTests == 0 && errorQuestions == 0) { return "ok"; }
            else { return "有" + errorQuestions + "条数据出错，请检查数据！"; }
        }
        private string CheckDataTable(DataTable dt)
        {
            if (dt.Columns.Count < 9)
            {
                return "您所提交的数据中缺少必须列，导入终止！";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    return "问题题干不可为空！";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
                {
                    return "问题类别不可为空";
                }

            }

            return "ok";
        }
    }
}
