using UnityEngine;

namespace ThinIce
{
    public class SettingsButton : MonoBehaviour
    {
        public GameObject mainMenuButtons;
        public GameObject inGameMenuButtons;

        public void OpenDialog(bool isMenu)
        {
            mainMenuButtons.SetActive(isMenu);
            inGameMenuButtons.SetActive(!isMenu);
            gameObject.SetActive(true);
        }

        public void CloseDialog()
        {
            gameObject.SetActive(false);
        }
    }
}