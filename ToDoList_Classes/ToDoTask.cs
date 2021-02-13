using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList_Classes
{
    public class ToDoTask
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }
    }
}
