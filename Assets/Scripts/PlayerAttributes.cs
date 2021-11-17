using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttributes : MonoBehaviour
{
    public static List<int> playerAbilityIDs;
    // Start is called before the first frame update
    void Start()
    {
        //This is temporary and will be loaded either with a player pref or txt document
        LoadPlayerAbilityIDs();
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
        playerAbilityIDs = new List<int>();
        playerAbilityIDs.Add(0);
        playerAbilityIDs.Add(4);
        playerAbilityIDs.Add(5);
        playerAbilityIDs.Add(7);
    }
}
