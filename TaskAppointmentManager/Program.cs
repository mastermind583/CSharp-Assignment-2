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
            //Initialize the Item List and List Navigator
            var itemList = new List<Item>();
            var itemNavigator = new ListNavigator<Item>(itemList, 2);

            Console.WriteLine("Welcome to the Task Manager!");

            bool cont = true;
            while (cont)
            {
                Console.WriteLine("\nPlease choose an option: ");
                Console.WriteLine("1. Create a new item");
                Console.WriteLine("2. Delete an existing item");
                Console.WriteLine("3. Edit an existing item");
                Console.WriteLine("4. Complete a task");
                Console.WriteLine("5. List all outstanding tasks");
                Console.WriteLine("6. List all items");
                Console.WriteLine("7. Search for an item");
                Console.WriteLine("8. Exit\n");

                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case 1:
                            //add item
                            Console.WriteLine("Would you like to add a task or an appointment?");
                            Console.WriteLine("1. Task");
                            Console.WriteLine("2. Appointment\n");
                            if (int.TryParse(Console.ReadLine(), out int itemtype))
                            {
                                switch (itemtype)
                                {
                                    case 1:
                                        //add task
                                        AddOrEditItem(itemList, 1);
                                        break;
                                    case 2:
                                        //add appointment
                                        AddOrEditItem(itemList, 2);
                                        break;
                                    default:
                                        Console.WriteLine("Invalid selection!");
                                        break;
                                }
                            }
                            break;
                        case 2:
                            //delete item
                            DeleteItem(itemList, itemNavigator);
                            break;
                        case 3:
                            //edit task
                            //check to see if there is anything in the list
                            if (itemList.FirstOrDefault() == null)
                                Console.WriteLine("\nThere are no tasks to edit.");
                            else
                            {
                                Console.WriteLine("\nEDITING A TASK");
                                Console.WriteLine("--------------");
                                PrintItemList(itemNavigator);
                                Console.WriteLine("\nWhich item would you like to edit?");

                                //only edit a task if the task exists
                                if (int.TryParse(Console.ReadLine(), out int editChoice))
                                {
                                    var itemToEdit = itemList.FirstOrDefault(t => t.Id == editChoice);
                                    if (itemToEdit == null)
                                        Console.WriteLine("\nID \"" + editChoice + "\" not found.");
                                    else
                                    {
                                        if (itemToEdit is Task)
                                            AddOrEditItem(itemList, 1, itemToEdit);
                                        else
                                            AddOrEditItem(itemList, 2, itemToEdit);
                                    }
                                }
                                else
                                    Console.WriteLine("Invalid selection!");
                            }
                            break;
                        case 4:
                            //complete task
                            if (itemList.FirstOrDefault() == null)
                            {
                                Console.WriteLine("\nThere are no tasks to complete.");
                                return;
                            }

                            Console.WriteLine("\nCOMPLETING A TASK");
                            Console.WriteLine("-----------------");

                            PrintIncompleteTasks(itemList);

                            Console.WriteLine("\nWhich task would you like to complete?");

                            CompleteTask(itemList);
                            break;
                        case 5:
                            //list incomplete tasks
                            PrintIncompleteTasks(itemList);
                            break;
                        case 6:
                            //list all tasks
                            //check to see if there is anything in the list
                            if (itemList.FirstOrDefault() == null)
                                Console.WriteLine("\nThere are no tasks in the list.");
                            else
                                PrintItemList(itemNavigator);
                            break;
                        case 7:
                            //search
                            Console.WriteLine("\nEnter a string to search for in the list");
                            string str = Console.ReadLine();

                            var searchList =
                                from items in itemList
                                where items.Name.Contains(str) == true ||
                                      items.Description.Contains(str) == true ||
                                     (items is Appointment)? (items as Appointment).Attendees.Contains(str) == true : false
                                select items;

                            foreach (var item in searchList)
                                Console.WriteLine(item);
                            break;
                        case 8:
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

        public static void AddOrEditItem(List<Item> itemList, int itemtype, Item item = null)
        {
            //check to see if the user is trying to create a new task or appointment
            bool isNewItem = false;
            if (item == null)
            {
                if (itemtype == 1)
                    item = new Task();
                else
                    item = new Appointment();
                isNewItem = true;
            }

            Console.WriteLine("\nEnter the name of the item: ");
            item.Name = Console.ReadLine();

            Console.WriteLine("\nEnter the description of the item: ");
            item.Description = Console.ReadLine();

            //Enter task specific information
            if (item is Task)
            {
                Console.WriteLine("\nEnter the deadline for the task: ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    (item as Task).Deadline = date;
                else
                {
                    Console.WriteLine("\nInvalid date. Setting date to today.");
                    (item as Task).Deadline = DateTime.Today;
                }
            }

            //Enter appointment specific information
            else
            {
                Console.WriteLine("\nEnter the start date for the appointment: ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime startdate))
                    (item as Appointment).Start = startdate;
                else
                {
                    Console.WriteLine("\nInvalid date! Setting date to today.");
                    (item as Appointment).Start = DateTime.Today;
                }

                Console.WriteLine("\nEnter the end date for the appointment: ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime enddate))
                    (item as Appointment).End = enddate;
                else
                {
                    Console.WriteLine("\nInvalid date! Setting date to today.");
                    (item as Appointment).End = DateTime.Today;
                }

                //enter attendees
                Console.WriteLine("\nEnter the number of attendees:");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("\nEnter " + choice + " attendees (separate names with a newline)");

                    for (int i = 0; i < choice; i++)
                        (item as Appointment).Attendees.Add(Console.ReadLine());
                }
                else
                {
                    Console.WriteLine("\nInvalid choice! Nobody will be attending this appointment!");
                    (item as Appointment).Attendees.Clear();
                    return;
                }
            }

            if (isNewItem)
            {
                itemList.Add(item);
                Console.WriteLine("\nNEW ITEM: \"" + item.Name + "\" has been added to the list.");
            }
            else
                Console.WriteLine("\nITEM UPDATED: \"" + item.Name + "\".");
        }

        public static void DeleteItem(List<Item> itemList, ListNavigator<Item> itemNavigator)
        {
            //check to see if there is anything in the list
            if (itemList.FirstOrDefault() == null)
            {
                Console.WriteLine("\nThere are no tasks to delete.");
                return;
            }

            Console.WriteLine("\nDELETING AN ITEM");
            Console.WriteLine("----------------");
            PrintItemList(itemNavigator);
            Console.WriteLine("\nWhich task would you like to delete?");

            //Remove the item, and remove from the outstanding task list if it is a task
            if (int.TryParse(Console.ReadLine(), out int deleteChoice))
            {
                var itemToDelete = itemList.FirstOrDefault(t => t.Id == deleteChoice);
                if (itemList.Remove(itemToDelete) == true)
                    Console.WriteLine("\nITEM DELETED: \"" + itemToDelete.Name + "\" has been removed from the list.");
                else
                    Console.WriteLine("\nID \"" + deleteChoice + "\" not found.");
            }
            else
                Console.WriteLine("\nInvalid Task ID");
        }

        public static void CompleteTask(List<Item> itemList)
        {
            //complete a task if the item is an incomplete task in the list, remove it from the outstanding task list
            // otherwise, there are checks for if it is an appointment and if the task has already been completed
            if (int.TryParse(Console.ReadLine(), out int completeChoice))
            {             
                var taskToComplete = itemList.FirstOrDefault(t => t.Id == completeChoice);
                if (itemList.Contains(taskToComplete) && taskToComplete is Task && (taskToComplete as Task).IsCompleted == false)
                {
                    (taskToComplete as Task).IsCompleted = true;
                    Console.WriteLine("\nTASK COMPLETED: \"" + taskToComplete.Name + "\" has now been completed.");
                }
                else if (itemList.Contains(taskToComplete) && taskToComplete is Task)
                    Console.WriteLine("\n\"" + taskToComplete.Name + "\" has already been completed.");
                else
                    Console.WriteLine("\nID \"" + completeChoice + "\" not found.");
            }
            else
                Console.WriteLine("\nInvalid Task ID");
        }

        public static void PrintIncompleteTasks(List<Item> taskList)
        {
            //Initialize the Outstanding Task List and Outstanding Task Navigator
            var outstandingTaskList = new List<Task>();
            var outstandingTaskNavigator = new ListNavigator<Task>(outstandingTaskList, 2);

            bool empty = true;
            foreach (var task in taskList)
            {
                if (task is Task && (task as Task).IsCompleted == false)
                {
                    outstandingTaskList.Add(task as Task);
                    empty = false;
                }
            }
            if (empty)
            {
                Console.WriteLine("\nThere are no outstanding tasks in the list.");
                return;
            }

            bool isNavigating = true;
            while (isNavigating)
            {
                Console.WriteLine();
                var temp_page = outstandingTaskNavigator.GetCurrentPage();
                foreach (var task in temp_page)
                    if (task.Value.IsCompleted == false)
                        Console.WriteLine($"{task.Value}");

                if (outstandingTaskNavigator.HasPreviousPage)
                    Console.WriteLine("P. Previous");

                if (outstandingTaskNavigator.HasNextPage)
                    Console.WriteLine("N. Next");

                if (outstandingTaskNavigator.HasPreviousPage == false && outstandingTaskNavigator.HasNextPage == false)
                {
                    isNavigating = false;
                    continue;
                }

                var selection = Console.ReadLine();
                if (selection.Equals("P", StringComparison.InvariantCultureIgnoreCase) && outstandingTaskNavigator.HasPreviousPage)
                    outstandingTaskNavigator.GoBackward();
                else if (selection.Equals("N", StringComparison.InvariantCultureIgnoreCase) && outstandingTaskNavigator.HasNextPage)
                    outstandingTaskNavigator.GoForward();
                else
                    isNavigating = false;
            }
        }

        public static void PrintItemList(ListNavigator<Item> itemNavigator)
        {
            itemNavigator.GoToFirstPage();
            bool isNavigating = true;
            while (isNavigating)
            {
                Console.WriteLine();
                var page = itemNavigator.GetCurrentPage();
                foreach (var item in page)
                    Console.WriteLine($"{item.Value}");

                if (itemNavigator.HasPreviousPage)
                    Console.WriteLine("P. Previous");

                if (itemNavigator.HasNextPage)
                    Console.WriteLine("N. Next");

                if (itemNavigator.HasPreviousPage == false && itemNavigator.HasNextPage == false)
                {
                    isNavigating = false;
                    continue;
                }

                var selection = Console.ReadLine();
                if (selection.Equals("P", StringComparison.InvariantCultureIgnoreCase) && itemNavigator.HasPreviousPage)
                    itemNavigator.GoBackward();
                else if (selection.Equals("N", StringComparison.InvariantCultureIgnoreCase) && itemNavigator.HasNextPage)
                    itemNavigator.GoForward();
                else
                    isNavigating = false;
            }
        }
    }
}