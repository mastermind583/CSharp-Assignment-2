//how to print full page on outstanding tasks, modify listnavigator class?
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
            var itemList = new List<Item>();
            var itemNavigator = new ListNavigator<Item>(itemList, 2);       
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
                            Console.WriteLine("Would you like to add a task or an appointment?");
                            Console.WriteLine("1. Task");
                            Console.WriteLine("2. Appointment");
                            if (int.TryParse(Console.ReadLine(), out int itemtype))
                            {
                                switch(itemtype)
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
                            //delete task
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
                                PrintTaskList(itemNavigator, false);
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
                            CompleteTask(itemList, itemNavigator);
                            break;
                        case 5:
                            //list incomplete tasks
                            //check to see if there is anything in the list
                            if (itemList.FirstOrDefault() == null)
                                Console.WriteLine("\nThere are no outstanding tasks in the list.");
                            else
                                PrintTaskList(itemNavigator, true);
                            break;
                        case 6:
                            //list all tasks
                            //check to see if there is anything in the list
                            if (itemList.FirstOrDefault() == null)
                                Console.WriteLine("\nThere are no tasks in the list.");
                            else
                                PrintTaskList(itemNavigator, false);
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
                    Console.WriteLine("\nInvalid date. Setting date to today.");
                    (item as Appointment).Start = DateTime.Today;
                }

                Console.WriteLine("\nEnter the end date for the appointment: ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime enddate))
                    (item as Appointment).End = enddate;
                else
                {
                    Console.WriteLine("\nInvalid date. Setting date to today.");
                    (item as Appointment).End = DateTime.Today;
                }

                //enter attendees
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
            if (itemList.FirstOrDefault() == null)
            {
                Console.WriteLine("\nThere are no tasks to delete.");
                return;
            }

            Console.WriteLine("\nDELETING AN ITEM");
            Console.WriteLine("----------------");
            PrintTaskList(itemNavigator, false);
            Console.WriteLine("\nWhich task would you like to delete?");

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

        public static void CompleteTask(List<Item> itemList, ListNavigator<Item> itemNavigator)
        {
            if (itemList.FirstOrDefault() == null)
            {
                Console.WriteLine("\nThere are no tasks to complete.");
                return;
            }

            Console.WriteLine("\nCOMPLETING A TASK");
            Console.WriteLine("-----------------");
            PrintTaskList(itemNavigator, true);
            Console.WriteLine("\nWhich task would you like to complete?");

            if (int.TryParse(Console.ReadLine(), out int completeChoice))
            {
                var taskToComplete = itemList.FirstOrDefault(t => t.Id == completeChoice);
                if (itemList.Contains(taskToComplete) && taskToComplete.IsCompleted == false)
                {
                    taskToComplete.IsCompleted = true;
                    Console.WriteLine("\nTASK COMPLETED: \"" + taskToComplete.Name + "\" has now been completed.");
                }
                else if (itemList.Contains(taskToComplete))
                    Console.WriteLine("\n\"" + taskToComplete.Name + "\" has already been completed.");
                else
                    Console.WriteLine("\nID \"" + completeChoice + "\" not found.");
            }
            else
                Console.WriteLine("\nInvalid Task ID");
        }

        public static void PrintTaskList(ListNavigator<Item> itemNavigator, bool onlyOutstanding)
        {
            itemNavigator.GoToFirstPage();
            bool isNavigating = true;
            while (isNavigating)
            {
                Console.WriteLine();
                var page = itemNavigator.GetCurrentPage();

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