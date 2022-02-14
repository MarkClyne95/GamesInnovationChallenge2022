using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    Rigidbody rb;
    public Transform rayPoint;
    public float distance;
    public string layer;
    public GameObject obj;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Debug.DrawRay(rayPoint.position, rayPoint.forward, Color.red);
        if (Physics.Raycast(rayPoint.position, rayPoint.forward, out RaycastHit hit))
        {
            distance = hit.distance;
            obj = hit.transform.gameObject;
            layer = hit.collider.tag.ToString();

            switch (layer)
            {
                case "Water":
                    if (hit.distance < 0)
                    {
                        rb.AddForce(0, 2, 0);
                    }
                    else if (hit.distance > 0)
                    {
                        rb.AddForce(0, 0, 0);
                    }
                    break;
                case "Ground":
                    if (hit.distance < 5)
                    {
                        rb.AddForce(0, 10, 0);
                    }
                    else if (hit.distance > 5)
                    {
                        rb.AddForce(0, 0, 0);
                    }
                    break;
            }
        }
    }

}