using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager Settings/ResourcesManager", fileName = "ResourcesManager Settings")]
public class ResourcesLoaderSO : ScriptableObject
{
    public bool dontDestoryOnLoad = false;

    #region Path

    public string prefabRoot;

    #endregion
}
