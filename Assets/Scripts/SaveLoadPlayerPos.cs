using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPlayerPos : MonoBehaviour
{
    [SerializeField]
    GameObject m_player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("X Position", m_player.transform.position.x);
        PlayerPrefs.SetFloat("Y Position", m_player.transform.position.y);
        PlayerPrefs.SetInt("Saved", 1);
        PlayerPrefs.Save();
    }
    public void LoadPlayerPosition()
    {
        if (PlayerPrefs.GetInt("Saved") == 1)
        {
            float xPos = PlayerPrefs.GetFloat("X Position");
            float yPos = PlayerPrefs.GetFloat("Y Position");
            m_player.transform.position = new Vector3(xPos, yPos, m_player.transform.position.z);
        }
        else
        {
            m_player.transform.position = new Vector3(0, -2, 0);
            SavePlayerPosition();
        }
    }
}
