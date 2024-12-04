public class DialogGraph {
    public Dictionary<string, DialogNode> Graph { get; set; }

    public DialogGraph() {
        Graph = [];
    }

    public void AddNode(string id, DialogNode dialogNode) {
        if (!Graph.ContainsKey(id)) {
            Graph.TryAdd(id, dialogNode);
        }
    }

    public DialogNode? GetNode(string id) {
        if (Graph.TryGetValue(id, out DialogNode? node)) {
            return node;
        }
        return null;
    }

    public List<string> GetNextDialogIds(string id) {
        DialogNode dialogNode = GetNode(id);

        if (dialogNode is null) {
            Console.WriteLine($"Dialog ID '{id}' not found.");
            return [];
        }

        List<string> nextDialogs = [];
        foreach (var option in dialogNode.Options) {
            nextDialogs.Add(option.Next);
        }
        return nextDialogs;
    }

    public void DisplayDialogText(string id) {
        DialogNode dialogNode = GetNode(id);
        if (dialogNode is not null) {
            Console.WriteLine(dialogNode.Text);
        }
        else Console.WriteLine($"Dialog ID '{id}' not found.");
    }
}