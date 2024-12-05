using System.Text.RegularExpressions;

public static class GraphHelper{
    public static void BuildGraph(Dictionary<string, DialogNode> dialogData, DialogGraph dialogGraph) {
        foreach (var dialogNode in dialogData) {
            dialogGraph.AddNode(dialogNode.Key, dialogNode.Value);
        }
    }
    
    public static void TraverseDialog(DialogGraph dialogGraph, string currentNodeId, User user) {
        var dialogNode = dialogGraph.GetNode(currentNodeId);

        if (dialogNode is null) {
            Console.WriteLine($"Dialog ID '{currentNodeId}' not found, stopping.");
            return;
        }

        if (user.Looping >= 3 && currentNodeId == "start") {
            if (user.Looping > 3) {
                Console.WriteLine("You are stuck in an infinite loop. You fear there may be no way out...");
                TraverseDialog(dialogGraph, "death", user);
                return;
            }
            else Console.WriteLine("The city seems like a dangerous place, maybe you can survive with the items you have found");
        }

        string formattedText = FormatText(dialogNode.Text, user);
        Console.WriteLine(formattedText);
        var nextDialogIds = dialogGraph.GetNextDialogIds(currentNodeId);

        if (nextDialogIds.Count == 0){
            return;
        }

        if (dialogNode.Options.Count == 1) {
            if(dialogNode.Options[0].Next == "start") user.Looping += 1;
            TraverseDialog(dialogGraph, dialogNode.Options[0].Next, user);
            return;
        }

        if (dialogNode.BagCheck) user.BagCheck = true;

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
                SaveHandler.SaveGame(saveName, currentNodeId, user);
                return;
            }
            else if (input.Trim().ToLower().Equals("exit")) {
                Environment.Exit(0);
            }
            else if (int.TryParse(input, out choice) && choice >= 1 && choice <= dialogNode.Options.Count)
            {
                Console.WriteLine($"You chose option {choice}: {dialogNode.Options[choice - 1].Text}");
                if (dialogNode.RollDice && dialogNode.Options[choice -1].Text == "Roll the dice") {
                    if(RNGHelper(user) == -1) {
                        TraverseDialog(dialogGraph, dialogNode.Options[choice - 1].Next + "_no", user);
                        break;
                    }
                    else {
                        TraverseDialog(dialogGraph, dialogNode.Options[choice - 1].Next + "_yes", user);
                        break;
                    }
                }
                else if (dialogNode.IsBagChecked && (dialogNode.Options[choice -1].Text == "Enter the door" || dialogNode.Options[choice -1].Text == "Leave the city")) {
                    string nextNode = user.BagCheck ? dialogNode.Options[choice - 1].Next + "_yes" : dialogNode.Options[choice - 1].Next + "_no";
                    TraverseDialog(dialogGraph, nextNode, user);
                    break;
                }
                TraverseDialog(dialogGraph, dialogNode.Options[choice - 1].Next, user);
            }
            else
            {
                Console.WriteLine("Invalid choice, please choose a valid option.");
            }
        }
    }

    private static int RNGHelper(User user) {
        Random rng = new();
        int randomValue = rng.Next(1,20);
        user.DiceRoll = randomValue;

        Console.WriteLine($"You rolled a {randomValue}");

        if (randomValue < 13) {
            return -1;
        }
        else {
            return 1;
        }
    }

    private static string FormatText(string text, User user) {
        Dictionary<string, string> variables = new () {
            {"user", user.Username},
            {"roll", user.DiceRoll.ToString()}
        };

        return Regex.Replace(text, @"\{(\w+)\}", match => {
            string key = match.Groups[1].Value;
            return variables.ContainsKey(key) ? variables[key] : match.Value;
        });
    }
}