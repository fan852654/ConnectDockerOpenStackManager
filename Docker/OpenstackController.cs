using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Docker.OpenStackUserentity;

namespace Docker
{
    class OpenstackController : IOpenStack
    {
        public string GetAuth(string url,string username, string password, string domainname)
        {
            string authurl = url + ":5000/v3/auth/tokens?nocatalog";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(authurl);
            request.Method = "POST";
            request.ContentType = "application/json";
            string jsondata = "{ \"auth\": { \"identity\": { \"methods\": [\"password\"]," +
                "\"password\": {\"user\": {\"domain\": {\"name\": \""+domainname+"\"}" +
                ",\"name\": \""+username+"\", \"password\": \""+password+"\"} } } }}";
            byte[] data = Encoding.UTF8.GetBytes(jsondata);
            request.ContentLength = data.Length;
            using(Stream reqstream = request.GetRequestStream())
            {
                reqstream.Write(data, 0, data.Length);
                reqstream.Close();
            }
            HttpWebResponse resp;
            int returnid = 200;
            string auth = "";
            try
            {
                resp = (HttpWebResponse)request.GetResponse();
                auth = resp.Headers.Get("X-Subject-Token");
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                returnid = (int)rsp.StatusCode;
            }
            if(returnid != 200)
            {
                if(returnid == 404)
                    MessageBox.Show("404 NOT FOUND 没有找到网页", "Error", MessageBoxButtons.OK);
                else if(returnid == 401)
                    MessageBox.Show("用户信息错误请检查", "Error", MessageBoxButtons.OK);
                else
                    MessageBox.Show("错误的返回代码", "Error", MessageBoxButtons.OK);
                return null;
            }else
                return auth;
        }

        public ImagesInfo GetImageList(string url ,string auth)
        {
            List<OpenStackImage> list = new List<OpenStackImage>();
            string imageurl = url + ":9292/v2/images";
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(imageurl);
            rq.Method = "GET";
            SetHeaderValue(rq.Headers, "X-Auth-Token", auth);
            int returnid = 200;
            HttpWebResponse resp;
            Stream stream;
            try
            {
                resp = (HttpWebResponse)rq.GetResponse();
                stream = resp.GetResponseStream();
                string result = "";
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                JArray jlist = JArray.Parse(jo["images"].ToString());
                int count = jlist.Count;
                for (int i = 0; i < count; i++)
                {
                    string num = jo["images"][i]["size"].ToString();
                    jo["images"][i]["size"] = num;
                }
                ImagesInfo iif = JsonConvert.DeserializeObject<ImagesInfo>(jo.ToString());
                return iif;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                returnid = (int)rsp.StatusCode;
                if (returnid == 404)
                    MessageBox.Show("404 NOT FOUND 没有找到网页", "Error", MessageBoxButtons.OK);
                else if (returnid == 401)
                    MessageBox.Show("用户信息错误请检查", "Error", MessageBoxButtons.OK);
                else
                    MessageBox.Show("错误的返回代码", "Error", MessageBoxButtons.OK);
                return null;
            }
        }

        public int DelImage(string url, string auth, string imageid)
        {
            var httpstatuscode = 200;
            string realurl = url + ":9292/v2/images/" + imageid;
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(realurl);
            rq.Method = "DELETE";
            SetHeaderValue(rq.Headers, "X-Auth-Token", auth);
            try
            {
                var rsp = rq.GetResponse() as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            return httpstatuscode;
        }

        public int DeactiveImage(string url, string auth, string imageid)
        {
            var httpstatuscode = 200;
            string realurl = url + ":9292/v2/images/" + imageid + "/actions/deactivate";            
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(realurl);
            rq.Method = "POST";
            SetHeaderValue(rq.Headers, "X-Auth-Token", auth);
            try
            {
                var rsp = rq.GetResponse() as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            return httpstatuscode;
        }

        public int ReactiveImage(string url, string auth, string imageid)
        {
            var httpstatuscode = 200;
            string realurl = url + ":9292/v2/images/" + imageid + "/actions/reactivate";
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(realurl);
            rq.Method = "POST";
            SetHeaderValue(rq.Headers, "X-Auth-Token", auth);
            try
            {
                var rsp = rq.GetResponse() as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            return httpstatuscode;
        }

        public OpenStackUserInfo GetUserList(string url, string auth)
        {
            var httpstatuscode = 200;
            string realurl = url + ":35357/v3/users";
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(realurl);
            rq.Method = "GET";
            SetHeaderValue(rq.Headers, "X-Auth-Token", auth);
            OpenStackUserInfo osu;
            try
            {
                var rsp = rq.GetResponse() as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
                Stream stream = rsp.GetResponseStream();
                string result = "";
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                osu = JsonConvert.DeserializeObject<OpenStackUserInfo>(result);
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
                if (httpstatuscode == 404)
                    MessageBox.Show("404 NOT FOUND 没有找到网页", "Error", MessageBoxButtons.OK);
                else if (httpstatuscode == 401)
                    MessageBox.Show("用户信息或者权限错误请检查", "Error", MessageBoxButtons.OK);
                else if (httpstatuscode == 403)
                    MessageBox.Show("用户无权访问", "Error", MessageBoxButtons.OK);
                else
                    MessageBox.Show("错误的返回代码", "Error", MessageBoxButtons.OK);
                return null;
            }
            return osu;
        }

        public int ChangePwd(string url, string userid, string oldpass, string newpass)
        {
            int httpstatuscode = 200;
            string realurl = url + ":35357/v3/users/" + userid + "/password";
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(realurl);
            rq.Method = "POST";
            SetHeaderValue(rq.Headers, "Content-Type", "application/json");
            string json = "{\"user\": {\"password\": \"" + newpass + "\",\"original_password\": \"" + oldpass + "\"}}";
            byte[] data = Encoding.UTF8.GetBytes(json);
            rq.ContentLength = data.Length;
            using (Stream reqstream = rq.GetRequestStream())
            {
                reqstream.Write(data, 0, data.Length);
                reqstream.Close();
            }
            try
            {
                HttpWebResponse resp = (HttpWebResponse)rq.GetResponse();
                httpstatuscode = (int)resp.StatusCode;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            return httpstatuscode;
        }

        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }

        public int StopUser(string url, string auth, string userid)
        {
            string realurl = url + ":35357/v3/users/" + userid;
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(realurl);
            rq.Method = "PATCH";
            int httpstatuscode = 200;
            SetHeaderValue(rq.Headers, "X-Auth-Token", auth);
            SetHeaderValue(rq.Headers, "Content-Type", "application/json");
            string json = "{\"user\": {\"enabled\": false}}";
            byte[] data = Encoding.UTF8.GetBytes(json);
            rq.ContentLength = data.Length;
            using (Stream reqstream = rq.GetRequestStream())
            {
                reqstream.Write(data, 0, data.Length);
                reqstream.Close();
            }
            try
            {
                HttpWebResponse resp = (HttpWebResponse)rq.GetResponse();
                httpstatuscode = (int)resp.StatusCode;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            return httpstatuscode;
        }

        public int StartUser(string url, string auth, string userid)
        {
            string realurl = url + ":35357/v3/users/" + userid;
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(realurl);
            rq.Method = "PATCH";
            int httpstatuscode = 200;
            SetHeaderValue(rq.Headers, "X-Auth-Token", auth);
            SetHeaderValue(rq.Headers, "Content-Type", "application/json");
            string json = "{\"user\": {\"enabled\": true}}";
            byte[] data = Encoding.UTF8.GetBytes(json);
            rq.ContentLength = data.Length;
            using (Stream reqstream = rq.GetRequestStream())
            {
                reqstream.Write(data, 0, data.Length);
                reqstream.Close();
            }
            try
            {
                HttpWebResponse resp = (HttpWebResponse)rq.GetResponse();
                httpstatuscode = (int)resp.StatusCode;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            return httpstatuscode;
        }

        public int AddUser(string url, string auth, string defaultprojectid, string domain_id, string name, string pwd, string description, string email)
        {
            string realurl = url + ":35357/v3/users/";
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(realurl);
            rq.Method = "POST";
            int httpstatuscode = 200;
            SetHeaderValue(rq.Headers, "X-Auth-Token", auth);
            SetHeaderValue(rq.Headers, "Content-Type", "application/json");
            string json = "{\"user\": {\"default_project_id\": \""+defaultprojectid+"\"," +
                "\"domain_id\": \""+domain_id+"\",\"enabled\": true," +
                "\"name\": \""+name+"\",\"password\": \""+pwd+"\"," +
                " \"description\": \""+description+"\",\"email\": \""+email+"\"}}";
            byte[] data = Encoding.UTF8.GetBytes(json);
            rq.ContentLength = data.Length;
            using (Stream reqstream = rq.GetRequestStream())
            {
                reqstream.Write(data, 0, data.Length);
                reqstream.Close();
            }
            try
            {
                HttpWebResponse resp = (HttpWebResponse)rq.GetResponse();
                httpstatuscode = (int)resp.StatusCode;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                httpstatuscode = (int)rsp.StatusCode;
            }
            return httpstatuscode;
        }
    }
}
