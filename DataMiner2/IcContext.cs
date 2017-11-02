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
            string stage_from, datetaking, datecomplit;
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
                        datecomplit = task.Dateofcomlation == DateTime.MinValue ? "NULL" : task.Dateofcomlation.ToString(DATEFORMAT);
                        datetaking = task.Dateoftaking == DateTime.MinValue ? "NULL" : task.Dateoftaking.ToString(DATEFORMAT);
                        string strSQL = "INSERT INTO TASKS (ID_TASK,ID_TYPE_PROCESS,ID_PROCESS,ID_STAGE_TO,ID_STAGE_FROM,TYPE_TRANSACTION," +
                                    "DATEOFCOMMING,DATEOFTAKING,DATEOFCOMLATION,ID_USER,TYPE_COMPLATION,ID_DEPARTMENT) VALUES (";
                        strSQL = strSQL + String.Format("{0},{1},{2},{3},{4},{5},'{6}',{7},{8},'{9}',{10},{11},'{12}')", task.Id_task,task.Id_type_process,task.Id_process,
                            task.Id_stage_to, stage_from, task.Type_transaction,task.Dateofcomming.ToString(DATEFORMAT),datetaking,datecomplit,
                            task.Id_user,task.Type_complation,task.Id_department);
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
                            Log.we(DateTime.Now, "Выполнение команды добавления в БД TASKS", e.Message);
                            throw;
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
    }
}
