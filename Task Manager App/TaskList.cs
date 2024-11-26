using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Task_Manager_App
{
    public class TaskList
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public List<string> Tasks { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }

        public TaskList(int id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            Tasks = new List<string>();
            Created = DateTime.Now;
            IsDeleted = isDeleted;
        }
    }
}
