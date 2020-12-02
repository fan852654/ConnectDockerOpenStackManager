using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Docker
{
    class SMSSender
    {
        public readonly static string api_key = "82072934";
        public readonly static string api_secret = "xBgORmuxBzW0ObyL";

        public static async Task<bool> SendSMS(string to,string msg,string from)
        {
            using (var client = new HttpClient())
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("api_key", api_key));
                values.Add(new KeyValuePair<string, string>("api_secret", api_secret));
                values.Add(new KeyValuePair<string, string>("to", "86" + to));
                values.Add(new KeyValuePair<string, string>("from", from));
                values.Add(new KeyValuePair<string, string>("text", msg));
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://rest.nexmo.com/sms/json", content);

                var responseString = await response.Content.ReadAsStringAsync();
            }
            return true;
        }
    }
}
