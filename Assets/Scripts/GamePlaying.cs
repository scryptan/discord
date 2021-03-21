using System;
using UnityEngine;

public class GamePlaying : MonoBehaviour
{
    private float _stamina = 100f;
    private uint _round = 0;

    private float _timer = 0f;
    private float _roundTimer = 0f;

    private float _roundMaxTimer = 2f;
    private bool _girlSearchingAndThrow = false;

    [Header("Game Objects")]
    public PlayerController guy;
    public Girl girl;
    public HeapController heap;
    public GameObject[] items;

    [Header("Start Game Config")]
    public float startStamina = 100f;
    public float startHeap = 0f;
    public Vector3 startGuyPosition = new Vector3(0, -4, 0);
    public uint startRound = 0;
    public float timerNextRound = 60f;
    public float forceToPush = 3f;
    public float horizontalCoef = 1.5f;

    [Header("Rounds Config")] 
    public float[] rounds;

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
        _girlSearchingAndThrow = false;

        if (rounds.Length > 0)
            _roundMaxTimer = rounds[0];
        
        guy.PlayerSpawn(startGuyPosition);
        girl.Initialize();
        heap.Initialize();
    }

    // Update is called once per frame
    private void Update()
    {
        _roundTimer += Time.deltaTime;

        if (!_girlSearchingAndThrow)
        {
            _timer += Time.deltaTime;
            
            if (_timer >= _roundMaxTimer)
            {
                _timer = 0f;
                girl.PlaySearch();
                _girlSearchingAndThrow = true;
            }
        }

        if (_roundTimer >= timerNextRound)
        {
            _roundTimer = 0f;
            NextRound();
        }
        
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

    public void SpawnItem()
    {
        var itemId = UnityEngine.Random.Range(0, items.Length);
        var item = Instantiate<GameObject>(items[itemId], transform);
        item.transform.position = girl.transform.position;
        
        var x = UnityEngine.Random.Range(-forceToPush, forceToPush);
        var y = UnityEngine.Random.Range(-forceToPush / horizontalCoef, forceToPush / horizontalCoef);
        item.GetComponent<Item>().AddForce(new Vector2(x, y));

        _girlSearchingAndThrow = false;
    }
    
    private void NextRound()
    {
        if (_round >= rounds.Length - 1)
            return;

        _roundMaxTimer = rounds[++_round];
        Debug.Log($"Next round [${_round}]!");
    }

    public void AddHeap(float score)
    {
        heap.AddHeap(score);
    }

}
