using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner2
{
    class IcContext
    {
        const string DATEFORMAT = "yyyy-MM-dd HH:mm:ss.fff";
        public string ConnectionString { get; set; }
        public IcContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public int SetTasks(List<Task> lstTasks)
        {
            int result = 0;
            string stage_from, stage_to, datecomming, datetaking, datecomplit,typecomplit;
            int AllAdded = 0;
            using (MySqlConnection conn = GetConnection())
            {
                try

                {
                    conn.Open();
                    foreach (Task task in lstTasks)
                    {
                        //формирование команды INSERT
                        stage_from = task.Id_stage_from == 0 ? "NULL" : task.Id_stage_from.ToString();
                        stage_to = task.Id_stage_to == 0 ? "NULL" : task.Id_stage_to.ToString();
                        datecomming =task.Dateofcomming == DateTime.MinValue ? "NULL": "'"+task.Dateofcomming.ToString(DATEFORMAT)+"'";
                        datecomplit = task.Dateofcomlation == DateTime.MinValue ? "NULL" : "'"+task.Dateofcomlation.ToString(DATEFORMAT)+"'";
                        datetaking = task.Dateoftaking == DateTime.MinValue ? "NULL" : "'"+task.Dateoftaking.ToString(DATEFORMAT)+"'";
                        typecomplit = task.Type_complation == 0 ? "NULL" : task.Type_complation.ToString();
                        string strSQL = "INSERT INTO TASKS (ID_TASK,ID_TYPE_PROCESS,ID_PROCESS,ID_STAGE_TO,ID_STAGE_FROM,TYPE_TRANSACTION," +
                                    "DATEOFCOMMING,DATEOFTAKING,DATEOFCOMPLATION,ID_USER,TYPE_COMPLATION,ID_DEPARTMENT) VALUES (";
                        strSQL = strSQL + String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},'{9}',{10},{11})", task.Id_task,task.Id_type_process,task.Id_process,
                            stage_to, stage_from, task.Type_transaction,datecomming,datetaking,datecomplit,
                            task.Id_user,typecomplit,task.Id_department);
                        try
                        {
                            using (MySqlCommand comInsert = new MySqlCommand(strSQL,conn))
                            {
                                int affected = comInsert.ExecuteNonQuery();
                                AllAdded += affected;
                               

                            }
                        }
                        catch (Exception e)
                        {
                            Log.we(DateTime.Now, "Выполнение команды добавления в БД TASKS. id="+task.Id_task.ToString(), e.Message);
                        }

                    }
                    
                    Log.wi(DateTime.Now, "Результат добавления TASKS", String.Format("Всего выбрано {0}.Добавлено {1}",lstTasks.Count(),AllAdded));

                }
                catch(Exception e)
                {
                    Log.we(DateTime.Now, "Соединение с БД Infocenter", e.Message);
                }
            }
            return AllAdded;
        }

        internal DateTime GetLastDate()
        {
            DateTime d = new DateTime(2017, 1, 1);
            using(MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand commLastDate = new MySqlCommand("SELECT MAX(DATEOFCOMMING) FROM TASKS", conn);
                try
                {
                    MySqlDataReader readLastDate = commLastDate.ExecuteReader();
                    if (readLastDate.Read())
                    {
                        d = readLastDate.GetDateTime(0);
                        Log.wi(DateTime.Now, "Последняя дата в Infocenter.TASKS ", d.ToString(DATEFORMAT));
                    }
                }
                catch (Exception e)
                {
                    Log.we(DateTime.Now, "Нахождение последней даты в таблице TASK", e.Message);
                    
                }
            }

            return d;
        }
    }
}
