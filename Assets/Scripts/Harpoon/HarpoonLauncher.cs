using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GIC.Harpoon{
    public class HarpoonLauncher : MonoBehaviour{
        [Tooltip("The distance that the harpoon will fire")] [SerializeField]
        private float harpoonRange = 10.0f;

        private Rigidbody harpoonRb;
        [Tooltip ("Objects on this layer can be detected and fired at")]
        [SerializeField] private LayerMask harpoonableTrash;
        
        [Tooltip("The script wheich does the trajectory calculations for the harpoon")]
        [SerializeField] private PhysicsTrajectory physicsTrajectory;

        [Tooltip("The Harpoon GameObject ")]
        [SerializeField] private Harpoon harpoonObject;
        //private bool isHarpoonFired = false;
        //[SerializeField] private GameObject platform;

        private Vector3 resetLocalPosition;
        private Quaternion resetLocalRotation;
        private Transform startParent;

        private void Awake() {
            harpoonRb = GetComponentInChildren<Rigidbody>();
            resetLocalPosition = transform.localPosition;
            resetLocalRotation = transform.localRotation;
            startParent = transform;
        }

        private void Update() {
            //if (Input.GetKey(KeyCode.K)) {
                DrawTrajectory();
            //}
            if (Input.GetKeyUp(KeyCode.K)) {
                FireHarpoon();
            }
            if (Input.GetKeyDown(KeyCode.L)) {
                harpoonObject.ResetHarpoon();
            }

            /*if (isHarpoonFired) {
                harpoonRb.rotation = Quaternion.LookRotation(harpoonRb.velocity.normalized);
            }*/
        }

        private void DrawTrajectory() {
            GameObject closestGarbage = FindClosestGarbage();
            if (closestGarbage == null) {
                return;
            }
            physicsTrajectory.Target = closestGarbage.transform;
            physicsTrajectory.RenderPath(physicsTrajectory.GetPathPoints());
        }

        private void FireHarpoon() {
            physicsTrajectory.ClearLine();
            GameObject target = FindClosestGarbage();
            if (target == null) {
                return;
            }
            Fire(target);
            
        }


        private GameObject FindClosestGarbage() {
            Collider[] trashArray = Physics.OverlapSphere(transform.position, harpoonRange, harpoonableTrash.value);
            if (trashArray.Length == 0) {
                return null;
            }
            trashArray = trashArray.OrderBy(t => Vector3.Distance(transform.position, t.gameObject.transform.position)).ToArray();
            return trashArray[0].gameObject;
        }

        private void Fire(GameObject target) {
            physicsTrajectory.Target = target.transform;
            Vector3 harpoonVelocity = physicsTrajectory.CalculateLaunchData().initialVelocity;
            harpoonObject.FireHarpoon(harpoonVelocity);
        }
        private void OnDrawGizmosSelected() {
            //Gizmos.color = Color.black;
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawSphere(transform.position, harpoonRange);
        }

        /*private void ResetHarpoon() {
            harpoonRb.position = resetLocalPosition;
            harpoonRb.rotation = resetLocalRotation;
            harpoonRb.velocity = Vector3.zero;
            harpoonRb.isKinematic = true;
            harpoonRb.transform.parent = transform;
        }*/

      
    }
    
    

}