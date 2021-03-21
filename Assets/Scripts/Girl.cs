using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : MonoBehaviour
{
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Initialize()
    {
        _animator.Play("Girl_idle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
