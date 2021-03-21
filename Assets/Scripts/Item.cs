using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[Serializable]
public enum ItemType { backpack, bag, bear, boomerang, box, carpet, chair, energy, guitar, pillow, robot, serpent, shawarma, vase }

public class Item : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    public SpriteAtlas spriteAtlas;
    public ItemType itemType;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = spriteAtlas.GetSprite($"{itemType.ToString()}_01");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }
}
