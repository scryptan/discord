using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteAlways]
public class GameDialog : MonoBehaviour
{
    [Header("Canvas Game Object")]
    public GameObject canvasDialog = null;

    public GameObject commonDialog = null;
    public GameObject failedDialog = null;

    [Header("Dialogs")] 
    public string startGuyText = "";
    public DialogCommon[] dialogCommon;
    public SpriteAtlas girlDialog = null;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }
    
    private void OnEnable()
    {
        if (!(canvasDialog is null)) canvasDialog?.SetActive(true);
    }

    private void OnDisable()
    {
        if (!(canvasDialog is null)) canvasDialog?.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    [Serializable]
    public struct DialogCommon
    {
        [Multiline(3)]
        public string textGirl;
        public GirlEmotion girlEmotion;

        public TextGuy[] textGuy;
    }

    [Serializable]
    public class TextGuy
    {
        [Multiline(3)]
        public string text = "";
        public bool badText = false;

        public string failText = "";
    }

    public enum GirlEmotion
    {
        common,
        questionable,
        hey,
        angry,
        wrong,
        cute,
    }
    
}
