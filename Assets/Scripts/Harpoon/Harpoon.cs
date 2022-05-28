using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GIC.Harpoon{
    public class Harpoon : MonoBehaviour{
        private Rigidbody harpoonRb;
        private bool isHarpoonInAir = false;

        //--Used to reset the harpoon  
        private Vector3 resetLocalPosition;
        private Quaternion resetLocalRotation;
        private Transform startParent;
        

        private void Awake() {
            harpoonRb = GetComponent<Rigidbody>();
            resetLocalPosition = transform.localPosition;
            resetLocalRotation = transform.localRotation;
            startParent = transform.parent;
        }

        /// <summary>
        /// T
        /// </summary>
        /// <param name="initialVelocity">The initial velocity required to reach the target</param>
        /// <param name="velocityOfTarget">This will be added onto the initial velocity so a moving target can be hit </param>
        public void Throw(Vector3 initialVelocity, Vector3 velocityOfTarget = new Vector3()) {
            harpoonRb.velocity = Vector3.zero;
            harpoonRb.isKinematic = false;
            harpoonRb.AddForce(initialVelocity + velocityOfTarget, ForceMode.VelocityChange);
            harpoonRb.transform.parent = null;
            isHarpoonInAir = true;
        }

        public void ResetHarpoon() {
            isHarpoonInAir = false;
            harpoonRb.velocity = Vector3.zero;
            harpoonRb.isKinematic = true;
            transform.parent = startParent;
            transform.localPosition = resetLocalPosition;
            transform.localRotation = resetLocalRotation;
        }

        private void Update() {
            if (isHarpoonInAir) {
                transform.rotation = Quaternion.LookRotation(harpoonRb.velocity.normalized);
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (!isHarpoonInAir) {
                return;
            }
            //print("Harpoon Collision: " + collision.gameObject.name);
            isHarpoonInAir = false;
            harpoonRb.isKinematic = true;
            if (collision.gameObject.layer == LayerMask.NameToLayer("HarpoonableTrash")) {
                transform.parent = collision.transform;
            }
            

            

            /*if (collision.gameObject.layer == harpoonableTrash.value) {
            print("Hit Trash");
        }*/
        }
    }
}