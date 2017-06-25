﻿//.net framework references
using System;
using System.Collections.Generic;

//solution projects refereces
using GFT.DeveloperPracticum.Kernel.Enumerators;
using GFT.DeveloperPracticum.Kernel.Models;
using GFT.DeveloperPracticum.CompositionRoot;

//third party references
using DryIoc;
using GFT.DeveloperPracticum.Abstractions.Services;

namespace GFT.DeveloperPracticum.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new Container(
                rules: Rules.Default.WithTrackingDisposableTransients()
            ))
            {
                var input = args != null
                    && args.Length > 0
                    ? string.Join("", args)
                    : "";

                ServicesBinder.Bind(container);

                var menuData = new Dictionary<Tuple<TimeOfDayType, DishType>, Dish>()
                {
                    { new Tuple<TimeOfDayType, DishType>(TimeOfDayType.Morning, DishType.Entree),  new Dish("eggs",   AllowedOrderType.Single)   },
                    { new Tuple<TimeOfDayType, DishType>(TimeOfDayType.Morning, DishType.Side),    new Dish("toast",  AllowedOrderType.Single)   },
                    { new Tuple<TimeOfDayType, DishType>(TimeOfDayType.Morning, DishType.Drink),   new Dish("coffee", AllowedOrderType.Multiple) },
                    { new Tuple<TimeOfDayType, DishType>(TimeOfDayType.Night,   DishType.Entree),  new Dish("steak",  AllowedOrderType.Single)   },
                    { new Tuple<TimeOfDayType, DishType>(TimeOfDayType.Night,   DishType.Side),    new Dish("potato", AllowedOrderType.Multiple) },
                    { new Tuple<TimeOfDayType, DishType>(TimeOfDayType.Night,   DishType.Drink),   new Dish("wine",   AllowedOrderType.Single)   },
                    { new Tuple<TimeOfDayType, DishType>(TimeOfDayType.Night,   DishType.Dessert), new Dish("cake",   AllowedOrderType.Single)   }
                };

                var inputProcesser = container.Resolve<IProcessManagerService>();
                var setupResult = inputProcesser.Process(menuData, input);

                setupResult.Match(
                    failure: ex => Console.WriteLine($"error"),
                    success: output => Console.WriteLine(output)
                );
            }
        }
    }
}
