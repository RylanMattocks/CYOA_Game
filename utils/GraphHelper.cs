using System.Text.RegularExpressions;

public static class GraphHelper{
    public static void BuildGraph(Dictionary<string, DialogNode> dialogData, DialogGraph dialogGraph) {
        foreach (var dialogNode in dialogData) {
            dialogGraph.AddNode(dialogNode.Key, dialogNode.Value);
        }
    }
    
    public static void TraverseDialog(DialogGraph dialogGraph, string currentNodeId) {
        var dialogNode = dialogGraph.GetNode(currentNodeId);

        if (dialogNode is null) {
            Console.WriteLine($"Dialog ID '{currentNodeId}' not found, stopping.");
            return;
        }

        string formattedText = FormatText(dialogNode.Text);
        Console.WriteLine(formattedText);
        var nextDialogIds = dialogGraph.GetNextDialogIds(currentNodeId);

        if (nextDialogIds.Count == 0){
            Console.WriteLine("No more dialog options.");
            return;
        }

        if (dialogNode.Options[0].Next == "death") {
            Console.WriteLine("You have died.");
            return;
        }

        if (dialogNode.Options[0].Next == "live") {
            Console.WriteLine("Congratulations! You have survived!");
            return;
        }

        for (int i = 0; i < dialogNode.Options.Count; i++) {
            Console.WriteLine($"{i + 1}. {dialogNode.Options[i].Text}");
        }

        int choice = -1;

        while (choice < 1 || choice > dialogNode.Options.Count){
            Console.Write("Choose an option (1 to {0}): ", dialogNode.Options.Count);
            string input = Console.ReadLine();

            if(input.Trim().ToLower().Equals("save")) {
                Console.WriteLine("Please enter a save name");
                string saveName = Console.ReadLine();
                SaveHandler.SaveGame(saveName, currentNodeId);
                return;
            }
            else if (input.Trim().ToLower().Equals("exit")) {
                Environment.Exit(0);
            }
            else if (int.TryParse(input, out choice) && choice >= 1 && choice <= dialogNode.Options.Count)
            {
                Console.WriteLine($"You chose option {choice}: {dialogNode.Options[choice - 1].Text}");
                if (dialogNode.RollDice && dialogNode.Options[choice -1].Text == "Roll the dice") {
                    if(RNGHelper() == -1) {
                        TraverseDialog(dialogGraph, dialogNode.Options[choice - 1].Next + "_no");
                        break;
                    }
                    else {
                        TraverseDialog(dialogGraph, dialogNode.Options[choice - 1].Next + "_yes");
                        break;
                    }
                }
                TraverseDialog(dialogGraph, dialogNode.Options[choice - 1].Next);
            }
            else
            {
                Console.WriteLine("Invalid choice, please choose a valid option.");
            }
        }
    }

    private static int RNGHelper() {
        Random rng = new();
        int randomValue = rng.Next(1,20);
        User.DiceRoll = randomValue;

        Console.WriteLine($"You rolled a {randomValue}");

        if (randomValue < 13) {
            return -1;
        }
        else {
            return 1;
        }
    }

    private static string FormatText(string text) {
        Dictionary<string, string> variables = new () {
            {"user", User.Username},
            {"roll", User.DiceRoll.ToString()}
        };

        return Regex.Replace(text, @"\{(\w+)\}", match => {
            string key = match.Groups[1].Value;
            return variables.ContainsKey(key) ? variables[key] : match.Value;
        });
    }
}