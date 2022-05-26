using UnityEngine;

namespace GIC.Harpoon{
    public struct LaunchData{
        public readonly Vector3 initialVelocity;
        public readonly float timeTotarget;

        public LaunchData(Vector3 initialVelocity, float timeTotarget) {
            this.initialVelocity = initialVelocity;
            this.timeTotarget = timeTotarget;
        }
    }
}