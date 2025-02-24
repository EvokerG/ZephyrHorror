using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class DevConsole : MonoBehaviour
{
    private Scrollbar Scroll;
    private TMP_InputField ConsoleInput;
    private TMP_Text ConsoleInputField;
    private TMP_Text ConsoleOutput;

    private float ConsoleOutputLength;
    private static float WindowWidth;
    private static float WindowHeight;

    public static bool ConsoleVisibility = false;

    public static void ToggleConsole()
    {
        ConsoleVisibility = !ConsoleVisibility;
    }

    private void Resize(bool ScrollDown)
    {
        float PrevOutputLength = ConsoleOutputLength;        
        ConsoleOutputLength = ConsoleOutput.GetComponent<TMP_Text>().preferredHeight;
        if (ScrollDown)
        {
            Scroll.value = 0;
        }
        else
        {
            Scroll.value = Scroll.value * ConsoleOutputLength / PrevOutputLength;
        }
    }

    private void Print(string text)
    {
        ConsoleOutput.text += text + "\n";
    }

    public void Log(string text)
    {
        Debug.Log(text);
        Print("[LOG] " + text);
    }

    public void Warn(string text)
    {
        Debug.LogWarning(text);
        Print("[WARNING] " + text);
    }

    public void Error(string text)
    {
        Debug.LogError(text);
        Print("[Error] " + text);
    }

    public void LogCommand(string text)
    {
        Debug.Log("User comand -> " + text);
        Print("[COMMAND] " + text);
    }

    private void Process(string command)
    {
        if (ConsoleInput.text.Length > 0)
        {
            LogCommand(ConsoleInput.text);
            ExecuteCommand(ConsoleInput.text);
            ConsoleInput.text = "";
        }
    }

    private void ExecuteCommand(string command)
    {
        string[] breakdown = command.Split(" ");
        string Output = "";
        switch (breakdown[0])
        {
            case "shutdown":
                Output = "Closing game";
                #if UNITY_EDITOR                
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                break;
            default:
                Output = "Failed to execute command: " + command;
                break;
        }
        Log(Output);
    }

    private void Awake()
    {
        WindowWidth = Screen.width;
        WindowHeight = Screen.height;
        DontDestroyOnLoad(gameObject);
        Scroll = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Scrollbar>();
        ConsoleInput = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_InputField>();
        ConsoleInput.onSubmit.AddListener(Process);
        ConsoleInputField = gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>();
        ConsoleOutput = gameObject.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMP_Text>();        
    }

    private void Update()
    {
        ConsoleOutput.fontSize = ConsoleInputField.fontSize;
        if (WindowWidth != Screen.width || WindowHeight != Screen.height)
        {            
            WindowWidth = Screen.width;
            WindowHeight = Screen.height;
            Resize(false);
        }
    }
}
