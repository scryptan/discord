using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThinIce
{
    public class Toggle : MonoBehaviour, IPointerClickHandler
    {
        public bool isEnabled;
        public Transform disabledPoint, enabledPoint, togglePoint;

        private void Awake()
        {
            PlayAnimation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            isEnabled = !isEnabled;
            PlayAnimation(1);
        }

        private void PlayAnimation(int duration = 0)
        {
            togglePoint.DOMove(isEnabled ? enabledPoint.position : disabledPoint.position, duration);
        }
    }
}