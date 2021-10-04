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

    // Start is called before the first frame update
    void Start()
    {
        if (!m_rigidBody)
            Debug.LogError("Main Character rigid body not found");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        m_vMovementVector = new Vector3(x, y, 0).normalized * m_fMovementSpeed;

        m_rigidBody.velocity = m_vMovementVector;
        
    }
}
