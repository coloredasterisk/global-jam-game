using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GridManager : MonoBehaviour
{
    //Stores all objects with a grid system, items that would overlap are stored in a list
    public static Dictionary<Vector2Int, List<GridComponent>> gridPosition = new Dictionary<Vector2Int, List<GridComponent>>();

    private static List<TileType> solidObjects = new List<TileType>()
    {
        TileType.Block,
        TileType.Player,
        TileType.Wall,
    };

    public static bool InsertSelf(GridComponent gridComp)
    {
        return InsertSelf(gridComp, gridComp.gridPosition);
    }
    /// <summary>
    /// This function adds a gridComp to a dictionary based on position.
    /// Will not add if two solid objects are in the same spot.
    /// Returns true if successfully adds gridComp
    /// </summary>
    /// <param name="gridComp">The GridComponent to be inserted</param>
    /// <param name="position">The position to be stored in the dictionary</param>
    /// <returns></returns>
    public static bool InsertSelf(GridComponent gridComp, Vector2Int position)
    {
        List<GridComponent> pointPosition = null;
        bool positionExists = gridPosition.TryGetValue(position, out pointPosition);

        if (positionExists)
        {
            bool solidInsert = solidObjects.Contains(gridComp.tileType);
            foreach(GridComponent grid in pointPosition)
            {
                //dont add if two objects collide
                if(solidInsert && solidObjects.Contains(grid.tileType) )
                {
                    return false;
                }
            }
            pointPosition.Add(gridComp);
        }
        else
        {//make new list
            gridPosition.Add(position, new List<GridComponent>() { gridComp });
        }
        return true;
    }
    /// <summary>
    /// Remove a given gridComp from it's current position
    /// </summary>
    /// <param name="gridComp"></param>
    /// <returns></returns>
    public static bool RemoveSelf(GridComponent gridComp)
    {
        List<GridComponent> pointPosition = null;
        bool positionExists = gridPosition.TryGetValue(gridComp.gridPosition, out pointPosition);
        if (positionExists)
        {
            pointPosition.Remove(gridComp);
            if(pointPosition.Count <= 0)
            {
                gridPosition.Remove(gridComp.gridPosition);
            }
            return true;
        }
        Debug.Log("This GridComponent cannot be found and thus cannot be removed");
        return false;
    }
    
    public static bool MoveSelf(GridComponent gridComp, Vector2Int moveTo)
    {
        if(InsertSelf(gridComp, moveTo))
        {
            if (RemoveSelf(gridComp))
            {
                return true;
            }
        }
        return false;
    }
}
