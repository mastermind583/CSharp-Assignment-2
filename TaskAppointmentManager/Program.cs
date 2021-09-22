﻿//how to print full page on outstanding tasks, modify listnavigator class?
//can appointments be completed?

using Library.TaskAppointmentManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var taskList = new List<Task>();
            var taskNavigator = new ListNavigator<Task>(taskList, 2);

            Console.WriteLine("Welcome to the Task Manager!");

            bool cont = true;
            while (cont)
            {
                Console.WriteLine("\nPlease choose an option: ");
                Console.WriteLine("1. Create a new task");
                Console.WriteLine("2. Delete an existing task");
                Console.WriteLine("3. Edit an existing task");
                Console.WriteLine("4. Complete a task");
                Console.WriteLine("5. List all outstanding tasks");
                Console.WriteLine("6. List all tasks");
                Console.WriteLine("7. Exit\n");

                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case 1:
                            //add task
                            AddOrEditTask(taskList);
                            break;
                        case 2:
                            //delete task
                            DeleteTask(taskList, taskNavigator);
                            break;
                        case 3:
                            //edit task
                            //check to see if there is anything in the list
                            if (taskList.FirstOrDefault() == null)
                                Console.WriteLine("\nThere are no tasks to edit.");
                            else
                            {
                                Console.WriteLine("\nEDITING A TASK");
                                Console.WriteLine("--------------");
                                PrintTaskList(taskNavigator, false);
                                Console.WriteLine("\nWhich task would you like to edit?");

                                //only edit a task if the task exists
                                if (int.TryParse(Console.ReadLine(), out int editChoice))
                                {
                                    var taskToEdit = taskList.FirstOrDefault(t => t.Id == editChoice);
                                    if (taskToEdit == null)
                                        Console.WriteLine("\nID \"" + editChoice + "\" not found.");
                                    else
                                        AddOrEditTask(taskList, taskToEdit);
                                }
                                else
                                    Console.WriteLine("Invalid selection!");
                            }
                            break;
                        case 4:
                            //complete task
                            CompleteTask(taskList, taskNavigator);
                            break;
                        case 5:
                            //list incomplete tasks
                            //check to see if there is anything in the list
                            if (taskList.FirstOrDefault() == null)
                                Console.WriteLine("\nThere are no outstanding tasks in the list.");
                            else
                                PrintTaskList(taskNavigator, true);
                            break;
                        case 6:
                            //list all tasks
                            //check to see if there is anything in the list
                            if (taskList.FirstOrDefault() == null)
                                Console.WriteLine("\nThere are no tasks in the list.");
                            else
                                PrintTaskList(taskNavigator, false);
                            break;
                        case 7:
                            //exit
                            cont = false;
                            break;
                        default:
                            Console.WriteLine("\nInvalid option. Try Again!");
                            break;
                    }
                }
                else
                    Console.WriteLine("\nInvalid option. Try Again!");
            }
            Console.WriteLine("\nThank you for using the Task Manager!\n");
        }

        public static void AddOrEditTask(List<Task> taskList, Task task = null)
        {
            //check to see if the user is trying to create a new task
            bool isNewTask = false;
            if (task == null)
            {
                task = new Task();
                isNewTask = true;
            }

            Console.WriteLine("\nEnter the name of the task: ");
            task.Name = Console.ReadLine();

            Console.WriteLine("\nEnter the description of the task: ");
            task.Description = Console.ReadLine();

            bool cont;
            do
            {
                Console.WriteLine("\nEnter the deadline for the task: ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    task.Deadline = date;
                    cont = false;
                }
                else
                {
                    Console.WriteLine("\nInvalid date. Try again.");
                    cont = true;
                }
            }
            while (cont);

            if (isNewTask)
            {
                taskList.Add(task);
                Console.WriteLine("\nNEW TASK: \"" + task.Name + "\" has been added to the list.");
            }
            else
                Console.WriteLine("\nTASK UPDATED: \"" + task.Name + "\".");
        }

        public static void DeleteTask(List<Task> taskList, ListNavigator<Task> taskNavigator)
        {
            if (taskList.FirstOrDefault() == null)
            {
                Console.WriteLine("\nThere are no tasks to delete.");
                return;
            }

            Console.WriteLine("\nDELETING A TASK");
            Console.WriteLine("---------------");
            PrintTaskList(taskNavigator, false);
            Console.WriteLine("\nWhich task would you like to delete?");

            if (int.TryParse(Console.ReadLine(), out int deleteChoice))
            {
                var taskToDelete = taskList.FirstOrDefault(t => t.Id == deleteChoice);
                if (taskList.Remove(taskToDelete) == true)
                    Console.WriteLine("\nTASK DELETED: \"" + taskToDelete.Name + "\" has been removed from the list.");
                else
                    Console.WriteLine("\nID \"" + deleteChoice + "\" not found.");
            }
            else
                Console.WriteLine("\nInvalid Task ID");
        }

        public static void CompleteTask(List<Task> taskList, ListNavigator<Task> taskNavigator)
        {
            if (taskList.FirstOrDefault() == null)
            {
                Console.WriteLine("\nThere are no tasks to complete.");
                return;
            }

            Console.WriteLine("\nCOMPLETING A TASK");
            Console.WriteLine("-----------------");
            PrintTaskList(taskNavigator, false);
            Console.WriteLine("\nWhich task would you like to complete?");

            if (int.TryParse(Console.ReadLine(), out int completeChoice))
            {
                var taskToComplete = taskList.FirstOrDefault(t => t.Id == completeChoice);
                if (taskList.Contains(taskToComplete) && taskToComplete.IsCompleted == false)
                {
                    taskToComplete.IsCompleted = true;
                    Console.WriteLine("\nTASK COMPLETED: \"" + taskToComplete.Name + "\" has now been completed.");
                }
                else if (taskList.Contains(taskToComplete))
                    Console.WriteLine("\n\"" + taskToComplete.Name + "\" has already been completed.");
                else
                    Console.WriteLine("\nID \"" + completeChoice + "\" not found.");
            }
            else
                Console.WriteLine("\nInvalid Task ID");
        }

       /* public static void ListAllIncomplete(List<Task> taskList)
        {
            bool empty = true;
            Console.WriteLine();

            foreach (var task in taskList)
                if (task.IsCompleted == false)
                {
                    Console.WriteLine(task.ToString());
                    empty = false;
                }

            if (empty)
                Console.WriteLine("There are no outstanding tasks in the list.");
        }

        public static void ListAll(List<Task> taskList)
        {
            if (taskList.FirstOrDefault() == null)
            {
                Console.WriteLine("\nThere are no tasks in the list.");
                return;
            }

            Console.WriteLine();
            foreach (var task in taskList)
                Console.WriteLine(task.ToString());
        }*/

        public static void PrintTaskList(ListNavigator<Task> taskNavigator, bool onlyOutstanding)
        {
            taskNavigator.GoToFirstPage();
            bool isNavigating = true;
            while (isNavigating)
            {
                Console.WriteLine();
                var page = taskNavigator.GetCurrentPage();

                //Only show oustandings tasks if onlyOutstanding is true
                if (onlyOutstanding)
                {
                    bool empty = true;
                    foreach (var item in page)
                        if (item.Value.IsCompleted == false)
                        {
                            Console.WriteLine($"{item.Value}");
                            empty = false;
                        }
                    if (empty)
                    {
                        Console.WriteLine("There are no outstanding tasks in the list.");
                        return;
                    }
                }

                else
                    foreach (var item in page)
                        Console.WriteLine($"{item.Value}");

                if (taskNavigator.HasPreviousPage)
                    Console.WriteLine("P. Previous");

                if (taskNavigator.HasNextPage)
                    Console.WriteLine("N. Next");

                if (taskNavigator.HasPreviousPage == false && taskNavigator.HasNextPage == false)
                {
                    isNavigating = false;
                    continue;
                }

                var selection = Console.ReadLine();
                if (selection.Equals("P", StringComparison.InvariantCultureIgnoreCase) && taskNavigator.HasPreviousPage)
                    taskNavigator.GoBackward();
                else if (selection.Equals("N", StringComparison.InvariantCultureIgnoreCase) && taskNavigator.HasNextPage)
                    taskNavigator.GoForward();
                else
                    isNavigating = false;
            }
        }
    }
}