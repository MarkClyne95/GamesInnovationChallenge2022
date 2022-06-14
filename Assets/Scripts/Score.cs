using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    int overallScore = 0;
    [SerializeField] TMPro.TMP_Text Scoreboard;

    public int OverallScore
    {
        get { return overallScore; }
        set { overallScore = value; }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<TMPro.TMP_Text>();
        Scoreboard.text = ("Money: $ " + overallScore);
    }
    public void SetScoreInUI()
    {
        Scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<TMPro.TMP_Text>();
        Scoreboard.text = ("Money: $ " + overallScore);
    }
}
