using System;
using UnityEngine;

public class Girl : MonoBehaviour
{
    private Animator _animator;

    public GamePlaying gamePlaying;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Initialize()
    {
        _animator.Play("Girl_rage");
    }

    public void PlayIdle()
    {
        _animator.Play("Girl_idle");
    }

    public void PlaySearch()
    {
        _animator.Play("Girl_searching");
    }

    public void PlayThrow()
    {
        var rnd = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        if (rnd)
            _animator.Play("Girl_throw_hor");
        else
            _animator.Play("Girl_throw_ver");
    }

    public void Throw()
    {
        gamePlaying.SpawnItem();
        PlayIdle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
