using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class QuestionHeading:BaseBLL<question>
    {
       /// <summary>
       /// 根据类别返回分页Json数据
       /// </summary>
       /// <param name="qclass"></param>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <returns></returns>
       public string GetQuestionsByClass(string qclass,int pageIndex,int pageSize)
       {
           long totalNum = 0;
           List<question> questions = new List<question>();
          
           if (qclass == "allClass")
           {
               totalNum = SearchCount(u => u.question_IsDel == false);
               questions= Search(k => k.question_IsDel == false, k => k.@class.class_Class, pageIndex, pageSize);
           }
           else{
               totalNum = SearchCount(u => u.question_IsDel == false&&u.@class.class_Class==qclass);
               questions = Search(k => k.question_IsDel == false && k.@class.class_Class==qclass, k => k.@class.class_Class, pageIndex, pageSize);
           }
           return ToJson(totalNum,questions);
           
       }
       public string ToJson(long totalNum ,List<question> questions)
       {
           StringBuilder sb = new StringBuilder();
           string s = "{\"total\":" + totalNum + " ,\"rows\":[";
           sb.Append(s);
           foreach (question q in questions)
           {
               s = "{\"id\":\"" + q.question_Id + "\",\"q\":\"" + q.qusetion_Stem + "\",\"qclass\":\"" + q.@class.class_Class + "\",\"o1\":\"" + q.question_Option1 + "\",\"o2\":\"" + q.question_Option2 + "\",\"o3\":\"" + q.question_Option3 + "\",\"o4\":\"" + q.question_Option4 + "\",\"o5\":\"" + q.question_Option5 + "\",\"o6\":\"" + q.question_Option6 + "\",\"o7\":\"" + q.question_Option7 + "\"},";
               sb.Append(s);
           }
           s = sb.ToString().Substring(0, sb.Length - 1);
           s += "]}";
           return s;
       }
      
    }
}
