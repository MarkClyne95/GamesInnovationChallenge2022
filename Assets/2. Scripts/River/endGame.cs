using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGame : MonoBehaviour
{
    [SerializeField]GameObject winScreen;
    [SerializeField] SC_MasterSaveLoad saveLoadSystem;
    TrashCollection tc;

    private void Awake() { 
        tc = (TrashCollection)GameObject.FindObjectOfType(typeof(TrashCollection));
        saveLoadSystem = GameObject.FindGameObjectWithTag("SaveLoad").GetComponent<SC_MasterSaveLoad>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){
            saveLoadSystem.SaveGame();
            Time.timeScale = 0;
            winScreen.SetActive(true);
            PlayerPrefs.SetInt("Money", tc.GetTrashCollected());
            Debug.LogWarning(tc.GetTrashCollected());
        }
    }
}
