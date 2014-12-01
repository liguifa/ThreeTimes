using Common.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public  class FindGGFatherCompanyByCompany
    {
        public company FindGGFatherCompany(company com)
        {
            List<company> c =new List<company>() ;
            c.Add(com);
            Company s = new Company();
            while (c[0].company_OwnerCompany != "0")
            {
                string temp = null;
                temp = c[0].company_OwnerCompany;
                c.Clear();
                c = s.Search(u => u.company_Id == temp);
            }
            return c[0];
        }
    }
}
