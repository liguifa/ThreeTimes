using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyManage.Controllers
{
    public class QuestionsController : Controller
    {
        //
        // GET: /Questions/

        public string DelQuestions()
        {
            string datas = Request["datas"];
            string json = new QuestionsHanding().DelQuestions(datas);
            return "{\"back\":\"" + json + "\"}";
        }
        public string AddQuestion()
        {
            string datas = Request["datas"];
            string json = new QuestionsHanding().AddQuestion(datas);
            return "{\"back\":\"" + json + "\"}";
        }
        public string Modify()
        {
            string datas = Request["datas"];
            string json = new QuestionsHanding().Modify(datas);
            return "{\"back\":\"" + json + "\"}";
        }
    }
}
