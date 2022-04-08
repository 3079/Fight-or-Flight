using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using Enemies;
using Miscellaneous;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Mortar : Tower
{
    [SerializeField] private GameObject shellPrefab;

    private Vector3 _targetLocation;

    protected override void Attack()
    {
        Enemy enemy = GetClosestEnemy();

        if (enemy != null)
        {
            _targetLocation = GetClosestEnemy().transform.position;
            
            Shell shell = Instantiate(shellPrefab, transform.position, Quaternion.identity).GetComponent<Shell>();
            shell.SetTarget(_targetLocation);
            
            _timer = fireRate * 3 / _population;
        }
    }
}
