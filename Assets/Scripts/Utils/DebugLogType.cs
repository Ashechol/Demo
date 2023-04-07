using Unity.VisualScripting;
using UnityEngine;

public class DebugLogType : MonoBehaviour
{
    public enum Verbose
    {
        Log,
        Warning,
        Error
    }

    public static void Tips(string message, Color color, Verbose verb=Verbose.Log)
    {
        CreateLog("Tip", Color.yellow, message, color, verb);
    }

    public static void Editor(string message, Color color, Verbose verb=Verbose.Log)
    {
        CreateLog("Editor", Color.cyan, message, color, verb);
    }

    public static void GamePlay(string message, Color color, Verbose verb=Verbose.Log)
    {
        CreateLog("GamePlay", Color.green, message, color, verb);
    }

    public static void CreateLog(string label, Color labelColor, string message, Color color, Verbose verb)
    {
        switch (verb)
        {
            case Verbose.Log:
                Debug.Log($"<color=#{labelColor.ToHexString()}>#{label}: </color>" + 
                          $"<color=#{color.ToHexString()}>{message}</color>");
                break;
            case Verbose.Warning:
                Debug.LogWarning($"<color=#{labelColor.ToHexString()}>#{label}: </color>" + 
                                 $"<color=#{color.ToHexString()}>{message}</color>");
                break;
            case Verbose.Error:
                Debug.Log($"<color=#{labelColor.ToHexString()}>#{label}: </color>" + 
                          $"<color=#{color.ToHexString()}>{message}</color>");
                break;
        }
    }
}
