using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MasterSaveLoad : MonoBehaviour
{
    [SerializeField]SC_GameData SaveData = new SC_GameData();
    private SC_SaveSystem System;
    [SerializeField]Score Score;

    private void Awake()
    {
        System = gameObject.AddComponent<SC_SaveSystem>();
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame()
    {
        SaveData.AddOverallScore(Score.OverallScore);

        System.SaveGame(SaveData);

        Debug.Log(SaveData.overallScore.ToString());
    }

    public void LoadGame()
    {
        SaveData = System.LoadGame();
        //Debug.Log(SaveData.ToString());

        if (SaveData != null)
            Score.OverallScore = SaveData.overallScore;
        else
        {
            SaveData = new SC_GameData();
            SaveGame();
        }
            

    }

    public void ResetData()
    {
        SaveData.ResetData();
        System.SaveGame(SaveData);
        Score.SetScoreInUI();
    }
}
