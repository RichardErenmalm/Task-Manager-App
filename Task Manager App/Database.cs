using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Task_Manager_App
{
    public class Database
    {
        [JsonPropertyName("Lists")]
        public List<TaskList> UserLists { get; set; }

        [JsonPropertyName("ListHistory")]
        public List <History> HistoryList { get; set; }

        [JsonPropertyName("IdCounter")]
        public int IdCounter { get; set; }

        public Database()
        {
            UserLists = new List<TaskList>();
            HistoryList = new List<History>();
            
        }
    }
}
