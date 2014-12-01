using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProvinceOperating:BaseBLL<province>
    {
        public List<province> GetProvinceList()
        {
            return base.Search(u => u.province_Id!=0);
        }
    }
}
