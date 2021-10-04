using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
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
