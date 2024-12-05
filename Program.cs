using System.Dynamic;
using System.Reflection;

namespace CYOA;

// Choose your own Adventure Game
class Program
{
    static void Main(string[] args)
    {
        // Introducing Player to the game
        Console.WriteLine("Welcome! This is a choose your own adventure game. Your choices will decide your fate so think carefully!");
        Console.WriteLine("To answer each question type the corresponding number. You will be given up to three choices: 1, 2 or 3.");
        Console.WriteLine("At any time you can save by typing \'save\'. To exit without saving type \'exit\'");
        Console.WriteLine("Please enter your username to begin: ");
        string username = Console.ReadLine();
        SaveState savedState = SaveHelper.DisplayUserSaves(username);

        var path = Directory.GetCurrentDirectory();
        path += "\\textfiles\\dialog.json";

        var dialogData = DialogLoader.LoadDialogData(path);

        var dialogGraph = new DialogGraph();

        GraphHelper.BuildGraph(dialogData, dialogGraph);

        if (savedState is null) {
            User newUser = new() {
                Username = username
            };
            GraphHelper.TraverseDialog(dialogGraph, "start", newUser);
        }
        else {
            GraphHelper.TraverseDialog(dialogGraph, savedState.SaveLocation, savedState.User);
        }
    }
}
