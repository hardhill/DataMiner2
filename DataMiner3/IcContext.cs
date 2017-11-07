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
            using(MySqlConnection conTasks = GetConnection())
            {
                try
                {
                    string sql = "SELECT ID_TASK FROM TASKS WHERE DATEOFCOMPLATION is NULL";
                    using(MySqlCommand comSelect = new MySqlCommand(sql, conTasks))
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
                        catch(Exception e)
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
            Log.wi(DateTime.Now, "Выборка не выполненных задач.", String.Format("Всего {0}",list.Count));
            return list;
        }
    }
}
