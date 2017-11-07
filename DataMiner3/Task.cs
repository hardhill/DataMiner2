using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiner3
{
    class Task
    {
        public long Id_task { get; set; }
        public long? Id_type_process { get; set; }
        public long? Id_process { get; set; }
        public int Id_stage_to { get; set; }
        public int? Id_stage_from { get; set; }
        public int? Type_transaction { get; set; }
        public DateTime Dateofcomming { get; set; }
        public DateTime Dateoftaking { get; set; }
        public DateTime Dateofcomlation { get; set; }
        public string Id_user { get; set; }
        public int? Type_complation { get; set; }
        public int Id_department { get; set; }
    }
}
