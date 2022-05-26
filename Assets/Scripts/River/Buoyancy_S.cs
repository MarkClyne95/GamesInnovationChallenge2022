using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GIC.River {
    [RequireComponent(typeof(Rigidbody))]
    public class Buoyancy_S : MonoBehaviour {
        Rigidbody rb;                                                       // Boat
        [SerializeField] public float upForce = 500.0f;                            // Buoyancy force
        //[SerializeField] Transform waterTransform;                          // Water
        float waterLevelY;                                                  // Water level y component
        [SerializeField] Transform buoyancyTransform;                       // Force is added at this position   


        [SerializeField] float UP_CONST = 0.0f;
        List<GameObject> waterTransformList = new List<GameObject>();
        [SerializeField] List<GameObject> boatList = new List<GameObject> ();
        [SerializeField] CharacterControllerPlayerRaft raftControls;


        private void Start() {
            rb = GetComponent<Rigidbody>();

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Water"))
            {
                waterTransformList.Add(go);
            }
        }

        private void Awake()
        {
            
        }

        private void FixedUpdate() {
            foreach(GameObject go in waterTransformList)
            {
                waterLevelY = go.transform.position.y;
                if (transform.position.y < go.transform.position.y)
                {
                    float dst = Vector3.Distance(transform.position, new Vector3(transform.position.x, waterLevelY, transform.position.z));
                    rb.AddForceAtPosition(Vector3.up * dst * upForce, buoyancyTransform.position, ForceMode.Force);
                }
            }
            
        }





    } 
}
