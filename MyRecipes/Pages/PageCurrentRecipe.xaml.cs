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
    /// Логика взаимодействия для PageCurrentRecipe.xaml
    /// </summary>
    public partial class PageCurrentRecipe : Page, INotifyPropertyChanged
    {
        private Dish _dish;
        private readonly IDataService _dataService;

        public PageCurrentRecipe(Dish dish = null)
        {
            InitializeComponent();
            _dataService = new DemoDataService();
            Dish = dish ?? new Dish();

            EditDishCommand = new RelayCommand(EditDish);
            DeleteDishCommand = new RelayCommand(DeleteDish);
            ShowDescriptionCommand = new RelayCommand(ShowDescription);
            GoBackCommand = new RelayCommand(GoBack);

            DataContext = this;
        }

        public Dish Dish
        {
            get => _dish;
            set { _dish = value; OnPropertyChanged(); }
        }

        public ICommand EditDishCommand { get; }
        public ICommand DeleteDishCommand { get; }
        public ICommand ShowDescriptionCommand { get; }
        public ICommand GoBackCommand { get; }

        private void EditDish(object parameter)
        {
            var editPage = new PageAddEditDish(Dish);
            NavigationService.Navigate(editPage);
        }

        private void DeleteDish(object parameter)
        {
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить блюдо '{Dish.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _dataService.DeleteRecipe(Dish.Id);
                NavigationService.GoBack();
            }
        }

        private void ShowDescription(object parameter)
        {
            var descriptionPage = new PageDescription(Dish);
            NavigationService.Navigate(descriptionPage);
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
