using CodeDevelop.Class;
using CodeDevelop.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
using System.Windows.Shapes;

namespace CodeDevelop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }
        
        public void userRegistration()
        {
            try
            {
                if (string.IsNullOrEmpty(regLoginbx.Text) ||
                string.IsNullOrEmpty(regPasswordBx.Password))
                {
                    FeedbackClass.Warning("Все поля обязательны для заполнения!");
                }

                else
                {
                    User newUser = new User()
                    {
                        login = regLoginbx.Text,
                        password = regPasswordBx.Password,
                        roleID = 1,
                        regDate = DateTime.Now,
                        avatar = null
                    };
                    App.context.User.Add(newUser);
                    App.context.SaveChanges();

                    FeedbackClass.Informatiom("Вы зарегистрировались!");

                    DialogResult = true;

                    this.Close();
                    HomeWindow homeWindow = new HomeWindow();
                    homeWindow.Show();
                }
            }
            catch (DbUpdateException exception)
            {
                FeedbackClass.Error($"Пользователь с таким логином уже существует. {exception.Message}");
                DialogResult = false;
            }
            catch (Exception exception)
            {
                FeedbackClass.Error($"Ошибка при добавлении пользователя. {exception.Message}");
            }
        }
        private void regCompleteBt_Click(object sender, RoutedEventArgs e)
        {
            userRegistration();
        }

        private void returnBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
