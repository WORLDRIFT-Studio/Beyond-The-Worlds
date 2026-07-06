using BeyondTheWorlds.autoloads;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using FileAccess = Godot.FileAccess;

namespace BeyondTheWorlds.common.debug_console;

public partial class DebugConsole: Node
{
    
    private string LogFilePath { get; set; } = $"user://logs/btw-logs-{Time.GetDatetimeStringFromSystem().Replace(":", "-").Replace("T", "-")}.log";


    /// <summary>
    /// Hisotira logów
    /// </summary>
    private readonly List<String> _logHistory = new();
    
    /// <summary>
    /// Instancja okna, umożliwająca dostęp do niego
    /// </summary>
    private Window CurrentWindowInstance { get; set; }
    
    /// <summary>
    /// Instancja umożliwiająca dostęp do skryptu bez wyszkuwania go w drzeiw sceny
    /// </summary>
    public static DebugConsole Instance { get; private set; }

    #region Nodes
    /// <summary>
    /// Zawiera scenę okna konsoli
    /// </summary>
    [Export] private PackedScene ConsoleWindowScene { get; set; }
    
    #endregion

    #region Godot Functions

    public override void _Ready()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;
        
        using var dir = DirAccess.Open("user://");
        if (dir != null && !dir.DirExists("logs"))
        {
            dir.MakeDir("logs");
        }
    }


    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("debug_console"))
        {
            ToggleConsole();
        }
    }


    #endregion
    

    #region Comands

    /// <summary>
    /// Funkcja służąca do przyjmowania i wywoływania komend z konsoli debugowania
    /// </summary>
    /// <param name="command">Parametr przyjmuje komendę i na jej podsatwie wykonuje operacje</param>
    private void OnCommandInput(string command)
        {
            switch (command)
            {
                case "TakeDamage":
                    break;
            }
        }
    
    private void SuggestCommand(string input)
        {
            switch (input)
            {
                case "pause-game":
                    break;
                
            }
        }
    

    #endregion

    #region Logs
    
    /// <summary>
    /// Służy do logowania z zewnątrz
    /// </summary>
    /// <param name="level">Poziom logu - INFO, WARNING, ERROR, oraz pozostałe</param>
    /// <param name="type">Typ logu - skąd pochodzi</param>
    /// <param name="message">Wiadomość logu</param>
    public static void Log(string level, string type, string message)
    {
        Instance.ConsoleLog(level, type, message);
    }

    /// <summary>
    /// Funkcja wewnętrzna odpowiedzialna za logowanie zdarzeń
    /// </summary>
    /// <param name="level">Poziom logu - INFO, WARNING, ERROR, oraz pozostałe</param>
    /// <param name="type">Typ logu - skąd pochodzi</param>
    /// <param name="message">Wiadomość logu</param>
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

        string cleanMessage = 
            $"{Time.GetDatetimeStringFromSystem()} [ {level} ] ({type}) >>> {message}";
        string formatedMessage = 
            $"\n[color=#787878][i]{Time.GetDatetimeStringFromSystem().Split("T")[1]}[/i][/color] [color={color}][b][ {level} ][/b][/color] [i][color=#39cc9b]({type})[/color][/i] >>> {message}";
        _logHistory.Add(formatedMessage);

        SaveLogs(LogFilePath, cleanMessage);
        
        if (CurrentWindowInstance != null && GodotObject.IsInstanceValid(CurrentWindowInstance))
        {
            RichTextLabel Console = CurrentWindowInstance.GetNode<RichTextLabel>("%ConsoleOutput");
            Console.AppendText(formatedMessage);
        }
        
    }

    /// <summary>
    /// Zapisuje historie logów do pliku.
    /// </summary>
    /// <param name="pathLogsDir">Ścieżka do pliku logów</param>
    /// <param name="plainLog">Wejśćie dla logów</param>
    private void SaveLogs(string pathLogsDir, string plainLog)
    {
        using var logsFile = FileAccess.Open(pathLogsDir, FileAccess.ModeFlags.ReadWrite);
        if (logsFile != null)
        {
            logsFile.SeekEnd();
            logsFile.StoreLine(plainLog);
        }
        else
        {
            using var newFile = FileAccess.Open(pathLogsDir, FileAccess.ModeFlags.Write);
            newFile?.StoreLine(plainLog);
        }
    }
    #endregion

    #region Tools

    private void ToggleConsole()
        {
            // Sprawdza czy okno faktycznie nie istnieje i nie zalega w pamięci
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
                
                RichTextLabel ConsoleOutput = CurrentWindowInstance.GetNode<RichTextLabel>("%ConsoleOutput");
                foreach (String oldLog in _logHistory)
                {
                    ConsoleOutput.AppendText(oldLog);
                }
                
                Log("info", "Console", "Console succesfully opened!");
                
            }
        }
    

    #endregion
}
