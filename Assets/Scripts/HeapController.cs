﻿using System;
using UnityEngine;

public class HeapController : MonoBehaviour
{
    private float _value = 0f;
    
    public float startHeap = 0f;
    public Vector2 limitY = new Vector2(-8f, -0.86f);

    public GameObject heapMain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize()
    {
        heapMain.transform.position = new Vector3(0f, limitY.x, 0f);
        SetHeap(startHeap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHeap(float addValue)
    {
        SetHeap(_value + addValue);
    }

    public void SetHeap(float newValue)
    {
        _value = newValue;
        var _100Percent = Math.Abs(limitY.x) - Math.Abs(limitY.y);
        var _1Percent = _100Percent / 100;

        var _newValue = _1Percent * _value;
        
        heapMain.transform.position = new Vector3(0f, limitY.x + _newValue, 0f);

        if (_value >= 100f)
        {
            GameController.Instance.GameWin();
        }
    }
}
