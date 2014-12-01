using Common;
using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Verification;

namespace BLL
{
    public class Admin : BaseBLL<admin>
    {
        public bool IsSuperAdmin(string id)
        {

            long adminId = Convert.ToInt64(id);
            return new Admin().Search(d => d.admin_Id == adminId && d.admin_IsDel == false)[0].admin_Power == 0;
        }

        public long totalNum { get; set; }
        public List<admin> GetPagedCompany(int pageNum, int pageSize, string companyclass)
        {
            if (pageSize < 0)
            {
                pageSize = 30;
            }
            int totalPage = 0;
            int status = -1; //调查状态 -1->全部  0->未开始  1->调查中  2->已结束
            switch (companyclass)
            {
                case "all": status = -1; break;
                case "notstart": status = 0; break;
                case "started": status = 1; break;
                case "finished": status = 2; break;
                default: status = -1; break;
            }

            if (status == -1)
            {
                totalNum = base.SearchCount(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1);
            }
            else
            {
                totalNum = base.SearchCount(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Status == status);
            }
            totalPage = Convert.ToInt32(totalNum / pageSize);
            if (totalNum % pageSize > 0)
            {
                totalPage++;
            }
            if (pageNum <= 0)
            {
                pageNum = 1;
            }
            else if (pageNum > totalPage && totalPage != 0)
            {
                pageNum = totalPage;
            }
            if (status == -1)
            {
                return Search(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1, u => u.company.company_Id, pageNum, pageSize).ToList();
            }
            else
            {
                return Search(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Status == status, u => u.company.company_Id, pageNum, pageSize).ToList();
            }
        }
        public List<admin> GetPagedAdmins(int pageIndex, int pageSize, out long total)
        {
            if (pageSize < 0)
            {
                pageSize = 30;
            }
            int totalPage = 0;

            totalNum = base.SearchCount(u => u.admin_IsDel == false && (u.admin_Power == 1 || u.admin_Power == 0));
            totalPage = Convert.ToInt32(totalNum / pageSize);

            if (totalNum % pageSize > 0)
            {
                totalPage++;
            }
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            else if ((pageIndex > totalPage) && totalPage != 0)
            {
                pageIndex = totalPage;
            }
            total = totalNum;
            return Search(u => u.admin_IsDel == false && (u.admin_Power == 1 || u.admin_Power == 0), u => u.admin_Id, pageIndex, pageSize);
        }

        public List<admin> SearchCompany(int pageNum, int pageSize, string value, string name, string companyclass)
        {
            if (pageSize < 0)
            {
                pageSize = 30;
            }
            int totalPage = 0;
            int status = -1; //调查状态 -1->全部  0->未开始  1->调查中  2->已结束
            switch (companyclass)
            {
                case "all": status = -1; break;
                case "notstart": status = 0; break;
                case "started": status = 1; break;
                case "finished": status = 2; break;
                default: status = -1; break;
            }
            if (name == "id")
            {
                if (status == -1)
                {
                    totalNum = base.SearchCount(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Id == value);
                }
                else
                {
                    totalNum = base.SearchCount(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Status == status && u.company.company_Id == value);
                }
            }
            else
            {
                if (status == -1)
                {
                    totalNum = base.SearchCount(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Name.Contains(value));
                }
                else
                {
                    totalNum = base.SearchCount(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Status == status && u.company.company_Name.Contains(value));
                }
            }
            totalPage = Convert.ToInt32(totalNum / pageSize);
            if (totalNum % pageSize > 0)
            {
                totalPage++;
            }
            if (pageNum <= 0)
            {
                pageNum = 1;
            }
            else if (pageNum > totalPage && totalPage != 0)
            {
                pageNum = totalPage;
            }
            if (name == "id")
            {
                if (status == -1)
                {
                    return Search(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Id == value, u => u.company.company_Id, pageNum, pageSize).ToList();
                }
                else
                {
                    return Search(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Status == status && u.company.company_Id == value, u => u.company.company_Id, pageNum, pageSize).ToList();
                }
            }
            else
            {
                if (status == -1)
                {
                    return Search(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Name.Contains(value), u => u.company.company_Id, pageNum, pageSize).ToList();
                }
                else
                {
                    return Search(u => u.admin_IsDel == false && u.admin_Power == 2 && u.company.company_IsDel == false && u.company.company_Level == 1 && u.company.company_Status == status && u.company.company_Name.Contains(value), u => u.company.company_Id, pageNum, pageSize).ToList();
                }
            }
        }

        public List<object> AddCompany(string id, string name, string profession, string province, string user, string email, string phone)
        {
            List<object> result = new List<object>();
            if (!(String.IsNullOrEmpty(id) && String.IsNullOrEmpty(name) && String.IsNullOrEmpty(user) && String.IsNullOrEmpty(email) && String.IsNullOrEmpty(phone)))
            {
                if (CheckDataOperation.CheckData(id, @"^[A-Za-z0-9]{0,25}$") && CheckDataOperation.CheckData(email, @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$") && CheckDataOperation.CheckData(phone, @"^[0-9]{7,11}$"))
                {
                    Company cy = new Company();
                    if (cy.SearchCount(u => u.company_Id == id) == 0)
                    {
                        try
                        {
                            Random r = new Random();
                            admin person = new admin()
                            {
                                admin_Id = 0,
                                admin_Email = email,
                                admin_Username = id + r.Next(10000),
                                admin_PhoneNum = phone,
                                admin_TrueName = user,
                                admin_CompanyID = id,
                                admin_Password = r.Next(10000).ToString(),
                                admin_IsDel = false,
                                admin_Power = 2,
                            };
                            company com = new company()
                            {
                                company_Id = id,
                                company_IsSonIn = false,
                                company_Level = 1,
                                company_Name = name,
                                company_OwnerCompany = "0",
                                company_IsDel = false,
                                company_Province = province,
                                company_Status = 0,
                                company_Trade = profession
                            };

                            cy.Add(com);
                            base.Add(person);
                            result.Add(1);
                            result.Add("添加成功！");
                        }
                        catch (Exception ex)
                        {
                            result.Add(0);
                            result.Add("添加失败,系统未知错误!");
                        }
                    }
                    else
                    {
                        result.Add(0);
                        result.Add("添加失败,公司编号重复!");
                    }
                }
                else
                {
                    result.Add(0);
                    result.Add("添加失败,数据错误!");
                }

            }
            else
            {
                result.Add(0);
                result.Add("添加失败,数据为空!");
            }
            return result;

        }


        public List<object> ModifyCompany(string id, string companyName, string trueName, string userName, string password, string email, string phone, string privor, string sf)
        {
            List<object> result = new List<object>();
            if (base.SearchCount(d => d.admin_CompanyID == id && d.admin_IsDel == false) != 0)
            {
                if (companyName.Length <= 32 && trueName.Length <= 10 && CheckDataOperation.CheckData(userName, @"^[A-Za-z0-9]{0,25}$") && CheckDataOperation.CheckData(password, @"^[A-Za-z0-9]{0,25}$") && CheckDataOperation.CheckData(email, @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$") && CheckDataOperation.CheckData(phone, @"^[0-9]{7,11}$"))
                {
                    try
                    {
                        admin a = base.Search(d => d.admin_CompanyID == id && d.admin_IsDel == false)[0];
                        a.admin_Email = email;
                        a.admin_Password = password;
                        a.admin_PhoneNum = phone;
                        a.admin_TrueName = trueName;
                        a.admin_Username = userName;
                        base.Modify(a, new string[] { "admin_Email", "admin_Password", "admin_PhoneNum", "admin_TrueName", "admin_Username", });
                        Company cy = new Company();
                        company c = cy.Search(d => d.company_Id == id && d.company_IsDel == false)[0];
                        c.company_Name = companyName;
                        c.company_Province = sf;
                        c.company_Trade = privor;
                        cy.Modify(c, new string[] { "company_Name", "company_Province", "company_Trade" });
                        result.Add(0);
                        result.Add("保存成功！");

                    }
                    catch
                    {
                        result.Add(0);
                        result.Add("保存失败！数据不合法.....");
                    }
                }
                else
                {
                    result.Add(0);
                    result.Add("保存失败！数据不合法.....");
                }
            }
            else
            {
                result.Add(0);
                result.Add("保存失败！公司不存在.....");
            }
            return result;
        }

        public List<object> RemoveCompany(string id)
        {
            List<object> result = new List<object>();
            if (new Companyhandling().GetCompnayStatus(id) != 1)
            {
                if (base.SearchCount(d => d.admin_CompanyID == id && d.admin_IsDel == false) != 0)
                {
                    try
                    {
                        admin a = base.Search(d => d.admin_CompanyID == id && d.admin_IsDel == false)[0];
                        a.admin_IsDel = true;
                        new TestInfoHanding().DeleteTestInfoAddAllTest(id);
                        base.Modify(a, "admin_IsDel");
                        new Companyhandling().RemoveCompany(id, "0");
                       
                        result.Add(1);
                        result.Add("删除成功！");
                    }
                    catch
                    {
                        result.Add(0);
                        result.Add("删除失败！系统未知错误....");
                    }
                }
                else
                {
                    result.Add(0);
                    result.Add("删除失败！公司不存在....");
                }
            }
            else
            {
                result.Add(0);
                result.Add("公司正在调查中，不能删除！");
            }
            return result;
        }

        public List<object> SendEmail(string id)
        {
            List<object> result = new List<object>();
            if (base.SearchCount(d => d.admin_CompanyID == id && d.admin_IsDel == false) != 0)
            {
                admin a = base.Search(d => d.admin_IsDel == false && d.admin_CompanyID == id)[0];
                if (new SendEmailOperation().SendEmail(a.admin_Email, a.company.company_Name, null, null, null, a.admin_Username, a.admin_Password, "admin"))
                {
                    result.Add(1);
                    result.Add("发送成功!");
                }
                else
                {
                    result.Add(0);
                    result.Add("发送失败！");
                }
            }
            else
            {
                result.Add(0);
                result.Add("发送失败！公司不存在.....");
            }
            return result;
        }

        public List<object> AdminLogin(string user, string pwd, string yzm, string systemYzm)
        {
            List<object> result = new List<object>();
            if (VerificationCode.ComparisonVerCode(yzm, systemYzm))
            {
                pwd = Md5.GetMd5Word(pwd);
                List<admin> admins = base.Search(d => d.admin_Username == user && d.admin_Password == pwd && d.admin_IsDel == false && (d.admin_Power == 0 || d.admin_Power == 1));
                if (admins.Count != 0)
                {
                    result.Add(1);
                    result.Add(admins[0].admin_Id);
                }
                else
                {
                    result.Add(0);
                    result.Add("用户名或密码错误！");
                }
            }
            else
            {
                result.Add(0);
                result.Add("验证码错误！");
            }
            return result;

        }

        public List<object> HrLogin(string user, string pwd, string yzm, string systemYzm)
        {
            List<object> result = new List<object>();
            if (VerificationCode.ComparisonVerCode(yzm, systemYzm))
            {
                List<admin> admins = base.Search(d => d.admin_Username == user && d.admin_Password == pwd && d.admin_IsDel == false && d.admin_Power == 2);
                if (admins.Count != 0)
                {
                    result.Add(1);
                    result.Add(admins[0].admin_Id);
                }
                else
                {
                    result.Add(0);
                    result.Add("用户名或密码错误！");
                }
            }
            else
            {
                result.Add(0);
                result.Add("验证码错误！");
            }
            return result;
        }

        public admin GetAdminMsg(long id)
        {
            return base.Search(d => d.admin_Id == id && d.admin_IsDel == false && d.admin_Power == 2)[0];
        }

        public List<object> ChangePassword(long id, string pwd, string nPwd, string rePwd)
        {
            List<object> reslut = new List<object>();
            admin a = new Admin().Search(d => d.admin_Id == id && d.admin_IsDel == false)[0];
            if (a.admin_Password == pwd)
            {
                if (nPwd == rePwd)
                {
                    if (CheckDataOperation.CheckData(nPwd, @"^[A-Za-z0-9]{0,25}$"))
                    {
                        a.admin_Password = Md5.GetMd5Word(nPwd);
                        base.Modify(a, "admin_Password");
                        reslut.Add(1);
                        reslut.Add("密码成功!");
                    }
                    else
                    {
                        reslut.Add(0);
                        reslut.Add("密码修改失败！密码格式不合法.....");
                    }
                }
                else
                {
                    reslut.Add(0);
                    reslut.Add("密码修改失败！二次输入不一至.....");
                }
            }
            else
            {
                reslut.Add(0);
                reslut.Add("密码修改失败！旧密码错误.....");
            }

            return reslut;
        }

        public List<object> ResetPassword(string id, string mark)
        {
            List<object> result = new List<object>();
            try
            {
                admin a = new admin();
                if (mark == "admin")
                {
                    a = base.Search(d => d.admin_CompanyID == id && d.admin_IsDel == false)[0];
                    a.admin_Password = new Random().Next(10000).ToString();
                }
                else
                {
                    long Id = Convert.ToInt64(id);
                    a = base.Search(d => d.admin_Id == Id && d.admin_IsDel == false)[0];
                    a.admin_Password = Md5.GetMd5Word("123456");
                }
                
                base.Modify(a, "admin_Password");
                result.Add(1);
                result.Add("重置成功！");
            }
            catch
            {
                result.Add(1);
                result.Add("重置失败！未知错误.....");
            }
            return result;
        }
    }

}
