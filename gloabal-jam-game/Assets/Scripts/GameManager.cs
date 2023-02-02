using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject presentObject;
    public GameObject pastObject;
    public GameObject createdParent;
    public GameObject clonedParent;
    public bool isPresent = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleState();
        }
    }

    void ToggleState()
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
            if(compo != null) compo.RemoveFromGrid();
            Destroy(child.gameObject);
        }

        Transform[] createdChildren = createdParent.transform.GetComponentsInChildren<Transform>();
        foreach(Transform child in createdChildren)
        {
            if (child.gameObject.name == "CreatedObjects") continue;
            Instantiate(child.gameObject, clonedParent.transform);
        }

    }



}
