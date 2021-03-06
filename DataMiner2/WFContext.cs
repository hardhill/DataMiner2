﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace DataMiner2
{
    class WFContext
    {
        const string DATEFORMAT = "yyyy-MM-dd HH:mm:ss.fff";
        public string ConnectionString { get; set; }
        ErrorLog errorLog = ErrorLog.getInstance();

        public WFContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private OdbcConnection GetConnection()
        {
            return new OdbcConnection(ConnectionString);
        }

        public List<Task> GetDeltaTasks(DateTime startDate, int deltaDay, int department)
        {
            DateTime d = startDate.AddDays(deltaDay);
            string start = startDate.ToString(DATEFORMAT);
            string finish = d.ToString(DATEFORMAT);
            List<Task> list = new List<Task>();
            using (OdbcConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string sql = String.Format("SELECT T1.* FROM DB2ADMIN.TASKS as T1 WHERE (TIMESTAMP(T1.DATEOFCOMMING)>='{0}')AND(TIMESTAMP(T1.DATEOFCOMMING)<'{1}')AND(T1.ID_DEPARTMENT={2})", start, finish, department);
                    OdbcCommand comGetTasks = new OdbcCommand(sql, conn);
                    try
                    {
                        using (OdbcDataReader readTask = comGetTasks.ExecuteReader())
                        {
                            while (readTask.Read())
                            {
                                Task task = new Task();
                                task.Id_task = (readTask.IsDBNull(0)) ? 0 : readTask.GetInt64(0);
                                task.Id_type_process = readTask.IsDBNull(1) ? 0 : readTask.GetInt64(1);
                                task.Id_process = readTask.IsDBNull(2) ? 0 : readTask.GetInt64(2);
                                task.Id_stage_to = readTask.IsDBNull(3) ? 0 : readTask.GetInt32(3);
                                task.Id_stage_from = readTask.IsDBNull(4) ? 0 : readTask.GetInt32(4);
                                task.Type_transaction = readTask.IsDBNull(5) ? 0 : readTask.GetInt32(5);
                                if (!readTask.IsDBNull(6))
                                {
                                    task.Dateofcomming = readTask.GetDateTime(6);
                                }
                                else task.Dateofcomming = DateTime.MinValue;
                                if (!readTask.IsDBNull(7))
                                {
                                    task.Dateoftaking = readTask.GetDateTime(7);
                                }
                                else task.Dateoftaking = DateTime.MinValue;
                                if (!readTask.IsDBNull(8))
                                {
                                    task.Dateofcomlation = readTask.GetDateTime(8);
                                }
                                else task.Dateofcomlation = DateTime.MinValue;
                                if (!readTask.IsDBNull(9))
                                {
                                    task.Id_user = readTask.GetString(9);
                                }
                                task.Type_complation = readTask.IsDBNull(10) ? 0 : readTask.GetInt32(10);

                                task.Id_department = department;
                                list.Add(task);
                            }
                        }
                    }catch(Exception e)
                    {
                        Log.we(DateTime.Now, "Выполнение запроса в ПФР<TASKS>", e.Message);
                        errorLog.AddError(e.Message);
                    }
                }
                catch(Exception e)
                {
                    Log.we(DateTime.Now, "Соединение с БД ПФР", e.Message);
                    errorLog.AddError(e.Message);

                }
            }
            Log.wi(DateTime.Now, "Выборка данных в ПФР<TASKS>", String.Format("В период {0} по {1} найдено {2}",start,finish,list.Count()));
            return list;
        }
    }
}
