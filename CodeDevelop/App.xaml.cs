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
        public static CodeDevelopEntities CodeDevelopEntities = new CodeDevelopEntities();

        public static User currentUser;

        private static CodeDevelopEntities _context;

        public static CodeDevelopEntities Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new CodeDevelopEntities();
                }
                return _context;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Освобождение ресурсов контекста при выходе из приложения
            _context?.Dispose();
        }
    }
}
