using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Enemies;
using Unity.Mathematics;
using UnityEngine;

namespace Miscellaneous
{
    public class Shell : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _damage;
        private Vector3 _target;
        private int _opacity = -1;
        private bool falling;
        [SerializeField] private float _opacitySpeed = 0.01f;
        private SpriteRenderer sprite;

        private CircleCollider2D _collider;

        public void SetTarget(Vector3 target)
        {
            _target = target;
        }

        private void Awake()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            _collider = GetComponent<CircleCollider2D>();
            _collider.radius = _explosionRadius;
            StartCoroutine(Launch());
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * _speed;
            
            Color alpha = sprite.color;
            alpha.a = math.clamp(alpha.a + _opacity * _opacitySpeed, 0, 1);
            sprite.color = alpha;
        }

        private void Update()
        {
            if (falling && (_target - transform.position).magnitude < 0.05f)
            {
                Explode();
            }
        }

        private void Explode()
        {
            List<Collider2D> contacts = new List<Collider2D>(); 
            _collider.GetContacts(contacts);
            foreach (var col in contacts)
            {
                Enemy enemy = col.GetComponent<Enemy>();
                if(enemy != null)
                    enemy.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }

        private IEnumerator Launch()
        {
            yield return new WaitForSeconds(1);
            transform.position = new Vector3(_target.x, transform.position.y, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 180);
            _opacity = 1;
            falling = true;
        }
    }
}