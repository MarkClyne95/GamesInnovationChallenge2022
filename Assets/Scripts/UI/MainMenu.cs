using System;
using UnityEngine;
public class MainMenu : MonoBehaviour {

    #region private properties
    [Tooltip("Main Menu Canvas")]
    [SerializeField] GameObject mmCanvas;

    [Tooltip("Upgrade Canvas")]
    [SerializeField] GameObject upgradeCanvas;

    [Tooltip("Credits Canvas")]
    [SerializeField] GameObject creditsCanvas;

    //set the current boat to basic on first time playing
    private int currentBoat = (int)Boats.BASIC;
    #endregion

    [Flags]
    enum Boats
    {
        None = 0b_0000_0000,
        BASIC = 0b_0000_0001, 
        UPGRADE1 = 0b_0000_0010,
        UPGRADE2 = 0b_0000_0011,
        UPGRADE3 = 0b_0000_0100
    }

    public Int32 GetCurrentBoat()
    {
        return currentBoat;
    }

    private void Start() {
        
    }

    private void Awake() {
        
    }

    private void Update() {
        
    }

    #region MainMenu Controls
    public void StartGame() => LoadTrigger.LoadScene("RiverScene");
        

    public void OpenUpgradeMenu()
    {
        mmCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);
    }

    public void CloseUpgradeMenu()
    {
        mmCanvas.SetActive(true);
        upgradeCanvas.SetActive(false);
    }
    public void OpenCreditsMenu()
    {
        mmCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void CloseCreditsMenu()
    {
        mmCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}