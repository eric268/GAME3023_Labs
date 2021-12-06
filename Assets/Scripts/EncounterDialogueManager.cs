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
        m_timeBetweenSentences = 2.25f;
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
                    sentenceList.Add(abilityName + " was a critical strike and did: " + damage + " damage to " + playerName);
                else
                    sentenceList.Add(abilityName + " was a critical strike and did: " + damage + " damage to the " + enemyName);
            }
            else
            {
                if (name == enemyName)
                    sentenceList.Add(abilityName + " did: " + damage + " damage to " + playerName);
                
                else
                    sentenceList.Add(abilityName + " did: " + damage + " damage to the " + enemyName);

            }
            if (accuraccyAfflication > 0)
            {
                if (name == enemyName)
                    sentenceList.Add(abilityName + " lowered " + playerName + "'s accuracy by " + (accuraccyAfflication * 100));
                else
                {
                    if (m_playerRef.GetComponent<PlayerEncounterManager>().playerAccuracyAtLowestValue)
                    {
                        sentenceList.Add(abilityName + " attempted to lower " + playerName + "'s accuracy");
                        sentenceList.Add("However, " + playerName + "'s accuracy is already at its lowest possible value");
                    }
                    else
                        sentenceList.Add(abilityName + " lowered the " + enemyName + "'s accuracy by " + (accuraccyAfflication * 100));
                }


            }
            foreach (string line in sentenceList)
            {
                m_sentences.Enqueue(line);
            }
        }
        else
        {
            string sentence = name + " used the ability " + abilityName + ". However, it missed.";
            m_sentences.Enqueue(sentence);
        }

    }

    public void PlayerDidNotLevelUp(int xpGiven)
    {
        exitEncounter = true;
        m_sentences.Enqueue("Congratulations you won and you gained " + xpGiven + " XP!");
        m_sentences.Enqueue("Goodluck on the rest of your adventure!");
    }

    public void PlayerEndedLevelingUp()
    {
        m_sentences.Enqueue("Be sure to use your new skills and higher health to fight more challenging enemies!");
        m_sentences.Enqueue("Goodluck on the rest of your adventure!");
    }

    void DisplayDialogueUI()
    {
        gameObject.SetActive(true);
        fightScene.SetActive(false);
        optionsScene.SetActive(false);
    }

    public void PlayerLeveledUp(int xpGiven)
    {
        DisplayDialogueUI();
        playerLeveledUp = true;
        m_sentences.Enqueue("Congratulations you won and you gained " + xpGiven + " XP!");
        m_sentences.Enqueue("Wow you leveled up!");
        m_sentences.Enqueue("You gained " + PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel - 2].healthIncrease + "health!");
        m_sentences.Enqueue("As a reward please select a new move!");
        m_sentences.Enqueue("Beware by selecting a move it will delete the move that was previously in that slot!");
    }

    public void PlayerLeveledUpMultipleLevels()
    {
        DisplayDialogueUI();
        playerLeveledUp = true;
        m_sentences.Enqueue("Wow you leveled up again!");
        m_sentences.Enqueue("You gained " + PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel - 2].healthIncrease + " health!");
        m_sentences.Enqueue("Again please select a new move!");
        m_sentences.Enqueue("Remember selecting a move will delete the move that was previously in that slot!");
    }

    public void NewAbilitySelected(string abilityName)
    {
        DisplayDialogueUI();
        m_sentences.Enqueue("Wahoo you learned " + abilityName);
        if (!m_playerRef.GetComponent<PlayerEncounterManager>().playerLeveledUpMultipleLevels)
        {
            exitEncounter = true;
            m_sentences.Enqueue("Goodluck on the rest of your adventure!");
        }
        else
        {
            m_playerRef.GetComponent<PlayerEncounterManager>().PlayerLeveledUpAgain();
        }

    }

    public void PlayerWonMaxLevel()
    {
        DisplayDialogueUI();
        playerLeveledUp = true;
        m_sentences.Enqueue("Congratulations you won however you are already at the max level!");
        m_sentences.Enqueue("As a reward please select a new move!");
        m_sentences.Enqueue("Beware by selecting a move it will delete the move that was previously in that slot!");
    }

    public void PlayerLostEncounter()
    {
        DisplayDialogueUI();
        exitEncounter = true;
        m_sentences.Enqueue("You were defeated!");
        m_sentences.Enqueue("You will now be returned to the Overworld.");
        m_sentences.Enqueue("Better luck on your next encounter!");
    }

}
