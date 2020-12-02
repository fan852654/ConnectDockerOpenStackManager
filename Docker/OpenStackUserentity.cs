using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docker
{
    class OpenStackUserentity
    {
        public class Links
        {
            public string self { get; set; }
        }

        public class Options
        {
        }

        public class UsersItem
        {
            public string name { get; set; }
            public Links links { get; set; }
            public string domain_id { get; set; }
            public string enabled { get; set; }
            public Options options { get; set; }
            public string default_project_id { get; set; }
            public string id { get; set; }
            public string password_expires_at { get; set; }
        }

        public class links
        {
            public string self { get; set; }
            public string previous { get; set; }
            public string next { get; set; }
        }

        public class OpenStackUserInfo
        {
            public List<UsersItem> users { get; set; }
            public Links links { get; set; }
        }
    }
}
