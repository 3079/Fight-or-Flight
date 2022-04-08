using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineGizmo : MonoBehaviour
{
    [SerializeField] private int lineIndex;
    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    private void Awake()
    {
        waypoints = GetComponentsInChildren<Transform>().ToList();
        waypoints.RemoveAt(0);
    }

    private void OnDrawGizmos()
    {
        switch (lineIndex)
        {
            case 0:
                Gizmos.color = Color.red;
                break;
            case 1:
                Gizmos.color = Color.green;
                break;
            case 2:
                Gizmos.color = Color.yellow;
                break;
            default:
                Gizmos.color = Color.blue;
                break;
        }
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i+1].position);
        }
    }
}
