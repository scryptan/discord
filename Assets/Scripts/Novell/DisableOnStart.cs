using UnityEngine;

namespace ThinIce.Novell
{
    public class DisableOnStart : MonoBehaviour
    {
        void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
