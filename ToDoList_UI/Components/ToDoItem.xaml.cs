using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoList_Classes;

namespace ToDoList_UI.Components
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ToDoItem : UserControl
    {
        public ToDoTask TodoTask { get; set; }
        public bool IsBeingEdited { get; set; } = true;
        public bool IsDone { get; set; }
        private MainWindow mainWindow;
        public ToDoItem(ToDoTask tdt)
        {
            InitializeComponent();
            TodoName.Text = tdt.Name;
            TodoDescription.Text = tdt.Description;
            TodoTask = tdt;
            this.Margin = new Thickness(0, 10, 0, 0);
        }
        
        public ToDoItem()
        {
            InitializeComponent();
            this.Margin = new Thickness(0, 10, 0, 0);
        }

        public void Delete(object sender, EventArgs e)
        {

            foreach (ToDoTask task in mainWindow.repo.CurrentDate.Tasks)
            {
                if(task == TodoTask)
                {
                    mainWindow.repo.CurrentDate.Tasks.Remove(TodoTask);
                    mainWindow.TodoItems.Remove(this);
                    break;
                } 
            }
            mainWindow.RenderTodos();
            mainWindow.RenderActualDays();
        }

        public void onDonePressed(object sender, EventArgs e)
        {
            TodoTask = new ToDoTask();
            IsBeingEdited = false;
            TodoTask.Description = this.TodoDescription.Text;
            TodoTask.Name = this.TodoName.Text;
            DoneButton.Visibility = Visibility.Hidden;
            DeleteButton.Visibility = Visibility.Visible;
            this.TodoName.IsReadOnly = true;
            this.TodoDescription.IsReadOnly = true;
            this.TodoName.BorderThickness = new Thickness(0);
            this.TodoDescription.BorderThickness = new Thickness(0);
            TaskCheckBox.Visibility = Visibility.Visible;
            mainWindow.repo.CurrentUser.Days.FirstOrDefault<Day>(day => day.Date == mainWindow.repo.CurrentDate.Date).Tasks.Add(TodoTask);
            mainWindow.RenderActualDays();
        }

        public void Checked(object sender, EventArgs e)
        {
            foreach (ToDoTask task in mainWindow.repo.CurrentDate.Tasks)
            {
                if (task == TodoTask && ((CheckBox)sender).IsChecked.Value)
                {
                    task.IsDone = true;
                    TodoName.TextDecorations = TextDecorations.Strikethrough;
                    IsDone = true;
                    //MainWindow.TodoItems[MainWindow.TodoItems.IndexOf(this)].IsDone = true;
                    break;
                }
            }
            mainWindow.RenderTodos();
        }
        public void Unchecked(object sender, EventArgs e)
        {
            foreach (ToDoTask task in mainWindow.repo.CurrentDate.Tasks)
            {
                if ((task == TodoTask && !((CheckBox)sender).IsChecked.Value))
                {
                    task.IsDone = false;
                    TodoName.TextDecorations = null;
                    IsDone = false;
                    //MainWindow.TodoItems[MainWindow.TodoItems.IndexOf(this)].IsDone = true;
                    break;
                }
            }
            mainWindow.RenderTodos();
        }

        public void SetMainWindow(MainWindow mw)
        {
            mainWindow = mw;
        }
    }
}
