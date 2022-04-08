using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTowerBuildingEffect : BuildingEffect
{
    private void Awake()
    {
        _sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        _sprite.sortingOrder = 1;
        _sprite.color = Color.green;
    }
}
