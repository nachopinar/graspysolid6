//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent // Modificado por DIP
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();

        public Product FinalProduct { get; set; }

        // Agregado por Creator

        public bool Cooked {get; private set;} = false;


        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }


        public int GetCookTime()
        {
            int total_time = 0;
            foreach (Step step in steps)
            {
                total_time += step.Time;
            }
            return total_time;
        }

         public void Cook()
         {
            //tengo que hacer un timer que cuente
            //tengo que llamar a RecipeAdapter
            int cook_time = GetCookTime();
            CountdownTimer timer1 = new CountdownTimer();
            RecipeAdapter recipeadapter1 = new RecipeAdapter(this);
            timer1.Register(cook_time, recipeadapter1);
         }
    
    
        public class RecipeAdapter : TimerClient // Corregido el nombre de la clase
        {
            private Recipe recipe; // Corregido el nombre de la variable

            public RecipeAdapter(Recipe recipe)
            {
                this.recipe = recipe;
            }

            public void TimeOut()
            {
                this.recipe.Cooked = true;
            }
        }
    }
}

/*
Register(tiempo, client)

client -> timeout
tiempo -> tiempo receta

como no podemos modificar Recipe podemos crear otra clase.
Esa clase RecipeAdapter : TimerClient

    public RecipeaAdapter(Recipe recipe)
    {
        this.recipe = recipe;
    }
    public void TimeOut()
    {
        this.recipe.TimeOut = true;
    }

podemos meter esta clase adentro de Recipe.


*/