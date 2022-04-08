using System.Collections.Generic;
using Core;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private float health;
        [SerializeField] private int damage;
        [SerializeField] private float speed;
        [SerializeField] private float nextDistance;
        [SerializeField] private List<Transform> waypoints = new List<Transform>();

        private Transform _currentWaypoint;
        [SerializeField] private Vector3 direction;

        protected virtual void Start()
        {
            if (waypoints.Count > 0)
            {
                waypoints.RemoveAt(0);
                _currentWaypoint = waypoints[0];
            }
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }

        protected virtual void Move()
        {
            if(_currentWaypoint == null)
                return;
            
            var distance = _currentWaypoint.position - transform.position;
            
            if (distance.magnitude > nextDistance)
            {
                direction = distance.normalized;
                transform.position += direction * speed;
            }
            else
            {
                if (waypoints.Count > 0)
                {
                    _currentWaypoint = waypoints[0];
                    waypoints.RemoveAt(0); 
                }
                else
                    DamageBase();
            }
        }

        protected virtual void DamageBase()
        {
            GameManager.Instance.TakeDamage(damage);
            Death();
        }

        public void TakeDamage(float value)
        {
            health -= value;
            if (health <= 0)
                Death();
        }

        protected virtual void Death()
        {
            Destroy(gameObject);
        }

        public void SetWaypoints(List<Transform> points)
        {
            waypoints = points;
        }
    }
}