using ThinIce.Helpers;
using TMPro;
using UnityEngine;

namespace ThinIce.Novell
{
    public class ReviewButton : MonoBehaviour
    {
        public ReviewType reviewType;
        [SerializeField]
        private TMP_InputField inputField;
        [SerializeField]
        private GameObject bugTitle;
        [SerializeField]
        private GameObject supplyTitle;
        private Telegram _telegramClient;

        private void OnEnable()
        {
            _telegramClient = FindObjectOfType<Telegram>(true);
        }

        public void SendMessage()
        {
            _telegramClient.SendMessage(FormatMessage());
        }

        private string FormatMessage()
        {
            return $"Тип #{reviewType}\nСообщение:\n\n{inputField.text}";
        }

        public void SetType(bool isBug)
        {
            reviewType = isBug ? ReviewType.Bug : ReviewType.Supply;
            bugTitle.SetActive(bugTitle);
            supplyTitle.SetActive(!bugTitle);
        }
    }

    public enum ReviewType
    {
        Bug,
        Supply
    }
}