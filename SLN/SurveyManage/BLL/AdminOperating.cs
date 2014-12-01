using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AdminOperating
    {
        public bool DelAdminById(string id)
        {


            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            try
            {
                long Id = Convert.ToInt64(id);
                Admin a = new Admin();

                List<admin> delAdmins = a.Search(u => u.admin_Id == Id && u.admin_IsDel == false);

                delAdmins[0].admin_IsDel = true;

                a.Modify(delAdmins[0], "admin_IsDel");
                return true;
            }
            catch
            {
                return false;
            }

        }
        public string ModifyAdminById(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return "空数据，无法完成修改";
            }
            string[] str = data.Replace(" ", "").Split('+');

            try
            {
                long id = Convert.ToInt64(str[0]);
                Admin a = new Admin();
                List<admin> admins = a.Search(u => u.admin_Id == id && u.admin_IsDel == false);
                if (admins[0].admin_Username.Replace(" ", "") == str[1])
                {//未改变用户名,执行修改

                }
                else
                {//改变用户名了
                    string tempUsername = str[1];
                    //查询个数
                    long count = a.SearchCount(u => u.admin_Username == tempUsername && u.admin_IsDel == false);
                    if (count < 1)
                    {
                        //无此用户名，执行修改
                    }
                    else
                    {
                        return "此用户名已被使用，修改失败！";
                    }
                }
                admins[0].admin_Username = str[1];
               
                admins[0].admin_TrueName = str[3];
                admins[0].admin_Power = Convert.ToInt32(str[4]);
                admins[0].admin_PhoneNum = str[5];

                a.Modify(admins[0], new string[] { "admin_Username",  "admin_TrueName", "admin_Power", "admin_PhoneNum" });
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "OK";
        }
        public string AddAdmin(string data)
        {
            Admin ah = new Admin();


            string[] str = data.Replace(" ", "").Split('+');
            string tempU = str[0];
            long count = ah.SearchCount(u => u.admin_Username == tempU && u.admin_IsDel == false);
            if (count >= 1)
            {
                return "该用户已存在，请使用其他用户名.";
            }
            try
            {
                admin a = new admin();
                a.admin_Username = str[0];
                a.admin_CompanyID = "0";
                a.admin_Email = "";
                a.admin_IsDel = false;
                a.admin_Password = Md5.GetMd5Word("123456");
                a.admin_TrueName = str[2];
                a.admin_PhoneNum = str[4];
                a.admin_Power = Convert.ToInt32(str[3]);
                a.admin_Spare = null;
                ah.Add(a);
            }

            catch
            {
                return "非法的用户输入！";
            }
            return "OK";
        }
        public string InputChecker(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return "空数据，无法完成插入";
            }
            string[] str = data.Replace(" ", "").Split('+');
            for (int i = 0; i < str.Length; i++)
            {
                if (string.IsNullOrEmpty(str[i]))
                {
                    return "不允许有空值！";
                }
            }
            return "OK";
        }

       
    }

}
