using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    
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


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
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


        //Debug.Log("X: " +  x);
        //Debug.Log("Y: " + y);




        m_vMovementVector = new Vector3(x, y, 0).normalized * m_fMovementSpeed;

        m_rigidBody.velocity = m_vMovementVector;
        
    }
    public void PlayerPosLoad()
    {
        float xPos = PlayerPrefs.GetFloat("X Position");
        float yPos = PlayerPrefs.GetFloat("Y Position");

        transform.position = new Vector3(xPos,yPos,transform.position.z);
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
}
