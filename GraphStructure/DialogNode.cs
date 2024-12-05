using System.Runtime.InteropServices;

public class DialogNode{
    public string Text { get; set; }
    public List<DialogData> Options { get; set; }
    public bool RollDice { get; set; } = false;
    public bool BagCheck { get; set; } = false;
    public bool IsBagChecked { get; set; }
    public DialogNode() {
        Options = new List<DialogData>();
    }
}