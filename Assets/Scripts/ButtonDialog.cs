using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDialog : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image _imageRender = null;
    private bool _mouseHover = false;

    public TypeButton typeButton = TypeButton.One;
    
    public Sprite spriteCommon = null;
    public Sprite spriteHover = null;
    public Sprite spritePressed = null;

    public TMP_Text buttonText;

    private void Awake()
    {
        _imageRender = GetComponent<Image>();
        _mouseHover = false;
    }

    private void Update()
    {
        if (!_mouseHover) return;
        
        if (Input.GetMouseButton(0))
            _imageRender.sprite = spritePressed;
            
        else if (Input.GetMouseButtonUp(0))
            Process();
    }
    
    private void Process()
    {
        _imageRender.sprite = _mouseHover ? spriteHover : spriteCommon;
        GameController.Instance.gameDialog.GetComponent<GameDialog>().PressedButtonDialog(typeButton);
    }

    public void SetTextButton(string text)
    {
        buttonText.text = text;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _imageRender.sprite = spriteHover;
        _mouseHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _imageRender.sprite = spriteCommon;
        _mouseHover = false;
    }
}