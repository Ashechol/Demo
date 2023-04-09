using System;
using Framework.Core;
using UnityEngine;
using Utils.Log;

namespace Framework
{
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
            if (settings && settings.dontDestoryOnLoad) 
                DontDestroyOnLoad(gameObject);
        }

        public GameObject LoadPrefab(string path)
        {
            if (!settings) return null;
            
            var go = Resources.Load(settings.prefabRoot + path);
            if (!go)
            {
                DebugLog.LabelLog(_debugLabel, $"Cannot find object {settings.prefabRoot + path}", Verbose.Error);
                return null;
            }
            Instantiate(go);
            return go as GameObject;
        }
        
        /// <summary>
        /// Load and instantiate a prefab and return its component
        /// </summary>
        /// <param name="path">Prefab path in the Prefabs/</param>
        /// <typeparam name="T">Component Type</typeparam>
        /// <returns></returns>
        public T GetPrefabComponent<T>(string path) where T: Component
        {
            var go = LoadPrefab(path);
            if (!go) return null;
            return go.GetComponent<T>();
        }
    }
}
