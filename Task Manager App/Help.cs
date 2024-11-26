using FluentValidation;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Task_Manager_App
{
    public class Help
    {

        public static T GetUserInfo<T>() 
        {
            while (true)
            {
                T userInput;
                try
                {
                    string input = Console.ReadLine() ?? string.Empty;

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("du angav ingen input... försök igen");
                        continue;
                    }

                    userInput = (T)Convert.ChangeType(input, typeof(T));

                    return userInput;
                }
                catch
                {
                   ReusableText.TryAgain();
                }
            }
        }


        public static T AnsiPrompt<T>(string title, List<T> addChoice) where T : notnull
        {
            var choice = AnsiConsole.Prompt(
                      new SelectionPrompt<T>()
                      .Title(title)
                      .PageSize(20) 
                      .AddChoices(addChoice)
                  );


            return choice;
        }

        public static Tree AddToTree(Tree tree, TreeNode? treeNode, History history)
        {
            treeNode = tree.AddNode(new Table()
                    .RoundedBorder()
                    .AddColumn($"Change: {history.Changed}    time of update: {history.TimeOfUpdate}")
                    .AddRow($"Created: {history.Created}")
                    .AddRow($"Is deleted: {history.IsDeleted}")
                    .AddRow($"Current tasks: {(history.Tasks.Any() ? string.Join(", ", history.Tasks) : "No current tasks")}")
                   );

            return tree;
        }
    } 
}
