using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gali.AppServer.Entities.Users
{
    public class Model
    {
        public Guid Id;
        public string User;
        public string Names;
        public string LastNames;
        public string Password;
        public string PasswordSalt;
        public string State;
    }
    public class States {
        public const string Active = "Active";
        public const string InActive = "InActive";
    }

    
}
