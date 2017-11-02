using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner2
{
    public class Params
    {
        private DateTime startDate;
        public string ConnectionString { get; set; }
        
        public int Delta { get; set; }
        public int Department { get; set; }

        public string GetStartDate()
        {
            return startDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        public void SetStartDate(string sd)
        {
            startDate = Convert.ToDateTime(sd);
        }
    }
}
