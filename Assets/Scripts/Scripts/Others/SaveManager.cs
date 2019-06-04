using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();
    }

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("TotalCoins", GameManager.totalCoins);
        PlayerPrefs.SetInt("TotalWood", GameManager.totalWood);
        PlayerPrefs.SetInt("TotalRock", GameManager.totalRock);
        PlayerPrefs.SetInt("TotalFabric", GameManager.totalFabrics);
        PlayerPrefs.SetInt("TotalWood2", GameManager.totalWood2);
        PlayerPrefs.SetInt("TotalRock2", GameManager.totalRock2);
        PlayerPrefs.SetInt("TotalFabric2", GameManager.totalFabrics2);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        GameManager.totalCoins = PlayerPrefs.GetInt("TotalCoins");
        GameManager.totalWood = PlayerPrefs.GetInt("TotalWood");
        GameManager.totalRock = PlayerPrefs.GetInt("TotalRock");
        GameManager.totalFabrics = PlayerPrefs.GetInt("TotalFabric");
        GameManager.totalWood2 = PlayerPrefs.GetInt("TotalWood2");
        GameManager.totalRock2 = PlayerPrefs.GetInt("TotalRock2");
        GameManager.totalFabrics2 = PlayerPrefs.GetInt("TotalFabric2");
    }

    public void ResetSaving()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}