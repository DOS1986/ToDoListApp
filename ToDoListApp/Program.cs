using System;
using System.Collections.Generic;
using ToDoListApp;
using NLog;
using System.Diagnostics;

class Program
{
    static List<ToDoItem> todoList = new List<ToDoItem>();
    private static Logger logger = LogManager.GetCurrentClassLogger();


    static void Main(string[] args)
    {
        logger.Info("Application starting...");
        bool running = true;

        while (running)
        {
            logger.Info("Application running...");
            Console.Clear();
            DisplayTodoList();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Add To-Do Item");
            Console.WriteLine("2. Mark To-Do Item as Completed");
            Console.WriteLine("3. Exit");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddTodoItem();
                    break;
                case "2":
                    MarkTodoItem();
                    break;
                case "3":
                    running = false;
                    break;
                default:
                    MessageHandler($"Invalid option, try again.");
                    break;
            }

        }

    }

    static void DisplayTodoList()
    {
        Console.WriteLine("To-Do List:");
        for (int i = 0; i < todoList.Count; i++)
        {
            var todo = todoList[i];
            string status = todo.IsCompleted ? "[Completed]" : "[Pending]";
            Console.WriteLine($"{i + 1}. {todo.Description} {status}");
        }
    }

    static void AddTodoItem()
    {
        try
        {
     
            Console.WriteLine("Enter the description of the to-do item:");
            string description = Console.ReadLine();
            if (string.IsNullOrEmpty(description))
            {
                MessageHandler($"The description cannot be empty. Please enter a valid description.");
                logger.Warn("Attempted to add an empty to-do item.");
                AddTodoItem();
                return;
            }
            todoList.Add(new ToDoItem { Description = description });
            Console.WriteLine("To-do item added successfully!");
            logger.Info($"To-do item added: {description}");
        }
        catch (Exception ex)
        {
            MessageHandler($"An unexpected error occurred: {ex.Message}");
            logger.Error(ex, "Error occurred in AddTodoItem.");
        }
        
    }

    static void MarkTodoItem()
    {
        try
        {
            DisplayTodoList();
            Console.WriteLine("Enter the number of the item to mark as completed:");
            string itemNumber = Console.ReadLine();
            if (string.IsNullOrEmpty(itemNumber))
            {
                MessageHandler($"Item number cannot be empty. Please enter a valid item number.");
                logger.Warn("Attempted to mark an empty to-do item.");
                MarkTodoItem();
                return;
            }
            
            if (int.TryParse(itemNumber, out int number) && number >= 1 && number <= todoList.Count)
            {
                todoList[number - 1].IsCompleted = true;
                logger.Info($"{todoList[number - 1].Description} is marked as complete.");
            }
            else
            {
                MessageHandler($"Invalid item number.");
                logger.Warn("Invalid item number.");
                MarkTodoItem();
            }
        } 
        catch (Exception ex)
        {
            MessageHandler($"An unexpected error occurred: {ex.Message}");
            logger.Error(ex, "Error occurred in AddTodoItem.");
        }
    }

    static void MessageHandler(string message)
    {
        Console.WriteLine($"{message}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}