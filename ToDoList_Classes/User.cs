using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList_Classes
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Day> Days { get; set; } = new List<Day> { };
    }
}
