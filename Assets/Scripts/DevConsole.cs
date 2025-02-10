using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevConsole : MonoBehaviour
{
    private Scrollbar Scroll;
    private InputField ConsoleInput;
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
        ConsoleOutput.fontSize = ConsoleInput.GetComponentInChildren<TMP_Text>().fontSize;
        ConsoleOutputLength = ConsoleOutput.preferredHeight;
        if (ScrollDown)
        {
            Scroll.value = 0;
        }
        else
        {
            Scroll.value = Scroll.value * ConsoleOutputLength / PrevOutputLength;
        }
        Debug.Log("ggg");
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

    private void Awake()
    {
        WindowWidth = Screen.width;
        WindowHeight = Screen.height;
    }

    private void Update()
    {
        if (WindowWidth != Screen.width || WindowHeight != Screen.height)
        {
            Resize(false);
            WindowWidth = Screen.width;
            WindowHeight = Screen.height;
        }
    }
}
