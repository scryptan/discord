using System;
using UnityEngine;

namespace ThinIce.TowerDefence.Targets
{
    [RequireComponent(typeof(BoxCollider))]
    public class TargetSpawnPoint: MonoBehaviour
    {
        public Action OnTargetExit;
        
        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent<Target>(out _))
                OnTargetExit?.Invoke();
        }
    }
}