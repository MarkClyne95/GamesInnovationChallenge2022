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
        /// <param name="velocity">The initial velocity required to reach the target</param>
        public void Throw(Vector3 velocity) {
            harpoonRb.velocity = Vector3.zero;
            harpoonRb.isKinematic = false;
            harpoonRb.AddForce(velocity, ForceMode.VelocityChange);
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
            if (isHarpoonInAir && harpoonRb.velocity.sqrMagnitude > 0) {
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
            if (collision.gameObject.layer == LayerMask.NameToLayer("HarpoonableTrash") ||collision.gameObject.layer == LayerMask.NameToLayer("Default")) {
                transform.parent = collision.transform;
                ResetHarpoon();
            }
            

            

            /*if (collision.gameObject.layer == harpoonableTrash.value) {
            print("Hit Trash");
        }*/
        }
    }
}