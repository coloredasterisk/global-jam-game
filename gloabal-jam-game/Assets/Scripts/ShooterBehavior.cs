using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{
    public LaserBehavior laserHorizontalPrefab;
    public LaserBehavior laserVerticalPrefab;
    public Vector2Int directionFacing;
    public GridComponent gridComponent;
    public LaserBehavior firstChain;


    private void Awake()
    {
        gridComponent = GetComponent<GridComponent>();
    }

    public enum Direction {
        up,
        down,
        left,
        right
    }

    void Start() {
        CreateLaser(directionFacing);
    }



    public void CreateLaser()
    {
        bool laser1 = GridManager.CheckItemAtPosition(gridComponent, TileType.LaserHorizontal, gridComponent.gridPosition + directionFacing);
        bool laser2 = GridManager.CheckItemAtPosition(gridComponent, TileType.LaserVertical, gridComponent.gridPosition + directionFacing);
        //takes in list of unpassable to not spawn lazer
        
        if(!laser1 && !laser2)
        {
            if(directionFacing.x != 0)
            {
                laserHorizontalPrefab.transform.position = GridManager.convertToVector3(gridComponent.gridPosition + directionFacing);
                laserHorizontalPrefab.direction = directionFacing;
                firstChain = Instantiate(laserHorizontalPrefab, transform.parent);

            } else if(directionFacing.y != 0)
            {
                laserVerticalPrefab.direction = directionFacing;
                laserVerticalPrefab.transform.position = GridManager.convertToVector3(gridComponent.gridPosition + directionFacing);
                firstChain = Instantiate(laserVerticalPrefab, transform.parent);
            }
            
        }
    }
    public void CreateLaser(Vector2Int dir)
    {

        if(dir.x != 0)
        {
            GridComponent test = GridManager.CheckItemAtPosition(gridComponent, laserHorizontalPrefab.impassable, gridComponent.gridPosition + dir);
            if (test == null) {
                laserHorizontalPrefab.transform.position = GridManager.convertToVector3(gridComponent.gridPosition + dir);
                laserHorizontalPrefab.direction = dir;
                firstChain = Instantiate(laserHorizontalPrefab, transform.parent);
            }
            
        }

        else if(dir.y != 0)

        {
            GridComponent test = GridManager.CheckItemAtPosition(gridComponent, laserVerticalPrefab.impassable, gridComponent.gridPosition + dir);
            if (test == null) {
                laserVerticalPrefab.transform.position = GridManager.convertToVector3(gridComponent.gridPosition + dir);
                laserVerticalPrefab.direction = dir;
                firstChain = Instantiate(laserVerticalPrefab, transform.parent);
            }

        }
    }

    public void CutLaser()
    {
        firstChain.DestroyChain();
    }
}
