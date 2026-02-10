using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyRecipes.Services
{
    public interface IDataService
    {
        List<Dish> GetAllRecipes();
        Dish GetRecipeById(int id);
        int AddRecipe(Dish recipe);
        void UpdateRecipe(Dish recipe);
        void DeleteRecipe(int id);
        List<Ingredient> GetAllIngredients();
        Ingredient GetIngredientById(int id);
        int AddIngredient(Ingredient ingredient);
        void UpdateIngredient(Ingredient ingredient);
        void DeleteIngredient(int id);
        ObservableCollection<Category> GetAllCategories();
    }
}
