using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    TextMeshProUGUI m_dialogueText;

    [SerializeField]
    float m_timeBetweenSentences;
    float m_timeCounter;
    bool startDisplayingMessages;
    string currentCharactesName;

    // Start is called before the first frame update
    void Start()
    {
        m_sentences = new Queue<string>();
        m_timeBetweenSentences = 3.0f;
        m_timeCounter = m_timeBetweenSentences;
        startDisplayingMessages = false;
        gameObject.SetActive(false);
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
                        m_dialogueText.text = "";
                        startDisplayingMessages = false;
                        optionsScene.SetActive(true);
                        gameObject.SetActive(false);
                }
            }
        }
    }

    public void MoveMade(string name, string abilityName, bool hit, bool critAttack, int damage, float accuraccyAfflication)
    {
  
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
                sentenceList.Add("The ability was a critical strike and did: " + damage + " damage");
            }
            else
            {
                sentenceList.Add("The ability did: " + damage + " damage");
            }
            if (accuraccyAfflication > 0)
            {
                sentenceList.Add(abilityName + " lowered characters's accuraccy by " + (accuraccyAfflication * 100));
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
}
