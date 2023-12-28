using System;
using System.Collections.Generic;
using ToDoListApp;

class Program
{
    static List<ToDoItem> todoList = new List<ToDoItem>();

    static void Main(string[] args)
    {
        bool running = true;

        while (running)
        {
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
                    Console.WriteLine("Invalid option, try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
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
                Console.WriteLine("The description cannot be empty. Please enter a valid description.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                AddTodoItem();
                return;
            }
            todoList.Add(new ToDoItem { Description = description });
            Console.WriteLine("To-do item added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
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
                Console.WriteLine("Item number cannot be empty. Please enter a valid item number.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                MarkTodoItem();
                return;
            }
            
            if (int.TryParse(itemNumber, out int number) && number >= 1 && number <= todoList.Count)
            {
                todoList[number - 1].IsCompleted = true;
            }
            else
            {
                Console.WriteLine("Invalid item number.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                MarkTodoItem();
                
            }
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}