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
        [Header("Canvas Game Object")] public GameObject canvasDialog = null;

        public TMP_Text headerText = null;
        public ButtonDialog[] buttons;

        public string defaultNextButtonText = "...";

        [Header("Girl")] public GameObject girl = null;
        public Sprite[] emotions;

        [Header("Dialogs")] public List<LocalizedText> startGuyText = new List<LocalizedText>();
        public List<LocalizedText> totalFailedText = new List<LocalizedText>();
        public GirlEmotion totalFailedEmotion = GirlEmotion.Angry;

        private DialogCommon[] _currentDialogCommon;
        public List<LanguageDialogCommon> localizedDialogCommons = new List<LanguageDialogCommon>();
        public uint dialogPage = 0;
        public DialogState dialogState = DialogState.StartPhrase;

        public Dictionary<uint, TextGuy> TextGuys => localizedDialogCommons
            .FirstOrDefault(x => x.Language == GameController.Instance.CurrentLanguage)?.DialogCommons
            .SelectMany(x => x.textGuy).ToDictionary(x => x.id);

        private bool _dialogFailed = false;
        private Random _random;
        private DialogAnimationController m_DialogAnimationController;


        // Start is called before the first frame update
        private void Start()
        {
            _random = new Random();
            SetCurrentLanguageDialogCommon();
            RestartDialog();
        }

        private void OnEnable()
        {
            if (canvasDialog != null)
                canvasDialog.SetActive(true);

            SetCurrentLanguageDialogCommon();
            m_DialogAnimationController = FindObjectOfType<DialogAnimationController>(true);
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

            var dlg = localizedDialogCommons?.FirstOrDefault(x => x.Language == GameController.Instance.CurrentLanguage)
                ?.DialogCommons[(int) page] ?? throw new ArgumentException("No common with this language or page");
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
                    headerText.text = totalFailedText.First(x => x.Language == GameController.Instance.CurrentLanguage)
                        .Text;
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
                    buttons[0].SetTextButton(startGuyText
                        .First(x => x.Language == GameController.Instance.CurrentLanguage).Text);

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

        public async Task PressedButtonDialog(TextGuy textGuy)
        {
            switch (dialogState)
            {
                case DialogState.StartPhrase:
                    if (m_DialogAnimationController != null)
                        await Task.Delay(m_DialogAnimationController
                            .PlayTrigger(DialogAnimationStates.StartPhrasePressed).Seconds());
                    RenderDialog(DialogState.Common, 0);
                    break;

                case DialogState.Common:
                    m_DialogAnimationController?.PlayTrigger(DialogAnimationStates.PressedVariantButton);
                    RenderDialog(DialogState.GirlAnswer, dialogPage, textGuy);
                    break;

                case DialogState.GirlAnswer:
                    if (_dialogFailed)
                    {
                        m_DialogAnimationController?.PlayTrigger(DialogAnimationStates.PressedAnswerButton);
                        m_DialogAnimationController?.PlayTrigger(DialogAnimationStates.PressedVariantButton);
                        RenderDialog(DialogState.Failed, dialogPage);
                    }
                    else
                    {
                        m_DialogAnimationController?.PlayTrigger(DialogAnimationStates.PressedAnswerButton);
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

        private void SetCurrentLanguageDialogCommon()
        {
            try
            {
                _currentDialogCommon = localizedDialogCommons?
                    .First(x => x.Language == GameController.Instance.CurrentLanguage).DialogCommons.ToArray();
                RenderDialog(dialogState, dialogPage);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public void OnLanguageChanged(Language language)
        {
            _currentDialogCommon = localizedDialogCommons?
                .First(x => x.Language == language).DialogCommons.ToArray();
            RenderDialog(dialogState, dialogPage);
        }
    }
}