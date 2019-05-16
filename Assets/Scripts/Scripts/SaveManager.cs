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
        PlayerPrefs.Save();
    }

    public void Load()
    {
        GameManager.totalCoins = PlayerPrefs.GetInt("TotalCoins");
        GameManager.totalWood = PlayerPrefs.GetInt("TotalWood");
        GameManager.totalRock = PlayerPrefs.GetInt("TotalRock");
        GameManager.totalFabrics = PlayerPrefs.GetInt("TotalFabric");
    }

    public void ResetSaving()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}