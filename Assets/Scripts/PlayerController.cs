using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerState _playerState;
    private PlayerDirectional _directional = PlayerDirectional.Center;
    
    [Header("WalkSpeed")]
    public float walkSpeed = 3f;
    
    [Header("Limits")]
    public Vector2 limitX = new Vector2(-3.15f, 3.35f);
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerMove(Vector3 directional)
    {
        if (directional.magnitude > 0)
        {
            directional.Normalize();

            if (directional.magnitude != 0)
            {
                transform.Translate(directional * (walkSpeed * Time.deltaTime));

                _directional = directional.x > 0 ? PlayerDirectional.Right : PlayerDirectional.Left;
                
                var posY = transform.position.y;
                var posZ = transform.position.z;

                if (transform.position.x < limitX.x)
                    transform.position = new Vector3(limitX.x, posY, posZ);
                
                if (transform.position.x > limitX.y)
                    transform.position = new Vector3(limitX.y, posY, posZ);
            }
        }

    }

    public void PlayerIdle()
    {
        _directional = PlayerDirectional.Center;
    }

    public void PlayerSpawn(Vector3 position)
    {
        transform.position = position;
        _playerState = PlayerState.Free;
        _directional = PlayerDirectional.Center;
    }

    private enum PlayerDirectional
    {
        Left = -1,
        Center = 0,
        Right = 1,
    }
    
    private enum PlayerState
    {
        Free = 0,
        Grabbed = 1,
    }
}
