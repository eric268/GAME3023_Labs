using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerAttributes : MonoBehaviour
{
    public string m_playerName;
    public static List<int> playerAbilityIDs;
    public float accuraccyAfflication = 0.0f;
    public int playerStartingHealth = 30;
    

    public int currentXP;
    public int playerCurrentHealth;
    public static int maxLevel = 10;
    public static int currentLevel;
    public static LevelUp[] levelUpInfo;

    // Start is called before the first frame update
    void Start()
    {
        //This is temporary and will be loaded either with a player pref or txt document
        LoadPlayerStats();
        LoadPlayerAbilityIDs();
        m_playerName = "Mike";
        levelUpInfo = LevelUp.PopulateLevelUpArray();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void LoadPlayerAbilityIDs()
    {
        playerAbilityIDs = new List<int>() {0,0,0,0};
        if (PlayerPrefs.GetInt("Player Abilities Saved") != 1)
        {
            playerAbilityIDs[0] = 0;
            playerAbilityIDs[1] = 4;
            playerAbilityIDs[2] = 5;
            playerAbilityIDs[3] = 7;
            SavePlayerAbilities();
        }
        else
        {
            LoadPlayerAbilities();
        } 
    }
    public static void LoadPlayerAbilities()
    {
        playerAbilityIDs[0] = PlayerPrefs.GetInt("Player Ability 1");
        playerAbilityIDs[1] = PlayerPrefs.GetInt("Player Ability 2");
        playerAbilityIDs[2] = PlayerPrefs.GetInt("Player Ability 3");
        playerAbilityIDs[3] = PlayerPrefs.GetInt("Player Ability 4");
        PlayerPrefs.Save();
    }
    public static void SavePlayerAbilities()
    {
        PlayerPrefs.SetInt("Player Ability 1", playerAbilityIDs[0]);
        PlayerPrefs.SetInt("Player Ability 2", playerAbilityIDs[1]);
        PlayerPrefs.SetInt("Player Ability 3", playerAbilityIDs[2]);
        PlayerPrefs.SetInt("Player Ability 4", playerAbilityIDs[3]);
        PlayerPrefs.SetInt("Player Abilities Saved", 1);
        PlayerPrefs.Save();
    }  
    public void LoadPlayerStats()
    {
        if (PlayerPrefs.GetInt("PlayerSavedStats") != 1)
        {
            currentLevel = 1;
            currentXP = 0;
            playerCurrentHealth = playerStartingHealth;
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("PlayerLevel");
            currentXP = PlayerPrefs.GetInt("PlayerXP");
            playerCurrentHealth = PlayerPrefs.GetInt("PlayerHealth");
        }
    }
    public void SavePlayerStats()
    {
        PlayerPrefs.SetInt("PlayerSavedStats", 1);
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerXP", currentXP);
        PlayerPrefs.SetInt("PlayerHealth", playerCurrentHealth);
        PlayerPrefs.Save();
    }
}

public class LevelUp
{
    public int level;
    public int xpTillNextLevel;
    public int healthIncrease;

    public LevelUp(int l, int xp, int h)
    {
        level = l;
        xpTillNextLevel = xp;
        healthIncrease = h;
    }
    public static LevelUp[] PopulateLevelUpArray()
    {
        LevelUp[] array = new LevelUp[PlayerAttributes.maxLevel];

        for (int i = 1; i < array.Length + 1; i++)
        {
            LevelUp levelInfo = new LevelUp(i + 1, i * 5, i * 4);
            array[i-1] = levelInfo;
        }

        return array;
    }
}
