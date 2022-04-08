using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

namespace Core
{
    public class WaveGenerator : MonoBehaviour
    {
        // [System.Serializable]
        // public class Line
        // {
        //     public List<Transform> waypoints;
        // }
        // [SerializeField] private List<Line> lines = new List<Line>();

        [SerializeField] private List<Transform> lines;
        
        [SerializeField] private List<GameObject> enemyTypes;
        
        private int waveNumber;
        private List<Entry> wave = new List<Entry>();
        
        public static WaveGenerator Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        }

        private void Start()
        {
            StartCoroutine(Test());
        }

        public IEnumerator Test()
        {
            SpawnEnemy(enemyTypes[0], 0);
            yield return new WaitForSeconds(1);
            SpawnEnemy(enemyTypes[0], 0);
            yield return new WaitForSeconds(1);
            SpawnEnemy(enemyTypes[0], 1);
            yield return new WaitForSeconds(1);
            SpawnEnemy(enemyTypes[0], 2);
            yield return new WaitForSeconds(1);
            SpawnEnemy(enemyTypes[0], 3);
        }

        public void GenerateWave()
        {
            
            waveNumber++;
        }

        public void SpawnEnemy(GameObject enemy, int line)
        {
            List<Transform> waypoints = lines[line].GetComponentsInChildren<Transform>().ToList();
            var e = Instantiate(enemy, waypoints[0].position, Quaternion.identity);
            e.GetComponentInChildren<SpriteRenderer>().flipX = waypoints[1].position.x - waypoints[0].position.x > 0;
            e.GetComponent<Enemy>().SetWaypoints(waypoints);
        }
    }

    public class Entry
    {
        private Enemy _enemy;
        private float _pause;

        public Entry(Enemy enemy, float pause)
        {
            _enemy = enemy;
            _pause = pause;
        }
    }
}