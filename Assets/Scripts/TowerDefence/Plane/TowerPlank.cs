using ThinIce.TowerDefence.Towers;
using UnityEngine;

namespace ThinIce.TowerDefence.Plane
{
    [RequireComponent(typeof(MeshRenderer))]
    public class TowerPlank: MonoBehaviour, IAttachable
    {
        [SerializeField] private Material empty;
        [SerializeField] private Material selected;
        [SerializeField] private Material set;
        [SerializeField] private Tower holdingTower;

        public bool IsEmpty => holdingTower == null;


        private MeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public void OnPointerEnter()
        {
            _renderer.material = selected;
        }

        public void OnPointerExit()
        {
            _renderer.material = IsEmpty ? empty : set;
        }

        public void HoldTower(Tower tower)
        {
            holdingTower = tower;
            OnPointerExit();
        }
    }
}