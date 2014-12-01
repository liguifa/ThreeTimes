using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class QuestionsHanding : BaseBLL<question>
    {
        public string DelQuestions(string datas)
        {

            string[] questionId = datas.Split('#');

            for (int i = 0; i < questionId.Length; i++)
            {
                long id = Convert.ToInt64(questionId[i]);
                List<question> questions = Search(u => u.question_Id == id);
                questions[0].question_IsDel = true;
                Modify(questions[0], "question_IsDel");
            }

            return "ok";
        }
        public string AddQuestion(string datas)
        {
            string[] data = datas.Split('#');
            string que = data[0];
            string qClass = data[1];
            for (int i = 2; i < data.Length; i++)
            {
                if (data[i] == "undefined")
                {
                    data[i] = "";
                }
            }
            List<@class> classes = new _Class().Search(u => u.class_Class == qClass);
            if (classes.Count <= 0)
            {
                return "您所输入的问题类别不存在！";
            }
            List<question> questions = Search(u => u.qusetion_Stem == que);
            if (questions.Count > 0)
            {
                questions[0].question_ClassId = classes[0].class_Id;
                questions[0].question_IsDel = false;
                questions[0].question_Option1 = data[2];
                questions[0].question_Option2 = data[3];
                questions[0].question_Option3 = data[4];
                questions[0].question_Option4 = data[5];
                questions[0].question_Option5 = data[6];
                questions[0].question_Option6 = data[7];
                questions[0].question_Option7 = data[8];
                Modify(questions[0], new string[] { "question_ClassId", "question_IsDel", "question_Option1", "question_Option2", "question_Option3", "question_Option4", "question_Option5", "question_Option6", "question_Option7" });
                return "ok";
            }
            question q = new question();
            q.qusetion_Stem = que;
            q.question_ClassId = classes[0].class_Id;
            q.question_Option1 = data[2];
            q.question_Option2 = data[3];
            q.question_Option3 = data[4];
            q.question_Option4 = data[5];
            q.question_Option5 = data[6];
            q.question_Option6 = data[7];
            q.question_Option7 = data[8];
            Add(q);

            return "ok";
        }
        public string Modify(string datas)
       {

           string[] data = datas.Split('#');
            long id=Convert.ToInt64(data[0]);
              string qClass=data[2];
            List<@class> classes = new _Class().Search(u => u.class_Class == qClass);
           if (classes.Count <= 0)
           {
               return "您所输入的问题类别不存在！";
           }
            question q = Search(u => u.question_Id == id)[0];
            q.qusetion_Stem = data[1];
            q.question_ClassId = classes[0].class_Id;
            q.question_Option1 = data[3];
            q.question_Option2 = data[4];
            q.question_Option3 = data[5];
            q.question_Option4 = data[6];
            q.question_Option5 = data[7];
            q.question_Option6 = data[8];
            q.question_Option7 = data[9];
            Modify(q, new string[] { "qusetion_Stem", "question_ClassId", "question_Option1", "question_Option2", "question_Option3", "question_Option4", "question_Option5", "question_Option6", "question_Option7" });
           return "ok";
       }
    }
}
