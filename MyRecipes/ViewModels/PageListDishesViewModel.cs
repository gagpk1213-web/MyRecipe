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
    public class PageListDishesViewModel : INotifyPropertyChanged
    {
        private readonly IDataService _dataService;
        private readonly Page _page;
        private ObservableCollection<Dish> _allDishes;
        private ObservableCollection<Dish> _filteredDishes;
        private ObservableCollection<Dish> _currentPageDishes;
        private ObservableCollection<Category> _categories;
        private List<int> _pageNumbers;
        private Dish _selectedDish;
        private Category _selectedCategory;
        private string _searchText;
        private int _currentPage = 1;
        private int _itemsPerPage = 6;
        private int _totalPages;
        private string _isAdminVisibility;

        public PageListDishesViewModel(Page page)
        {
            _dataService = new DatabaseService();
            _page = page;

            AllDishes = new ObservableCollection<Dish>();
            FilteredDishes = new ObservableCollection<Dish>();
            Categories = new ObservableCollection<Category>();

            LoadDishesCommand = new RelayCommand(LoadDishes);
            AddDishCommand = new RelayCommand(AddDish);
            ViewDishCommand = new RelayCommand(ViewDish);
            EditDishCommand = new RelayCommand(EditDish);
            DeleteDishCommand = new RelayCommand(DeleteDish);
            SearchCommand = new RelayCommand(SearchDishes);
            FirstPageCommand = new RelayCommand(FirstPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            NextPageCommand = new RelayCommand(NextPage);
            LastPageCommand = new RelayCommand(LastPage);
            GoToPageCommand = new RelayCommand(GoToPage);

            LoadDishes(null);
            LoadCategories();
        }

        public ObservableCollection<Dish> AllDishes
        {
            get => _allDishes;
            set { _allDishes = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Dish> FilteredDishes
        {
            get => _filteredDishes;
            set { _filteredDishes = value; OnPropertyChanged(); UpdatePagination(); }
        }

        public ObservableCollection<Dish> CurrentPageDishes
        {
            get => _currentPageDishes;
            set { _currentPageDishes = value; OnPropertyChanged(); }
        }

        public List<int> PageNumbers
        {
            get => _pageNumbers;
            set { _pageNumbers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }

        public Dish SelectedDish
        {
            get => _selectedDish;
            set { _selectedDish = value; OnPropertyChanged(); }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        public string IsAdminVisibility => App.CurrentUserRole == "admin" ? "Visible" : "Collapsed";

        public ICommand LoadDishesCommand { get; }
        public ICommand AddDishCommand { get; }
        public ICommand ViewDishCommand { get; }
        public ICommand EditDishCommand { get; }
        public ICommand DeleteDishCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand GoToPageCommand { get; }

        private void LoadDishes(object parameter)
        {
            var dishes = _dataService.GetAllRecipes();
            AllDishes.Clear();
            foreach (var dish in dishes)
            {
                AllDishes.Add(dish);
            }

            ApplyFilters();
        }

        private void LoadCategories()
        {
            var categories = _dataService.GetAllCategories();
            Categories.Clear();
            Categories.Add(new Category { Id = 0, Name = "Все категории" }); // Добавляем вариант "Все"
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
            SelectedCategory = Categories.First(); // Выбираем "Все категории" по умолчанию
        }

        private void ApplyFilters()
        {
            var filtered = AllDishes.AsEnumerable();

            // Фильтр по категории
            if (SelectedCategory != null && SelectedCategory.Id != 0)
            {
                filtered = filtered.Where(d => d.CategoryId == SelectedCategory.Id);
            }

            // Фильтр по поиску
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(d =>
                    d.Name.ToLower().Contains(SearchText.ToLower()) ||
                    (d.RecipeLink != null && d.RecipeLink.ToLower().Contains(SearchText.ToLower())));
            }

            FilteredDishes.Clear();
            foreach (var dish in filtered)
            {
                FilteredDishes.Add(dish);
            }
        }

        private void AddDish(object parameter)
        {
            var addPage = new Pages.PageAddEditDish();
            _page.NavigationService.Navigate(addPage);
        }

        private void ViewDish(object parameter)
        {
            if (parameter is Dish dish)
            {
                if (App.CurrentUserRole == "admin")
                {
                    var viewPage = new Pages.PageCurrentRecipe(dish);
                    _page.NavigationService.Navigate(viewPage);
                }
                else
                {
                    // Для user - добавить в заказы (предположим, переход к PageOrders)
                    var ordersPage = new Pages.PageOrders();
                    _page.NavigationService.Navigate(ordersPage);
                }
            }
        }

        private void EditDish(object parameter)
        {
            if (parameter is Dish dish)
            {
                var editPage = new Pages.PageAddEditDish(dish);
                _page.NavigationService.Navigate(editPage);
            }
        }

        private void DeleteDish(object parameter)
        {
            if (parameter is Dish dish)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить блюдо '{dish.Name}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _dataService.DeleteRecipe(dish.Id);
                    LoadDishes(null);
                }
            }
        }

        private void SearchDishes(object parameter)
        {
            ApplyFilters();
        }

        private void UpdatePagination()
        {
            _totalPages = (int)Math.Ceiling((double)FilteredDishes.Count / _itemsPerPage);
            if (_totalPages == 0) _totalPages = 1;

            if (_currentPage > _totalPages) _currentPage = _totalPages;
            if (_currentPage < 1) _currentPage = 1;

            PageNumbers = Enumerable.Range(1, _totalPages).ToList();
            LoadCurrentPage();
        }

        private void LoadCurrentPage()
        {
            var startIndex = (_currentPage - 1) * _itemsPerPage;
            var items = FilteredDishes.Skip(startIndex).Take(_itemsPerPage).ToList();

            CurrentPageDishes = new ObservableCollection<Dish>(items);
        }

        private void FirstPage(object parameter)
        {
            _currentPage = 1;
            LoadCurrentPage();
        }

        private void PreviousPage(object parameter)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadCurrentPage();
            }
        }

        private void NextPage(object parameter)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadCurrentPage();
            }
        }

        private void LastPage(object parameter)
        {
            _currentPage = _totalPages;
            LoadCurrentPage();
        }

        private void GoToPage(object parameter)
        {
            if (parameter is int pageNumber)
            {
                _currentPage = pageNumber;
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
