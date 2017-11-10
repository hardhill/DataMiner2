using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner3
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

        public List<long> GetNonCompleteTasks()
        {
            List<long> list = new List<long>();
            using (MySqlConnection conTasks = GetConnection())
            {
                try
                {
                    string sql = "SELECT ID_TASK FROM TASKS WHERE DATEOFCOMPLATION is NULL";
                    using (MySqlCommand comSelect = new MySqlCommand(sql, conTasks))
                    {
                        comSelect.Connection.Open();
                        MySqlDataReader reader = comSelect.ExecuteReader();
                        try
                        {
                            while (reader.Read())
                            {
                                list.Add(reader.GetInt64(0));
                            }
                        }
                        catch (Exception e)
                        {
                            Log.we(DateTime.Now, "Читаем данные с БД ИЦ<TASKS>", e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.we(DateTime.Now, "Соединение с БД ИЦ<TASKS>", e.Message);
                }
            }
            Log.wi(DateTime.Now, "Выборка не выполненных задач.", String.Format("Всего {0}", list.Count));
            return list;
        }

        public long UpdateTasks(List<Task> lstTasks)
        {
            long count = 0;
            string stageTo, stageFrom, dateofTaking, dateofComplation, typeComplation;
            foreach (Task task in lstTasks)
            {
                using (MySqlConnection conn = GetConnection())
                {
                    stageFrom = task.Id_stage_from == 0 ? "NULL" : task.Id_stage_from.ToString();
                    stageTo = task.Id_stage_to == 0 ? "NULL" : task.Id_stage_to.ToString();
                    dateofComplation = task.Dateofcomlation == DateTime.MinValue ? "NULL" : "'" + task.Dateofcomlation.ToString(DATEFORMAT) + "'";
                    dateofTaking = task.Dateoftaking == DateTime.MinValue ? "NULL" : "'" + task.Dateoftaking.ToString(DATEFORMAT) + "'";
                    typeComplation = task.Type_complation == 0 ? "NULL" : task.Type_complation.ToString();
                    string sql = String.Format("UPDATE TASKS SET ID_STAGE_TO = {0}, ID_STAGE_FROM = {1}, DATEOFTAKING = {2}, DATEOFCOMPLATION = {3}, TYPE_COMPLATION = {4} WHERE = ID_TASK = {5}", stageTo, stageFrom, dateofTaking, dateofComplation, typeComplation, task.Id_task);
                    using(MySqlCommand comm = new MySqlCommand(sql, conn))
                    {
                        try
                        {
                            comm.Connection.Open();
                            var  i =comm.ExecuteNonQuery();
                            if (i > 0)
                            {
                                count++;
                            }
                        }
                        catch (Exception e)
                        {
                            Log.we(DateTime.Now,"Обновление данных в БД ИЦ<TASKS>",e.Message);
                        }
                    }
                }
            }
            
            return count;
        }
    }
}
