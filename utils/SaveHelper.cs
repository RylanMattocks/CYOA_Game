public static class SaveHelper{
    public static string DisplayUserSaves() {
        string username = User.Username;
        string savesDirectory = Path.Combine(Environment.CurrentDirectory, "Saves", username);
        if (Directory.Exists(savesDirectory)) {
            Console.WriteLine($"\nSaves for {username}:");
            var savedFiles = Directory.GetFiles(savesDirectory).Where(file => file.EndsWith(".save")).ToList();

            if (savedFiles.Count > 0) {
                for (int i = 0; i < savedFiles.Count; i++) {
                    Console.WriteLine($"{i + 1}. {Path.GetFileName(savedFiles[i])}");
                }

                Console.WriteLine("\nChoose a save to load (enter number): ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= savedFiles.Count){
                    return SaveHandler.LoadSave(Path.GetFileName(savedFiles[choice - 1]));
                }
                else{
                    Console.WriteLine("Invalid choice.");
                }
            }
            else {
                Console.WriteLine($"No saves found for {username}. Starting a new game.");
            }
        }
        return "start";
    }
}