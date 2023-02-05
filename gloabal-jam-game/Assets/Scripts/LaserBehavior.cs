using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserBehavior : MonoBehaviour


{

    public List<TileType> impassable = new List<TileType>(){
            TileType.Player,
            TileType.Block,
            TileType.Wall,
            TileType.Mirror135,
            TileType.Mirror45,
            TileType.Shooter,
            TileType.Pillar,
        };
    public GridComponent laserPrefab;
    public Vector2Int direction;
    public GameObject chainAhead = null;

    private void Start()
    {
        CreateSelf();
    }

    public void CreateSelf()
    {
        /* if horizontal GridManager.CheckItemAtPosition(gc, list of passable, one unit left / right of current position) != null {
            isntasntiate(laser, position left)
            do the same thing for right side

            do if statement for vertical
        }*/
        impassable.Add(laserPrefab.tileType);
        GridComponent test = GridManager.CheckItemAtPosition(laserPrefab, impassable, laserPrefab.gridPosition + direction);
        if (test == null)
        {
            laserPrefab.transform.position = GridManager.convertToVector3(laserPrefab.gridPosition + direction);
            chainAhead = Instantiate(laserPrefab.gameObject, transform.parent);
        }
        
    }

    public void DestroyChain()
    {
        
        if (chainAhead == null)
        {
            Destroy(gameObject);
            return;
        }
        chainAhead.GetComponent<LaserBehavior>().DestroyChain();


    }

    public static bool LaserCollision(List<TileType> typesToCheck, List<TileType> restricted, StateType location, Vector2Int position)
    {
        foreach(TileType t in restricted)
        {
            bool solidInsert = GridManager.solidObjects.Contains(t);
            List<GridComponent> gridPosition = GridManager.retrieveCell(location, position);

            if (gridPosition == null) return false;

            foreach (GridComponent grid in gridPosition)
            {
                //dont add if restricted is found
                if (solidInsert && GridManager.solidObjects.Contains(grid.tileType))
                {
                    if (grid.tileType == TileType.Mirror135 || grid.tileType == TileType.Mirror45)
                    {
                        //call function
                    }
                    return true;
                }
            }
        }
        
        return false;
    }

}
