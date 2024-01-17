using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    void Start()
    {
        Config.CreateScoreFile();
    }
    public void LoadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void ResetGameSettings()
    {
        GameSettings.Instance.ResetGameSettings();
    }
}
