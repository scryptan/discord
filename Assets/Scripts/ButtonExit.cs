using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonExit : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler
{
    private bool _pointerEntered = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        _pointerEntered = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _pointerEntered)
        {
            Application.Quit();
        }

    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerEntered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerEntered = false;
    }

}
