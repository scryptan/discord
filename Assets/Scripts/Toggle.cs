using System;
using UnityEngine;

namespace ThinIce
{
    public class Toggle : MonoBehaviour
    {
        public bool isEnabled;
        public SettingsType settingsType;

        public void SetEnabled(bool value)
        {
            isEnabled = value;
            switch (settingsType)
            {
                case SettingsType.Music:
                    GameSettings.MusicEnabled = isEnabled;
                    break;
                case SettingsType.Audio:
                    GameSettings.AudioEnabled = isEnabled;
                    break;
                case SettingsType.Vibration:
                    GameSettings.VibrationEnabled = isEnabled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum SettingsType
    {
        Music,
        Audio,
        Vibration
    }
}