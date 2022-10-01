using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinIce.Animations.Controllers;
using ThinIce.Animations.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

// ReSharper disable Unity.NoNullPropagation

namespace ThinIce
{
    // [ExecuteAlways]
    [RequireComponent(typeof(DialogStateKeeper))]
    public class GameDialogWithLanguages : MonoBehaviour
    {
        [Header("Canvas Game Object")] public GameObject canvasDialog;

        public TMP_Text headerText;
        public ButtonDialog[] buttons;

        public string defaultNextButtonText = "...";

        [Header("Girl")] public GameObject girl;
        public Sprite[] emotions;

        [Header("Dialogs")] public List<LocalizedText> startGuyText = new List<LocalizedText>();
        public List<LocalizedText> totalFailedText = new List<LocalizedText>();
        public GirlEmotion totalFailedEmotion = GirlEmotion.Angry;

        public List<LanguageDialogCommon> localizedDialogCommons = new List<LanguageDialogCommon>();
        public uint dialogPage;
        public DialogState dialogState = DialogState.StartPhrase;

        private bool _dialogFailed;
        private Random _random;
        private DialogAnimationController _dialogAnimationController;

        private Image _girlImage;

        // Start is called before the first frame update
        private void Start()
        {
            _random = new Random();
            _girlImage = girl.GetComponent<Image>();
            RestartDialog();
        }

        private void OnEnable()
        {
            if (canvasDialog != null)
                canvasDialog.SetActive(true);

            _dialogAnimationController = FindObjectOfType<DialogAnimationController>(true);
            RenderDialog(dialogState, dialogPage);
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

            if (GameController.Instance == null)
                return;

            var dlg = localizedDialogCommons.First(x => x.Language == GameController.Instance.CurrentLanguage)
                .DialogCommons[(int) page];

            var i = 0;
            switch (dlgState)
            {
                case DialogState.Common:
                    headerText.text = dlg.textGirl;
                    _girlImage.sprite = emotions[Convert.ToInt32(dlg.girlEmotion)];

                    var tempGuys = new List<TextGuy>(dlg.textGuy);
                    tempGuys = tempGuys.OrderBy(x => _random.Next()).ToList();
                    foreach (var button in buttons)
                    {
                        button.gameObject.SetActive(true);
                        button.SetTextGuy(tempGuys[i++]);
                    }

                    break;

                case DialogState.Failed:
                    headerText.text = totalFailedText.First(x => x.Language == GameController.Instance.CurrentLanguage)
                        .Text;
                    _girlImage.sprite = emotions[Convert.ToInt32(totalFailedEmotion)];

                    foreach (var button in buttons)
                        button.gameObject.SetActive(false);

                    buttons[0].gameObject.SetActive(true);
                    buttons[0].SetTextButton(defaultNextButtonText);

                    break;

                case DialogState.StartPhrase:
                    headerText.text = defaultNextButtonText;
                    _girlImage.sprite = emotions[Convert.ToInt32(GirlEmotion.Hey)];

                    foreach (var button in buttons)
                        button.gameObject.SetActive(false);

                    buttons[0].gameObject.SetActive(true);
                    buttons[0].SetTextButton(startGuyText
                        .First(x => x.Language == GameController.Instance.CurrentLanguage).Text);

                    break;

                case DialogState.GirlAnswer:
                    _girlImage.sprite = emotions[Convert.ToInt32(textGuy!.girtAnswerEmotion)];

                    // Если диалог провален, после ответа девушки будет totalFailedText
                    _dialogFailed = textGuy.badText;

                    headerText.text = textGuy.girlAnswer;

                    foreach (var button in buttons)
                        button.gameObject.SetActive(false);

                    buttons[0].gameObject.SetActive(true);
                    buttons[0].SetTextButton(defaultNextButtonText);

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dlgState), dlgState, null);
            }
        }

        public async Task PressedButtonDialog(TextGuy textGuy)
        {
            switch (dialogState)
            {
                case DialogState.StartPhrase:
                    if (_dialogAnimationController != null)
                        await Task.Delay(_dialogAnimationController
                            .PlayTrigger(DialogAnimationStates.StartPhrasePressed).Seconds());
                    RenderDialog(DialogState.Common, 0);
                    break;

                case DialogState.Common:
                    _dialogAnimationController?.PlayTrigger(DialogAnimationStates.PressedVariantButton);
                    RenderDialog(DialogState.GirlAnswer, dialogPage, textGuy);
                    break;

                case DialogState.GirlAnswer:
                    if (_dialogFailed)
                    {
                        _dialogAnimationController?.PlayTrigger(DialogAnimationStates.PressedAnswerButton);
                        _dialogAnimationController?.PlayTrigger(DialogAnimationStates.PressedVariantButton);
                        RenderDialog(DialogState.Failed, dialogPage);
                    }
                    else
                    {
                        _dialogAnimationController?.PlayTrigger(DialogAnimationStates.PressedAnswerButton);
                        RenderDialog(DialogState.Common, dialogPage + 1);
                    }

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