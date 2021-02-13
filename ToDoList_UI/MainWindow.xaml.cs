using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoList_UI.Components;
using ToDoList_Classes;

namespace ToDoList_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ToDoItem EditableTodo { get; set; }
        public List<ToDoItem> TodoItems { get; set; } = new List<ToDoItem> { };
        public Repository repo { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
        }

        public void RenderTodos(object sender, EventArgs e)
        {
            DateTime? selectedDate = DatePickerControl.SelectedDate;
            if (repo == null)
                return;
            bool exisis = false;
            if (selectedDate.HasValue)
            {
                TodosPanel.Children.Clear();
                TodoItems.Clear();
                foreach (Day day in repo.CurrentUser.Days)
                {
                    if(day.Date == selectedDate.Value)
                    {
                        exisis = true;
                        repo.CurrentDate = day;
                        break;
                    }
                }

                if (exisis)
                {
                    RenderTodoItems();
                }
                else
                {
                    TodosPanel.Children.Clear();
                    TodoItems.Clear();
                    Day d = new Day() { Date = selectedDate.Value };
                    repo.CurrentUser.Days.Add(d);
                    repo.CurrentDate = d;
                }
            }
            
        }

        private void RenderTodoItems()
        {
            foreach (ToDoTask task in repo.CurrentDate.Tasks)
            {
                ToDoItem tdi = new ToDoItem(task);
                tdi.SetMainWindow(this);
                tdi.TodoDescription.IsReadOnly = true;
                tdi.TodoName.IsReadOnly = true;
                tdi.TodoDescription.BorderThickness = new Thickness(0);
                tdi.TodoName.BorderThickness = new Thickness(0);
                tdi.DoneButton.Visibility = Visibility.Hidden;
                tdi.DeleteButton.Visibility = Visibility.Visible;
                if (tdi.TodoTask.IsDone)
                {
                    tdi.TodoName.TextDecorations = TextDecorations.Strikethrough;
                    tdi.TaskCheckBox.IsChecked = true;

                }
                tdi.TaskCheckBox.Visibility = Visibility.Visible;
                TodosPanel.Children.Add(tdi);
                TodoItems.Add(tdi);
                RenderActualDays();
            }
        }

        public void RenderTodos()
        {
            TodosPanel.Children.Clear();
            foreach (ToDoItem item in TodoItems)
            {
                //if (!((ToDoItem)item).IsBeingEdited)
                //{
                //    CurrentDate.Tasks.Remove(((ToDoItem)item).TodoTask);
                //    TodosPanel.Children.Remove(item as UIElement);
                //}
                TodosPanel.Children.Add(item);
            }
        }

        public void RenderActualDays()
        {
            ActualDaysListBox.Items.Clear();
            repo.CurrentUser.Days = repo.CurrentUser.Days.OrderBy(d => d.Date).ToList<Day>();
            foreach (Day day in repo.CurrentUser.Days)
            {
                if(day.Tasks.Count != 0)
                    ActualDaysListBox.Items.Add(new ListBoxItem() { Content = new TextBlock() { Text = $"{day.Date.ToShortDateString()}" }, HorizontalAlignment = HorizontalAlignment.Center });
            }
            foreach (ListBoxItem item in ActualDaysListBox.Items)
            {
                item.PreviewMouseDown += onActualDayPressed;
            }
        }

        public void onActualDayPressed(object sender, MouseButtonEventArgs e)
        {
            DateTime actualDayDate = DateTime.Parse(((TextBlock)((ListBoxItem)sender).Content).Text);
            foreach (Day day in repo.CurrentUser.Days)
            {
                if (day.Date == actualDayDate)
                {
                    repo.CurrentDate = day;
                    break;
                }
            }
            DatePickerControl.SelectedDate = actualDayDate;
            RenderActualDays();
        }


        public void onAddPressed(object sender, EventArgs e)
        {
            EditableTodo = new ToDoItem();
            EditableTodo.SetMainWindow(this);
            TodosPanel.Children.Add(EditableTodo);
            TodoItems.Add(EditableTodo);
        }

        public void SetRepository(Repository r)
        {
            repo = r;
        }

        public void MainWindow_Closing(object sender, EventArgs e)
        {
            repo.SaveDataToJson();
        }

        public void onLogoutPressed(object sender, EventArgs e)
        {
            repo.SaveDataToJson();
            LoginWindow lw = new LoginWindow();
            lw.SetRepository(repo);
            lw.Show();
            this.Close();
        }
    }
}
