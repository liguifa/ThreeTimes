using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SendEmailByListStaff
    {
        public string SendEmail(List<staff> staffs, string topComId)
        {
            SendEmailOperation seo = new SendEmailOperation();
            string email = null;
            string companyId = topComId;
            string companyName = null;
            string phone = null;
            string startTime = null;
            string deadline = null;
            string username = null;
            string password = null;
            TestInfo ti = new TestInfo();
            Admin a = new Admin();
            List<admin> admins = a.Search(u => u.admin_CompanyID == companyId && u.admin_IsDel == false);
            phone = admins[0].admin_PhoneNum;
            companyName = admins[0].company.company_Name;
            List<testInfo> testinfos = ti.Search(u => u.testInfo_OwnComPanyId == companyId && u.testInfo_IsDel == false);
            if (testinfos.Count <= 0)
            {
                return "请先在问卷管理中设置问卷信息，再执行！";
            }
            startTime = testinfos[0].testInfo_StartTime.ToString();
            deadline = testinfos[0].testInfo_Deadline.ToString();
            List<string> notSend = new List<string>();
            foreach (var s in staffs)
            {
                email = s.staff_Email;
                username = s.staff_Username;
                password = s.staff_Password;
                if (!seo.SendEmail(email, companyName, phone, startTime, deadline, username, "123456","staff"))
                {
                    notSend.Add(s.staff_TrueName);
                }

            }
            if (notSend.Count == 0)
            {
                return "ok";
            }
            else
            {
                return notSend.ToString();
            }
        }
    }
}
