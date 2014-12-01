using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class _Class : BaseBLL<@class>
    {
        public long totalNum { get; set; }
        public List<@class> GetQuestionClass(int pageIndex, int pageSize)
        {
            int totalPage = 0;
            totalNum = base.SearchCount(u => u.class_Id > 0);
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (totalNum % pageSize != 0)
            {
                totalPage = Convert.ToInt32(totalNum / Convert.ToInt64(pageSize)) + 1;
            }
            else
            {
                totalPage = Convert.ToInt32(totalNum / Convert.ToInt64(pageSize));
            }
            if (pageIndex > totalPage)
            {
                pageIndex = totalPage;
            }
            return base.Search(u => u.class_Id > 0, u => u.class_Id, pageIndex, pageSize).ToList();
        }

        public List<int> RemoveQuestionClass(params string[] datas)
        {
            List<int> dataId = new List<int>();
            foreach (string data in datas)
            {
                int id = Convert.ToInt32(data);

                if (new Question().Search(u => u.question_ClassId == id).Count == 0)
                {
                    List<@class> C = base.Search(u => u.class_Id == id);

                    if (C.Count == 1)
                    {
                        foreach (@class c in C)
                        {
                            try
                            {
                                base.Del(c);
                            }
                            catch (Exception ex)
                            {
                                //TODO
                            }

                        }
                    }
                }
                else
                {
                    dataId.Add(id);
                }

            }

            return dataId;
        }
        public List<object> AddQuestionClass(string data)
        {
            List<object> result = new List<object>();
            if (data.Length > 0 && data.Length < 12)
            {
                if (base.Search(u => u.class_Class == data).Count == 0)
                {
                    base.Add(new @class() { class_Class = data, class_Id = 1 });
                    List<@class> qc = base.Search(u => u.class_Class == data);
                    if (qc.Count == 1)
                    {
                        foreach (@class q in qc)
                        {
                            result.Add(q.class_Id);
                            result.Add("数据插入成功！");
                        }
                    }
                    else
                    {
                        result.Add(0);
                        result.Add("数据插入失败！未知错误....");
                    }
                }
                else
                {
                    result.Add(0);
                    result.Add("数据重复！");
                }
            }
            else
            {
                result.Add(0);
                result.Add("数据不合法！");
            }
            return result;
        }

        public List<object> ModifyQuestionClass(string datas_p,string dataIds_p)
        {
            string[] datas = datas_p.Split(',');
            string[] dataIds = dataIds_p.Split(',');
            List<object> result = new List<object>();
            if (datas.Length <= 0 || dataIds.Length <= 0)
            {
                result.Add(0);
                result.Add("要修改的数据为空！请检查数据...");
            }
            if(dataIds.Length!=dataIds.Length)
            {
                result.Add(0);
                result.Add("数据不合法！请检查数据...");
            }
            for(int i=0;i<datas.Length;i++)
            {
                int id=Convert.ToInt32(dataIds[i]);
                if (base.Search(u => u.class_Id == id).Count != 0)
                {
                    string data = datas[i];
                    if (base.Search(u => u.class_Class == data).Count == 0)
                    {
                        
                        base.Modify(new @class() { class_Id = id, class_Class = datas[i] }, new string[1] { "class_Class" });
                    }
                    else
                    {
                        result.Add(id);
                        result.Add("数据重复！请检查数据...");
                    }
                }
                else
                {
                    result.Add(id);
                    result.Add("数据不存在！请刷新...");
                }
            }
            return result;
        }
    }


}
