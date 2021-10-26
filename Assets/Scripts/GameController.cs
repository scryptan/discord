using System;
using UnityEngine;

namespace ThinIce
{
    public class GameController : MonoBehaviour
    {
        #region private region

        private static GameController _instance = null;
        private GameState _gameState;

        #endregion

        #region public region

        public GameState startState = GameState.Intro;
        public GameObject gameIntro = null;
        public GameObject gameMenu = null;
        public GameObject startMenu = null;
        public GameObject settings = null;
        public GameObject gameDialog = null;
        public GameObject gamePlaying = null;
        public GameObject gameWin = null;
        public GameObject gameLose = null;

        public bool seePreviousAnswers = false;

        public static GameController Instance => _instance;

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

            gameIntro.SetActive(true);
            gameMenu.SetActive(false);
            gameDialog.SetActive(false);
            gamePlaying.SetActive(false);
            gameWin.SetActive(false);
            gameLose.SetActive(false);
            settings.SetActive(false);
        }

        public void GameMenu()
        {
            _gameState = GameState.MainMenu;

            gameIntro.SetActive(false);
            gameMenu.SetActive(true);
            startMenu.SetActive(false);
            gameDialog.SetActive(false);
            settings.SetActive(false);
            gamePlaying.SetActive(false);
            gameWin.SetActive(false);
            gameLose.SetActive(false);
        }

        public void StartMenu()
        {
            _gameState = GameState.StartMenu;

            gameIntro.SetActive(false);
            gameMenu.SetActive(true);
            startMenu.SetActive(true);
            gameDialog.SetActive(false);
            settings.SetActive(false);
            gamePlaying.SetActive(false);
            gameWin.SetActive(false);
            gameLose.SetActive(false);
        }

        public void GameDialog()
        {
            _gameState = GameState.Dialog;

            gameIntro.SetActive(false);
            gameMenu.SetActive(false);
            gameDialog.SetActive(true);
            startMenu.SetActive(false);
            settings.SetActive(false);
            gamePlaying.SetActive(false);
            gameWin.SetActive(false);
            gameLose.SetActive(false);
        }

        public void GameStart()
        {
            _gameState = GameState.Game;

            gameIntro.SetActive(false);
            gameMenu.SetActive(false);
            gameDialog.SetActive(false);
            gamePlaying.SetActive(true);
            startMenu.SetActive(false);
            gameWin.SetActive(false);
            gameLose.SetActive(false);

            gamePlaying.GetComponent<GamePlaying>().Initialize();
        }

        public void GameWin()
        {
            _gameState = GameState.Win;

            gameIntro.SetActive(false);
            gameMenu.SetActive(false);
            gameDialog.SetActive(false);
            gamePlaying.SetActive(false);
            gameWin.SetActive(true);
            startMenu.SetActive(false);
            gameLose.SetActive(false);
        }

        public void GameLose()
        {
            _gameState = GameState.Lose;

            gameIntro.SetActive(false);
            gameMenu.SetActive(false);
            gameDialog.SetActive(false);
            gamePlaying.SetActive(false);
            gameWin.SetActive(false);
            startMenu.SetActive(false);
            gameLose.SetActive(true);
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
        }
    }
}