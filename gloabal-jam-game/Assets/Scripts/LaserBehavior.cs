using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    public GridComponent laserPrefab;
    public Vector2Int direction;
    public GameObject chainAhead = null;

    public void CreateSelf()
    {
        laserPrefab.gridPosition = GetComponent<GridComponent>().gridPosition + direction;
        chainAhead = Instantiate(laserPrefab.gameObject, transform.parent);
    }
}
