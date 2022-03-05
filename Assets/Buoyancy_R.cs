using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GIC.River
{
    [RequireComponent(typeof(Rigidbody))]
    public class Buoyancy_R : MonoBehaviour
    {
        Rigidbody rb;                                                       // Boat
        [SerializeField] public float upForce = 500.0f;                            // Buoyancy force
        [SerializeField] Transform waterTransform;                          // Water
        float waterLevelY;                                                  // Water level y component
        [SerializeField] Transform buoyancyTransform;                       // Force is added at this position   

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            waterLevelY = waterTransform.position.y;
            if (transform.position.y < waterTransform.position.y)
            {
                float dst = Vector3.Distance(transform.position, new Vector3(transform.position.x, waterLevelY, transform.position.z));
                rb.AddForceAtPosition(Vector3.up * dst * upForce, buoyancyTransform.position, ForceMode.Acceleration);
            }
        }





    }
}
