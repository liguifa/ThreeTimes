using Common;
using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 员工操作类
    /// </summary>
    public class StaffHanding : BaseBLL<staff>
    {
        #region 查询属于此公司的员工public List<staff> SelectStaffByComID(string comId)
        /// <summary>
        /// 查询属于此公司的员工
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public List<staff> SelectStaffByComID(string comId, int pageIndex, int pageSize, out long totalNum)
        {
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            totalNum = SearchCount(u => u.staff_IsDel == false && u.staff_OwnCompanyId == comId);
            return Search(u => u.staff_IsDel == false && u.staff_OwnCompanyId == comId, u => u.staff_Department, pageIndex, pageSize);
        }
        #endregion

        #region 查询此公司及以下公司的所有员工
        public List<staff> SelectAllStaffByComID(string comId, int pageIndex, int PageSize, out long totalNum)
        {
            Companyhandling ch = new Companyhandling();
            List<company> companys = ch.SearchSonCompany(comId);
            List<string> comIds = new List<string>();
            for (int i = 0; i < companys.Count; i++)
            {
                comIds.Add(companys[i].company_Id);
            }

            totalNum = base.SearchCount(d => comIds.Contains(d.staff_OwnCompanyId) && d.staff_IsDel == false);
            long totalPage = totalNum / PageSize;
            if (totalNum % PageSize != 0)
            {
                totalPage++;
            }
            if (pageIndex > totalPage)
            {
                pageIndex = Convert.ToInt32(totalPage);
            }
            if (pageIndex < 0 || pageIndex == 0)
            {
                pageIndex = 1;
            }


            //获取此ID下的所有子公司

            List<staff> staffs = base.Search(d => comIds.Contains(d.staff_OwnCompanyId) && d.staff_IsDel == false, u => u.staff_Department, pageIndex, PageSize);
            return staffs;
        }
        #endregion

        #region 增加员工 public string AddStaff(string datas, string comId, out staff os)
        public string AddStaff(string datas, string comId, out staff os)
        {
            string[] data = datas.Split('#');
            staff s = new staff();
            if (string.IsNullOrEmpty(data[0]))
            {
                os = null;
                return "员工名称不能为空";
            }
            s.staff_TrueName = data[0];
            if (data[1] == "undefined")
            {
                long staffCount = SearchCount(u => u.staff_IsDel == true || u.staff_IsDel == false);
                s.staff_Username = comId + staffCount;
            }
            else
            {
                s.staff_Username = data[1];
            }
            //密码
            s.staff_Password = Md5.GetMd5Word("123456");

            if (string.IsNullOrEmpty(data[3]))
            {
                os = null;
                return "邮箱不可为空!";
            }
            s.staff_Email = data[3];
            if (string.IsNullOrEmpty(data[4]))
            {
                os = null;
                return "部门不可为空";
            }
            s.staff_Department = data[4];
            s.staff_IsDel = false;
            s.staff_IsWrite = false;
            s.staff_OwnCompanyId = comId;

            try
            {
                base.Add(s);
                os = s;
            }
            catch
            {
                os = null;
                return "更新条目时出错";
            }

            return "ok";
        }
        #endregion

        #region 根据id批量删除Staff  public string RemoveStaffs(string datas)
        public string RemoveStaffs(string cid,string datas, out string[] ids)
        {
            
            string[] data = datas.Split(',');
            ids = data;
            if (new Companyhandling().GetCompnayStatus(cid) == 1)
            {
                return "公司正在调查中，不能删除！";
            }
            List<staff> s = new List<staff>();
            long id = 0;
            for (int i = 0; i < data.Length; i++)
            {
                id = Convert.ToInt64(data[i]);
                s = Search(u => u.staff_Id == id);
                s[0].staff_IsDel = true;
                try
                {
                    base.Modify(s[0], new string[] { "staff_IsDel" });
                }
                catch
                {
                    return "删除错误，请重试";
                }
            }

            return "ok";
        }
        #endregion

        public string ModifyStaffs(string datas, string ids)
        {
            string[] rows = datas.Split(',');

            string[] id = ids.Split(',');

            List<staff> staffs = new List<staff>();
            for (int i = 0; i < rows.Length; i++)
            {
                long tempId = Convert.ToInt64(id[i]);
                string[] s = rows[i].Split('#');
                staffs = Search(u => u.staff_Id == tempId);
                staffs[0].staff_TrueName = s[0];
                string username = s[1];
                if (SearchCount(u => u.staff_Username == username && u.staff_Id != tempId) > 0)
                {
                    return "您所更改的用户名已被占用";
                }
                staffs[0].staff_Username = s[1];

                staffs[0].staff_Email = s[3];
                staffs[0].staff_Department = s[4];
                try
                {
                    base.Modify(staffs[0], new string[] { "staff_TrueName", "staff_Username", "staff_Email", "staff_Department" });
                }
                catch
                {
                    return "非法输入，修改异常";
                }

            }
            return "ok";
        }

        public List<object> SendEmail(string id)
        {
            admin HeadOffice = new Admin().Search(d => d.admin_CompanyID == id && d.admin_IsDel == false)[0];
            List<company> com = new Companyhandling().SearchSonCompany(id);
            SendEmailOperation se = new SendEmailOperation();
            List<object> result = new List<object>();
            foreach (company c in com)
            {
                List<staff> sf = base.Search(d => d.staff_OwnCompanyId == c.company_Id && d.staff_IsDel == false);
                testInfo ti = new TestInfo().Search(d => d.testInfo_OwnComPanyId == c.company_Id && d.testInfo_IsDel == false)[0];
                foreach (staff s in sf)
                {
                    if (!se.SendEmail(s.staff_Email, HeadOffice.company.company_Name, HeadOffice.admin_PhoneNum, ti.testInfo_StartTime.ToString(), ti.testInfo_Deadline.ToString(), s.staff_Username, "123456", "staff"))
                    {
                        if (result.Count == 0)
                        {
                            result.Add(0);
                            result.Add("邮件发送结束,但有有员工发送失败,这可能是网络不通畅造成的,你可以在员工管理处重发邮件或在试一次！");
                        }
                        result.Add(s.staff_Id);
                        result.Add(s.staff_TrueName);
                        result.Add(s.company.company_Name);
                        result.Add(s.staff_Department);
                        result.Add(s.staff_Email);

                    }
                }
            }
            if (result.Count == 0)
            {
                result.Add(1);
                result.Add("邮件全部发送成功！");
            }
            return result;
        }
        public List<staff> SearchAllStaffsByValue(string comId, string value)
        {
            Companyhandling ch = new Companyhandling();
            List<company> companys = ch.SearchSonCompany(comId);
            List<string> comIds = new List<string>();
            for (int i = 0; i < companys.Count; i++)
            {
                comIds.Add(companys[i].company_Id);
            }

            List<staff> staffs = base.Search(d => comIds.Contains(d.staff_OwnCompanyId) && d.staff_TrueName.Contains(value) && d.staff_IsDel == false).ToList();
            return staffs;
        }
        public List<staff> SearchAllStaffsByEmail(string comId, string email)
        {
            Companyhandling ch = new Companyhandling();
            List<company> companys = ch.SearchSonCompany(comId);
            List<string> comIds = new List<string>();
            for (int i = 0; i < companys.Count; i++)
            {
                comIds.Add(companys[i].company_Id);
            }

            List<staff> staffs = base.Search(d => comIds.Contains(d.staff_OwnCompanyId) && d.staff_Email == email && d.staff_IsDel == false).ToList();
            return staffs;
        }
        public string ReSendEmailBystaffsId(string data, string comId)
        {
            string[] ids = data.Split(',');
            List<long> lids = new List<long>();

            foreach (string s in ids)
            {
                lids.Add(Convert.ToInt64(s));
            }
            List<staff> staffs = base.Search(u => lids.Contains(u.staff_Id) && u.staff_IsDel == false);


            SendEmailByListStaff se = new SendEmailByListStaff();
            return se.SendEmail(staffs, comId);
        }

        public List<object> Login(string user, string pwd, string yzm, string systemYzm)
        {
            List<object> result = new List<object>();

            if (Verification.VerificationCode.ComparisonVerCode(yzm,systemYzm))
            {
                pwd=Md5.GetMd5Word(pwd);
                List<staff> staffs = base.Search(d => d.staff_Password == pwd && d.staff_Username == user && d.staff_IsDel == false);
                if (staffs.Count != 0)
                {
                    staff sf = staffs[0];
                    if (sf.staff_IsWrite == false)
                    {
                        company parseCompany = new FindGGFatherCompanyByCompany().FindGGFatherCompany(sf.company);
                        if (parseCompany.company_Status == 1)
                        {
                            result.Add(1);
                            result.Add("登陆成功！");
                            result.Add(sf.staff_Id);
                        }
                        else if (parseCompany.company_Status == 0)
                        {
                            result.Add(0);
                            result.Add("调查未开始！无权登陆.....");
                        }
                        else if (parseCompany.company_Status >= 2)
                        {
                            result.Add(0);
                            result.Add("调查已结束！无权登陆.....");
                        }
                    }
                    else
                    {
                        result.Add(2);
                        result.Add("问卷已填写！无权登陆.....");
                        result.Add(sf.staff_Id);
                    }

                }
                else
                {
                    result.Add(0);
                    result.Add("账号或密码输入错误！");
                }

            }
            else
            {
                result.Add(0);
                result.Add("验证码错误！");

            }
            return result;
        }

        public staff GetStaffMsg(long id)
        {
            return base.Search(d => d.staff_Id == id && d.staff_IsDel == false)[0];
        }

        public List<object> ModifyDivisional(string id, string OwnId, string name, string changeName)
        {
            List<object> result = new List<object>();
            if (!String.IsNullOrEmpty(name) && name.Length <= 25)
            {
                Staff S = new Staff();
                List<staff> staffs = S.Search(d => d.company.company_Id == OwnId && d.staff_Department == changeName && d.staff_IsDel == false);
                if (staffs.Count != 0)
                {
                    foreach (staff s in staffs)
                    {
                        s.staff_Department = name;
                        S.Modify(s, "staff_Department");
                    }
                    result.Add(1);
                    result.Add("更改成功！");
                }
                else
                {
                    result.Add(0);
                    result.Add("更改失败！部门数据错误......");
                }
            }
            else
            {
                result.Add(0);
                result.Add("更改失败！数据不合法.....");
            }
            return result;
        }

        public List<object> RemoveDivisional(string id, string OwnId, string remove)
        {
            List<object> result = new List<object>();
            if (new Companyhandling().GetCompnayStatus(id) != 1)
            {
                Staff S = new Staff();
                List<staff> staffs = S.Search(d => d.company.company_Id == OwnId && d.staff_Department == remove && d.staff_IsDel == false);
                if (staffs.Count != 0)
                {
                    Key K = new Key();
                    try
                    {
                        foreach (staff s in staffs)
                        {
                            s.staff_IsDel = true;
                            S.Modify(s, "staff_IsDel");
                            List<key> keys = K.Search(d => d.key_StaffId == s.staff_Id);
                            foreach (key k in keys)
                            {
                                k.key_IsDel = true;
                                K.Modify(k, "key_IsDel");
                            }
                        }
                        result.Add(1);
                        result.Add("删除成功！");
                    }
                    catch
                    {
                        result.Add(0);
                        result.Add("删除失败！未知错误.....");
                    }
                }
                else
                {
                    result.Add(0);
                    result.Add("删除失败！数据错误......");
                }
            }
            else
            {
                result.Add(0);
                result.Add("删除失败！公司正在进行调查无法删除......");
            }
            return result;
        }

        public List<staff> GetStaffByDepartment(string ownId, string dep)
        {
            return base.Search(d => d.staff_OwnCompanyId == ownId && d.staff_IsDel == false && d.staff_Department == dep);
        }

        public List<object> ModifyPassword(long id, string name, string email, string pwd)
        {
            List<object> reslut = new List<object>();
            Staff S= new Staff();
            staff s =S.Search(d => d.staff_Id == id && d.staff_IsDel == false)[0];
            if (s.staff_TrueName == name && s.staff_Email == email)
            {
                if (pwd != "123456")
                {
                    if (!String.IsNullOrEmpty(pwd) && CheckDataOperation.CheckData(pwd, @"^[A-Za-z0-9]{0,25}$"))
                    {
                        s.staff_Password = Md5.GetMd5Word(pwd);
                        S.Modify(s, "staff_Password");
                        reslut.Add(1);
                    }
                    else
                    {
                        reslut.Add(0);
                        reslut.Add("密码格式错误！请输入小于25个非空字母或数字.....");
                    }
                }
                else
                {
                    reslut.Add(0);
                    reslut.Add("密码错误！请输入非原始密码.....");
                }
            }
            else
            {
                reslut.Add(0);
                reslut.Add("信息错误！请核对你的信息.....");
            }
            return reslut;
        }

        public void RecordAnswers(long id,string datas)
        {
            Staff S = new Staff();
            staff s=S.Search(d => d.staff_Id == id && d.staff_IsDel == false)[0];
            s.staff_Spare = datas;
            S.Modify(s, "staff_Spare");
        }

        public List<object> ResetPassword(string datas)
        {
            string[] data = datas.Split(',');
            List<object> result = new List<object>();
            try
            {
                foreach (string str in data)
                {
                    long id = Convert.ToInt64(str);
                    Staff S=new Staff();
                    staff s = S.Search(d => d.staff_Id == id && d.staff_IsDel == false)[0];
                    s.staff_Password =Md5.GetMd5Word("123456");
                    S.Modify(s, "staff_Password");
                }
                result.Add(1);
                result.Add("修改成功！");
            }
            catch
            {
                result.Add(0);
                result.Add("修改失败！未知错误....");
            }
            return result;
        }

        #region 生成随机数
        public string GetChar(Random rnd)
        {
            // a-z  ASCII值  97-122
            int i = rnd.Next(97, 123);


            char c = (char)i;

            return char.IsLower(c) ? c.ToString() : GetChar(rnd);
        }
        #endregion
    }
}
