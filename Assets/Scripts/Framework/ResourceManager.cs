using System;
using Framework.Core;
using UnityEngine;
using Utils.Log;

namespace Framework
{
    [ExecuteAlways]
    public class ResourceManager: SingletonMono<ResourceManager>
    {
        public ResourcesManagerSO settings;

        private readonly DebugLabel _debugLabel = new("ResourceManager", Color.magenta, Color.white);

        private void Awake()
        {
            settings = Resources.Load("Settings/Managers/ResourceManager Settings") as ResourcesManagerSO;
            DebugLog.LabelLog(_debugLabel, "Settings file not found!", Verbose.Assert, settings);
            
        }

        private void Start()
        {
            if (settings && settings.dontDestoryOnLoad && Application.isPlaying) 
                DontDestroyOnLoad(gameObject);
        }

        public GameObject LoadPrefab(string path, string newName = null)
        {
            if (!settings) return null;
            
            var prefab = Resources.Load(settings.prefabRoot + path);
            if (!prefab)
            {
                DebugLog.LabelLog(_debugLabel, $"Cannot find object {settings.prefabRoot + path}", Verbose.Error);
                return null;
            }

            var go = Instantiate(prefab);
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
        public T GetPrefabComponent<T>(string path, string newName = null) where T: Component
        {
            var go = LoadPrefab(path, newName);
            if (!go) return null;
            return go.GetComponent<T>();
        }
    }
}
