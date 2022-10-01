using UnityEngine;

namespace ThinIce.TowerDefence.Targets
{
    public interface IHittable
    {
        public Transform Transform { get; }
    }
}