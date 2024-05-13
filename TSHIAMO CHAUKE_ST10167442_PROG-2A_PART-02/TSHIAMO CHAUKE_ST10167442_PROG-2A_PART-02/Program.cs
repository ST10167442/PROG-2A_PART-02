using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApplication
{
    // Event handler for recipe calories exceeding limit
    public delegate void ExceededCaloriesEventHandler(string recipeName, int totalCalories);

    class Program
    {
        static void Main(string[] args)
        {
            List<Cooking> recipes = new List<Cooking>();

            while (true)
            {
                Console.WriteLine("1. Add a new recipe");
                Console.WriteLine("2. Display all recipes");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Cooking recipe = CreateRecipe();
                        recipes.Add(recipe);
                        Console.WriteLine("Recipe added successfully!");
                        break;
                    case "2":
                        ShowAllRecipes(recipes);
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static Cooking CreateRecipe()
        {
            Cooking recipe = new Cooking();

            Console.WriteLine("Enter recipe name:");
            string name = Console.ReadLine();
            recipe.SetName(name);

            Console.WriteLine("Enter number of ingredients: ");
            int numIngredients = int.Parse(Console.ReadLine());

            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine($"Enter ingredient {i + 1} name:");
                string ingredientName = Console.ReadLine();

                Console.WriteLine($"Enter quantity of {ingredientName}:");
                double quantity = double.Parse(Console.ReadLine());

                Console.WriteLine($"Enter unit for {ingredientName}:");
                string units = Console.ReadLine();

                Console.WriteLine($"Enter calories for {ingredientName}:");
                int calories = int.Parse(Console.ReadLine());

                recipe.IncludeIngredient(ingredientName, quantity, units, calories);
            }

            Console.WriteLine("Enter number of steps: ");
            int numSteps = int.Parse(Console.ReadLine());

            for (int i = 0; i < numSteps; i++)
            {
                Console.WriteLine($"Enter step {i + 1}:");
                string step = Console.ReadLine();
                recipe.AddStep(step);
            }

            return recipe;
        }

        static void ShowAllRecipes(List<Cooking> recipes)
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes available.");
                return;
            }

            Console.WriteLine("\nList of all recipes:");

            foreach (var recipe in recipes.OrderBy(r => r.GetName()))
            {
                Console.WriteLine($"- {recipe.GetName()}");
            }
        }
    }

    class Cooking
    {
        private string recipeName;
        private List<Ingredient> ingredients = new List<Ingredient>();
        private List<string> steps = new List<string>();

        public event ExceededCaloriesEventHandler CaloriesExceeded;

        public void SetName(string name)
        {
            recipeName = name;
        }

        public string GetName()
        {
            return recipeName;
        }

        public void IncludeIngredient(string name, double quantity, string units, int calories)
        {
            Ingredient ingredient = new Ingredient(name, quantity, units, calories);
            ingredients.Add(ingredient);
        }

        public void AddStep(string step)
        {
            steps.Add(step);
        }

        public void DisplayRecipe()
        {
            Console.WriteLine($"\nRecipe: {recipeName}");
            Console.WriteLine("\nIngredients:");

            int totalCalories = 0;

            foreach (var ingredient in ingredients)
            {
                Console.WriteLine($"- {ingredient.Quantity} {ingredient.Units} of {ingredient.Name} ({ingredient.Calories} calories)");
                totalCalories += ingredient.Calories;
            }

            Console.WriteLine("\nTotal Calories: " + totalCalories);

            if (totalCalories > 300)
            {
                CaloriesExceeded?.Invoke(recipeName, totalCalories);
            }

            Console.WriteLine("\nSteps:");

            for (int i = 0; i < steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {steps[i]}");
            }
        }
    }

    class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Units { get; set; }
        public int Calories { get; set; }

        public Ingredient(string name, double quantity, string units, int calories)
        {
            Name = name;
            Quantity = quantity;
            Units = units;
            Calories = calories;
        }
    }
}