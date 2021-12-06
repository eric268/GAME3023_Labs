using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PlayerEncounterManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_healthBar, m_DialogueBox, StatDisplayManager;

    float currentHealth;
    int[] playerMovesID;
    bool isPlayersTurn = true;
    float minimumAccuracyThreshold = 0.3f;
    [SerializeField]
    public GameObject fightScene, optionsScene, move1Button, move2Button, move3Button, move4Button, fightButton, fleeButton, backButton, enemyCharacter;
    [SerializeField]
    public Ability move1Ability, move2Ability, move3Ability, move4Ability;
    public TextMeshProUGUI damageText, critChanceText, accuraccyText, accAfflicationText;
    public GameObject enemyRef;
    PlayerAttributes playerAttributes;
    bool chooseNewAbilities = false;
    public bool playerAccuracyAtLowestValue = false;
    public bool playerLeveledUpMultipleLevels = false;

    // Start is called before the first frame update

    delegate void PlayerMadeMoveDelegate(string playerName, string abilityName, bool attackHit, bool critAttack, int damage, float accuraccyAfflication);
    PlayerMadeMoveDelegate playerMadeMoveDelegate;

    delegate void NewMoveSelectedDelegate(string abilityName);
    NewMoveSelectedDelegate newMoveSelectedDelegate;

    void Start()
    { 
        playerMovesID = new int[4];

        if (PlayerAttributes.playerAbilityIDs == null)
            PlayerAttributes.LoadPlayerAbilityIDs();
        if (EncounterAbilities.abilityList == null)
            EncounterAbilities.LoadAbilities();

        playerAttributes = GetComponent<PlayerAttributes>();
        playerAttributes.LoadPlayerStats();
        //Initalize health bar values
        m_healthBar.GetComponent<Slider>().maxValue = playerAttributes.playerCurrentHealth;
        m_healthBar.GetComponent<Slider>().value = playerAttributes.playerCurrentHealth;
        currentHealth = playerAttributes.playerCurrentHealth;
        StatDisplayManager.GetComponent<CharacterStats>().m_playerCurrentHealth.text = currentHealth.ToString();

        enemyRef = GameObject.Find("Enemy");
        
        playerMadeMoveDelegate = m_DialogueBox.GetComponent<EncounterDialogueManager>().MoveMade;
        playerMadeMoveDelegate += enemyRef.GetComponent<EnemyEncounterManager>().PlayerMoveRecieved;

        newMoveSelectedDelegate = m_DialogueBox.GetComponent<EncounterDialogueManager>().NewAbilitySelected;

        LoadButtonAbility(ref move1Button, ref move1Ability, PlayerAttributes.playerAbilityIDs[0]);
        LoadButtonAbility(ref move2Button, ref move2Ability, PlayerAttributes.playerAbilityIDs[1]);
        LoadButtonAbility(ref move3Button, ref move3Ability, PlayerAttributes.playerAbilityIDs[2]);
        LoadButtonAbility(ref move4Button, ref move4Ability, PlayerAttributes.playerAbilityIDs[3]);


        move1Button.GetComponent<Button>().onClick.AddListener(Move1ButtonPressed);
        move2Button.GetComponent<Button>().onClick.AddListener(Move2ButtonPressed);
        move3Button.GetComponent<Button>().onClick.AddListener(Move3ButtonPressed);
        move4Button.GetComponent<Button>().onClick.AddListener(Move4ButtonPressed);
        fightButton.GetComponent<Button>().onClick.AddListener(FightButtonPressed);
        fleeButton.GetComponent<Button>().onClick.AddListener(FleeButtonPressed);
        backButton.GetComponent<Button>().onClick.AddListener(BackButtonPressed);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayersTurn)
        {
            isPlayersTurn = true;
        }
    }

    void Move1ButtonPressed()
    {
        if (chooseNewAbilities)
        {
            PlayerAttributes.playerAbilityIDs[0] = NewAbilityScript.newAbilityID[0];
            newMoveSelectedDelegate(move1Ability.abilityName);
            PlayerAttributes.SavePlayerAbilities();
        }
        else if (isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move1Ability, "Player");
        }

    }
    void Move2ButtonPressed()
    {
        if (chooseNewAbilities)
        {
            PlayerAttributes.playerAbilityIDs[1] = NewAbilityScript.newAbilityID[1];
            newMoveSelectedDelegate(move2Ability.abilityName);
            PlayerAttributes.SavePlayerAbilities();
        }
        else if(isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move2Ability, "Player");
        }
    }
    void Move3ButtonPressed()
    {
        if (chooseNewAbilities)
        {
            PlayerAttributes.playerAbilityIDs[2] = NewAbilityScript.newAbilityID[2];
            newMoveSelectedDelegate(move3Ability.abilityName);
            PlayerAttributes.SavePlayerAbilities();
        }
        else if(isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move3Ability, "Player");
        }
    }
    void Move4ButtonPressed()
    {
        if (chooseNewAbilities)
        {
            PlayerAttributes.playerAbilityIDs[3] = NewAbilityScript.newAbilityID[3];
            newMoveSelectedDelegate(move4Ability.abilityName);
            PlayerAttributes.SavePlayerAbilities();
        }
        else if(isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move4Ability, "Player");
        }
    }

    void LoadButtonAbility(ref GameObject button, ref Ability ablity, int abilityID)
    {
        ablity = Ability.CopyAbility(EncounterAbilities.abilityList[abilityID]);
        button.GetComponentInChildren<TextMeshProUGUI>().text = ablity.abilityName;
    }

    void UseAbility(Ability ability, string userName)
    {
        bool attackHit = false;
        bool critHit = false;
        int damage = 0;
        float accAfflication = 0;
        string abilityName = ability.abilityName;

        float ranAccuraccy = Random.Range(0.0f, 1.0f);
        float playerAccuracy = (ability.accuracy - playerAttributes.accuraccyAfflication);
        if (playerAccuracy <= minimumAccuracyThreshold)
        {
            playerAccuracy = minimumAccuracyThreshold;
            playerAccuracyAtLowestValue = true;
        }

        
        if (ranAccuraccy <= playerAccuracy)
        {
            float ranCrit = Random.Range(0.0f, 1.0f);
            attackHit = true;

            accAfflication = ability.accuraccyAfflication;
            if (ranCrit <= ability.critChance)
            {
                critHit = true;
                damage = (2 * ability.damage);
            }
            else
            {
                damage = ability.damage;
            }
        }
        playerMadeMoveDelegate(playerAttributes.m_playerName, abilityName, attackHit, critHit, damage, accAfflication);
    }

    public void Button1MouseOver()
    {
        UpdateMoveStatDisplay(move1Ability);
    }
    public void Button2MouseOver()
    {
        UpdateMoveStatDisplay(move2Ability);
    }
    public void Button3MouseOver()
    {
        UpdateMoveStatDisplay(move3Ability);
    }
    public void Button4MouseOver()
    {
        UpdateMoveStatDisplay(move4Ability);
    }
    void UpdateMoveStatDisplay(Ability ability)
    {
        float accuraccyDebuff = 0.0f;


        float accuracy = ability.accuracy;
        accuraccyDebuff = playerAttributes.accuraccyAfflication;
        if (ability.accuracy - accuraccyDebuff < minimumAccuracyThreshold)
            accuracy = minimumAccuracyThreshold;
        else
            accuracy -= accuraccyDebuff;

        damageText.text = "Damage: " + ability.damage;
        critChanceText.text = "Crit Chance: " + (int)(ability.critChance * 100) + "%";
        if (!chooseNewAbilities)
            accuraccyText.text = "Accuracy: " + (int)((accuracy)* 100) + "%";
        else
            accuraccyText.text = "Accuracy: " + (int)((ability.accuracy) * 100) + "%";
        accAfflicationText.text = "Acc. Debuff " + (int)(ability.accuraccyAfflication * 100) + "%";
        backButton.SetActive(false);
    }
    public void ClearMoveStatDisplay()
    {
        damageText.text = "";
        critChanceText.text = "";
        accAfflicationText.text = "";
        accuraccyText.text = "";
        backButton.SetActive(true);
    }

    void FightButtonPressed()
    {
        fightScene.SetActive(true);
        optionsScene.SetActive(false);
    }

    void FleeButtonPressed()
    {
        SceneManager.LoadScene("Main Scene");
    }

    void BackButtonPressed()
    {
        fightScene.SetActive(false);
        optionsScene.SetActive(true);
    }

    public void EnemyMadeMove(string enemyName,  string abilityName, bool hit,bool critAttack, int damage, float accurraccyAfflication)
    {
        if (hit)
        {
            currentHealth -= damage;
            currentHealth = (currentHealth < 0) ? 0 : currentHealth;
            StatDisplayManager.GetComponent<CharacterStats>().m_playerCurrentHealth.text = currentHealth.ToString();
            m_healthBar.GetComponent<Slider>().value = currentHealth;
            playerAttributes.accuraccyAfflication += accurraccyAfflication;
            if (currentHealth <= 0)
                m_DialogueBox.GetComponent<EncounterDialogueManager>().PlayerLostEncounter();
        }
    }

    public void LoadNewAbilities()
    {
        NewAbilityScript.GenerateAbilityListPlayerDoesntHave();
        
        LoadButtonAbility(ref move1Button, ref move1Ability, NewAbilityScript.newAbilityID[0]);
        LoadButtonAbility(ref move2Button, ref move2Ability, NewAbilityScript.newAbilityID[1]);
        LoadButtonAbility(ref move3Button, ref move3Ability, NewAbilityScript.newAbilityID[2]);
        LoadButtonAbility(ref move4Button, ref move4Ability, NewAbilityScript.newAbilityID[3]);

        chooseNewAbilities = true;
        m_DialogueBox.SetActive(false);
        fightScene.SetActive(true);
    }
    public void PlayerWonEncounter()
    {
        int xpGiven = enemyRef.GetComponent<EnemyAttributes>().xpGivenOnKill;
        int levelXPNeeded = PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel-1].xpTillNextLevel;

        if (PlayerAttributes.currentLevel >= 10)
        {
            m_DialogueBox.GetComponent<EncounterDialogueManager>().PlayerWonMaxLevel();
        }
        else if (levelXPNeeded <= xpGiven + GetComponent<PlayerAttributes>().currentXP)
        {
            int leftOverXP = PlayerLeveledUp(xpGiven, levelXPNeeded);

            GetComponent<PlayerAttributes>().SavePlayerStats();

            m_DialogueBox.GetComponent<EncounterDialogueManager>().PlayerLeveledUp(xpGiven);

            if (leftOverXP >= PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel -1].xpTillNextLevel)
            {
                playerLeveledUpMultipleLevels = true;
            }
        }
        else
        {
            GetComponent<PlayerAttributes>().currentXP += xpGiven;
            GetComponent<PlayerAttributes>().SavePlayerStats();
            StatDisplayManager.GetComponent<CharacterStats>().m_playerXP.text = GetComponent<PlayerAttributes>().currentXP.ToString();
            m_DialogueBox.GetComponent<EncounterDialogueManager>().PlayerDidNotLevelUp(xpGiven);
        }
    }

    public int PlayerLeveledUp(int xPGained, int xpNeeded)
    {
        int leftOverXP = xPGained + GetComponent<PlayerAttributes>().currentXP - xpNeeded;
        StatDisplayManager.GetComponent<CharacterStats>().m_playerXPLimit.text = PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel - 1].xpTillNextLevel.ToString();
        GetComponent<PlayerAttributes>().currentXP = leftOverXP;
        GetComponent<PlayerAttributes>().playerCurrentHealth += PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel - 1].healthIncrease;
        currentHealth += PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel - 1].healthIncrease;
        PlayerAttributes.currentLevel++;

        StatDisplayManager.GetComponent<CharacterStats>().m_playerLevel.text = PlayerAttributes.currentLevel.ToString();
        StatDisplayManager.GetComponent<CharacterStats>().m_playerXP.text = GetComponent<PlayerAttributes>().currentXP.ToString() + "/";
        StatDisplayManager.GetComponent<CharacterStats>().m_playerXPLimit.text = PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel - 1].xpTillNextLevel.ToString();
        StatDisplayManager.GetComponent<CharacterStats>().m_playerCurrentHealth.text = currentHealth.ToString();
        m_healthBar.GetComponent<Slider>().maxValue = playerAttributes.playerCurrentHealth;
        m_healthBar.GetComponent<Slider>().value = currentHealth;

        return leftOverXP;
    }

    public void PlayerLeveledUpAgain()
    {
        int currentXP = GetComponent<PlayerAttributes>().currentXP;
        int xpNeeded = PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel - 1].xpTillNextLevel;
        
        if (xpNeeded <= currentXP)
        {
            int leftOverXP = PlayerLeveledUp(0, xpNeeded);

            if (PlayerAttributes.levelUpInfo[PlayerAttributes.currentLevel - 1].xpTillNextLevel >= leftOverXP)
                playerLeveledUpMultipleLevels = false;
            m_DialogueBox.GetComponent<EncounterDialogueManager>().PlayerLeveledUpMultipleLevels();
        }
        else
        {
            GetComponent<PlayerAttributes>().SavePlayerStats();
            StatDisplayManager.GetComponent<CharacterStats>().m_playerXP.text = GetComponent<PlayerAttributes>().currentXP.ToString();
            m_DialogueBox.GetComponent<EncounterDialogueManager>().PlayerEndedLevelingUp();
        }
       
    }
}
