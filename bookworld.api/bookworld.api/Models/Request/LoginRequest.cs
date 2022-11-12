using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookworld.api.Models.Request
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
