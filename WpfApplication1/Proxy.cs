using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace brute
{
    public class Proxy : INotifyPropertyChanged
    {
        private List<string> pList = new List<string>();
        public static string PATH = System.IO.Directory.GetCurrentDirectory();
        public static string PATHSETTING = System.IO.Directory.GetCurrentDirectory() + @"/Proxy/settings.xml";

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

        private bool pUseStopEmtpy = false;
        public bool ProxyUseStopEmtpy
        {
            get { return pUseStopEmtpy; }
            set
            {
                if (pUseStopEmtpy != value)
                {
                    pUseStopEmtpy = value;
                    NotifyPropertyChanged();
                }
            }
        }

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

        private int pTypeGetProxy = 0;
        public int ProxyTypeGetProxy
        {
            get { return pTypeGetProxy; }
            set
            {
                if (pTypeGetProxy != value)
                {
                    pTypeGetProxy = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region METHODES
        /// <summary>
        /// Сохранение настройк формы
        /// </summary>
        public void SaveProperty()
        {
            using (var wfile = new System.IO.StreamWriter(PATHSETTING, false, Encoding.Default))
            {
                var writer = new System.Xml.Serialization.XmlSerializer(typeof(Proxy));
                writer.Serialize(wfile, proxy);
            }
        }

        /// <summary>
        /// Получение настройк формы
        /// </summary>
        public static void GetProperty()
        {
            using (var file = new System.IO.StreamReader(PATHSETTING))
            {
                var reader = new System.Xml.Serialization.XmlSerializer(typeof(Proxy));
                proxy = (Proxy)reader.Deserialize(file);
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
            using (var sw = new System.IO.StreamReader(path, Encoding.Default))
            {
                string s;
                pList.Clear();
                while ((s = sw.ReadLine()) != null)
                {
                    pList.Add(s);
                }
                ProxyAllCount = pList.Count;
            }
        }

        private static object locker = new object();
        private static int index = new int();

        /// <summary>
        /// Получения прокси строки
        /// </summary>
        /// <returns></returns>
        public string GetProxyLine()
        {
            string line = string.Empty;

            lock(locker)
            {
                if(pList.Count > 0)
                {
                    if(index < pList.Count)
                    {
                        line = pList[index];
                        index = index + 1;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else
                {
                   if(ProxyEmpty!= null)
                    {
                        ProxyEmpty("Список прокси пуст!");
                    }
                    ProxyUseStopEmtpy = true;
                }
            }

            return line;
        }

        /// <summary>
        /// Запись в файл прокси
        /// </summary>       
        public void WriteProxy(string line,int index = 0)
        {
            string path;
            if(index == 0){
                path = pPathGood;
            }else{
                path = pPathBad;
            }

            lock(locker)
            {
                using (var sw = new StreamWriter(path, true, Encoding.Default)){
                    sw.WriteLine(line);
                }

                pList.Remove(line);
                ProxyAllCount = pList.Count;
                ProxyBadCount = ProxyBadCount + 1;
            }

        }

        #endregion

        #region COMMANDES
        private ICommand setPathGood;
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region EVENTES
        public delegate void ProxyEvent(string msg);
        public event ProxyEvent ProxyEmpty;
        #endregion
    }
}
