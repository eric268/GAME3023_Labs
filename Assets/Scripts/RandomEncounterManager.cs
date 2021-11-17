using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RandomEncounterManager : MonoBehaviour
{
    int[] playerMovesID;
    bool isPlayersTurn = true;

    public GameObject fightScene, optionsScene, move1Button, move2Button, move3Button, move4Button, fightButton, fleeButton, backButton;
    public Ability move1Ability, move2Ability, move3Ability, move4Ability;
    public TextMeshProUGUI damageText, critChanceText, accuraccyText;
    // Start is called before the first frame update
    
    void Start()
    {
        playerMovesID = new int[4];

        if (PlayerAttributes.playerAbilityIDs == null)
            PlayerAttributes.LoadPlayerAbilityIDs();
        if (EncounterAbilities.abilityList == null)
            EncounterAbilities.LoadAbilities();

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
            Debug.Log("Enemy made a move");
        }
    }

    void Move1ButtonPressed()
    {
        if (isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move1Ability);
        }

    }
    void Move2ButtonPressed()
    {
        if (isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move2Ability);
        }
    }
    void Move3ButtonPressed()
    {
        if (isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move2Ability);
        }
    }
    void Move4ButtonPressed()
    {
        if (isPlayersTurn)
        {
            isPlayersTurn = false;
            UseAbility(move2Ability);
        }
    }

    void LoadButtonAbility(ref GameObject button, ref Ability ablity, int abilityID)
    {
        ablity = Ability.CopyAbility(EncounterAbilities.abilityList[abilityID]);
        button.GetComponentInChildren<TextMeshProUGUI>().text = ablity.abilityName;
    }

    void UseAbility(Ability ability)
    {
        float ranAccuraccy = Random.Range(0.0f, 1.0f);
        if (ranAccuraccy <= ability.accuracy)
        {
            float ranCrit = Random.Range(0.0f, 1.0f);
            if (ranCrit <= ability.critChance)
                Debug.Log("Player did: " + (2.0f * ability.damage) + " damage to enemy from a critical strike");
            else
                Debug.Log("Player did: " + ability.damage + " damage to enemy");
        }
        else
            Debug.Log("Player missed their attack");

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
}
