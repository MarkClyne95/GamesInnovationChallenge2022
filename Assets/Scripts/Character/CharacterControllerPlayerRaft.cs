using GIC.River;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterControllerPlayerRaft : MonoBehaviour
{
    [SerializeField] GameObject raftToRotate;
    bool rotating = false;
    bool moving = false;
    [SerializeField] float appliedForce = 0;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] AudioSource audioSrc;
    [SerializeField] AudioClip ButtonPress;

    [SerializeField] GameObject mmCanvas;
    [SerializeField] GameObject upgradeCanvas;
    [SerializeField] GameObject creditsCanvas;

    [Flags]
    enum Boats
    {
        None = 0b_0000_0000,
        BASIC = 0b_0000_0001, 
        UPGRADE1 = 0b_0000_0010,
        UPGRADE2 = 0b_0000_0011,
        UPGRADE3 = 0b_0000_0100
    }

    //set the current boat to basic on first time playing
    private int currentBoat = (int)Boats.BASIC;

    public Int32 GetCurrentBoat()
    {
        return currentBoat;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RotateCW()
    {
        //rotate 10 degrees CW
        StartCoroutine(rotateRaft(raftToRotate, new Vector3(0, 10, 0), 0.25f));
    }

    public void RotateCCW()
    {
        //rotate 10 degrees CCW
        StartCoroutine(rotateRaft(raftToRotate, new Vector3(0, -10, 0), 0.25f));
    }

    public void MoveLeft()
    {
        rb.AddForce(raftToRotate.transform.right * -appliedForce);
        //StartCoroutine(moveRaft(raftToRotate, new Vector3(1, 0 , 0), 2f));
    }

    public void MoveRight()
    {
        rb.AddForce(raftToRotate.transform.right * appliedForce);
        //StartCoroutine(moveRaft(raftToRotate, new Vector3(-1, 0, 0), 2f));
    }

    public void MoveForward()
    {
        rb.AddForce(raftToRotate.transform.forward * appliedForce);
        //rb.AddForce(raftToRotate.transform.position + (Vector3.forward*100));
        //StartCoroutine(moveRaft(raftToRotate, new Vector3(0, 0, -1), 2f));
    }

    public void MoveBackward()
    {
        rb.AddForce(raftToRotate.transform.forward * -appliedForce);
        //rb.AddForce(raftToRotate.transform.position + (Vector3.back*100));
    }

    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
    }

    public void PauseGame()
    {
        if(pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
            
    }

    /// <summary>
    /// Rotate the raft by a given number of degrees over a specified duration
    /// </summary>
    /// <param name="raft"></param>
    /// <param name="angles"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator rotateRaft(GameObject raft, Vector3 angles, float duration)
    {
        //if already rotating, do nothing
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        //get desired rotation
        Vector3 eulerRot = raft.transform.eulerAngles + angles;

        //get current rotation
        Vector3 currentRot = raft.transform.eulerAngles;

        //timer
        float count = 0;

        //rotate over time, stopping when duration is met
        while (count < duration)
        {
            count += Time.deltaTime;
            raft.transform.eulerAngles = Vector3.Lerp(currentRot, eulerRot, count / duration);
            yield return null;
        }
        rotating = false;
    }

    IEnumerator moveRaft(GameObject raft, Vector3 direction, float duration)
    {
        //if already moving, do nothing
        if (moving)
        {
            yield break;
        }
        moving = true;

        Vector3 currentPos = raft.transform.position;

        Vector3 destination = raft.transform.position + direction;

        float count = 0;

        while (count < duration)
        {
            count += Time.deltaTime;
            rb.MovePosition(currentPos + direction);
            yield return new WaitForFixedUpdate();
        }

        if (count >= duration)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            yield return null;
        }
        moving = false;


    }


    #region MainMenu Controls
    public void StartGame()
    {
        SceneManager.LoadScene("RiverScene");
    }

    public void RestartGame(){
        audioSrc.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ReturnToMenu(){
        audioSrc.Stop();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

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

    public void BasicBoat()
    {
        currentBoat = (int)Boats.BASIC;
    }

    public void UpgradeBoat1()
    {
        currentBoat = (int)Boats.UPGRADE1;
    }

    public void UpgradeBoat2()
    {
        currentBoat = (int)Boats.UPGRADE2;
    }

    public void UpgradeBoat3()
    {
        currentBoat = (int)Boats.UPGRADE3;
    }
    #endregion
}
