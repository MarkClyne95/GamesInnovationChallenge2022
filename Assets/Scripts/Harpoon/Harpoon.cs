using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GIC.Harpoon{
    public class Harpoon : MonoBehaviour{
        [Tooltip("The distance that the harpoon will fire")] [SerializeField]
        private float harpoonRange = 10.0f;

        private Rigidbody rb;
        [Tooltip ("Objects on this layer can be detected and fired at")]
        [SerializeField] private LayerMask harpoonableTrash;
        
        [SerializeField] private PhysicsTrajectory physicsTrajectory;
        private bool faceDirection = false;
        [SerializeField] private GameObject platform;


        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.K)) {
                FireHarpoon();
            }

            if (faceDirection) {
                transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
            }
        }

        private void FireHarpoon() {
            GameObject target = FindClosestGarbage();
            if (target == null) {
                return;
            }

            physicsTrajectory.Target = target.transform;
            Vector3 harpoonVelocity = physicsTrajectory.CalculateLaunchData().initialVelocity;
            rb.AddForce(harpoonVelocity, ForceMode.VelocityChange);
            faceDirection = true;
            platform.SetActive(false);
        }


        private GameObject FindClosestGarbage() {
            Collider[] trashArray = Physics.OverlapSphere(transform.position, harpoonRange, harpoonableTrash.value);
            if (trashArray.Length == 0) {
                return null;
            }

            trashArray = trashArray.OrderBy(t => Vector3.Distance(transform.position, t.gameObject.transform.position)).ToArray();
            return trashArray[0].gameObject;
        }

        private void OnDrawGizmosSelected() {
            //Gizmos.color = Color.black;
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawSphere(transform.position, harpoonRange);
        }

        private void OnCollisionEnter(Collision collision) {
            if (!platform.activeSelf) {
                faceDirection = false;
            }
        }
    }
    
    

}