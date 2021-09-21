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
            bool cont = true;

            Console.WriteLine("Welcome to the Task Manager!");
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
                            AddTask(taskList);
                            break;
                        case 2:
                            //delete task
                            DeleteTask(taskList);
                            break;
                        case 3:
                            //edit task
                            EditTask(taskList);
                            break;
                        case 4:
                            //complete task
                            CompleteTask(taskList);
                            break;
                        case 5:
                            //list incomplete tasks
                            ListAllIncomplete(taskList);
                            break;
                        case 6:
                            //list all tasks
                            ListAll(taskList);
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
            /*
            if (ticket is SupportTicket)
            {
                Console.WriteLine("What is the deadline?");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
                {
                    (ticket as SupportTicket).Deadline = deadline;
                }
                else
                {
                    Console.WriteLine("Invalid choice, defaulting to today");
                    (ticket as SupportTicket).Deadline = DateTime.Today;
                }
            }
            
            ticket.DateAdded = DateTime.Now;*/

            if (isNewTask)
            {
                taskList.Add(task);
                Console.WriteLine("\nNEW TASK: \"" + newTask.Name + "\" has been added to the list.");
            }
        }

        public static void DeleteTask(List<Task> taskList)
        {
            if (taskList.FirstOrDefault() == null)
            {
                Console.WriteLine("\nThere are no tasks to delete.");
                return;
            }

            Console.WriteLine("\nWhich task would you like to delete?");
            ListAll(taskList);
            Console.WriteLine();

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

        public static void EditTask(List<Task> taskList)
        {
            if (taskList.FirstOrDefault() == null)
            {
                Console.WriteLine("\nThere are no tasks to edit.");
                return;
            }

            Console.WriteLine("\nWhich task would you like to edit?");
            ListAll(taskList);
            Console.WriteLine();

            if (int.TryParse(Console.ReadLine(), out int editChoice))
            {
                var taskToEdit = taskList.FirstOrDefault(t => t.Id == editChoice);
                if (taskList.Contains(taskToEdit))
                {
                    Console.WriteLine("\nEnter the new name of the task: ");
                    taskToEdit.Name = Console.ReadLine();

                    Console.WriteLine("\nEnter the new description of the task: ");
                    taskToEdit.Description = Console.ReadLine();

                    bool cont;
                    do
                    {
                        Console.WriteLine("\nEnter the deadline for the task: ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        {
                            taskToEdit.Deadline = date;
                            cont = false;
                            Console.WriteLine("\nTASK UPDATED: \"" + taskToEdit.Name + "\".");
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid date. Try again.");
                            cont = true;
                        }
                    }
                    while (cont);
                }
                else
                    Console.WriteLine("\nID \"" + editChoice + "\" not found.");
            }
            else
                Console.WriteLine("\nInvalid Task ID");
        }

        public static void CompleteTask(List<Task> taskList)
        {
            if (taskList.FirstOrDefault() == null)
            {
                Console.WriteLine("\nThere are no tasks to complete.");
                return;
            }

            Console.WriteLine("\nWhich task would you like to complete?");
            ListAll(taskList);
            Console.WriteLine();

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

        public static void ListAllIncomplete(List<Task> taskList)
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
        }
    }
}