using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThinIce.TowerDefence.Input
{
    public abstract class InputControllerBase : MonoBehaviour
    {
    }
    public abstract class InputControllerBase<T> : InputControllerBase where T : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        private readonly RaycastHit[] _hits = new RaycastHit[10];

        protected List<T> TempObjects = new List<T>();
        protected T HoldingObject;


        private void Update()
        {
            PointerChanges();
            DragAndDropChanges();
        }

        private void PointerChanges()
        {
            var ray = Camera.main!.ScreenPointToRay(UnityEngine.Input.mousePosition);

            foreach (var tempObject in TempObjects)
                OnPointerExit(tempObject);

            TempObjects.Clear();

            if (Physics.RaycastNonAlloc(ray, _hits, 100, layerMask) > 0)
            {
                TempObjects = _hits
                    .Where(x => x.collider != null)
                    .Select(x => x.collider.GetComponent<T>())
                    .Where(x => x != null)
                    .ToList();
                
                foreach (var tempObject in TempObjects)
                    OnPointerEnter(tempObject);
            }
        }

        private void DragAndDropChanges()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0) && TempObjects.SingleOrDefault() != null)
            {
                HoldingObject = TempObjects.First();
                OnTakeObject();
            }

            if (UnityEngine.Input.GetMouseButtonUp(0) && HoldingObject != null)
            {
                OnDropObject();
                HoldingObject = null;
            }

            if (HoldingObject != null)
                OnDragObject();
        }

        protected virtual void OnPointerEnter(T enterObject)
        {
        }

        protected virtual void OnPointerExit(T exitObject)
        {
        }

        protected virtual void OnTakeObject()
        {
        }

        protected virtual void OnDropObject()
        {
        }

        protected virtual void OnDragObject()
        {
        }
    }
}