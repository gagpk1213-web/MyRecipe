using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyRecipes.Services
{
    public class DatabaseService : IDataService
    {
        private readonly MyRecipesEntities _context;

        public DatabaseService()
        {
            try
            {
                _context = new MyRecipesEntities();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Не удалось подключиться к базе данных. Приложение будет работать в демо-режиме.");
            }
        }

        public List<Dish> GetAllRecipes()
        {
            try
            {
                return _context.Dish.ToList();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Не удалось подключиться к базе данных. Приложение будет работать в демо-режиме.");
            }
        }

        public Dish GetRecipeById(int id)
        {
            return _context.Dish.FirstOrDefault(d => d.Id == id);
        }

        public int AddRecipe(Dish recipe)
        {
            _context.Dish.Add(recipe);
            _context.SaveChanges();
            return recipe.Id;
        }

        public void UpdateRecipe(Dish recipe)
        {
            var existingDish = _context.Dish.FirstOrDefault(d => d.Id == recipe.Id);
            if (existingDish != null)
            {
                existingDish.Name = recipe.Name;
                existingDish.ServingQuantity = recipe.ServingQuantity;
                existingDish.CategoryId = recipe.CategoryId;
                existingDish.RecipeLink = recipe.RecipeLink;
                existingDish.Photo = recipe.Photo;
                existingDish.PhotoPath = recipe.PhotoPath;
                _context.SaveChanges();
            }
        }

        public void DeleteRecipe(int id)
        {
            var dish = _context.Dish.FirstOrDefault(d => d.Id == id);
            if (dish != null)
            {
                _context.Dish.Remove(dish);
                _context.SaveChanges();
            }
        }

        public List<Ingredient> GetAllIngredients()
        {
            return _context.Ingredient.ToList();
        }

        public Ingredient GetIngredientById(int id)
        {
            return _context.Ingredient.FirstOrDefault(i => i.Id == id);
        }

        public int AddIngredient(Ingredient ingredient)
        {
            _context.Ingredient.Add(ingredient);
            _context.SaveChanges();
            return ingredient.Id;
        }

        public void UpdateIngredient(Ingredient ingredient)
        {
            var existingIngredient = _context.Ingredient.FirstOrDefault(i => i.Id == ingredient.Id);
            if (existingIngredient != null)
            {
                existingIngredient.Name = ingredient.Name;
                existingIngredient.Cost = ingredient.Cost;
                existingIngredient.CostForCount = ingredient.CostForCount;
                existingIngredient.UnitId = ingredient.UnitId;
                existingIngredient.AvailableCount = ingredient.AvailableCount;
                _context.SaveChanges();
            }
        }

        public void DeleteIngredient(int id)
        {
            var ingredient = _context.Ingredient.FirstOrDefault(i => i.Id == id);
            if (ingredient != null)
            {
                _context.Ingredient.Remove(ingredient);
                _context.SaveChanges();
            }
        }

        public ObservableCollection<Category> GetAllCategories()
        {
            try
            {
                return new ObservableCollection<Category>(_context.Category.ToList());
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Не удалось подключиться к базе данных. Приложение будет работать в демо-режиме.");
            }
        }
    }
}
