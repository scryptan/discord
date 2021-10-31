using System;
using UnityEngine;

namespace ThinIce
{
    public class GameController : MonoBehaviour
    {
        #region private region

        private static GameController _instance;
        private GameState _gameState;

        #endregion

        #region public region

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

        public bool seePreviousAnswers;

        public static GameController Instance => _instance;

        public GameDialog gameDialog;
        
        #endregion

        // Start is called before the first frame update
        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;

            Initialize();
            gameDialog = FindObjectOfType<GameDialog>();
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
                case GameState.Win:
                    break;
                case GameState.Lose:
                    break;

                default:
                    throw new Exception($"Undefined GameState on Initialize GameController [{startState}]");
            }
        }

        #region Game States

        public void GameIntro()
        {
            _gameState = GameState.Intro;

            SetAllWindowsFalse();
            gameIntro.SetActive(true);
        }

        public void GameMenu()
        {
            _gameState = GameState.MainMenu;

            SetAllWindowsFalse();
            gameMenu.SetActive(true);
        }

        public void StartMenu()
        {
            _gameState = GameState.StartMenu;

            SetAllWindowsFalse();
            gameMenu.SetActive(true);
            startMenu.SetActive(true);
        }

        public void GameDialog()
        {
            _gameState = GameState.Dialog;

            SetAllWindowsFalse();
            gameDialog.RestartDialog();
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
        }

        #endregion

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
        }
    }
}