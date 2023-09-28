using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Create Config")]
public class Config : ScriptableObject
{
    public string CatalogRoot = "catalog";
    
    public string CatalogPath => Application.streamingAssetsPath + "/catalog.json";

    public string UserRepositoryPath => Application.persistentDataPath + "/user_0.json";
}
