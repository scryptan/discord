using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerState _playerState;
    private PlayerDirectional _directional = PlayerDirectional.Center;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private GameObject _grabbedItem = null;
    private List<GameObject> _headItems;

    public GamePlaying gamePlaying;
    
    [Header("WalkSpeed")]
    public float walkSpeed = 3f;
    
    [Header("Limits")]
    public Vector2 limitX = new Vector2(-3.15f, 3.35f);
    
    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _headItems = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerMove(Vector3 directional)
    {
        if (_directional != PlayerDirectional.Center)
        {
            if (_playerState == PlayerState.Free)
                _animator.Play("Guy_run");
            else
                _animator.Play("Guy_run");  
        }

        
        if (directional.magnitude > 0)
        {
            directional.Normalize();

            if (directional.magnitude != 0)
            {
                transform.Translate(directional * (walkSpeed * Time.deltaTime));

                if (directional.x > 0)
                {
                    _directional = PlayerDirectional.Right;
                    _spriteRenderer.flipX = true;
                }
                else
                {
                    _directional = PlayerDirectional.Left;
                    _spriteRenderer.flipX = false;
                }

                var posY = transform.position.y;
                var posZ = transform.position.z;

                if (transform.position.x < limitX.x)
                    transform.position = new Vector3(limitX.x, posY, posZ);
                
                if (transform.position.x > limitX.y)
                    transform.position = new Vector3(limitX.y, posY, posZ);
                
            }
        }
    }

    public void PlayerIdle()
    {
        _directional = PlayerDirectional.Center;
        _animator.Play("Guy_idle");
    }

    public void PlayerSpawn(Vector3 position)
    {
        transform.position = position;
        _playerState = PlayerState.Free;
        _grabbedItem = null;
        PlayerIdle();
    }

    private void DeliveryItems()
    {
        var score = 0f;

        if (_grabbedItem)
        {
            score += _grabbedItem.GetComponent<Item>().points;
            Destroy(_grabbedItem);
            _grabbedItem = null;
        }

        foreach (var item in _headItems)
        {
            score += item.GetComponent<Item>().points;
            Destroy(item);
        }

        gamePlaying.AddHeap(score);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Items"))
        {
            if (_playerState != PlayerState.Grabbed)
            {
                _playerState = PlayerState.Grabbed;
                _grabbedItem = other.gameObject;
                other.gameObject.GetComponent<Item>().SetGrabbed(transform);
            }
        }
        
        if (other.gameObject.CompareTag("Finish"))
        {
            DeliveryItems();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            DeliveryItems();
        }
    }

    private enum PlayerDirectional
    {
        Left = -1,
        Center = 0,
        Right = 1,
    }
    
    private enum PlayerState
    {
        Free = 0,
        Grabbed = 1,
        Head = 2,
        GrabbedAndHead = 3,
    }
}
