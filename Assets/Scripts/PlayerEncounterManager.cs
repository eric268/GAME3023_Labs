using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PlayerEncounterManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_healthBar;

    float currentHealth;
    int[] playerMovesID;
    bool isPlayersTurn = true;
    float minimumAccuracyThreshold = 0.3f;
    public GameObject fightScene, optionsScene, move1Button, move2Button, move3Button, move4Button, fightButton, fleeButton, backButton, enemyCharacter;
    public Ability move1Ability, move2Ability, move3Ability, move4Ability;
    public TextMeshProUGUI damageText, critChanceText, accuraccyText;
    public GameObject enemyRef;
    PlayerAttributes playerAttributes;

    // Start is called before the first frame update

    delegate void PlayerMadeMoveDelegate(bool attackHit, float damage, float accuraccyAfflication);
    PlayerMadeMoveDelegate playerMadeMoveDelegate;

    void Start()
    { 

        playerMovesID = new int[4];

        if (PlayerAttributes.playerAbilityIDs == null)
            PlayerAttributes.LoadPlayerAbilityIDs();
        if (EncounterAbilities.abilityList == null)
            EncounterAbilities.LoadAbilities();

        playerAttributes = GetComponent<PlayerAttributes>();

        //Initalize health bar values
        m_healthBar.GetComponent<Slider>().maxValue = playerAttributes.playerHealth;
        m_healthBar.GetComponent<Slider>().value = playerAttributes.playerHealth;
        currentHealth = playerAttributes.playerHealth;

        enemyRef = GameObject.Find("Enemy");
        playerMadeMoveDelegate = enemyRef.GetComponent<EnemyEncounterManager>().PlayerMoveRecieved;

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
        if (isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move1Ability, "Player");
        }

    }
    void Move2ButtonPressed()
    {
        if (isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move2Ability, "Player");
        }
    }
    void Move3ButtonPressed()
    {
        if (isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move3Ability, "Player");
        }
    }
    void Move4ButtonPressed()
    {
        if (isPlayersTurn)
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
        if (userName == "Player")
            Debug.Log("Player used " + ability.abilityName);
        else
            Debug.Log(userName + " used " + ability.abilityName);



        float ranAccuraccy = Random.Range(0.0f, 1.0f);
        float enemyAccuracy = (ability.accuracy - playerAttributes.accuraccyAfflication);
        enemyAccuracy = (enemyAccuracy <= minimumAccuracyThreshold) ? minimumAccuracyThreshold : enemyAccuracy;
        if (ranAccuraccy <= enemyAccuracy)
        {
            float ranCrit = Random.Range(0.0f, 1.0f);
            float damage = 0;
            if (ranCrit <= ability.critChance)
            {
                damage = (2.0f * ability.damage);
                Debug.Log(userName + " did: " + damage + " damage from a critical strike");
            }
            else
            {
                damage = ability.damage;
                Debug.Log(userName + " did: " + damage + " damage");
            }
            Debug.Log("Enemies accuraccy decreased by: " + (ability.accuraccyAfflication * 100) + "%");
            playerMadeMoveDelegate(true, damage, ability.accuraccyAfflication);
        }
        else
        {
            Debug.Log(userName + " missed their attack");
            playerMadeMoveDelegate(false, 0, 0.0f);
        }
        fightScene.SetActive(false);
        optionsScene.SetActive(true);
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
        damageText.text = "Damage: " + ability.damage;
        critChanceText.text = "Crit Chance: " + (int)(ability.critChance * 100) + "%";
        accuraccyText.text = "Accuracy: " + (int)(ability.accuracy * 100) + "%";
        backButton.SetActive(false);
    }
    public void ClearMoveStatDisplay()
    {
        damageText.text = "";
        critChanceText.text = "";
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

    public void EnemyMadeMove(bool hit, int damage, float accurraccyAfflication)
    {
        if (hit)
        {
            currentHealth -= damage;
            m_healthBar.GetComponent<Slider>().value = currentHealth;
            playerAttributes.accuraccyAfflication += accurraccyAfflication;
        }

        fightScene.SetActive(false);
        optionsScene.SetActive(true);
    }
}
