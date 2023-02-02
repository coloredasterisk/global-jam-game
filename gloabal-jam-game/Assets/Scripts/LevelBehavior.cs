using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdjacentType
{
    Left,
    Right,
    Up,
    Down,
}
public class LevelBehavior : MonoBehaviour
{
    public bool isPresent = true;
    public GameObject pastObject;
    public GameObject presentObject;
    public GameObject clonedParent;
    public GameObject createdParent;

    public Vector2Int dimensions;

    public GameObject originalState; //prefab of itself
    public List<string> turnInformation;
    
    public List<levelDirection> AdjacentLevels;

    [System.Serializable] public struct levelDirection
    {
        public AdjacentType type;
        public LevelBehavior level;
    }

    public void ToggleState()
    {

        if (isPresent)//switch to past
        {
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

    void ReplaceSection()
    {
        Transform[] clonedChildren = clonedParent.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in clonedChildren)
        {
            if (child.gameObject.name == "ClonedObjects") continue;
            GridComponent compo = child.gameObject.GetComponentInChildren<GridComponent>();
            if (compo != null) compo.RemoveFromGrid();
            Destroy(child.gameObject);
        }

        Transform[] createdChildren = createdParent.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in createdChildren)
        {
            if (child.gameObject.name == "CreatedObjects") continue;
            Instantiate(child.gameObject, clonedParent.transform);
        }

    }
}
