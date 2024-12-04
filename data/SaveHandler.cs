using System.Text.Json;

public static class SaveHandler{
    public static string LoadSave(string saveName) {
        string username = User.Username;
        string saveFilePath = Path.Combine(Environment.CurrentDirectory, "Saves", username, saveName);
        
        if (File.Exists(saveFilePath)) {
            string json = File.ReadAllText(saveFilePath);

            string loadedSave = JsonSerializer.Deserialize<string>(json);

            Console.WriteLine("Game loaded successfully!");
            return loadedSave;
        }
        else {
            Console.WriteLine("Save file not found.");
            return null;
        }
    }

    public static void SaveGame(string saveName, string saveLocation) {
        string username = User.Username;
        saveName += ".save";
        string saveDirectory = Path.Combine(Environment.CurrentDirectory, "Saves", username);
        Directory.CreateDirectory(saveDirectory);

        string json = JsonSerializer.Serialize(saveLocation);

        string saveFilePath = Path.Combine(saveDirectory, saveName);

        File.WriteAllText(saveFilePath, json);

        Console.WriteLine("Game saved successfully!");
    }
}