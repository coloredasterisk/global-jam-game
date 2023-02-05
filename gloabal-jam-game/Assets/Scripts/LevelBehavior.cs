using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public enum AdjacentType
{
    Zero,
    Left,
    Right,
    Up,
    Down,
}

public class LevelBehavior : MonoBehaviour
{
    private static Dictionary<Vector2Int, AdjacentType> directionalMap = new Dictionary<Vector2Int, AdjacentType>()
    {
        {Vector2Int.zero, AdjacentType.Zero},
        {Vector2Int.left, AdjacentType.Left },
        {Vector2Int.right, AdjacentType.Right },
        {Vector2Int.up, AdjacentType.Up },
        {Vector2Int.down, AdjacentType.Down },
    };
    public bool isPresent = true;
    public GameObject pastObject;
    public GameObject presentObject;
    public GameObject dynamicPast;
    public GameObject dynamicPresent;
    public GameObject clonedParent;
    public GameObject createdParent;

    public Vector2Int dimensions;
    public float cameraRadius;

    public GridComponent presentSpawnPoint;
    public GridComponent pastSpawnPoint;
    
    public List<levelAdajcent> AdjacentLevels;
    public List<LevelBehavior> VisibleLevels;

    [System.Serializable] public struct levelAdajcent
    {
        public AdjacentType type;
        public LevelBehavior level;
        
    }

    public void ToggleVisibleStates()
    {
        ToggleState();
        foreach (LevelBehavior level in VisibleLevels)
        {
            SetStates(level, isPresent);
        }
    }

    public void ToggleState()
    {

        if (isPresent)//switch to past
        {
            InteractionLog.NewSwitchedLog(isPresent, this, null, null);

            isPresent = false;
            pastObject.SetActive(true);
            presentObject.SetActive(false);

        }
        else//switch to present
        {
            //clone all created objects into the present and remove/reset all created items in the present
            ReplaceSection();

            isPresent = true;
            pastObject.SetActive(false);
            presentObject.SetActive(true);

        }
    }
    public static void SetStates(LevelBehavior levelToSwitch, bool present)
    {
        if (present)//switch to present
        {
            levelToSwitch.isPresent = true;
            levelToSwitch.pastObject.SetActive(false);
            levelToSwitch.presentObject.SetActive(true);

        }
        else
        {
            levelToSwitch.isPresent = false;
            levelToSwitch.pastObject.SetActive(true);
            levelToSwitch.presentObject.SetActive(false);

        }
    }

    void ReplaceSection()
    {
        
        Transform[] clonedChildren = clonedParent.transform.GetComponentsInChildren<Transform>();
        //prepare a copy of data values in order to recreate destroyed objects
        List<InteractionLog.ReconstructObject> reconstructObjects = new List<InteractionLog.ReconstructObject>();
        List<GameObject> storedCloneObjects = new List<GameObject>();

        //Destroy all objects in clonedObjects
        foreach (Transform child in clonedChildren)
        {
            if (child.gameObject.name.Contains("ClonedObjects")) continue;
            GridComponent compo = child.gameObject.GetComponentInChildren<GridComponent>();
            if (compo != null) compo.RemoveFromGrid();

            reconstructObjects.Add(InteractionLog.PrepareReconstruction(compo, clonedParent.transform));

            FindObjectOfType<GameManager>().SendToGraveyard(child.gameObject);
            //Destroy(child.gameObject);
        }
        

        //Clone all objects in createdObjects and push them into clonedObjects
        Transform[] createdChildren = createdParent.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in createdChildren)
        {
            if (child.gameObject.name.Contains("CreatedObjects")) continue;
            GameObject childClone = Instantiate(child.gameObject, clonedParent.transform);

            storedCloneObjects.Add(childClone);
        }
        InteractionLog.NewSwitchedLog(isPresent, this, storedCloneObjects, reconstructObjects);


        //Debug.Log("Stored isPresent:" + isPresent + " \nlist of:" + storedCloneObjects + " \nlist of:" + reconstructObjects);

    }

    public Vector2Int getVerticalBounds()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.y) - dimensions.y / 2, Mathf.RoundToInt(transform.position.y) + dimensions.y / 2);
    }
    public Vector2Int getHorizontalBounds()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x) - dimensions.x / 2, Mathf.RoundToInt(transform.position.x) + dimensions.x / 2);
    }
    /// <summary>
    /// Check if a component is within the level bounds, returns 0,0 if inside
    /// and the vector direction where the component left 
    /// (1,0) right | (-1,0) left | (0,1) up | (0,-1) down
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    public Vector2Int checkIfInBounds(GridComponent component)
    {
        Vector2Int verticalBounds = getVerticalBounds();
        Vector2Int horiztonalBounds = getHorizontalBounds();
        Vector2Int position = component.gridPosition;
        Vector2Int result = new Vector2Int(0, 0);

        if(position.y > verticalBounds.y)
        {
            result.y += 1;
        } else if(position.y < verticalBounds.x)
        {
            result.y -= 1;
        }
        if(position.x > horiztonalBounds.y)
        {
            result.x += 1;
        } else if(position.x < horiztonalBounds.x)
        {
            result.x -= 1;
        }
        return result;
    }

    public void checkBoundsBehavior(GridComponent component)
    {
        Vector2Int vector = checkIfInBounds(component);
        AdjacentType direction = directionalMap[vector];

        if (component.GetComponent<PlayerController>() && direction != AdjacentType.Zero)
        {
            foreach(levelAdajcent level in AdjacentLevels)
            {
                if (level.type == direction)
                {
                    GameManager manager = FindObjectOfType<GameManager>();
                    manager.ChangeLevel(level.level);
                    break;
                }
            }
        } 
    }
    public void EraseChildrenFromGrid()
    {
        EraseSpecificChildrenFromGrid(clonedParent);
        EraseSpecificChildrenFromGrid(createdParent);
        EraseSpecificChildrenFromGrid(dynamicPresent);
        EraseSpecificChildrenFromGrid(dynamicPast);
    }
    private void EraseSpecificChildrenFromGrid(GameObject parent)
    {
        for(int i = 0; i < parent.transform.childCount; i++) 
        {
            Debug.Log(parent.transform.GetChild(i).name);
            parent.transform.GetChild(i).GetComponent<GridComponent>().RemoveFromGrid();
        }
    }
}
