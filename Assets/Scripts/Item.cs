using System;
using UnityEngine;
using UnityEngine.U2D;

[Serializable]
public enum ItemType { backpack, ball, bag, bear, boomerang, box, carpet, chair, energy, guitar, pillow, robot, serpent, shawarma, vase }

public class Item : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    
    public SpriteAtlas spriteAtlas;
    public ItemType itemType;

    // Start is called before the first frame update
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer.sprite = spriteAtlas.GetSprite($"{itemType.ToString()}_01");
    }

    public void AddForce(Vector2 forceVector)
    {
        _rigidbody2D.AddForce(forceVector, ForceMode2D.Impulse);
    }

    private void Feature()
    {
        
    }

    private void Crash()
    {
        _spriteRenderer.sprite = spriteAtlas.GetSprite($"{itemType.ToString()}_02");
        Feature();
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        _boxCollider2D.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
            Crash();
    }
}
