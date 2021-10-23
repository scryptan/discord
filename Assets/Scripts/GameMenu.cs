using UnityEngine;

[ExecuteAlways]
public class GameMenu : MonoBehaviour
{
    public GameObject canvasMenu = null;

    // Start is called before the first frame update

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
}