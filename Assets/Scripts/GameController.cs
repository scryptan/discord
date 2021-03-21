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
    public GameObject gameIntro;
    public GameObject gameDialog;
    public GameObject gamePlaying;

    public static GameController Instance
    {
        get => _instance;
    }
    #endregion
    
    // Start is called before the first frame update
    private void Start()
    {
        _instance = this;

        Application.targetFrameRate = 60;
        
        Initialize();
    }

    private void Initialize()
    {
        switch (startState)
        {
            case GameState.Intro:
                GameIntro();
                break;
            
            case GameState.Dialog:
                GameDialog();
                break;
            
            case GameState.Game:
                GameStart();
                break;
            
            default:
                throw new Exception($"Undefined GameState on Initialize GameController [{startState}]");
        }
    }

    public void GameStart()
    {
        _gameState = GameState.Game;
        
        gamePlaying.SetActive(true);
        gameDialog.SetActive(false);
        gameIntro.SetActive(false);
        
        gamePlaying.GetComponent<GamePlaying>().Initialize();
    }

    public void GameDialog()
    {
        _gameState = GameState.Dialog;

        gameDialog.SetActive(true);
        gameIntro.SetActive(false);
        gamePlaying.SetActive(false);
    }

    public void GameIntro()
    {
        _gameState = GameState.Intro;

        gameIntro.SetActive(true);
        gameDialog.SetActive(false);
        gamePlaying.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    [Serializable]
    public enum GameState
    {
        Intro = 0,
        Dialog = 1,
        Game = 2,
    }
}
