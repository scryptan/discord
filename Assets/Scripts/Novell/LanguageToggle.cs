using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThinIce.Novell
{
    public class LanguageToggle : MonoBehaviour, IPointerClickHandler
    {
        public bool isEnabled;
        public Transform disabledPoint, enabledPoint, togglePoint;
        private void OnEnable()
        {
            isEnabled = GameController.Instance.CurrentLanguage == Language.English;
            PlayAnimation();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            isEnabled = !isEnabled;
            PlayAnimation(1);
            GameController.Instance.SetLanguage(isEnabled ? Language.English : Language.Russian);
        }
        
        private void PlayAnimation(int duration = 0)
        {
            if (togglePoint != null)
                togglePoint.DOMove(isEnabled ? enabledPoint.position : disabledPoint.position, duration);
        }
    }
}