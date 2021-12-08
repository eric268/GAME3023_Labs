using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    private int randNum = 0;
    private bool isInTallGrass = false;
    [SerializeField]
    private float m_fMovementSpeed = 1.0f;

    [SerializeField]
    private Rigidbody2D m_rigidBody;

    private Vector3 m_vMovementVector = Vector3.zero;

    private Animator m_animator;

    string[] animBoolNames = { "AnimUp", "AnimDown", "AnimLeft", "AnimRight" };

    // Start is called before the first frame update
    void Start()
    {
        if (!m_rigidBody)
            Debug.LogError("Main Character rigid body not found");

        if (PlayerPrefs.GetInt("Saved") == 1)
        {
            PlayerPosLoad();
        }
        m_animator = GetComponent<Animator>();
        if (!m_animator)
            Debug.LogError("Main Character animator not found");

        GetComponent<SaveLoadPlayerPos>().LoadPlayerPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    GetComponent<SaveLoadPlayerPos>().SavePlayerPosition();
        //    SceneManager.LoadScene("RandomEncounterScene");
        //}

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");


        if (y > 0)
        {
            ResetAnimBools("AnimUp");
        }
        else if (y < 0)
        {
            ResetAnimBools("AnimDown");
        }
        else if (x < 0)
        {
            ResetAnimBools("AnimLeft");
        }
        else if (x > 0)
        {
            ResetAnimBools("AnimRight");
        }
        else
        {
            m_animator.enabled = false;
        }


        m_vMovementVector = new Vector3(x, y, 0).normalized * m_fMovementSpeed;

        m_rigidBody.velocity = m_vMovementVector;
        if(m_rigidBody.velocity.magnitude!=0)
        {
            if(isInTallGrass)
            {
                Debug.Log("touching tall grass");
                randNum = Random.RandomRange(0, 1500);
                if (randNum < 10)
                {
                    Debug.Log("the odds are right");

                    GetComponent<SaveLoadPlayerPos>().SavePlayerPosition();
                    StartCoroutine(BattleEntrySequence());
                }
            }
        }
        
    }
    IEnumerator BattleEntrySequence()
    {
        Debug.Log("entering battle");
        PlayerEvents events = FindObjectOfType<PlayerEvents>();
        events.onEnterEncounterEvent.Invoke();
        MusicManager.Instance.PlayTrack(MusicManager.TrackID.Battle);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("RandomEncounterScene");
    }
    public void PlayerPosLoad()
    {

    }

    void ResetAnimBools(string trueBool)
    {
        m_animator.enabled = true;
        foreach (string name in animBoolNames)
        {
            if (name == trueBool)
                m_animator.SetBool(name, true);
            else
                m_animator.SetBool(name, false);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TallGrass")
        {
            isInTallGrass = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TallGrass")
        {
            isInTallGrass = false;
        }
    }
}
