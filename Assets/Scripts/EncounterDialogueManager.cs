using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum EncounterCharacter
{
    PLAYER,
    ENEMY,
}

public class EncounterDialogueManager : MonoBehaviour
{
    public Queue<string> m_sentences;
    public GameObject fightScene, optionsScene;

    [SerializeField]
    public GameObject m_playerRef, m_enemyRef;

    [SerializeField]
    TextMeshProUGUI m_dialogueText;

    [SerializeField]
    float m_timeBetweenSentences;
    float m_timeCounter;
    bool startDisplayingMessages;
    string currentCharactesName;
    bool playerLeveledUp = false, exitEncounter = false;
    bool enemyTurn = false;
    string enemyName, playerName;

    delegate void LoadNewPlayerAbilitiesDelegate();
    LoadNewPlayerAbilitiesDelegate loadNewPlayerAbilitiesDelegate;

    delegate void MakeEnemyUseAbilityDelegate();
    MakeEnemyUseAbilityDelegate makeEnemyUseAbilityDelegate;

    // Start is called before the first frame update
    void Start()
    {
        m_sentences = new Queue<string>();
        m_timeBetweenSentences = 2.5f;
        m_timeCounter = m_timeBetweenSentences;
        startDisplayingMessages = false;
        gameObject.SetActive(false);

        loadNewPlayerAbilitiesDelegate = m_playerRef.GetComponent<PlayerEncounterManager>().LoadNewAbilities;
        makeEnemyUseAbilityDelegate = m_enemyRef.GetComponent<EnemyEncounterManager>().MakeEnemyUseAbility;
    }

    // Update is called once per frame
    void Update()
    {
        if (startDisplayingMessages)
        {
            m_timeCounter += Time.deltaTime;

            if(m_timeCounter > m_timeBetweenSentences)
            {
                if (m_sentences.Count > 0)
                {
                    m_dialogueText.text = m_sentences.Dequeue();
                    m_timeCounter = 0.0f;
                }
                else
                {
                    if (exitEncounter)
                    {
                        SceneManager.LoadScene("Main Scene");
                    }
                    else if (playerLeveledUp)
                    {
                        loadNewPlayerAbilitiesDelegate();
                    }
                    else if (enemyTurn)
                    {
                        makeEnemyUseAbilityDelegate();
                    }
                    else
                    {
                        m_dialogueText.text = "";
                        startDisplayingMessages = false;
                        optionsScene.SetActive(true);
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void MoveMade(string name, string abilityName, bool hit, bool critAttack, int damage, float accuraccyAfflication)
    {
        enemyName = m_enemyRef.GetComponent<EnemyAttributes>().m_enemyName;
        playerName = m_playerRef.GetComponent<PlayerAttributes>().m_playerName;

        if (name == playerName)
            enemyTurn = true;
        else
            enemyTurn = false;

        fightScene.SetActive(false);
        gameObject.SetActive(true);
        startDisplayingMessages = true;
        currentCharactesName = name;
        if (hit)
        {
            List<string> sentenceList = new List<string>();
            sentenceList.Add(name + " used the ability " + abilityName);
            if (critAttack)
            {
                if (name == enemyName)
                    sentenceList.Add( abilityName + " was a critical strike and did: " + damage + " damage to" + playerName);
                else
                    sentenceList.Add(abilityName + " was a critical strike and did: " + damage + " damage to " + enemyName);
            }
            else
            {
                if (name == enemyName)
                    sentenceList.Add(abilityName + " did: " + damage + " damage to " + playerName);
                
                else
                    sentenceList.Add(abilityName + " did: " + damage + " damage to " + enemyName);

            }
            if (accuraccyAfflication > 0)
            {
                if (name == enemyName)
                    sentenceList.Add(abilityName + " lowered the " + playerName +"'s accuraccy by " + (accuraccyAfflication * 100));
                else
                    sentenceList.Add(abilityName + " lowered the " + enemyName + "'s accuraccy by " + (accuraccyAfflication * 100));

            }
            foreach (string line in sentenceList)
            {
                m_sentences.Enqueue(line);
            }
        }
        else
        {
            string sentence = name + " used " + abilityName + ". However, it missed.";
            m_sentences.Enqueue(sentence);
        }

    }

    public void PlayerDidNotLevelUp(int xpGiven)
    {
        exitEncounter = true;
        m_sentences.Enqueue("Congratulations you won and you gained " + xpGiven + " XP!");
        m_sentences.Enqueue("Goodluck on the rest of your adventure!");
    }

    public void PlayerLeveledUp(int xpGiven)
    {
        gameObject.SetActive(true);
        fightScene.SetActive(false);
        optionsScene.SetActive(false);
        playerLeveledUp = true;
        m_sentences.Enqueue("Congratulations you won and you gained " + xpGiven + " XP!");
        m_sentences.Enqueue("Wow you leveled up!");
        m_sentences.Enqueue("As a reward please select a new move!");
        m_sentences.Enqueue("Beware by selecting a move it will delete the move that was previously in that slot!");
    }

    public void NewAbilitySelected(string abilityName)
    {
        gameObject.SetActive(true);
        fightScene.SetActive(false);
        optionsScene.SetActive(false);
        exitEncounter = true;
        m_sentences.Enqueue("Wahoo you learned " + abilityName);
        m_sentences.Enqueue("Goodluck on the rest of your adventure!");
    }
}
