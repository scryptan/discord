using ThinIce.TowerDefence.Targets;
using UnityEngine;

namespace ThinIce.TowerDefence.Towers
{
    public class TowerTrigger : MonoBehaviour
    {
        [SerializeField] private Tower tower;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IHittable>(out var hittable))
            {
                tower.AddTarget(hittable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IHittable>(out var hittable))
            {
                tower.RemoveTarget(hittable);
            }
        }
    }
}