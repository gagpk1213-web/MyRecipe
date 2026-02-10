using MyRecipes.Services;
using MyRecipes.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MyRecipes.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddEditIngredient.xaml
    /// </summary>
    public partial class PageAddEditIngredient : Page, INotifyPropertyChanged
    {
        private readonly IDataService _dataService;
        private Ingredient _ingredient;
        private string _windowTitle;

        public PageAddEditIngredient(Ingredient ingredient = null)
        {
            InitializeComponent();
            _dataService = new DatabaseService(); // Используем БД

            if (ingredient == null)
            {
                Ingredient = new Ingredient { CostForCount = 1, UnitId = 1, AvailableCount = 1 };
                WindowTitle = "Добавление ингредиента";
            }
            else
            {
                Ingredient = new Ingredient
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                    Cost = ingredient.Cost,
                    CostForCount = ingredient.CostForCount,
                    UnitId = ingredient.UnitId,
                    AvailableCount = ingredient.AvailableCount
                };
                WindowTitle = "Редактирование ингредиента";
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            DataContext = this;
        }

        public Ingredient Ingredient
        {
            get => _ingredient;
            set { _ingredient = value; OnPropertyChanged(); }
        }

        public string WindowTitle
        {
            get => _windowTitle;
            set { _windowTitle = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save(object parameter)
        {
            var errors = CheckErrors();
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Ingredient.Id == 0)
            {
                _dataService.AddIngredient(Ingredient);
            }
            else
            {
                _dataService.UpdateIngredient(Ingredient);
            }

            // Возврат на предыдущую страницу
            NavigationService.GoBack();
        }

        private void Cancel(object parameter)
        {
            NavigationService.GoBack();
        }

        private string CheckErrors()
        {
            var errors = "";

            if (string.IsNullOrWhiteSpace(Ingredient.Name))
                errors += "Введите название ингредиента\n";

            if (Ingredient.Cost <= 0)
                errors += "Цена должна быть больше 0\n";

            if (Ingredient.CostForCount <= 0)
                errors += "Количество для цены должно быть больше 0\n";

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
