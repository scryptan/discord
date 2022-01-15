using ThinIce.Animations.Controllers;
using ThinIce.Animations.States;
using UnityEngine;

namespace ThinIce
{
    public class SettingsButton : MonoBehaviour
    {
        public GameObject mainMenuButtons;
        public GameObject inGameMenuButtons;

        private SettingsAnimationController m_SettingsAnimationController;
        private void OnEnable()
        {
            m_SettingsAnimationController = FindObjectOfType<SettingsAnimationController>(true);
        }

        public void OpenDialog(bool isMenu)
        {
            mainMenuButtons.SetActive(isMenu);
            inGameMenuButtons.SetActive(!isMenu);
            gameObject.SetActive(true);
            m_SettingsAnimationController.PlayTrigger(OnlyAppearingAnimationStates.Appear);
            gameObject.SetActive(true);
        }

        public void CloseDialog()
        {
            m_SettingsAnimationController.PlayTrigger(OnlyAppearingAnimationStates.Disappear);
        }

        public void SetDisabled()
        {
            gameObject.SetActive(false);
        }
    }
}