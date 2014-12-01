using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class Companyhandling : BaseBLL<company>
    {
        #region 判断编号为ID的总公司是否存在  public bool IsCompanyIdExist(string id)
        /// <summary>
        /// 判断编号为ID的公司是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsheadCompanyIdExist(string id)
        {

            return base.SearchCount(u => u.company_Id == id && u.company_IsDel == false) == 1 && id != "0";

        }
        #endregion
        #region 判断编号为id的公司是否存在
        /// <summary>
        /// 判断编号为id的公司是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsCompanyIdExist(string id)
        {

            return base.SearchCount(u => u.company_Id == id && u.company_IsDel == false) == 1 && id != "0";

        }

        #endregion
        #region 根据Id查询出公司public company SearchCompany(string id)
        public company SearchCompany(string id)
        {
            List<company> coms = Search(u => u.company_Id == id && u.company_IsDel == false);

            return coms[0];

        }
        #endregion

        #region 返回编号为Id的总公司下的所有公司列表public List<company> SearchSonCompany(string id)
        /// <summary>
        /// 返回编号为Id的总公司下的所有公司列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<company> SearchSonCompany(string id)
        {
            List<company> targetCompany = Search(u => u.company_Id == id && u.company_IsDel == false);
            List<company> waitCompany = new List<company>();
            List<company> sonCom = Search(u => u.company_OwnerCompany == id && u.company_IsDel == false);

            foreach (company c in sonCom)
            {
                targetCompany.Add(c);
                if (c.company_IsSonIn == true)
                {
                    waitCompany.Add(c);
                }
            }
            while (waitCompany.Count != 0)
            {
                company com = waitCompany[0];
                List<company> c = Search(u => u.company_OwnerCompany == com.company_Id && u.company_IsDel == false);
                foreach (company s in c)
                {
                    targetCompany.Add(s);
                    if (s.company_IsSonIn == true)
                    {
                        waitCompany.Add(s);
                    }
                }


                waitCompany.Remove(waitCompany[0]);
            }

            return targetCompany;
        }

        #endregion

        #region 修改公司信息 public List<object> ModifyCompany(string owecompanyid, string id, string name, string province, string trade)
        public List<object> ModifyCompany(string owecompanyid, string id, string name, string province, string trade)
        {
            List<object> result = new List<object>();
            if (!(String.IsNullOrEmpty(id) || String.IsNullOrEmpty(name) || name.Length > 32 || province.Length > 12 || trade.Length > 12))
            {
                List<company> list = base.Search(d => d.company_Id == id || d.company_OwnerCompany == "0" && d.company_OwnerCompany == owecompanyid && d.company_IsDel == false);
                if (list.Count() == 1)
                {
                    company com = list[0];
                    com.company_Name = name;
                    com.company_Province = province;
                    com.company_Trade = trade;
                    base.Modify(com, new string[] { "company_Name", "company_Trade", "company_Province" });
                    result.Add(1);
                    result.Add("保存成功！");


                }
                else
                {
                    result.Add(0);
                    result.Add("修改失败！公司不存在.....");
                }
            }
            else
            {
                result.Add(0);
                result.Add("修改失败！数据非法.....");
            }
            return result;
        }
        #endregion

        public List<object> RemoveCompany(string id, string owecompanyid)
        {
            List<object> result = new List<object>();
            if (new Companyhandling().GetCompnayStatus(id) != 1)
            {
                if (base.SearchCount(d => d.company_Id == id && d.company_IsDel == false) != 0)
                {

                    List<company> list = base.Search(d => d.company_IsDel == false && (d.company_Id == id || d.company_OwnerCompany == id));
                    foreach (company com in list)
                    {
                        if (com.company_Id == id)
                        {
                            if (base.SearchCount(d => d.company_OwnerCompany == com.company_OwnerCompany && d.company_IsDel == false) == 1 && base.SearchCount(d => d.company_Id == com.company_OwnerCompany && d.company_IsDel == false) != 0)
                            {
                                company c = base.Search(d => d.company_Id == com.company_OwnerCompany && d.company_IsDel == false)[0];
                                c.company_IsSonIn = false;
                                base.Modify(c, "company_IsSonIn");
                            }
                        }
                        com.company_IsDel = true;
                        com.company_IsSonIn = false;
                        base.Modify(com, new string[] { "company_IsDel", "company_IsSonIn" });
                        Staff S = new Staff();
                        List<staff> sf = S.Search(d => d.staff_IsDel == false && d.staff_OwnCompanyId == com.company_Id);
                        foreach (staff s in sf)
                        {
                            s.staff_IsDel = true;
                            S.Modify(s, "staff_IsDel");
                            Key K = new Key();
                            List<key> k = K.Search(d => d.key_StaffId == s.staff_Id && d.key_IsDel == false);
                            foreach (key ky in k)
                            {
                                ky.key_IsDel = true;
                                K.Modify(ky, "key_IsDel");
                            }
                        }
                    }
                    result.Add(1);
                    result.Add("删除成功！");
                }
                else
                {
                    result.Add(0);
                    result.Add("删除失败！公司不存在.....");
                }
            }
            else
            {
                result.Add(0);
                result.Add("公司正在调查中，不能删除！");
            }
            return result;
        }

        public List<object> AddCompany(string id, string parentid, string name, string province, string trade)
        {
            List<object> result = new List<object>();
            Regex r = new Regex("^[A-Za-z0-9]{0,25}$");
            if (String.IsNullOrEmpty(province))
            {
                province = "未知";
            }
            if (String.IsNullOrEmpty(trade))
            {
                trade = "未知";
            }
            if ((!String.IsNullOrEmpty(parentid)) && (!String.IsNullOrEmpty(name)) && (!String.IsNullOrEmpty(trade)) && (r.Match(id).Value == id && name.Length <= 32 && province.Length <= 12 && trade.Length <= 12))
            {
                if (base.SearchCount(d => d.company_Id == parentid && d.company_IsDel == false) != 0)
                {
                    if (base.SearchCount(d => d.company_Id == id && d.company_IsDel == false) == 0)
                    {
                        try
                        {
                            company parentcompany = base.Search(d => d.company_Id == parentid && d.company_IsDel == false)[0];
                            company c = new company()
                            {
                                company_Id = id,
                                company_IsDel = false,
                                company_IsSonIn = false,
                                company_Level = parentcompany.company_Level + 1,
                                company_Name = name,
                                company_OwnerCompany = parentid,
                                company_Province = province,
                                company_Status = 0,
                                company_Trade = trade,
                            };
                            base.Add(c);
                            if (parentcompany.company_IsSonIn == false)
                            {
                                parentcompany.company_IsSonIn = true;
                                base.Modify(parentcompany, "company_IsSonIn");
                            }
                            result.Add(1);
                            result.Add("添加成功!");
                        }
                        catch (Exception ex)
                        {
                            result.Add(0);
                            result.Add("添加失败！系统未知错误....");
                        }
                    }
                    else
                    {
                        result.Add(0);
                        result.Add("添加失败！公司编号已存在....");
                    }
                }
                else
                {
                    result.Add(0);
                    result.Add("添加失败！父公司不存在....");
                }
            }
            else
            {
                result.Add(0);
                result.Add("添加失败！数据不合法....");
            }
            return result;
        }

        public string GetCompanyInspectionProcess(string id)
        {

            List<company> list = SearchSonCompany(id);
            string json = "{\"total\":9,\"rows\":["; ;
            int sf = 0;
            int st = 0;
            int staffnumber = 0;
            int startnumber = 0;
            int i = 1;
            int bh = 1;
            List<string> mark = new List<string>();
            mark.Add("0");
            mark.Add("0");
            while (list.Count != 0)
            {
                List<company> temp = new List<company>();
                foreach (company com in list)
                {
                    if (com.company_Level == i && (com.company_OwnerCompany == mark[i] || i == 1))
                    {
                        temp.Add(com);
                    }
                }
                if (temp.Count != 0)
                {
                    list.Remove(temp[0]);
                    sf = temp[0].staffs.Count(d => d.staff_IsDel == false);
                    st = temp[0].staffs.Count(d => d.staff_IsDel == false && d.staff_IsWrite == true);
                    json += "{\"Id\":\"" + (bh++) + "\",";
                    json += "\"iconCls\":\"icon-company_company\",";
                    json += "\"id\":\"" + temp[0].company_Id + "\",";
                    json += "\"name\":\"" + temp[0].company_Name + "\",";
                    json += "\"staffnumber\":\"" + sf + "\",";
                    json += "\"startnumber\":\"" + st + "\",";
                    int progress = (int)(st == 0 ? 0 : st / (sf / 1.0) * 100 + 0.5);
                    json += "\"progress\":\"" + progress + "\"";
                    staffnumber += sf;
                    startnumber += st;

                    List<string> departments = temp[0].staffs.Where(d => d.staff_IsDel == false).Select(d => d.staff_Department).Distinct().ToList();
                    if (departments.Count != 0)
                    {
                        json = json + ",\"children\":[";
                    }
                    foreach (string department in departments)
                    {

                        sf = temp[0].staffs.Count(d => d.staff_IsDel == false && d.staff_Department == department);
                        st = temp[0].staffs.Count(d => d.staff_IsDel == false && d.staff_IsWrite == true && d.staff_Department == department);
                        json += "{\"Id\":\"" + (bh++) + "\",";
                        json += "\"iconCls\":\"icon-blank\",";
                        json += "\"name\":\"\",";
                        json += "\"dep\":\"" + department + "\",";
                        json += "\"staffnumber\":\"" + temp[0].staffs.Count(d => d.staff_IsDel == false && d.staff_Department == department) + "\",";
                        progress = (int)(st == 0 ? 0 : st / (sf / 1.0) * 100 + 0.5);
                        json += "\"progress\":\"" + progress + "\",";
                        json += "\"startnumber\":\"" + temp[0].staffs.Count(d => d.staff_IsDel == false && d.staff_IsWrite == true && d.staff_Department == department) + "\"},";
                    }
                    if (departments.Count != 0)
                    {
                        if (temp[0].company_IsSonIn)
                        {

                            i++;
                            mark.Add(temp[0].company_Id);
                        }
                        else
                        {
                            if (temp.Count == 1)
                            {
                                json = json.Substring(0, json.Length - 1);
                                json = json + "]}]},";
                                mark.Remove(mark[i]);
                                i--;
                            }
                            else
                            {
                                json = json.Substring(0, json.Length - 1) + "]},";
                            }
                        }
                    }
                    else
                    {
                        if (temp[0].company_IsSonIn)
                        {
                            json = json + ",\"children\":[";
                            i++;
                            mark.Add(temp[0].company_Id);
                        }
                        else
                        {
                            if (temp.Count == 1)
                            {
                                json = json + "}]},";
                                mark.Remove(mark[i]);
                                i--;
                            }
                            else
                            {
                                json = json + "},";
                            }
                        }
                    }

                }
                else
                {
                    json = json.Substring(0, json.Length - 1);
                    json = json + "]},";
                    mark.Remove(mark[i]);
                    i--;
                }
            }
            json = json.Substring(0, json.Length - 2);
            for (int t = i; t > 0; t--)
            {
                json = json + "}]";
            }
            json = json + ",";
            int p = (int)(startnumber == 0 ? 0 : startnumber / (staffnumber / 1.0) * 100 + 0.5);
            json += "\"footer\":[{\"name\":\"总进度\",\"iconCls\":\"icon-progress\",\"staffnumber\":\"" + staffnumber + "\",\"startnumber\":\"" + startnumber + "\",\"progress\":\"" + p + "\"}]}";
            return json;
        }

        public List<object> SetSurveyStatus(string id, string status)
        {
            List<object> result = new List<object>();
            if (base.SearchCount(d => d.company_IsDel == false && d.company_Id == id) != 0)
            {
                company com = base.Search(d => d.company_IsDel == false && d.company_Id == id)[0];
                try
                {
                    com.company_Status = Convert.ToInt32(status);
                    base.Modify(com, "company_Status");
                    result.Add(0);
                    result.Add("设置成功！");
                }
                catch
                {
                    result.Add(0);
                    result.Add("设置失败！数据不合法....");
                }

            }
            else
            {
                result.Add(0);
                result.Add("设置失败！公司不存在....");
            }
            return result;
        }

        public string GetOwnCompanyName(string id)
        {
            return base.Search(d => d.company_Id == id && d.company_IsDel == false)[0].company_Name;
        }
        public string DepartmentTreeGridJson(string id)
        {
            List<company> list = SearchSonCompany(id);
            string json = "[";

            int i = 1;
            int bh = 1;
            List<string> mark = new List<string>();
            mark.Add("0");
            mark.Add("0");
            while (list.Count != 0)
            {
                List<company> temp = new List<company>();
                foreach (company com in list)
                {
                    if (com.company_Level == i && (com.company_OwnerCompany == mark[i] || i == 1))
                    {
                        temp.Add(com);
                    }
                }
                if (temp.Count != 0)
                {
                    list.Remove(temp[0]);
                    json += "{\"Id\":\"" + (bh++) + "\",";
                    json += "\"iconCls\":\"icon-company_company\",";
                    json += "\"id\":\"" + temp[0].company_Id + "\",";
                    json += "\"name\":\"" + temp[0].company_Name + "\",";
                    json += "\"staffnumber\":\"" + temp[0].staffs.Count(d => d.staff_IsDel == false) + "\"";
                    List<string> departments = temp[0].staffs.Where(d => d.staff_IsDel == false).Select(d => d.staff_Department).Distinct().ToList();
                    if (departments.Count != 0)
                    {
                        json = json + ",\"children\":[";
                    }
                    foreach (string department in departments)
                    {

                        json += "{\"Id\":\"" + (bh++) + "\",";
                        json += "\"isOwnId\":\"" + temp[0].company_Id + "\",";
                        json += "\"iconCls\":\"icon-blank\",";
                        json += "\"name\":\"\",";
                        json += "\"isDep\":\"1\",";
                        json += "\"dep\":\"" + department + "\",";
                        json += "\"staffnumber\":\"" + temp[0].staffs.Count(d => d.staff_IsDel == false && d.staff_Department == department) + "\"},";
                    }
                    if (departments.Count != 0)
                    {
                        if (temp[0].company_IsSonIn)
                        {

                            i++;
                            mark.Add(temp[0].company_Id);
                        }
                        else
                        {
                            if (temp.Count == 1)
                            {
                                json = json.Substring(0, json.Length - 1);
                                json = json + "]}]},";
                                mark.Remove(mark[i]);
                                i--;
                            }
                            else
                            {
                                json = json.Substring(0, json.Length - 1) + "]},";
                            }
                        }
                    }
                    else
                    {
                        if (temp[0].company_IsSonIn)
                        {
                            json = json + ",\"children\":[";
                            i++;
                            mark.Add(temp[0].company_Id);
                        }
                        else
                        {
                            if (temp.Count == 1)
                            {
                                json = json + "}]},";
                                mark.Remove(mark[i]);
                                i--;
                            }
                            else
                            {
                                json = json + "},";
                            }
                        }
                    }

                }
                else
                {
                    json = json.Substring(0, json.Length - 1);
                    json = json + "]},";
                    mark.Remove(mark[i]);
                    i--;
                }
            }
            json = json.Substring(0, json.Length - 2);
            for (int t = i; t > 0; t--)
            {
                json = json + "}]";
            }
            return json;
        }

        public int GetCompnayStatus(string id)
        {
            company companys = new FindGGFatherCompanyByCompany().FindGGFatherCompany(base.Search(d => d.company_Id == id && d.company_IsDel == false)[0]);
            return companys.company_Status;
        }

        
    }

}
