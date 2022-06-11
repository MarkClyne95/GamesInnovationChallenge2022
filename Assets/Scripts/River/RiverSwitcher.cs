using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using GIC.River;
using UnityEngine;

namespace GIC.River{
    

    /// <summary>
    /// This Script is used to switch the target river of a RiverTrash.
    /// </summary>
    public class RiverSwitcher : MonoBehaviour{
        private SplineProjector splineProjector;
        private Coroutine trySwitchRiverRoutine;

        
        private void Start() {
            splineProjector = GetComponent<SplineProjector>();
        }
        

        public void TrySwitchSpline() {
            TargetRiverSplineController.SetTrashToClosestRiver(splineProjector);
        }

        public void TriggerCrossed() {
            if (trySwitchRiverRoutine != null) {
                StopCoroutine(trySwitchRiverRoutine);
                trySwitchRiverRoutine = null;
                return;
            }

            trySwitchRiverRoutine = StartCoroutine(TrySwitchRiverRoutine());
        }

        IEnumerator TrySwitchRiverRoutine() {
            while (transform) {
                TargetRiverSplineController.SetTrashToClosestRiver(splineProjector);
                yield return new WaitForSeconds(1);
            }
        }
    }

}