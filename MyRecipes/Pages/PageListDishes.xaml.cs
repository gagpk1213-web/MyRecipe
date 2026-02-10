using MyRecipes.ViewModels;
using System.Windows.Controls;

namespace MyRecipes.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageListDishes.xaml
    /// </summary>
    public partial class PageListDishes : Page
    {
        public PageListDishes()
        {
            InitializeComponent();
            DataContext = new PageListDishesViewModel(this);
        }
    }
}
