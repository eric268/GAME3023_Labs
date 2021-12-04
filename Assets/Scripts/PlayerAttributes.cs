using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttributes : MonoBehaviour
{
    public string m_playerName;
    public static List<int> playerAbilityIDs;
    public float accuraccyAfflication = 0.0f;
    public float playerHealth = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        //This is temporary and will be loaded either with a player pref or txt document
        LoadPlayerAbilityIDs();
        m_playerName = "Mike";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("RandomEncounterScene");
        }
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
}
