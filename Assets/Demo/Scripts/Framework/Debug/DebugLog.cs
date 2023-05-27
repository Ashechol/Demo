using Unity.VisualScripting;
using UnityEngine;

namespace Demo.Framework.Debug
{ 
    public enum Verbose
    {
        Log,
        Warning,
        Error,
        Assert
    }

    public struct DebugLabel
    {
        public readonly string name;
        public Color labelColor;
        public Color messageColor;
        public DebugLabel(string name, Color? labelColor = null, Color? messageColor = null)
        {
            this.name = name;
            this.labelColor = labelColor ?? Color.yellow;
            this.messageColor = messageColor?? Color.white;
        }
    }

    public class DebugLog : MonoBehaviour
    {
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

        public static void CreateLog(string label, Color labelColor, string message, Color color , Verbose verb, bool condition = false)
        {
            CreateLog(label, labelColor.ToHexString(), message, color.ToHexString(), verb, condition);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void CreateLog(string label, string labelColor, string message, string color, Verbose verb, bool condition = false)
        {
            switch (verb)
            {
                case Verbose.Log:
                    UnityEngine.Debug.Log($"<color=#{labelColor}>#{label}: </color>" + 
                                          $"<color={color}>{message}</color>");
                    break;
                case Verbose.Warning:
                    UnityEngine.Debug.LogWarning($"<color=#{labelColor}>#{label}: </color>" + 
                                                 $"<color={color}>{message}</color>");
                    break;
                case Verbose.Error:
                    UnityEngine.Debug.LogError($"<color=#{labelColor}>#{label}: </color>" + 
                                               $"<color={color}>{message}</color>");
                    break;
                case Verbose.Assert:
                    UnityEngine.Debug.Assert(condition, $"<color=#{labelColor}>#{label}: </color>" + 
                                                        $"<color=#{color}>{message}</color>");
                    break;
            }
        }

        public static void LabelLog(DebugLabel label, string message, Verbose verbose, bool condition = false)
        {
            CreateLog(label.name, label.labelColor, message, label.messageColor, verbose, condition);
        }

        public static void VariableLog()
        {
            
        }
    }
}
