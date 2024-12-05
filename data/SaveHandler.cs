using System.Text.Json;

public static class SaveHandler{
    public static SaveState LoadSave(string saveName, string username) {
        string saveFilePath = Path.Combine(Environment.CurrentDirectory, "Saves", username, saveName);
        
        if (File.Exists(saveFilePath)) {
            string json = File.ReadAllText(saveFilePath);

            var loadedSave = JsonSerializer.Deserialize<SaveState>(json);

            Console.WriteLine("Game loaded successfully!");
            return loadedSave;
        }
        else {
            Console.WriteLine("Save file not found.");
            return null;
        }
    }

    public static void SaveGame(string saveName, string saveLocation, User user) {
        string username = user.Username;
        saveName += ".save";
        SaveState saveState = new() {
            SaveLocation = saveLocation,
            User = user
        };
        string saveDirectory = Path.Combine(Environment.CurrentDirectory, "Saves", username);
        Directory.CreateDirectory(saveDirectory);

        string json = JsonSerializer.Serialize(saveState);

        string saveFilePath = Path.Combine(saveDirectory, saveName);

        File.WriteAllText(saveFilePath, json);

        Console.WriteLine("Game saved successfully!");
    }
}