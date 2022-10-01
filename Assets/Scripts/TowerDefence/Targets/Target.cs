using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThinIce.TowerDefence.Targets
{
    public class Target : MonoBehaviour, IHittable
    {
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float rotateSpeed = 5;
        [SerializeField] private float magnitudeEpsilon = 0.03f;
        [SerializeField] private List<Transform> wayPoints;

        public int Health = 100;
        public Transform Transform => transform;

        public void Init(List<Transform> localWayPoints)
        {
            Health = 100;
            wayPoints = new List<Transform>(localWayPoints);
        }
        
        private void Update()
        {
            FollowWayPoints();
        }

        private void FollowWayPoints()
        {
            if (wayPoints == null || wayPoints.Count == 0)
                return;

            MoveToPoint(wayPoints.First().position);
        }

        private void MoveToPoint(Vector3 endpoint)
        {
            var lookRotation = Quaternion.LookRotation((endpoint - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

            transform.Translate(GetNormalizedDirection() * (moveSpeed * Time.deltaTime), Space.World);


            if (Mathf.Abs(GetDirection().magnitude) <= magnitudeEpsilon)
                wayPoints.Remove(wayPoints.First());

            Vector3 GetNormalizedDirection() => GetDirection().normalized;
            Vector3 GetDirection() => endpoint - transform.position;
        }
    }
}