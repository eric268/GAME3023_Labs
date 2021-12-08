using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        PlayerEvents events = FindObjectOfType<PlayerEvents>();
        events.onEnterMainEvent.Invoke();
        SceneManager.LoadScene("Main Scene");
    }
    public void OnQuitButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
