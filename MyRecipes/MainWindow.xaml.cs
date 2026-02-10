using MyRecipes.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MyRecipes
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Frame _mainFrame;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
            Title = "Nyam-Nyam - Мои рецепты";

            // Находим Frame в XAML
            _mainFrame = FindName("MainFrame") as Frame;

            // Загружаем страницу входа
            NavigateToPage(new Pages.LoginPage());
        }

        public void NavigateToPage(Page page)
        {
            if (_mainFrame != null)
            {
                _mainFrame.Navigate(page);
            }
        }
    }
}
