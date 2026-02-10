using MyRecipes.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MyRecipes.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IDataService _dataService;
        private readonly MainWindow _mainWindow;
        private string _isAdminVisibility;

        public MainViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _dataService = new DatabaseService();

            // Определяем видимость элементов в зависимости от роли
            IsAdminVisibility = App.CurrentUserRole == "admin" ? "Visible" : "Collapsed";

            NavigateToDishesCommand = new RelayCommand(NavigateToDishes);
            NavigateToIngredientsCommand = new RelayCommand(NavigateToIngredients);
            NavigateToOrdersCommand = new RelayCommand(NavigateToOrders);
            LogoutCommand = new RelayCommand(Logout);
        }

        public string IsAdminVisibility
        {
            get => _isAdminVisibility;
            set { _isAdminVisibility = value; OnPropertyChanged(); }
        }

        public string CurrentUser => App.CurrentUser;
        public string CurrentUserRole => App.CurrentUserRole;

        public ICommand NavigateToDishesCommand { get; }
        public ICommand NavigateToIngredientsCommand { get; }
        public ICommand NavigateToOrdersCommand { get; }
        public ICommand LogoutCommand { get; }

        private void NavigateToDishes(object parameter)
        {
            _mainWindow.NavigateToPage(new Pages.PageListDishes());
        }

        private void NavigateToIngredients(object parameter)
        {
            _mainWindow.NavigateToPage(new Pages.PageListIngredients());
        }

        private void NavigateToOrders(object parameter)
        {
            _mainWindow.NavigateToPage(new Pages.PageOrders());
        }

        private void Logout(object parameter)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                App.CurrentUser = null;
                App.CurrentUserRole = null;

                // Обновляем свойства
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(CurrentUserRole));
                IsAdminVisibility = "Collapsed";

                // Переход к странице входа
                _mainWindow.NavigateToPage(new Pages.LoginPage());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
