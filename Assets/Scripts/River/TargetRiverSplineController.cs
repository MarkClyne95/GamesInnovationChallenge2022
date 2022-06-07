using System;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

namespace GIC.River{
    public class TargetRiverSplineController: MonoBehaviour{
        public static SplineComputer[] RiverSplines { get; set; } 

        private void Start() {
            RiverSplines = GameObject.FindGameObjectsWithTag("RiverSpline").Select(x=> x.GetComponent<SplineComputer>()).ToArray();
            foreach (var computer in RiverSplines) {
                if (computer.triggerGroups.Length==0) { continue;}

                SplineTrigger[] triggers = computer.triggerGroups[0].triggers;
                foreach (var trigger in triggers) {
                    trigger.onUserCross += OnCross;

                }
            }
        }

        public static void SetTrashToClosestRiver(SplineProjector projector) {
            var minDst = float.MaxValue;
            int closestRiverIndex = 0;
            for (var i = 0; i < RiverSplines.Length; i++) {
                SplinePoint closestSP = River.GetClosestPointOnSpline(RiverSplines[i],projector.transform.position);
                float currentDst = Vector3.Distance(closestSP.position, projector.transform.position);
                if (currentDst < minDst) {
                    minDst = currentDst;
                    closestRiverIndex = i;
                }
            }
            
            projector.spline = RiverSplines[closestRiverIndex];
        }

        public void OnCross(SplineUser splineUser) {
            print("CROSS");
            if (splineUser.TryGetComponent(out RiverSwitcher switcher)) {
                switcher.TriggerCrossed();
            }
        }
    }
}
