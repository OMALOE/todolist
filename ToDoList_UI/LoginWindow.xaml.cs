using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToDoList_Classes;

namespace ToDoList_UI
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private Repository repo = new Repository();
        public LoginWindow()
        {
            InitializeComponent();
        }

        public void onTextChanged(object sender, EventArgs e)
        {
            if(UsernameInput.Text != null && PasswordInput.Password != null && UsernameInput.Text.Length != 0 &&  PasswordInput.Password.Length != 0)
            {
                LoginButton.IsEnabled = true;
            }
        }

        public void Login(object sender, EventArgs e)
        {
            repo.CurrentUser = repo.Users.FirstOrDefault<User>(u => u.Username == UsernameInput.Text && u.Password == PasswordInput.Password);
            if (repo.CurrentUser != null)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.SetRepository(repo);
                mainWindow.DatePickerControl.SelectedDate = DateTime.Today;
                mainWindow.RenderActualDays();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect Username or Password", "Unable to login!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Register(object sender, EventArgs e)
        {
            User checkUser = repo.Users.FirstOrDefault<User>(u => u.Username == UsernameInput.Text);
            if(checkUser != null)
            {
                MessageBox.Show("User with that username already exists", "Unable to register!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(UsernameInput.Text== "" || PasswordInput.Password == "")
            {
                MessageBox.Show("Inaproppriate input! Enter at least 1 symbol in each field...", "Unable to register!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            User newUser = new User();
            newUser.Username = UsernameInput.Text;
            newUser.Password = PasswordInput.Password;
            repo.Users.Add(newUser);
            Login(sender, e);
            //repo.SaveDataToJson();
        }

        public void SetRepository(Repository r)
        {
            this.repo = r.Instance;
        }
    }
}
