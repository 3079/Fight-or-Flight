using System.Collections;
using System.Collections.Generic;
using Core;
using Enemies;
using UnityEngine;

namespace Buildings
{
    public class Tower: Building
    {
        [SerializeField] protected float damage;
        [SerializeField] protected float fireRate;
        [SerializeField] protected float range;
        [SerializeField] protected SpriteRenderer areaVisuals;

        protected List<Enemy> enemies = new List<Enemy>();
        protected CircleCollider2D _triggerArea;
        protected Enemy _target;
        private LineRenderer _lineRenderer;

        protected float _timer;

        protected new void Awake()
        {
            base.Awake();
            
            foreach (var collider in GetComponentsInChildren<CircleCollider2D>())
            {
                if (collider.CompareTag("Tower Trigger Area"))
                {
                    _triggerArea = collider;
                    break;
                }
            }
            
            _triggerArea.radius = range;
            
            areaVisuals.transform.localScale = new Vector3(range * 2, range * 2, 0);
            areaVisuals.gameObject.SetActive(false); 
        }

        protected void Start()
        {
            _timer = fireRate;
            _lineRenderer = GetComponent<LineRenderer>();
        }

        protected void Update()
        {
            if (_population > 0)
            {
                _timer -= Time.deltaTime;
                if(_timer <= 0)
                    Attack();
            }
        }

        protected virtual void Attack()
        {
            if(_target == null)
                _target = GetClosestEnemy();

            if (_target == null) return;

            StartCoroutine(Burst());
            _target.TakeDamage(damage);
            _timer = fireRate * 3 / _population;
        }

        protected IEnumerator Burst()
        {
            if (_target != null)
            {
                _lineRenderer.positionCount = 2;
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, _target.transform.position);
                AudioManager.Instance.PlaySFX("gun");
                yield return new WaitForSeconds(0.1f);
            }
            else yield break;

            _lineRenderer.positionCount = 0;
        }

        protected void OnTriggerEnter2D(Collider2D col)
        {
            var enemy = col.GetComponent<Enemy>();
            if(enemy != null)
                enemies.Add(enemy);
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (_target == enemy)
                    _target = null;
                
                enemies.Remove(enemy);
            }
        }

        protected Enemy GetClosestEnemy()
        {
            Enemy enemy = null;
            float minDistance = int.MaxValue;
            for (int i = 0; i < enemies.Count; i++)
            {
                float distance = (transform.position - enemies[i].transform.position).magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    enemy = enemies[i];
                }
            }

            return enemy;
        }

        public override IEnumerator LvlUP()
        {
            if (_level < _maxLevel && GameManager.Instance.Science >= levels[_level].scienceCost &&
                GameManager.Instance.Gold >= levels[_level].goldCost)
            {
                _canUpgrade = false;
                GameManager.Instance.Gold -= levels[_level].goldCost;
                yield return new WaitForSeconds(levels[_level].timeToUpgrade);
                cost = levels[_level].goldCost;
                damage = levels[_level].damage;
                fireRate = levels[_level].fireRate;
                range = levels[_level].range;
                areaVisuals.transform.localScale = new Vector3(range * 2, range * 2, 0);
                _triggerArea.radius = range;
                _level++;
                _canUpgrade = true;
            }
        }

        public override void OnSelected()
        {
            if(_menu != null)
                _menu.gameObject.SetActive(true);

            areaVisuals.gameObject.SetActive(true);
        }
        
        public override void OnDisselected()
        {
            if(_menu != null)
                _menu.gameObject.SetActive(false);
            
            areaVisuals.gameObject.SetActive(false);
        }
    }
}