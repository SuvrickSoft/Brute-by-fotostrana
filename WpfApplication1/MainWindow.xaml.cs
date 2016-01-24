using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using xNet;

namespace brute
{
    public partial class MainWindow : Window
    {
        Proxy p;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            p = Proxy.Settings;
            p.ProxyEmpty += P_ProxyEmpty;
        }

        private void P_ProxyEmpty(string msg)
        {
            Stop();
        }

        public static List<string> emails = new List<string>();

        private void btn_Click(object sender, RoutedEventArgs e)
        {

            using (var reader = new StreamReader(Directory.GetCurrentDirectory() + @"\Emails\bad.txt"))
            {
                string str = string.Empty;
                while ((str = reader.ReadLine()) != null)
                {
                    if (!emails.Contains(str))
                        emails.Add(str); 
                }
            }

            Start();
        }

        List<Thread> threads = new List<Thread>();

        public void Start(int ind = 10)
        {
            index = 0;
            for (int i = 0; i < ind; i++)
            {
                var thread = new Thread(new ThreadStart(Work));
                thread.IsBackground = true;
                thread.Start();
                threads.Add(thread);
            }
        }

        public void Stop()
        {
            foreach (var item in threads)
            {
                try
                {
                    item.Abort();
                }
                catch 
                {   
                }
            }
        }

        public static object locker = new object();
        public static int index;
        public static bool finish;
        public void Work()
        {
            while (!finish)
            {


                //Begin:;
                string email = null;

                lock (locker)
                {
                    if (index < emails.Count)
                    {
                        email = emails[index].Trim();
                        index = index + 1;
                    }
                    else
                    {
                        finish = true;
                        index = 0;
                        //goto Begin;
                    }

                }


                string s;
                Unit unit = new Unit(email);
                unit.Registr(out s);
                tb.Dispatcher.BeginInvoke(new Action(() => { tb.Text += s + Environment.NewLine; }));

                
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            p.SaveProperty();
        }
    }
}
