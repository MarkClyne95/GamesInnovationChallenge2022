using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrashFall : MonoBehaviour
{
    [SerializeField] Transform Rubbish;
    [SerializeField] Transform SpawnRubbish;
    [SerializeField] Transform DestroyRubbish;
    [SerializeField] int Speed = 5;
    [SerializeField] int rotationSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        //Rubbish.gameObject.transform.position = SpawnRubbish.transform.position;
        Rubbish.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Fall();
    }

    void Fall()
    {
        transform.position = Vector3.MoveTowards(Rubbish.position, DestroyRubbish.position, Speed * Time.deltaTime);
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("KillTrash"))
        {
            this.gameObject.SetActive(false);
            Rubbish.gameObject.transform.position = SpawnRubbish.transform.position;
            Rubbish.gameObject.SetActive(true);
        }
    }
}
