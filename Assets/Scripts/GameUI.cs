using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnSaveButtonPressed()
    {
        PlayerPrefs.SetFloat("X Position", player.transform.position.x);
        PlayerPrefs.SetFloat("Y Position", player.transform.position.y);
        PlayerPrefs.SetInt("Saved", 1);
        PlayerPrefs.Save();
    }
}
