using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildingEffect : BuildingEffect
{
    private SpriteRenderer _area;
    private void Awake()
    {
        _sprite = gameObject.GetComponentsInChildren<SpriteRenderer>()[0];
        _area = gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
        _sprite.sortingOrder = 1;
        _area.sortingOrder = -1;
        _sprite.color = Color.green;
    }
}
