using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public  class TestHanding:BaseBLL<test>
    {
       public int GetTestCount(long id)
       {
           return Convert.ToInt32(SearchCount(k => k.test_TestInfoId == id));
       }
       public List<test> GetTests(long id,int pageIndex,int PageSize)
       {
           if (pageIndex <= 0)
           {
               pageIndex = 1;
           }
           if (pageIndex >GetMaxPage(PageSize, (int)id))
           {
               pageIndex = GetMaxPage(PageSize, (int)id);
           }
           return Search(k => k.test_TestInfoId == id,k=>k.question.question_ClassId,pageIndex,PageSize);
       }
       public int GetTestClassCount(long testInfoId)
       {
           return new Test().Search(d => d.test_TestInfoId == testInfoId).Select(d => d.question.question_ClassId).Distinct().Count();
         
       }
       public int GetMaxPage(int pageSize, int id)
       {
           int dataSize = GetTestCount(id);
           return dataSize % pageSize == 0 ? dataSize / pageSize : dataSize / pageSize + 1;
       }
       public List<test> GetTestByComId(string id,int pageIndex,int pageSize,out long testInfoId)
       {
       
          testInfoId=  new TestInfo().Search(u => u.testInfo_OwnComPanyId == id && u.testInfo_IsDel == false)[0].testInfo_Id;
         List<test> tests= GetTests(testInfoId, pageIndex, pageSize);
         foreach (test t in tests)
         {
             t.test_Spare = t.question.@class.class_Class;
         }
         return tests;
       }
       public string DelTestQusetions(string cid,string data)
       {
           if (new Companyhandling().GetCompnayStatus(cid) == 1)
           {
               return "公司正在调查中，不能删除！";
           }
           string[] datas = data.Split(',');
           for (int i = 0; i < datas.Length; i++)
           {
               try { 
               long id=Convert.ToInt64(datas[i]);
               base.DelWhere(u => u.test_Id == id);
               }
               catch
               {
                   return "数据异常，请刷新！";
               }
           }
           return "ok";
       }
       public string AddTestQuestion(string data,long testInfoId)
       {
           string[] datas=data.Split('#');
           string stem=datas[0];
           Question Q = new Question();
           List<question> ques = Q.Search(u => u.qusetion_Stem == stem);
           string qclass = datas[1];
           if (ques.Count < 1)
           {
               //Create new 
               question q = new question();
            

             List<@class> c = new _Class().Search(u => u.class_Class == qclass);
             if (c.Count < 1)
             {
                 return "不存在次类别，添加失败！";
             }
               q.question_IsDel = false;
               q.qusetion_Stem = datas[0];
               q.question_ClassId = c[0].class_Id;
               q.question_Option1 = datas[2];
               q.question_Option2 = datas[3];
               q.question_Option3 = datas[4];
               q.question_Option4 = datas[5];
               q.question_Option5 = datas[6];
               q.question_Option6 = datas[7];
               q.question_Option7 = datas[8];
               try
               {
                   new Question().Add(q);
               }
               catch
               {
                   return "数据插入异常！";
               }
              
               test t = new test();
               t.test_QuestionId = q.question_Id;
               t.test_TestInfoId = testInfoId;
               try
               {
                   Add(t);
               }
               catch
               {
                   return "数据插入异常！";
               }
           }
           else
           {
               long qId = ques[0].question_Id;
             long tests = new Test().SearchCount(k => k.test_QuestionId == qId);
             if (tests > 1)
             {
                 return "此问卷中已包含此问题，请勿重复添加！";
             }
               List<@class> c = new _Class().Search(u => u.class_Class == qclass);
               if (c.Count < 1)
               {
                   return "不存在次类别，添加失败！";
               }
               test t = new test();
               t.test_QuestionId = ques[0].question_Id;
               t.test_TestInfoId = testInfoId;
              
               try
               {
                   Add(t);
               }
               catch
               {
                   return "数据插入异常！";
               }
           }
           return "ok";
       }
        public string AddQuestionTotest(long testInfoId,string datas)
       {
            string[] ids=datas.Split('#');
              test t = new test();
              t.test_TestInfoId = testInfoId;
           string s ="ok";
           for (int i = 0; i < ids.Length; i++)
			{
               long questionId=Convert.ToInt64(ids[i]);
               t.test_QuestionId = questionId;
               Test T = new Test();
              long count= T.SearchCount(u => u.test_QuestionId == questionId&&u.test_TestInfoId==testInfoId);
              if (count > 0)
              {
                  s = "您所选问题中含有问卷中已存在问题，已为您自动跳过。";
                  continue;
              }

                
               try
               {
                   T.Add(t);
               }
               catch
               {
                   return "添加错误，请重试！";
               }
			}
        
          
           return s;
       }
    }
}
