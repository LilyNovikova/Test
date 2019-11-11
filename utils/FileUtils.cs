using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexMarket.utils
{
    public class FileUtils
    {
        private string path;
        private string OutputFile;
        private string os;
        private string PathSeparator;
        private string browser;
        private string url;
        private string email;
        private string password;
        private List<string> options;

        public FileUtils(string InputFileName)
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(location);

            string[] Config;
            try
            {
                //if windows
                Config = File.ReadAllLines(path + "\\" + InputFileName);

            }
            catch (FileNotFoundException)
            {
                //if linux
                Config = File.ReadAllLines(path + "/" + InputFileName);
            }
            int i = 0;

            os = Config[i++].TrimEnd(',');
            if (os.Equals("Windows"))
            {
                PathSeparator = "\\";
            }
            else if (os.Equals("Linux"))
            {
                PathSeparator = "/";
            }
            browser = Config[i++].TrimEnd(',');
            url = Config[i++].TrimEnd(',');
            OutputFile = path + PathSeparator + Config[i++].TrimEnd(',');
            email = Config[i++].TrimEnd(',');
            password = Config[i++].TrimEnd(',');
            options = new List<string>();
            while (i < Config.Length)
            {
                options.Add(Config[i++].TrimEnd(','));
            }
        }
        public string GetBrowser() { return browser; }

        public string GetUrl() { return url; }
        public string GetEmail()
        {
            return email;
        }

        public string GetPassword() { return password; }

        public List<string> GetOptions() { return options; }

        public string GetOutputFile() { return OutputFile; }
        public string GetPath() { return path; }
        public string GetPathSeparator() { return PathSeparator; }

        public bool CompareFiles(string Filename1, string Filename2)
        {
            string[] Text1 = File.ReadAllLines(Filename1);
            string[] Text2 = File.ReadAllLines(Filename2);
            if(Text1.Length != Text2.Length)
            {
                return false;
            }
            for(int i = 0; i < Text1.Length; i++)
            {
                if (!Text1[i].Equals(Text2[i]))
                    return false;
            }
            return true;
        }

        public bool Write(string text)
        {
            StreamWriter swFile = File.CreateText(OutputFile);
            swFile.WriteLine(text);
            swFile.Close();
            GC.SuppressFinalize(this);
            return true;
        }
    }
}
