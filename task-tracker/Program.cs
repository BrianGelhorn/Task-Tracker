using System.Text;
using Microsoft.Win32.SafeHandles;

namespace task_tracker;
using System.Text.Json;
class Program
{
    private const string TaskDataBasePath = "taskDatabase.json";
    private static readonly string[] StatusTypes = ["todo", "in-progress", "done"];

    public class TaskItem
    {
        public required int Id { get; set; }
        public required string Description { get; set; }
        public  required string Status  { get; set; }
        public required DateTime CreatedAt  { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
    
    static void Main(string[] args)
    {
        if (!File.Exists(TaskDataBasePath))
        {
            File.WriteAllText(TaskDataBasePath, "[\n]");
        }
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: task-tracker <command> [arguments]");
            return;
        }

        switch (args[0])
        {
            case "help":
                Console.WriteLine("""
                                  Usage: task-tracker <command> [ARGUMENTS]
                                  Commands:
                                    add "description"           Add a task with the specified description
                                    update <id> "description"   Update the description of the task with the specified id
                                    delete <id>                 Delete the task with the specified id
                                    list                        List all the tasks
                                    list [status]               List all the tasks with the specified status
                                    mark-in-progress <id>       Change the status of the task with the specified id to "in-progress"
                                    mark-done <id>              Change the status of the task with the specified id to "done"
                                  """);
                break;
            case "add":
                if (args.Length > 1)
                {
                    AddTask(args[1]);   
                }
                else
                {
                    Console.WriteLine("Error: No task description was specified");
                }
                break;
            case "update":
                if (args.Length > 2)
                {
                    bool couldParse = int.TryParse(args[1], out int id);
                    if (couldParse)
                    {
                        UpdateTask(id, args[2]);
                    }
                    else
                    {
                        Console.WriteLine("The specified id is not valid");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Missing Arguments");
                }
                break;
            case "delete":
                if (args.Length > 1)
                {
                    bool couldParse = int.TryParse(args[1], out int id);
                    if (couldParse)
                    {
                        DeleteTask(id);
                    }
                    else
                    {
                        Console.WriteLine("The specified id is not valid");
                    }
                }
                break;
            case "list":
                if (args.Length > 1)
                {
                    if (StatusTypes.Contains(args[1]))
                    {
                        ListTasks(args[1]);   
                    }
                    else
                    {
                        Console.WriteLine("Error: The specified status is not valid");
                    }
                }
                else
                {
                    ListTasks();
                }
                break;
            case "mark-in-progress":
                if (args.Length > 1)
                {
                    bool couldParse = int.TryParse(args[1], out int id);
                    if (couldParse)
                    {
                        //Change status to in-progress
                        ChangeStatus(id, StatusTypes[1]); 
                    }
                    else
                    {
                        Console.WriteLine("The specified id is not valid");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Missing id");
                }
                break;
            case "mark-done":
                if (args.Length > 1)
                {
                    bool couldParse = int.TryParse(args[1], out int id);
                    if (couldParse)
                    {
                        //Change status to done
                        ChangeStatus(id, StatusTypes[2]); 
                    }
                    else
                    {
                        Console.WriteLine("The specified id is not valid");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Missing id");
                }
                break;
            default:
                Console.WriteLine("Unknown command " + args[0]);
                break;
        }
    }

    private static readonly string TaskDataBase = File.ReadAllText(TaskDataBasePath);
    private static readonly JsonSerializerOptions IndentedSerializerOptions = new(){ WriteIndented = true };
    private static List<TaskItem> GetAllTasks() => JsonSerializer.Deserialize<List<TaskItem>>(TaskDataBase)!;

    private static void UpdateTaskJson(List<TaskItem> taskItem)
    {
        string serializedTaskList = JsonSerializer.Serialize(taskItem, IndentedSerializerOptions);
        File.WriteAllText(TaskDataBasePath, serializedTaskList);
    }

    public static void AddTask(string description)
    {
        int id = GetAllTasks().Count;
        string status = StatusTypes[0];
        DateTime createdAt = DateTime.Now;
        DateTime updatedAt = createdAt;
        TaskItem newTask = new TaskItem
        {
            Id = id,
            Description = description,
            Status = status,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
        List<TaskItem> tasksToUpdate = GetAllTasks();
        tasksToUpdate.Add(newTask);
        UpdateTaskJson(tasksToUpdate);
    }

    public static void DeleteTask(int id)
    {
        List<TaskItem> taskList = GetAllTasks();
        TaskItem? itemToDelete = taskList.Find(x => x.Id == id);
        if (itemToDelete != null)
        {
            taskList.Remove(itemToDelete);
            UpdateTaskJson(taskList);
            Console.WriteLine("Task with the id " + id + " has been deleted successfully");
        }
        else
        {
            Console.WriteLine("Could not find the task with the id " + id + "to delete");
        }
    }

    public static void UpdateTask(int id, string description)
    {
        List<TaskItem> taskList = GetAllTasks();
        TaskItem? itemToUpdate = taskList.Find(x => x.Id == id);
        if (itemToUpdate != null)
        {
            itemToUpdate.Description = description;
            itemToUpdate.UpdatedAt = DateTime.Now;
            UpdateTaskJson(taskList);
            Console.WriteLine("The task with the id " + id + " has been updated successfully");
        }
        else
        {
            Console.WriteLine("Could not find the task with the id " + id + "to update");
        }
    }
    

    public static void ListTasks()
    {
        List<TaskItem> taskItems = GetAllTasks();
        Console.WriteLine(JsonSerializer.Serialize(taskItems, IndentedSerializerOptions));
    }
    
    public static void ListTasks(string status)
    {
        List<TaskItem> taskItems = GetAllTasks();
        List<TaskItem> taskItemsToShow = new();
        foreach (TaskItem taskItem in taskItems)
        {
            if (taskItem.Status == status)
            {
                taskItemsToShow.Add(taskItem);
            }
        }
        Console.WriteLine(JsonSerializer.Serialize(taskItemsToShow, IndentedSerializerOptions));
    }

    public static void ChangeStatus(int id, string status)
    {
        List<TaskItem> taskItems = GetAllTasks();
        TaskItem? itemToUpdate = taskItems.Find(x => x.Id == id);
        if (itemToUpdate == null) return;
        if (itemToUpdate.Status != status)
        {
            itemToUpdate.Status = status;
            itemToUpdate.UpdatedAt = DateTime.Now;
            UpdateTaskJson(taskItems);
            Console.WriteLine("The task with the id " + id + " has been marked as " + status + " successfully");
        }
        else
        {
            Console.WriteLine("The task with the id " + id + "is already marked as " + status);
        }
    }
}