using UnityEngine;

namespace ThinIce.TowerDefence.Towers
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float timeToGoal = 2;
        [SerializeField] private float epsilon = .1f;
        private Vector3 _startPos;
        private Transform _targetPos;
        private float _pastTime;

        public void Init(Vector3 startPos, Transform targetPos)
        {
            _startPos = startPos;
            _targetPos = targetPos;
        }

        private void Update()
        {
            _pastTime += Time.deltaTime;
            transform.LookAt(_targetPos);
            transform.position = Vector3.Lerp(_startPos, _targetPos.position, _pastTime / timeToGoal);
            if (Vector3.Distance(transform.position, _targetPos.position) < epsilon)
                Destroy(gameObject);
        }
    }
}