using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using xNet;

namespace brute
{
    public class Proxy : INotifyPropertyChanged
    {
        private List<string> pList = new List<string>();
        public static string PATH = Directory.GetCurrentDirectory();
        public static string PATHSETTING = Directory.GetCurrentDirectory() + @"/Proxy/settings.xml";

        #region CONCTRUCTOR
        static Proxy()
        {
           
            if (System.IO.File.Exists(PATHSETTING))
            {
                GetProperty();
            }

            if (System.IO.File.Exists(proxy.pPathGood))
            {
                proxy.LoadProxy(proxy.pPathGood);
            }

        }

        ~Proxy()
        {
            SaveProperty();
        }


        private static Proxy proxy = new Proxy();
        public static Proxy Settings
        {
            get
            {
                return proxy;
            }
        }

        #endregion

        #region PROPERTES

        /// <summary>
        /// Прокси строка
        /// </summary>
        private string pLine = null;
        public string ProxyLine
        {
            get { return pLine; }
            set
            {
                if (pLine != value)
                {
                    pLine = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Флаг Ошики прокси
        /// </summary>
        private bool pIsError = false;
        public bool ProxyIsError
        {
            get { return pIsError; }
            set
            {
                if (pIsError != value)
                {
                    pIsError = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Флаг использования прокси в программе
        /// </summary>
        private bool pUse = false;
        public bool ProxyUse
        {
            get { return pUse; }
            set
            {
                if (pUse != value)
                {
                    pUse = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Флаг сигнализирующий если список прокси пуст (true - да , false - нет)
        /// </summary>
        private bool pIsEmpty = false;
        public bool ProxyIsEmtpy
        {
            get { return pIsEmpty; }
            set
            {
                if (pIsEmpty != value)
                {
                    pIsEmpty = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Флаг авторизации прокси
        /// </summary>
        private bool pUseAuth = false;
        public bool ProxyUseAuth
        {
            get { return pUseAuth; }
            set
            {
                if (pUseAuth != value)
                {
                    pUseAuth = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Логин прокси
        /// </summary>
        private string pLogin = "Login";
        public string ProxyLogin
        {
            get { return pLogin; }
            set
            {
                if (pLogin != value)
                {
                    pLogin = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Пароль прокси
        /// </summary>
        private string pPassword = "Password";
        public string ProxyPassword
        {
            get { return pPassword; }
            set
            {
                if (pPassword != value)
                {
                    pPassword = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Путь к файлу с хорошеми проксями
        /// </summary>
        private string pPathGood = PATH + @"\Proxy\good.txt";
        public string ProxyPathGood
        {
            get { return pPathGood; }
            set
            {
                if (pPathGood != value)
                {
                    pPathGood = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Путь к файлу с плохими проксями
        /// </summary>
        private string pPathBad = PATH + @"\Proxy\bad.txt";
        public string ProxyPathBad
        {
            get { return pPathBad; }
            set
            {
                if (pPathBad != value)
                {
                    pPathBad = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Общий счетчик прокси
        /// </summary>
        private int pAllCount;
        public int ProxyAllCount
        {
            get { return pAllCount; }
            set
            {
                if (pAllCount != value)
                {
                    pAllCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Счетчик плохих прокси
        /// </summary>
        private int pBadCount = 0;
        public int ProxyBadCount
        {
            get { return pBadCount; }
            set
            {
                if (pBadCount != value)
                {
                    pBadCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Свойство возращающее тип прокси
        /// по умолчанию SOCK5
        /// </summary>
        private int pType = 2;
        public int ProxyType
        {
            get { return pType; }
            set
            {
                if (pType != value)
                {
                    pType = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Свойство возращающее тип прокси в стровом виде
        /// </summary>
        private string pTypeGetString;
        public string ProxyTypeGetString
        {
            get
            {
                switch (ProxyType)
                {
                    case 0:
                        {
                            pTypeGetString = "HTTP";
                            break;
                        }
                    case 1:
                        {

                            pTypeGetString = "SOCK4";
                            break;
                        }
                    case 2:
                        {
                            pTypeGetString = "SOCK5";
                            break;
                        }
                }
                NotifyPropertyChanged();
                return pTypeGetString;
            }
        }

        /// <summary>
        /// Свойство возращающее таймаут соеденения 
        /// </summary>
        private int pTimeOut = 30;
        public int ProxyTimeOut
        {
            get { return pTimeOut; }
            set
            {
                if (pTimeOut != value)
                {
                    pTimeOut = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Делеметр прокси строки
        /// </summary>
        private string pDelemitor;
        public string ProxyDelemitor
        {
            get { return pDelemitor; }
            set
            {
                if (pDelemitor != value)
                {
                    pDelemitor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region METHODES

        /// <summary>
        /// Сохранение настройк формы
        /// </summary>
        public static void SaveProperty()
        {
            try {

                using (var wfile = new System.IO.StreamWriter(PATHSETTING, false, Encoding.Default))
                {
                    var writer = new System.Xml.Serialization.XmlSerializer(typeof(Proxy));
                    writer.Serialize(wfile, proxy);
                }
            }
            catch
            {
                throw new Exception("Ошибка при сохранении настроик прокси.");
            }
        }

        /// <summary>
        /// Получение настройк формы
        /// </summary>
        public static void GetProperty()
        {
            try
            {
                using (var file = new System.IO.StreamReader(PATHSETTING))
                {
                    var reader = new System.Xml.Serialization.XmlSerializer(typeof(Proxy));
                    proxy = (Proxy)reader.Deserialize(file);
                }
            }
            catch
            {
                throw new Exception("Ошибка при получении настроик прокси.");
            }
        }

        /// <summary>
        /// Получение пути до файла
        /// </summary>
        /// <returns>возращает путь до выбранного файла,если файл не выбран возращает null</returns>
        private string OpenFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Загружает прокси в List<string>
        /// </summary>
        private void LoadProxy(string path)
        {
            try
            {
                using (var sw = new StreamReader(path, Encoding.Default))
                {
                    pList.Clear();
                    string s;
                    while ((s = sw.ReadLine()) != null)
                    {
                        pList.Add(s);
                    }
                    ProxyAllCount = pList.Count;
                    if(ProxyAllCount == 0)
                    {
                        throw new Exception("Ошибка прокси.Список прокси пуст.");
                    }
                }
            }
            catch
            {
                throw new Exception("Ошибка прокси.Ошибка при загрузке прокси из файла.");
            }
           
        }

        private static object locker = new object();
        private static int index = new int();

        /// <summary>
        /// Получения прокси строки
        /// </summary>
        public string GetProxyLine()
        {
            CallProxyMsg("Получения прокси строки");
            string line = null;
            lock(locker)
            {
                if(pList.Count > 0)
                {
                    begin:;
                    if(index < pList.Count)
                    {
                        line = pList[index];
                        index++;
                    }
                    else
                    {
                        index = 0;
                        goto begin;
                    }
                }
                else
                {
                    throw new Exception("Ошибка прокси.Список прокси пуст.");
                }
                CallProxyMsg($"Прокси : {line}");
                return line;
            } 
        }

        /// <summary>
        /// Запись в файл прокси 
        /// 0 - записывает в файл good.txt 
        /// 1- bad.txt
        /// </summary>       
        public void WriteProxyToFile(string line, int index = 0)
        {
            CallProxyMsg("Запись прокси в bad файл");
            string path;


            if (index == 0)
                path = pPathGood;
            else
                path = pPathBad;

            lock (locker)
            {
                try
                {

                    using (var sw = new StreamWriter(path , true, Encoding.Default))
                    {
                        sw.WriteLine(line);
                    }
                    pList.Remove(line);
                    ProxyAllCount = pList.Count;
                    ProxyBadCount++;

                }
                catch
                {
                    throw new Exception("Ошибка прокси.Не могу записать строку в файл.");
                }

            }

        }


        /// <summary>
        /// Установка прокси HttpRequest объекту
        /// </summary>
        public void SetProxy(ref HttpRequest request,string line)
        {
            CallProxyMsg("Установка типа прокси");
            try {
                switch (proxy.ProxyType)
                {
                    case 0:
                        {
                            request.Proxy = request.Proxy = HttpProxyClient.Parse(line);
                            break;
                        }
                    case 1:
                        {
                            request.Proxy = request.Proxy = Socks4ProxyClient.Parse(line);
                            break;
                        }
                    case 2:
                        {
                            request.Proxy = request.Proxy = Socks5ProxyClient.Parse(line);
                            break;
                        }

                    default:
                        throw new Exception("Ошибка прокси.Не могу установить тип прокси.");
                }

                if (proxy.ProxyUseAuth)
                {
                    request.Proxy.Password = proxy.ProxyPassword;
                    request.Proxy.Username = proxy.ProxyLogin;
                }

                CallProxyMsg($"Установлен тип прокси {ProxyTypeGetString}");
            }
            catch
            {
                throw new Exception("Ошибка прокси.Не могу установить тип прокси.");
            }
        }
        #endregion

        #region COMMANDES


        private ICommand setPathGood;

        /// <summary>
        /// Установка пути к файлу с прокси и загрузка прокси в программу
        /// </summary>
        public ICommand SetPathGood
        {
            get
            {
                if (setPathGood == null)
                {
                    setPathGood = new RelayCommand(
                        () =>
                        {
                            string path = OpenFile();
                            if (path != null)
                            {
                                ProxyPathGood = path;
                                LoadProxy(path);
                            }
                        });
                }
                return setPathGood;
            }
        }


        private ICommand setPathBad;
     
        /// <summary>
        /// Установка пути к файлу с плохими прокси
        /// </summary>
        public ICommand SetPathBad
        {
            get
            {
                if (setPathBad == null)
                {
                    setPathBad = new RelayCommand(
                        () =>
                        {
                            string path = OpenFile();
                            if (path != null)
                            {
                                ProxyPathBad = path;
                            }
                        });
                }
                return setPathBad;
            }
        }
     
        #endregion

        #region EVENTES
        /// <summary>
        /// Вызов события ProxyMsg 
        /// </summary>
        /// <param name="msg">Сообщения</param>
        private void CallProxyMsg(string msg)
        {
            if (ProxyMsg != null)
                ProxyMsg(msg);
        }

        public delegate void ProxyEventMsg(string msg);
        public event ProxyEventMsg ProxyMsg;
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
}
