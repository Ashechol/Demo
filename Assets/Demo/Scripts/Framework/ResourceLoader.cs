using Demo.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public static class ResourceLoader
    {
        private static readonly ResourcesLoaderSO Settings;

        private static readonly DebugLabel DebugLabel = new("ResourcesLoader", Color.magenta, Color.white);

        static ResourceLoader()
        {
            Settings = Resources.Load("Settings/ResourcesLoader Settings") as ResourcesLoaderSO;
            DebugLog.LabelLog(DebugLabel, "Settings file not found!", Verbose.Assert, Settings);
        }
        
        public static GameObject LoadPrefab(string path, Transform parent = null, string newName = null)
        {
            if (!Settings) return null;
            
            var prefab = Resources.Load(Settings.prefabRoot + path);
            if (!prefab)
            {
                DebugLog.LabelLog(DebugLabel, $"Cannot find object {Settings.prefabRoot + path}", Verbose.Error);
                return null;
            }
            
            var go = Object.Instantiate(prefab, parent);
            if (newName != null) go.name = newName;
            return go as GameObject;
        }

        /// <summary>
        /// Load and instantiate a prefab and return its component
        /// </summary>
        /// <param name="path">Prefab path in the Prefabs/</param>
        /// <param name="parent">Parent attach to</param>
        /// <param name="newName">Rename instantiated prefab</param>
        /// <typeparam name="T">Component Type</typeparam>
        /// <returns></returns>
        public static T GetPrefabComponent<T>(string path, Transform parent = null, string newName = null) where T: Component
        {
            var go = LoadPrefab(path, parent, newName);
            if (!go) return null;
            return go.GetComponent<T>();
        }
    }
}
