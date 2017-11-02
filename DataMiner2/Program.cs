using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Runtime.InteropServices;
using System.IO;

namespace DataMiner2
{
    
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> lstTasks = new List<Task>();
            Params wfParams = new Params();
            Log.w("================================== START ver. 171102-1 =======================================");
            INIManager setINI = new INIManager();
            
            setINI.Path = Path.GetDirectoryName(AppContext.BaseDirectory) + @"\dataminer2.ini";
            
            wfParams.ConnectionString = setINI.GetPrivateString("DB2", "ConnectStr", "Driver ={ IBM DB2 ODBC DRIVER}; Database = WF; Hostname = 10.3.30.135; Port = 50666; Protocol = TCPIP; Uid = wfuser; Pwd = wfuser017;");
            wfParams.Department = Int32.Parse(setINI.GetPrivateString("DB2","Department", "35"));
            wfParams.Delta = Int32.Parse(setINI.GetPrivateString("DB2","Delta","1"));
            wfParams.SetStartDate(setINI.GetPrivateString("DB2","Startdate","2017-01-01"));
            //WFContext wfContext = new WFContext(wfParams.ConnectionString);

//c сохранение параметров
            setINI.WritePrivateString("DB2", "ConnectStr", wfParams.ConnectionString);
            setINI.WritePrivateString("DB2", "Department", wfParams.Department.ToString());
            setINI.WritePrivateString("DB2", "Delta", wfParams.Delta.ToString());
            setINI.WritePrivateString("DB2","Startdate",wfParams.GetStartDate());

        }
}
}
