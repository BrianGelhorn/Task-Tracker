using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Task_Tracker;
using System.Text.Json;
class Program
{
    private const string TASKDATABASEDIR = "taskDatabase.json";
    private static readonly string[] STATUS = ["todo", "in-progress", "done"];

    public class TaskItem
    {
        public int id { get; set; }
        public string description { get; set; }
        public string status  { get; set; }
        public DateTime createdAt  { get; set; }
        public DateTime updatedAt { get; set; }
    }
    
    static void Main(string[] args)
    {
        if (!File.Exists(TASKDATABASEDIR))
        {
            File.WriteAllText(TASKDATABASEDIR, "[\n]");
        }
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: task-tracker <command> [arguments]");
            return;
        }

        switch (args[0])
        {
            case "help":
                //TODO: Implement help message
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
                //TODO: Implement update logic
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
                    ListTasks(args[1]);   
                }
                else
                {
                    ListTasks();
                }
                break;
            default:
                Console.WriteLine("Unknown command " + args[0]);
                break;
        }
    }

    private static string _taskDataBase = File.ReadAllText(TASKDATABASEDIR);
    private static JsonSerializerOptions indentedSerializerOptions = new(){ WriteIndented = true };
    private static List<TaskItem> GetAllTasks() => JsonSerializer.Deserialize<List<TaskItem>>(_taskDataBase)!;

    private static void UpdateTaskJson(List<TaskItem> taskItem)
    {
        string serializedTaskList = JsonSerializer.Serialize(taskItem, indentedSerializerOptions);
        File.WriteAllText(TASKDATABASEDIR, serializedTaskList);
    }

    public static void AddTask(string description)
    {
        int id = GetAllTasks().Count;
        string status = STATUS[0];
        DateTime createdAt = DateTime.Now;
        DateTime updatedAt = createdAt;
        TaskItem newTask = new TaskItem
        {
            id = id,
            description = description,
            status = status,
            createdAt = createdAt,
            updatedAt = updatedAt
        };
        List<TaskItem> tasksToUpdate = GetAllTasks();
        tasksToUpdate.Add(newTask);
        UpdateTaskJson(tasksToUpdate);
    }

    public static void DeleteTask(int id)
    {
        List<TaskItem> taskList = GetAllTasks();
        TaskItem? itemToDelete = taskList.Find(x => x.id == id);
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


    public static void ListTasks()
    {
        List<TaskItem> taskItems = GetAllTasks();
        Console.WriteLine(JsonSerializer.Serialize(taskItems, indentedSerializerOptions));
    }
    
    public static void ListTasks(string status)
    {
        List<TaskItem> taskItems = GetAllTasks();
        List<TaskItem> taskItemsToShow = new();
        foreach (TaskItem taskItem in taskItems)
        {
            if (taskItem.status == status)
            {
                taskItemsToShow.Add(taskItem);
            }
        }
        Console.WriteLine(JsonSerializer.Serialize(taskItemsToShow, indentedSerializerOptions));
    }
}