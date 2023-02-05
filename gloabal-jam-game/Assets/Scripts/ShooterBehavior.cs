using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{
    public LaserBehavior laserPrefab;
    public Vector2Int directionFacing;
    public GridComponent gridComponent;

    private void Awake()
    {
        gridComponent = GetComponent<GridComponent>();
    }


    void CreateLaser()
    {
        bool laser1 = GridManager.CheckItemAtPosition(gridComponent, TileType.LaserHorizontal, gridComponent.gridPosition + directionFacing);
        bool laser2 = GridManager.CheckItemAtPosition(gridComponent, TileType.LaserVertical, gridComponent.gridPosition + directionFacing);
        if(!laser1 && !laser2)
        {
            laserPrefab.direction = directionFacing;
            laserPrefab.CreateSelf();
        }
    }
}
