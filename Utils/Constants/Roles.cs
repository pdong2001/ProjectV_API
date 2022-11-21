using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Constants
{
    public static class Roles
    {
        public const string Admin = "_ad";
        public const string Staff = "_st";
        public const string Collaborator = "_cb";
        public const string AdminStaff = Admin + "," + Staff;
    }
}
