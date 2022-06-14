using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTrigger : MonoBehaviour
{
    public static void LoadScene(string targetScene)
    {
        LoadingManager.sceneToLoad = targetScene;
        SceneManager.LoadScene("LoadingScene");
    }
}
