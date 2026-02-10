using MyRecipes.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MyRecipes.ViewModels
{
    public class AddEditRecipeViewModel : INotifyPropertyChanged
    {
        private readonly IDataService _dataService;
        private readonly Window _window;
        private Dish _dish;
        private Ingredient _selectedIngredient;
        private string _windowTitle;

        public AddEditRecipeViewModel(Window window, Dish dish = null)
        {
            _dataService = new DatabaseService(); // Используем БД
            _window = window;

            if (dish == null)
            {
                Dish = new Dish { ServingQuantity = 2, CategoryId = 1 };
                WindowTitle = "Добавление нового блюда";
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
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            AddIngredientCommand = new RelayCommand(AddIngredient);
            RemoveIngredientCommand = new RelayCommand(RemoveIngredient, CanRemoveIngredient);
        }

        public Dish Dish
        {
            get => _dish;
            set { _dish = value; OnPropertyChanged(); }
        }

        public Ingredient SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {
                _selectedIngredient = value;
                OnPropertyChanged();
                ((RelayCommand)RemoveIngredientCommand).RaiseCanExecuteChanged();
            }
        }

        public string WindowTitle
        {
            get => _windowTitle;
            set { _windowTitle = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddIngredientCommand { get; }
        public ICommand RemoveIngredientCommand { get; }

        private void Save(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Dish.Name))
            {
                MessageBox.Show("Введите название блюда!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Dish.Id == 0)
            {
                // Добавление нового блюда
                _dataService.AddRecipe(Dish);
            }
            else
            {
                // Обновление существующего блюда
                _dataService.UpdateRecipe(Dish);
            }

            _window.DialogResult = true;
            _window.Close();
        }

        private void Cancel(object parameter)
        {
            _window.DialogResult = false;
            _window.Close();
        }

        private void AddIngredient(object parameter)
        {
            var addIngredientWindow = new Views.AddIngredientWindow();
            var result = addIngredientWindow.ShowDialog();
            if (result == true)
            {
                var newIngredient = addIngredientWindow.GetIngredient();
                if (newIngredient != null)
                {
                    _dataService.AddIngredient(newIngredient);
                }
            }
        }

        private void RemoveIngredient(object parameter)
        {
            if (SelectedIngredient != null)
            {
                _dataService.DeleteIngredient(SelectedIngredient.Id);
            }
        }

        private bool CanRemoveIngredient(object parameter)
        {
            return SelectedIngredient != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
