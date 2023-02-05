using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour


{
    public GridComponent laserPrefab;
    public Vector2Int direction;
    public GameObject chainAhead = null;

    private void Start()
    {
        CreateSelf();
    }

    public void CreateSelf()
    {
        laserPrefab.gridPosition = GetComponent<GridComponent>().gridPosition + direction;
        chainAhead = Instantiate(laserPrefab.gameObject, transform.parent);
    }

    public void DestroyChain()
    {
        chainAhead.GetComponent<LaserBehavior>().DestroyChain();
        if (chainAhead == null)
        {
            Destroy(gameObject);
            return;
        }
        
        
    }

    public static bool LaserCollision(TileType typeToCheck, StateType location, Vector2Int position)
    {
        bool solidInsert = GridManager.solidObjects.Contains(typeToCheck);
        List<GridComponent> gridPosition = GridManager.retrieveCell(location, position);

        if (gridPosition == null) return false;

        foreach (GridComponent grid in gridPosition)
        {
            //dont add if two objects collide
            if (solidInsert && GridManager.solidObjects.Contains(grid.tileType))
            {
                if (grid.tileType == TileType.Mirror135 || grid.tileType == TileType.Mirror45)
                {
                    //call function
                }
                return true;
            }
        }
        return false;
    }

}
