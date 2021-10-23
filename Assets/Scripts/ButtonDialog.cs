using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ThinIce
{
    public class ButtonDialog : MonoBehaviour, IPointerClickHandler
    {
        private Image _imageRender = null;

        public TypeButton typeButton = TypeButton.One;

        public Sprite spriteCommon = null;
        public Sprite spriteGood = null;
        public Sprite spriteBad = null;

        public TMP_Text buttonText;

        private TextGuy _currentTextGuy;
        private GameDialog _cachedGameDialog;

        private void OnEnable()
        {
            _imageRender = GetComponent<Image>();
        }

        public void SetTextGuy(TextGuy textGuy)
        {
            _cachedGameDialog ??= GameController.Instance.gameDialog.GetComponent<GameDialog>();
            _currentTextGuy = textGuy;
            SetTextButton(textGuy.text);
            SetSprite();
        }

        public void SetTextButton(string text)
        {
            buttonText.text = text;
            SetSprite();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _cachedGameDialog ??= GameController.Instance.gameDialog.GetComponent<GameDialog>();
            if (_currentTextGuy != null && buttonText.text != _cachedGameDialog.defaultNextButtonText)
                AnsweredTextGuys.Answer(_currentTextGuy);

            _currentTextGuy = null;

            _cachedGameDialog.PressedButtonDialog(typeButton);
        }

        private void SetSprite()
        {
            if (_imageRender != null)
                _imageRender.sprite = spriteCommon;
            if (GameController.Instance.seePreviousAnswers &&
                _currentTextGuy != null &&
                _currentTextGuy.text != _cachedGameDialog.defaultNextButtonText &&
                AnsweredTextGuys.IsAnswered(_currentTextGuy))
            {
                _imageRender.sprite = _currentTextGuy.badText ? spriteBad : spriteGood;
            }
        }
    }
}