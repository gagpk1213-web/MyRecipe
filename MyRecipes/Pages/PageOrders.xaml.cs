using MyRecipes.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace MyRecipes.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageOrders.xaml
    /// </summary>
    public partial class PageOrders : Page, INotifyPropertyChanged
    {
        public PageOrders()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string CurrentUserInfo => $"Пользователь: {App.CurrentUser} ({App.CurrentUserRole})";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
