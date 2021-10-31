using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThinIce
{
    public class Toggle : MonoBehaviour, IPointerClickHandler
    {
        public bool isEnabled;
        public SettingsType settingsType;
        public Transform disabledPoint, enabledPoint, togglePoint;

        private void OnEnable()
        {
            PlayAnimation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            isEnabled = !isEnabled;
            PlayAnimation(1);
            switch (settingsType)
            {
                case SettingsType.Music:
                    GameSettings.MusicEnabled = isEnabled;
                    break;
                case SettingsType.Audio:
                    GameSettings.MusicEnabled = isEnabled;
                    break;
                case SettingsType.Vibration:
                    GameSettings.MusicEnabled = isEnabled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayAnimation(int duration = 0)
        {
            if (togglePoint != null)
                togglePoint.DOMove(isEnabled ? enabledPoint.position : disabledPoint.position, duration);
        }

        public void SetEnabled(bool value)
        {
            isEnabled = value;
        }
    }

    public enum SettingsType
    {
        Music,
        Audio,
        Vibration
    }
}