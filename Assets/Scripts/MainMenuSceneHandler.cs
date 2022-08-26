using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneHandler : MonoBehaviour
{

    public void QuitApps() {
        Application.Quit();
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

}
