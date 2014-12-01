using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class PasswordChecker
    {
        public static bool AllNotNull(string password1, string password2, string password3)
        {
            if ((String.IsNullOrEmpty(password1) || String.IsNullOrEmpty(password2) || String.IsNullOrEmpty(password3)))
            {
                return false;
            }

            return true;

        }
        public static bool Equals(string newPassword, string rePassword) {
            
            if(newPassword.Equals(rePassword))
            {
                return true;
            }
            
            return false;

        }

    }
}
