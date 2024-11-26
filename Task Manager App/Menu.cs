using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Spectre.Console;

namespace Task_Manager_App
{
    public static class Menu
    {
        public static void MainMenu()
        {

            AllUsersLists allLists = new AllUsersLists();

            bool keepGoing = true;
            while (keepGoing) 
            {
                Console.Clear();
                string userInput = Help.AnsiPrompt("[bold blue]Menu[/]", new List<string> {  
                        "My lists",
                        "Create new list",
                        "Delete list",
                        "See history",
                        "Quit application"});


                switch (userInput)
                {
                    case "My lists":
                        allLists.ListsMenu();
                        break;
                    case "Create new list" :
                        allLists.AddList();
                         break;
                    case "Delete list":
                        allLists.Delete();
                        break;
                    case "See history":
                        allLists.SeeHistoryOfLists();
                        break;
                    case "Quit application":
                        keepGoing = false;

                    break;
                } 
            }

        }

       
    }
}
