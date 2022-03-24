using Pinwheel.Poseidon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TrashCollection : MonoBehaviour
{
    List<GameObject> trash = new List<GameObject>();
    [SerializeField]Transform playerTransform;
    [SerializeField]Animator anim;
    [SerializeField]AudioSource audioSrc;
    public AudioClip collectSound;
    private int trashCollected;

    public TMPro.TMP_Text countText;
    int trashCount;

    //getter for trash collected to use in another class
    public int GetTrashCollected(){
        return trashCollected;
    }

    private void Awake()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Trash"))
        {
            trash.Add(go);
        }
    }

    private void Start()
    {
        trashCount = trash.Count;
        countText.text = trashCount.ToString();
    }

    private void FixedUpdate()
    {
        CleanUpTrash();
    }

    private void CleanUpTrash()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        try
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Trash"))
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    Vector3 distanceBetweenCharAndTrash = playerTransform.position - clickedObject.transform.position;
                    
                    if (clickedObject != null && distanceBetweenCharAndTrash.magnitude <= 10.0f)
                    {
                        audioSrc.PlayOneShot(collectSound);
                        anim.SetTrigger("PickingUp");
                        trash.Remove(clickedObject);
                        countText.text = trash.Count.ToString();
                        Destroy(clickedObject);
                        Debug.Log($"{clickedObject.name} hit");
                        trashCollected += 1;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}
