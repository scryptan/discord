using System;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[ExecuteAlways]
public class GameDialog : MonoBehaviour
{
    [Header("Canvas Game Object")]
    public GameObject canvasDialog = null;
    public GameObject commonDialog = null;
    public GameObject failedDialog = null;
    
    public TMP_Text headerText = null;
    public GameObject[] buttons;

    [Header("Girl")] 
    public GameObject girl = null;
    public Sprite[] emotions;

    [Header("Dialogs")] 
    public string startGuyText = "";
    public DialogCommon[] dialogCommon;
    public SpriteAtlas girlDialog = null;
    public uint dialogPage = 0;
    public DialogState dialogState = DialogState.Common;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        RenderDialog(dialogState, dialogPage);
    }
    
    private void OnEnable()
    {
        if (canvasDialog != null)
            canvasDialog.SetActive(true);
    }

    private void OnDisable()
    {
        if (canvasDialog != null)
            canvasDialog.SetActive(false);
    }

    private void RenderDialog(DialogState dlgState, uint page)
    {
        dialogState = dlgState;
        dialogPage = page;

        var dlg = dialogCommon[page];
        var girlImage = girl.GetComponent<Image>();

        girlImage.sprite = emotions[Convert.ToInt32(dlg.girlEmotion)];

        switch (dlgState)
        {
            case DialogState.Common:
                headerText.text = dlg.textGirl;

                var i = 0;
                foreach (var button in buttons)
                {
                    button.SetActive(true);
                    button.GetComponent<ButtonDialog>().SetTextButton(dlg.textGuy[i++].text);
                }
                break;
            
            case DialogState.Failed:
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(dlgState), dlgState, null);
        }
        
    }

    public void PressedButtonDialog(TypeButton typeButton)
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public enum DialogState
    {
        Common = 0,
        Failed = 1,
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

}
