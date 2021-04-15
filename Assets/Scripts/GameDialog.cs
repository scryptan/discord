using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameDialog : MonoBehaviour
{
    public GameObject canvasDialog = null;
    // Start is called before the first frame update
    private void Start()
    {
        
    }
    
    private void OnEnable()
    {
        canvasDialog?.SetActive(true);
    }

    private void OnDisable()
    {
        canvasDialog?.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
