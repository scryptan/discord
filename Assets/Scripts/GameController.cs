using System;
using UnityEngine;

namespace ThinIce
{
    public class GameController : MonoBehaviour
    {
        #region private region

        private static GameController _instance;
        private GameState _gameState;
        private const string LanguageKey = "language";
        [SerializeField] private Language currentLanguage;

        #endregion

        #region public region

        private Action<Language> _languageChanged;
        private ReviewButton _reviewButton;

        public GameState startState = GameState.Intro;
        public GameObject gameIntro;
        public GameObject gameMenu;
        public GameObject startMenu;
        public GameObject settings;
        public GameObject gameDialogWindow;
        public GameObject gamePlaying;
        public GameObject gameWin;
        public GameObject gameLose;
        public GameObject tipDialog;
        public GameObject reviewDialog;

        public bool seePreviousAnswers;

        public static GameController Instance => _instance;

        public GameDialogWithLanguages gameDialog;

        public Language CurrentLanguage
        {
            get => currentLanguage;
            private set
            {
                currentLanguage = value;
                _languageChanged?.Invoke(currentLanguage);
            }
        }

        public void SetLanguage(int language)
        {
            CurrentLanguage = (Language) language;
        }

        public void SetLanguage(Language language)
        {
            CurrentLanguage = language;
        }

        #endregion

        // Start is called before the first frame update
        private void Awake()
        {
            _instance = this;
            foreach (var item in FindObjectsOfType<UiLocalizedItem>(true))
            {
                _languageChanged += item.LanguageChanged;
            }
        }

        private void Start()
        {
            Application.targetFrameRate = 60;

            Initialize();
            gameDialog = FindObjectOfType<GameDialogWithLanguages>();
            _languageChanged += gameDialog.OnLanguageChanged;
            CurrentLanguage = GetCurrentLanguage();
        }

        // Update is called once per frame
        private void Update()
        {
            var quit = Input.GetKey(KeyCode.Escape);

            if (quit)
                Application.Quit();
        }

        private void Initialize()
        {
            switch (startState)
            {
                case GameState.Intro:
                    GameIntro();
                    break;

                case GameState.MainMenu:
                    GameMenu();
                    break;

                case GameState.Dialog:
                    GameDialog();
                    break;

                case GameState.Game:
                    GameStart();
                    break;

                case GameState.Tip:
                    TipDialog();
                    break;
                case GameState.Review:
                    ReviewState(false);
                    break;
                case GameState.Win:
                    break;
                case GameState.Lose:
                    break;

                default:
                    throw new Exception($"Undefined GameState on Initialize GameController [{startState}]");
            }
        }

        public void StartGameWithAnswers()
        {
            seePreviousAnswers = true;
            RestartGame();
        }

        public void RestartGame()
        {
            gameDialog.RestartDialog();
            GameDialog();
        }

        #region Game States

        public void GameIntro()
        {
            _gameState = GameState.Intro;

            SetAllWindowsFalse();
            gameIntro.SetActive(true);
        }

        public void ReviewState(bool isBug)
        {
            _gameState = GameState.Review;
            _reviewButton = reviewDialog.GetComponentInChildren<ReviewButton>();

            SetAllWindowsFalse();
            _reviewButton.SetType(isBug);
            reviewDialog.SetActive(true);
        }

        public void GameMenu()
        {
            _gameState = GameState.MainMenu;

            SetAllWindowsFalse();
            gameMenu.SetActive(true);
        }

        public void StartMenu()
        {
            seePreviousAnswers = false;
            _gameState = GameState.StartMenu;

            SetAllWindowsFalse();
            gameMenu.SetActive(true);
            startMenu.SetActive(true);
        }

        public void GameDialog()
        {
            _gameState = GameState.Dialog;

            SetAllWindowsFalse();
            gameDialogWindow.SetActive(true);
        }

        public void TipDialog()
        {
            _gameState = GameState.Tip;

            SetAllWindowsFalse();
            tipDialog.SetActive(true);
        }

        public void GameStart()
        {
            _gameState = GameState.Game;

            SetAllWindowsFalse();
            gamePlaying.SetActive(true);

            gamePlaying.GetComponent<GamePlaying>().Initialize();
        }

        public void GameWin()
        {
            _gameState = GameState.Win;

            SetAllWindowsFalse();
            gameWin.SetActive(true);
        }

        public void GameLose()
        {
            _gameState = GameState.Lose;
            SetAllWindowsFalse();
            gameLose.SetActive(true);
        }

        private void SetAllWindowsFalse()
        {
            gameIntro.SetActive(false);
            gameMenu.SetActive(false);
            gameDialogWindow.SetActive(false);
            startMenu.SetActive(false);
            settings.SetActive(false);
            gamePlaying.SetActive(false);
            gameWin.SetActive(false);
            gameLose.SetActive(false);
            tipDialog.SetActive(false);
            reviewDialog.SetActive(false);
        }

        #endregion

        private Language GetCurrentLanguage()
        {
            if (PlayerPrefs.HasKey(LanguageKey) &&
                Enum.TryParse<Language>(PlayerPrefs.GetString(LanguageKey), out var language))
                return language;
            if (Application.systemLanguage == SystemLanguage.Russian)
            {
                PlayerPrefs.SetString(LanguageKey, Language.Russian.ToString());
                return Language.Russian;
            }

            PlayerPrefs.SetString(LanguageKey, Language.English.ToString());
            return Language.English;
        }

        [Serializable]
        public enum GameState
        {
            Intro = 0,
            MainMenu = 1,
            StartMenu = 2,
            Dialog = 3,
            Game = 4,
            Win = 5,
            Lose = 6,
            Tip = 7,
            Review = 8
        }
    }
}