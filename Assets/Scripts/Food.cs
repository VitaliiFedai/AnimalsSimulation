using AnimalSimulation.Core;
using System;
using Unity.VisualScripting;
using UnityEditor.Sprites;
using UnityEngine;

namespace AnimalSimulation
{
    [RequireComponent(typeof(SphereCollider))]
    public class Food : Entity
    {
        public event Action<Food, Animal> OnPick;
        public float Radius => GetCollider().radius;
        public Vector3 Position => transform.position;
        private SphereCollider _collider;

        public bool CanBePickedBy(Animal picker)
        {
            if (!GetCollider().bounds.Intersects(picker.Collider.bounds))
            {
                return false;
            }

            return Physics.ComputePenetration(
                GetCollider(), transform.position, transform.rotation,
                picker.Collider, picker.transform.position, picker.transform.rotation,
                out Vector3 direction, out float distance); 
        }

        public void Pick(Animal picker)
        {
            OnPick?.Invoke(this, picker);
            Despawn();
        }

        protected override void OnDespawnInner()
        {
            base.OnDespawnInner();
            OnPick = null;
        }

        private SphereCollider GetCollider() 
        {
            if (_collider == null)
            {
                _collider = GetComponent<SphereCollider>();
            }
            return _collider;
        }
    }
}