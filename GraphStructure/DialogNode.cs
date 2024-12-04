using System.Runtime.InteropServices;

public class DialogNode{
    public string Text { get; set; }
    public List<DialogData> Options { get; set; }
    public bool RollDice { get; set; } = false;
    public DialogNode() {
        Options = new List<DialogData>();
    }
}