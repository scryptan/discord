using TMPro;
using UnityEngine;

namespace ThinIce
{
    public class ReviewButton : MonoBehaviour
    {
        public ReviewType reviewType;
        [SerializeField]
        private TMP_InputField inputField;
        private Telegram _telegramClient;

        private void OnEnable()
        {
            _telegramClient = FindObjectOfType<Telegram>();
        }

        public void SendMessage()
        {
            _telegramClient.SendMessage(FormatMessage());
        }

        private string FormatMessage()
        {
            return $"Тип #{reviewType}\nСообщение:\n\n{inputField.text}";
        }
    }

    public enum ReviewType
    {
        Bug,
        Supply
    }
}