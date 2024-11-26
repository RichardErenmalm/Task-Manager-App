using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Task_Manager_App
{
    public class History
    {
        public string Changed { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Tasks { get; set; }
        public DateTime Created { get; set; }
        public DateTime TimeOfUpdate {  get; set; }
        public bool IsDeleted { get; set; }

        public  History(int id, string name, List<string> tasks, DateTime created, bool isDeleted, string changed) 
        {
            Id = id;
            Name = name;
            Created = created;
            TimeOfUpdate = DateTime.Now;
            Changed = changed;
            IsDeleted = isDeleted;
            Tasks = new List<string>(tasks);

            if (isDeleted) 
            {
                Tasks.Clear();
            }
        }
    }
}
