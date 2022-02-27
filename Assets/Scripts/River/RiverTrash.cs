using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;

namespace GIC.River {
    [RequireComponent(typeof(Rigidbody))]
    public class RiverTrash : MonoBehaviour {

        private River river;
        private SplineProjector projector;
        private Rigidbody rb;
        private SplinePoint closestPoint;
        private void Awake() {
            river = FindObjectOfType<River>();
            rb = GetComponent<Rigidbody>();
            projector = GetComponent<SplineProjector>();
        }
        // Update is called once per frame
        void Update() {
            //closestPoint = river.GetClosestPointOnSpline(transform.position);
            //Debug.DrawLine(transform.position, closestPoint.position);
            //Debug.DrawLine(transform.position, transform.position + closestPoint.tangent2);
            //AddForceFromRiver(closestPoint);
            //AddForceFromRiver(projector.result);
            CalculateForceFromRiver();
        }

        void CalculateForceFromRiver() {
            Vector3 force = Vector3.zero;
            //--Force from river direction
            force += GetForceFromRiverDirection();
            
            //--Force away from edges
            force += GetForceAwayFromRiverEdge();

            //--Force away from edges
            force += GetForceAwayFromRiverCenter();

            //--Force away from river center
            rb.AddForce(force, ForceMode.Acceleration);

        }


        private Vector3 GetForceFromRiverDirection() {
            Vector3 force = projector.result.forward;
            force *= river.RiverStrength;
            //--Add Randomness
            //force = Quaternion.ro
            return force;
        }
        private Vector3 GetForceAwayFromRiverEdge() {
            Vector3 toCenter = projector.result.position - transform.position;
            //float dstToCenter = toCenter.magnitude;
            //float dstFromedge = projector.result.size - dstToCenter;
            float percentAwayFromCenter = toCenter.magnitude / (projector.result.size/2);
            if (percentAwayFromCenter > river.ApplyForceAwayFromEdge) {
                Debug.DrawLine(transform.position, transform.position + toCenter);
                return toCenter * river.ForceTowardsRiverCenter;
            }
            else
                return Vector3.zero;
        }
        private Vector3 GetForceAwayFromRiverCenter() {
            Vector3 fromCenter =  transform.position - projector.result.position;
            float percentAwayFromCenter = fromCenter.magnitude / (projector.result.size / 2);
            if (percentAwayFromCenter < river.ApplyForceAwayFromCenter) {
                Debug.DrawLine(transform.position, transform.position + fromCenter);
                return fromCenter * river.ForceAwayFromRiverCenter;
            }
            else
                return Vector3.zero;
        }

        private void AddForceFromRiver(SplineSample closestPoint) {
            if (rb.velocity.magnitude < 10) {
                rb.AddForce(closestPoint.forward * 10, ForceMode.Acceleration);
            }
        }
        private void AddForceFromRiver(SplinePoint closestPoint) {
            if (rb.velocity.magnitude < 10) {
                rb.AddForce(closestPoint.tangent.normalized * 10, ForceMode.Acceleration);
            }
            Debug.DrawLine(transform.position, transform.position + closestPoint.tangent);

        }
    }
}
