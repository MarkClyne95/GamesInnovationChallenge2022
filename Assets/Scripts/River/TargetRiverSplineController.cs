using System;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GIC.River{
    public class TargetRiverSplineController : MonoBehaviour{
        public static SplineComputer[] RiverSplines { get; set; }

        private void Awake() {
            RiverSplines = GameObject.FindGameObjectsWithTag("RiverSpline").Select(x => x.GetComponent<SplineComputer>()).ToArray();
            //--Same as line above
            /*GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("RiverSpline");
            RiverSplines = new SplineComputer[gameObjects.Length];
            for (var i = 0; i < gameObjects.Length; i++) {
                RiverSplines[i] =  gameObjects[i].GetComponent<SplineComputer>();
            }*/
        }

        //--Subscribe to Dreamteck Splines Trigger
        private void OnEnable() {
            SubscribeToSplineTriggers(true);
        }

        //--Unsubscribe to Dreamteck Splines Trigger
        private void OnDisable() {
            SubscribeToSplineTriggers(false);
        }

        //--Finds all triggers on the all river splines and subscribes/unsubscribes the OnCross method to them
        void SubscribeToSplineTriggers(bool shouldSubscribe) {
            foreach (var computer in RiverSplines) {
                if (computer.triggerGroups.Length == 0) {
                    continue;
                }
                SplineTrigger[] triggers = computer.triggerGroups[0].triggers;
                foreach (var trigger in triggers) {
                    if (shouldSubscribe) {
                        trigger.onUserCross += OnCross;
                    }
                    else {
                        trigger.onUserCross -= OnCross;
                    }
                }
            }
        }


        //--Sets the spline that the projector is moving along to the the spline with the closest point to the projector
        public static void SetTrashToClosestRiver(SplineProjector projector) {
            var minDst = float.MaxValue;
            int closestRiverIndex = 0;
            for (var i = 0; i < RiverSplines.Length; i++) {
                SplinePoint closestSP = River.GetClosestPointOnSpline(RiverSplines[i], projector.transform.position);
                float currentDst = Vector3.Distance(closestSP.position, projector.transform.position);
                if (currentDst < minDst) {
                    minDst = currentDst;
                    closestRiverIndex = i;
                }
            }
            projector.spline = RiverSplines[closestRiverIndex];
        }

        
        private void OnCross(SplineUser splineUser) {
            //print("CROSS");
            if (splineUser.TryGetComponent(out RiverSwitcher switcher)) {
                switcher.TriggerCrossed();
            }
        }
    }
}