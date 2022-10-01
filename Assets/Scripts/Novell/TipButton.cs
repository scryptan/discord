using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace ThinIce.Novell
{
    public class TipButton : MonoBehaviour
    {
        private Random _random;

        private void Awake()
        {
            _random = new Random();
        }

        public void ActivateRandomButton()
        {
            var button = GameController.Instance.gameDialog.buttons.Where(x => !x.isAnswered).OrderBy(x => _random.Next()).FirstOrDefault();
            button?.Answer();
            GameController.Instance.GameDialog();
        }

        public void CloseModal()
        {
            GameController.Instance.GameDialog();
        }

        public void OpenModal()
        {
            GameController.Instance.TipDialog();
        }
    }
}