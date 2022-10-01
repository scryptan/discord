using ThinIce.TowerDefence.Towers;
using UnityEngine;

// ReSharper disable Unity.NoNullPropagation

namespace ThinIce.TowerDefence.Input
{
    public class TowerInput : InputControllerBase<Tower>
    {
        [SerializeField] private PlateInput plateInput;
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
            var isModified = false;

            if (plateInput.holdingPlate != null)
            {
                holdPos.SetParent(plateInput.holdingPlate.transform);
                
                plateInput.holdingPlate.HoldTower(HoldingObject);
                HoldingObject.holdingPlank = plateInput.holdingPlate;
                
                holdPos.position = plateInput.holdingPlate.transform.position;
                
                isModified = true;
            }

            if (!isModified)
            {
                SetDefaultPosition(holdPos);
            }

            HoldingObject.gameObject.layer = HoldingObject.startLayer;
        }

        private void SetDefaultPosition(Transform holdPos)
        {
            holdPos.SetParent(_startParent);
            holdPos.position = HoldingObject.startPosition;
            HoldingObject.holdingPlank?.HoldTower(null);
            HoldingObject.holdingPlank = null;
        }

        protected override void OnDragObject()
        {
            var holdPos = HoldingObject.transform;
            holdPos.SetParent(null);
            holdPos.position = planeInput.currentPosition;
            HoldingObject.holdingPlank?.HoldTower(null);
            HoldingObject.holdingPlank = null;
        }
    }
}