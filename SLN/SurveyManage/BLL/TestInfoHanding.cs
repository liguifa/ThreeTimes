using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
  public  class TestInfoHanding:BaseBLL<testInfo>
    {
      public bool HasTestInfo(string id)
      {
         long count= SearchCount(u => u.testInfo_OwnComPanyId == id&&u.testInfo_IsDel==false);
         if (count <= 0)
         {
             return false;
         }
         else
         {
             return true;
         }
      }
      public testInfo GetTestInfo(string id)
      {
          return Search(i => i.testInfo_OwnComPanyId == id && i.testInfo_IsDel == false)[0];
        
      }

     
     public string CreateTestInfo(string comid,string name,string startTime,string deadline)
      {
          DateTime sdt = DateTime.Now;
          DateTime ddt = DateTime.Now;
          try {
              sdt = Convert.ToDateTime(startTime);
               ddt = Convert.ToDateTime(deadline);
               if (sdt >= ddt)
               {
                   return "开始日期应小于结束日期！";
               }
               if (sdt < DateTime.Now || ddt < DateTime.Now)
               {
                   return "不允许开始时间或者结束时间早于当前时间！";
               }
          }
          catch
          {
              return "输入的时间格式错误！";
          }
          testInfo ti = new testInfo();
          ti.testInfo_Name = name;
          ti.testInfo_OwnComPanyId = comid;
          ti.testInfo_StartTime = sdt;
          ti.testInfo_Deadline = ddt;
          ti.testInfo_IsDel = false;
          ti.testInfo_GenerateTime = DateTime.Now;
          try
          {
              Add(ti);
          }
          catch
          {
              return "创建失败，请检查数据!";
          }
          return "ok";
      }
      public string DeleteTestInfoAddAllTest(string id)
     {
         if (new Companyhandling().GetCompnayStatus(id) == 1)
         {
             return "公司正在调查中，不能删除！";
         }
         try { 
         List<testInfo> testInfos = Search(k => k.testInfo_OwnComPanyId == id && k.testInfo_IsDel == false);
         testInfos[0].testInfo_IsDel = true;
         Modify(testInfos[0], "testInfo_IsDel");
         Test t = new Test();
         long testinfoId = testInfos[0].testInfo_Id;
         t.DelWhere(k => k.test_TestInfoId == testinfoId);
             }
         catch
         {
             return "删除失败!";
         }
         return "ok";
     }
    }
}
