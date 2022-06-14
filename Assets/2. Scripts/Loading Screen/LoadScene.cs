using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    //load the scene
    void Start() => StartCoroutine(LoadSceneAsync());

    IEnumerator LoadSceneAsync(){
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync(LoadingManager.sceneToLoad);
    }
}
