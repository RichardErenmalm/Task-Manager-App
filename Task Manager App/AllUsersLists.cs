using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;


//Skapa en class och metod som gör att man kan se all historik på ett smidigt sätt. man ska sortera efter listans id och sedan visa efter tid, så man smidigt ser alla ändringar
//efter det, refaktorera
//sedan kolla varje punkt på github och fixa så att du gjort alla krav

namespace Task_Manager_App
{
    public class AllUsersLists
    {
        private Database database = new Database();

        public AllUsersLists() 
        {
            try
            {
                string json = File.ReadAllText("Databas.json");

                if (!string.IsNullOrWhiteSpace(json))
                {
                    database = JsonSerializer.Deserialize<Database>(json) ?? new Database(); ;
                }
                else
                {
                    database = new Database();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
           
        }
 
        public void SaveToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true 
            };
            var jsonData = JsonSerializer.Serialize(database, options);
            File.WriteAllText("Databas.json", jsonData);
        }

        public void SaveChanges(TaskList taskList, string changed)
        {
            
            History changedTaskList = new History(taskList.Id,taskList.Name, taskList.Tasks, taskList.Created, taskList.IsDeleted, changed);

            database.HistoryList.Add(changedTaskList);
        }
       

        public void AddList()
        {       
            ReusableText.Header("[bold blue]Create new list[/]");

            Console.WriteLine("Write list name: ");
            string name = Help.GetUserInfo<string>();

            
            database.IdCounter++;
            int id = database.IdCounter;


            TaskList taskList = new TaskList(id ,name, false);

            database.UserLists.Add(taskList);
            SaveChanges(taskList, "Created list");
            SaveToJson();

            Console.WriteLine();
            Console.WriteLine("list added");
            ReusableText.ReturnToMenu();      
        }

        public void Delete()
        {
            if (database.UserLists.Count != 0)
            {
                var nameOfListToDelete = Help.AnsiPrompt<string>("Choose list to delete", database.UserLists.Select(list => list.Name).ToList());

                TaskList listToDelete = database.UserLists.First(list => list.Name == nameOfListToDelete);
                listToDelete.IsDeleted = true;

                SaveChanges(listToDelete, "Deleted list");
                database.UserLists.Remove(listToDelete);
                SaveToJson();
            }
            else
            {
                Console.WriteLine("You have no lists to delete");
                ReusableText.ReturnToMenu();
            }
           
        }


        public void ListsMenu()
        {
            ReusableText.Header("[bold blue]Lists[/]");

            if (database.UserLists.Count == 0)     
            {
                Console.WriteLine("You have no lists as of yet");
                ReusableText.ReturnToMenu();
            }
            else
            {
                string userListChoiceName = Help.AnsiPrompt<string>("", database.UserLists.Select(list => list.Name).ToList());
                TaskList userListChoice = database.UserLists.First(list => list.Name == userListChoiceName);

                InTaskList(userListChoice);
            }
        }

        public void InTaskList(TaskList taskList)
        {

            bool keepGoing = true;
            while (keepGoing)
            {
                Console.Clear(); 
                string userChoice = Help.AnsiPrompt<string>($"[bold blue]{taskList.Name}[/]", new List<string> {"All tasks", "Add task", "Exit list"});


                switch (userChoice)
                {
                    case "All tasks":
                        ReusableText.Header($"[bold blue]{taskList.Name} : All tasks[/]");

                        if (taskList.Tasks.Count == 0)
                        {
                            Console.WriteLine("No tasks in list");
                            ReusableText.ReturnToMenu();
                        }
                        else
                        {
                           var deleteFromList = AnsiConsole.Prompt(
                           new MultiSelectionPrompt<string>()
                           .NotRequired() 
                           .PageSize(10)
                            .InstructionsText(
                                "[grey](Press [blue]<space>[/] to cross off tasks from list," +
                                "[green]and <enter>[/] to confirm and return to menu)[/]"
                            )
                            .AddChoices(taskList.Tasks
                            ));

                            if (deleteFromList.Count != 0)
                            {
                                string taskOrTasks;
                                if (deleteFromList.Count == 1)
                                {
                                     taskOrTasks = "finished 1 task";
                                }
                                else
                                {
                                    taskOrTasks = $"finished {deleteFromList.Count} tasks";
                                }
                            
                                foreach (var task in deleteFromList)
                                {
                                    taskList.Tasks.Remove(task);
                                }
                                SaveChanges(taskList, taskOrTasks);
                                SaveToJson();
                            }
                        }
                        break;

                    case "Add task":

                        ReusableText.Header($"[bold blue]{taskList.Name} : Add tasks[/]");

                        Console.WriteLine("Type in new task:");
                        string newTask = Help.GetUserInfo<string>();
                

                        taskList.Tasks.Add(newTask);
                        SaveChanges(taskList, ("Created new task"));                
                        SaveToJson();

                        Console.WriteLine($"new task added: {newTask}");
                        ReusableText.ReturnToMenu();         
                        break;

                    case "Exit list":
                        keepGoing = false;
                    break;
    

                }
            }
        }

        public void SeeHistoryOfLists()
        {
            ReusableText.Header("History");

            database.HistoryList = database.HistoryList.OrderBy(list => list.Id).ThenBy(list => list.TimeOfUpdate).ToList();

            List<Tree> TreeList = new List<Tree>();
            Tree tree = new Tree("");
            TreeNode node = new TreeNode(new Table());
            int id = -1;
            foreach (History history in database.HistoryList)
            {
                if (id == history.Id)
                {
                    tree = Help.AddToTree(tree, node, history);
                }
                else
                {    
                    tree = new Tree($"[yellow]{history.Name} - ID: {history.Id}[/]");

                    tree = Help.AddToTree(tree, node, history);
                    TreeList.Add(new Tree(tree));

                    id = history.Id;
                }
            }

            foreach (Tree trees in TreeList)
            {
                AnsiConsole.Write(trees);
            }
            ReusableText.ReturnToMenu();
        }
    }
}
