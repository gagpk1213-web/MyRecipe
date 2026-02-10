namespace MyRecipes
{
    public partial class Ingredient
    {
        public System.Collections.Generic.IEnumerable<Tag> Tags
        {
            get
            {
                foreach (var toi in TagOfIngredients)
                {
                    yield return toi.Tag;
                }
            }
        }

        public string PriceColor
        {
            get
            {
                if (Cost < 50) return "#4CAF50"; // зеленый
                else if (Cost < 200) return "#FF9800"; // оранжевый
                else return "#F44336"; // красный
            }
        }
    }
}
