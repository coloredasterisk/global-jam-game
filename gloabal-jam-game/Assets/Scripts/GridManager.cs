using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class GridManager : MonoBehaviour
{
    //Stores all objects with a grid system, items that would overlap are stored in a list
    public static Vector2Int left,up,down,right;
    public static Dictionary<Vector2Int, List<GridComponent>> pastGridPosition = new Dictionary<Vector2Int, List<GridComponent>>();
    public static Dictionary<Vector2Int, List<GridComponent>> presentGridPosition = new Dictionary<Vector2Int, List<GridComponent>>();

    public static Dictionary<StateType, Dictionary<Vector2Int, List<GridComponent>>> gridStates = new Dictionary<StateType, Dictionary<Vector2Int, List<GridComponent>>>() 
    {
        { StateType.Past, pastGridPosition },
        { StateType.Present, presentGridPosition },
    };

    //store solid object positions
    public static List<TileType> solidObjects = new List<TileType>()
    {
        TileType.Block,
        TileType.Player,
        TileType.Wall,
        TileType.Shooter,
        TileType.Mirror45,
        TileType.Mirror135,
        TileType.Pillar,
    };

//overloaded function
    public static bool InsertSelf(GridComponent gridComp, bool updateWorld)
    {
        return InsertSelf(gridComp, gridComp.gridPosition, updateWorld);
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
    public static bool InsertSelf(GridComponent gridComp, Vector2Int position, bool updateWorld)
    {
        List<GridComponent> pointPosition = retrieveCell(gridComp, position);

        if (pointPosition != null)
        {
            bool collide = GoingToCollide(gridComp.tileType, GetState(gridComp), position);
            if (collide)
            {
                return false;
            }
            bool solid = solidObjects.Contains(gridComp.tileType);
            if (solid)
            {
                GridComponent pressurePlate = CheckItemAtPosition(gridComp, TileType.PressurePlate, position);
                if (pressurePlate != null)
                {
                    pressurePlate.GetComponent<PressurePlateBehavior>().SwitchOn();
                }

            }
            
            pointPosition.Add(gridComp);

            if (solid)
            {
                GridComponent laserH = CheckItemAtPosition(gridComp, TileType.LaserHorizontal, position);
                GridComponent laserV = CheckItemAtPosition(gridComp, TileType.LaserVertical, position);
                if (laserH != null)
                {
                    LaserBehavior laser = laserH.GetComponent<LaserBehavior>();
                    laser.parent.CutLaser(laser.index);
                }
                if (laserV != null)
                {
                    LaserBehavior laser = laserV.GetComponent<LaserBehavior>();
                    laser.parent.CutLaser(laser.index);
                }
            }

            if (solid)
            {
                bool coin = CheckItemAtPosition(gridComp, TileType.EndGame, position);
                if (coin)
                {
                    FindObjectOfType<GameManager>(true).EndGame();
                }
            }

            if (updateWorld)
            {
                //update all shooters
                foreach (ShooterBehavior shooter in FindObjectsOfType<ShooterBehavior>())
                {
                    shooter.UpdateLaser();
                }
            }
            if(gridComp.tileType == TileType.Wall)
            {
                //Debug.Log("WAll added" + gridComp.name);
            }

            
                
        }
        else
        {//make new list if the list does not exist

            List<GridComponent> gridList = new List<GridComponent>() { gridComp };
            GetGridState(gridComp).Add(position, gridList);

        }
        //Debug.Log(retrieveCell(gridComp, position));
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
            //Debug.Log("WE are going to remove this:" + gridComp.name + "List size:" + pointPosition.Count);
            for(int index = 0; index < pointPosition.Count; index++)
            {
                GridComponent gc = pointPosition[index];
                if (gc == gridComp)
                {
                    index--;
                    pointPosition.Remove(gridComp);
                }
            }
           // Debug.Log("AFter removal List size:" + pointPosition.Count);
            if (pointPosition.Count <= 0)
            {//delete empty list
                GetGridState(gridComp).Remove(gridComp.gridPosition);
            }

            //check pressure plate
            GridComponent pressurePlate = CheckItemAtPosition(gridComp.GetComponentInParent<ParentState>(true).stateType, TileType.PressurePlate, gridComp.gridPosition);
            if (pressurePlate != null)
            {
                pressurePlate.GetComponent<PressurePlateBehavior>().SwitchOff();
            }
            //Debug.Log("AFter werid stuff List size:" + pointPosition.Count);


            if (pointPosition.Count > 0)
            {
                Debug.Log(pointPosition[0] + gridComp.name);
            }


            return true;
        }
        //Debug.Log("GridCOmp :"+ gridComp.name + "Position: " + gridComp.gridPosition);
        Debug.Log("This GridComponent cannot be found and thus cannot be removed");
        return false;
    }
    public static GridComponent CheckItemAtPosition(StateType location, TileType type, Vector2Int position)
    {
        List<GridComponent> pointPosition = null;
        bool positionExists = gridStates[location].TryGetValue(position, out pointPosition);
        //Debug.Log("Position exists: " + positionExists);
        if (positionExists)
        {
            foreach (GridComponent grid in pointPosition)
            {
                if (grid.tileType == type)
                {
                    //Debug.Log("Item equals type: " + type);
                    return grid;
                }
            }
        }
    
        return null;
    }
    public static GridComponent CheckItemAtPosition(GridComponent location, List<TileType> types, Vector2Int position)
    {
        foreach(TileType type in types)
        {
            GridComponent gc = CheckItemAtPosition(location, type, position);
            if (gc != null)
            {
                return gc;
            }
        }
        return null;
    }
    public static GridComponent CheckItemAtPosition(GridComponent location, TileType type, Vector2Int position)
    {
        return CheckItemAtPosition(location.GetComponentInParent<ParentState>(true).stateType, type, position);
    }
    public static GridComponent CheckItemAtPosition(GridComponent location, TileType type, Vector3 position)
    {
        Vector2Int newVector = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        return CheckItemAtPosition(location.GetComponentInParent<ParentState>(true).stateType, type, newVector);
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
    public static List<GridComponent> retrieveCell(GridComponent location, Vector2Int position)
    {
        return retrieveCell(GetState(location), position);
    }
    /// <summary>
    /// This function returns a list of all gridcomponents within a cell within a cell
    /// </summary>
    /// <param name="location"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static List<GridComponent> retrieveCell(StateType location, Vector2Int position)
    {
        Dictionary<Vector2Int, List<GridComponent>> gridPosition = gridStates[location];

        List<GridComponent> pointPosition = null;
        gridPosition.TryGetValue(position, out pointPosition);

        return pointPosition;
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
                if(grid.tileType == typeToCheck)
                {
                    return false;
                }
                return true;
            }
        }
        if(typeToCheck == TileType.Player)
        {
            foreach (GridComponent grid in gridPosition)
            {
                //dont move if yes
                if (grid.tileType == TileType.PlayerForcefield)
                {
                    return true;
                }
            }
        } else if(typeToCheck == TileType.Block)
        {
            foreach (GridComponent grid in gridPosition)
            {
                //dont move if yes
                if (grid.tileType == TileType.PushableForcefield)
                {
                    return true;
                }
            }
        }
        return false;
    }

    

    //Move the object provided to a new given position
    public static bool MoveSelf(GridComponent gridComp, Vector2Int moveTo)
    {
        if (InsertSelf(gridComp, moveTo, true))
        {
            if (RemoveSelf(gridComp))
            {
                //update all shooters
                foreach (ShooterBehavior shooter in FindObjectsOfType<ShooterBehavior>())
                {
                    shooter.UpdateLaser();
                }
                return true;
            }
        }
        
        return false;
    }

    public static Vector2Int convertToVector2(Vector3 vector)
    {
        return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }
    public static Vector3 convertToVector3(Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}
