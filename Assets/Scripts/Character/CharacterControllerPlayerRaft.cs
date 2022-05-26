using GIC.River;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterControllerPlayerRaft : MonoBehaviour
{
    [Tooltip("The raft GameObject that is to be rotated")]
    [SerializeField] GameObject raftToRotate;

    [Tooltip("Whether or not the raft is currently rotating")]
    bool rotating = false;

    [Tooltip("Whether or not the raft is currently in motion")]
    bool moving = false;

    [Tooltip("The force to apply to the rigidbody to move it")]
    [SerializeField] float appliedForce = 0;

    [Tooltip("The Rigidbody of the raft")]
    [SerializeField] Rigidbody rb;

    [Tooltip("The pause menu GameObject")]
    [SerializeField] GameObject pauseMenu;

    [Tooltip("Audio Source for sound effects")]
    [SerializeField] AudioSource audioSrc;

    [Tooltip("Sound effect for button presses")]
    [SerializeField] AudioClip ButtonPress;

    
    #region Public Methods
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
    
    public void RestartGame(){
        audioSrc.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ReturnToMenu(){
        audioSrc.Stop();
        LoadTrigger.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Private Methods
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
    #endregion

    private void Update() {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector3 touchDelta = Input.GetTouch(0).deltaPosition;

            raftToRotate.transform.Rotate(0, touchDelta.x * 0.25f, 0);
            Debug.Log("Yes");
            
        }
    }
}
