using Demo.Utils.Debug;
using UnityEngine;

namespace Demo.Framework.Core
{
    public class SingletonMono<T> : MonoBehaviour where T: MonoBehaviour 
    {
        private static T _instance;
        private static bool _isApplicationQuit;
        protected SingletonMono() {}
        
        public static T Instance
        {
            get
            {
                if (_isApplicationQuit)
                    return _instance;
                
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

        private void OnApplicationQuit()
        {
            _isApplicationQuit = true;
        }

        public static bool HasInstance => _instance != null;
    }
}
