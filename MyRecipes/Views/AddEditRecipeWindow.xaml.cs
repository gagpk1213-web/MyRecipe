using MyRecipes.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MyRecipes.Views
{
    /// <summary>
    /// Логика взаимодействия для AddEditRecipeWindow.xaml
    /// </summary>
    public partial class AddEditRecipeWindow : Window
    {
        public AddEditRecipeWindow(Dish dish = null)
        {
            InitializeComponent();
            DataContext = new AddEditRecipeViewModel(this, dish);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
