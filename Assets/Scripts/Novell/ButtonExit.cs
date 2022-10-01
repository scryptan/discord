using UnityEngine;
using UnityEngine.EventSystems;

namespace ThinIce.Novell
{
    public class ButtonExit : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}