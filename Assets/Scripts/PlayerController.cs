using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region private region
    private PlayerState _playerState;
    private PlayerState _oldPlayerState;
    private PlayerDirectional _directional = PlayerDirectional.Center;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private GameObject _grabbedItem = null;
    private List<GameObject> _headItems;
    #endregion

    #region public region
    public GamePlaying gamePlaying;
    public GameObject hand;

    public Vector3 handOffset = new Vector3(-0.4f, -0.07f, 0f);
    public Vector3 itemOffset = new Vector3(-0.27f, -0.07f, 0);

    [Header("WalkSpeed")]
    public float walkSpeed = 4f;
    
    [Header("Limits")]
    public Vector2 limitX = new Vector2(-3.15f, 3.35f);
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _headItems = new List<GameObject>();
    }

    public void PlayerMove(Vector3 directional)
    {
        if (_directional != PlayerDirectional.Center || _oldPlayerState != _playerState)
        {
            switch(_playerState)
            {
                case PlayerState.Free:
                    _animator.Play("Guy_run");
                    hand.SetActive(false);
                    break;
                
                case PlayerState.Grabbed:
                    _animator.Play("Guy_run_grabbed");
                    hand.SetActive(true);
                    break;
                
                case PlayerState.Head:
                    _animator.Play("Guy_run_head");
                    break;
                
                case PlayerState.GrabbedAndHead:
                    _animator.Play("Guy_run_head_grabbed");
                    hand.SetActive(true);
                    break;
            }
        }
        
        if (directional.magnitude > 0)
        {
            directional.Normalize();

            if (directional.magnitude != 0)
            {
                transform.Translate(directional * (walkSpeed * Time.deltaTime));
                var handPos = hand.transform.position;

                if (directional.x > 0)
                {
                    _directional = PlayerDirectional.Right;
                    _spriteRenderer.flipX = true;
                    
                    hand.transform.localPosition = new Vector3(-handOffset.x, handOffset.y, handOffset.z);
                    hand.GetComponent<SpriteRenderer>().flipX = true;

                    if (_grabbedItem)
                    {
                        _grabbedItem.transform.localPosition = new Vector3(-itemOffset.x, itemOffset.y, itemOffset.z);
                        hand.GetComponent<SpriteRenderer>().flipX = true;
                    }
                }
                else
                {
                    _directional = PlayerDirectional.Left;
                    _spriteRenderer.flipX = false;
                    
                    hand.transform.localPosition = new Vector3(handOffset.x, handOffset.y, handOffset.z);
                    hand.GetComponent<SpriteRenderer>().flipX = false;
                    
                    if (_grabbedItem)
                    {
                        _grabbedItem.transform.localPosition = new Vector3(itemOffset.x, itemOffset.y, itemOffset.z);
                        hand.GetComponent<SpriteRenderer>().flipX = false;
                    }
                }

                var posY = transform.position.y;
                var posZ = transform.position.z;

                if (transform.position.x < limitX.x)
                    transform.position = new Vector3(limitX.x, posY, posZ);
                
                if (transform.position.x > limitX.y)
                    transform.position = new Vector3(limitX.y, posY, posZ);
                
            }
        }

        _oldPlayerState = _playerState;
    }

    public void PlayerIdle()
    {
        _directional = PlayerDirectional.Center;
        hand.SetActive(false);
        
        if (_playerState == PlayerState.Head)
            _animator.Play("Guy_idle_head");
        else
            _animator.Play("Guy_idle");
    }

    public void PlayerSpawn(Vector3 position)
    {
        transform.position = position;
        _playerState = PlayerState.Free;
        _grabbedItem = null;
        _oldPlayerState = PlayerState.Free;
        hand.SetActive(false);
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
            score += item.GetComponent<Item>().points * gamePlaying.caughtCoefItemScore;
            Destroy(item);
        }
        
        _headItems.Clear();

        gamePlaying.AddHeap(score);

        _playerState = PlayerState.Free;
    }

    public void Caught(GameObject item)
    {
        if (_playerState == PlayerState.Grabbed)
            _playerState = PlayerState.GrabbedAndHead;
        else
            _playerState = PlayerState.Head;
        
        _headItems.Add(item);
        
        
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

                if (_playerState == PlayerState.Head)
                    _playerState = PlayerState.GrabbedAndHead;
                else
                    _playerState = PlayerState.Grabbed;
                
                gamePlaying.SubtractWeight(other.gameObject.GetComponent<Item>().weight);
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
