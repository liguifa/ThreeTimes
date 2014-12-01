using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BLL
{
    public class KeyOperation : BaseBLL<key>
    {
        public List<object> SubmitKey(string datas, long id, long testInfoId)
        {
            List<object> result = new List<object>();
            try
            {
                string[] data = datas.Substring(0, datas.Length - 1).Split(',');
                string[] qId = new string[data.Length];
                string[] keys = new string[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    qId[i] = data[i].Split('=')[0];
                    keys[i] = data[i].Split('=')[1];
                }
                for (int i = 0; i <= data.Length; i++)
                {
                    for (int k = i + 1; k < data.Length; k++)
                    {
                        if (Convert.ToInt32(qId[i]) > Convert.ToInt32(qId[k]))
                        {
                            string nark = qId[i];
                            qId[i] = qId[k];
                            qId[k] = nark;
                            string temp = keys[i];
                            keys[i] = keys[k];
                            keys[k] = temp;
                        }
                    }
                }
                key ky = new key();
                Type myData = typeof(key);

                for (int i = 0; i < data.Length; i++)
                {
                    string value = "key_key" + (i + 1);
                    foreach (PropertyInfo info in myData.GetProperties())
                    {
                        if (info.Name == value)
                        {
                            info.SetValue(ky, keys[i]);
                        }
                    }
                }
                ky.key_StaffId = id;
                ky.key_TestInfoId = testInfoId;
                ky.key_IsDel = false;
                base.Add(ky);
                Staff sf = new Staff();
                staff s = sf.Search(d => d.staff_Id == id && d.staff_IsDel == false)[0];
                s.staff_IsWrite = true;
                sf.Modify(s,"staff_IsWrite");
                result.Add(1);
                result.Add("提交成功！谢谢参与");
            }
            catch
            {
                result.Add(0);
                result.Add("提交失败！系统未知错误.....");
            }
            return result;
        }
    }
}
