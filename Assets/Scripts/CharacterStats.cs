using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI m_playerCurrentHealth, m_playerLevel, m_playerXP, m_opponenetName, m_opponentHealth, m_playerXPLimit;

    [SerializeField]
    public GameObject m_playerRef, m_enemyRef;

    bool updateOnce = false;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (!updateOnce)
        {
            updateOnce = true;
            m_playerLevel.text = PlayerAttributes.currentLevel.ToString();
            m_playerXP.text = m_playerRef.GetComponent<PlayerAttributes>().currentXP.ToString();
            m_playerCurrentHealth.text = m_playerRef.GetComponent<PlayerAttributes>().playerCurrentHealth.ToString();
            m_opponenetName.text = m_enemyRef.GetComponent<EnemyAttributes>().m_enemyName;
            m_opponentHealth.text = m_enemyRef.GetComponent<EnemyEncounterManager>().currentHealth.ToString();
            m_playerXPLimit.text = "/" + PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel - 1].xpTillNextLevel;
        }
    }
}
