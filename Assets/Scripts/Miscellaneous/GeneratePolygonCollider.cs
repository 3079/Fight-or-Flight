using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePolygonCollider : MonoBehaviour
{
    private List<Vector2> _points = new List<Vector2>();
    private List<Vector2> _simplifiedPoints = new List<Vector2>();
    private PolygonCollider2D _polygonCollider2D;
    private Sprite _sprite;

    private void Awake()
    {
        _polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        _sprite = GetComponent<Sprite>();   
    }

    public void UpdatePolygonCollider2D(float tolerance = 0.1f)
    {
        _polygonCollider2D.pathCount = _sprite.GetPhysicsShapeCount();
        for(int i = 0; i < _polygonCollider2D.pathCount; i++)
        {
            _sprite.GetPhysicsShape(i, _points);
            LineUtility.Simplify(_points, tolerance, _simplifiedPoints);
            _polygonCollider2D.SetPath(i, _simplifiedPoints);
        }
    }
}
