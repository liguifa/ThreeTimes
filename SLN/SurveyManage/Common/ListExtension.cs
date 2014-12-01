using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ListExtension
    {
        public static string ToJson<T>(this List<T> list)
        {
            int i = 0;
            string result = "{\"data\":[";
            foreach (var item in list)
            {
                result += ("{\"datakey" + "\":\"" + item + "\"},");
                i++;
            }
            if (i != 0)
            {
                result = result.Substring(0, result.Length - 1);
            }
            result += "],";
            result += ("\"count\":\"" + i + "\"");
            result += "}";
            return result;
        }

        public static string ToTreeGrid<T>(this List<T> list) where T : company
        {
            string json = "[";

            int i = 1;
            List<string> id = new List<string>();
            id.Add("0");
            id.Add("0");
            while (list.Count != 0)
            {
                List<company> temp = new List<company>();
                foreach (company com in list)
                {
                    if (com.company_Level == i && (com.company_OwnerCompany == id[i] || i == 1))
                    {
                        temp.Add(com);
                    }
                }
                if (temp.Count != 0)
                {
                    list.Remove(temp[0] as T);
                    int departmentnumber = temp[0].staffs.Where(d => d.staff_IsDel == false).Select(d => d.staff_Department).Distinct().Count();
                    //List<string> mark = new List<string>();
                    //foreach (staff s in temp[0].staffs)
                    //{
                    //    if ((!mark.Contains(s.staff_Department)) && s.staff_IsDel == false)
                    //    {
                    //        mark.Add(s.staff_Department);
                    //    }
                    //}
                    //int departmentnumber = mark.Count();
                    int staffnumber = temp[0].staffs.Count(u => u.staff_IsDel == false);

                    json = json + "{\"companyid\":" + "\"" + temp[0].company_Id + "\",";
                    json = json + "\"id\":\"" + temp[0].company_Id + "\",";
                    json = json + "\"text\":" + "\"" + temp[0].company_Name + "\",";
                    json = json + "\"iconCls\":\"icon-company_company\",";
                    json = json + "\"trade\":" + "\"" + temp[0].company_Trade + "\",";
                    json = json + "\"departmentnumber\":" + "\"" + departmentnumber + "\",";
                    json = json + "\"staffnumber\":" + "\"" + staffnumber + "\",";
                    json = json + "\"province\":" + "\"" + temp[0].company_Province + "\"";

                    if (temp[0].company_IsSonIn)
                    {
                        json = json + ",\"children\":[";
                        i++;
                        id.Add(temp[0].company_Id);
                    }
                    else
                    {
                        if (temp.Count == 1)
                        {
                            json = json + "}]},";
                            id.Remove(id[i]);
                            i--;
                        }
                        else
                        {
                            json = json + "},";
                        }
                    }
                }
                else
                {
                    json = json.Substring(0, json.Length - 1);
                    json = json + "]},";
                    id.Remove(id[i]);
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

        public static string ToKeyValue<T>(this List<T> list) where T : province
        {

            string result = "{\"data\":[";
            foreach (province item in list)
            {
                result += ("{\"value\":\"" + item.province_Name + "\",\"text\":\"" + item.province_Name + "\"},");
            }

            result = result.Substring(0, result.Length - 1) + "]}";

            return result;
        }
    }
}
