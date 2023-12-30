using System;
using System.Collections.Generic;
using ToDoListApp;
using NLog;
using System.Diagnostics;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

class Program
{
    static List<ToDoItem> todoList = new List<ToDoItem>();
    private static Logger logger = LogManager.GetCurrentClassLogger();


    static void Main(string[] args)
    {
        logger.Info("Application starting...");
        LoadFromJson();
        bool running = true;

        while (running)
        {
            logger.Info("Application running...");
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
                    SaveToJson();
                    running = false;
                    break;
                default:
                    MessageHandler($"Invalid option, try again.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        logger.Info("Application ending...");

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
            SaveToJson();
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
                Console.WriteLine($"{todoList[number - 1].Description} is marked as complete.");
                logger.Info($"{todoList[number - 1].Description} is marked as complete.");
                SaveToJson();
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
        Console.Clear();
    }

    static void HandleLoadingError(Exception ex, string userMessage)
    {
        logger.Error(ex, "Error occurred while loading from JSON.");
        Console.WriteLine($"{userMessage} Error: {ex.Message}");
        Console.WriteLine("Starting with an empty to-do list. Consider restoring from a backup if available.");
        // Initialize todoList to prevent null references elsewhere
        todoList = new List<ToDoItem>();
    }

    static void SaveToJson()
    {
        try
        {
            string json = JsonConvert.SerializeObject(todoList, Formatting.Indented);
            File.WriteAllText("todolist.json", json);
            Console.WriteLine("To-do list saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving: {ex.Message}");
            logger.Error(ex, "Error occurred while saving to JSON.");
        }
    }

    static void LoadFromJson()
    {
        try
        {
            if (File.Exists("todolist.json"))
            {
                string json = File.ReadAllText("todolist.json");

                // Basic validation of JSON structure
                if (string.IsNullOrWhiteSpace(json) || (json[0] != '{' && json[0] != '['))
                {
                    throw new InvalidDataException("JSON file does not have a valid format.");
                }

                List<ToDoItem> loadedList = JsonConvert.DeserializeObject<List<ToDoItem>>(json);
                if (loadedList != null)
                {
                    todoList = loadedList;
                    Console.WriteLine("To-do list loaded successfully.");
                }
                else
                {
                    throw new InvalidDataException("JSON file is corrupted and could not be loaded.");
                }
            }
        }
        catch (JsonException ex)
        {
            HandleLoadingError(ex, "The JSON file is malformed or corrupted.");
        }
        catch (InvalidDataException ex)
        {
            HandleLoadingError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            HandleLoadingError(ex, "An unexpected error occurred while loading the to-do list.");
        }
    }
}