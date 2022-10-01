using UnityEngine;

namespace ThinIce.TowerDefence.Input
{
    public class PlaneInput : MonoBehaviour
    {
        public Vector3 currentPosition;

        [SerializeField] private LayerMask layerMask;
        private readonly RaycastHit[] _hits = new RaycastHit[1];
        
        private void Update()
        {
            var ray = Camera.main!.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.RaycastNonAlloc(ray, _hits, 100, layerMask) > 0)
            {
                currentPosition = _hits[0].point;
            }
        }
    }
}
