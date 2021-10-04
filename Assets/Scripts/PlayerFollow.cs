using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject m_tPlayerPosition;
    void Start()
    {
        m_tPlayerPosition = GameObject.Find("Main Character");
        if (!m_tPlayerPosition)
            Debug.LogError("Main Camera: Unable to find player");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_tPlayerPosition)
        {
            this.transform.position = new Vector3(m_tPlayerPosition.transform.position.x, m_tPlayerPosition.transform.position.y, this.transform.position.z);
        }
        else
            m_tPlayerPosition = GameObject.Find("Main Character");

    }
}
