using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject prefabActive;
    public Sprite uiDisplay;
    
    public void Collect()
    {
        FindObjectOfType<GameManager>().pastPlayer.GetComponentInChildren<PlaceBlockController>().AddPlaceableItem(this);
    }
}
