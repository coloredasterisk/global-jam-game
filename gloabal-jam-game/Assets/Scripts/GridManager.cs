using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class GridManager : MonoBehaviour
{
    //Stores all objects with a grid system, items that would overlap are stored in a list
    public static Dictionary<Vector2Int, List<GridComponent>> pastGridPosition = new Dictionary<Vector2Int, List<GridComponent>>();
    public static Dictionary<Vector2Int, List<GridComponent>> presentGridPosition = new Dictionary<Vector2Int, List<GridComponent>>();

    public static Dictionary<StateType, Dictionary<Vector2Int, List<GridComponent>>> gridStates = new Dictionary<StateType, Dictionary<Vector2Int, List<GridComponent>>>() 
    {
        { StateType.Past, pastGridPosition },
        { StateType.Present, presentGridPosition },
    };

    //store solid object positions
    private static List<TileType> solidObjects = new List<TileType>()
    {
        TileType.Block,
        TileType.Player,
        TileType.Wall,
    };
    //overloaded function
    public static bool InsertSelf(GridComponent gridComp)
    {
        return InsertSelf(gridComp, gridComp.gridPosition);
    }
    /// <summary>
    /// This function adds a gridComp to a dictionary based on position.
    /// Will not add if two solid objects are in the same spot.
    /// Same spot checks occur with gridComp location and the persistent location
    /// Returns true if successfully adds gridComp
    /// </summary>
    /// <param name="gridComp">The GridComponent to be inserted</param>
    /// <param name="position">The position to be stored in the dictionary</param>
    /// <returns></returns>
    public static bool InsertSelf(GridComponent gridComp, Vector2Int position)
    {
        List<GridComponent> pointPosition = retrieveCell(gridComp, position);

        if (pointPosition != null)
        {
            bool collide = GoingToCollide(gridComp.tileType, GetState(gridComp), position);
            if (collide)
            {
                return false;
            }

            pointPosition.Add(gridComp);
        }
        else
        {//make new list if the list does not exist
            GetGridState(gridComp).Add(position, new List<GridComponent>() { gridComp });
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
        List<GridComponent> pointPosition = retrieveCell(gridComp, gridComp.gridPosition);
        if (pointPosition != null)
        {
            pointPosition.Remove(gridComp);
            if(pointPosition.Count <= 0)
            {//delete empty list
                GetGridState(gridComp).Remove(gridComp.gridPosition);
            }
            return true;
        }
        Debug.Log("This GridComponent cannot be found and thus cannot be removed");
        return false;
    }
    public static GridComponent CheckItemAtPosition(StateType location, TileType type, Vector2Int position)
    {
        List<GridComponent> pointPosition = null;
        bool positionExists = gridStates[location].TryGetValue(position, out pointPosition);
        if (positionExists)
        {
            foreach (GridComponent grid in pointPosition)
            {
                if (grid.tileType == type)
                {
                    return grid;
                }
            }
        }
    
        return null;
    }
    /// <summary>
    /// This function takes a gridComponent 'location' and and returns the stateType found in the parent 
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    private static StateType GetState(GridComponent location)
    {
        return location.GetComponentInParent<ParentState>(true).stateType;
    }
    /// <summary>
    /// This function returns a dictionary of the known positions of gridcomponents
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    private static Dictionary<Vector2Int, List<GridComponent>> GetGridState(GridComponent location)
    {
        return gridStates[GetState(location)];
    }
    //overloaded function
    private static List<GridComponent> retrieveCell(GridComponent location, Vector2Int position)
    {
        return retrieveCell(GetState(location), position);
    }
    /// <summary>
    /// This function returns a list of all gridcomponents within a cell within a cell
    /// </summary>
    /// <param name="location"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static List<GridComponent> retrieveCell(StateType location, Vector2Int position)
    {
        Dictionary<Vector2Int, List<GridComponent>> gridPosition = gridStates[location];

        List<GridComponent> pointPosition = null;
        gridPosition.TryGetValue(position, out pointPosition);

        return pointPosition;
    }
    /// <summary>
    /// Returns the gridComponents found in the first location and second location provided, they are combined into a single list
    /// </summary>
    /// <param name="firstLocation"></param>
    /// <param name="secondLocation"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static List<GridComponent> retrieveMultipleCells(StateType firstLocation, StateType secondLocation, Vector2Int position) 
    {
        List<GridComponent> firstGrid = null;
        gridStates[firstLocation].TryGetValue(position, out firstGrid);

        List<GridComponent> secondGrid = null;
        gridStates[secondLocation].TryGetValue(position, out secondGrid);

        if(firstGrid != null && secondGrid != null)
        {
            return null;
        }
        else
        {
            List<GridComponent> mixture = new List<GridComponent>();
            if(firstGrid != null)
            {
                mixture.Union(firstGrid);
            }
            if(secondGrid != null)
            {
                mixture.Union(secondGrid);
            }
            return mixture;
        }
    }
    /// <summary>
    /// Return false if the item at the position is solid and the typeToCheck is solid
    /// </summary>
    /// <param name="typeToCheck"></param>
    /// <param name="location"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static bool GoingToCollide(TileType typeToCheck, StateType location, Vector2Int position)
    {
        bool solidInsert = solidObjects.Contains(typeToCheck);
        List<GridComponent> gridPosition = retrieveCell(location, position);

        if (gridPosition == null) return false;

        foreach (GridComponent grid in gridPosition)
        {
            //dont add if two objects collide
            if (solidInsert && solidObjects.Contains(grid.tileType))
            {
                //Debug.Log("Colliding with: " + grid.tileType + " since the original object is: " + typeToCheck);
                return true;
            }
        }
        return false;
    }

    //Move the object provided to a new given position
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