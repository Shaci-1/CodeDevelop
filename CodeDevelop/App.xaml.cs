using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CodeDevelop.Model;

namespace CodeDevelop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CodeDevelopEntities1 context = new CodeDevelopEntities1();

        public static User currentUser;        

    }
}
