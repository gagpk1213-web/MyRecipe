using MyRecipes.ViewModels;
using System.Windows.Controls;

namespace MyRecipes.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageListIngredients.xaml
    /// </summary>
    public partial class PageListIngredients : Page
    {
        public PageListIngredients()
        {
            InitializeComponent();
            DataContext = new PageListIngredientsViewModel(this);
        }
    }
}
