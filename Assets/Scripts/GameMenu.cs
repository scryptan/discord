using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameMenu : MonoBehaviour
{
    public GameObject canvasMenu = null;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        if (canvasMenu != null)
            canvasMenu.SetActive(true);
    }

    private void OnDisable()
    {
        if (canvasMenu != null)
            canvasMenu.SetActive(false);
    }

    public void NewGame()
    {
        GameController.Instance.GameDialog();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
