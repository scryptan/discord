using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinIce.TowerDefence.Plane;
using ThinIce.TowerDefence.Targets;
using UnityEngine;

namespace ThinIce.TowerDefence.Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform attackPos;

        [SerializeField] private double defaultAttackDelay = 1.5f;
        private bool _canShoot;
        private readonly HashSet<IHittable> _targets = new HashSet<IHittable>();

        public Vector3 startPosition;
        public int startLayer;
        public TowerPlank holdingPlank;

        private void Update()
        {
            if (_targets.Count > 0)
                Attack(_targets.First());
        }

        public void Attack(IHittable hittable)
        {
            if (_canShoot)
            {
                // Rotate(hittable.Transform.position);
                SpawnBullet(hittable);
#pragma warning disable CS4014
                StartRecharge();
#pragma warning restore CS4014
            }
        }

        private void Start()
        {
#pragma warning disable CS4014
            StartRecharge();
#pragma warning restore CS4014
            startPosition = transform.position;
            startLayer = gameObject.layer;
        }

        private void Rotate(Vector3 point)
        {
            transform.LookAt(point);
        }

        private void SpawnBullet(IHittable target)
        {
            var position = attackPos.position;
            var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            bullet.Init(position, target.Transform);
        }

        private async Task StartRecharge()
        {
            _canShoot = false;
            await Task.Delay(TimeSpan.FromSeconds(defaultAttackDelay));
            _canShoot = true;
        }

        public void AddTarget(IHittable target)
        {
            _targets.Add(target);
        }

        public void RemoveTarget(IHittable target)
        {
            _targets.Remove(target);
        }
    }
}