using System;
using UnityEngine;

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
    public GameObject gameDialog = null;
    public GameObject gamePlaying = null;
    public GameObject gameWin = null;
    public GameObject gameLose = null;

    public static GameController Instance => _instance;

    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        _instance = this;

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

            case GameState.Menu:
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
    }

    public void GameMenu()
    {
        _gameState = GameState.Menu;

        gameIntro.SetActive(false);
        gameMenu.SetActive(true);
        gameDialog.SetActive(false);
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
        gameLose.SetActive(true);
    }

    #endregion

    [Serializable]
    public enum GameState
    {
        Intro = 0,
        Menu = 1,
        Dialog = 2,
        Game = 3,
        Win = 4,
        Lose = 5,
    }
}