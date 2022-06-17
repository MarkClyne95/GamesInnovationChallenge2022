using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField]int overallScore = 0;
    [SerializeField] TMPro.TMP_Text Scoreboard;

    public static Score instance;

    public int OverallScore
    {
        get { return overallScore; }
        set { overallScore = value; }
    }
    private void Awake()
    {
        #region Singleton

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        #endregion

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
