using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyRecipes
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string CurrentUser { get; set; }
        public static string CurrentUserRole { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Открываем главное окно с LoginPage
        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
    }
}
