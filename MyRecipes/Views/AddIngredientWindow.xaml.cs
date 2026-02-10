using MyRecipes.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MyRecipes.Views
{
    /// <summary>
    /// Логика взаимодействия для AddIngredientWindow.xaml
    /// </summary>
    public partial class AddIngredientWindow : Window
    {
        public AddIngredientWindow()
        {
            InitializeComponent();
            DataContext = new AddIngredientViewModel(this);
        }

        public Ingredient GetIngredient()
        {
            return ((AddIngredientViewModel)DataContext).Ingredient;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
