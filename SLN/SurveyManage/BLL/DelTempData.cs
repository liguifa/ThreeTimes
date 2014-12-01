using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace BLL
{
   public class DelTempData
    {
       public string DelAllTemp(string path1,string path2)
       {
           try {
               Directory.Delete(path1, true);
               Directory.Delete(path2, true);
               Directory.CreateDirectory(path1);
               Directory.CreateDirectory(path2);
           }
           catch
           {
               return "清除失败";
           }
           if (!Directory.Exists(path1))
           {
               Directory.CreateDirectory(path1);
           }
           if (!Directory.Exists(path2))
           {
               Directory.CreateDirectory(path2);
           }
           return "ok";
       }
    }
}
