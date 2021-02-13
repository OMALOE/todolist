using System;
using System.Collections.Generic;

namespace ToDoList_Classes
{
    public class Day
    {
        public DateTime Date { get; set; }
        public List<ToDoTask> Tasks { get; set; } = new List<ToDoTask> { };
    }
}
