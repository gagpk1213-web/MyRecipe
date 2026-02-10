using MyRecipes.Services;
using MyRecipes.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyRecipes.ViewModels
{
    public class PageListIngredientsViewModel : INotifyPropertyChanged
    {
        private readonly IDataService _dataService;
        private readonly Page _page;
        private ObservableCollection<Ingredient> _allIngredients;
        private ObservableCollection<Ingredient> _currentPageIngredients;
        private List<int> _pageNumbers;
        private int _currentPage = 1;
        private int _itemsPerPage = 5;
        private int _totalPages;
        private string _totalItemsText;
        private string _totalCostText;

        public PageListIngredientsViewModel(Page page)
        {
            _dataService = new DatabaseService();
            _page = page;

            AllIngredients = new ObservableCollection<Ingredient>();
            CurrentPageIngredients = new ObservableCollection<Ingredient>();
            PageNumbers = new List<int>();

            LoadIngredientsCommand = new RelayCommand(LoadIngredients);
            AddIngredientCommand = new RelayCommand(AddIngredient);
            EditIngredientCommand = new RelayCommand(EditIngredient);
            DeleteIngredientCommand = new RelayCommand(DeleteIngredient);
            FirstPageCommand = new RelayCommand(FirstPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            NextPageCommand = new RelayCommand(NextPage);
            LastPageCommand = new RelayCommand(LastPage);
            GoToPageCommand = new RelayCommand(GoToPage);

            LoadIngredients(null);
        }

        public ObservableCollection<Ingredient> AllIngredients
        {
            get => _allIngredients;
            set { _allIngredients = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Ingredient> CurrentPageIngredients
        {
            get => _currentPageIngredients;
            set { _currentPageIngredients = value; OnPropertyChanged(); }
        }

        public List<int> PageNumbers
        {
            get => _pageNumbers;
            set { _pageNumbers = value; OnPropertyChanged(); }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); }
        }

        public string TotalItemsText
        {
            get => _totalItemsText;
            set { _totalItemsText = value; OnPropertyChanged(); }
        }

        public string TotalCostText
        {
            get => _totalCostText;
            set { _totalCostText = value; OnPropertyChanged(); }
        }

        public string IsAdminVisibility => App.CurrentUserRole == "admin" ? "Visible" : "Collapsed";

        public ICommand LoadIngredientsCommand { get; }
        public ICommand AddIngredientCommand { get; }
        public ICommand EditIngredientCommand { get; }
        public ICommand DeleteIngredientCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand GoToPageCommand { get; }

        private void LoadIngredients(object parameter)
        {
            var ingredients = _dataService.GetAllIngredients();
            AllIngredients.Clear();
            foreach (var ingredient in ingredients)
            {
                AllIngredients.Add(ingredient);
            }

            UpdatePagination();
            UpdateSummaryInfo();
        }

        private void UpdatePagination()
        {
            _totalPages = (int)Math.Ceiling((double)AllIngredients.Count / _itemsPerPage);
            if (_totalPages == 0) _totalPages = 1;

            if (_currentPage > _totalPages) _currentPage = _totalPages;
            if (_currentPage < 1) _currentPage = 1;

            // Обновляем номера страниц
            PageNumbers = Enumerable.Range(1, _totalPages).ToList();

            // Загружаем текущую страницу
            LoadCurrentPage();
        }

        private void LoadCurrentPage()
        {
            var startIndex = (_currentPage - 1) * _itemsPerPage;
            var items = AllIngredients.Skip(startIndex).Take(_itemsPerPage).ToList();

            CurrentPageIngredients.Clear();
            foreach (var item in items)
            {
                CurrentPageIngredients.Add(item);
            }
        }

        private void UpdateSummaryInfo()
        {
            TotalItemsText = $"Всего наименований: {AllIngredients.Count}";
            var totalCost = AllIngredients.Sum(i => i.Cost);
            TotalCostText = $"Общая стоимость: {totalCost:N2} руб.";
        }

        private void AddIngredient(object parameter)
        {
            var addPage = new Pages.PageAddEditIngredient();
            _page.NavigationService.Navigate(addPage);
        }

        private void EditIngredient(object parameter)
        {
            if (parameter is Ingredient ingredient)
            {
                var editPage = new Pages.PageAddEditIngredient(ingredient);
                _page.NavigationService.Navigate(editPage);
            }
        }

        private void DeleteIngredient(object parameter)
        {
            if (parameter is Ingredient ingredient)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить ингредиент '{ingredient.Name}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _dataService.DeleteIngredient(ingredient.Id);
                    LoadIngredients(null);
                }
            }
        }

        private void FirstPage(object parameter)
        {
            CurrentPage = 1;
            LoadCurrentPage();
        }

        private void PreviousPage(object parameter)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadCurrentPage();
            }
        }

        private void NextPage(object parameter)
        {
            if (CurrentPage < _totalPages)
            {
                CurrentPage++;
                LoadCurrentPage();
            }
        }

        private void LastPage(object parameter)
        {
            CurrentPage = _totalPages;
            LoadCurrentPage();
        }

        private void GoToPage(object parameter)
        {
            if (parameter is int pageNumber)
            {
                CurrentPage = pageNumber;
                LoadCurrentPage();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
