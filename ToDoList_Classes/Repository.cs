using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.IO;

namespace ToDoList_Classes
{
    public class Repository
    {
        public List<Day> Days { get; set; } = new List<Day> { };
        public Day CurrentDate { get; set; }
        public User CurrentUser { get; set; }
        public List<User> Users { get; set; }

        private Repository instance;

        public Repository Instance
        {
            get {
                if (instance == null)
                    instance = new Repository();
                return instance; 
            }
            set { instance = value; }
        }

        public Repository()
        {
            if (!File.Exists("users.json"))
                GenerateJsonData();
            RestoreDataFromJson();
        }

        public void RestoreDataFromJson()
        {
            using (var sr = new StreamReader("users.json"))
            {
                using (var jsonWriter = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer() { Formatting = Formatting.Indented };
                    Users = serializer.Deserialize<List<User>>(jsonWriter);
                }
            }
        }
        public void SaveDataToJson()
        {
            if(CurrentUser!=null)
                foreach (User user in Users)
                {
                    if(user.Username == CurrentUser.Username)
                    {
                        Users[Users.IndexOf(user)] = CurrentUser;
                        break;
                    }
                }

            using (var sw = new StreamWriter("users.json"))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    var serializer = new JsonSerializer() { Formatting = Formatting.Indented };
                    serializer.Serialize(jsonWriter, Users);
                }
            }
        }

        public void GenerateJsonData()
        {
            Users = new List<User>
            {
                new User()
                {
                    Username="dimas",
                    Password="1234",
                    Days=new List<Day>
                    {
                        new Day()
                        {
                            Date=DateTime.Today,
                            Tasks=new List<ToDoTask>
                            {
                                new ToDoTask()
                                {
                                    Date=DateTime.Today,
                                    Name="Помыть посуду",
                                    Description="Это делается в раковине",
                                    IsDone=false
                                },
                                new ToDoTask()
                                {
                                    Date=DateTime.Today,
                                    Name="Убраться в комнате",
                                    Description="Швабра в кладовке, мусорные пакеты под раковиной",
                                    IsDone=false
                                }
                            }
                        },
                        new Day()
                        {
                            Date=DateTime.Today.AddDays(-1),
                            Tasks=new List<ToDoTask>
                            {
                                new ToDoTask()
                                {
                                    Date=DateTime.Today,
                                    Name="Сделать ДЗ",
                                    Description="Это делается за столом",
                                    IsDone=false
                                },
                                new ToDoTask()
                                {
                                    Date=DateTime.Today,
                                    Name="Сходить в магазин",
                                    Description="Он находится на соседней улице",
                                    IsDone=false
                                }
                            }
                        }
                    }
                }
            };
            SaveDataToJson();
        }


    }
}
