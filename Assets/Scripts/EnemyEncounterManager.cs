using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public class EnemyEncounterManager : MonoBehaviour
{
    public GameObject playerRef;
    public EnemyAttributes enemyAttributes;
    public List<Ability> enemyAbilities;
    float minimumAccuracyThreshold = 0.3f;
    float AIIntelligence = 0.6f;


    delegate void EnemyMadeMoveDelegate(bool hit, int damage, float accuraccyAfflication);
    EnemyMadeMoveDelegate enemyMadeMoveDelegate;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.Find("Player");
        enemyMadeMoveDelegate = playerRef.GetComponent<PlayerEncounterManager>().EnemyMadeMove;
        enemyAttributes = GetComponent<EnemyAttributes>();
        enemyAbilities = new List<Ability>();
        LoadEnemeyAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerMoveRecieved(bool hit, float damage, float accuraccyAfflication)
    {
        if (hit)
        {
            enemyAttributes.enemyHealth -= damage;
            enemyAttributes.enemyAccuraccyAfflication += accuraccyAfflication;

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

        UseAbility(ChooseEnemyAttack(), "Enemy");
    }

    public void UseAbility(Ability ability, string userName)
    {
        float ranAccuraccy = UnityEngine.Random.Range(0.0f, 1.0f);
        float enemyAccuracy = (ability.accuracy - enemyAttributes.enemyAccuraccyAfflication);


        enemyAccuracy = (enemyAccuracy <= minimumAccuracyThreshold) ? minimumAccuracyThreshold : enemyAccuracy;
        Debug.Log("Enemy used " + ability.abilityName);
        if (ranAccuraccy <= enemyAccuracy)
        {
            float ranCrit = UnityEngine.Random.Range(0.0f, 1.0f);
            int damage = 0;
            if (ranCrit <= ability.critChance)
            {
                damage = (int)(2.0f * ability.damage);
                Debug.Log(userName + " did: " + damage + " damage from a critical strike");
                enemyMadeMoveDelegate(true, damage, ability.accuraccyAfflication);
            }
            else
            {
                damage = (int)ability.damage;
                Debug.Log(userName + " did: " + damage + " damage");
                enemyMadeMoveDelegate(true, damage, ability.accuraccyAfflication);
            }
            Debug.Log("Players accuraccy decreased by: " + (ability.accuraccyAfflication * 100) + "%");
        }
        else
        {
            Debug.Log(userName + " missed their attack");
            enemyMadeMoveDelegate(false, 0, 0.0f);
        }
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