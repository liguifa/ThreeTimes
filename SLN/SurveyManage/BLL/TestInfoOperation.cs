using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TestInfoOperation:BaseBLL<testInfo>
    {
        public List<testInfo> GetTestInfo(string id)
        {
            string parentId = new FindGGFatherCompanyByCompany().FindGGFatherCompany(new Company().Search(d => d.company_Id == id && d.company_IsDel == false)[0]).company_Id;
            return base.Search(d => d.testInfo_OwnComPanyId == parentId && d.testInfo_IsDel == false);
        }

        public List<object> SetTime(string id, string start, string end)
        {
            List<object> result = new List<object>();
            if (base.SearchCount(d => d.testInfo_OwnComPanyId == id && d.testInfo_IsDel == false) != 0)
            {
                try
                {
                    if ((Convert.ToDateTime(start).CompareTo(Convert.ToDateTime(end))) < 0 && Convert.ToDateTime(end).CompareTo(DateTime.Now) > 0)
                    {
                        try
                        {
                            testInfo ti = base.Search(d => d.testInfo_IsDel == false && d.testInfo_OwnComPanyId == id)[0];
                            ti.testInfo_StartTime = Convert.ToDateTime(start);
                            ti.testInfo_Deadline = Convert.ToDateTime(end);
                            base.Modify(ti, new string[2] { "testInfo_StartTime", "testInfo_Deadline" });
                            result.Add(1);
                            result.Add("设置成功!");
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
                        result.Add("设置失败！开始时间不应晚与结束时间且结束时间应晚与当前时间....");
                    }
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
    }
}
