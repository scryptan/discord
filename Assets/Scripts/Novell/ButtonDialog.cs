using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ThinIce.Novell

{
    [RequireComponent(typeof(Image))]
    public class ButtonDialog : MonoBehaviour, IPointerClickHandler
    {
        private Image _imageRender = null;

        public Sprite spriteCommon = null;
        public Sprite spriteGood = null;
        public Sprite spriteBad = null;

        public TMP_Text buttonText;

        public bool isAnswered;

        private TextGuy _currentTextGuy;
        private GameDialogWithLanguages _cachedGameDialog;

        private void OnEnable()
        {
            _imageRender = GetComponent<Image>() ?? throw new ArgumentException("Image component doesn't exist");
        }

        public void SetTextGuy(TextGuy textGuy)
        {
            _cachedGameDialog ??= GameController.Instance.gameDialog;
            SetTextButton(textGuy.text, textGuy);
            SetSprite();
        }

        public void SetTextButton(string text, TextGuy textGuy = null)
        {
            _currentTextGuy = textGuy;
            buttonText.text = text.Trim();
            SetSprite();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _cachedGameDialog ??= GameController.Instance.gameDialog;
            if (_currentTextGuy != null && buttonText.text != _cachedGameDialog.defaultNextButtonText)
                AnsweredTextGuys.Answer(_currentTextGuy);

            var tempTextGuy = _currentTextGuy;
            _currentTextGuy = null;

            _cachedGameDialog.PressedButtonDialog(tempTextGuy);
        }

        private void SetSprite()
        {
            if (_imageRender != null)
                _imageRender.sprite = spriteCommon;
            
            if (GameController.Instance != null && 
                GameController.Instance.seePreviousAnswers &&
                _currentTextGuy != null &&
                _currentTextGuy.text != _cachedGameDialog.defaultNextButtonText &&
                AnsweredTextGuys.IsAnswered(_currentTextGuy))
            {
                Answer();
            }
        }

        public void Answer()
        {
            _imageRender.sprite = _currentTextGuy.badText ? spriteBad : spriteGood;
            isAnswered = true;
        }
    }
}