using System;
using UnityEngine;

public class GamePlaying : MonoBehaviour
{
    private float _stamina = 100f;
    private uint _round = 0;

    private float _timer = 0f;
    private float _roundTimer = 0f;

    private float _roundMaxTimer = 2f;
    private bool _girlSearching = false;

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
        _girlSearching = false;

        if (rounds.Length > 0)
            _roundMaxTimer = rounds[0];
        
        guy.PlayerSpawn(startGuyPosition);
        girl.Initialize();
        heap.Initialize();
    }

    // Update is called once per frame
    private void Update()
    {
        _timer += Time.deltaTime;
        _roundTimer += Time.deltaTime;

        if (_timer >= _roundMaxTimer - 0.3f && !_girlSearching)
        {
            _girlSearching = true;
            girl.PlaySearch();
        }
        
        if (_timer >= _roundMaxTimer)
        {
            _timer = 0f;
            _girlSearching = false;
            SpawnItem();
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

    private void SpawnItem()
    {
        var itemId = UnityEngine.Random.Range(0, items.Length);
        var item = Instantiate<GameObject>(items[itemId], transform);
        item.transform.position = girl.transform.position;
        
        var x = UnityEngine.Random.Range(-forceToPush, forceToPush);
        var y = UnityEngine.Random.Range(-forceToPush / 3, forceToPush / 3);
        item.GetComponent<Item>().AddForce(new Vector2(x, y));
    }
    
    private void NextRound()
    {
        if (_round >= rounds.Length - 1)
            return;

        _roundMaxTimer = rounds[++_round];
    }

}
