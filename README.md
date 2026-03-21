# Task Tracker

A simple tool written in C# CLI made to track ur pendent tasks

# Features

- Add tasks to the list
- Update the tasks when they are already in the list
- Update the status of the tasks
- List the tasks in a specific status
- Delete the tasks once we don´t want them anymore

# Usage

` ./task-tracker <command> [ARGUMENT] `

# Compilation

* Install SDK of .NET

``` 
git clone https://github.com/BrianGelhorn/Task-Tracker 
cd Task-Tracker
dotnet build
dotnet publish -c Release -o ./Publish
cd Publish
```
Now you can execute the Task Tracker using the command

` ./task-tracker <command> [ARGUMENT] `

# Examples

Add a task to the list with the description "Wash the dishes"
` ./task-tracker add "Wash the dishes" `

We can see the item on the list using:

` ./task-tracker list todo `

Showing on the console:
```
[
  {
    "Id": 0,
    "Description": "Wash the dishes",
    "Status": "todo",
    "CreatedAt": "2026-03-21T01:58:05.5647625-03:00",
    "UpdatedAt": "2026-03-21T01:58:05.5647625-03:00"
  }
]
```

We can change the description with the following:

` ./task-tracker update 0 "Wash the car" `

If we show in the console it may show:

```
[
  {
    "Id": 0,
    "Description": "Wash the car",
    "Status": "todo",
    "CreatedAt": "2026-03-21T01:58:05.5647625-03:00",
    "UpdatedAt": "2026-03-21T02:01:03.4536321-03:00"
  }
]
```

We can now mark the task as "in-progress" or as "done":

` ./task-tracker mark-in-progress 0 `

returning in:

```
[
  {
    "Id": 0,
    "Description": "Wash the car",
    "Status": "in-progress",
    "CreatedAt": "2026-03-21T01:58:05.5647625-03:00",
    "UpdatedAt": "2026-03-21T02:04:02.5396038-03:00"
  }
]
```

The same way can be done with
` ./task-tracker mark-done 0 `

returning

```
[
  {
    "Id": 0,
    "Description": "Wash the car",
    "Status": "done",
    "CreatedAt": "2026-03-21T01:58:05.5647625-03:00",
    "UpdatedAt": "2026-03-21T02:05:20.6320648-03:00"
  }
]
```

## Future Improvements

- Add unit tests
- Improve task list display
- Add more flexible status updates
- Improve input validation

## Autor

Developed by Brian Gelhorn.
Project made for Roadmap.sh
https://roadmap.sh/projects/task-tracker

## License

This project is intended for educational and practice purposes.

