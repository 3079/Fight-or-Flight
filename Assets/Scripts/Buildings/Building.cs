using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    [System.Serializable]
    public struct Level
    {
        public int goldCost;
        public int scienceCost;
        public int damage;
        public float fireRate;
        public float range;
        public int gmp;
        public float timeToUpgrade;
    }

    public abstract class Building : MonoBehaviour
    {
        [SerializeField] protected int cost;
        [SerializeField] private float buildingTime;
        [SerializeField] private int capacity;
        [SerializeField] private Image populationIcon;
        [SerializeField] private Sprite population1;
        [SerializeField] private Sprite population2;
        [SerializeField] private Sprite population3;
        
        [SerializeField] protected Canvas _menu;

        [SerializeField] protected List<Level> levels = new List<Level>();
            
        protected bool _canUpgrade = true;
        [SerializeField] protected int _population;
        protected int _level;
        protected int _maxLevel;

        protected virtual void Awake()
        {
            if(_menu != null)
                _menu.gameObject.SetActive(false);

            _maxLevel = levels.Count;
            UpdatePopulationIcon();
        }

        public int GetCost()
        {
            return cost;
        }
        
        public float GetBuildingTime()
        {
            return buildingTime;
        }
        
        public virtual int GetPopulation()
        {
            return _population;
        }

        public virtual void Delete()
        {
            GameManager.Instance.RemoveBuilding(this);
            Destroy(gameObject);
        }
        
        public virtual void Sell()
        {
            GameManager.Instance.Gold += cost / 2;
            GameManager.Instance.Workers += _population;
            // AudioManager.Instance.PlaySFX("coins");
            Delete();
        }

        public void Upgrade()
        {
            if(_canUpgrade) 
            {
                StartCoroutine(LvlUP());
                // AudioManager.Instance.PlaySFX("upgrade");
            }
        }

        public virtual IEnumerator LvlUP()
        {
            if (_level < _maxLevel && GameManager.Instance.Science >= levels[_level].scienceCost && GameManager.Instance.Gold >= levels[_level].goldCost)
            {
                _canUpgrade = false;
                GameManager.Instance.Gold -= levels[_level].goldCost;
                yield return new WaitForSeconds(levels[_level].timeToUpgrade);
                cost = levels[_level].goldCost;
                _level++;
                _canUpgrade = true;
            }
        }

        public virtual void Populate()
        {
            if (_population < capacity && GameManager.Instance.Workers > 0)
            {
                _population++;
                AudioManager.Instance.PlaySFX("plus");
                GameManager.Instance.Workers--;
                UpdatePopulationIcon();
                AudioManager.Instance.PlaySFX("plus");
            }
        }

        private void UpdatePopulationIcon()
        {
            if (_population == 0)
            {
                populationIcon.gameObject.SetActive(false);
            }
            else
            {
                populationIcon.gameObject.SetActive(true);
                
                switch (_population)
                {
                    case 1:
                        populationIcon.sprite = population1;
                        break;
                    case 2:
                        populationIcon.sprite = population2;
                        break;
                    case 3:
                        populationIcon.sprite = population3;
                        break;
                }
            }
        }
        
        public virtual void Depopulate()
        {
            if (_population > 0)
            {
                _population--;
                AudioManager.Instance.PlaySFX("minus");
                GameManager.Instance.Workers++;
                UpdatePopulationIcon();
            }
        }

        public virtual void OnSelected()
        {
            if(_menu != null)
                _menu.gameObject.SetActive(true);
        }
        
        public virtual void OnDisselected()
        {
            if(_menu != null)
                _menu.gameObject.SetActive(false);
        }
    }
}
