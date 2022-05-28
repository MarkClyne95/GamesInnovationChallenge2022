using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GIC.Harpoon{
    public class Harpoon : MonoBehaviour{
        private Rigidbody harpoonRb;
        private bool isHarpoonInAir = false;

        private Vector3 resetLocalPosition;
        private Quaternion resetLocalRotation;
        private Transform startParent;
        

        private void Awake() {
            harpoonRb = GetComponent<Rigidbody>();
            resetLocalPosition = transform.localPosition;
            resetLocalRotation = transform.localRotation;
            startParent = transform.parent;
        }

        public void FireHarpoon(Vector3 velocity, Vector3 targetVelocity = new Vector3()) {
            harpoonRb.velocity = Vector3.zero;
            harpoonRb.isKinematic = false;
            harpoonRb.AddForce(velocity + targetVelocity, ForceMode.VelocityChange);
            harpoonRb.transform.parent = null;
            isHarpoonInAir = true;
        }

        public void ResetHarpoon() {
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
            print("Harpoon Collision: " + collision.gameObject.name);
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