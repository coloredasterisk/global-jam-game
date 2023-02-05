using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{

    public List<TileType> verticalImpassable = new List<TileType>(){
            TileType.Player,
            TileType.Block,
            TileType.Wall,
            TileType.Mirror135,
            TileType.Mirror45,
            TileType.Shooter,
            TileType.Pillar,
            TileType.LaserVertical,
        };
    public List<TileType> horizontalImpassable = new List<TileType>(){
            TileType.Player,
            TileType.Block,
            TileType.Wall,
            TileType.Mirror135,
            TileType.Mirror45,
            TileType.Shooter,
            TileType.Pillar,
            TileType.LaserHorizontal,
        };


    public GridComponent laserHorizontalPrefab;
    public GridComponent laserVerticalPrefab;
    public Vector2Int directionFacing;
    private Vector2Int directionToMake;
    private Vector2Int locationToMake;
    public GridComponent gridComponent;
    public List<GridComponent> chain;

    [SerializeField] private int num;


    private void Awake()
    {
        gridComponent = GetComponent<GridComponent>();
    }


    void Start() {
        directionToMake = directionFacing;
        locationToMake = gridComponent.gridPosition;
        //CreateLaser(directionFacing);
    }
    


    public void CreateLaser()
    {
        if(directionFacing.x != 0)
        {
            while (CheckImpassible(horizontalImpassable, laserHorizontalPrefab, directionToMake))
            {

            }

        } else if(directionFacing.y != 0)
        {
            while (CheckImpassible(verticalImpassable, laserVerticalPrefab, directionToMake))
            {

            }
        }
        directionToMake = directionFacing;
        locationToMake = gridComponent.gridPosition;

    }

    public void CutLaser()
    {
        while(chain.Count > 0)
        {
            GridComponent laser = chain[0];
            chain.Remove(laser);
            laser.RemoveFromGrid();
            Destroy(laser.gameObject);
        }
        directionToMake = directionFacing;
        locationToMake = gridComponent.gridPosition;

    }

    public bool CheckImpassible(List<TileType> types, GridComponent laserPrefab, Vector2Int direction)
    {
        GridComponent test = GridManager.CheckItemAtPosition(gridComponent, types, locationToMake + direction);
        if (test == null)
        {
            laserPrefab.transform.position = 
                GridManager.convertToVector3(locationToMake + direction);
            chain.Add(Instantiate(laserPrefab, transform.parent));
            locationToMake = locationToMake + direction;
            return true;
        }
        else if (test.tileType == TileType.Mirror135 || test.tileType == TileType.Mirror45)
        {
            directionToMake = test.GetComponent<MirrorProperties>().reflectLaser(test.tileType, chain[chain.Count-1]);
            locationToMake = locationToMake + direction; 
            return true;
        }
        else
        {
            return false;
        }
    }
}
