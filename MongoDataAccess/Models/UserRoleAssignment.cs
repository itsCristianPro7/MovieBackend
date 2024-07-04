using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDataAccess.Models
{
    public class UserRoleAssignment
    {
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
