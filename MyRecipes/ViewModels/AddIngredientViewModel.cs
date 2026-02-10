using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MyRecipes.ViewModels
{
    public class AddIngredientViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;
        private Ingredient _ingredient;

        public AddIngredientViewModel(Window window)
        {
            _window = window;
            Ingredient = new Ingredient { CostForCount = 1, UnitId = 1, AvailableCount = 1 };

            AddCommand = new RelayCommand(Add);
            CancelCommand = new RelayCommand(Cancel);
        }

        public Ingredient Ingredient
        {
            get => _ingredient;
            set { _ingredient = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        private void Add(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Ingredient.Name))
            {
                MessageBox.Show("Введите название ингредиента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Ingredient.CostForCount <= 0)
            {
                MessageBox.Show("Количество должно быть больше 0!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _window.DialogResult = true;
            _window.Close();
        }

        private void Cancel(object parameter)
        {
            _window.DialogResult = false;
            _window.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
