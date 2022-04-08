using System.Collections;
using System.Collections.Generic;
using Buildings;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldValue;
        [SerializeField] private TextMeshProUGUI scienceValue;
        [SerializeField] private TextMeshProUGUI workersValue;
        [SerializeField] private int totalPopulation;
        [SerializeField] private float tick;

        public int Gold;
        public int Science;
        public int Workers;
        
        [SerializeField] private List<Building> _buildings = new List<Building>();
        private float _timer;
        private int _initialPopulation;
        private int _evacuated;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            
            Workers = totalPopulation;
            _initialPopulation = totalPopulation;
            _timer = tick;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0)
                Tick();
        }
        

        private void Tick()
        {
            _timer = tick;
            
            GainGold();
            GainScience();
            UpdateStats();
        }

        private void GainGold()
        {
            foreach (var building in _buildings)
            {
                var mine = building.GetComponent<Mine>();
                if(mine != null)
                    Gold += mine.GetGold();
            }
        }

        private void GainScience()
        {
            Science += _evacuated;
        }

        private void UpdateStats()
        {
            goldValue.text = Gold.ToString();
            scienceValue.text = Science.ToString();
            workersValue.text = Workers.ToString();
        }

        public void Evacuate(int amount)
        {
            _evacuated += amount;
            totalPopulation -= amount;
        }

        public void TakeDamage(int amount)
        {
            totalPopulation = math.clamp(totalPopulation - amount, 0, _initialPopulation);

            if (Workers >= amount)
            {
                Workers -= amount;
            }
            else
            {
                amount -= Workers;
                Workers = 0;

                for (int i = 0; i < _buildings.Count; i++)
                {
                    var rocket = _buildings[i].GetComponent<Rocket>();
                    if (rocket != null)
                    {
                        int t = rocket.GetPopulation();
                        for (int j = 0; j < t; j++)
                        {
                            rocket.Depopulate();
                            totalPopulation--;
                            Workers--;
                            amount--;
                            if (amount <= 0)
                                break;
                        }
                        if (amount <= 0)
                            break;
                    }
                }
                
                for (int i = 0; i < _buildings.Count; i++)
                {
                    var mine = _buildings[i].GetComponent<Mine>();
                    if (mine != null)
                    {
                        int t = mine.GetPopulation();
                        for (int j = 0; j < t; j++)
                        {
                            mine.Depopulate();
                            totalPopulation--;
                            Workers--;
                            amount--;
                            if (amount <= 0)
                                break;
                        }
                        if (amount <= 0)
                            break;
                    }
                }

                for (int i = 0; i < _buildings.Count; i++)
                {
                    var tower = _buildings[i].GetComponent<Tower>();
                    
                    // if(tower == null)
                    //     tower = _buildings[i].GetComponent<Mortar>();
                    
                    if (tower != null)
                    {
                        int t = tower.GetPopulation();
                        for (int j = 0; j < t; j++)
                        {
                            tower.Depopulate();
                            Workers--;
                            amount--;
                            if (amount <= 0)
                                break;
                        }
                        if (amount <= 0)
                            break;
                    }
                }
            }

            if(totalPopulation <= 0)
                GameOver();
        }

        public void GameOver()
        {
            Debug.Log("GAME OVER");
        }

        public IEnumerator AddBuilding(Building building, Vector3 mousePos)
        {
            if (building == null)
                yield break;
            
            Gold -= building.GetCost();
            // Workers--;
            yield return new WaitForSeconds(building.GetBuildingTime());

            _buildings.Add(Instantiate(building, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity));

            // Workers++;
        }

        public void RemoveBuilding(Building building)
        {
            _buildings.Remove(building);
        }
    }
}