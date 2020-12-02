using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Docker.OpenStackUserentity;

namespace Docker
{
    interface IOpenStack
    {
        string GetAuth(string  url,string username,string password,string domainname);
        ImagesInfo GetImageList(string url ,string auth);
        int DelImage(string url, string auth, string imageid);
        int DeactiveImage(string url, string auth, string imageid);
        int ReactiveImage(string url, string auth, string imageid);
        OpenStackUserInfo GetUserList(string url, string auth);
        int ChangePwd(string url, string userid, string oldpass, string newpass);
        int StopUser(string url,string auth,string userid);
        int StartUser(string url, string auth, string userid);
        int AddUser(string url, string auth, string defaultprojectid, string domain_id, string name, string pwd, string description, string email);
    }
}
