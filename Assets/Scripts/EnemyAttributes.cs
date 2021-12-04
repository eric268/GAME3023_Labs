using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttributes : MonoBehaviour
{
    [SerializeField]
    public string m_enemyName;
    public static List<int> enemyAbilityIDs;
    public float enemyAccuraccyAfflication = 0.0f;
    [SerializeField]
    public float enemyHealth;


    [SerializeField]
    Enemy[] enemyArray;

    const int numberOfEnemies = 5;


    //The higher the value the better the moves the AI will make
    public int xpGivenOnKill;
    public float AIIntelligence;
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
        LoadRandomEnemy(ChooseRandomEnemy());
    }

    Enemy ChooseRandomEnemy()
    {
        float randomNumber = Random.Range(0.0f, 1.0f);
        if (randomNumber < 0.3f)
            return enemyArray[0];
        else if (randomNumber < 0.55f)
            return enemyArray[1];
        else if (randomNumber < 0.75f)
            return enemyArray[2];
        else if (randomNumber < 0.9f)
            return enemyArray[3];
        else
            return enemyArray[4];
    }

    void LoadRandomEnemy(Enemy enemy)
    {
        m_enemyName = enemy.name;
        enemyHealth = enemy.health;
        AIIntelligence = enemy.Intelligence;
        xpGivenOnKill = enemy.xpGiven;
        GetComponent<Image>().sprite = enemy.spriteIcon;
    }

    void LoadEnemyAbilityIDs()
    {
        int abilitiesChosen = 0;

        while(abilitiesChosen < 4)
        {
            int randomAbilityIndex = Random.Range(0, EncounterAbilities.totalNumberOfAbilities);
           
            if (!enemyAbilityIDs.Contains(randomAbilityIndex))
            {
                abilitiesChosen++;
                enemyAbilityIDs.Add(randomAbilityIndex);
            }
        }
    }
}
