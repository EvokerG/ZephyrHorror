using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public void ToggleConsole()
    {
        ConsoleVisibility = !ConsoleVisibility;
        if (ConsoleVisibility)
        {
            ConsoleInput.Select();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void IgnoreBackQuote(string text)
    {
        if (ConsoleInput.text.Length > 0 && ConsoleInput.text[ConsoleInput.text.Length - 1] == '`') 
        {
            ConsoleInput.text = ConsoleInput.text.Substring(0, ConsoleInput.text.Length - 1);
        }
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
        gameObject.transform.GetChild(0).position = new Vector3(gameObject.transform.GetChild(0).position.x,Mathf.Clamp(gameObject.transform.GetChild(0).position.y,0,Screen.height * 1.45f), gameObject.transform.GetChild(0).position.z);
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
            Resize(true);            
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
        ConsoleInput.onValueChanged.AddListener(IgnoreBackQuote);
        ConsoleInput.onFocusSelectAll = false;
        ConsoleInputField = gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>();
        ConsoleOutput = gameObject.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TMP_Text>();
        Print(gameObject.transform.GetChild(0).position.ToString() + " " + Screen.height);
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
        gameObject.transform.GetChild(0).position = new Vector3(gameObject.transform.GetChild(0).position.x, (((Convert.ToInt32(!ConsoleVisibility) * 2) - 1) * Mathf.Min((Mathf.Abs((Convert.ToInt32(!ConsoleVisibility) * Screen.height * 1.5f) - gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin.y) * 6 * Time.deltaTime),Mathf.Abs((Convert.ToInt32(!ConsoleVisibility) * Screen.height * 1.5f) - gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin.y))) + gameObject.transform.GetChild(0).position.y, gameObject.transform.GetChild(0).position.z);
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleConsole();
        }
    }
}
