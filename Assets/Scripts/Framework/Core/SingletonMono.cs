using UnityEngine;
using Utils.Log;

namespace Framework.Core
{
    public class SingletonMono<T> : MonoBehaviour where T: MonoBehaviour 
    {
        private static T _instance;
        protected SingletonMono() {}
        
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<T>();
                    if (!_instance)
                    {
                        DebugLog.CreateLog("SingletonMono", Color.magenta, 
                            $"Creating {typeof(T).Name}.", Color.white, Verbose.Log);
                        var go = new GameObject{name = typeof(T).Name};
                        _instance = go.AddComponent<T>();
                    }
                }
                
                return _instance;
            }
        }
        
        public static bool HasInstance => _instance != null;
    }
}