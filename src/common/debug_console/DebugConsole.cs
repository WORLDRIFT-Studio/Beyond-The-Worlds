using BeyondTheWorlds.autoloads;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeyondTheWorlds.common.debug_console;

public partial class DebugConsole: Node
{
    // private string LogFilePath = $"user://logs/btw-logs-{System.DateTime.Now}.log";
    // TODO: Zrobic zapis logow do pliku

    private readonly List<String> _logHistory = new();
    private int _logLine = 1;
    
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

    public static void Log(string level, string type, string message)
    {
        Instance.ConsoleLog(level, type, message);
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
            
            Log("info", "Console", "Console succesfully opened!");
            
            CurrentWindowInstance.PopupCentered(new Vector2I(800, 600));
            ConsoleInput.GrabFocus();
            
            RichTextLabel ConsoleOutput = CurrentWindowInstance.GetNode<RichTextLabel>("%ConsoleOutput");
            foreach (String oldLog in _logHistory)
            {
                ConsoleOutput.AppendText(oldLog);
            }
        }
    }

    private void ConsoleLog(string level, string type, string message)
    {
        level = level.ToUpper();
        string color = level switch
        {
            "INFO" => "#437ee3", //blue
            "WARNING" => "#c18d48", //orange
            "ERROR" => "#c4473c", //red
            _ => "#b8b3ab" //gray
        };

        string time = $"{DateTime.Now.TimeOfDay.Hours}.{DateTime.Now.TimeOfDay.Minutes}.{DateTime.Now.TimeOfDay.Seconds}";
        string formatedMessage = 
            $"\n[color=#787878][i]{time}[/i][/color] [color={color}][b][ {level} ][/b][/color] [i][color=#39cc9b]({type})[/color][/i] >>> {message}";
        _logHistory.Add(formatedMessage);


        if (CurrentWindowInstance != null && GodotObject.IsInstanceValid(CurrentWindowInstance))
        {
            RichTextLabel Console = CurrentWindowInstance.GetNode<RichTextLabel>("%ConsoleOutput");
            Console.AppendText(formatedMessage);
        }
        
    }
}