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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page, INotifyPropertyChanged
    {
        private string _login;
        private string _password;
        private string _captchaInput;
        private string _errorMessage;
        private string _currentCaptcha;
        private Frame _captchaFrame;

        public LoginPage()
        {
            InitializeComponent();
            DataContext = this;

            _captchaFrame = FindName("CaptchaFrame") as Frame;

            LoginCommand = new RelayCommand(PerformLogin);
            RefreshCaptchaCommand = new RelayCommand(RefreshCaptcha);

            RefreshCaptcha(null);
        }

        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string CaptchaInput
        {
            get => _captchaInput;
            set { _captchaInput = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand RefreshCaptchaCommand { get; }

        private void PerformLogin(object parameter)
        {
            // Отладка
            System.Diagnostics.Debug.WriteLine($"Login: '{Login}', Password: '{Password}'");

            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Введите логин и пароль";
                return;
            }

            if (CaptchaInput != _currentCaptcha)
            {
                ErrorMessage = "Неверно введена капча";
                RefreshCaptcha(null);
                return;
            }

            // Здесь должна быть логика проверки пользователя в БД
            // Пока используем демо-проверку
            if ((Login == "admin" && Password == "admin") || (Login == "user" && Password == "user"))
            {
                // Успешная авторизация
                App.CurrentUser = Login;
                App.CurrentUserRole = Login == "admin" ? "admin" : "user";

                // Переход к списку блюд
                NavigationService.Navigate(new PageListDishes());
            }
            else
            {
                ErrorMessage = "Неверный логин или пароль";
                RefreshCaptcha(null);
            }
        }

        private void RefreshCaptcha(object parameter)
        {
            // Генерация простой капчи
            var random = new System.Random();
            _currentCaptcha = "";
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            for (int i = 0; i < 4; i++)
            {
                _currentCaptcha += chars[random.Next(chars.Length)];
            }

            // Отображение капчи
            _captchaFrame.Navigate(new PageCaptcha(_currentCaptcha));
            CaptchaInput = "";
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = ((PasswordBox)sender).Password;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
