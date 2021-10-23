﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace ThinIce
{
    [ExecuteAlways]
    public class GameDialog : MonoBehaviour
    {
        [Header("Canvas Game Object")] public GameObject canvasDialog = null;

        public TMP_Text headerText = null;
        public GameObject[] buttons;

        public string defaultNextButtonText = "Next";

        [Header("Girl")] public GameObject girl = null;
        public Sprite[] emotions;

        [Header("Dialogs")] public string startGuyText = "";
        public string totalFailedText = "";
        public GirlEmotion totalFailedEmotion = GirlEmotion.Angry;

        public DialogCommon[] dialogCommon;
        public uint dialogPage = 0;
        public DialogState dialogState = DialogState.StartPhrase;

        private bool _dialogFailed = false;


        // Start is called before the first frame update
        private void Start()
        {
            _dialogFailed = false;
            RenderDialog(dialogState, dialogPage);
        }

        private void OnEnable()
        {
            if (canvasDialog != null)
                canvasDialog.SetActive(true);
        }

        private void OnDisable()
        {
            if (canvasDialog != null)
                canvasDialog.SetActive(false);
        }

        private void RenderDialog(DialogState dlgState, uint page, TypeButton typeButton = TypeButton.One)
        {
            dialogState = dlgState;
            dialogPage = page;

            var dlg = dialogCommon[page];
            var girlImage = girl.GetComponent<Image>();

            var i = 0;
            switch (dlgState)
            {
                case DialogState.Common:
                    headerText.text = dlg.textGirl;
                    girlImage.sprite = emotions[Convert.ToInt32(dlg.girlEmotion)];

                    foreach (var button in buttons)
                    {
                        button.SetActive(true);
                        button.GetComponent<ButtonDialog>().SetTextGuy(dlg.textGuy[i++]);
                    }

                    break;

                case DialogState.Failed:
                    headerText.text = totalFailedText;
                    girlImage.sprite = emotions[Convert.ToInt32(totalFailedEmotion)];

                    buttons[0].SetActive(true);
                    buttons[0].GetComponent<ButtonDialog>().SetTextButton(defaultNextButtonText);

                    for (i = 1; i < buttons.Length; ++i)
                    {
                        buttons[i].SetActive(false);
                    }

                    break;

                case DialogState.StartPhrase:
                    headerText.text = startGuyText;
                    girlImage.sprite = emotions[Convert.ToInt32(GirlEmotion.Hey)];

                    buttons[0].SetActive(true);
                    buttons[0].GetComponent<ButtonDialog>().SetTextButton(defaultNextButtonText);

                    for (i = 1; i < buttons.Length; ++i)
                    {
                        buttons[i].SetActive(false);
                    }

                    break;

                case DialogState.GirlAnswer:
                    var textGuy = dlg.textGuy[Convert.ToInt32(typeButton) - 1];
                    girlImage.sprite = emotions[Convert.ToInt32(textGuy.girtAnswerEmotion)];

                    // Если диалог провален, после ответа девушки будет totalFailedText
                    _dialogFailed = textGuy.badText;

                    headerText.text = textGuy.girlAnswer;

                    buttons[0].SetActive(true);
                    buttons[0].GetComponent<ButtonDialog>().SetTextButton(defaultNextButtonText);

                    for (i = 1; i < buttons.Length; ++i)
                    {
                        buttons[i].SetActive(false);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dlgState), dlgState, null);
            }
        }

        public void PressedButtonDialog(TypeButton typeButton)
        {
            switch (dialogState)
            {
                case DialogState.StartPhrase:
                    RenderDialog(DialogState.Common, 0);
                    break;

                case DialogState.Common:
                    RenderDialog(DialogState.GirlAnswer, dialogPage, typeButton);
                    break;

                case DialogState.GirlAnswer:
                    if (_dialogFailed)
                        RenderDialog(DialogState.Failed, dialogPage);
                    else
                        RenderDialog(DialogState.Common, dialogPage + 1);
                    break;

                case DialogState.Failed:
                    GameController.Instance.GameStart();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum DialogState
        {
            StartPhrase = 0,
            Common = 1,
            GirlAnswer = 2,
            Failed = 3,
        }
    }
}