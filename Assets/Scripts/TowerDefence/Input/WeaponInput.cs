using System.Linq;
using ThinIce.TowerDefence.Upgrades;
using UnityEngine;

// ReSharper disable Unity.NoNullPropagation

namespace ThinIce.TowerDefence.Input
{
    public class WeaponInput : InputControllerBase<Weapon>
    {
        [SerializeField] private PlaneInput planeInput;

        private Transform _startParent;

        protected override void OnTakeObject()
        {
            var tempTransform = HoldingObject.transform;
            _startParent = tempTransform.parent;
            HoldingObject.transform.SetParent(null);
            HoldingObject.gameObject.layer = 0;
        }

        protected override void OnDropObject()
        {
            var holdPos = HoldingObject.transform;

            if (TempObjects.Any() && !TempObjects.First().TryUpgrade(HoldingObject)) SetDefaultPosition(holdPos);

            HoldingObject.gameObject.layer = HoldingObject.startLayer;
        }

        private void SetDefaultPosition(Transform holdPos)
        {
            holdPos.SetParent(_startParent);
            holdPos.position = HoldingObject.startPosition;
        }

        protected override void OnDragObject()
        {
            var holdPos = HoldingObject.transform;
            holdPos.SetParent(null);
            holdPos.position = planeInput.currentPosition;
        }
    }
}