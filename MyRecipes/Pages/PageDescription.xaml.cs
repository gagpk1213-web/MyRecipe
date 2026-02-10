using MyRecipes.Services;
using MyRecipes.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyRecipes.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageDescription.xaml
    /// </summary>
    public partial class PageDescription : Page, INotifyPropertyChanged
    {
        private Dish _dish;
        private readonly IDataService _dataService;

        public PageDescription(Dish dish = null)
        {
            InitializeComponent();
            _dataService = new DemoDataService();
            Dish = dish ?? new Dish();

            ShareRecipeCommand = new RelayCommand(ShareRecipe);
            PrintRecipeCommand = new RelayCommand(PrintRecipe);
            GoBackCommand = new RelayCommand(GoBack);

            DataContext = this;
        }

        public Dish Dish
        {
            get => _dish;
            set { _dish = value; OnPropertyChanged(); }
        }

        public ICommand ShareRecipeCommand { get; }
        public ICommand PrintRecipeCommand { get; }
        public ICommand GoBackCommand { get; }

        private void ShareRecipe(object parameter)
        {
            // Placeholder для функции поделиться
            MessageBox.Show("Функция 'Поделиться рецептом' находится в разработке",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PrintRecipe(object parameter)
        {
            // Placeholder для функции печати
            MessageBox.Show("Функция 'Печать рецепта' находится в разработке",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GoBack(object parameter)
        {
            NavigationService.GoBack();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
