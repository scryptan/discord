using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Random = System.Random;

namespace ThinIce
{
    [ExecuteAlways]
    public class GameDialog : MonoBehaviour
    {
        [Header("Canvas Game Object")] public GameObject canvasDialog = null;

        public TMP_Text headerText = null;
        public ButtonDialog[] buttons;

        public string defaultNextButtonText = "...";

        [Header("Girl")] public GameObject girl = null;
        public Sprite[] emotions;

        [Header("Dialogs")] public string startGuyText = "";
        public string totalFailedText = "";
        public GirlEmotion totalFailedEmotion = GirlEmotion.Angry;

        public DialogCommon[] dialogCommon;
        public uint dialogPage = 0;
        public DialogState dialogState = DialogState.StartPhrase;

        public Dictionary<uint, TextGuy> TextGuys => dialogCommon?.SelectMany(x => x.textGuy).ToDictionary(x => x.id);

        private bool _dialogFailed = false;
        private Random _random;


        // Start is called before the first frame update
        private void Start()
        {
            _random = new Random();
            RestartDialog();
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

        public void RestartDialog()
        {
            dialogState = DialogState.StartPhrase;
            dialogPage = 0;
            _dialogFailed = false;
            RenderDialog(dialogState, dialogPage);
        }

        private void RenderDialog(DialogState dlgState, uint page, TextGuy textGuy = null)
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

                    var tempGuys = new List<TextGuy>(dlg.textGuy);
                    tempGuys = tempGuys.OrderBy(x => _random.Next()).ToList();
                    foreach (var button in buttons)
                    {
                        button.gameObject.SetActive(true);
                        button.SetTextGuy(tempGuys[i++]);
                    }

                    break;

                case DialogState.Failed:
                    headerText.text = totalFailedText;
                    girlImage.sprite = emotions[Convert.ToInt32(totalFailedEmotion)];

                    buttons[0].gameObject.SetActive(true);
                    buttons[0].SetTextButton(defaultNextButtonText);

                    for (i = 1; i < buttons.Length; ++i)
                    {
                        buttons[i].gameObject.SetActive(false);
                    }

                    break;

                case DialogState.StartPhrase:
                    headerText.text = defaultNextButtonText;
                    girlImage.sprite = emotions[Convert.ToInt32(GirlEmotion.Hey)];

                    buttons[0].gameObject.SetActive(true);
                    buttons[0].SetTextButton(startGuyText);

                    for (i = 1; i < buttons.Length; ++i)
                    {
                        buttons[i].gameObject.SetActive(false);
                    }

                    break;

                case DialogState.GirlAnswer:
                    girlImage.sprite = emotions[Convert.ToInt32(textGuy.girtAnswerEmotion)];

                    // Если диалог провален, после ответа девушки будет totalFailedText
                    _dialogFailed = textGuy.badText;

                    headerText.text = textGuy.girlAnswer;

                    buttons[0].gameObject.SetActive(true);
                    buttons[0].SetTextButton(defaultNextButtonText);

                    for (i = 1; i < buttons.Length; ++i)
                    {
                        buttons[i].gameObject.SetActive(false);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dlgState), dlgState, null);
            }
        }

        public void PressedButtonDialog(TextGuy textGuy)
        {
            switch (dialogState)
            {
                case DialogState.StartPhrase:
                    RenderDialog(DialogState.Common, 0);
                    break;

                case DialogState.Common:
                    RenderDialog(DialogState.GirlAnswer, dialogPage, textGuy);
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