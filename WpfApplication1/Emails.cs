using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xNet;

namespace brute
{
    public class Emails : INotifyPropertyChanged
    {
        string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value != email)
                {
                    email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        int emailCount;
        public int EmailCount
        {
            get
            {
                return emailCount;
            }
            set
            {
                if (value != emailCount)
                {
                    emailCount = value;
                    NotifyPropertyChanged("EmailCount");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void GetEmail(Int64 index)
        {
            using (var request = new HttpRequest())
            {
                request.CharacterSet = Encoding.UTF8;
                string url = "https://otvet.mail.ru/question/" + index.ToString();

                var line = request.Get(url).ToString();

                if (!String.IsNullOrEmpty(line))
                    ParseEmail(line);
            }
        }

        private void ParseEmail(string text)
        {
            string pattern = @"(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))";

            var reg = new Regex(pattern);
            var match = reg.Matches(text);

            if (match.Count != 0)
            {
                EmailCount += match.Count;
                using (var sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\Emails\good.txt", true, Encoding.Default))
                {
                    foreach (var item in match)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
            }
        }
    }
}
