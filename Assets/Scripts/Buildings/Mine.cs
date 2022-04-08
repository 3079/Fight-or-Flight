using System;
using System.Collections;
using Core;
using UnityEngine;

namespace Buildings
{
    public class Mine : Building
    {
        [SerializeField] private int goldPerTick;
        
        public override IEnumerator LvlUP()
        {
            if (_level < _maxLevel && GameManager.Instance.Science >= levels[_level].scienceCost &&
                GameManager.Instance.Gold >= levels[_level].goldCost)
            {
                _canUpgrade = false;
                GameManager.Instance.Gold -= levels[_level].goldCost;
                yield return new WaitForSeconds(levels[_level].timeToUpgrade);
                goldPerTick = levels[_level].gmp;
                cost = levels[_level].goldCost;
                _level++;
                _canUpgrade = true;
            }
        }

        public int GetGold()
        {
            if (_population > 0)
                return goldPerTick * _population / 3;
            
            return 0;
        }
    }
}