using Godot;

namespace BeyondTheWorlds.common.debug_console;

public partial class DebugConsole: Node
{
    [Export]
    private RichTextLabel ConsoleOutput { get; set; }
    
    [Export]
    private LineEdit ConsoleInput { get; set; }

    public override void _Ready()
    {
        ConsoleInput.TextSubmitted += OnCommandInput;
    }

    private void OnCommandInput(string command)
    {
        switch (command)
        {
            case "TakeDamage":
                break;
        }
    }
    
}