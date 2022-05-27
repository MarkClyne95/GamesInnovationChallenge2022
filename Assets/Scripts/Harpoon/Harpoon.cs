using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GIC.Harpoon{
    public class Harpoon : MonoBehaviour{
        private Rigidbody harpoonRb;
        private bool isHarpoonFired = false;

        private Vector3 resetLocalPosition;
        private Quaternion resetLocalRotation;
        private Transform startParent;
        

        private void Awake() {
            harpoonRb = GetComponent<Rigidbody>();
            resetLocalPosition = transform.localPosition;
            resetLocalRotation = transform.localRotation;
            startParent = transform;
        }

        public void FireHarpoon(Vector3 velocity) {
            harpoonRb.velocity = Vector3.zero;
            harpoonRb.isKinematic = false;
            harpoonRb.AddForce(velocity, ForceMode.VelocityChange);
            harpoonRb.transform.parent = null;
            isHarpoonFired = true;
        }

        public void ResetHarpoon() {
            harpoonRb.velocity = Vector3.zero;
            harpoonRb.isKinematic = true;
            transform.position = resetLocalPosition;
            transform.rotation = resetLocalRotation;
            transform.parent = transform;
        }

        private void Update() {
            if (isHarpoonFired) {
                transform.rotation = Quaternion.LookRotation(harpoonRb.velocity.normalized);
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (isHarpoonFired) {
                isHarpoonFired = false;
                harpoonRb.isKinematic = true;
            }

            /*if (collision.gameObject.layer == harpoonableTrash.value) {
            print("Hit Trash");
        }*/
        }
    }
}