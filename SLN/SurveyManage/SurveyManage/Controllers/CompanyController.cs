using BLL;
using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;

namespace SurveyManage.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/
        #region 暂时包起来吧

        public ActionResult Index(string id)
        {

            Companyhandling ch = new Companyhandling();
            //如果公司Id不存在

            if (!ch.IsheadCompanyIdExist(id))
            {
                ViewBag.id = id;
                return View("NotFind");
            }
            company com = ch.SearchCompany(id);
            ViewBag.company_Name = com.company_Name;
            ViewBag.id = id;
            ViewBag.companyStatus = new Companyhandling().GetCompnayStatus(id);
            return View();
        }

        public string GetTreeGridJson(string id)
        {

            Companyhandling ch = new Companyhandling();

            List<company> group = ch.SearchSonCompany(id);
            return group.ToTreeGrid();
        }
        public ActionResult CompanyTree(string id, string dataId)
        {

            ViewBag.id = dataId;
            ViewBag.tableId = dataId.Split('#')[1] + "_table";
            ViewBag.input = dataId.Split('#')[1] + "_input";
            ViewBag.submit = dataId.Split('#')[1] + "_submit";
            ViewBag.form = dataId.Split('#')[1] + "_form";
            ViewBag.iframe = dataId.Split('#')[1] + "_iframe";
            ViewBag.province = new ProvinceOperating().GetProvinceList().ToKeyValue();
            return PartialView();
        }

        public string ModifyCompany(string id, string comId, string newid, string name, string province, string trade)
        {
            return new Companyhandling().ModifyCompany(id, comId, name, province, trade).ToJson();
        }
        public string RemoveCompany(string id, string comId)
        {
            return new Companyhandling().RemoveCompany(comId, id).ToJson();
        }

        public string AddCompany(string id, string comId, string parentid, string name, string province, string trade)
        {
            return new Companyhandling().AddCompany(comId, parentid, name, province, trade).ToJson();
        }

        public string ImportStaff(string id)
        {
            HttpPostedFileBase table = Request.Files["table"];
            string filename = "~/Execl/" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx";
            filename = this.Server.MapPath(filename);
            table.SaveAs(filename);
            return new ImportExcel().ImportDataTable(true, filename, id).ToJson();
        }
        public string ImportCompany(string id)
        {
            HttpPostedFileBase table = Request.Files["table"];
            string filename = "~/Execl/" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx";
            filename = this.Server.MapPath(filename);
            table.SaveAs(filename);
            return new ImportExcel().ImportDataTable(false, filename, id).ToJson();
        }
        /// <summary>
        /// 员工管理
        /// </summary>
        /// <returns></returns>
        public ActionResult StaffManage(string id)
        {
            string comId = Request["parameter0"];
            int pageIndex = Convert.ToInt32(Request["parameter1"]);
            int pageSize = Convert.ToInt32(Request["parameter2"]);
            string pageClass = Request["parameter3"];
            long totalNum = 0;
            ViewBag.searchVal = "all";
            Companyhandling ch = new Companyhandling();
            //编号为COmid的公司不存在
            if (!ch.IsCompanyIdExist(comId))
            {
                return View("NotFind");
            }
            StaffHanding sh = new StaffHanding();
            //如果请求公司下属全部员工
            if (pageClass == "all")
            {

                List<staff> staffs = sh.SelectAllStaffByComID(comId, pageIndex, pageSize, out totalNum);
                ViewBag.staffs = staffs;

            }
            else if (pageClass == "search")
            {
                string name = Request["parameter4"];
                string value = Request["parameter5"];
                if (name == "trueName")
                {
                    //按名称搜，返回所有结果
                    List<staff> staffs = sh.SearchAllStaffsByValue(comId, value);
                    pageIndex = 1;

                    totalNum = staffs.Count;
                    ViewBag.staffs = staffs;

                }
                else if (name == "email")
                {
                    //按邮件搜,，返回所有结果
                    List<staff> staffs = sh.SearchAllStaffsByEmail(comId, value);
                    pageIndex = 1;
                    totalNum = staffs.Count;
                    ViewBag.staffs = staffs;

                }

            }
            else if (pageClass == "dep")
            {
                string value = Request["parameter4"];
                List<staff> staffs = sh.GetStaffByDepartment(comId, value);
                pageIndex = 1;

                totalNum = staffs.Count;
                ViewBag.staffs = staffs;
            }
            else
            {//请求本公司员工
                List<staff> staffs = sh.SelectStaffByComID(comId, pageIndex, pageSize, out totalNum);
                ViewBag.staffs = staffs;
                ViewBag.searchVal = "notAll";

            }
            ViewBag.pageIndex = pageIndex;

            ViewBag.pageSize = pageSize;

            ViewBag.totNum = totalNum;
            ViewBag.id = Request["dataId"].Split('#')[1];
            ViewBag.tableId = ViewBag.id + "1";
            ViewBag.ComId = comId;
            return PartialView();
        }

        public string StaffMangeBack(string id, string datas, string dataIds, string comId)
        {
            string mark = Request["mark"];//请求类型

            StaffHanding sh = new StaffHanding();
            staff s = new staff();
            JavaScriptSerializer ssj = new JavaScriptSerializer();
            JsonModel jm = new JsonModel();
            if (mark == "add")
            {


                jm.state = sh.AddStaff(datas, comId, out s);
                jm.datas = new string[] { s.staff_Id.ToString(), s.staff_TrueName, s.staff_Username, s.staff_Password, s.staff_Email, s.staff_Department, s.staff_IsWrite.ToString() };
                string json = ssj.Serialize(jm);
                return json;
            }
            else if (mark == "modify")
            {
                jm.state = sh.ModifyStaffs(datas, dataIds);
                return ssj.Serialize(jm);

            }
            else if (mark == "remove")
            {
                string[] ids = null;
                jm.state = sh.RemoveStaffs(id,datas, out ids);
                jm.datas = ids;
                return ssj.Serialize(jm);
            }
            return "{\"data\":\"未知错误\"}";
        }

        public ActionResult SurveyManage(string id, string dataId)
        {
            List<testInfo> list = new TestInfoOperation().GetTestInfo(id);
            ViewBag.tableid = dataId.Split('#')[1] + "_table";
            ViewBag.setid = dataId.Split('#')[1] + "_set";
            ViewBag.id = dataId;
            ViewBag.testInfo = list;
            ViewBag.isHaveTest = list.Count == 0 ? false : true;
            ViewBag.status = list.Count == 0 ? -1 : list[0].company.company_Status;
            return PartialView();
        }

        public ActionResult InspectionProcess(string id, string dataId)
        {
            ViewBag.id = dataId;
            ViewBag.tableid = dataId.Split('#')[1] + "_table";
            List<testInfo> list = new TestInfoOperation().GetTestInfo(id);
            ViewBag.isHaveTest = list.Count == 0 ? false : true;
            return PartialView();
        }

        public string SetTime(string id, string start, string end)
        {
            return new TestInfoOperation().SetTime(id, start, end).ToJson();
        }

        public string InspectionProcessTree(string id)
        {

            return new Companyhandling().GetCompanyInspectionProcess(id);
        }

        public ActionResult SendEmail(string id, string dataId)
        {
            ViewBag.id = dataId;
            ViewBag.tableid = dataId.Split('#')[1] + "_table";
            List<testInfo> list = new TestInfoOperation().GetTestInfo(id);
            ViewBag.isHaveTest = list.Count == 0 ? false : true;
            return PartialView();
        }

        public string SendEmailing(string id, string data)
        {
            StaffHanding sh = new StaffHanding();
            return sh.SendEmail(id).ToJson();
        }

        public string SetSurveyStatus(string id, string status)
        {
            return new Companyhandling().SetSurveyStatus(id, status).ToJson();
        }
        public string ReSendEmail(string id, string datas)
        {
            StaffHanding sh = new StaffHanding();


            return "{\"state\":\"" + sh.ReSendEmailBystaffsId(datas, id) + "\"}";
        }
        #endregion
        public ActionResult TestManage(string id, string dataId)
        {
            ViewBag.topCompanyId = id;
            ViewBag.id = dataId;
            ViewBag.a = dataId.Split('#')[1] + "a";
            ViewBag.tableId1 = dataId.Split('#')[1] + "table1";
            ViewBag.tableId2 = dataId.Split('#')[1] + "table2";
            ViewBag.tableId3 = dataId.Split('#')[1] + "table3";
            ViewBag.win01 = dataId.Split('#')[1] + "win01";
            ViewBag.win02 = dataId.Split('#')[1] + "win02";
            ViewBag.input01 = dataId.Split('#')[1] + "input01";
            ViewBag.input02 = dataId.Split('#')[1] + "input02";
            ViewBag.input03 = dataId.Split('#')[1] + "input03";
            ViewBag.input04 = dataId.Split('#')[1] + "input04";
            ViewBag.sub01 = dataId.Split('#')[1] + "sub01";
            ViewBag.sub02 = dataId.Split('#')[1] + "sub02";
            ViewBag.iframe = dataId.Split('#')[1] + "iframe";
            ViewBag.select = dataId.Split('#')[1] + "select";
            TestInfoHanding tif = new TestInfoHanding();
            ViewBag.hastest = 0;
            //如果存在id公司的问卷
            List<@class> qclasses = new _Class().GetQuestionClass(1, 1000000);
            string s = "<select style=\"float:left;margin-left:10px;margin-top:6px;\" id="+ViewBag.select+" class=\"companyclass\" name=\"cars\">";
            foreach (@class c in qclasses)
            {
                s += "<option class=\"notstart\" value=\"" + c.class_Class + "\">" + c.class_Class + "</option> ";
            }
            s += "</select>";
            ViewBag.options = s;
            if (tif.HasTestInfo(id))
            {
               
                ViewBag.hastest = 1;
                TestHanding th = new TestHanding();
                testInfo t = tif.GetTestInfo(id);
                ViewBag.testInfoId = t.testInfo_Id;
                ViewBag.title = t.testInfo_Name;
                ViewBag.comId = t.testInfo_OwnComPanyId;
                ViewBag.totalNum = th.GetTestCount(t.testInfo_Id);
                ViewBag.qclass = th.GetTestClassCount(t.testInfo_Id);
                ViewBag.creatTime = t.testInfo_GenerateTime;
                ViewBag.startTime = t.testInfo_StartTime;
                ViewBag.deadline = t.testInfo_Deadline;
               
            }
            //不存在id公司的问卷
            else
            {
                ViewBag.title = "未创建";
                ViewBag.comId = "未创建";
                ViewBag.totalNum = "未创建";
                ViewBag.qclass = "未创建";
                ViewBag.creatTime = "未创建";
                ViewBag.startTime = "未创建";
                ViewBag.deadline = "未创建";
            }
            return PartialView();
        }
        public string AddTestInfo(string id)
        {
            string testName = Request["parameter0"];
            string startTime = Request["parameter1"];
            string deadline = Request["parameter2"];
            TestInfoHanding tif = new TestInfoHanding();
            string json = tif.CreateTestInfo(id, testName, startTime, deadline);
            return "{\"back\":\"" + json + "\"}";
        }
        public string DelTestInfo(string id)
        {
            TestInfoHanding tif = new TestInfoHanding();
            string json = tif.DeleteTestInfoAddAllTest(id);
            return "{\"back\":\"" + json + "\"}";
        }
        public string ImportTests(string id)
        {

            HttpPostedFileBase hpf = Request.Files["exFile"];
            string filename = "~/Execl/" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx";
            filename = this.Server.MapPath(filename);
            hpf.SaveAs(filename);
            string json = new ImportExcel().ImportQusetions(filename, id);
            return "{\"back\":\"" + json + "\"}";
        }
        public ActionResult Test(string id, string dataId)
        {
            int pageIndex = 1;
            int pageSize = 1000;
            long testInfoId = 0;
            ViewBag.topCompanyId = id;
            ViewBag.id = dataId;
            ViewBag.tableId1 = dataId.Split('#')[1] + "table1";
            ViewBag.tests = new TestHanding().GetTestByComId(id, pageIndex, pageSize, out testInfoId);
            ViewBag.testInfoId = testInfoId;
            ViewBag.topCompanyId = id;
            return PartialView();
        }
        public string DelTests(string id)
        {
            string datas = Request["datas"];
            string json = new TestHanding().DelTestQusetions(id,datas);
            return "{\"back\":\"" + json + "\"}";
        }
        public string AddQuestion(long id)
        {
            long testInfoId = id;
            string datas = Request["datas"];
            string json = new TestHanding().AddTestQuestion(datas, testInfoId);
            return "{\"back\":\"" + json + "\"}";
        }
        public string OutPut(string id)
        {
            string json = null;
            string zipPath = null;
            if (Request["datas"] == "output")
            {
                string path = this.Server.MapPath("~/OutPutExcel");
                json = new OutPutResult().OutPut(id, path, out zipPath);
            }
            return "{\"back\":\"" + json + "\",\"zipPath\":\"" + zipPath + "\"}";
        }
        public ActionResult BackFile(string id)
        {
            string path = this.Server.MapPath("~/OutPutExcel");
            string zipPath = path + "\\" + id + ".zip";
            return File(zipPath, "application/x-zip-compressed");
        }
        public ActionResult ScanTest(long id, string CID)
        {
            int pageSize = 20;
            testInfo ti = new TestInfoOperation().GetTestInfo(CID)[0];
            List<test> tests = new TestHanding().GetTests(ti.testInfo_Id, (int)id, pageSize);
            ViewBag.tests = tests;
            ViewBag.title = ti.testInfo_Name;
            ViewBag.dataSize = new TestHanding().GetTestCount(Convert.ToInt32(ti.testInfo_Id));
            int maxPage = new TestHanding().GetMaxPage(pageSize, Convert.ToInt32(ti.testInfo_Id)); ;
            ViewBag.maxDataSize = maxPage;
            ViewBag.index = id < maxPage ? id : maxPage;
            ViewBag.staffId = 0;
            ViewBag.CID = CID;
            return View("~/Views/Staff/Index.cshtml");
        }

        public ActionResult DivisionalManagement(string id, string dataId)
        {
            ViewBag.table = dataId.Split('#')[1] + "_table";
            ViewBag.companyStatus = new Companyhandling().GetCompnayStatus(id);
            return PartialView();
        }

        public string DepartmentTreeGridJson(string id)
        {
            return new Companyhandling().DepartmentTreeGridJson(id);
        }

        public string ModifyDivisional(string id, string OwnId, string name, string changeName)
        {
            return new StaffHanding().ModifyDivisional(id, OwnId, name, changeName).ToJson();
        }

        public string RemoveDivisional(string id, string OwnId, string remove)
        {
            return new StaffHanding().RemoveDivisional(id, OwnId, remove).ToJson();
        }
        public string QusetionsJson(string id, int page, int rows)
        {
            string qclass = Request["qClass"];
            if (string.IsNullOrEmpty(qclass))
            {
                return "error";
            }
            string json = new QuestionHeading().GetQuestionsByClass(qclass,page,rows);
            return json;
        }
        public string AddQuestionToTest(string id)
        {
            string datas=Request["datas"];
            long testinfoId = Convert.ToInt64(id);
            TestHanding th = new TestHanding();
            string json = new TestHanding().AddQuestionTotest(testinfoId, datas);
            return "{\"back\":\"" + json + "\"}";
        }

        public string ResetPassword(string datas)
        {
            return new StaffHanding().ResetPassword(datas).ToJson();
        }
    }
}
