using BeyondTheWorlds.autoloads;
using Godot;
using System;

namespace BeyondTheWorlds.common.debug_console;

public partial class DebugConsole: Node
{
    // private string LogFilePath = $"user://logs/btw-logs-{System.DateTime.Now}.log";
    
    public static DebugConsole Instance { get; private set; }

    #region Nodes
    [ExportCategory("Debug Console")]
    [ExportGroup("Console Nodes")]
    [Export] private PackedScene ConsoleWindowScene { get; set; }
    #endregion
    
    private Window CurrentWindowInstance { get; set; }
    
    public override void _Ready()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;
    }

    public static void Log(string type, string massage)
    {
        Instance.ConsoleLog(type, massage);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("debug_console"))
        {
            ToggleConsole();
        }
    }
    
    private void OnCommandInput(string command)
    {
        switch (command)
        {
            case "TakeDamage":
                break;
        }
    }

    private void SuggestCommand(string[] input)
    {
        switch (input[0])
        {
            case "pause-game":
                break;
            
        }
    }

    private void ToggleConsole()
    {
        if (CurrentWindowInstance != null && GodotObject.IsInstanceValid(CurrentWindowInstance) && CurrentWindowInstance.Visible)
        {
            CurrentWindowInstance.QueueFree();
            CurrentWindowInstance = null;
            GD.Print("DebugConsole: Console Closed");
        }
        else
        {
            CurrentWindowInstance = ConsoleWindowScene.Instantiate<Window>();
            AddChild(CurrentWindowInstance);

            LineEdit ConsoleInput = CurrentWindowInstance.GetNode<LineEdit>("%ConsoleInput");
            ConsoleInput.TextSubmitted += OnCommandInput;
            
            CurrentWindowInstance.PopupCentered(new Vector2I(800, 600));
            ConsoleInput.GrabFocus();
        }
    }

    private void ConsoleLog(string type, string message)
    {
        RichTextLabel Console = CurrentWindowInstance.GetNode<RichTextLabel>("%ConsoleOutput");
        Console.AppendText($"\n> ({type}) {message}");
    }
}