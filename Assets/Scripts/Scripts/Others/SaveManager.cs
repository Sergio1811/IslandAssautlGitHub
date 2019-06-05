using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
    static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }

        Instance = this;
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

        print(ButtonManager.boughtString);
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "mastil") == 1)
            GameManager.mastil = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "casco") == 1)
            GameManager.casco = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "remos") == 1)
            GameManager.remos = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "timon") == 1)
            GameManager.timon = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "cañon") == 1)
            GameManager.cañon = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "vela") == 1)
            GameManager.velas = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "mapa") == 1)
            GameManager.mapa = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "brujula") == 1)
            GameManager.brujula = true;
    }

    public void ResetSaving()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}