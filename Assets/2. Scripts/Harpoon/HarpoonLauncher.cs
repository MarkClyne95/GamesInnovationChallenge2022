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
        
        [Header("Throw Power")]
        [Tooltip("The initial throw power when the player begins holding the throw button (throw power of one will hit the target)")]
        [SerializeField] private float minThrowPower = 0.1f;
        [Tooltip("The max throw power when the player is holding the throw button (throw power of one will hit the target)")]
        [SerializeField] private float maxThrowPower = 1.5f;
        [Tooltip("The time it takes to go from min to max throw power")]
        [SerializeField] private float powerBuildTime = 2;
        private float throwPower = 1;
        private Coroutine launchBuildUpRoutine;


        private void Update() {
            if (Input.GetKeyDown(KeyCode.K)) {
                //DrawTrajectory();
                StartLaunchBuildUp();
            }

            if (Input.GetKeyUp(KeyCode.K)) {
                LaunchHarpoon();
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                harpoon.ResetHarpoon();
            }
        }

        public void StartLaunchBuildUp() {
            if (launchBuildUpRoutine != null) {
                StopCoroutine(launchBuildUpRoutine);
            }
            launchBuildUpRoutine = StartCoroutine(LaunchBuildUpRoutine());
        }

        //--Increase the throw power over time so that the trajectory line changes as the power builds up
        private IEnumerator LaunchBuildUpRoutine() {
            throwPower = minThrowPower;
            float timeWhenRoutineStarts = Time.realtimeSinceStartup;
            while (true) {              //Coroutine stopped when harpoon is launched
                float timeSinceRoutineStarted = Time.realtimeSinceStartup - timeWhenRoutineStarts;
                throwPower = Mathf.Lerp(minThrowPower, maxThrowPower, (timeSinceRoutineStarted / powerBuildTime));
                DrawTrajectory();
                yield return new WaitForFixedUpdate();
            }
        }

        /// <summary>
        /// Drawn the trajectory of the harpoon to the closest trash
        /// </summary>
        public void DrawTrajectory() {
            var closestGarbage = FindClosestTrash();
            if (closestGarbage == null) {
                return;
            }
            physicsTrajectory.Target = closestGarbage.transform;
            if (!physicsTrajectory.TryCalculateLaunchData(out var launchData)) {
                return;
            }

            var pathPoints = physicsTrajectory.GetPathPoints(launchData * throwPower);
            if (pathPoints.Length > 0) {
                physicsTrajectory.RenderPath(pathPoints);
            }
        }

        public void LaunchHarpoon() {
            if (launchBuildUpRoutine!=null) {
                StopCoroutine(launchBuildUpRoutine);
            }
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
            harpoon.Throw((harpoonVelocity + targetVelocity) * throwPower);     //--Add the velocity of the target to account for the trash moving
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