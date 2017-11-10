using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner2
{
    public class ErrorLog
    {
        List<string> _list;
        private static ErrorLog instance;

        private ErrorLog()
        {
            _list = new List<string>();
        }

        public static ErrorLog getInstance()
        {
            if(instance == null)
            {
                instance = new ErrorLog();
            }
            return instance;

        }

        public void AddError(string s)
        {
            _list.Add(s);
        }

        public int CountErr()
        {
            return _list.Count;
        }

        public bool HasError()
        {
            return _list.Count > 0;
        }

        public void ClearErr()
        {
            _list.Clear();
        }
    }
}
