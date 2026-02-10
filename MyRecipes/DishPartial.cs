namespace MyRecipes
{
    public partial class Dish
    {
        public decimal TotalCost
        {
            get
            {
                decimal total = 0;
                foreach (var stage in CookingStages)
                {
                    foreach (var ios in stage.IngredientOfStages)
                    {
                        total += ios.Ingredient.Cost * ios.Count;
                    }
                }
                return total;
            }
        }
    }
}
