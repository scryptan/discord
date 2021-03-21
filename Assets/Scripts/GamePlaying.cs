using System;
using UnityEngine;

public class GamePlaying : MonoBehaviour
{
    private float _stamina = 100f;
    private uint _round = 1;

    private float _timer = 0f;
    private float _roundTimer = 0f;

    [Header("Game Objects")]
    public PlayerController guy;
    public Girl girl;
    public HeapController heap;

    [Header("Start Game Config")]
    public float startStamina = 100f;
    public float startHeap = 0f;
    public Vector3 startGuyPosition = new Vector3(0, -4, 0);
    public uint startRound = 1;
    public float timerNextRound = 60f;

    // Start is called before the first frame update
    void Awake()
    {
    }

    public void Initialize()
    {
        _stamina = startStamina;
        _round = startRound;
        _timer = 0f;
        _roundTimer = 0f;
        
        guy.PlayerSpawn(startGuyPosition);
        girl.Initialize();
        heap.Initialize();
    }

    // Update is called once per frame
    private void Update()
    {
        _timer += Time.deltaTime;
        _roundTimer += Time.deltaTime;
        
        KeyboardUpdate();
    }

    private void KeyboardUpdate()
    {
        // Horizontal Movement
        var moveLeft = Input.GetKey(KeyCode.A);
        var moveRight = Input.GetKey(KeyCode.D);

        if (moveLeft || moveRight)
        {
            var xPosition = -Convert.ToInt16(moveLeft) + Convert.ToInt16(moveRight);
            const float yPosition = 0f;
            
            var directional = new Vector3(xPosition, yPosition, 0f);
            
            guy.PlayerMove(directional);
        }
        else
        {
            guy.PlayerIdle();
        }
    }
    
    

}
