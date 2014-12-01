using SurveyManage;
using System.Web.Mvc;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using Common.Models;
using Verification;

namespace SurveyManage.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public static SessionController sc = null;
        #region public ActionResult Index()+管理员首页

        public ActionResult Index()
        {
            string adminId = sc.GetAdminSession().ToString();
            ViewBag.isSuperAdmmin = new Admin().IsSuperAdmin(adminId);
            return View();
        }
        #endregion

        public ActionResult Login()
        {
            return View();
        }

        public string LoginIn(string user,string pwd,string yzm)
        {
            List<object> res=new Admin().AdminLogin(user,pwd,yzm,(string)Session["ver"]);
            if (res.Contains(1))
            {
                sc.ClearSession();
                sc.SetAdminSession(res[1].ToString());
                res.Remove(res[1]);
            }
            return res.ToJson();
        }

        #region public ActionResult PartChart(int parameter0, int parameter1, string dataId, string parameter3)+公司列表界面
        /// <summary>
        /// 请求公司列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PartChart(int parameter0, int parameter1, string dataId, string parameter3)
        {
            Admin com = new Admin();
            List<admin> companys = com.GetPagedCompany(parameter0, parameter1, parameter3);
            ViewBag.pageIndex = parameter0;
            ViewBag.list = companys;
            ViewBag.totNum = com.totalNum;
            ViewBag.divId = dataId;
            ViewBag.pageSize = parameter1;
            ViewBag.value = String.Empty;
            ViewBag.tableId = dataId.Split('#')[1] + "_table";
            ViewBag.companyclass = parameter3;
            ViewBag.province = new ProvinceOperating().GetProvinceList().ToKeyValue();
            return PartialView();
        }

        public string ModifyCompany(string id,string companyName,string trueName,string userName,string password,string email,string phone,string privor,string sf)
        {
            return new Admin().ModifyCompany(id,companyName,trueName,userName,password,email,phone,privor,sf).ToJson() ;                        
        }   
        
        public string RemoveCompany(string id)
        {
            return new Admin().RemoveCompany(id).ToJson();
        }
        public string SendEmail(string id)
        {
            return new Admin().SendEmail(id).ToJson();
        }
        /// <summary>               
        #endregion                 
                                   
        #region public ActionResult CompanyAdd(string dataId)+添加公司页面
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyAdd(string dataId)
        {
            ViewData["id"] = dataId;
            string id0 = dataId.Split('#')[1];
            ViewData["id1"] = id0 + 1;
            ViewData["id2"] = id0 + 2;
            ViewData["id3"] = id0 + 3;
            ViewData["id4"] = id0 + 4;
            ViewData["id5"] = id0 + 5;
            ViewData["id6"] = id0 + 6;
            ViewData["id7"] = id0 + 7;
            ViewData["id8"] = id0 + 8;
            ViewData["id9"] = id0 + 9;
            ViewBag.province = new ProvinceOperating().GetProvinceList();
            return PartialView();
        }

        #endregion

        #region  public ActionResult SearchCompany(int parameter0, int parameter1, string dataId, string parameter2, string parameter3, string parameter4)搜索公司
        public ActionResult SearchCompany(int parameter0, int parameter1, string dataId, string parameter2, string parameter3, string parameter4)
        {
            Admin com = new Admin();
            List<admin> companys = com.SearchCompany(parameter0, parameter1, parameter2, parameter4, parameter3);
            ViewBag.pageIndex = parameter0;
            ViewBag.list = companys;
            ViewBag.totNum = com.totalNum;
            ViewBag.divId = dataId;
            ViewBag.pageSize = parameter1;
            ViewBag.tableId = dataId.Split('#')[1] + "_table";
            ViewBag.value = parameter2;
            ViewBag.companyclass = parameter3;
            ViewBag.searchclass = parameter4;
            return PartialView("~/Views/Admin/PartChart.cshtml");
        }
        #endregion

        #region public string CompanyAddPostBack(string dataId, string parameter0, string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6)添加公司返回
        public string CompanyAddPostBack(string dataId, string parameter0, string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6)
        {

            List<object> reslut = new Admin().AddCompany(parameter0, parameter1, parameter2, parameter3, parameter4, parameter5, parameter6);
            return reslut.ToJson();
        }
        #endregion
       
        #region  public ActionResult QuestionClass(int parameter0, int parameter1)+题库类别
        public ActionResult QuestionClass(int parameter0, int parameter1)
        {
            string id = Request["dataId"];
            _Class qc = new _Class();
            ViewBag.questionClass = qc.GetQuestionClass(parameter0, parameter1);
            ViewBag.pageIndex = parameter0;
            ViewBag.totNum = qc.totalNum;
            ViewBag.divId = id;
            ViewBag.pageSize = parameter1;
            ViewBag.tableId = id.Split('#')[1] + "_table";
            return PartialView();
        }
        #endregion
        
        #region 删除题库类别+public string RemoveQuestionClass(string datas)
        public string RemoveQuestionClass(string datas)
        {
            _Class qc = new _Class();
            List<int> result = qc.RemoveQuestionClass(datas.Split(','));
            return result.ToJson();
        }
        #endregion
       
        #region 添加题库类别public string AddQuestionClass(string datas)
        public string AddQuestionClass(string datas)
        {
            _Class qc = new _Class();
            List<object> result = qc.AddQuestionClass(datas);
            return result.ToJson();
        }
        #endregion
       
        #region 修改题库类别 public string ModifyQuestionClass(string datas, string dataIds)
        public string ModifyQuestionClass(string datas, string dataIds)
        {
            _Class qc = new _Class();
            List<object> result = qc.ModifyQuestionClass(datas, dataIds);
            return result.ToJson();
        }
        #endregion
       
        #region 修改密码 public ActionResult ChangePassword(string dataId)
        public ActionResult ChangePassword(string dataId)
        {

            ViewData["Id"] = dataId;
            string id0 = dataId.Split('#')[1];
            ViewData["id1"] = id0 + 1;
            ViewData["id2"] = id0 + 2;
            ViewData["id3"] = id0 + 3;

            return PartialView();

        }
        #endregion
       
        #region 修改密码返回
        public string ChangePasswordPostBack()
        {
            string oldPassword = Request["oldPassword"];
            string newPassword = Request["newPassword"];
            string rePassword = Request["rePassword"];
            return new Admin().ChangePassword(Convert.ToInt64(sc.GetAdminSession()),oldPassword,newPassword,rePassword).ToJson();
        }
        #endregion
       
        #region 管理员管理 public ActionResult admin(string dataId, int parameter0, int parameter1)
        public ActionResult admin(string dataId, int parameter0, int parameter1)
        {

            ViewData["Id"] = dataId;
            ViewBag.pageIndex = parameter0;
            ViewBag.pageSize = parameter1;
            string id0 = dataId.Split('#')[1];
            ViewBag.tableId = id0 + 1;


            Admin a = new Admin();
            long total = 0;
            List<admin> admins = a.GetPagedAdmins(1, 30, out total);
            ViewBag.admins = admins;
            ViewBag.total = total;


            return PartialView();

        } 
        #endregion
        
        #region 删除管理员返回public string adminPostBack(string data)
        public string adminPostBack(string data)
        {
            //TODO 检测当前管理员权限
            AdminOperating ao = new AdminOperating();
            if (ao.DelAdminById(data))
            {

                return "{\"data\":\"ok\"}";
            }

            return "{\"data\":\"error\"}";
        } 
        #endregion
        
        #region 修改管理员public string adminModify(string data)
        public string adminModify(string data)
        {
            string method = Request["method"];

            string s = null;
            AdminOperating ao = new AdminOperating();
            if (!(ao.InputChecker(data) == "OK"))
            {
                return "{\"data\":\"" + ao.InputChecker(data) + "\"}";
            }

            if (method == "modify")
            {

                s = ao.ModifyAdminById(data);
            }
            else if (method == "add")
            {

                s = ao.AddAdmin(data);

            }
            if (s == "OK")
            {

                return "{\"data\":\"ok\"}";
            }
            return
                "{\"data\":\"" + s + "\"}";
        } 

        #endregion
        public ActionResult LogOut()
        {
            sc.ClearSession();

            return View("~/Views/Admin/Login.cshtml");
        }

        public string ClearData()
        {
            string json = null;
            string importpath = this.Server.MapPath("~/Execl");
            string outpath = this.Server.MapPath("~/OutPutExcel");
            json = new DelTempData().DelAllTemp(importpath, outpath);
            return "{\"data\":\""+json+"\"}";
        }

        public ActionResult VerCode()
        {
            VerificationCode vc = new VerificationCode();
            Session["ver"] = vc.GetVerCode();
            return File(vc.StreamVerCod(), @"image/jpeg");
        }

        public ActionResult QusetionManage(string dataId)
        {
            ViewBag.id = dataId;
            ViewBag.table01 = dataId.Split('#')[1] + "table01";
            ViewBag.select = dataId.Split('#')[1] + "select";
            List<@class> qclasses = new _Class().GetQuestionClass(1, 1000000);
            string s = "<select style=\"float:left;margin-left:10px;margin-top:6px;\" id=" + ViewBag.select + " class=\"companyclass\" name=\"cars\">";
            foreach (@class c in qclasses)
            {
                s += "<option class=\"notstart\" value=\"" + c.class_Class + "\">" + c.class_Class + "</option> ";
            }
            s += "</select>";
            ViewBag.options = s;
            return PartialView();
        }

        public string ResetPassword(string datas,string mark)
        {
            return new Admin().ResetPassword(datas,mark).ToJson();
        }
    }
}
