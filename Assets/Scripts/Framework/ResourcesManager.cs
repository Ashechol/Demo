using Framework.Core;
using UnityEngine;

namespace Framework
{
    public class ResourcesManager: SingletonMono<ResourcesManager>
    {
        public string prefabsRootPath;
        
        public GameObject LoadPrefab(string path)
        {
            var go = Resources.Load(prefabsRootPath + path);
            Instantiate(go);
            return go as GameObject;
        }
    }
}
