using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateBehavior : MonoBehaviour
{
    private AudioSource clicked;
    public List<GridComponent> turnObjectsOn;
    public List<GridComponent> turnObjectsOff;

    public Sprite onSprite;
    public Sprite offSprite;

    public bool isOn = false;

    private void Start()
    {
        if (!isOn)
        {
            TurnListOff(turnObjectsOn);
            TurnListOn(turnObjectsOff);
            GetComponent<SpriteRenderer>().sprite = offSprite;
        }
        else
        {
            TurnListOff(turnObjectsOff);
            TurnListOn(turnObjectsOn);
            GetComponent<SpriteRenderer>().sprite = onSprite;
        }
        clicked = GetComponent<AudioSource>();
        
    }

    public void DynamicReset()
    {
        isOn = false;
        TurnListOff(turnObjectsOn);
        TurnListOn(turnObjectsOff);
        GetComponent<SpriteRenderer>().sprite = offSprite;

    }

    public void SwitchOn()
    {

        isOn = true;
        TurnListOff(turnObjectsOff);
        TurnListOn(turnObjectsOn);
        GetComponent<SpriteRenderer>().sprite = onSprite;
        //clicked.Play();


    }
    public void SwitchOff()
    {
        isOn = false;
        TurnListOff(turnObjectsOn);
        TurnListOn(turnObjectsOff);
        if( clicked != null)
        {
            clicked.Play();
        }
        GetComponent<SpriteRenderer>().sprite = offSprite;




    }

    private void TurnListOff(List<GridComponent> list)
    {
        if (list != null && list.Count > 0)
        {
            foreach (GridComponent component in list)
            {
                if(component.GetComponent<ShooterBehavior>() != null)
                {
                    component.GetComponent<ShooterBehavior>().ToggleLaser(false);
                }
                else
                {
                    //Debug.Log("About to remove :" + component.name);
                    component.RemoveFromGrid();
                    component.gameObject.SetActive(false);
                }
                
            }
        }
    }
    private void TurnListOn(List<GridComponent> list)
    {
        if (list != null && list.Count > 0)
        {
            foreach (GridComponent component in list)
            {
                if (component.GetComponent<ShooterBehavior>() != null)
                {
                    component.GetComponent<ShooterBehavior>().ToggleLaser(true);
                }
                else
                {
                    bool placed = component.AddToGrid(false, false, false);
                    if (placed)
                    {
                        component.gameObject.SetActive(true);
                    }
                }
                
            }
        }
    }

}
