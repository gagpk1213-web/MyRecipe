using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyRecipes.Services
{
    public class DemoDataService : IDataService
    {
        private List<Dish> _demoDishes;
        private List<Ingredient> _demoIngredients;

        public DemoDataService()
        {
            InitializeDemoData();
        }

        private void InitializeDemoData()
        {
            _demoDishes = new List<Dish>
            {
                new Dish
                {
                    Id = 1,
                    Name = "Борщ",
                    ServingQuantity = 6,
                    CategoryId = 1,
                    RecipeLink = "Традиционный украинский борщ с говядиной"
                },
                new Dish
                {
                    Id = 2,
                    Name = "Паста Карбонара",
                    ServingQuantity = 4,
                    CategoryId = 1,
                    RecipeLink = "Классическая итальянская паста с беконом и яйцом"
                },
                new Dish
                {
                    Id = 3,
                    Name = "Цезарь с курицей",
                    ServingQuantity = 2,
                    CategoryId = 2,
                    RecipeLink = "Популярный салат с курицей и соусом Цезарь"
                }
            };

            _demoIngredients = new List<Ingredient>
            {
                new Ingredient { Id = 1, Name = "Говядина", Cost = 300, CostForCount = 500, UnitId = 1, AvailableCount = 1000 },
                new Ingredient { Id = 2, Name = "Свекла", Cost = 50, CostForCount = 3, UnitId = 2, AvailableCount = 10 },
                new Ingredient { Id = 3, Name = "Картофель", Cost = 40, CostForCount = 4, UnitId = 2, AvailableCount = 20 },
                new Ingredient { Id = 4, Name = "Капуста", Cost = 60, CostForCount = 300, UnitId = 1, AvailableCount = 2000 },
                new Ingredient { Id = 5, Name = "Морковь", Cost = 30, CostForCount = 2, UnitId = 2, AvailableCount = 15 },
                new Ingredient { Id = 6, Name = "Лук", Cost = 25, CostForCount = 1, UnitId = 2, AvailableCount = 12 },
                new Ingredient { Id = 7, Name = "Спагетти", Cost = 80, CostForCount = 400, UnitId = 1, AvailableCount = 1000 },
                new Ingredient { Id = 8, Name = "Бекон", Cost = 200, CostForCount = 200, UnitId = 1, AvailableCount = 500 },
                new Ingredient { Id = 9, Name = "Яйца", Cost = 60, CostForCount = 3, UnitId = 2, AvailableCount = 12 },
                new Ingredient { Id = 10, Name = "Пармезан", Cost = 150, CostForCount = 100, UnitId = 1, AvailableCount = 200 },
                new Ingredient { Id = 11, Name = "Чеснок", Cost = 20, CostForCount = 2, UnitId = 3, AvailableCount = 8 },
                new Ingredient { Id = 12, Name = "Куриная грудка", Cost = 180, CostForCount = 300, UnitId = 1, AvailableCount = 600 },
                new Ingredient { Id = 13, Name = "Салат ромэн", Cost = 70, CostForCount = 200, UnitId = 1, AvailableCount = 400 },
                new Ingredient { Id = 14, Name = "Сыр пармезан", Cost = 150, CostForCount = 50, UnitId = 1, AvailableCount = 100 },
                new Ingredient { Id = 15, Name = "Сухарики", Cost = 40, CostForCount = 50, UnitId = 1, AvailableCount = 150 },
                new Ingredient { Id = 16, Name = "Соус Цезарь", Cost = 120, CostForCount = 100, UnitId = 4, AvailableCount = 300 }
            };
        }

        public List<Dish> GetAllRecipes()
        {
            return new List<Dish>(_demoDishes);
        }

        public Dish GetRecipeById(int id)
        {
            return _demoDishes.FirstOrDefault(d => d.Id == id);
        }

        public int AddRecipe(Dish recipe)
        {
            int newId = _demoDishes.Count + 1;
            recipe.Id = newId;
            _demoDishes.Add(recipe);
            return newId;
        }

        public void UpdateRecipe(Dish recipe)
        {
            var existingDish = _demoDishes.FirstOrDefault(d => d.Id == recipe.Id);
            if (existingDish != null)
            {
                existingDish.Name = recipe.Name;
                existingDish.ServingQuantity = recipe.ServingQuantity;
                existingDish.CategoryId = recipe.CategoryId;
                existingDish.RecipeLink = recipe.RecipeLink;
                existingDish.Photo = recipe.Photo;
                existingDish.PhotoPath = recipe.PhotoPath;
            }
        }

        public void DeleteRecipe(int id)
        {
            _demoDishes.RemoveAll(d => d.Id == id);
        }

        public List<Ingredient> GetAllIngredients()
        {
            return new List<Ingredient>(_demoIngredients);
        }

        public Ingredient GetIngredientById(int id)
        {
            return _demoIngredients.FirstOrDefault(i => i.Id == id);
        }

        public int AddIngredient(Ingredient ingredient)
        {
            int newId = _demoIngredients.Count + 1;
            ingredient.Id = newId;
            _demoIngredients.Add(ingredient);
            return newId;
        }

        public void UpdateIngredient(Ingredient ingredient)
        {
            var existingIngredient = _demoIngredients.FirstOrDefault(i => i.Id == ingredient.Id);
            if (existingIngredient != null)
            {
                existingIngredient.Name = ingredient.Name;
                existingIngredient.Cost = ingredient.Cost;
                existingIngredient.CostForCount = ingredient.CostForCount;
                existingIngredient.UnitId = ingredient.UnitId;
                existingIngredient.AvailableCount = ingredient.AvailableCount;
            }
        }

        public void DeleteIngredient(int id)
        {
            _demoIngredients.RemoveAll(i => i.Id == id);
        }

        public ObservableCollection<Category> GetAllCategories()
        {
            return new ObservableCollection<Category>
            {
                new Category { Id = 1, Name = "Завтраки", BackColor = "#FFE4B5" },
                new Category { Id = 2, Name = "Обеды", BackColor = "#98FB98" },
                new Category { Id = 3, Name = "Ужины", BackColor = "#DDA0DD" },
                new Category { Id = 4, Name = "Десерты", BackColor = "#F0E68C" },
                new Category { Id = 5, Name = "Салаты", BackColor = "#98FB98" },
                new Category { Id = 6, Name = "Супы", BackColor = "#87CEEB" }
            };
        }
    }
}
