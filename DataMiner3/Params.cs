using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner3
{
    public class Params
    {
        private DateTime startDate;
        public string DB2ConnectionString { get; set; }
        public string MySQLConnectionString { get; set; }

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

        public DateTime StartDateTime
        {
            get { return startDate; }
            set { startDate = value; }
        }
    }
}
