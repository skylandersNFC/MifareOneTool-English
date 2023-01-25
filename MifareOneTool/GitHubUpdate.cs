using System;
using System.IO;
using System.Net;
using System.Reflection;

using MifareOneTool.Properties;

using Newtonsoft.Json.Linq;

namespace MifareOneTool
{
    class GitHubUpdate
    {
        public Version localVersion;
        public string remoteVersion=Resources.未知;
        public void Update(string GitHubR)
        {
            try
            {
                //.net4.5 available
                // Handle HttpWebRequest access to https with secure certificate (Request aborted: Failed to create SSL/TLS secure channel.)
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/" + GitHubR + "/releases/latest");
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject jo = JObject.Parse(responseString);
                if (jo.GetValue("message") == null)
                {
                    dynamic json = Newtonsoft.Json.Linq.JToken.Parse(responseString) as dynamic;
                    if (json.prerelease == false)
                    {
                        this.remoteVersion = (string)json.tag_name;
                    }
                }
                else
                {
                    Console.Error.WriteLine("GitHub update is invalid");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public GitHubUpdate(string GitHubR)
        {
            this.localVersion = Assembly.GetExecutingAssembly().GetName().Version;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/" + GitHubR + "/releases/latest");
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject jo = JObject.Parse(responseString);
                if (jo.GetValue("message") == null)
                {
                    dynamic json = Newtonsoft.Json.Linq.JToken.Parse(responseString) as dynamic;
                    if(json.prerelease==false){
                        this.remoteVersion=(string)json.tag_name;
                    }
                }
                else
                {
                    Console.Error.WriteLine("GitHub update is invalid");
                }
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
