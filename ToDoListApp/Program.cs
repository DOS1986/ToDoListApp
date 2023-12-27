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
                    Console.WriteLine("1 Chosen.");
                    break;
                case "2":
                    MarkTodoItem();
                    Console.WriteLine("2 Chosen.");
                    break;
                case "3":
                    running = false;
                    Console.WriteLine("3 Chosen.");
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
        Console.WriteLine("Enter the description of the to-do item:");
        string description = Console.ReadLine();
        todoList.Add(new ToDoItem { Description = description });
    }

    static void MarkTodoItem()
    {
        DisplayTodoList();
        Console.WriteLine("Enter the number of the item to mark as completed:");
        if (int.TryParse(Console.ReadLine(), out int itemNumber) && itemNumber >= 1 && itemNumber <= todoList.Count)
        {
            todoList[itemNumber - 1].IsCompleted = true;
        }
        else
        {
            Console.WriteLine("Invalid item number.");
        }
    }
}