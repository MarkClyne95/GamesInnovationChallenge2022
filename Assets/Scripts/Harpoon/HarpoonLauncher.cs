using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace GIC.Harpoon{
    public class HarpoonLauncher : MonoBehaviour{
        [Tooltip("The distance that the harpoon can detect/hot trash from")] [SerializeField]
        private float harpoonRange = 10.0f;

        [Tooltip("Objects on this layer can be detected and fired at")] [SerializeField]
        private LayerMask harpoonableTrash;

        [Tooltip("The script which does the trajectory calculations for the harpoon")] [SerializeField]
        private PhysicsTrajectory physicsTrajectory;

        [FormerlySerializedAs("harpoonObject")] [Tooltip("The Harpoon which is lauched ")] [SerializeField]
        private Harpoon harpoon;

        [Tooltip("Draw a sphere to show the range of the harpoon launcher")]
        [SerializeField] private bool showRange;


        private void Update() {
            if (Input.GetKey(KeyCode.K)) {
                DrawTrajectory();
            }

            if (Input.GetKeyUp(KeyCode.K)) {
                LaunchHarpoon();
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                harpoon.ResetHarpoon();
            }
        }

        /// <summary>
        /// Drawn the trajectory of the harpoon to the closest trash
        /// </summary>
        private void DrawTrajectory() {
            var closestGarbage = FindClosestTrash();
            if (closestGarbage == null) {
                return;
            }
            physicsTrajectory.Target = closestGarbage.transform;
            var pathPoints = physicsTrajectory.GetPathPoints();
            if (pathPoints.Length > 0) {
                physicsTrajectory.RenderPath(pathPoints);
            }
        }

        private void LaunchHarpoon() {
            physicsTrajectory.ClearLine();
            var closestTrash = FindClosestTrash();
            //print("Closest Trash: " + closestTrash);
            if (closestTrash == null) { return; }
            ThrowHarpoon(closestTrash);
        }

        /// <summary>
        ///  Returns the closest trash to the Harpoon Launcher
        /// </summary>
        private GameObject FindClosestTrash() {
            var trashArray = Physics.OverlapSphere(transform.position, harpoonRange, harpoonableTrash.value);
            if (trashArray.Length == 0) {
                return null;
            }
            
            //--Sorts the array of collider by distance to the harpoon launcher (closest first)
            trashArray = trashArray.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).ToArray();
            return trashArray[0].gameObject;
        }

        /// <summary>
        /// Calculate the velocity the harpoon needs to reach the current position of the trash.
        /// Get the velocity of the trash. The two velocities will be added together and applied to the harpoon. This will account for the
        /// distance the trash traveled while the harpoon was in the air
        /// </summary>
        /// <param name="trash">The game object the harpoon is going to hit</param>
        private void ThrowHarpoon(GameObject trash) {
            physicsTrajectory.Target = trash.transform;
            if (!physicsTrajectory.TryCalculateLaunchData(out var launchData)) {
                //print("Throw Failed: Couldn't Calculate Launch Data");
                return;
            }
            //--Get the velocity of the target if it has a rigidbody
            var targetVelocity = Vector3.zero;
            var targetRb = trash.GetComponent<Rigidbody>();
            if (targetRb != null) {
                targetVelocity = targetRb.velocity;
            }
            var harpoonVelocity = launchData.initialVelocity;
            harpoon.Throw(harpoonVelocity, targetVelocity);
        }
        
        //Draws the harpoon launchers range
        private void OnDrawGizmosSelected() {
            if (showRange) {
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                Gizmos.DrawSphere(transform.position, harpoonRange);
            }
        }
    }
}