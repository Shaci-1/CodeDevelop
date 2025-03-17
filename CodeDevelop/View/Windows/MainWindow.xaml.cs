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
using CodeDevelop.Class;
using CodeDevelop.Model;
using CodeDevelop.View.Windows;

namespace CodeDevelop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool Validation()

        {
            if (loginBx.Text == string.Empty && passwordBx.Password == string.Empty)
            {
                FeedbackClass.Warning("Поля для ввода логина и пароля обязательны для заполнения. Введите логин и пароль.");
                return false;
            }
            else
            {
                if (loginBx.Text == string.Empty)
                {
                    FeedbackClass.Warning("Поля для ввода логина обязательно для заполнения. Введите логин");
                    return false;
                }
                else if (passwordBx.Password == string.Empty)
                {
                    FeedbackClass.Warning("Поля для ввода пароль бязательно для заполнения. Введите пароль");
                    return false;
                }
            }
            
            return true;
        }
        public void Authorization()
        {
            switch (App.currentUser.roleID)
            {
                case 1:
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    break;
                case 2:
                    TechSuppWindow techSuppWindow = new TechSuppWindow();
                    techSuppWindow.Show();
                    break;
                case 3:
                    FeedbackClass.Informatiom("Вход в качестве пользователя");
                    break;
            }
            Close();
        }

        public void Authentication()
        {
            //Проверка
            App.currentUser = App.Context.User.FirstOrDefault(user => user.login == loginBx.Text && user.password == passwordBx.Password);

            if (App.currentUser == null)
            {
                FeedbackClass.Error($"Вы ввели неверный логин или пароль. Пожалуйста, проверьте еще раз введеные данные!");
            }
            else
            {
                FeedbackClass.Informatiom("Вы успешно авторизовались");
                Authorization();
            }

        }
        private void NextBt_Click(object sender, RoutedEventArgs e)
        {
            if (Validation() == true)
            {
                Authentication();
            }

        }

        private void RegBt_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();

            this.Close();
        }
    }
}
