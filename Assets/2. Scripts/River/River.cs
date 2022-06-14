using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

namespace GIC.River {
    public class River : MonoBehaviour {
        private SplineComputer computer;
        [SerializeField] private float riverStrength = 5.0f;
        [SerializeField] private float forceTowardsRiverCenter = 0.25f;
        [SerializeField] private float forceAwayFromRiverCenter = 0.25f;

        [Range(0, 1)]
        [SerializeField] float applyForceAwayFromEdge = 0.25f;
        [Range(0, 1)]
        [SerializeField] float applyForceAwayFromCenter = 0.25f;

        public float ForceTowardsRiverCenter {
            get { return forceTowardsRiverCenter; }
            set { forceTowardsRiverCenter = value; }
        }
        public float ForceAwayFromRiverCenter {
            get { return forceAwayFromRiverCenter; }
            set { forceAwayFromRiverCenter = value; }
        }
        public float ApplyForceAwayFromEdge {
            get { return applyForceAwayFromEdge; }
            set { applyForceAwayFromEdge = value; }
        }
        public float ApplyForceAwayFromCenter {
            get { return applyForceAwayFromCenter; }
            set { applyForceAwayFromCenter = value; }
        }


        //--Preperties
        public float RiverStrength {
            get { return riverStrength; }
            
        }

        private void Awake() {
            computer = GetComponentInChildren<SplineComputer>();
        }
        public SplinePoint GetClosestPointOnSpline(Vector3 position) {
            SplinePoint[] points = computer.GetPoints();
            SplinePoint closestSplinePoint = points[0];
            float closestDistance = float.MaxValue;
            foreach (var p in points) {
                float newDst = Vector3.Distance(p.position, position);
                if (newDst < closestDistance) {
                    closestDistance = newDst;
                    closestSplinePoint = p;
                }
            }
            return closestSplinePoint;
        }
    } 
}
