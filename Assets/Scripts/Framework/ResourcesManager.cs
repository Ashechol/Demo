using Framework.Core;
using UnityEngine;

namespace Framework
{
    public class ResourcesManager: SingletonMono<ResourcesManager>
    {
        #region Path
        
        private const string PrefabsRootPath = "Prefabs/";
        
        #endregion

        public GameObject LoadPrefab(string path)
        {
            var go = Resources.Load(PrefabsRootPath + path);
            Instantiate(go);
            return go as GameObject;
        }
    }
}
