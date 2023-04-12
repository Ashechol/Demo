using UnityEngine;
using Utils.Log;
using Object = UnityEngine.Object;

namespace Framework
{
    [ExecuteAlways]
    public static class ResourceLoader
    {
        private static readonly ResourcesManagerSO Settings;

        private static readonly DebugLabel DebugLabel = new("ResourceManager", Color.magenta, Color.white);

        static ResourceLoader()
        {
            Settings = Resources.Load("Settings/Managers/ResourceManager Settings") as ResourcesManagerSO;
            DebugLog.LabelLog(DebugLabel, "Settings file not found!", Verbose.Assert, Settings);
        }

        public static GameObject LoadPrefab(string path, string newName = null)
        {
            if (!Settings) return null;
            
            var prefab = Resources.Load(Settings.prefabRoot + path);
            if (!prefab)
            {
                DebugLog.LabelLog(DebugLabel, $"Cannot find object {Settings.prefabRoot + path}", Verbose.Error);
                return null;
            }

            var go = Object.Instantiate(prefab);
            if (newName != null) go.name = newName;
            return go as GameObject;
        }

        /// <summary>
        /// Load and instantiate a prefab and return its component
        /// </summary>
        /// <param name="path">Prefab path in the Prefabs/</param>
        /// <param name="newName">rename instantiated prefab</param>
        /// <typeparam name="T">Component Type</typeparam>
        /// <returns></returns>
        public static T GetPrefabComponent<T>(string path, string newName = null) where T: Component
        {
            var go = LoadPrefab(path, newName);
            if (!go) return null;
            return go.GetComponent<T>();
        }
    }
}
