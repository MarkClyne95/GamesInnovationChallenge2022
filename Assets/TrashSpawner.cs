using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    GameObject player;
    ScriptableInteractableObject_Base trash;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        IsPlayerClose();
    }

    void IsPlayerClose()
    {
        Vector3 posOffset = transform.position - player.transform.position;
        if(posOffset.sqrMagnitude < 5)
        {
        }
    }
}
