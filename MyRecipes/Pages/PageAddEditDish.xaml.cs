using MyRecipes.Services;
using MyRecipes.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MyRecipes.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddEditDish.xaml
    /// </summary>
    public partial class PageAddEditDish : Page, INotifyPropertyChanged
    {
        private readonly IDataService _dataService;
        private Dish _dish;
        private string _windowTitle;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;

        public PageAddEditDish(Dish dish = null)
        {
            InitializeComponent();
            _dataService = new DatabaseService();

            Categories = _dataService.GetAllCategories();

            if (dish == null)
            {
                Dish = new Dish { ServingQuantity = 1, CategoryId = 1 };
                WindowTitle = "Добавление блюда";
                SelectedCategory = Categories.FirstOrDefault();
            }
            else
            {
                Dish = new Dish
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    ServingQuantity = dish.ServingQuantity,
                    CategoryId = dish.CategoryId,
                    RecipeLink = dish.RecipeLink,
                    Photo = dish.Photo,
                    PhotoPath = dish.PhotoPath
                };
                WindowTitle = "Редактирование блюда";
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == dish.CategoryId);
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            SelectPhotoCommand = new RelayCommand(SelectPhoto);

            DataContext = this;
        }

        public Dish Dish
        {
            get => _dish;
            set { _dish = value; OnPropertyChanged(); }
        }

        public string WindowTitle
        {
            get => _windowTitle;
            set { _windowTitle = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SelectPhotoCommand { get; }

        private void Save(object parameter)
        {
            var errors = CheckErrors();
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Dish.CategoryId = SelectedCategory?.Id ?? 1;

            if (Dish.Id == 0)
            {
                _dataService.AddRecipe(Dish);
            }
            else
            {
                _dataService.UpdateRecipe(Dish);
            }

            NavigationService.GoBack();
        }

        private void Cancel(object parameter)
        {
            NavigationService.GoBack();
        }

        private void SelectPhoto(object parameter)
        {
            // Placeholder для выбора фото
            MessageBox.Show("Функция выбора фото находится в разработке",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string CheckErrors()
        {
            var errors = "";

            if (string.IsNullOrWhiteSpace(Dish.Name))
                errors += "Введите название блюда\n";

            if (Dish.ServingQuantity <= 0)
                errors += "Количество порций должно быть больше 0\n";

            return errors.Trim();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
