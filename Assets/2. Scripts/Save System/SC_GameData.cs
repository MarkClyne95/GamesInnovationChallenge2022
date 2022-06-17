using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SC_GameData
{
    //Public Properties
    public int overallScore = 0;

    //public methods
    public void AddOverallScore(int amount)
    {
        overallScore = amount;
    }


    public void ResetData()
    {
        overallScore = 0;
    }
}
