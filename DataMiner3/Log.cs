using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner3
{
    public static class Log
    {
        static Log()
        {
            if (File.Exists("dataminer3.log"))
            {
                string[] arrLines = File.ReadAllLines("dataminer3.log");
                if (arrLines.Length > 5000)
                {
                    List<string> lstLines = new List<string>();
                    lstLines.AddRange(arrLines);
                    for (int i = 0; i > 100; i++)
                    {
                        lstLines.RemoveAt(0);
                    }
                }
            }
        }

        static public void w(string inpstr)
        {
            using (StreamWriter sw = new StreamWriter("dataminer3.log", true))
            {
                sw.WriteLine(inpstr);
            }
        }
        static public void we(DateTime dt, string inpstr, string e)
        {
            using (StreamWriter sw = new StreamWriter("dataminer3.log", true))
            {
                string s = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} - (E). Процесс:{1}. Сообщение:{2}", dt, inpstr, e);
                sw.WriteLine(s);
            }
        }
        static public void wi(DateTime dt, string inpstr, string i)
        {
            using (StreamWriter sw = new StreamWriter("dataminer3.log", true))
            {
                string s = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} - (I). Процесс:{1}. Сообщение:{2}", dt, inpstr, i);
                sw.WriteLine(s);
            }
        }
    }
}
