using System.Text.Json;

public static class DialogLoader{
    public static Dictionary<string, DialogNode> LoadDialogData(string filePath) {
        try {
            var json = File.ReadAllText(filePath);
            var dialogData = JsonSerializer.Deserialize<Dictionary<string, DialogNode>>(json);
            return dialogData;
        }
        catch (Exception e) {
            Console.WriteLine($"Error loading dialog data: {e.Message}");
            return new Dictionary<string, DialogNode>();
        }
    }
}