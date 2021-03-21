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
        
        Initialize();
    }

    private void Initialize()
    {
        switch (startState)
        {
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
        
        gamePlaying.GetComponent<GamePlaying>().Initialize();
    }

    public void GameDialog()
    {
        _gameState = GameState.Dialog;

        gameDialog.SetActive(true);
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
        Menu = 1,
        Dialog = 2,
        Game = 3,
    }
}
