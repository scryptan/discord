using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ThinIce.Novell
{
    public class UiLocalizedItem : MonoBehaviour
    {
        public string Key;
        public List<LocalizedText> LocalizedTexts = new List<LocalizedText>();
        private TMP_Text _text;

        private void OnEnable()
        {
            _text = GetComponent<TMP_Text>() ?? GetComponentInChildren<TMP_Text>();
        }

        public void LanguageChanged(Language language)
        {
            _text ??= GetComponent<TMP_Text>() ?? GetComponentInChildren<TMP_Text>();
            _text?.SetText(LocalizedTexts.First(x => x.Language == language).Text);
        }
    }

    [Serializable]
    public class LocalizedText
    {
        public string Text;
        public Language Language;
    }
}