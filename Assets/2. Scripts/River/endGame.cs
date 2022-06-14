using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGame : MonoBehaviour
{
    [SerializeField]GameObject winScreen;
    TrashCollection tc;

    private void Awake() {
        tc = (TrashCollection)GameObject.FindObjectOfType(typeof(TrashCollection));
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){
            Time.timeScale = 0;
            winScreen.SetActive(true);
            PlayerPrefs.SetInt("Money", tc.GetTrashCollected());
            Debug.LogWarning(tc.GetTrashCollected());
        }
    }
}
