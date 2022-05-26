﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GIC.Harpoon{
    public class PhysicsTrajectory : MonoBehaviour{
        [SerializeField] private Rigidbody player;
        public Transform Target { get; set; }

        [SerializeField] float height = 5; // Height of ball ??
        [SerializeField] float gravity = -9.81f;
        [SerializeField] float maxHeight = 10f;
        [SerializeField] float minHeight = 1.5f;
        [SerializeField] float heightChangeSpeed = 10f; // how fast the height on player input

        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] GameObject leapLandObject;


        public LaunchData CalculateLaunchData() {
            //SetHeight();

            float displacementY = Target.position.y - player.position.y; // Calculate Py from diagram
            Vector3 displacementXZ =
                new Vector3(Target.position.x - player.position.x, 0,
                    Target.position.z - player.position.z); // Calculate Px but for XZ axis not just X 
            float time = Mathf.Sqrt(-2 * height / gravity) +
                         Mathf.Sqrt(2 * (displacementY - height) / gravity); // This is  Thorizontal  = Tup + Tdown
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2.0f * gravity * height); // This is the initial up Velocity

            Vector3 velocityXZ = displacementXZ / time; // this is  horizontal velocity Uh (the one that uses Tup + Tdown)
            return
                new LaunchData(velocityXZ + velocityY,
                    time); // velocityXZ has ) as Y value and velocityY has 0 as XZ values so this just combines them
        }

        private void SetHeight() {
            if (Input.GetButton("IncreaseHeight")) {
                height += heightChangeSpeed * Time.deltaTime;
            }

            if (Input.GetButton("DecreaseHeight")) {
                height -= heightChangeSpeed * Time.deltaTime;
            }

            height = Mathf.Clamp(height, minHeight, maxHeight);
        }

        //Returns an array containg the displacement of the ball at time intervals through flight
        public Vector3[] GetPathPoints() {
            LaunchData launchData = CalculateLaunchData();
            Vector3 previousDrawPoint = player.position;
            int resolution = 30; // how many times are we checking the path when drawing the line
            Vector3[] linePath = new Vector3[resolution + 1];

            for (int i = 0; i <= resolution; i++) {
                float simulationTime =
                    i / (float)resolution *
                    launchData.timeTotarget; // gives a variable going from 0 to the timeToTarget over the course of the for loop
                Vector3 displacement = launchData.initialVelocity * simulationTime +
                                       Vector3.up * gravity * simulationTime * simulationTime /
                                       2f; // using 3rd suvat equation  s = ut + at^2 / 2
                Vector3 drawPoint = player.position + displacement;
                Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
                previousDrawPoint = drawPoint;
                linePath[i] = drawPoint; // will miss first point i.e player position
            }

            return linePath;
        }

        public Vector3[] GetPathPointsNoLimit() {
            float maxCheckTime = 5f;
            float timeIntervalBetweenPoints = 0.01f;
            float time = 0;
            float playerBallRadius = leapLandObject.transform.lossyScale.x / 2;
            LaunchData launchData = CalculateLaunchData();
            Vector3 previousDrawPoint = player.position;
            //int resolution = 30;        // how many times are we checking the path when drawing the line
            List<Vector3> linePathList = new List<Vector3>();
            //Vector3[] linePath = new Vector3[resolution + 1];
            while (time < maxCheckTime) {
                Vector3 displacement =
                    launchData.initialVelocity * time +
                    Vector3.up * gravity * time * time / 2f; // using 3rd suvat equation  s = ut + at^2 / 2
                Vector3 drawPoint = player.position + displacement;
                Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
                previousDrawPoint = drawPoint;
                linePathList.Add(drawPoint);
                if (time > 0 && CheckBallCollision(drawPoint, playerBallRadius)) {
                    // if the ball will hit a wall/ ground then return the array early
                    return linePathList.ToArray();
                }

                time += timeIntervalBetweenPoints;
            }

            return linePathList.ToArray();
        }

        private bool CheckBallCollision(Vector3 drawPoint, float radius) {
            /*if (Physics.CheckSphere(drawPoint,radius, 1 << LayerMask.NameToLayer("Ground"))) {
            Debug.DrawLine(drawPoint, drawPoint + (Vector3.up * 5),Color.black, 10);
        }*/
            return Physics.CheckSphere(drawPoint, radius, 1 << LayerMask.NameToLayer("Ground"));
        }

        // Takes an array of points (from DrawPath and draws a line between each of the creating an arc)
        public void RenderPath(Vector3[] linePoints) {
            lineRenderer.positionCount = linePoints.Length;
            lineRenderer.SetPositions(linePoints);
            lineRenderer.SetColors(Color.black, Color.black);
            //lineRenderer.SetWidth(0.5f, 0.5f);
        }
    }
}
