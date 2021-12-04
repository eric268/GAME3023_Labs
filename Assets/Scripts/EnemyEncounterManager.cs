using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

[System.Serializable]
public class EnemyEncounterManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_healthBar, m_DialogueBox;


    public GameObject playerRef, m_optionsScene;
    public EnemyAttributes enemyAttributes;
    public List<Ability> enemyAbilities;
    float minimumAccuracyThreshold = 0.3f;
    float AIIntelligence = 0.6f;
    float currentHealth;
    bool useAbility = false;
    
    delegate void EnemyMadeMoveDelegate(string enemyName, string abilityName, bool hit, bool critAttack, int damage, float accuraccyAfflication);
    EnemyMadeMoveDelegate enemyMadeMoveDelegate;

    delegate void PlayerWonDelegate();
    PlayerWonDelegate playerWonDelegate;



    // Start is called before the first frame update
    void Start()
    {
        if (PlayerAttributes.playerAbilityIDs == null)
            PlayerAttributes.LoadPlayerAbilityIDs();
        if (EncounterAbilities.abilityList == null)
            EncounterAbilities.LoadAbilities();

        playerRef = GameObject.Find("Player");

        enemyMadeMoveDelegate = m_DialogueBox.GetComponent<EncounterDialogueManager>().MoveMade;
        enemyMadeMoveDelegate += playerRef.GetComponent<PlayerEncounterManager>().EnemyMadeMove;
       

        playerWonDelegate = m_DialogueBox.GetComponent<EncounterDialogueManager>().PlayerWonEncounter;

        enemyAttributes = GetComponent<EnemyAttributes>();
        enemyAbilities = new List<Ability>();

        //Initalize health bar values
        m_healthBar.GetComponent<Slider>().maxValue = enemyAttributes.enemyHealth;
        m_healthBar.GetComponent<Slider>().value = enemyAttributes.enemyHealth;
        currentHealth = enemyAttributes.enemyHealth;

        Debug.Log("Enemy health: " + currentHealth);

        LoadEnemeyAbilities();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MakeEnemyUseAbility()
    {
        UseAbility(ChooseEnemyAttack(), "Enemy");
        m_DialogueBox.SetActive(true);
    }

    public void PlayerMoveRecieved(string name, string abilityName, bool hit,bool critAttack, int damage, float accuraccyAfflication)
    {
        useAbility = true;
        if (hit)
        {
            currentHealth -= damage;
            m_healthBar.GetComponent<Slider>().value = currentHealth;
            enemyAttributes.enemyAccuraccyAfflication += accuraccyAfflication;

            if (currentHealth <=0)
            {
                playerWonDelegate();
            }

            if (enemyAttributes.enemyHealth < enemyAttributes.enemyLowHealthThreshold)
            {
                enemyAttributes.enemyLowHealth = true;
            }
        }
        if (playerRef.GetComponent<PlayerAttributes>().accuraccyAfflication > enemyAttributes.playerLowAccuraccyAfflictionThreshold)
        {
            enemyAttributes.playerLowAccuraccyAffliction = true;
        }
        if (playerRef.GetComponent<PlayerAttributes>().playerHealth < enemyAttributes.playerLowHealthThreshold)
        {
            enemyAttributes.playerLowHealth = true;
        }
    }

    public void UseAbility(Ability ability, string userName)
    {
        useAbility = false;
        bool attackHit = false;
        int damage = 0;
        bool critHit = false;
        float accAfflication = 0;
        string abilityName = "";

        float ranAccuraccy = UnityEngine.Random.Range(0.0f, 1.0f);
        float enemyAccuracy = (ability.accuracy - enemyAttributes.enemyAccuraccyAfflication);

        enemyAccuracy = (enemyAccuracy <= minimumAccuracyThreshold) ? minimumAccuracyThreshold : enemyAccuracy;
        if (ranAccuraccy <= enemyAccuracy)
        {
            attackHit = true;
            accAfflication = ability.accuraccyAfflication;
            abilityName = ability.abilityName;
            float ranCrit = UnityEngine.Random.Range(0.0f, 1.0f);
            if (ranCrit <= ability.critChance)
            {
                critHit = true;
                damage = (int)(2.0f * ability.damage);
            }
            else
            {
                damage = (int)ability.damage;
            }
        }
        m_optionsScene.SetActive(false);
        enemyMadeMoveDelegate(enemyAttributes.m_enemyName, abilityName, attackHit, critHit, damage, accAfflication);

    }

    void LoadEnemeyAbilities()
    {
        for (int i = 0; i < 4; i++)
        {
            int abilityID = EnemyAttributes.enemyAbilityIDs[i];
            enemyAbilities.Add(Ability.CopyAbility(EncounterAbilities.abilityList[abilityID]));
        }
        enemyAbilities = enemyAbilities.OrderByDescending(x => x.damage).ToList();
    }  

    Ability ChooseEnemyAttack()
    {
        float damageUtilityScore = 1.0f;
        float accuraccyAfflictionUtilityScore = 1.0f;
        float criticalStrikeUtilityScore = 1.0f;
        float intelligenceOffset = UnityEngine.Random.Range(0.0f, 1.0f);
        int choiceID = (intelligenceOffset <= AIIntelligence) ? 0 : 1;
        List<AIMoveValue> scoreArray = new List<AIMoveValue>();
        for (int i = 0; i < 4; i++)
        {
            int abilityID = EnemyAttributes.enemyAbilityIDs[i];
            scoreArray.Add(new AIMoveValue(0.0f, i));
        }

        if (enemyAttributes.playerLowHealth)
            damageUtilityScore = 2.0f;
        else
            damageUtilityScore = 1.0f;

        if (!enemyAttributes.playerLowAccuraccyAffliction && !enemyAttributes.enemyLowHealth)
            accuraccyAfflictionUtilityScore = 2.0f;
        else
            accuraccyAfflictionUtilityScore = 1.0f;

        if (enemyAttributes.enemyLowHealth)
            criticalStrikeUtilityScore = 1.5f;
        else
            criticalStrikeUtilityScore = 1.0f;

        for (int i = 0; i < 4; i++)
        {
            float damageScore = damageUtilityScore * enemyAbilities[i].damage;
            float accuraccyAfflictionScore = (accuraccyAfflictionUtilityScore * enemyAbilities[i].accuraccyAfflication * 100);
            float criticalStrikeScore = (criticalStrikeUtilityScore * enemyAbilities[i].critChance * 100);
            float accurraccyScore = (enemyAbilities[i].accuracy * 5);

            scoreArray[i].utilityScore =  damageScore + accuraccyAfflictionScore + criticalStrikeUtilityScore + accurraccyScore;

        }

        scoreArray = scoreArray.OrderByDescending(x => x.utilityScore).ToList();
        return enemyAbilities[scoreArray[choiceID].position];
    }
}


class AIMoveValue
{
    public float utilityScore;
    public int position;

    public AIMoveValue()
    {
        utilityScore = 0.0f;
        position = -1;
    }
    public AIMoveValue(float uScore, int ID)
    {
        utilityScore = uScore;
        position = ID;
    }
}