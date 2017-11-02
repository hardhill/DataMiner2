﻿using System;
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
            Params taskParams = new Params();
            Log.w("================================== START ver. 171102-1 =======================================");
            INIManager setINI = new INIManager();
            
            setINI.Path = Path.GetDirectoryName(AppContext.BaseDirectory) + @"\dataminer2.ini";
            
            taskParams.DB2ConnectionString = setINI.GetPrivateString("DB2", "ConnectStr", "Driver ={ IBM DB2 ODBC DRIVER}; Database = WF; Hostname = 10.3.30.135; Port = 50666; Protocol = TCPIP; Uid = wfuser; Pwd = wfuser017;");
            taskParams.MySQLConnectionString = setINI.GetPrivateString("MYSQL", "ConnectStr", "Database=infocenter;Data Source=localhost;User Id=icadmin;Password=Inf0Center");
            taskParams.Department = Int32.Parse(setINI.GetPrivateString("DB2","Department", "35"));
            taskParams.Delta = Int32.Parse(setINI.GetPrivateString("TASKS","Delta","1"));
            taskParams.SetStartDate(setINI.GetPrivateString("TASKS","Startdate","2017-01-01"));
            WFContext wfContext = new WFContext(taskParams.DB2ConnectionString);
            //проверка даты начала отбора с текущим днем, часом, минутой и т.п )))
            DateTime dt = DateTime.Now;
            dt = dt > taskParams.StartDateTime ? taskParams.StartDateTime : dt;
            // выборка TASKS из ПФР
            List<Task> newTask = wfContext.GetDeltaTasks(dt, taskParams.Delta, taskParams.Department);
            // подсчитываем результат выборки из БД "ЗАДАЧИ"
            if (newTask.Count() > 0)
            {//здесь данные пойдут в БД Инфоцентр


            }else   //ничего не выбрано в БД ПФР
            {
                // увеличиваем на Н-дней начальную дату отбора
                DateTime newDate = taskParams.StartDateTime.AddDays(taskParams.Delta);
                //сохраняем в параметр
                taskParams.StartDateTime = newDate;
            }

//c сохранение параметров
            setINI.WritePrivateString("DB2", "ConnectStr", taskParams.DB2ConnectionString);
            setINI.WritePrivateString("DB2", "Department", taskParams.Department.ToString());
            setINI.WritePrivateString("TASKS", "Delta", taskParams.Delta.ToString());
            setINI.WritePrivateString("TASKS","Startdate",taskParams.GetStartDate());
            setINI.WritePrivateString("MYSQL", "ConnectStr", taskParams.MySQLConnectionString);
            Log.wi(DateTime.Now, "Завершаем программу", String.Format("Нач.дата следующей выборки:{0}", taskParams.GetStartDate()));
        }
}
}
