using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Create Config")]
public class Config : ScriptableObject
{
    public string CatalogRoot = "catalog";
    
    public string CatalogPath
    {
        get { return Application.streamingAssetsPath + "/catalog.json"; }
    }

    public string UserRepositoryPath 
    {
        get { return Application.persistentDataPath + "/user_0.json";
    }
}

}
