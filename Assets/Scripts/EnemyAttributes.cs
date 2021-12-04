using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributes : MonoBehaviour
{ 

    public static List<int> enemyAbilityIDs;
    public float enemyAccuraccyAfflication = 0.0f;
    public float enemyHealth = 100.0f;
    
    //The higher the value the better the moves the AI will make
    public float AIIntelligence = 0.60f;
    public int playerLowHealthThreshold = 50;
    public int enemyLowHealthThreshold = 50;
    public float playerLowAccuraccyAfflictionThreshold = 0.75f;
    public bool enemyLowHealth = false;
    public bool playerLowHealth = false;
    public bool playerLowAccuraccyAffliction = false;


    void Start()
    {
        enemyAbilityIDs = new List<int>();
        LoadEnemyAbilityIDs();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadEnemyAbilityIDs()
    {
        enemyAbilityIDs.Add(1);
        enemyAbilityIDs.Add(3);
        enemyAbilityIDs.Add(4);
        enemyAbilityIDs.Add(6);
    }
}
