using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Task_Tracker;
using System.Text.Json;
using System.Text;
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
                //TODO: Implement delete logic
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