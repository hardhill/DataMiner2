using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> lstTasks = new List<Task>();
            List<long> lstIdTask = new List<long>();
            Params taskParams = new Params();
            Log.w("================================== START Updater DataMiner 3 ver. 171107-1 =======================================");
            Log.w(String.Format("Начало программы {0}", DateTime.Now));
            Console.WriteLine("DataMIner3 Updater ver.20171107-1");
            INIManager setINI = new INIManager();

            setINI.Path = Path.GetDirectoryName(AppContext.BaseDirectory) + @"\dataminer3.ini";
            taskParams.DB2ConnectionString = setINI.GetPrivateString("DB2", "ConnectStr", "Driver ={IBM DB2 ODBC DRIVER}; Database = WF; Hostname = 10.3.30.135; Port = 50666; Protocol = TCPIP; Uid = wfuser; Pwd = wfuser017;");
            taskParams.MySQLConnectionString = setINI.GetPrivateString("MYSQL", "ConnectStr", "Database=infocenter;Data Source=localhost;User Id=icadmin;Password=Inf0Center");
            taskParams.Department = Int32.Parse(setINI.GetPrivateString("DB2", "Department", "35"));
            IcContext icContext = new IcContext(taskParams.MySQLConnectionString);
            WFContext wfContext = new WFContext(taskParams.DB2ConnectionString);
            //получаем список ID_TASK 
            lstIdTask = icContext.GetNonCompleteTasks();
            if (lstIdTask.Count > 0)
            {

            }
            else
            {
                Log.wi(DateTime.Now,"Обновление.","Нечего обновлять. Все задачи выполнены.");
            }
            //c сохранение параметров
            setINI.WritePrivateString("DB2", "ConnectStr", taskParams.DB2ConnectionString);
            setINI.WritePrivateString("DB2", "Department", taskParams.Department.ToString());
            setINI.WritePrivateString("MYSQL", "ConnectStr", taskParams.MySQLConnectionString);
            Log.wi(DateTime.Now, "Завершаем программу", "");

        }
    }
}
